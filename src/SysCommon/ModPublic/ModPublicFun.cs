using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;

namespace SysCommon
{
    public static class ModPublicFun
    {

        /// <summary>
        /// 从文件路径获得一个PolyGon
        /// </summary>
        /// <param name="path">文件全路径</param>
        /// <returns></returns>
        ///  //cast the polyline object to the polygon xisheng 20110926 
        private static IPolygon GetPolygonFormLine(IPolyline pPolyline)
        {
            ISegmentCollection pRing;
            IGeometryCollection pPolygon = new PolygonClass();
            IGeometryCollection pPolylineC = pPolyline as IGeometryCollection;
            object o = Type.Missing;
            for (int i = 0; i < pPolylineC.GeometryCount; i++)
            {
                pRing = new RingClass();
                pRing.AddSegmentCollection(pPolylineC.get_Geometry(i) as ISegmentCollection);
                pPolygon.AddGeometry(pRing as IGeometry, ref o, ref o);
            }
            IPolygon polygon = pPolygon as IPolygon;
            return polygon;
        }
        public static IPolygon GetPolyGonFromFile(string path)
        {
            IPolygon pGon = null;
            if (path.EndsWith(".mdb"))
            {
                string errmsg = "";
                IWorkspaceFactory pwf = new ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactoryClass();
                IWorkspace pworkspace = pwf.OpenFromFile(path, 0);
                IEnumDataset pEnumdataset = pworkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                pEnumdataset.Reset();
                IDataset pDataset = pEnumdataset.Next();
                while (pDataset != null)
                {
                    IFeatureClass pFeatureclass = pDataset as IFeatureClass;
                    if (pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolygon && pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolyline)
                    {
                        pDataset = pEnumdataset.Next();
                        continue;
                    }
                    else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                        IFeature pFeature = pCursor.NextFeature();
                        if (pFeature != null)
                        {
                            pGon = pFeature.Shape as IPolygon;
                            break;
                        }
                        else
                        {
                            pDataset = pEnumdataset.Next();
                            continue;
                        }
                    }
                    else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    {
                        IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                        IFeature pFeature = pCursor.NextFeature();
                        if (pFeature != null)
                        {

                            IPolyline pPolyline = pFeature.Shape as IPolyline;
                            pGon = GetPolygonFormLine(pPolyline);
                            if (pGon.IsClosed == false)
                            {
                                errmsg = "选择的要素不能构成封闭多边形！";
                                pGon = null;
                                pDataset = pEnumdataset.Next();
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                }
                if (pGon == null)
                {
                    IEnumDataset pEnumdataset1 = pworkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                    pEnumdataset1.Reset();
                    pDataset = pEnumdataset1.Next();
                    while (pDataset != null)
                    {
                        IFeatureDataset pFeatureDataset = pDataset as IFeatureDataset;
                        IEnumDataset pEnumDataset2 = pFeatureDataset.Subsets;
                        pEnumDataset2.Reset();
                        IDataset pDataset1 = pEnumDataset2.Next();
                        while (pDataset1 != null)
                        {
                            if (pDataset1 is IFeatureClass)
                            {

                                IFeatureClass pFeatureclass = pDataset1 as IFeatureClass;
                                if (pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolygon && pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolyline)
                                {
                                    pDataset1 = pEnumDataset2.Next();
                                    continue;
                                }
                                else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
                                {
                                    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                                    IFeature pFeature = pCursor.NextFeature();
                                    if (pFeature != null)
                                    {
                                        pGon = pFeature.Shape as IPolygon;
                                        break;
                                    }
                                    else
                                    {
                                        pDataset1 = pEnumDataset2.Next();
                                        continue;
                                    }
                                }
                                else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
                                {
                                    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                                    IFeature pFeature = pCursor.NextFeature();
                                    if (pFeature != null)
                                    {

                                        IPolyline pPolyline = pFeature.Shape as IPolyline;
                                        pGon = GetPolygonFormLine(pPolyline);
                                        if (pGon.IsClosed == false)
                                        {
                                            errmsg = "选择的要素不能构成封闭多边形！";
                                            pGon = null;
                                            pDataset1 = pEnumDataset2.Next();
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (pGon != null)
                            break;
                        pDataset = pEnumdataset1.Next();
                    }
                }
                if (pGon == null)
                {
                    if (errmsg != "")
                    {
                        System.Windows.Forms.MessageBox.Show(errmsg, "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("请选择一个包含面要素和线要素的文件", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    }
                    return pGon;
                }
            }
            else if (path.EndsWith(".shp"))
            {
                IWorkspaceFactory pwf = new ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactoryClass();
                string filepath = System.IO.Path.GetDirectoryName(path);
                string filename = path.Substring(path.LastIndexOf("\\") + 1);
                IFeatureWorkspace pFeatureworkspace = (IFeatureWorkspace)pwf.OpenFromFile(filepath, 0);
                IFeatureClass pFeatureclass = pFeatureworkspace.OpenFeatureClass(filename);
                if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                    IFeature pFeature = pCursor.NextFeature();
                    if (pFeature != null)
                    {
                        pGon = pFeature.Shape as IPolygon;
                    }
                }
                else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
                {
                    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                    IFeature pFeature = pCursor.NextFeature();
                    if (pFeature != null)
                    {

                        IPolyline pPolyline = pFeature.Shape as IPolyline;
                        pGon = GetPolygonFormLine(pPolyline);
                        if (pGon.IsClosed == false)
                        {
                            System.Windows.Forms.MessageBox.Show("选择的线要素不能构成封闭多边形！", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                            return null;
                        }
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("请选择一个面或者线要素文件！", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    return null;
                }


            }
            else if (path.EndsWith(".txt"))
            {
                string txtpath = path;
                System.IO.StreamReader smRead = new System.IO.StreamReader(txtpath, System.Text.Encoding.Default); //设置路径  
                string line;

                IPointCollection pc = pGon as IPointCollection;
                double x, y;
                while ((line = smRead.ReadLine()) != null)
                {
                    if (line.IndexOf(",") > 0)
                    {
                        try
                        {
                            x = double.Parse(line.Substring(0, line.IndexOf(",")));
                            y = double.Parse(line.Substring(line.IndexOf(",") + 1));
                        }
                        catch
                        {
                            System.Windows.Forms.MessageBox.Show("文本文件格式不正确！", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                            smRead.Close();
                            return null;
                        }
                        IPoint tmpPoint = new ESRI.ArcGIS.Geometry.Point();
                        tmpPoint.X = x;
                        tmpPoint.Y = y;
                        object ep = System.Reflection.Missing.Value;

                        pc.AddPoint(tmpPoint, ref ep, ref ep);
                    }

                }
                smRead.Close();
                ICurve pCurve = pGon as ICurve;
                if (pCurve.IsClosed == false)
                {
                    System.Windows.Forms.MessageBox.Show("导入点坐标不能构成封闭多边形！", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    return null;
                }

            }
            else if (path.EndsWith("gdb"))
            {
                string errmsg = "";
                IWorkspaceFactory pwf = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
                IWorkspace pworkspace = pwf.OpenFromFile(path.Substring(0, path.LastIndexOf("\\")), 0);
                IEnumDataset pEnumdataset = pworkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                pEnumdataset.Reset();
                IDataset pDataset = pEnumdataset.Next();
                while (pDataset != null)
                {
                    IFeatureClass pFeatureclass = pDataset as IFeatureClass;
                    if (pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolygon && pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolyline)
                    {
                        pDataset = pEnumdataset.Next();
                        continue;
                    }
                    else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                        IFeature pFeature = pCursor.NextFeature();
                        if (pFeature != null)
                        {
                            pGon = pFeature.Shape as IPolygon;
                            break;
                        }
                        else
                        {
                            pDataset = pEnumdataset.Next();
                            continue;
                        }
                    }
                    else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    {
                        IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                        IFeature pFeature = pCursor.NextFeature();
                        if (pFeature != null)
                        {

                            IPolyline pPolyline = pFeature.Shape as IPolyline;
                            pGon = GetPolygonFormLine(pPolyline);
                            if (pGon.IsClosed == false)
                            {
                                errmsg = "选择的要素不能构成封闭多边形！";
                                pGon = null;
                                pDataset = pEnumdataset.Next();
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                }
                if (pGon == null)
                {
                    IEnumDataset pEnumdataset1 = pworkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
                    pEnumdataset1.Reset();
                    pDataset = pEnumdataset1.Next();
                    while (pDataset != null)
                    {
                        IFeatureDataset pFeatureDataset = pDataset as IFeatureDataset;
                        IEnumDataset pEnumDataset2 = pFeatureDataset.Subsets;
                        pEnumDataset2.Reset();
                        IDataset pDataset1 = pEnumDataset2.Next();
                        while (pDataset1 != null)
                        {
                            if (pDataset1 is IFeatureClass)
                            {

                                IFeatureClass pFeatureclass = pDataset1 as IFeatureClass;
                                if (pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolygon && pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolyline)
                                {
                                    pDataset1 = pEnumDataset2.Next();
                                    continue;
                                }
                                else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
                                {
                                    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                                    IFeature pFeature = pCursor.NextFeature();
                                    if (pFeature != null)
                                    {
                                        pGon = pFeature.Shape as IPolygon;
                                        break;
                                    }
                                    else
                                    {
                                        pDataset1 = pEnumDataset2.Next();
                                        continue;
                                    }
                                }
                                else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
                                {
                                    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                                    IFeature pFeature = pCursor.NextFeature();
                                    if (pFeature != null)
                                    {

                                        IPolyline pPolyline = pFeature.Shape as IPolyline;
                                        pGon = GetPolygonFormLine(pPolyline);
                                        if (pGon.IsClosed == false)
                                        {
                                            errmsg = "选择的要素不能构成封闭多边形！";
                                            pGon = null;
                                            pDataset1 = pEnumDataset2.Next();
                                            continue;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (pGon != null)
                            break;
                        pDataset = pEnumdataset1.Next();
                    }
                }
                if (pGon == null)
                {
                    if (errmsg != "")
                    {
                        System.Windows.Forms.MessageBox.Show(errmsg, "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("请选择一个包含面要素和线要素的文件", "提示", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    }
                    return pGon;
                }
            }
            return pGon;
        }

        //added by chulili 20111119 获取图层是否真正的可见
        public static bool GetVisibleOfLayerScale(double dCurScale, ILayer pLayer)
        {
            bool bRes = false;
            if (pLayer == null) //排除NULL值
            {
                return bRes;
            }
            //if (!pLayer.Visible)    //图层本来不可见
            //{
            //    return bRes;
            //}
            //获取图层的限制比例尺
            double dMaxScale = pLayer.MaximumScale;
            double dMinScale = pLayer.MinimumScale;
            if (dMaxScale > 0)  //若最大比例尺存在
            {
                if (dCurScale <= dMaxScale) //不满足最大比例尺限制
                {
                    return bRes;
                }
            }
            if (dMinScale > 0)  //若最小比例尺存在
            {
                if (dCurScale >= dMinScale) //不满足最小比例尺限制
                {
                    return bRes;
                }
            }
            return true;    //最大最小比例尺不存在  或者  满足最大最小比例尺
        }
        public static bool GetAllDataSets(string strServer,string strServerName,string strDataBase,string strUser,string strPassWord,string strVersion,string strDataType,out List<string> listDatasets)
        {
            //cyf 20110609 测试数据库连接 包括SDE、PDB、gdb
            SysCommon.Gis.SysGisDataSet pSysDt = new SysCommon.Gis.SysGisDataSet();
            Exception pError = null;
            listDatasets = null;
            //if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString())
            //{
            //    if (!Directory.Exists(txtDataBase.Text))
            //    {
            //        pError = new Exception("数据库文件路径不存在！");
            //        return false;
            //    }
            //    pSysDt.SetWorkspace(this.txtDataBase.Text, enumWSType.GDB, out pError);
            //}
            //else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
            //{
                pSysDt.SetWorkspace(strServer,strServerName, strDataBase ,strUser,strPassWord,strVersion , out pError);

            //}
            //else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
            //{
            //    if (!File.Exists(txtDataBase.Text))
            //    {pError = new Exception("数据库路径不存在！");
            //        return false;
            //    }

            //    pSysDt.SetWorkspace(this.txtDataBase.Text, enumWSType.PDB, out pError);
            //}
            //if (pError != null)
            //{
            //    pError = new Exception("连接数据库失败！");
            //    return false;
            //}
            ESRI.ArcGIS.Geodatabase.IWorkspace pWs = pSysDt.WorkSpace;
            if (pWs == null) return false;
            //连接成功后，将数据集加载到下拉列表框中
            if (strDataType.Equals("DLG"))
            {
                //框架要素库中数据集名称
                List<string> LstDtName = pSysDt.GetAllFeatureDatasetNames();
                if (LstDtName.Count == 0)
                {
                    //pError = new Exception("该数据库下不存在数据集，请检查！");
                    return false;
                }
                //遍历数据集名称，将数据集加载在下拉列表框中
                if (listDatasets == null)
                {
                    listDatasets = new List<string>();
                }
                foreach (string item in LstDtName)
                {
                    //历史数据集，不添加
                    if (item.ToLower().EndsWith("_GOH"))
                        continue;
                    string GetDataSetName = item;//数据集名称
                    //添加
                    listDatasets.Add(GetDataSetName );

                }

            }
            else if (strDataType.Equals("DOM") || strDataType.Equals("DEM"))
            {
                IEnumDataset pEnumDataset = null;

                pEnumDataset = pWs.get_Datasets(esriDatasetType.esriDTRasterDataset);
                if (pEnumDataset == null)
                {
                    pError = new Exception("获取栅格数据名称出错！");
                    return false;
                }
                pEnumDataset.Reset();
                IDataset pDt = pEnumDataset.Next();
                if (listDatasets == null)
                {
                    listDatasets = new List<string>();
                }
                //遍历栅格数据集
                while (pDt != null)
                {
                    string rasteName = ""; //栅格数据名称
                    rasteName = pDt.Name;
                    //添加
                    listDatasets.Add(rasteName );
                    pDt = pEnumDataset.Next();
                }


                pEnumDataset = pWs.get_Datasets(esriDatasetType.esriDTRasterCatalog);
                if (pEnumDataset == null)
                {
                    pError = new Exception("获取栅格数据名称出错！");
                    return false;
                }
                pEnumDataset.Reset();
                pDt = pEnumDataset.Next();
                if (pDt == null)
                {
                    pError = new Exception("获取栅格数据名称出错！");
                    return false;
                }

                //遍历栅格编目
                while (pDt != null)
                {
                    string rasteName = ""; //栅格数据名称
                    rasteName = pDt.Name;

                    //added by chulili 20110715过滤掉原本存在的历史库
                    if (!rasteName.ToLower().EndsWith("_GOH"))
                    {
                        //将栅格数据名称添加到数组中
                        listDatasets.Add(rasteName);

                    }
                    pDt = pEnumDataset.Next();
                }                

            }
            return true;    
        }
        /// <summary>
        /// 调整矩形框的大小，中心点不变，矩形框放大或缩小，仅作二维矩形
        /// </summary>
        /// <param name="pEnve"></param>
        /// <param name="dSize">j矩形框的大大小</param>
        public static void ResizeEnvelope(IEnvelope pEnve, double dSize)
        {
            //排错
            if (pEnve == null)
                return;
            if (dSize == 1)
                return;
            if (dSize <= 0)
                return;
            //取矩形框的高度、宽度
            double dHight = pEnve.Height;
            double dWidth = pEnve.Width;
            //取矩形框的最小最大X Y
            double dxmin = pEnve.XMin;
            double dxmax = pEnve.XMax;
            double dymin = pEnve.YMin;
            double dymax = pEnve.YMax;
            //对矩形框进行缩放
            pEnve.XMin = dxmin - ((dSize - 1) / 2) * dWidth;
            pEnve.XMax = dxmax + ((dSize - 1) / 2) * dWidth;
            pEnve.YMin = dymin - ((dSize - 1) / 2) * dHight;
            pEnve.YMax = dymax + ((dSize - 1) / 2) * dHight;

        }



       

    }
}
