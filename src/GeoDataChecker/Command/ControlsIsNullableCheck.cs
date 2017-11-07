using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace GeoDataChecker
{
    public class ControlsIsNullableCheck:Plugin.Interface.CommandRefBase
    {
       private Plugin.Application.IAppGISRef _AppHk;

        public ControlsIsNullableCheck()
        {
            base._Name = "GeoDataChecker.ControlsIsNullableCheck";
            base._Caption = "空值检查";
            base._Tooltip = "检查字段内容是否为空";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "空值检查";
        }

        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
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
            Exception eError = null;

            //FrmMathematicsCheck mFrmMathematicsCheck = new FrmMathematicsCheck(_AppHk, enumErrorType.空值);
            //mFrmMathematicsCheck.ShowDialog();

            #region 错误日志保存
            //错误日志连接信息
            string logPath = TopologyCheckClass.GeoLogPath + "Log" + System.DateTime.Today.Year.ToString() + System.DateTime.Today.Month.ToString() + System.DateTime.Today.Day.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + System.DateTime.Now.Second.ToString()+ ".xls"; ;
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

                DataTable mTable =TopologyCheckClass.GetParaValueTable(pSysTable, 2, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                    pSysLog.CloseDbConnection();
                    pSysTable.CloseDbConnection();
                    return;
                }
                if (mTable.Rows.Count == 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未进行空值检查配置！");
                    pSysLog.CloseDbConnection();
                    pSysTable.CloseDbConnection();
                    return;
                }
                //执行空值检查
                IsNullCheck(dataCheckCls, LstFeaClass, mTable, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "空值检查失败。" + eError.Message);
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
        /// 空值检查
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaClsLst"></param>
        /// <param name="pTable">参数配置表</param>
        /// <param name="eError"></param>
        private void IsNullCheck(DataCheckClass pDataCheckClass,List<IFeatureClass> pFeaClsLst, DataTable pTable, out Exception eError)
        {
            eError = null;

            //用来保存图层名和合要检查的字段名
            Dictionary<string, List<string>> feaClsInfodic = new Dictionary<string, List<string>>();
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string pFeaClsName = pTable.Rows[i]["图层"].ToString().Trim();   //图层名
                string fieldName = pTable.Rows[i]["字段项"].ToString().Trim();   //要进行检查的字段名
                if (pFeaClsName == "")
                {
                    eError = new Exception("图层名为空!");
                    return;
                }
                if (fieldName == "")
                {
                    eError = new Exception("字段名为空!");
                    return;
                }
                if (!feaClsInfodic.ContainsKey(pFeaClsName))
                {
                    List<string> fieldList = new List<string>();
                    fieldList.Add(fieldName);
                    feaClsInfodic.Add(pFeaClsName, fieldList);
                }
                else
                {
                    if (!feaClsInfodic[pFeaClsName].Contains(fieldName))
                    {
                        feaClsInfodic[pFeaClsName].Add(fieldName);
                    }
                }
            }
            pDataCheckClass.IsNullableCheck(pFeaClsLst, feaClsInfodic, out eError);
        }

    }
}
