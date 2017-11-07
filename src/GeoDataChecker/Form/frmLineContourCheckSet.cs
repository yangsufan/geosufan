using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using ESRI.ArcGIS.Geometry ;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace GeoDataChecker
{
    public partial class frmLineContourCheckSet : DevComponents.DotNetBar.Office2007Form
    {
        Plugin.Application.IAppGISRef _AppHk;
        IPolyline m_PolyLine;
        public frmLineContourCheckSet(Plugin.Application.IAppGISRef hook,IPolyline pPolyLine)
        {
            InitializeComponent();
            if (cmbOrient.Items.Count > 0)
            {
                cmbOrient.SelectedIndex = 0;
            }
            if (hook == null) return;
            _AppHk = hook;
            if (pPolyLine == null) return;
            m_PolyLine = pPolyLine;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.OverwritePrompt = false;
            saveFile.Title = "保存为EXCEL格式";
            saveFile.Filter = "EXCEL格式(*.xls)|*.xls";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                txtLog.Text = saveFile.FileName;
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            Exception eError = null;
            //等高线方向
            string orient = cmbOrient.SelectedItem.ToString().Trim();
            //错误日志连接信息
            string logPath = txtLog.Text;
            if (logPath.Trim() == string.Empty)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择日志输出路径!");
                return;
            }
            if (File.Exists(logPath))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据文件已存在!\n" + logPath);
            }
            #region 生成日志信息EXCEL格式
            SysCommon.DataBase.SysDataBase pSysLog = new SysCommon.DataBase.SysDataBase();
            pSysLog.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + logPath + "; Extended Properties=Excel 8.0;", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "日志信息表连接失败！");
                return;
            }
            string strCreateTableSQL = @" CREATE TABLE ";
            strCreateTableSQL += @" 错误日志 ";
            strCreateTableSQL += @" ( ";
            strCreateTableSQL += @" 检查功能名 VARCHAR, ";
            strCreateTableSQL += @" 错误类型 VARCHAR, ";
            strCreateTableSQL += @" 错误描述 VARCHAR, ";
            strCreateTableSQL += @" 数据图层1 VARCHAR, ";
            strCreateTableSQL += @" 数据OID1 VARCHAR, ";
            strCreateTableSQL += @" 数据图层2 VARCHAR, ";
            strCreateTableSQL += @" 数据OID2 VARCHAR, ";
            strCreateTableSQL += @" 定位点X VARCHAR , ";
            strCreateTableSQL += @" 定位点Y VARCHAR , ";
            strCreateTableSQL += @" 检查时间 VARCHAR ,";
            strCreateTableSQL += @" 数据文件路径 VARCHAR ";
            strCreateTableSQL += @" ) ";

            pSysLog.UpdateTable(strCreateTableSQL, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "插入表头出错！");
                pSysLog.CloseDbConnection();
                return;
            }

            #endregion

            DataCheckClass dataCheckCls = new DataCheckClass(_AppHk);
            //将日志表连接信息和表名保存起来
            dataCheckCls.ErrDbCon = pSysLog.DbConn;
            dataCheckCls.ErrTableName = "错误日志";

            #region 连接GIS数据检察配置表 ,并获取相关参数
            SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();
            string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + TopologyCheckClass.GeoDataCheckParaPath;
            pSysTable.SetDbConnection(conStr, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "GIS数据检查配置表连接失败！");
                pSysLog.CloseDbConnection();
                return;
            }

            //等高线图层名，参数ID为20
            string pFeaClsName = GetParaValue(pSysTable, 20, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                pSysLog.CloseDbConnection();
                pSysTable.CloseDbConnection();
                return;
            }
            //等高线要素类
            IFeatureClass pFeaCls = GetLineFeaCls(pFeaClsName);
            if (pFeaCls == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在等高线图层，图层名为：" + pFeaClsName);
                pSysLog.CloseDbConnection();
                pSysTable.CloseDbConnection();
                return;
            }
            //等高线高程字段名,参数ID23
            string pFieldName = GetParaValue(pSysTable, 23, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                pSysLog.CloseDbConnection();
                pSysTable.CloseDbConnection();
                return;
            }

            //等高线高程间距参数值,参数ID为21
            string paraValue = GetParaValue(pSysTable, 21, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                pSysLog.CloseDbConnection();
                pSysTable.CloseDbConnection();
                return;
            }
            double intevalValue = Convert.ToDouble(paraValue);

            //执行等高线高程值检查
            dataCheckCls.LineIntervalCheck(pFeaCls, pFieldName, m_PolyLine, intevalValue, orient, out eError);
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "等高线高程值检查失败。" + eError.Message);
                pSysLog.CloseDbConnection();
                pSysTable.CloseDbConnection();
                return;
            }
            #endregion

            pSysLog.CloseDbConnection();
            pSysTable.CloseDbConnection();

            this.Close();
        }


        /// <summary>
        /// 根据参数ID获得参数值
        /// </summary>
        /// <param name="pSysTable"></param>
        /// <param name="checkParaID"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private string GetParaValue(SysCommon.DataBase.SysTable pSysTable, int checkParaID, out Exception eError)
        {
            eError = null;
            string paraValue = "";

            string selStr = "select * from GeoCheckPara where 参数ID=" + checkParaID;
            DataTable pTable = pSysTable.GetSQLTable(selStr, out eError);
            if (eError != null)
            {
                eError = new Exception("查询表格错误，表名为：GeoCheckPara，参数ID为:" + checkParaID);
                return "";
            }

            if (pTable == null || pTable.Rows.Count == 0)
            {
                eError = new Exception("找不到记录，参数ID为:" + checkParaID);
                return "";
            }
            paraValue = pTable.Rows[0]["参数值"].ToString().Trim();            //参数类型
            return paraValue;
        }

        /// <summary>
        /// 根据图层名获得等高线要素类
        /// </summary>
        /// <param name="feaClsName"></param>
        /// <returns></returns>
        private IFeatureClass GetLineFeaCls(string feaClsName)
        {
            IFeatureClass pFeaCls = null;
            for(int i=0;i<_AppHk.MapControl.LayerCount;i++)
            {
                ILayer mLayer= _AppHk.MapControl.get_Layer(i);
                if (mLayer == null) return null;
                IFeatureLayer mFeaLyer = mLayer as IFeatureLayer;
                if (mFeaLyer == null) return null;
                IFeatureClass mFeaCls=mFeaLyer.FeatureClass;
                if (mFeaCls == null) return null;
                string tempName = (mFeaCls as IDataset).Name;
                if(tempName.Contains("."))
                {
                    tempName = tempName.Substring(tempName.IndexOf('.') + 1);
                }
                if (tempName == feaClsName)
                {
                    pFeaCls = mFeaCls;
                    break;
                }
            }
            return pFeaCls;
        }

    }
}