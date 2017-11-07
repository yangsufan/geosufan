using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Carto;


namespace GeoDBATool
{
    /// <summary>
    /// 栅格数据提取  陈亚飞
    /// </summary>
    public class ControlsRasterStract: Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;     //主功能应用APP
        ILayer m_Layer = null;                            //栅格数据图层

        public ControlsRasterStract()
        {
            base._Name = "GeoDBATool.ControlsRasterStract";
            base._Caption = "数据提取";
            base._Tooltip = "根据条件提取数据";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "根据条件提取数据";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.MapControl == null) return false;
                if (m_Hook.MapControl.LayerCount == 0) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;

                if (m_Hook.ProjectTree.SelectedNode.DataKeyString != "project") return false;
                if (m_Hook.ProjectTree.SelectedNode.Tag == null) return false;

                bool b=false;
                for (int i = 0; i < m_Hook.MapControl.LayerCount; i++)
                {
                    ILayer pLayer = m_Hook.MapControl.Map.get_Layer(i);
                    if(pLayer is IGroupLayer)
                    {
                        //cyf 20110608 modify
                        //if(pLayer.Name.StartsWith("栅格数据库"))  //cyf 20110626  modify
                        //{
                            //若加载了工程中的栅格数据组，则返回true
                            ICompositeLayer pComLayer = pLayer as ICompositeLayer;
                            if(pComLayer.Count>0)
                            {
                                m_Layer = pComLayer.get_Layer(0);
                            }
                            b = true;
                            break;
                        //}
                    }
                    else 
                    {
                        continue;
                    }
                }
                    //XmlNode ProNode = m_Hook.ProjectTree.SelectedNode.Tag as XmlNode;
                    //if (ProNode == null) return false;
                    return b;
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
            FrmRasterDataStract dataRasterStractFrm = new FrmRasterDataStract(m_Hook,m_Layer);
            dataRasterStractFrm.ShowDialog();

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
    }
}
