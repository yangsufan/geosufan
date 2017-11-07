using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;

using ESRI.ArcGIS.Carto;

namespace GeoDataChecker
{
    /// <summary>
    /// 代码标准化检查
    /// </summary>
    public class ControlsCodeStandardizeCheck: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef  _AppHk;
        private DataTable _ResultTable;
        private clsCodeCheckProccess _CodeErrorPro;
       
        public ControlsCodeStandardizeCheck()
        {
            base._Name = "GeoDataChecker.ControlsCodeStandardizeCheck";
            base._Caption = "代码标准化检查";
            base._Tooltip = "检查加载的图层中要素代码是否符合数据标准定义";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "代码标准化检查";
        }

        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                try
                {

                    if (_AppHk == null) return false;
                    if (_AppHk.MapControl == null) return false;
                    if (_AppHk.MapControl.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }
                    else
                    {
                        base._Enabled = true;
                        return true;
                        
                    }
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
            }
        }
        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        } 
              
        public override void OnClick()
        {
            Exception eError = null;
            #region 初始化错误列表,并绑定到DataGrid上面
            //_ResultTable = new DataTable();
            //_ResultTable.Columns.Add("要素类名称", typeof(string));
            //_ResultTable.Columns.Add("OBJECTID", typeof(string));
            //_ResultTable.Columns.Add("错误名", typeof(string));
            //_ResultTable.Columns.Add("具体描述", typeof(string));
            //_ResultTable.Columns.Add("检查时间", typeof(string));

            //_AppHk.DataCheckGrid.DataSource = null;
            //_AppHk.DataCheckGrid.DataSource = _ResultTable;
            //_AppHk.DataCheckGrid.Visible = true;
            //_AppHk.DataCheckGrid.ReadOnly = true;
            //_AppHk.DataCheckGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //for (int j = 0; j < _AppHk.DataCheckGrid.Columns.Count; j++)
            //{
            //    _AppHk.DataCheckGrid.Columns[j].Width = (_AppHk.DataCheckGrid.Width - 15) / _AppHk.DataCheckGrid.Columns.Count;
            //}
            //_AppHk.DataCheckGrid.RowHeadersWidth = 20;
            #endregion

            #region 获得要检查的IFeatureLayer的集合
            //将Mapcontrol上所有的图层名保存起来
            List<IFeatureLayer> LstFeaLayer = new List<IFeatureLayer>();
            for (int i = 0; i < _AppHk.MapControl.LayerCount; i++)
            {
                ILayer player = _AppHk.MapControl.get_Layer(i);
                if (player is IGroupLayer)
                {
                    if (player.Name == "范围")
                    {
                        continue;
                    }
                    ICompositeLayer pComLayer = player as ICompositeLayer;
                    for (int j = 0; j < pComLayer.Count; j++)
                    {
                        ILayer mLayer = pComLayer.get_Layer(j);
                        IFeatureLayer mfeatureLayer = mLayer as IFeatureLayer;
                        if (mfeatureLayer == null) return;
                        if (!LstFeaLayer.Contains(mfeatureLayer))
                        {
                            LstFeaLayer.Add(mfeatureLayer);
                        }

                    }
                }
                else
                {
                    IFeatureLayer pfeatureLayer = player as IFeatureLayer;
                    if (pfeatureLayer == null) return;
                    if (!LstFeaLayer.Contains(pfeatureLayer))
                    {
                        LstFeaLayer.Add(pfeatureLayer);
                    }

                }
            }
            #endregion

            string path = TopologyCheckClass.GeoDataCheckParaPath;// Application.StartupPath + "\\..\\Res\\checker\\GeoCheckPara.mdb";
            Plugin.Application.IAppFormRef pAppForm = _AppHk as Plugin.Application.IAppFormRef;

            #region 错误日志保存
            //错误日志连接信息
            string logPath = TopologyCheckClass.GeoLogPath + "Log" + System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString() + System.DateTime.Today.Day.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString() + ".xls"; ;
            if (File.Exists(logPath))
            {
                if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "日志文件\n'" + logPath + "'\n已经存在,是否替换?"))
                {
                    File.Delete(logPath);
                }
                else
                {
                    return;
                }
            }
            //生成日志信息EXCEL格式
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

            //DataCheckClass dataCheckCls = new DataCheckClass(_AppHk);
            //将日志表连接信息和表名保存起来
            //dataCheckCls.ErrDbCon = pSysLog.DbConn;
            //dataCheckCls.ErrTableName = "错误日志";
            #endregion
           
            //_CodeErrorPro = new clsCodeCheckProccess();
            ClsCodeCheck CodeErrerCheck = new ClsCodeCheck(_AppHk, path, LstFeaLayer);
            CodeErrerCheck.ErrDbCon = pSysLog.DbConn;
            CodeErrerCheck.ErrTableName = "错误日志";

            //CodeErrerCheck.FindErr += new GOGISErrorChecker.EventHandle(CodeErrerCheck_FindErr);
            //CodeErrerCheck.ProgressStep += new GOGISErrorChecker.ProgressHandle(CodeErrerCheck_ProgressStep);

            pAppForm.ProgressBar.Visible = true;
            CodeErrerCheck.ExcuteCheck();
            pAppForm.ProgressBar.Visible = false;


            pSysLog.CloseDbConnection();
            //_CodeErrorPro.Dispose();
        }

       private  void CodeErrerCheck_ProgressStep(object sender, int CurStep, int MaxValue)
        {
            Plugin.Application.IAppFormRef pAppForm = sender as Plugin.Application.IAppFormRef;
            pAppForm.ProgressBar.Maximum = MaxValue;
            pAppForm.ProgressBar.Minimum = 0;
            pAppForm.ProgressBar.Value = CurStep;
        }

        /// <summary>
        /// 发现错误后的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ErrorArg"></param>
        private void CodeErrerCheck_FindErr(object sender, ErrorEventArgs ErrorArg)
        {
            //若错误结果不为空
            if (ErrorArg !=null)
            {
                for (int i = 0; i < ErrorArg.OIDs.Length; i++)
                {
                    DataRow newRow = _ResultTable.NewRow();
                    newRow["要素类名称"] = ErrorArg.FeatureClassName;
                    newRow["OBJECTID"] = ErrorArg.OIDs[i];
                    newRow["错误名"] = ErrorArg.ErrorName;
                    newRow["具体描述"] = ErrorArg.ErrDescription;
                    newRow["检查时间"] = ErrorArg.CheckTime;
                    _ResultTable.Rows.Add(newRow);
                }
                //用户界面上表现出错误信息
                Plugin.Application.IAppGISRef mHook = (Plugin.Application.IAppGISRef)sender;    //错误结果列表
                if (mHook == null) return;
                mHook.DataCheckGrid.Update();

                //调用错误处理类的函数,将检查到的错误信息，写入日志
                if (_ResultTable.Rows.Count > 0)
                {
                    _CodeErrorPro.LogErr(sender, ErrorArg);
                }
               
            }

        }

       


        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;
        }
    }
}