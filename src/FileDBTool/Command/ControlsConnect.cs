using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;

namespace FileDBTool
{
    public class ControlsConnect:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;

        public ControlsConnect()
        {
            base._Name = "FileDBTool.ControlsConnect";
            base._Caption = "连接";
            base._Tooltip = "连接库体";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "连接库体";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey == null) return false;
                if (m_Hook.MapControl == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey.ToString() != EnumTreeNodeType.DATACONNECT.ToString() && m_Hook.ProjectTree.SelectedNode.DataKey.ToString() != EnumTreeNodeType.DATABASE.ToString())
                    return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey.ToString() == EnumTreeNodeType.DATABASE.ToString()&&m_Hook.ProjectTree.SelectedNode.ImageIndex==2)
                {
                    //已连接上，按钮不可用
                    return false;
                }
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
            Exception eError = null;
            DevComponents.AdvTree.Node SelNode = m_Hook.ProjectTree.SelectedNode;
            if (SelNode.DataKey.ToString() == EnumTreeNodeType.DATACONNECT.ToString())
            {
                //建立连接
                if(SelNode.Name=="FileDBConnNode")
                {
                    //文件连接
                    frmConSet frmconset = new frmConSet();
                    frmconset.ShowDialog();
                }
                else if (SelNode.Name == "SpatialDBConnNode")
                {
                    //空间连接
                }
                
            }
            else if (SelNode.DataKey.ToString() == EnumTreeNodeType.DATABASE.ToString())
            {
                //读取xml连接参数，连接库体
                Exception ex = null;
                string ConType = SelNode.Name;
                if ("文件连接" == ConType)
                {
                    //文件库连接
                    XmlElement Ele = SelNode.Tag as XmlElement;
                    if (null == Ele) return;
                    string ip = Ele.GetAttribute("服务器");
                    string id = Ele.GetAttribute("用户");
                    string password = Ele.GetAttribute("密码");
                    string dbConnStr = Ele.GetAttribute("MetaDBConn");
                    ModDBOperator.ConnectDB(dbConnStr, ip, id, password, out eError);
                    if(eError!=null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", eError.Message);
                        return;
                    }
                    /////////////////////加载底图(2010.5.31临时添加)/////////////////////////
                    //SysCommon.Gis.SysGisDataSet pSysGis = new SysCommon.Gis.SysGisDataSet();
                    //pSysGis.SetWorkspace(dbConnStr, SysCommon.enumWSType.PDB, out ex);
                    //if (ex != null)
                    //{
                    //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接范围图层失败！");
                    //    return;
                    //}
                    //IFeatureLayer featurelayer = new FeatureLayerClass();
                    //IFeatureClass featureclass = pSysGis.GetFeatureClass("ProjectRange", out ex);
                    //if (ex != null)
                    //{
                    //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                    //    return;
                    //}
                    //featurelayer.FeatureClass = featureclass;
                    //ILayer layer = featurelayer as ILayer;
                    //layer.Name = "项目范围图";
                    //ModData.v_AppFileDB.MapControl.Map.AddLayer(layer);
                    //////////////////////////////////////////////
                   
                }
                if (ConType == "空间连接")
                {
                    //空间连接
                }
            }
            
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }
       
    }
}
