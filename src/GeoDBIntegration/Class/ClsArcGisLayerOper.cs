using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace GeoDBIntegration
{
    public static  class ClsArcGisLayerOper
    {
        /////////该类实现将各种ArcGis图层加载至数据库集成管理界面的图层控件当中来////////
        /// <summary>
        /// 加载矢量库体
        /// </summary>
        /// <param name="DbEleInfo">矢量库体对应的库体信息XmlElement</param>
        /// <param name="in_MXDFile">符号化mxd文件路径</param>
        /// <param name="ex">输出错误信息</param>
        public static void AddFeaLayer(Plugin.Application.IAppDBIntegraRef m_Hook,XmlElement DbEleInfo,string in_MXDFile, out Exception ex)
        {
            ex = null;
            try
            {
                XmlElement elementTemp = DbEleInfo.SelectSingleNode(".//现势库/连接信息") as XmlElement;
                IWorkspace TempWorkSpace = ModDBOperate.GetDBInfoByXMLNode(elementTemp, "") as IWorkspace;
                if (TempWorkSpace == null)
                {
                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据库失败!");
                    ex = new Exception("连接数据库失败!");
                    return;
                }
                ILayer player = ModDBOperate.GetGroupLayer(m_Hook.MapControl, m_Hook.ProjectTree.SelectedNode.DataKeyString + "_" + m_Hook.ProjectTree.SelectedNode.Text);
                if (player != null)
                {
                    m_Hook.MapControl.Map.DeleteLayer(player);
                    m_Hook.TOCControl.Update();
                }

                IGroupLayer pGroupLayer = new GroupLayerClass();
                SysCommon.Gis.SysGisDataSet sysGisDataset = new SysCommon.Gis.SysGisDataSet(TempWorkSpace);
                IFeatureDataset featureDataset = sysGisDataset.GetFeatureDataset((elementTemp.FirstChild as XmlElement).GetAttribute("名称"), out ex);
                if (ex != null)
                {
                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据失败!");
                    ex = new Exception("获取数据发生异常:" + ex.Message);
                    return;
                }

                pGroupLayer.Name = m_Hook.ProjectTree.SelectedNode.DataKeyString + "_" + m_Hook.ProjectTree.SelectedNode.Text;
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
                        if (dataset.Name.ToUpper().Contains(userName.Trim().ToUpper()))
                        {
                            //SDE用户图层名去掉用户名
                            pFeatureLayer.Name = dataset.Name.Substring(userName.Trim().Length + 1);
                        }
                        else
                        {
                            pFeatureLayer.Name = dataset.Name;
                        }
                    }
                    else
                    {
                        pFeatureLayer.Name = dataset.Name;
                    }
                    pFeatureLayer.Visible = false;
                    pGroupLayer.Add(pFeatureLayer as ILayer);
                }
                m_Hook.MapControl.Map.AddLayer(pGroupLayer);
                SysCommon.Gis.ModGisPub.LayersCompose(m_Hook.MapControl);
            }
            catch (Exception eError)
            {
                ex = eError;
                return;
            }
        }

        /// <summary>
        /// 加载栅格库体
        /// </summary>
        /// <param name="m_Hook">主程序hook</param>
        /// <param name="DbEleInfo">矢量库体对应的库体信息XmlElement</param>
        /// <param name="ex">输出错误信息</param>
        public static void AddRasterLayer(Plugin.Application.IAppDBIntegraRef m_Hook, XmlElement DbEleInfo, out Exception ex)
        {
            ex = null;
            try
            {
                IGroupLayer pGroupLayer = new GroupLayerClass();
                pGroupLayer.Name = m_Hook.ProjectTree.SelectedNode.DataKeyString + "_" + m_Hook.ProjectTree.SelectedNode.Text;
                // string rasterDBType = (m_Hook.ProjectTree.SelectedNode.Tag as XmlElement).GetAttribute("存储类型");
               // string rasterDBType = DbEleInfo.GetAttribute("存储类型");
                string rasterDBType = string.Empty;
                XmlElement RasterEle = DbEleInfo.SelectSingleNode(".//栅格数据库") as XmlElement;
                rasterDBType = RasterEle.GetAttribute("存储类型").Trim();
                XmlElement elementTemp = DbEleInfo.SelectSingleNode(".//栅格数据库/连接信息") as XmlElement;
                IWorkspace TempWorkSpace = ModDBOperate.GetDBInfoByXMLNode(elementTemp, "") as IWorkspace;
                if (TempWorkSpace == null)
                {
                    // SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据库失败!");
                    ex = new Exception("连接数据库失发生异常");
                    return;
                }
                IRasterWorkspaceEx pRasterWS = TempWorkSpace as IRasterWorkspaceEx;
                if (pRasterWS == null) return;
                string feaclsName = (elementTemp.FirstChild as XmlElement).GetAttribute("名称");
                if (rasterDBType.Trim() == "栅格编目")
                {
                    //栅格编目数据加载
                    IRasterCatalog pRasterCatalog = pRasterWS.OpenRasterCatalog(feaclsName);
                    IGdbRasterCatalogLayer pGDBRCLayer = new GdbRasterCatalogLayerClass();
                    if (!pGDBRCLayer.Setup(pRasterCatalog as ITable)) return;
                    IFeatureLayer mFeaLayer = pGDBRCLayer as IFeatureLayer;
                    pGroupLayer.Add(mFeaLayer as ILayer);
                }
                else if (rasterDBType.Trim() == "栅格数据集")
                {
                    //栅格数据集加载

                    IRasterDataset pRasterDataset = pRasterWS.OpenRasterDataset(feaclsName);
                    IRasterLayer pRasterLayer = new RasterLayerClass();
                    pRasterLayer.CreateFromDataset(pRasterDataset);
                    if (pRasterLayer == null) return;
                    pGroupLayer.Add(pRasterLayer as ILayer);
                }
                m_Hook.MapControl.Map.AddLayer(pGroupLayer);         
        }
        catch (Exception eError)
        {
            ex = eError;
            return;
        }
     }
    }
}
