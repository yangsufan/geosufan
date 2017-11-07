using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.Xml;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using SysCommon.Gis;
using SysCommon.Error;
using System.Windows.Forms;
namespace GeoPageLayout
{
    public static class ModuleMap
    {
        private static string _MatchConfigPath = Application.StartupPath + "\\..\\res\\xml\\MatchConfig.xml";
	 	//判断文件格式是否符合图层目录格式 added by chulili 20110729
        private static string _HideFieldConfigPath = Application.StartupPath + "\\..\\res\\xml\\MatchFieldConfig.xml";
        public static List<string> _ListHideFields =null;//隐藏字段名称的链表，设置主显字段时，避开隐藏字段
      
      
        #region 图层排序
        /// <summary>
        /// 对mapcontrol上的图层进行排序
        /// </summary>
        /// <param name="vAxMapControl"></param>
        public static void LayersComposeEx(IMap inMap)
        {
            IMap pMap = inMap;
            int[] iLayerIndex = new int[2] { 0, 0 };
            int[] iFeatureLayerIndex = new int[3] { 0, 0, 0 };

            int iCount = pMap.LayerCount;
            //组内排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IGroupLayer groupTempLayeri = pMap.get_Layer(iIndex) as IGroupLayer;
                if (groupTempLayeri != null)
                {
                    LayersComposeEx(pMap, groupTempLayeri);
                }
            }
            //图层组排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                ILayer TempLayeri = pMap.get_Layer(iIndex) as ILayer;
                //LayersComposeEx(groupTempLayeri);
                for (int jindex = iIndex+1; jindex < iCount; jindex++)
                {

                    ILayer TempLayerj = pMap.get_Layer(jindex) as ILayer;
                    int iOrderi = -1;
                    int iOrderj = -1;
                    if (TempLayeri != null && TempLayerj != null)
                    {

                        string strOrderid_i = GetOrderIDofLayer(TempLayeri);
                        string strOrderid_j = GetOrderIDofLayer(TempLayerj);
                        if (!strOrderid_i.Equals("") && !strOrderid_j.Equals(""))
                        {
                            try
                            {
                                iOrderi = int.Parse(strOrderid_i);
                                iOrderj = int.Parse(strOrderid_j);

                            }
                            catch
                            { }
                        }
                        if (iOrderi >0 && iOrderj>0)
                        {
                            if (iOrderi > iOrderj)
                            {
                                pMap.MoveLayer(TempLayerj, iIndex);
                                TempLayeri = pMap.get_Layer(iIndex) as ILayer;
                            }
                        }
                        else
                        {
                            int intDataTypeID_i = GetDataTypeIDofLayer(TempLayeri);
                            int intDataTypeID_j = GetDataTypeIDofLayer(TempLayerj);
                            if (intDataTypeID_i > intDataTypeID_j)
                            {
                                pMap.MoveLayer(TempLayerj, iIndex);
                                TempLayeri = pMap.get_Layer(iIndex) as ILayer;
                            }
                        }
                    }
                }                
            }
        }
        public static int GetDataTypeIDofLayer(ILayer pLayer)
        {
            //类型顺序是：注记：0 点：1 线：2  面：3  栅格：4
            if (pLayer is IGroupLayer)
                return 9;
            if (pLayer is IFeatureLayer)
            {
                IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
                if(pFeatureLayer.FeatureClass!=null)
                {
                    switch (pFeatureLayer.FeatureClass.FeatureType)
                    {
                        case esriFeatureType.esriFTAnnotation:
                            return 0;
                            break;
                        case esriFeatureType.esriFTSimple:
                            switch (pFeatureLayer.FeatureClass.ShapeType)
                            {
                                case esriGeometryType.esriGeometryPoint:
                                case esriGeometryType.esriGeometryMultipoint:
                                    return 1;
                                    break;
                                case esriGeometryType.esriGeometryLine:
                                case esriGeometryType.esriGeometryPolyline:
                                    return 2;
                                    break;
                                case esriGeometryType.esriGeometryPolygon:
                                    return 3;
                                    break;
                            }
                            break;
                    }
                }

            }
            else if(pLayer is IRasterLayer)
            {
                return 4;
            }
            else if(pLayer is IRasterCatalogLayer)
            {
                return 4;
            }
            return 999;//无效数据

        }
        public static void DealOrderOfNewLayer(ESRI.ArcGIS.Controls.IMapControlDefault pMapcontrol,ILayer pLayer)
        {
            int intOrderID = -1;
            int intDataTypeID = -1;//为不同的数据类型分配不同的整数，用来排序
            string strOrderID = GetOrderIDofLayer(pLayer);
            intDataTypeID = GetDataTypeIDofLayer(pLayer);
            if (!strOrderID.Equals(""))
            {
                intOrderID = int.Parse(strOrderID);
            }
            
            if (pLayer is IGroupLayer)
            {
                LayersComposeEx(pMapcontrol.Map ,pLayer as IGroupLayer);
            }
            int IndexOfNewLayer = -1;
            for (int i = 0; i < pMapcontrol.Map.LayerCount; i++)
            {
                ILayer pTmpLayer = pMapcontrol.Map.get_Layer(i);
                if (pTmpLayer.Equals(pLayer))
                {
                    if ((pTmpLayer as ILayerGeneralProperties).LayerDescription == (pLayer as ILayerGeneralProperties).LayerDescription)
                    {
                        IndexOfNewLayer = i;
                        break;
                    }
                }
            }
            for (int i = 0; i < pMapcontrol.Map.LayerCount; i++)
            {
                ILayer pCurLayer = pMapcontrol.Map.get_Layer(i);
                string strCurOrderID = GetOrderIDofLayer(pCurLayer);
                int intCurOrderID = -1;
                if (!strCurOrderID.Equals(""))
                    intCurOrderID = int.Parse(strCurOrderID);
                if (intOrderID > 0 && intCurOrderID > 0)
                {
                    if (intOrderID < intCurOrderID)
                    {
                        if (IndexOfNewLayer > i)
                        {
                            pMapcontrol.Map.MoveLayer(pLayer, i);
                            IndexOfNewLayer = i;
                            break;
                        }
                    }
                    else if (intOrderID > intCurOrderID)
                    {
                        if (IndexOfNewLayer < i)
                        {
                            pMapcontrol.Map.MoveLayer(pLayer, i);
                            IndexOfNewLayer = i;
                        }
                    }
                }
                else
                {
                    int intCurDataTypeID = GetDataTypeIDofLayer(pCurLayer);
                    if (intDataTypeID < intCurDataTypeID)
                    {
                        if (IndexOfNewLayer > i)
                        {
                            pMapcontrol.Map.MoveLayer(pLayer, i);
                            IndexOfNewLayer = i;
                            break;
                        }
                    }
                    else if (intDataTypeID > intCurDataTypeID)
                    {
                        if (IndexOfNewLayer < i)
                        {
                            pMapcontrol.Map.MoveLayer(pLayer, i);
                            IndexOfNewLayer = i;
                        }
                    }

                }

            }
        }
        //added by chulili 20110731 获取图层顺序号
        public static string  GetOrderIDofLayer(ILayer pLayer)
        {
            try
            {
                ILayerGeneralProperties pLayerGenPro = pLayer as ILayerGeneralProperties;
                //读取图层的描述
                string strNodeXml = pLayerGenPro.LayerDescription;
                XmlDocument pXmlDoc = new XmlDocument();
                pXmlDoc.LoadXml(strNodeXml);
                //构成xml节点，根据NodeKey在节点里查询
                XmlNode pNode = pXmlDoc.ChildNodes[0];
                string strOrder = "";
                try
                {
                    strOrder = pNode.Attributes["OrderID"].Value.ToString();
                }
                catch
                { }
                return strOrder;
            }
            catch
            { 
            }
            return "";

        }
        /// <summary>
        /// 对mapcontrol上groupLayer内的图层进行排序
        /// </summary>
        /// <param name="groupLayer"></param>
        public static void LayersComposeEx(IMap pMap, IGroupLayer groupLayer)
        {
            //判断参数有效性
            if (pMap == null) return;
            if (groupLayer == null) return;
            ICompositeLayer comLayer = groupLayer as ICompositeLayer;
            int iCount = comLayer.Count;
            IMapLayers pMapLayers = pMap as IMapLayers;
            //对Dimension层进行排序 冒泡排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                ILayer TempLayeri = comLayer.get_Layer(iIndex) as ILayer;
                for (int jindex = iIndex + 1; jindex < iCount; jindex++)
                {

                    ILayer TempLayerj = comLayer.get_Layer(jindex) as ILayer;
                    if (TempLayeri != null && TempLayerj != null)
                    {
                        //获取图层顺序号
                        string strOrderid_i = GetOrderIDofLayer(TempLayeri);
                        string strOrderid_j = GetOrderIDofLayer(TempLayerj);
                        int iOrderi = -1;
                        int iOrderj = -1;

                        if (!strOrderid_i.Equals("") && !strOrderid_j.Equals(""))
                        {
                            try
                            {
                                iOrderi = int.Parse(strOrderid_i);
                                iOrderj = int.Parse(strOrderid_j);
                            }
                            catch
                            { }
                        }
                        if (iOrderi>0 &&  iOrderj>0)
                        {
                            if (iOrderi > iOrderj)
                            {
                                groupLayer.Delete(TempLayerj);
                                pMapLayers.InsertLayerInGroup(groupLayer, TempLayerj, false, iIndex);
                                TempLayeri = comLayer.get_Layer(iIndex) as ILayer;
                            }

                        }
                        else
                        {
                            int intDataTypeID_i = GetDataTypeIDofLayer(TempLayeri);
                            int intDataTypeID_j = GetDataTypeIDofLayer(TempLayerj);
                            if (intDataTypeID_i > intDataTypeID_j)
                            {
                                groupLayer.Delete(TempLayerj);
                                pMapLayers.InsertLayerInGroup(groupLayer, TempLayerj, false, iIndex);
                                TempLayeri = comLayer.get_Layer(iIndex) as ILayer;
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region 图层排序
        /// <summary>
        /// 对mapcontrol上的图层进行排序
        /// </summary>
        /// <param name="vAxMapControl"></param>
        public static void LayersCompose(ESRI.ArcGIS.Controls.IMapControlDefault pMapcontrol)
        {
            IMap pMap = pMapcontrol.Map;
            int[] iLayerIndex = new int[2] { 0, 0 };
            int[] iFeatureLayerIndex = new int[3] { 0, 0, 0 };

            int iCount = pMapcontrol.LayerCount;
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IFeatureLayer pFeatureLayer = pMap.get_Layer(iIndex) as IFeatureLayer;
                IGroupLayer groupTempLayer = pMap.get_Layer(iIndex) as IGroupLayer;
                if (groupTempLayer != null)
                {
                    LayersCompose(groupTempLayer);
                }

                if (pFeatureLayer == null) continue ;
                switch (pFeatureLayer.FeatureClass.FeatureType)
                {
                    case esriFeatureType.esriFTDimension:
                        pMap.MoveLayer(pFeatureLayer, iLayerIndex[0]);
                        iLayerIndex[0] = iLayerIndex[0] + 1;
                        break;
                    case esriFeatureType.esriFTAnnotation:
                        pMap.MoveLayer(pFeatureLayer, iLayerIndex[0] + iLayerIndex[1]);
                        iLayerIndex[1] = iLayerIndex[1] + 1;
                        break;
                    case esriFeatureType.esriFTSimple:
                        switch (pFeatureLayer.FeatureClass.ShapeType)
                        {
                            case esriGeometryType.esriGeometryPoint:
                                pMap.MoveLayer(pFeatureLayer, iLayerIndex[0] + iLayerIndex[1] + iFeatureLayerIndex[0]);
                                iFeatureLayerIndex[0] = iFeatureLayerIndex[0] + 1;
                                break;
                            case esriGeometryType.esriGeometryLine:
                            case esriGeometryType.esriGeometryPolyline:
                                pMap.MoveLayer(pFeatureLayer, iLayerIndex[0] + iLayerIndex[1] + iFeatureLayerIndex[0] + iFeatureLayerIndex[1]);
                                iFeatureLayerIndex[1] = iFeatureLayerIndex[1] + 1;
                                break;
                            case esriGeometryType.esriGeometryPolygon:
                                pMap.MoveLayer(pFeatureLayer, iLayerIndex[0] + iLayerIndex[1] + iFeatureLayerIndex[0] + iFeatureLayerIndex[1] + iFeatureLayerIndex[2]);
                                iFeatureLayerIndex[2] = iFeatureLayerIndex[2] + 1;
                                break;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 对mapcontrol上groupLayer内的图层进行排序
        /// </summary>
        /// <param name="groupLayer"></param>
        public static void LayersCompose(IGroupLayer groupLayer)
        {
            ICompositeLayer comLayer = groupLayer as ICompositeLayer;
            int iCount = comLayer.Count;

            List<ILayer> listLays = new List<ILayer>();
            //对Dimension层进行排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IFeatureLayer pFeatureLayer = comLayer.get_Layer(iIndex) as IFeatureLayer;
                IGroupLayer groupTempLayer = comLayer.get_Layer(iIndex) as IGroupLayer;
                if (groupTempLayer != null)
                {
                    LayersCompose(groupTempLayer);
                }

                if (pFeatureLayer == null) continue ;
                if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTDimension)
                {
                    listLays.Add(pFeatureLayer as ILayer);
                }
            }
            foreach (ILayer pTempLay in listLays)
            {
                groupLayer.Delete(pTempLay);
                groupLayer.Add(pTempLay);
            }

            listLays = new List<ILayer>();
            //对Annotation层进行排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IFeatureLayer pFeatureLayer = comLayer.get_Layer(iIndex) as IFeatureLayer;
                IGroupLayer groupTempLayer = comLayer.get_Layer(iIndex) as IGroupLayer;
                if (groupTempLayer != null)
                {
                    LayersCompose(groupTempLayer);
                }
                if (pFeatureLayer == null) break;
                if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    listLays.Add(pFeatureLayer as ILayer);
                }
            }
            foreach (ILayer pTempLay in listLays)
            {
                groupLayer.Delete(pTempLay);
                groupLayer.Add(pTempLay);
            }

            listLays = new List<ILayer>();
            //对点层进行排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IFeatureLayer pFeatureLayer = comLayer.get_Layer(iIndex) as IFeatureLayer;
                IGroupLayer groupTempLayer = comLayer.get_Layer(iIndex) as IGroupLayer;
                if (groupTempLayer != null)
                {
                    LayersCompose(groupTempLayer);
                }
                if (pFeatureLayer == null) break;
                if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTSimple)
                {
                    if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    {
                        listLays.Add(pFeatureLayer as ILayer);
                    }
                }
            }
            foreach (ILayer pTempLay in listLays)
            {
                groupLayer.Delete(pTempLay);
                groupLayer.Add(pTempLay);
            }

            listLays = new List<ILayer>();
            //对线层进行排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IFeatureLayer pFeatureLayer = comLayer.get_Layer(iIndex) as IFeatureLayer;
                IGroupLayer groupTempLayer = comLayer.get_Layer(iIndex) as IGroupLayer;
                if (groupTempLayer != null)
                {
                    LayersCompose(groupTempLayer);
                }
                if (pFeatureLayer == null) break;
                if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTSimple)
                {
                    if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryLine || pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    {
                        listLays.Add(pFeatureLayer as ILayer);
                    }
                }
            }
            foreach (ILayer pTempLay in listLays)
            {
                groupLayer.Delete(pTempLay);
                groupLayer.Add(pTempLay);
            }

            listLays = new List<ILayer>();
            //对面层排序
            for (int iIndex = 0; iIndex < iCount; iIndex++)
            {
                IFeatureLayer pFeatureLayer = comLayer.get_Layer(iIndex) as IFeatureLayer;
                IGroupLayer groupTempLayer = comLayer.get_Layer(iIndex) as IGroupLayer;
                if (groupTempLayer != null)
                {
                    LayersCompose(groupTempLayer);
                }
                if (pFeatureLayer == null) break;
                if (pFeatureLayer.FeatureClass.FeatureType == esriFeatureType.esriFTSimple)
                {
                    if (pFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        listLays.Add(pFeatureLayer as ILayer);
                    }
                }
            }
            foreach (ILayer pTempLay in listLays)
            {
                groupLayer.Delete(pTempLay);
                groupLayer.Add(pTempLay);
            }

            listLays = null;
        }
        #endregion

      
    }
}
