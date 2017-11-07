using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesGDB;
using System.Threading;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.esriSystem;
namespace GeoDataChecker
{
    /// <summary>
    /// 用来处理要素数据集，如果没有就创建
    /// </summary>
    public partial class FrmInitiFeatureDataset : DevComponents.DotNetBar.Office2007RibbonForm
    {
        private Plugin.Application.IAppFormRef _AppHk;//得到要用到的进度条

        public FrmInitiFeatureDataset(Plugin.Application.IAppFormRef AppHk)
        {
            _AppHk = AppHk;
            InitializeComponent();
        }
        /// <summary>
        /// 打开一个我们要操作的数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_org_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "所有文件|*.*|MDB文件|*.mdb";
            open.InitialDirectory = @"d:\";
            DialogResult result = open.ShowDialog();//打开文件
            if (result == DialogResult.OK)
            {
                txt_org.Text = open.FileName;
            }

        }
        /// <summary>
        /// 打开一个用来空间参照的PRJ文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_prj_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "PRJ文件(*.prj)|*.prj";
            open.InitialDirectory = @"d:\";
            DialogResult result = open.ShowDialog();//打开文件
            if (result == DialogResult.OK)
            {
                txt_prj.Text = open.FileName;
            }
        }

        /// <summary>
        /// 开始处理数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_enter_Click(object sender, EventArgs e)
        {
            if (txt_prj.Text != "" && txt_org.Text != "")
            {
                txt_prj.Visible = false;
                txt_org.Visible = false;
                btn_cancel.Visible = false;
                btn_enter.Visible = false;
                lab_org.Visible = false;
                labelX1.Visible = false;
                btn_org.Visible = false;
                btn_prj.Visible = false;
                pic_processbar.Visible = true;
                lab_show.Visible = true;
                Thread thread = new Thread(CreateFeatureClass);
                thread.Start();

            }
            else
            {
                SetCheckState.Message(_AppHk,"提示", "请选择路径！");
            }
        }
        /// <summary>
        /// 创建一个要素集
        /// </summary>
        private void CreateFeatureClass()
        {
            string path = txt_org.Text;
            string PRJFile = txt_prj.Text;
            AccessWorkspaceFactory pAccessFact = new AccessWorkspaceFactoryClass();

            IFeatureWorkspace pFeatureWorkspace = pAccessFact.OpenFromFile(path, 0) as IFeatureWorkspace;
            ISpatialReferenceFactory2 Isp = new SpatialReferenceEnvironmentClass();//创建一个空间参照的接口空间

            ISpatialReference spatial = Isp.CreateESRISpatialReferenceFromPRJFile(PRJFile);//利用要素类的PRJ文件参照
            ISpatialReferenceResolution pSRR = (ISpatialReferenceResolution)spatial;//设置分辨率
            pSRR.SetDefaultXYResolution();//设置默认XY值

            ISpatialReferenceTolerance pSRT = (ISpatialReferenceTolerance)spatial;
            pSRT.SetDefaultXYTolerance();//设置默认容差值
            IWorkspace space = pFeatureWorkspace as IWorkspace;

            IEnumDatasetName Dataset_name = space.get_DatasetNames(esriDatasetType.esriDTAny);//得到有多少个要素集合名字
            Dataset_name.Reset();
            IDatasetName Name_set = Dataset_name.Next();
            while (Name_set != null)
            {
                if (Name_set.Name == "Geo_Topo_ice")
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据集已存在,不用创建！");
                    this.Close();//如果要创建的数据集已存在，就返回，并关闭窗体
                    return;
                }
                Name_set = Dataset_name.Next();
            }
            IFeatureDataset pfd = pFeatureWorkspace.CreateFeatureDataset("Geo_Topo_ice", spatial);//创建一个要素集

            IEnumDataset dst = space.get_Datasets(esriDatasetType.esriDTAny);//得到所有的要素类的一个集合
            dst.Reset();
            IDataset det = dst.Next();
            _AppHk.OperatorTips = "开始创建对应的要素类...";
            while (det != null)
            {
                #region 给要素集创建空的要素类
                if (det.Type == esriDatasetType.esriDTFeatureClass)//判定是不是要素类
                {

                    
                    string org_name = det.Name;//原始的名字
                    _AppHk.OperatorTips = "开始创建" + org_name + "要素类...";
                    IFeatureClass f_class = pFeatureWorkspace.OpenFeatureClass(org_name);//打开源要素类
                    det.Rename(org_name + "_t");//把源要素类进行重命名
                    IFields Fieldset = new FieldsClass();//建立一个字段集
                    IFieldsEdit sField = Fieldset as IFieldsEdit;//字段集
                    if (f_class.FeatureType != esriFeatureType.esriFTAnnotation)
                    {
                        //shape
                        IGeometryDefEdit d_edit;//定义一个用来接收要素类的SHAPE类型
                        d_edit = new GeometryDefClass();//实例一个操作类
                        d_edit.GeometryType_2 = f_class.ShapeType;//将源要素类的SHAPE赋值给我们要创建的几何类型
                        d_edit.SpatialReference_2 = spatial;//空间参考

                        string OID = f_class.OIDFieldName;//ODI名字
                        string SHAPE = f_class.ShapeFieldName;//SHAPE名字

                        //IFields Fieldset = new FieldsClass();//建立一个字段集
                        //IFieldsEdit sField = Fieldset as IFieldsEdit;//字段集


                        //创建要素类里的字段
                        int count = f_class.Fields.FieldCount;//确定有多少个字段

                        #region 创建字段
                        for (int n = 0; n < count; n++)
                        {
                            IField f_ield = f_class.Fields.get_Field(n);

                            IFieldEdit fieldEdit = f_ield as IFieldEdit;
                            //Annotate
                            if (f_ield.Name == SHAPE)
                            {
                                //shape field
                                fieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;//确定字段的类型
                                fieldEdit.GeometryDef_2 = d_edit;//把几何类型赋值给它
                                fieldEdit.Name_2 = SHAPE;//把几何类型SHPAE的名字赋值给它
                                f_ield = fieldEdit as IField;
                                sField.AddField(f_ield);//加入要素集
                            }
                            else if (f_ield.Name == OID)
                            {

                                //oid
                                fieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;//OID标识字段
                                fieldEdit.Name_2 = OID;//OID的名字
                                f_ield = fieldEdit as IField;
                                sField.AddField(f_ield);//加入OID

                            }
                            else
                            {
                                //一般字段
                                fieldEdit.Name_2 = f_ield.Name;
                                fieldEdit.Type_2 = f_ield.Type;//字段的类型

                                f_ield = fieldEdit as IField;
                                sField.AddField(f_ield);
                            }

                        }
                        #endregion

                        Fieldset = sField as IFields;//将可编辑的字段集转成一般字段集
                        pfd.CreateFeatureClass(org_name, Fieldset, f_class.CLSID, null, esriFeatureType.esriFTSimple, SHAPE, "");//给要素集中创建要素类
                    }
                    else
                    {
                            
                            createAnnoFeatureClass(org_name, pfd, pfd.Workspace as IFeatureWorkspace, sField, 2000);
                    }

                    det = dst.Next();//重新遍历下一个
                }
                else
                {
                    det = dst.Next();//重新遍历下一个
                }
                #endregion

            }
            _AppHk.OperatorTips = "要素集合创建成功！";
            GetValue(pFeatureWorkspace);//当对应的要素类建立好后，就开始给空要素类赋值
            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据处理完成！");
            this.Close();
            

        }

        /// <summary>
        /// 把值传入新创建的要素集中
        /// </summary>
        private void GetValue(IFeatureWorkspace pFeatureWorkspace)
        {
            IFeatureDataset ds = pFeatureWorkspace.OpenFeatureDataset("Geo_Topo_ice");
            IEnumDataset dd = ds.Subsets;//得到要素数据集下的所有要素类的集合
            dd.Reset();
            IDataset dt = dd.Next();
            if (dt == null) return;
            while (dt != null)
            {
                string des_name = dt.Name;//目标要素类的名字
                string temp = des_name + "_t";//得到对应源的CLASS 
                try
                {
                    _AppHk.OperatorTips = "开始对空的要素集合里的" + des_name + "要素类进行赋值操作...";
                    IFeatureClass org_class = pFeatureWorkspace.OpenFeatureClass(temp);//得到源的要素类
                    IFeatureClass des_class = pFeatureWorkspace.OpenFeatureClass(des_name);//目标要素类

                    #region 确定源不为空进行下列操作
                    if (org_class != null)
                    {

                        IFeatureBuffer pFeaturebuffer = des_class.CreateFeatureBuffer();//创建一个BUFFER
                        IFeatureCursor pCursor = des_class.Insert(true);//建立一个游标

                        IFeatureCursor pCursor_y = org_class.Search(null, false);//游标开始
                        IFeature Feat = pCursor_y.NextFeature();//遍历开始
                        ///遍历赋值
                        #region 遍历赋值
                        while (Feat != null)
                        {

                            int n = Feat.Fields.FieldCount;
                            for (int r = 0; r < n; r++)
                            {
                                //首先判别要素的类型
                                if (Feat.Fields.get_Field(r).Type == esriFieldType.esriFieldTypeGeometry || Feat.Fields.get_Field(r).Editable == false)
                                {
                                    continue;
                                }
                                else
                                {
                                    int index = pFeaturebuffer.Fields.FindField(Feat.Fields.get_Field(r).Name);//将源要素与目标要素字段对应起来，以字段列索引操作
                                    if (index != -1)
                                    {
                                        pFeaturebuffer.set_Value(index, Feat.get_Value(r));//给目标要素设置源要素对应的值
                                    }
                                }

                            }
                            pFeaturebuffer.Shape = Feat.ShapeCopy;//将表的几何属性给目标
                            pCursor.InsertFeature(pFeaturebuffer);

                            Feat = pCursor_y.NextFeature();
                        }
                        #endregion

                        //释放cursor
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor_y);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);

                    }

                    IDataset dataset_temp = org_class as IDataset;//转成DATASET，临时用来删除使用
                    dataset_temp.Delete();//因我们操作的数据是针对数据集，所以可将已COPY的要素类删除
                    #endregion
                }
                catch
                {
                    dt = dd.Next();//继续遍历下一个
                    continue;
                }

                dt = dd.Next();//继续遍历下一个
            }
            _AppHk.OperatorTips = "整个要素集合创建成功！";
        }
        /// <summary>
        /// 创建标注
        /// </summary>
        /// <param name="feaName"></param>
        /// <param name="featureDataset"></param>
        /// <param name="feaworkspace"></param>
        /// <param name="fsEditAnno"></param>
        /// <param name="intScale"></param>
        public void createAnnoFeatureClass(string feaName, IFeatureDataset featureDataset, IFeatureWorkspace feaworkspace, IFieldsEdit fsEditAnno, int intScale)
        {
            //创建注记的特殊字段
            try
            {
                //注记的workSpace
                IFeatureWorkspaceAnno pFWSAnno = feaworkspace as IFeatureWorkspaceAnno;//标注操作空间

                IGraphicsLayerScale pGLS = new GraphicsLayerScaleClass();//图形比例接口
                pGLS.Units = esriUnits.esriMeters;//图形比例设定
                pGLS.ReferenceScale = Convert.ToDouble(intScale);//创建注记必须要设置比例尺

                IFormattedTextSymbol myTextSymbol = new TextSymbolClass();//文本格式接口
                ISymbol pSymbol = (ISymbol)myTextSymbol;//标记
                //Anno要素类必须有的缺省符号
                ISymbolCollection2 pSymbolColl = new SymbolCollectionClass();
                ISymbolIdentifier2 pSymID = new SymbolIdentifierClass();
                pSymbolColl.AddSymbol(pSymbol, "Default", out pSymID);

                //Anno要素类的必要属性
                IAnnotateLayerProperties pAnnoProps = new LabelEngineLayerPropertiesClass();
                pAnnoProps.CreateUnplacedElements = true;
                pAnnoProps.CreateUnplacedElements = true;
                pAnnoProps.DisplayAnnotation = true;
                pAnnoProps.UseOutput = true;

                ILabelEngineLayerProperties pLELayerProps = (ILabelEngineLayerProperties)pAnnoProps;
                pLELayerProps.Symbol = pSymbol as ITextSymbol;
                pLELayerProps.SymbolID = 0;
                pLELayerProps.IsExpressionSimple = true;
                pLELayerProps.Offset = 0;
                pLELayerProps.SymbolID = 0;

                IAnnotationExpressionEngine aAnnoVBScriptEngine = new AnnotationVBScriptEngineClass();
                pLELayerProps.ExpressionParser = aAnnoVBScriptEngine;
                pLELayerProps.Expression = "[DESCRIPTION]";
                IAnnotateLayerTransformationProperties pATP = (IAnnotateLayerTransformationProperties)pAnnoProps;
                pATP.ReferenceScale = pGLS.ReferenceScale;
                pATP.ScaleRatio = 1;

                IAnnotateLayerPropertiesCollection pAnnoPropsColl = new AnnotateLayerPropertiesCollectionClass();
                pAnnoPropsColl.Add(pAnnoProps);

                IObjectClassDescription pOCDesc = new AnnotationFeatureClassDescription();
                IFields fields = pOCDesc.RequiredFields;
                IFeatureClassDescription pFDesc = pOCDesc as IFeatureClassDescription;

                for (int j = 0; j < pOCDesc.RequiredFields.FieldCount; j++)
                {
                    fsEditAnno.AddField(pOCDesc.RequiredFields.get_Field(j));
                }
                fields = fsEditAnno as IFields;
                pFWSAnno.CreateAnnotationClass(feaName, fields, pOCDesc.InstanceCLSID, pOCDesc.ClassExtensionCLSID, pFDesc.ShapeFieldName, "", featureDataset, null, pAnnoPropsColl, pGLS, pSymbolColl, true);
            }
            catch
            {

            }
        }
        private void FrmInitiFeatureDataset_Load(object sender, EventArgs e)
        {

        }
    }
}