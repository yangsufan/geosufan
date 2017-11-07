using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using System.IO;
using GeoDataEdit.Tools;

namespace GeoDataEdit
{
    public class ControlsImportRangePolygon : Plugin.Interface.ToolRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;

        private ITool _tool = null;
        private ICommand _cmd = null;
        public ControlsImportRangePolygon()
        {
            base._Name = "GeoDataEdit.ControlsImportRangePolygon";
            base._Caption = "保存描绘结果";
            base._Tooltip = "保存描绘结果";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Deactivate = false;
            base._Message = "保存描绘结果";
            //base._Cursor = (int)esriControlsMousePointer.esriPointerIdentify;
            //base._Image = "";
            //base._Category = "";
        }

        public override bool Enabled
        {
            get
            {
                try
                {
                    if (_AppHk.MapControl.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }
                    if (ToolDrawPolygons.ListGeometrys.Count == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }

                    base._Enabled = true;
                    return true;
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "shp文件|*.shp";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                SaveGeometry(dlg.FileName, ToolDrawPolygons.ListGeometrys);
                ToolDrawPolygons.ListGeometrys.Clear();
            }
            _AppHk.ArcGisMapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewForeground, null, null);

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;

            Plugin.Application.IAppFormRef pAppForm = hook as Plugin.Application.IAppFormRef;

        }

