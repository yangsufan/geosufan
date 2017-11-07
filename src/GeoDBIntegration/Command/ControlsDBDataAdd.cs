using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace GeoDBIntegration
{
    class ControlsDBDataAdd : Plugin.Interface.CommandRefBase
    {
        //********************************************************//
        /*
         * guozheng 2011-5-11 added 数据库集成管理界面的数据加载
         *  步骤：使用数据库ID获取对应的数据库连接信息，
         *        使用该信息进行加载图层
         */
        //********************************************************//
          private Plugin.Application.IAppDBIntegraRef m_Hook;
          public ControlsDBDataAdd()
        {
            base._Name = "GeoDBIntegration.ControlsDBDataAdd";
            base._Caption = "加载数据库图层";
            base._Tooltip = "加载数据库图层";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "加载数据库图层";

        }
        public override bool Enabled
        {
            get
            {
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.Parent == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.Parent.Text != "框架要素数据库" && m_Hook.ProjectTree.SelectedNode.Parent.Text != "影像数据库" && m_Hook.ProjectTree.SelectedNode.Parent.Text != "高程数据库") return false;
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
            ///////////第一步：获取数据库的ID以及数据库类型名，平台名//////////////////
            DevComponents.AdvTree.Node CurProNode = this.m_Hook.ProjectTree.SelectedNode;
            string sDBType = string.Empty;
            string sDBFormate = string.Empty;
            long iDBId = -1;
            if (CurProNode == null) return;
            XmlElement DbInfoEle = CurProNode.Tag as XmlElement;
            if (DbInfoEle == null) return;
            try
            {
                Exception eError=null;
                iDBId = (long)CurProNode.DataKey ;
                sDBType = DbInfoEle.GetAttribute("数据库类型");
                sDBFormate = DbInfoEle.GetAttribute("数据库平台");
                if (!sDBFormate.StartsWith("ARCGIS")) return;/////////////////////不是ARCGIS平台的数据暂时不处理
                XmlElement DBConnectinfo = null;
                if (sDBType == "框架要素数据库")
                {
                    /////////////////加载框架要素数据库///////////////////
                    #region 加载框架要素数据库
                    XmlDocument Doc = new XmlDocument();
                    Doc.Load(ModuleData.v_feaProjectXML);
                    DBConnectinfo = ProjectXml.GetProjectInfo(Doc, iDBId);
                    //////////////////////进行加载/////////////////////////
                    ClsArcGisLayerOper.AddFeaLayer(this.m_Hook, DBConnectinfo, "", out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "加载数据发生异常：" + eError.Message);
                        return;
                    }
                    else
                    {
                        ////符号化////
                        //符号化
                        GeoUtilities.ControlsRenderLayerByMxd RenderLayerByMxd = new GeoUtilities.ControlsRenderLayerByMxd();
                        RenderLayerByMxd.OnCreate(m_Hook);
                        RenderLayerByMxd.OnClick();
                    }
                    #endregion
                }
                else if (sDBType == "高程数据库" || sDBType == "影像数据库")
                {
                    #region 加载栅格数据库
                    ///////////////加载栅格数据库////////////////////////
                    XmlDocument Doc = new XmlDocument();
                    if (sDBType == "高程数据库")
                       Doc.Load(ModuleData.v_DEMProjectXml);
                    else
                        Doc.Load(ModuleData.v_ImageProjectXml);
                    DBConnectinfo = ProjectXml.GetProjectInfo(Doc, iDBId);
                    ///////////////////////进行加载//////////////////////
                    ClsArcGisLayerOper.AddRasterLayer(this.m_Hook, DBConnectinfo, out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "加载数据发生异常：" + eError.Message);
                        return;
                    }
                    #endregion
                }
                else
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不能加载的数据库图层");
                    return;
                }

            }
            catch
            {
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
            if (m_Hook == null) return;
        }
    }
}
