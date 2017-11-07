using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDBATool
{
    /// <summary>
    /// 陈亚飞添加
    /// </summary>
   public class ControlsDataMerge: Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;
       private IFeatureLayer m_MergeLayer = null;

       public ControlsDataMerge()
        {
            base._Name = "GeoDBATool.ControlsDataMerge";
            base._Caption = "要素融合";
            base._Tooltip = "将打断的要素融合";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "将打断的要素融合";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                try
                {
                    //判断是否加载了图层
                    if (m_Hook.MapControl.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }
                    //判断是否选中两个及两个以上的要素要素
                    if (m_Hook.MapControl.Map.SelectionCount < 2)
                    {
                        base._Enabled = false;
                        return false;
                    }

                    //获取被选中要素的数据集，判断是否编辑已经打开
                    bool isEditing = GetDatasetEditState();
                    if (!isEditing)
                    {
                        base._Enabled = false;
                        return false;
                    }

                    //所有条件都满足则设置为可用
                    base._Enabled = true;
                    return true;
                }
                catch(Exception e)
                {
                    //*******************************************************************
                    //guozheng added
                    if (ModData.SysLog != null)
                    {
                        ModData.SysLog.Write(e, null, DateTime.Now);
                    }
                    else
                    {
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                        ModData.SysLog.Write(e, null, DateTime.Now);
                    }
                    //********************************************************************

                    base._Enabled = false;
                    return false;
                }
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
            if (m_MergeLayer != null)
            {
                List<int> OIDLst = new List<int>();
                IFeatureSelection pFeatSel = m_MergeLayer as IFeatureSelection;
                ISelectionSet pSelectionSet = pFeatSel.SelectionSet;

                //如果选择的要素多余一个，则可以开始融合
                if (pSelectionSet.Count > 1)
                {
                    int pOID=-1;
                    IEnumIDs pEnumIDs=pSelectionSet.IDs;
                    pEnumIDs.Reset();
                    pOID=pEnumIDs.Next();
                    while (pOID != -1)
                    {
                        OIDLst.Add(pOID);
                        pOID = pEnumIDs.Next();
                    }
                    frmDataMerge pfrmDataMerge = new frmDataMerge(m_MergeLayer.FeatureClass, OIDLst, m_Hook);
                    pfrmDataMerge.ShowDialog();
                }
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
       //判断要素是否在同一图层以及图层的编辑状态
        private bool GetDatasetEditState()
       {

           int pSameLyr = 0;  //记录要素是否为同层
           ILayer pLayer = null;
           //判断选择的要素是否处于同一个图层
           for (int i = 0; i <m_Hook.MapControl.Map.LayerCount; i++)
           {
               IFeatureLayer pFeatLyr = null;
               IFeatureSelection pFeatSel = null;
               pLayer = m_Hook.MapControl.Map.get_Layer(i);
               if (pLayer is IGroupLayer)
               {
                   if (pLayer.Name == "示意图")
                   {
                       continue;
                   }
                   ICompositeLayer pComLayer = pLayer as ICompositeLayer;
                   for (int j = 0; j < pComLayer.Count; j++)
                   {
                       ILayer mLayer = pComLayer.get_Layer(j);
                       pFeatLyr = mLayer as IFeatureLayer;
                       if (pFeatLyr != null)
                       {
                           pFeatSel = pFeatLyr as IFeatureSelection;
                           if (pFeatSel.SelectionSet.Count > 0)
                           {
                               pSameLyr = pSameLyr + 1;
                               m_MergeLayer = pFeatLyr;//当只有一个图层被选中时，pFeatLyr就是要进行融合的目标图层
                           }
                       }
                   }
               }

               pFeatLyr = pLayer as IFeatureLayer;
               if (pFeatLyr != null)
               {
                   pFeatSel = pFeatLyr as IFeatureSelection;
                   if (pFeatSel.SelectionSet.Count > 0)
                   {
                       pSameLyr = pSameLyr + 1;
                       m_MergeLayer = pFeatLyr;//当只有一个图层被选中时，pFeatLyr就是要进行融合的目标图层
                   }
               }
           }
           //如果选择的要素所在的层数不是1，即要素不在同一个层
           if (pSameLyr != 1)
           {
               return false;
           }
           return true;
       }
    }
}

