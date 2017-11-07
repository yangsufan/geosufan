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
    /// 面面积逻辑性检查
    /// </summary>
    public class ControlsAreaLogicCheck:Plugin.Interface.CommandRefBase
    {
       private Plugin.Application.IAppGISRef _AppHk;

        public ControlsAreaLogicCheck()
        {
            base._Name = "GeoDataChecker.ControlsAreaLogicCheck";
            base._Caption = "面面积逻辑性检查";
            base._Tooltip = "检查指定面数据的面积是否符合指定逻辑值（可覆盖房屋面畸形检查）";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "面面积逻辑性检查";
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
            //FrmMathematicsCheck mFrmMathematicsCheck = new FrmMathematicsCheck(_AppHk, enumErrorType.面面积逻辑性);
            //mFrmMathematicsCheck.ShowDialog();

            Exception eError = null;

            FrmLineLengthCheck pFrmLineLengthCheck = new FrmLineLengthCheck();
            if (pFrmLineLengthCheck.ShowDialog() == DialogResult.OK)
            {
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

                    DataTable mTable = TopologyCheckClass.GetParaValueTable(pSysTable, 4, out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                        pSysLog.CloseDbConnection();
                        pSysTable.CloseDbConnection();
                        return;
                    }
                    if (mTable.Rows.Count == 0)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未进行面面积检查参数配置！");
                        pSysLog.CloseDbConnection();
                        pSysTable.CloseDbConnection();
                        return;
                    }
                    
                    double pmax = pFrmLineLengthCheck.MaxValue;
                    double pmin = pFrmLineLengthCheck.MinValue;
                    //执行面面积逻辑性检查
                    AreaCheck(dataCheckCls, LstFeaClass, mTable, codeName, pmin.ToString(), pmax.ToString(), out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "面面积逻辑性检查失败。" + eError.Message);
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
        }


       public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;
        }

        /// <summary>
        /// 面面积逻辑性检查
        /// </summary>
        /// <param name="pDataCheckClass"></param>
        /// <param name="pFeaDataset"></param>
        /// <param name="pTable">参数配置表</param>
        /// <param name="codeName">分类代码名</param>
        /// <param name="AreaMin">面积最小值</param>
        /// <param name="AreaMax">面积最大值</param>
        /// <param name="eError"></param>
        private void AreaCheck(DataCheckClass pDataCheckClass, List<IFeatureClass> LstFeaClass, DataTable pTable, string codeName, string AreaMin, string AreaMax, out Exception eError)
        {
            eError = null;
            Dictionary<string, string> feaClsInfodic = new Dictionary<string, string>();
            for (int i = 0; i < pTable.Rows.Count; i++)
            {
                string pFeaClsName = pTable.Rows[i]["图层"].ToString().Trim();//图层名
                string codeValue = pTable.Rows[i]["检查项"].ToString().Trim();//要检查的分类代码值集合
                if (pFeaClsName == "")
                {
                    eError = new Exception("图层名为空!");
                    return;
                }
                if (!feaClsInfodic.ContainsKey(pFeaClsName))
                {
                    feaClsInfodic.Add(pFeaClsName, codeValue);
                }
                else
                {
                    if (codeValue != "")
                    {
                        string tempStr = feaClsInfodic[pFeaClsName] + "," + codeValue;
                        feaClsInfodic[pFeaClsName] = tempStr;
                    }
                    else
                    {
                        feaClsInfodic[pFeaClsName] = "";
                    }
                }
            }
            pDataCheckClass.AreaLogicCheck(LstFeaClass, codeName, feaClsInfodic, AreaMin, AreaMax, out eError);
        }
    }
}
