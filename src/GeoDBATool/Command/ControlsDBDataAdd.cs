using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesRaster;

namespace GeoDBATool
{
    /// <summary>
    /// 数据加载
    /// </summary>
    public class ControlsDBDataAdd : Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;
        private ControlsDBHistoryManage HisCommand = null;

        public ControlsDBDataAdd()
        {
            base._Name = "GeoDBATool.ControlsDBDataAdd";
            base._Caption = "加载";
            base._Tooltip = "加载数据";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "加载数据";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                //cyf 20110625
                //if (!(m_Hook.ProjectTree.SelectedNode.DataKeyString == "现势库" || m_Hook.ProjectTree.SelectedNode.DataKeyString == "历史库" || m_Hook.ProjectTree.SelectedNode.DataKeyString=="栅格数据库")) return false;
                if (!(m_Hook.ProjectTree.SelectedNode.DataKeyString == "FD" || m_Hook.ProjectTree.SelectedNode.DataKeyString == "FC" || m_Hook.ProjectTree.SelectedNode.DataKeyString == "RC" || m_Hook.ProjectTree.SelectedNode.DataKeyString == "RD")) return false;
                //end
                if (m_Hook.MapControl == null) return false;
                if (HisCommand != null && HisCommand.Checked && m_Hook.ProjectTree.SelectedNode.DataKeyString == "历史库") return false;
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

        //cyf 2011065 modify
        public override void OnClick()
        {
            Exception err = null;
            /////获取工程项目名称
            DevComponents.AdvTree.Node ProjectNode = new DevComponents.AdvTree.Node();
            ProjectNode = m_Hook.ProjectTree.SelectedNode;
            while (ProjectNode.Parent != null)
            {
                ProjectNode = ProjectNode.Parent;
            }
            //cyf 20110625 add:
            DevComponents.AdvTree.Node DBNode = new DevComponents.AdvTree.Node(); //数据库树节点
            //获取数据库节点
            DBNode = m_Hook.ProjectTree.SelectedNode;
            while (DBNode.Parent != null && DBNode.DataKeyString != "DB")
            {
                DBNode = DBNode.Parent;
            }
            if (DBNode.DataKeyString != "DB")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据库节点失败!");
                return;
            }

            DevComponents.AdvTree.Node DtSetNode = new DevComponents.AdvTree.Node(); //数据集树节点
            #region 获取数据集节点
            if (DBNode.Text == "现势库" || DBNode.Text == "历史库" || DBNode.Text == "临时库") //.DataKeyString == "现势库"
            {
                //获取数据集节点
                DtSetNode = m_Hook.ProjectTree.SelectedNode;
                while (DtSetNode.Parent != null && DtSetNode.DataKeyString != "FD")
                {
                    DtSetNode = DtSetNode.Parent;
                }
                if (DtSetNode.DataKeyString != "FD")
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据集节点失败!");
                    return;
                }
            }
            else if (DBNode.Text == "栅格数据库")
            {
                //cyf 20110626 add:获取栅格数据库图层节点
                DtSetNode = m_Hook.ProjectTree.SelectedNode;
                if (m_Hook.ProjectTree.SelectedNode.DataKeyString == "RC")
                {
                    while (DtSetNode.Parent != null && DtSetNode.DataKeyString != "RC")
                    {
                        DtSetNode = DtSetNode.Parent;
                    }
                    if (DtSetNode.DataKeyString != "RC")
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据集节点失败!");
                        return;
                    }
                }
                else if (m_Hook.ProjectTree.SelectedNode.DataKeyString == "RD")
                {

                    while (DtSetNode.Parent != null && DtSetNode.DataKeyString != "RD")
                    {
                        DtSetNode = DtSetNode.Parent;
                    }
                    if (DtSetNode.DataKeyString != "RD")
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据集节点失败!");
                        return;
                    }
                }
                //end
            }
            #endregion


            XmlElement elementTemp = (DBNode.Tag as XmlElement).SelectSingleNode(".//连接信息") as XmlElement;
            IWorkspace TempWorkSpace = ModDBOperator.GetDBInfoByXMLNode(elementTemp, "") as IWorkspace;
            if (TempWorkSpace == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据库失败!");
                return;
            }
            //cyf 20110625 modify
            ILayer player = null;
            //ILayer player = ModDBOperator.GetGroupLayer(m_Hook.MapControl, m_Hook.ProjectTree.SelectedNode.DataKeyString + "_" + ProjectNode.Text);
            if (m_Hook.ProjectTree.SelectedNode.DataKeyString == "FD")
            {
                player = ModDBOperator.GetGroupLayer(m_Hook.MapControl, m_Hook.ProjectTree.SelectedNode.Text + "_" + ProjectNode.Text);
            }
            else if (m_Hook.ProjectTree.SelectedNode.DataKeyString == "FC")
            {
                player = ModDBOperator.GetGroupLayer(m_Hook.MapControl, DtSetNode.Text + "_" + ProjectNode.Text);
            }
            //cyf 20110626 add:添加获取栅格数据图层
            else if (m_Hook.ProjectTree.SelectedNode.DataKeyString == "RC" || m_Hook.ProjectTree.SelectedNode.DataKeyString == "RD")
            {
                //获取栅格数据图层
                player = ModDBOperator.GetGroupLayer(m_Hook.MapControl, DtSetNode.Text + "_" + ProjectNode.Text);
            }
            //end
            if (player != null)
            {
                m_Hook.MapControl.Map.DeleteLayer(player);
                m_Hook.TOCControl.Update();
            }
            //end

            IGroupLayer pGroupLayer = new GroupLayerClass();

            //cyf 20110625

            if (DBNode.Text == "现势库" || DBNode.Text == "历史库" || DBNode.Text == "临时库") //.DataKeyString == "现势库"
            {
                /////////若为历史库管理状态退出该状态，加载现势库
                m_Hook.MapControl.Map.ClearLayers();
                Plugin.Interface.ICommandRef HisBaseCommand = null;
                bool GetSeccess = Plugin.ModuleCommon.DicCommands.TryGetValue("GeoDBATool.ControlsDBHistoryManage", out HisBaseCommand);
                if (GetSeccess)
                {
                    HisCommand = HisBaseCommand as ControlsDBHistoryManage;
                    if (HisCommand.Checked)
                    {
                        HisCommand.IsHistory = false;//判断是不是历史库点的加载，若是现势库就卸载掉历史库，若是历史库就不操作
                        HisCommand.OnClick();
                    }
                }
                #region 加载数据
                SysCommon.Gis.SysGisDataSet sysGisDataset = new SysCommon.Gis.SysGisDataSet(TempWorkSpace);
                //cyf 20110625 modify
                IFeatureDataset featureDataset = null;        //数据集
                if (m_Hook.ProjectTree.SelectedNode.DataKeyString == "FD")
                {
                    featureDataset = sysGisDataset.GetFeatureDataset(m_Hook.ProjectTree.SelectedNode.Text, out err);
                    if (err != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据集失败，请检查该数据集是否存在!");
                        return;
                    }
                    pGroupLayer.Name = m_Hook.ProjectTree.SelectedNode.Text + "_" + ProjectNode.Text;
                }
                else if (m_Hook.ProjectTree.SelectedNode.DataKeyString == "FC")
                {
                    featureDataset = sysGisDataset.GetFeatureDataset(DtSetNode.Text, out err);
                    if (err != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据集失败!");
                        return;
                    }
                    pGroupLayer.Name = DtSetNode.Text + "_" + ProjectNode.Text;
                }
                //end

                List<IDataset> lstDataset = sysGisDataset.GetFeatureClass(featureDataset);
                //遍历要素类，加载图层
                string dbType = "";
                string userName = "";//用户名

                userName = elementTemp.GetAttribute("用户");
                dbType = elementTemp.GetAttribute("类型");
                foreach (IDataset dataset in lstDataset)
                {
                    IFeatureClass pFeatureClass = dataset as IFeatureClass;
                    if (pFeatureClass == null) continue;
                    IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                    if (pFeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                    {
                        pFeatureLayer = new FDOGraphicsLayerClass();
                    }
                    pFeatureLayer.FeatureClass = pFeatureClass;
                    if (dbType.Trim().ToUpper() == "SDE")
                    {
                        //cyf 20110706 modify:修改为不去掉用户名 changed by xisheng 0906 去掉用户名
                        if (dataset.Name.ToUpper().Contains(userName.Trim().ToUpper()))
                        {
                            //SDE用户图层名去掉用户名
                            pFeatureLayer.Name = dataset.Name.Substring(userName.Trim().Length + 1);
                        }
                        else
                        {
                            pFeatureLayer.Name = dataset.Name;
                        }
                        //end
                    }
                    else
                    {
                        pFeatureLayer.Name = dataset.Name;
                    }
                    //cyf 20110625 modify
                    if (m_Hook.ProjectTree.SelectedNode.DataKeyString == "FC")
                    {
                        //加载指定的图层
                        if (m_Hook.ProjectTree.SelectedNode.Text != pFeatureLayer.Name) continue;
                    }
                    else if (m_Hook.ProjectTree.SelectedNode.DataKeyString == "FD")
                    {
                        //加载具备权限的图层
                        XmlElement feaclsElem = null;
                        try { feaclsElem = (m_Hook.ProjectTree.SelectedNode.Tag as XmlElement).SelectSingleNode(".//图层名") as XmlElement; } catch { }
                        if (feaclsElem != null)
                        {
                            if (!feaclsElem.GetAttribute("名称").Contains(pFeatureLayer.Name))
                            {
                                //若不具备数据权限，则不进行加载
                                continue;
                            }
                        }

                    }
                    //end
                    pGroupLayer.Add(pFeatureLayer as ILayer);
                }

                m_Hook.MapControl.Map.AddLayer(pGroupLayer);
                #endregion
            }
            //else if (DBNode.Text == "历史库")
            //{
            //    //历史库加载
            //    //ModDBOperator.WriteLog("GetCommand");
            //    Plugin.Interface.ICommandRef HisBaseCommand = null;
            //    bool GetSeccess = Plugin.ModuleCommon.DicCommands.TryGetValue("GeoDBATool.ControlsDBHistoryManage", out HisBaseCommand);
            //    if (GetSeccess)
            //    {
            //        HisCommand = HisBaseCommand as ControlsDBHistoryManage;
            //        //判断是不是历史库点的加载，若是现势库就卸载掉历史库，若是历史库就不操作
            //        HisCommand.IsHistory = true;
            //        HisCommand.OnClick();
            //        if (HisCommand.MyControlHistoryBar != null)
            //        {
            //            string HisDBType = elementTemp.GetAttribute("类型");
            //            string[] strTemp = new string[] { elementTemp.GetAttribute("服务器"), elementTemp.GetAttribute("服务名"), elementTemp.GetAttribute("数据库"), elementTemp.GetAttribute("用户"), elementTemp.GetAttribute("密码"), elementTemp.GetAttribute("版本") };

            //            HisCommand.MyControlHistoryBar.AddHistoryData(strTemp, HisDBType);
            //        }
            //    }
            //}
            //加载sde数据后，注册版本
            //if (dbType.Trim().ToUpper() == "SDE")
            //{
            //    IDataset pFeaDt = featureDataset as IDataset;
            //    if (pFeaDt != null)
            //    {
            //        IVersionedObject pVerObj = pFeaDt as IVersionedObject;
            //        if (!pVerObj.IsRegisteredAsVersioned)
            //        {
            //            //注册版本
            //            pVerObj.RegisterAsVersioned(true);
            //        }
            //        else
            //        {
            //            pVerObj.RegisterAsVersioned(false);
            //        }
            //    }

            //}

            else if (DBNode.Text == "栅格数据库")
            {
                //栅格数据加载，分为两种情况：栅格数据集、栅格编目
                //cyf 20110625 modify
                pGroupLayer.Name = m_Hook.ProjectTree.SelectedNode.Text + "_" + ProjectNode.Text;
                //end
                string rasterDBType = (m_Hook.ProjectTree.SelectedNode.Tag as XmlElement).GetAttribute("存储类型");

                elementTemp = (DBNode.Tag as XmlElement).SelectSingleNode(".//连接信息") as XmlElement;  //cyf 20110626
                TempWorkSpace = ModDBOperator.GetDBInfoByXMLNode(elementTemp, "") as IWorkspace;
                if (TempWorkSpace == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据库失败!");
                    return;
                }
                IRasterWorkspaceEx pRasterWS = TempWorkSpace as IRasterWorkspaceEx;
                if (pRasterWS == null) return;
                //string feaclsName = (elementTemp.FirstChild as XmlElement).GetAttribute("名称");
                string feaclsName = m_Hook.ProjectTree.SelectedNode.Text;
                try
                {
                    if (rasterDBType.Trim() == "栅格编目")
                    {
                        //栅格编目数据加载
                        IRasterCatalog pRasterCatalog = pRasterWS.OpenRasterCatalog(feaclsName);
                        IGdbRasterCatalogLayer pGDBRCLayer = new GdbRasterCatalogLayerClass();
                        if (!pGDBRCLayer.Setup(pRasterCatalog as ITable)) return;
                        IFeatureLayer mFeaLayer = pGDBRCLayer as IFeatureLayer;
                        pGroupLayer.Add(mFeaLayer as ILayer);
                        //IFeatureClass pFeaCls = pRasterCatalog as IFeatureClass;
                        //if (pFeaCls == null) return;
                        //IFeatureCursor pFeaCursor=pFeaCls.Search(null,false);
                        //if(pFeaCursor==null) return;
                        //IFeature pFea=pFeaCursor.NextFeature();
                        //while (pFea != null)
                        //{
                        //    IRasterCatalogItem pRCItem = pFea as IRasterCatalogItem;
                        //    IRasterDataset pRasterDt = pRCItem.RasterDataset;
                        //    IRasterLayer mRasterLayer = new RasterLayerClass();
                        //    mRasterLayer.CreateFromDataset(pRasterDt);
                        //    if (mRasterLayer == null) return;
                        //    pGroupLayer.Add(mRasterLayer as ILayer);

                        //    pFea = pFeaCursor.NextFeature();

                        //    //IFeatureLayer pFeaLayer = new FeatureLayerClass();
                        //    //pFeaLayer.FeatureClass = pFeaCls as IFeatureClass;
                        //    //pFeaLayer.Name = feaclsName;
                        //    //pGroupLayer.Add(pFeaLayer as ILayer);
                        //}


                    }
                    else if (rasterDBType.Trim() == "栅格数据集")
                    {
                        //栅格数据集加载

                        IRasterDataset pRasterDataset = pRasterWS.OpenRasterDataset(feaclsName);
                        //IRasterPyramid pRasterPyramid = pRasterDataset as IRasterPyramid;
                        //if(!pRasterPyramid.Present)
                        //{
                        //    if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "未构建金字塔，是否构建金字塔？"))
                        //    {
                        //        pRasterPyramid.Create();
                        //    }
                        //}
                        IRasterLayer pRasterLayer = new RasterLayerClass();
                        pRasterLayer.CreateFromDataset(pRasterDataset);
                        if (pRasterLayer == null) return;
                        pGroupLayer.Add(pRasterLayer as ILayer);
                    }
                    m_Hook.MapControl.Map.AddLayer(pGroupLayer);
                } catch (Exception e)
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

                    return;
                }

            }
            //对图层进行排序

            SysCommon.Gis.ModGisPub.LayersCompose(m_Hook.MapControl);

            //符号化 去掉加载的符号化 20111025 席胜 

            //GeoUtilities.ControlsRenderLayerByMxd RenderLayerByMxd = new GeoUtilities.ControlsRenderLayerByMxd();
            //RenderLayerByMxd.OnCreate(m_Hook);
            //RenderLayerByMxd.OnClick();
        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
    }
}