        private void SaveGeometry(string strFileName, List<IGeometry> pGeometrys)
        {
            if (pGeometrys.Count != 0)
            {
                IWorkspace pWorkspace = null;
                switch (System.IO.Path.GetExtension(strFileName))
                {
                    case ".shp":
                        IWorkspaceFactory pWF = new ShapefileWorkspaceFactoryClass();
                        string wPath = System.IO.Path.GetDirectoryName(strFileName);
                        pWorkspace = pWF.OpenFromFile(wPath, 0);
                        break;
                    case ".mdb":
                        pWorkspace = CreateFileGDBWorkSpace(strFileName);
                        break;
                }
                if (pWorkspace == null)
                    return;

                //ITopologicalOperator top = pGeometrys[0] as ITopologicalOperator;
                //top.Simplify();
                //IGeometry geo = null;
                //if (index == 1)
                //{
                //    geo = pGeometrys[0];
                //}
                //else
                //{
                //    geo = top.Boundary;
                //}

                IFeatureClass targetFeatureclass = CreateFeatureClass(System.IO.Path.GetFileNameWithoutExtension(strFileName), pWorkspace, null, null, pGeometrys[0]);
                if (targetFeatureclass == null) { MessageBox.Show("保存失败", "提示！", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
                CopyGeometryToFeatureClass(pGeometrys, targetFeatureclass);
                
                MessageBox.Show("保存完成", "提示！",MessageBoxButtons.OK,MessageBoxIcon.Information);
                
            }

        }

        private void CopyGeometryToFeatureClass(List<IGeometry> pGeometrys, IFeatureClass targetFeatureclass)
        {
           // if (pGeometry == null) { return; }
            IFeatureCursor pFeatureCursor = targetFeatureclass.Insert(true);
            IFeatureBuffer pFeatureBuffer = targetFeatureclass.CreateFeatureBuffer();
            
            foreach (IGeometry geo in pGeometrys)
            {
                //IGeometry geo2=null;
                //if (index == 2)
                //{
                //    ITopologicalOperator top = geo as ITopologicalOperator;
                //    top.Simplify();
                //    geo2 = top.Boundary;
                //}
                //else
                //{
                //    geo2 = geo;
                //}
                pFeatureBuffer.Shape = geo;
                pFeatureCursor.InsertFeature(pFeatureBuffer);
                pFeatureCursor.Flush();
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;

        }
        public IWorkspace CreateFileGDBWorkSpace(string filename)
        {
            IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactoryClass();
            if (System.IO.File.Exists(filename))
            {
                if (pWorkspaceFactory.IsWorkspace(filename))
                {
                    IWorkspace pTempWks = pWorkspaceFactory.OpenFromFile(filename, 0);
                    return pTempWks;
                }
            }

            IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create("" + System.IO.Path.GetDirectoryName(filename) + "", "" + System.IO.Path.GetFileNameWithoutExtension(filename) + "", null, 0);
            IName name = (ESRI.ArcGIS.esriSystem.IName)pWorkspaceName;

            IWorkspace fileGDB_workspace = (IWorkspace)name.Open();
            return fileGDB_workspace;
        }

        public IWorkspace CreateShapeFileWorkSpace(string filename)
        {
            IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactory() as IWorkspaceFactory;
            filename = filename.Substring(0, filename.LastIndexOf("."));
            if (System.IO.Directory.Exists(filename))
            {
                if (pWorkspaceFactory.IsWorkspace(filename))
                {
                    IWorkspace pTempWks = pWorkspaceFactory.OpenFromFile(filename, 0);
                    return pTempWks;
                }
            }

            IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create("" + System.IO.Path.GetDirectoryName(filename) + "", "" + System.IO.Path.GetFileName(filename) + "", null, 0);
            IName name = (ESRI.ArcGIS.esriSystem.IName)pWorkspaceName;

            IWorkspace shapefile_workspace = (IWorkspace)name.Open();
            return shapefile_workspace;
        }
        //创建featureclass
        private IFeatureClass CreateFeatureClass(string name, IWorkspace pWorkspace, UID uidCLSID, UID uidCLSEXT, IGeometry pGeometry)
        {
            try
            {
                if (uidCLSID == null)
                {
                    uidCLSID = new UIDClass();
                    uidCLSID.Value = "{52353152-891A-11D0-BEC6-00805F7C4268}";
                }

                // 设置 uidCLSEXT (if Null)
                //uidCLSEXT == null;

                IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                IFields fields = new FieldsClass();
                IFieldsEdit fieldsEdit = (IFieldsEdit)fields;
                IField oidField = new FieldClass();
                IFieldEdit oidFieldEdit = (IFieldEdit)oidField; oidFieldEdit.Name_2 = "OID";
                oidFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
                fieldsEdit.AddField(oidField);


                IGeometryDef geometryDef = new GeometryDefClass();
                IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;
                geometryDefEdit.GeometryType_2 = pGeometry.GeometryType;
                ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironmentClass();
                ISpatialReference spatialReference = pGeometry.SpatialReference;
                ISpatialReferenceResolution spatialReferenceResolution = (ISpatialReferenceResolution)spatialReference;
                spatialReferenceResolution.ConstructFromHorizon();
                ISpatialReferenceTolerance spatialReferenceTolerance = (ISpatialReferenceTolerance)spatialReference;
                spatialReferenceTolerance.SetDefaultXYTolerance();
                geometryDefEdit.SpatialReference_2 = spatialReference;


                IField geometryField = new FieldClass();
                IFieldEdit geometryFieldEdit = (IFieldEdit)geometryField;
                geometryFieldEdit.Name_2 = "Shape";
                geometryFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;
                geometryFieldEdit.GeometryDef_2 = geometryDef;
                fieldsEdit.AddField(geometryField);

                // Use IFieldChecker to create a validated fields collection. 
                IFieldChecker fieldChecker = new FieldCheckerClass();
                IEnumFieldError enumFieldError = null;
                IFields validatedFields = null;
                fieldChecker.ValidateWorkspace = pWorkspace;
                fieldChecker.Validate(fields, out enumFieldError, out validatedFields);
                IFeatureClass targetFeatureclass = null;
                targetFeatureclass = pFeatureWorkspace.CreateFeatureClass("" + name + "", fields, uidCLSID, uidCLSEXT, esriFeatureType.esriFTSimple, "Shape", "");
                return targetFeatureclass;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Cannot create a low precision dataset in a high precision database.")
                {
                    MessageBox.Show("数据必须是ArcGis9.3的数据，请将数据处理成ArcGis9.2的数据！");
                }
            }
            IFeatureClass featureclass = null;
            return featureclass;
        }

    }
}

        

    
