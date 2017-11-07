using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace GeoDBATool
{
    /// <summary>
    /// 进行更新处理  陈亚飞添加20101124
    /// </summary>
    public class ControlsDBDataProccess: Plugin.Interface.CommandRefBase
    {
         private Plugin.Application.IAppGISRef m_Hook;

         public ControlsDBDataProccess()
        {
            base._Name = "GeoDBATool.ControlsDBDataProccess";
            base._Caption = "处理";
            base._Tooltip = "处理";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "处理";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.MapControl == null) return false;
                return true;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            Exception pError = null;
            DateTime pNow = DateTime.Now;
            if (ModData.m_ObjWS == null) return;

            if (ModData.m_CurOperType == EnumUpdateType.新增)
            {
                #region 新增的处理
                if (ModData.m_OrgFeature == null) return;
                //获得目标要素类
                if(ModData.m_CurLayer==null||ModData.m_CurLayer is IGroupLayer)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "在处理前请先选择目标图层");
                    return;
                }
                IFeatureLayer pFeaLayer = ModData.m_CurLayer as IFeatureLayer;
                if(pFeaLayer==null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "在处理前请先选择目标图层");
                    return;
                }

                IFeatureClass pObjFeaCls = null;//目标要素类

                pObjFeaCls = pFeaLayer.FeatureClass;
                if (pObjFeaCls == null) return;
                if (pObjFeaCls.ShapeType != ModData.m_OrgFeature.Shape.GeometryType)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "源要素和目标要素几何类型不一致！");
                    return;
                }
                //**********************************************************************************************
                //guozheng added 系统运行日志，功能记录
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                List<string> Pra = new List<string>();
                Pra.Add("源要素：OID:" + ModData.m_OrgFeature.OID.ToString() + "  图层：" + (ModData.m_OrgFeature.Table as IDataset).Name);
                Pra.Add("目标图层" + (pObjFeaCls as IDataset).Name);
                SysCommon.Log.Module.SysLog.Write("一致性更新处理：" + EnumUpdateType.新增.ToString(), Pra);
                //**********************************************************************************************
                ClsUpdate.UpdateFea(m_Hook.MapControl.Map, ModData.m_OrgFeature, null, pObjFeaCls, ModData.m_ObjWS, EnumUpdateType.新增, out pError);
                if (pError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "新增要素失败！\n" + pError.Message);
                    return;
                }
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "操作成功");
                #endregion
            }
            else
            {
                //获得目标要素
                IFeature pObjFea = ClsUpdate.getFea(m_Hook.MapControl.Map,EnumFeatureType.更新要素, out pError);
                if (pError != null||pObjFea==null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", pError.Message);
                    return;
                }
                IFeatureClass pObjFeaCls = pObjFea.Class as IFeatureClass;//目标要素类
                if (pObjFeaCls == null) return;
                if (ModData.m_CurOperType == EnumUpdateType.修改)
                {
                    #region 修改的处理
                    if (ModData.m_OrgFeature == null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取参照要素失败！");
                        return;
                    }
                    //**********************************************************************************************
                    //guozheng added 系统运行日志，功能记录
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    List<string> Pra = new List<string>();
                    Pra.Add("源要素：OID:" + ModData.m_OrgFeature.OID.ToString() + "  图层：" + (ModData.m_OrgFeature.Table as IDataset).Name);
                    Pra.Add("目标图层" + (pObjFeaCls as IDataset).Name);
                    Pra.Add("目标要素：OID:" + pObjFea.OID.ToString() + "  图层：" + (pObjFea.Table as IDataset).Name);
                    SysCommon.Log.Module.SysLog.Write("一致性更新处理：" + EnumUpdateType.修改.ToString(), Pra);
                    //**********************************************************************************************
                    //进行联动更新
                    ClsUpdate.UpdateFea(m_Hook.MapControl.Map,ModData.m_OrgFeature, pObjFea, pObjFeaCls, ModData.m_ObjWS, EnumUpdateType.修改, out pError);
                    if (pError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "修改要素失败！\n" + pError.Message);
                        return;
                    }
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "操作成功");
                    #endregion
                }
                else
                {
                    #region 删除的处理
                    //**********************************************************************************************
                    //guozheng added 系统运行日志，功能记录
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    List<string> Pra = new List<string>();
                    Pra.Add("目标图层" + (pObjFeaCls as IDataset).Name);
                    Pra.Add("目标要素：OID:" + pObjFea.OID.ToString() + "  图层：" + (pObjFea.Table as IDataset).Name);
                    SysCommon.Log.Module.SysLog.Write("一致性更新处理：" + EnumUpdateType.删除.ToString(), Pra);
                    //**********************************************************************************************
                    //进行联动更新
                    ClsUpdate.UpdateFea(m_Hook.MapControl.Map,null, pObjFea, pObjFeaCls, ModData.m_ObjWS, EnumUpdateType.删除, out pError);
                    if (pError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "删除要素失败！\n" + pError.Message);
                        return;
                    }
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "操作成功");
                    #endregion
                }
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
    }
}
