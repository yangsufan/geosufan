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
    /// 高程点注记一致性检查
    /// </summary>
    public class ElevationPointsAnnCheck:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;

        public ElevationPointsAnnCheck()
        {
            base._Name = "GeoDataChecker.ElevationPointsAnnCheck";
            base._Caption = "高程点注记检查";
            base._Tooltip = "检查图面中的高程点的z值是否和其对应的高程注记的内容相一致";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "高程点注记检查";
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
            //执行高程点注记检查
            //FrmMathematicsCheck mFrmMathematicsCheck = new FrmMathematicsCheck(_AppHk, enumErrorType.高程点注记一致性);
            //mFrmMathematicsCheck.ShowDialog();

            Exception eError = null;

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
                //分类代码字段名
                string codeName = TopologyCheckClass.GetCodeName(pSysTable, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                    pSysLog.CloseDbConnection();
                    pSysTable.CloseDbConnection();
                    return;
                }

                //高程点注记检查，参数ID为30
                DataTable mTable = TopologyCheckClass.GetParaValueTable(pSysTable, 30, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                    pSysLog.CloseDbConnection();
                    pSysTable.CloseDbConnection();
                    return;
                }
                if (mTable.Rows.Count == 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未进行等高线注记一致性检查参数配置！");
                    pSysLog.CloseDbConnection();
                    pSysTable.CloseDbConnection();
                    return;
                }
                //高程点注记检查搜索半径,参数ID为33
                string paraValue3 = TopologyCheckClass.GetParaValue(pSysTable, 33, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                    pSysLog.CloseDbConnection();
                    pSysTable.CloseDbConnection();
                    return;
                }
                double serchRadiu3 = Convert.ToDouble(paraValue3);
                //高程点注记检查精度控制，参数ID为36
                paraValue3 = TopologyCheckClass.GetParaValue(pSysTable, 36, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取数据减配置表失败。" + eError.Message);
                    pSysLog.CloseDbConnection();
                    pSysTable.CloseDbConnection();
                    return;
                }
                long precision3 = Convert.ToInt64(paraValue3);
                //执行高程点注记检查
                PointAnnoCheck(dataCheckCls, LstFeaClass, mTable, codeName, serchRadiu3, precision3, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "等高线注记检查失败。" + eError.Message);
                    pSysLog.CloseDbConnection();
                    pSysTable.CloseDbConnection();
                    return;
                }

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

        /// <summary>
        /// 控制点高程注记检查、高程点高程注记检查、等高线高程注记检查
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable"></param>
        /// <param name="codeName">分类代码名称</param>
        /// <param name="radiu">搜索半径</param>
        /// <param name="precision">精度控制</param>
        /// <param name="eError"></param>
        private void PointAnnoCheck(DataCheckClass pDataCheckClass, List<IFeatureClass> LstFeaClass, DataTable pTable, string codeName, double radiu, long precision, out Exception eError)
        {
            eError = null;

            if (_AppHk.DataTree == null) return;
            _AppHk.DataTree.Nodes.Clear();
            //创建处理树图
            pDataCheckClass.IntialTree(_AppHk.DataTree);
            //设置树节点颜色
            pDataCheckClass.setNodeColor(_AppHk.DataTree);
            _AppHk.DataTree.Tag = false;

            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string FeaClsName = pTable.Rows[i]["图层"].ToString().Trim();  //图层名
                string FieldName = pTable.Rows[i]["字段项"].ToString().Trim();  //字段名
                string codeValue = pTable.Rows[i]["检查项"].ToString().Trim();  //检查项
                if ((FeaClsName == "") || (!FeaClsName.Contains(";")))
                {
                    eError = new Exception("图层名为空或配置不正确!");
                    return;
                }
                if ((FieldName == "") || (!FeaClsName.Contains(";")))
                {
                    eError = new Exception("字段名为空!");
                    return;
                }
                string oriCodeValue = "";
                string desCodeValue = "";

                string[] feaNameArr = FeaClsName.Split(new char[] { ';' });
                string oriFeaClsName = feaNameArr[0].Trim();         //源要素类名称
                string desFeaClsName = feaNameArr[1].Trim();         //目标要素类名称
                string[] fieldNameArr = FieldName.Split(new char[] { ';' });
                string oriFieldName = fieldNameArr[0].Trim();        //源高程字段名
                string desFielsName = fieldNameArr[1].Trim();        //目标高程字段名

                if (codeValue != "" && codeValue.Contains(";"))
                {
                    string[] codeValueArr = codeValue.Split(new char[] { ';' });
                    oriCodeValue = codeValueArr[0].Trim();        //源要素类分类代码限制条件
                    desCodeValue = codeValueArr[1].Trim();        //目标要素类分类代码限制条件
                }

                //创建树图节点(以图层名作为根结点)
                DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node();
                pNode = (DevComponents.AdvTree.Node)pDataCheckClass.CreateAdvTreeNode(_AppHk.DataTree.Nodes, oriFeaClsName, oriFeaClsName, _AppHk.DataTree.ImageList.Images[6], true);//图层名节点
               
                pDataCheckClass.ElevAccordanceCheck(LstFeaClass, codeName, oriFeaClsName, oriCodeValue, oriFieldName, desFeaClsName, desCodeValue, desFielsName, radiu, precision, out eError);
                if (eError != null) return;

                //改变树图运行状态
                pDataCheckClass.ChangeTreeSelectNode(pNode, "完成图层" + oriFeaClsName + "高程点注记一致性检查！", false);
            }
        }
   
    }
}
