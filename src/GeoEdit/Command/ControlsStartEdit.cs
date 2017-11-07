using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using System.Windows.Forms;

namespace GeoEdit
{
    /// <summary>
    /// 开启编辑
    /// </summary>
    public class ControlsStartEdit : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        //private IWorkspace m_CurWorkspace;                                     //更新库体数据工作空间
        private Dictionary<IWorkspace, List<IFeatureLayer>> m_AllLayers;     //mapcontrol上所有图层
        public ControlsStartEdit()
        {

            base._Name = "GeoEdit.ControlsStartEdit";
            base._Caption = "开启编辑";
            base._Tooltip = "开启编辑";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "开启编辑";
        }

        /// <summary>
        /// 确定是否有数据打开，并确定各按扭的显示
        /// </summary>
        public override bool Enabled
        {
            get
            {
                try
                {
                    if (m_Hook == null) return false;
                    if (m_Hook.MapControl == null) return false;
                    if (m_Hook.CurrentThread != null) return false;
                    //获取mapcontrol上所有图层
                    m_AllLayers = ModPublic.GetAllLayersFromMap(m_Hook.MapControl);
                    if (m_AllLayers == null) return false;

                    bool bres = true;
                    //for (int i = 0; i < m_Hook.MapControl.Map.LayerCount; i++)
                    //{
                    //    ILayer pLay = m_Hook.MapControl.Map.get_Layer(i);
                    //    if (pLay is IGroupLayer && pLay.Name == "更新修编数据")
                    //    {
                    //        bres = true;
                    //        ICompositeLayer pCompositeLayer = pLay as ICompositeLayer;
                    //        if (pCompositeLayer.Count > 0)
                    //        {
                    //            IFeatureLayer pFeatLay = pCompositeLayer.get_Layer(0) as IFeatureLayer;
                    //            if (pFeatLay != null)
                    //            {
                    //                IDataset pDataset = pFeatLay.FeatureClass as IDataset;
                    //                m_CurWorkspace = pDataset.Workspace;
                    //                IWorkspaceEdit pWorkspaceEdit = pDataset.Workspace as IWorkspaceEdit;
                    //                if (pWorkspaceEdit.IsBeingEdited())
                    //                {
                    //                    bres = false;
                    //                }
                    //            }
                    //        }
                    //        break;
                    //    }
                    //}

                    foreach (KeyValuePair<IWorkspace, List<IFeatureLayer>> keyValue in m_AllLayers)
                    {
                        IWorkspaceEdit pWorkspaceEdit = keyValue.Key as IWorkspaceEdit;
                        if (pWorkspaceEdit.IsBeingEdited() == true)
                        {
                            bres = false;
                            break;
                        }
                    }

                    return bres;
                }
                catch
                {
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
            IWorkspace pCurWorkspace = null;// m_CurWorkspace;
            //当工作空间为多个时需提供选择对话框
            if (m_AllLayers == null || m_AllLayers.Count == 0) return;
            if (m_AllLayers.Count > 1)
            {
                FrmStartEdit frmStarEdit = new FrmStartEdit();
                frmStarEdit.AllEditInfo = m_AllLayers;
                frmStarEdit.ShowDialog();
                if (frmStarEdit.DialogResult == DialogResult.OK)
                {
                    pCurWorkspace = frmStarEdit.SelectWorkspace;
                }
                else
                {
                    return;
                }
            }
            else
            {
                foreach (IWorkspace pWorkspace in m_AllLayers.Keys)
                {
                    pCurWorkspace = pWorkspace;
                }
            }

            if (pCurWorkspace == null) return;

            //================================================================
            //chenyafei  20110105  add：在编辑前注册版本
            //只针对SDE数据
            if (pCurWorkspace.WorkspaceFactory.WorkspaceType != esriWorkspaceType.esriRemoteDatabaseWorkspace)
            {
                //若不是SDE数据，则不允许编辑
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请加载SDE数据进行编辑");
                return;
            }
            //只针对注册版本的数据
            IEnumDataset pEnumDataset = pCurWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            if (pEnumDataset == null) return;

            IDataset pFeaDt = pEnumDataset.Next();
           
            while (pFeaDt != null)
            {
                if (pFeaDt.Name.ToUpper().EndsWith("_GOH"))
                {
                    pFeaDt = pEnumDataset.Next();
                    continue;
                }
                IVersionedObject pVerObj = pFeaDt as IVersionedObject;
                if (!pVerObj.IsRegisteredAsVersioned)
                {
                    //注册版本
                    if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "该数据还未注册版本，是否注册版本以便进行编辑？"))
                    {
                        pVerObj.RegisterAsVersioned(true);
                        break;
                    }
                    else
                    {
                        return;
                    }
                }
                //else
                //{
                //    pVerObj.RegisterAsVersioned(false);
                //}
                pFeaDt = pEnumDataset.Next();
            }

            //==================================================================

            IWorkspaceEdit pWorkspaceEdit = pCurWorkspace as IWorkspaceEdit;
            if (!pWorkspaceEdit.IsBeingEdited())
            {
                try
                {
                    pWorkspaceEdit.StartEditing(true);
                    pWorkspaceEdit.EnableUndoRedo();
                }
                catch (Exception eError)
                {
                    //******************************************
                    //guozheng added System Exception log
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    SysCommon.Log.Module.SysLog.Write(eError);
                    //******************************************
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不能编辑该工作空间,请检查是否为只读或被其它用户占用！");
                    return;
                }
            }

            MoData.v_CurWorkspaceEdit = pWorkspaceEdit;

            //将开启编辑的图层加到图层下拉框中
            if (Plugin.ModuleCommon.DicCommands.ContainsKey("GeoEdit.LayerControl"))
            {
                LayerControl pLayerControl = Plugin.ModuleCommon.DicCommands["GeoEdit.LayerControl"] as LayerControl;
                if (pLayerControl != null)
                {
                    pLayerControl.GetLayers();
                }
            }
            //******************************************************************
            //guozheng 2010-11-4 added 开启编辑成功后获取当前的数据库版本
            Exception ex = null;
            clsUpdataEnvironmentOper UpOper = new clsUpdataEnvironmentOper();
            UpOper.HisWs = pCurWorkspace;
            MoData.DBVersion = UpOper.GetVersion(out ex);
            if (ex != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据库版本失败。\n原因：" + ex.Message);
            }
            //******************************************************************
            ////打开日志记录表,开启事务
            //Exception exError = null;
            //XmlNode DBNode = m_Hook.DBXmlDocument.SelectSingleNode(".//数据库工程");
            //XmlElement DBElement = DBNode as XmlElement;
            //string strLogMdbPath = DBElement.GetAttribute("路径") + "\\Log\\更新日志.mdb";
            //MoData.v_LogTable = new SysCommon.DataBase.SysTable();
            //MoData.v_LogTable.SetDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strLogMdbPath+";Mode=Share Deny None;Persist Security Info=False", SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out exError);
            //MoData.v_LogTable.StartTransaction();

            //委托主窗体关闭事件
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            pAppFormRef.MainForm.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
        }

        //在退出系统前如正在编辑数据则应提示
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MoData.v_CurWorkspaceEdit != null)
            {
                bool bHasEdits = false;
                bool bSave = false;
                MoData.v_CurWorkspaceEdit.HasEdits(ref bHasEdits);
                if (bHasEdits == true)
                {
                    bSave = SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "图层已进行过编辑，是否需要保存？");
                }

                MoData.v_CurWorkspaceEdit.StopEditing(bSave);
                MoData.v_CurWorkspaceEdit = null;
                ////保存日志记录表的修改
                //MoData.v_LogTable.EndTransaction(bSave);
                //MoData.v_LogTable.CloseDbConnection();
                //MoData.v_LogTable = null;

                m_Hook.CurrentTool = "";
                m_Hook.MapControl.CurrentTool = null;
                m_Hook.MapControl.Map.ClearSelection();
                m_Hook.MapControl.ActiveView.Refresh();
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
