using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using System.IO;
using GeoDataCenterFunLib;
using System.Xml;
using System.Data.OleDb;
using ESRI.ArcGIS.Geometry;
namespace GeoDataManagerFrame
{
    public class CommandImportAreaOverlay : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        public CommandImportAreaOverlay()
        {
            base._Name = "GeoDataManagerFrame.CommandImportAreaOverlay";
            base._Caption = "导入范围分析";
            base._Tooltip = "导入范围分析";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "导入范围分析";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            if (m_Hook.ArcGisMapControl == null)
                return;
            if (m_Hook.ArcGisMapControl.Map == null)
                return;
            IMap pMap = m_Hook.ArcGisMapControl.Map;
            if (pMap.LayerCount == 0)
                return;


            OpenFileDialog openFileDialog1=new OpenFileDialog();
            openFileDialog1.Filter = "shape文件(*.shp)|*.shp";

            //openFileDialog1.InitialDirectory = @"E:\test\文档和数据\Data";

            openFileDialog1.Multiselect = false;

            DialogResult pDialogResult = openFileDialog1.ShowDialog();

            if (pDialogResult != DialogResult.OK)

                return;
            IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactory();  //  1
            string pPath = openFileDialog1.FileName;

            string pFolder = System.IO.Path.GetDirectoryName(pPath);//文件夹

            string pFileName = System.IO.Path.GetFileName(pPath);//文件名


            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(pFolder, 0);  // 2

            IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;

            IFeatureClass pFC = pFeatureWorkspace.OpenFeatureClass(pFileName);  //3

