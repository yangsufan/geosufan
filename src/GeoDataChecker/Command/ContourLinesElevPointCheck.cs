using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace GeoDataChecker
{
    /// <summary>
    /// 等高线点线矛盾检查
    /// </summary>
   public class ContourLinesElevPointCheck:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;

        public ContourLinesElevPointCheck()
        {
            base._Name = "GeoDataChecker.ContourLinesElevPointCheck";
            base._Caption = "等高线点线矛盾性检查";
            base._Tooltip = "检查高程点的高程值（z值）是否在它邻近的两条等高线的elevation值之间";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "等高线点线矛盾性检查";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk == null) return false;
                if (_AppHk.MapControl == null) return false;
                if (_AppHk.MapControl.LayerCount == 0) return false;
                return true;
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
            //执行等高线点线矛盾检查
            //FrmMathematicsCheck mFrmMathematicsCheck = new FrmMathematicsCheck(_AppHk, enumErrorType.等高线点线矛盾);
            //mFrmMathematicsCheck.ShowDialog();

            Exception eError = null;

            #region 错误日志保存
            //错误日志连接信息
            string logPath = TopologyCheckClass.GeoLogPath + "Log" + System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString() + System.DateTime.Today.Day.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString()+".xls"; ;
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

            DataCheckClass dataCheckCls = new DataCheckClass(_AppHk);
            //将日志表连接信息和表名保存起来
            dataCheckCls.ErrDbCon = pSysLog.DbConn;
            dataCheckCls.ErrTableName = "错误日志";
            #endregion

            #region 获得要检查的IFeatureClass的集合
            //将Mapcontrol上所有的图层名保存起来
            List<IFeatureClass> LstFeaClass = new List<IFeatureClass>();
            for (int i = 0; i < _AppHk.MapControl.LayerCount; i++)
            {
                ILayer player = _AppHk.MapControl.get_Layer(i);
                if (player is IGroupLayer)
                {

                    ICompositeLayer pComLayer = player as ICompositeLayer;
                    for (int j = 0; j < pComLayer.Count; j++)
                    {
                        ILayer mLayer = pComLayer.get_Layer(j);
                        IFeatureLayer mfeatureLayer = mLayer as IFeatureLayer;
                        if (mfeatureLayer == null) continue;
                        IFeatureClass pfeaCls = mfeatureLayer.FeatureClass;
                        if (!LstFeaClass.Contains(pfeaCls))
                        {
                            LstFeaClass.Add(pfeaCls);
                        }
                    }
                }
                else
                {
                    IFeatureLayer pfeatureLayer = player as IFeatureLayer;
                    if (pfeatureLayer == null) continue;
                    IFeatureClass mFeaCls = pfeatureLayer.FeatureClass;
                    if (!LstFeaClass.Contains(mFeaCls))
                    {
                        LstFeaClass.Add(mFeaCls);
                    }
                }
            }
            #endregion
            try
            {
                SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();
                string conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + TopologyCheckClass.GeoDataCheckParaPath;
                pSysTable.SetDbConnection(conStr, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "GIS数据检查配置表连接失败！");
                    pSysLog.CloseDbConnection();
                    return;
                }
               
                //高程点图层,参数ID为19(还需要改进）
                string pointFeaclsname = TopologyCheckClass.GetParaValue(pSysTable, 19, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                    pSysLog.CloseDbConnection();
                    pSysTable.CloseDbConnection();
                    return;
                }
                //等高线图层,参数ID为20
                string lineFeaclsname = TopologyCheckClass.GetParaValue(pSysTable, 20, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                    pSysLog.CloseDbConnection();
                    pSysTable.CloseDbConnection();
                    return;
                }
                //高程点高程字段名,参数ID为22
                string pointFieldsname = TopologyCheckClass.GetParaValue(pSysTable, 22, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                    pSysLog.CloseDbConnection();
                    pSysTable.CloseDbConnection();
                    return;
                }
                //等高线高程字段名,参数ID为23
                string lineFieldname = TopologyCheckClass.GetParaValue(pSysTable, 23, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                    pSysLog.CloseDbConnection();
                    pSysTable.CloseDbConnection();
                    return;
                }
                //等高线间距值,参数ID为21
                string intervalValue = TopologyCheckClass.GetParaValue(pSysTable, 21, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                    pSysLog.CloseDbConnection();
                    pSysTable.CloseDbConnection();
                    return;
                }
                //高程点搜索半径,参数ID为38
                string radiu = TopologyCheckClass.GetParaValue(pSysTable, 38, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                    pSysLog.CloseDbConnection();
                    pSysTable.CloseDbConnection();
                    return;
                }

                if (_AppHk.DataTree == null) return;
                _AppHk.DataTree.Nodes.Clear();
                //创建处理树图
                dataCheckCls.IntialTree(_AppHk.DataTree);
                //设置树节点颜色
                dataCheckCls.setNodeColor(_AppHk.DataTree);
                _AppHk.DataTree.Tag = false;

                //创建树图节点(以图层名作为根结点)
                DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node();
                pNode = (DevComponents.AdvTree.Node)dataCheckCls.CreateAdvTreeNode(_AppHk.DataTree.Nodes, pointFeaclsname, pointFeaclsname, _AppHk.DataTree.ImageList.Images[6], true);//图层名节点
               

                //执行等高线点线矛盾检查
                dataCheckCls.PointLineElevCheck(LstFeaClass, lineFeaclsname, lineFieldname, pointFeaclsname, pointFieldsname, Convert.ToDouble(intervalValue), out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "等高线点线矛盾检查失败！" + eError.Message);
                    pSysLog.CloseDbConnection();
                    pSysTable.CloseDbConnection();
                    return;
                }
                //改变树图运行状态
                dataCheckCls.ChangeTreeSelectNode(pNode, "完成图层" + pointFeaclsname + "等高线点线矛盾检查！", false);

                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据检查完成!");


                pSysLog.CloseDbConnection();
                pSysTable.CloseDbConnection();
                //隐藏进度条
                dataCheckCls.ShowProgressBar(false);
            }
            catch (Exception ex)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                pSysLog.CloseDbConnection();
                return;
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