            if (pFC == null)
            {
                return;
            }
            string XmlPath = Application.StartupPath + "\\..\\Res\\Xml\\展示图层树0.xml";
            SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, XmlPath);
            FrmImpLandUseReport pFrm = null;
            DialogResult pResult=DialogResult.No ;
            try
            {
                pFrm = new FrmImpLandUseReport(XmlPath);
                pResult = pFrm.ShowDialog();
            }
            catch(Exception err1)
            { }
            if (pResult == DialogResult.OK)   //行政区代码改为从数据中自动获取，不再由用户设置 20110919
            {
                string strYear = pFrm._Year;
                //string strXZQcode = pFrm._XZQcode;
                string strAreaUnit = pFrm._AreaUnit;
                bool chkTDLY = pFrm._chkTDLY;
                bool chkZTGH = pFrm._chkZTGH;
                int FractionNum = pFrm._FractionNum;
                string ResPath = pFrm._ResultPath;
                pFrm = null;
                SysCommon.CProgress pProgress = new SysCommon.CProgress("导入区域叠置分析...");
                pProgress.EnableCancel = false;
                pProgress.ShowDescription = true;
                pProgress.FakeProgress = true;
                pProgress.TopMost = true;
                pProgress.ShowProgress();
                try
                {
                    IFeatureClass pPolygonFeaCls = GetPolygoneFeatureClass(pFC);
                    if (pPolygonFeaCls != null)
                    {
                        DoImportAreaLandUseStatic(XmlPath, pPolygonFeaCls, strYear, strAreaUnit, FractionNum, chkTDLY, chkZTGH, ResPath,pProgress);
                        if (!pPolygonFeaCls.Equals(pFC))
                        {
                            IDataset pDataset = pPolygonFeaCls as IDataset;
                            pDataset.Delete();
                        }
                    }
                }
                catch(Exception err2)
                { }
                pProgress.Close();
            }
            System.IO.File.Delete(XmlPath);

        }
        private IFeatureClass GetPolygoneFeatureClass(IFeatureClass pFeaCls)
        {
            //判断文件类型
            if (pFeaCls.ShapeType == ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon)
            {
                return pFeaCls;
            }
            else if(pFeaCls.ShapeType==ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline)
            {
                IDataset pDataset=pFeaCls as IDataset;
                IFeatureWorkspace  pWorkSpace=pDataset.Workspace as IFeatureWorkspace ;
                ////获取Geometry字段
                //String shapeFieldName = pFeaCls.ShapeFieldName;
                //IFields fields = pFeaCls.Fields;
                //int geometryIndex = fields.FindField(shapeFieldName);
                //IField pGeometryfield = fields.get_Field(geometryIndex);
                ////获取IGeometryDef
                
                
                //获取空间参数
                IGeoDataset pGeodataset=pFeaCls as IGeoDataset;
                ISpatialReference pSpatialRef = pGeodataset.SpatialReference;

                //构造属性
                IFields pFields = new FieldsClass();
                IFieldsEdit pFieldsEdit = (IFieldsEdit)pFields;
                //构造Geometry属性列
                IField pField = new FieldClass();
                IFieldEdit pFieldEdit = (IFieldEdit)pField;

                pFieldEdit.Name_2 = "SHAPE";
                pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                IGeometryDef geometryDef = new GeometryDefClass();
                IGeometryDefEdit pGeoDefEdit = (IGeometryDefEdit)geometryDef;
                pGeoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
                pGeoDefEdit.SpatialReference_2 = pSpatialRef;
                pFieldEdit.GeometryDef_2 = geometryDef;
                pFieldsEdit.AddField(pField);

                IFeatureClass pPolygoneFeatureClass = pWorkSpace.CreateFeatureClass("TmpPolygone"+System.DateTime.Now.ToString("YYYYMMDDHHmmss"), pFields, null, null, esriFeatureType.esriFTSimple, "Shape", "");
                //开始编辑
                IWorkspaceEdit pTargetFWorkspaceEdit = (pPolygoneFeatureClass as IDataset).Workspace as IWorkspaceEdit;
                if (!pTargetFWorkspaceEdit.IsBeingEdited())
                {
                    pTargetFWorkspaceEdit.StartEditing(true);
                    pTargetFWorkspaceEdit.StartEditOperation();
                }


                IFeatureCursor pCursor = pFeaCls.Search(null, false);
                IFeature pFeature = pCursor.NextFeature();
                while (pFeature != null)               
                {

                    IPolyline pPolyline = pFeature.Shape as IPolyline;
                    if (pPolyline != null)
                    {
                        if (pPolyline.IsClosed == true)
                        {
                            IPolygon pGon = GetPolygonFormLine(pPolyline);
                            if (pGon != null)
                            {
                                IFeature pNewFea = pPolygoneFeatureClass.CreateFeature();
                                pNewFea.Shape = pGon as IGeometry;
                                pNewFea.Store();
                            }

                        }
                    }                 
                    pFeature = pCursor.NextFeature();
                }
                pTargetFWorkspaceEdit.StopEditOperation();
                pTargetFWorkspaceEdit.StopEditing(true);
                return pPolygoneFeatureClass;
            }
            else//既不是线文件也不是面文件，直接退出
            {
                return null;
            }
        }
        //cast the polyline object to the polygon xisheng 20110926 
        private IPolygon GetPolygonFormLine(IPolyline pPolyline)
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
        private static IFeatureClass GetFeatureClassFromConfig(string XmlPath,XmlNode StatisNode, string NodeName, string NodeKeyFieldName)
        {
            XmlNode pTmpNode = null;
            IFeatureClass pFeatureClass = null;
            try
            {
                pTmpNode = StatisNode[NodeName];
            }
            catch (Exception err)
            { }
            //获取地类图斑层 线状地物层  零星地物层 的NODEKEY
            if (pTmpNode != null)
            {
                string strNodeKey = "";
                if ((pTmpNode as XmlElement).HasAttribute(NodeKeyFieldName))
                {
                    strNodeKey = pTmpNode.Attributes[NodeKeyFieldName].Value;
                }

                pFeatureClass = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, XmlPath, strNodeKey);
            }
            return pFeatureClass;
        }
        //added by chulili 20110914 指定范围森林资源现状统计，指定范围来自一个地物类
        //changed by chulili 20110919 删除行政区编码参数，改为从数据中直接获取
        private void DoImportAreaLandUseStatic(string XmlPath,IFeatureClass pImportFeaClass, string strYear,string strAreaUnit,int FractionNum,bool SelectTDLY,bool SelectZTGH,string ResultPath, SysCommon.CProgress pProgress)
        {
            string StaticConfigPath = Application.StartupPath + "\\..\\Res\\Xml\\StatisticConfig.xml";
            SysCommon.ModSysSetting.CopyConfigXml(Plugin.ModuleCommon.TmpWorkSpace, "查询配置", StaticConfigPath);
            //读取xml文件
            XmlDocument pXmlDoc = new XmlDocument();
            pXmlDoc.Load(StaticConfigPath);
            //读取
            ModStatReport.WriteStaticLog("读取统计配置");
            if (pProgress != null)
            {
                pProgress.SetProgress("读取统计配置...");
            }
            string strSearch = "//StatisticConfig";
            string LayerTreePath = Application.StartupPath + "\\..\\Res\\Xml\\展示图层树0.xml";
            SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, LayerTreePath);
            XmlNode pXmlNode = pXmlDoc.SelectSingleNode(strSearch);
            if (pXmlNode == null)
            {
                if (pProgress != null)
                {
                    pProgress.Close();
                }
                System.IO.File.Delete(StaticConfigPath);
                System.IO.File.Delete(LayerTreePath);
                return;
            }
            //直接从数据源中获取地物类
            if (pProgress != null)
            {
                pProgress.SetProgress("获取统计图层...");
            }
            ModStatReport.WriteStaticLog("读取统计配置中的行政区");
            IFeatureClass pXZQFeatureClass = null;
            XmlNode pXZQNode = null;
            try
            {
                pXZQNode = pXmlNode["XZQ"];
            }
            catch (Exception err)
            { }
            //获取行政区划层
            if (pXZQNode != null)
            {
                string strNodeKey = "";
                if ((pXZQNode as XmlElement).HasAttribute("TableNodeKey"))
                {
                    strNodeKey = pXZQNode.Attributes["TableNodeKey"].Value;
                }
                ModStatReport.WriteStaticLog("读取行政区层");
                pXZQFeatureClass =SysCommon.ModSysSetting.GetFeatureClassByNodeKey (Plugin.ModuleCommon.TmpWorkSpace, LayerTreePath, strNodeKey);
            }
            if (pXZQFeatureClass == null)
            {
                if (pProgress != null)
                {
                    pProgress.Close();
                }
                MessageBox.Show("未找到行政区划图层!请检查配置文件");
                ModStatReport.WriteStaticLog("未找到行政区划图层，退出");
                System.IO.File.Delete(StaticConfigPath);
                System.IO.File.Delete(LayerTreePath);
                return;
            }
            //string workpath = Application.StartupPath + @"\..\OutputResults\统计成果\" + System.DateTime.Now.ToString("yyyyMMddHHmmss");
            ModStatReport.WriteStaticLog("创建临时成果数据库");
            string strMDBName = "ImportStatistic" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".mdb";
            string workSpaceName = ResultPath + "\\" + strMDBName;
            //判断结果目录是否存在，不存在则创建
            if (System.IO.Directory.Exists(ResultPath) == false)
                System.IO.Directory.CreateDirectory(ResultPath);
            //创建一个新的mdb数据库,并打开工作空间
            if (pProgress != null)
            {
                pProgress.SetProgress("创建结果库...");
            }
            IWorkspace pOutWorkSpace = ChangeJudge.CreatePDBWorkSpace(ResultPath, strMDBName);
            IFeatureWorkspace pOutFeaWorkSpace = pOutWorkSpace as IFeatureWorkspace;
            //叠置分析
            if (pProgress != null)
            {
                pProgress.SetProgress("进行叠置分析...");
            }
            //行政区划叠置
            string strXZQResName = "XZQ_RES";
            IFeatureClassName pResDataName = new FeatureClassNameClass();
            IDataset pOutDataset = (IDataset)pOutWorkSpace;
            IDatasetName pOutDatasetName = (IDatasetName)pResDataName;
            pOutDatasetName.WorkspaceName = (IWorkspaceName)pOutDataset.FullName;
            pOutDatasetName.Name = strXZQResName;
            IBasicGeoprocessor pGeoProcessor = new BasicGeoprocessorClass();
            double rol = 0.001;
            ModStatReport.WriteStaticLog("叠置行政区图层");
            pGeoProcessor.Intersect(pXZQFeatureClass as ITable, false, pImportFeaClass as ITable, false, rol, pResDataName);
            string strXZQCode =ModStatReport.GetXZQcode(workSpaceName,strXZQResName);
            if (strXZQCode.Equals(""))
            {
                if (pProgress != null)
                {
                    pProgress.Close();
                }
                MessageBox.Show("找不到导入区域所在的行政区!请验证导入区域与行政区划图层的位置关系。");
                ModStatReport.WriteStaticLog("找不到导入区域所在的行政区，退出");
                pResDataName = null;
                pGeoProcessor = null;
                pXmlDoc = null;
                System.IO.File.Delete(StaticConfigPath);
                System.IO.File.Delete(LayerTreePath);
                return;
            }
            string DLTBNodeKey = "";
            string XZDWNodeKey = "";
            string LXDWNodeKey = "";
            string JBNTNodeKey = "";
            string YTFQNodeKey = "";
            string JSYDNodeKey = "";   

            IFeatureClass pDLTBFeaCls = null;            
            IFeatureClass pXZDWFeaCls = null;            
            IFeatureClass pLXDWFeaCls = null;
            
            string strDLTBResName = "DLTB_RES";
            string strXZDWResName = "XZDW_RES";
            string strLXDWResName = "LXDW_RES";

            IFeatureClass pJBNTFeaCls = null;            
            IFeatureClass pYTFQFeaCls = null;            
            IFeatureClass pJSYDFeaCls = null;
                     
            string strJBNTResName = "JBNT_RES";
            string strYTFQResName = "YTFQ_RES";
            string strJSYDResName = "JSYD_RES";
            if (SelectTDLY)
            {
                ModStatReport.WriteStaticLog("获取叠置行政区内的森林资源图层标识号");
                ModStatReport.GetTDLYLayerKey(XmlPath, strXZQCode, strYear, out DLTBNodeKey, out XZDWNodeKey, out LXDWNodeKey);
                ModStatReport.WriteStaticLog("获取叠置行政区内的森林资源图层");
                pDLTBFeaCls = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, XmlPath, DLTBNodeKey);
                pXZDWFeaCls = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, XmlPath, XZDWNodeKey);
                pLXDWFeaCls = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, XmlPath, LXDWNodeKey);

                pProgress.SetProgress("正在叠置分析地类图斑数据...");
                pOutDatasetName.Name = strDLTBResName;
                ModStatReport.WriteStaticLog("叠置分析地类图斑图层");
                pGeoProcessor.Intersect(pDLTBFeaCls as ITable, false, pImportFeaClass as ITable, false, rol, pResDataName);
                IFeatureClass pTBFeaCls = pOutFeaWorkSpace.OpenFeatureClass(strDLTBResName);
                SysCommon.modCalArea.CalCulateAllipsoidArea(pTBFeaCls, "TBMJ", 114);
                //线状地物叠置
                if (pXZDWFeaCls != null)
                {
                    pProgress.SetProgress("正在叠置分析线状地物数据...");
                    pOutDatasetName.Name = strXZDWResName;
                    //叠置分析
                    ModStatReport.WriteStaticLog("叠置分析线状地物图层");
                    pGeoProcessor.Intersect(pXZDWFeaCls as ITable, false, pImportFeaClass as ITable, false, rol, pResDataName);
                }
                else
                { strXZDWResName = ""; }

                //零星地物叠置
                if (pLXDWFeaCls != null)
                {
                    pProgress.SetProgress("正在叠置分析零星地物数据...");
                    pOutDatasetName.Name = strLXDWResName;
                    //叠置分析
                    ModStatReport.WriteStaticLog("叠置分析零星地物图层");
                    pGeoProcessor.Intersect(pLXDWFeaCls as ITable, false, pImportFeaClass as ITable, false, rol, pResDataName);
                }
                else
                { strLXDWResName = ""; }
                try
                {
                    pProgress.SetProgress("正在组织森林资源现状数据...");
                    ModStatReport.WriteStaticLog("拷贝行政区字典");
                    CopyPasteGDBData.CopyPasteGeodatabaseData(Plugin.ModuleCommon.TmpWorkSpace, pOutWorkSpace, Plugin.ModuleCommon.TmpWorkSpace.ConnectionProperties.GetProperty("User").ToString() + ".行政区字典表", esriDatasetType.esriDTTable);
                    //CopyPasteGDBData.CopyPasteGeodatabaseData(Plugin.ModuleCommon.TmpWorkSpace, pOutWorkSpace, Plugin.ModuleCommon.TmpWorkSpace.ConnectionProperties.GetProperty("User").ToString() + ".森林用途字典", esriDatasetType.esriDTTable);
                }
                catch
                { }
            }
            if (SelectZTGH)
            {
                ModStatReport.WriteStaticLog("获取叠置行政区内的总体规划图层标识号");
                ModStatReport.GetZTGHLayerKey(XmlPath, strXZQCode, out JBNTNodeKey, out YTFQNodeKey, out JSYDNodeKey);
                pJBNTFeaCls = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, XmlPath, JBNTNodeKey);
                pYTFQFeaCls = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, XmlPath, YTFQNodeKey);
                pJSYDFeaCls = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, XmlPath, JSYDNodeKey); 

                //基本农田叠置
                if (pJBNTFeaCls != null)
                {
                    pProgress.SetProgress("正在叠置分析基本农田数据...");
                    pOutDatasetName.Name = strJBNTResName;
                    //叠置分析
                    ModStatReport.WriteStaticLog("叠置分析基本农田图层");
                    pGeoProcessor.Intersect(pJBNTFeaCls as ITable, false, pImportFeaClass as ITable, false, rol, pResDataName);
                    IFeatureClass pResJBNTFeaCls = pOutFeaWorkSpace.OpenFeatureClass(strJBNTResName);
                    SysCommon.modCalArea.CalCulateAllipsoidArea(pResJBNTFeaCls, "JBNTMJ", 114);
                }
                else
                { strJBNTResName = ""; }
                //用途分区叠置
                if (pYTFQFeaCls != null)
                {
                    pProgress.SetProgress("正在叠置分析森林用途分区数据...");
                    pOutDatasetName.Name = strYTFQResName;
                    //叠置分析
                    ModStatReport.WriteStaticLog("叠置分析森林用途图层");
                    pGeoProcessor.Intersect(pYTFQFeaCls as ITable, false, pImportFeaClass as ITable, false, rol, pResDataName);
                    IFeatureClass pResTDYTFeaCls = pOutFeaWorkSpace.OpenFeatureClass(strYTFQResName);
                    SysCommon.modCalArea.CalCulateAllipsoidArea(pResTDYTFeaCls, "MJ", 114);
                }
                else
                { strYTFQResName = ""; }
                //建设用地叠置
                if (pJSYDFeaCls != null)
                {
                    pProgress.SetProgress("正在叠置分析建设用地管制分区数据...");
                    pOutDatasetName.Name = strJSYDResName;
                    //叠置分析
                    ModStatReport.WriteStaticLog("叠置分析建设用地图层");
                    pGeoProcessor.Intersect(pJSYDFeaCls as ITable, false, pImportFeaClass as ITable, false, rol, pResDataName);
                    IFeatureClass pResJSYDFeaCls = pOutFeaWorkSpace.OpenFeatureClass(strJSYDResName);
                    SysCommon.modCalArea.CalCulateAllipsoidArea(pResJSYDFeaCls, "MJ", 114);
                }
                else
                { strJSYDResName = ""; }
            }
            pOutWorkSpace = null;
            pResDataName = null;
            pGeoProcessor = null;
            //try
            //{
            //    File.Delete(workSpaceName);
            //}
            //catch(Exception err)
            //{}
            if (!SelectTDLY)
            {
                ModStatReport.WriteStaticLog("拷贝规划模板");
                if (File.Exists(ResultPath + "\\规划统计表.xls"))
                {
                    File.Delete(ResultPath + "\\规划统计表.xls");
                }
                File.Copy(Application.StartupPath + "\\..\\Template\\规划统计模板ZTGH.xls", ResultPath + "\\规划统计表.xls");
            }
            else if (!SelectZTGH)
            {
                ModStatReport.WriteStaticLog("拷贝森林资源模板");
                if (File.Exists(ResultPath + "\\规划统计表.xls"))
                {
                    File.Delete(ResultPath + "\\规划统计表.xls");
                }
                File.Copy(Application.StartupPath + "\\..\\Template\\规划统计模板TDLY.xls", ResultPath + "\\规划统计表.xls");
            }
            else
            {
                ModStatReport.WriteStaticLog("拷贝森林资源的总体模板");
                File.Copy(Application.StartupPath + "\\..\\Template\\规划统计模板.xls", ResultPath + "\\规划统计表.xls");
            }
                if (SelectTDLY)
            {
                if (pProgress != null)
                {
                    pProgress.SetProgress("生成森林资源现状数据基础统计表...");
                }
                string resDLTB = "";
                ModStatReport.WriteStaticLog("重新计算图斑的各种面积");
                ModStatReport.ComputeXZDWMJ(workSpaceName, strDLTBResName, strXZDWResName,out resDLTB);
                if (resDLTB!="") strDLTBResName = resDLTB;
                //string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + workSpaceName;
                //OleDbConnection oledbconn = new OleDbConnection(connstr);
                //oledbconn.Open();
                //OleDbCommand oledbcomm = oledbconn.CreateCommand();

                //oledbcomm.CommandText = "update " + strDLTBResName + " set " + ModFieldConst.g_TBJMJ + " =round(" + ModFieldConst.g_TBJMJ + "*shape_area/" + ModFieldConst.g_TBMJ + ",2)," + ModFieldConst.g_TKMJ + "=round(" + ModFieldConst.g_TKMJ + "*shape_area/" + ModFieldConst.g_TBMJ + ",2) where " + ModFieldConst.g_TBMJ + ">0 and (" + ModFieldConst.g_TBMJ + "-shape_area)>0.01";
                //oledbcomm.ExecuteNonQuery();
                //if (strXZDWResName != "")
                //{
                //    oledbcomm.CommandText = "update " + strXZDWResName + " set " + ModFieldConst.g_XZMJ + " =round(" + ModFieldConst.g_XZMJ + "*shape_length/" + ModFieldConst.g_ChangDu + ",2) where " + ModFieldConst.g_ChangDu + ">0 and (" + ModFieldConst.g_ChangDu + "-shape_length)>0.01";
                //    oledbcomm.ExecuteNonQuery();
                //}
                //oledbconn.Close();
                ModStatReport.WriteStaticLog("生成基础统计表");
                ModStatReport.DoLandUseStatic(workSpaceName, strDLTBResName, strXZDWResName, strLXDWResName, null);
                if (pProgress != null)
                {
                    pProgress.SetProgress("生成森林资源现状一级分类面积汇总表...");
                }
                ModStatReport.WriteStaticLog("生成森林资源现状一级分类面积汇总表");
                ModStatReport.LandUseCurReport(ResultPath, strMDBName, strXZQCode, strAreaUnit, FractionNum, 1, ResultPath + "\\一级分类面积.xls", null);
                if (pProgress != null)
                {
                    pProgress.SetProgress("生成森林资源现状二级分类面积汇总表...");
                }
                ModStatReport.WriteStaticLog("生成森林资源现状二级分类面积汇总表");
                ModStatReport.LandUseCurReport(ResultPath, strMDBName, strXZQCode, strAreaUnit, FractionNum, 2, ResultPath + "\\二级分类面积.xls", null);
            }
            if (SelectZTGH)
            {
                ModStatReport.WriteStaticLog("基本农田占用分析");
                ModStatReport.DoJBNTStatistic(ResultPath, strMDBName, strJBNTResName, Application.StartupPath + "\\..\\Template\\基本农田压占统计表.cel", ResultPath + "\\基本农田.xls", strAreaUnit, FractionNum);
                ModStatReport.WriteStaticLog("森林用途占用分析");
                ModStatReport.DoYTFQStatistic(ResultPath, strMDBName, strYTFQResName, Application.StartupPath + "\\..\\Template\\森林用途区压占统计表.cel", ResultPath + "\\森林用途.xls", strAreaUnit, FractionNum);
                ModStatReport.WriteStaticLog("建设用地占用分析");
                ModStatReport.DoJSYDStatistic(ResultPath, strMDBName, strJSYDResName, Application.StartupPath + "\\..\\Template\\建设用地管制区压占统计表.cel", ResultPath + "\\建设用地.xls", strAreaUnit, FractionNum);
            }
            if (SelectTDLY)
            {
                pProgress.SetProgress("生成森林资源现状数据分析结果...");
                ModStatReport.WriteStaticLog("拷贝EXCEL中工作区表");
                ModStatReport.CopyExcelSheet(ResultPath + "\\一级分类面积.xls", "Sheet1", ResultPath + "\\规划统计表.xls", "森林资源现状一级分类面积");
                ModStatReport.CopyExcelSheet(ResultPath + "\\二级分类面积.xls", "Sheet1", ResultPath + "\\规划统计表.xls", "森林资源现状二级分类面积");
                File.Delete(ResultPath + "\\一级分类面积.xls");
                File.Delete(ResultPath + "\\二级分类面积.xls");
            }
            if (SelectZTGH)
            {
                pProgress.SetProgress("生成森林资源总体规划数据分析结果...");
                ModStatReport.WriteStaticLog("拷贝EXCEL中工作区表");
                ModStatReport.CopyExcelSheet(ResultPath + "\\基本农田.xls", "Sheet1", ResultPath + "\\规划统计表.xls", "基本农田");
                ModStatReport.CopyExcelSheet(ResultPath + "\\森林用途.xls", "Sheet1", ResultPath + "\\规划统计表.xls", "森林用途分区");
                ModStatReport.CopyExcelSheet(ResultPath + "\\建设用地.xls", "Sheet1", ResultPath + "\\规划统计表.xls", "建设用地管制区");

                File.Delete(ResultPath + "\\基本农田.xls");
                File.Delete(ResultPath + "\\森林用途.xls");
                File.Delete(ResultPath + "\\建设用地.xls");
            }
            try
            {
                File.Delete(workSpaceName);
            }
            catch(Exception err)
            {}
            ModStatReport.WriteStaticLog("打开统计结果");
            ModStatReport.OpenExcelFile(ResultPath + "\\规划统计表.xls");
            if (pProgress != null)
            {
                pProgress.Close();
            }
            pResDataName = null;
            pGeoProcessor = null;
            pXmlDoc = null;
            ModStatReport.WriteStaticLog("统计结束");
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pOutWorkSpace);
            pOutWorkSpace = null;
            try
            {
                File.Delete(workSpaceName);
            }
            catch (Exception err)
            { }

            System.IO.File.Delete(StaticConfigPath);
            System.IO.File.Delete(LayerTreePath);

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
        }
    }
}
