using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using SysCommon.Authorize;
using SysCommon.Gis;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using System.Collections;

namespace GeoDBATool
{
    /// <summary>
    /// 作者：yjl
    /// 日期：2011.07.28
    /// 说明：元数据地图窗体
    /// </summary>
    public partial class frmMetaMap : DevComponents.DotNetBar.Office2007Form
    {
        private IWorkspace m_WS = null;        //系统维护库连接工作空间
        private IWorkspace dataWS = null;//系统数据库,元数据所在库
        private string MDConnPath = Application.StartupPath + "\\..\\MDxmlData\\MDConn.dat";
        private int m_type;
        List<string> con;//0=type,1=database
     
        public frmMetaMap(IWorkspace inWF)
        {
            InitializeComponent();
            //if (File.Exists(MDConnPath))
            //{
            //    comboBoxDataSource.Enabled = false;
            //    System.Collections.Hashtable conset = conset = new System.Collections.Hashtable();
            //    List<string> strVale = new List<string>();
            //    SysCommon.Authorize.AuthorizeClass.Deserialize(ref conset, MDConnPath);
            //    foreach (DictionaryEntry de in conset)
            //    {
            //        strVale.Add(de.Value.ToString());
            //    }
            //    comboBoxDataSource.Text = strVale[0].ToString();
            //    dataWS = GetWorkSpacefromConninfo(this.comboBoxDataSource.Text, Convert.ToInt32(con[0]));
            //    initAxMapCtrl(dataWS);

            //}
            //con = GetConnData(MDConnPath);
            m_WS = inWF;
            try
            {
                initAxMapCtrl(m_WS);
            }
            catch { }

        }
        //更换数据库时，重新初始化地图控件
        private void initAxMapCtrl(IWorkspace indataWS)
        {
            axMapControlR.ClearLayers();
            IFeatureWorkspace pFW = indataWS as IFeatureWorkspace;
            IFeatureClass pFC = null;
            if ((indataWS as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, "METADATA_MAP"))
            {
                pFC = pFW.OpenFeatureClass("METADATA_MAP");
                IFeatureLayer pFL = new FeatureLayerClass();
                pFL.FeatureClass = pFC;
                pFL.Name = pFC.AliasName;
                IGeoFeatureLayer pGFL = pFL as IGeoFeatureLayer;
                DefineUniqueValueRenderer(pGFL, "数据生产时间");
                //IAnnotationExpressionEngine pAEE = new AnnotationVBScriptEngineClass();
                //pAEE.SetExpression();
                ILabelEngineLayerProperties pLELP = new LabelEngineLayerPropertiesClass();
                pLELP.Expression = "\"数据生产单位：\" & [数据生产单位] & chr(13) & \"数据生产时间：\" & [数据生产时间] & chr(13)";//"[producedate]";
                IAnnotateLayerPropertiesCollection pALPC = pGFL.AnnotationProperties;
                pALPC.Clear();
                pALPC.Add(pLELP as IAnnotateLayerProperties);
                pGFL.DisplayAnnotation = true;

                axMapControlR.Map.AddLayer(pFL);
            }

            if ((indataWS as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, "XZQH_XIAN"))
            {
                pFC = pFW.OpenFeatureClass("XZQH_XIAN");
                IFeatureLayer pFL = new FeatureLayerClass();
                pFL.FeatureClass = pFC;
                pFL.Name = pFC.AliasName;
                ILayerEffects pLE = pFL as ILayerEffects;
                if (pLE.SupportsTransparency)
                    pLE.Transparency = 60;//设置图层透明
                axMapControlR.AddLayer(pFL);

            }
 
        }
        private List<string> GetConnData(string ConntPath)
        {
            List<string> strconn = new List<string>();
            try
            {
                System.Collections.Hashtable conset = conset = new System.Collections.Hashtable();
                List<string> strVale = new List<string>();

                SysCommon.Authorize.AuthorizeClass.Deserialize(ref conset, ConntPath);
                foreach (DictionaryEntry de in conset)
                {
                    strVale.Add(de.Value.ToString());
                }
                int index1 = strVale[0].IndexOf("|");
                int index2 = strVale[0].IndexOf("|", index1 + 1);
                int index3 = strVale[0].IndexOf("|", index2 + 1);
                int index4 = strVale[0].IndexOf("|", index3 + 1);
                int index5 = strVale[0].IndexOf("|", index4 + 1);
                int index6 = strVale[0].IndexOf("|", index5 + 1);
                strconn.Add(strVale[1].ToString());
                switch (strVale[1].ToString())
                {
                    case "1":
                        strconn.Add(strVale[0].Substring(index2 + 1, index3 - index2 - 1));
                        break;
                    case "3":
                        //Server
                        strconn.Add(strVale[0].Substring(0, index1));
                        //Service
                        strconn.Add(strVale[0].Substring(index1 + 1, index2 - index1 - 1));
                        //Database
                        strconn.Add(strVale[0].Substring(index2 + 1, index3 - index2 - 1));
                        //User
                        strconn.Add(strVale[0].Substring(index3 + 1, index4 - index3 - 1));
                        //Password
                        strconn.Add(strVale[0].Substring(index4 + 1, index5 - index4 - 1));
                        //Version
                        strconn.Add(strVale[0].Substring(index5 + 1, index6 - index5 - 1));
                        break;
                }
                return strconn;
            }
            catch
            {
                return strconn = null;
            }

        }

        //根据连接字符串获取工作空间
        //此处连接字符串是固定格式的连接串 Server|Service|Database|User|Password|Version
        private IWorkspace GetWorkSpacefromConninfo(string conninfostr, int type)
        {
            //added by chulili 20111109 添加保护
            if (conninfostr == "")
            {
                return null;
            }
            if (type < 0)
            {
                return null;
            }
            //end added by chulili 20111109
            int index1 = conninfostr.IndexOf("|");
            int index2 = conninfostr.IndexOf("|", index1 + 1);
            int index3 = conninfostr.IndexOf("|", index2 + 1);
            int index4 = conninfostr.IndexOf("|", index3 + 1);
            int index5 = conninfostr.IndexOf("|", index4 + 1);
            int index6 = conninfostr.IndexOf("|", index5 + 1);
            IPropertySet pPropSet = new PropertySetClass();
            IWorkspaceFactory pWSFact = null;
            string sServer = ""; string sService = ""; string sDatabase = "";
            string sUser = ""; string sPassword = ""; string strVersion = "";
            switch (type)
            {
                case 1://mdb
                    pWSFact = new AccessWorkspaceFactoryClass();
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    break;
                case 2://gdb
                    pWSFact = new FileGDBWorkspaceFactoryClass();
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    break;
                case 3://sde
                    pWSFact = new SdeWorkspaceFactoryClass();
                    sServer = conninfostr.Substring(0, index1);
                    sService = conninfostr.Substring(index1 + 1, index2 - index1 - 1);
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    sUser = conninfostr.Substring(index3 + 1, index4 - index3 - 1);
                    sPassword = conninfostr.Substring(index4 + 1, index5 - index4 - 1);
                    strVersion = conninfostr.Substring(index5 + 1, index6 - index5 - 1);
                    break;
            }

            pPropSet.SetProperty("SERVER", sServer);
            pPropSet.SetProperty("INSTANCE", sService);
            pPropSet.SetProperty("DATABASE", sDatabase);
            pPropSet.SetProperty("USER", sUser);
            pPropSet.SetProperty("PASSWORD", sPassword);
            pPropSet.SetProperty("VERSION", strVersion);
            try
            {

                IWorkspace pWorkspace = pWSFact.Open(pPropSet, 0);
                return pWorkspace;
            }
            catch
            {
                return null;
            }
        }

        private void comboBoxDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(MDConnPath))
                {
                    File.Delete(MDConnPath);
                }
                btnSetLabel.Enabled = true;
                SysGisTable sysTable = new SysGisTable(m_WS);
                Exception eError;
                string DataSourceName = this.comboBoxDataSource.Text;
                string conninfostr = sysTable.GetFieldValue("DATABASEMD", "CONNECTIONINFO", "DATABASENAME='" + DataSourceName + "'", out eError).ToString();
                int type = int.Parse(sysTable.GetFieldValue("DATABASEMD", "DATAFORMATID", "DATABASENAME='" + DataSourceName + "'", out eError).ToString());
                int iDataType = int.Parse(sysTable.GetFieldValue("DATABASEMD", "DATABASETYPEID", "DATABASENAME='" + DataSourceName + "'", out eError).ToString());
                string strDBPara = sysTable.GetFieldValue("DATABASEMD", "DBPARA", "DATABASENAME='" + DataSourceName + "'", out eError).ToString();
                string pID = sysTable.GetFieldValue("DATABASEMD", "ID", "DATABASENAME='" + DataSourceName + "'", out eError).ToString();
                int index6 = conninfostr.LastIndexOf("|");
                dataWS = GetWorkSpacefromConninfo(conninfostr, type);
                initAxMapCtrl(dataWS);
                
            }
            catch
            {
            }
            if (axMapControlR.LayerCount == 0)
                btnSetLabel.Enabled = false;//如果空图层，则不允许设置标注
          
        }

        private void comboBoxDataSource_DropDown(object sender, EventArgs e)
        {
            SysGisTable sysTable = new SysGisTable(m_WS);
            //Dictionary<string, object> dicData = new Dictionary<string, object>();
            Exception eError;
            //初始化数据源
            List<object> ListDatasource = sysTable.GetFieldValues("DATABASEMD", "DATABASENAME", "DATAFORMATID=1", out eError);
            this.comboBoxDataSource.Items.Clear();
            foreach (object datasource in ListDatasource)
            {
                this.comboBoxDataSource.Items.Add(datasource.ToString());
            }
        }
        //定义一个唯一值渲染
        private void DefineUniqueValueRenderer(IGeoFeatureLayer pGeoFeatureLayer, string fieldName)
        {

            IRandomColorRamp pRandomColorRamp = new RandomColorRampClass();
            //Make the color ramp for the symbols in the renderer.
            pRandomColorRamp.MinSaturation = 20;
            pRandomColorRamp.MaxSaturation = 40;
            pRandomColorRamp.MinValue = 85;
            pRandomColorRamp.MaxValue = 100;
            pRandomColorRamp.StartHue = 76;
            pRandomColorRamp.EndHue = 188;
            pRandomColorRamp.UseSeed = true;
            pRandomColorRamp.Seed = 43;

            //Make the renderer.
            IUniqueValueRenderer pUniqueValueRenderer = new UniqueValueRendererClass();

            ISimpleFillSymbol pSimpleFillSymbol = new SimpleFillSymbolClass();
            pSimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
            pSimpleFillSymbol.Outline.Width = 0.4;

            //These properties should be set prior to adding values.
            pUniqueValueRenderer.FieldCount = 1;
            pUniqueValueRenderer.set_Field(0, fieldName);
            pUniqueValueRenderer.DefaultSymbol = pSimpleFillSymbol as ISymbol;
            pUniqueValueRenderer.UseDefaultSymbol = true;

            IDisplayTable pDisplayTable = pGeoFeatureLayer as IDisplayTable;
            IFeatureCursor pFeatureCursor = pDisplayTable.SearchDisplayTable(null, false) as
                IFeatureCursor;
            IFeature pFeature = pFeatureCursor.NextFeature();


            bool ValFound;
            int fieldIndex;

            IFields pFields = pFeatureCursor.Fields;
            fieldIndex = pFields.FindField(fieldName);
            while (pFeature != null)
            {
                ISimpleFillSymbol pClassSymbol = new SimpleFillSymbolClass();
                pClassSymbol.Style = esriSimpleFillStyle.esriSFSSolid;
                pClassSymbol.Outline.Width = 0.4;

                string classValue;
                classValue = pFeature.get_Value(fieldIndex).ToString();

                //Test to see if this value was added
                //to the renderer. If not, add it.
                ValFound = false;
                for (int i = 0; i <= pUniqueValueRenderer.ValueCount - 1; i++)
                {
                    if (pUniqueValueRenderer.get_Value(i) == classValue)
                    {
                        ValFound = true;
                        break; //Exit the loop if the value was found.
                    }
                }
                //If the value was not found, it is new and it will be added.
                if (ValFound == false)
                {
                    pUniqueValueRenderer.AddValue(classValue, fieldName, pClassSymbol as
                        ISymbol);
                    pUniqueValueRenderer.set_Label(classValue, classValue);
                    pUniqueValueRenderer.set_Symbol(classValue, pClassSymbol as ISymbol);
                }
                pFeature = pFeatureCursor.NextFeature();
            }
            //Since the number of unique values is known, 
            //the color ramp can be sized and the colors assigned.
            pRandomColorRamp.Size = pUniqueValueRenderer.ValueCount;
            bool bOK;
            pRandomColorRamp.CreateRamp(out bOK);

            IEnumColors pEnumColors = pRandomColorRamp.Colors;
            pEnumColors.Reset();
            for (int j = 0; j <= pUniqueValueRenderer.ValueCount - 1; j++)
            {
                string xv;
                xv = pUniqueValueRenderer.get_Value(j);
                if (xv != "")
                {
                    ISimpleFillSymbol pSimpleFillColor = pUniqueValueRenderer.get_Symbol(xv)
                        as ISimpleFillSymbol;
                    pSimpleFillColor.Color = pEnumColors.Next();
                    pUniqueValueRenderer.set_Symbol(xv, pSimpleFillColor as ISymbol);

                }
            }

            //'** If you didn't use a predefined color ramp
            //'** in a style, use "Custom" here. Otherwise,
            //'** use the name of the color ramp you selected.
            pUniqueValueRenderer.ColorScheme = "Custom";
            ITable pTable = pDisplayTable as ITable;
            bool isString = pTable.Fields.get_Field(fieldIndex).Type ==
                esriFieldType.esriFieldTypeString;
            pUniqueValueRenderer.set_FieldType(0, isString);
            pGeoFeatureLayer.Renderer = pUniqueValueRenderer as IFeatureRenderer;

            //This makes the layer properties symbology tab
            //show the correct interface.
            IUID pUID = new UIDClass();
            pUID.Value = "{683C994E-A17B-11D1-8816-080009EC732A}";
            pGeoFeatureLayer.RendererPropertyPageClassID = pUID as UIDClass;

        }

        private void frmMetaMap_Load(object sender, EventArgs e)
        {
           
        }

        private void btnSetLabel_Click(object sender, EventArgs e)
        {
            IList<string> fdLst=new List<string>();
            IFeatureClass pFC = null;
            IGeoFeatureLayer pGFL = null;
            for (int i = 0; i < axMapControlR.LayerCount; i++)
            {
                if (axMapControlR.get_Layer(i).Name == "元数据地图")
                {
                    pFC = (axMapControlR.get_Layer(i) as IFeatureLayer).FeatureClass;
                    pGFL = axMapControlR.get_Layer(i) as IGeoFeatureLayer;
                }
            }
            if (pFC == null)
                return;
            for (int i = 0; i < pFC.Fields.FieldCount; i++)
            {
                IField pField = pFC.Fields.get_Field(i);
                fdLst.Add(pField.Name);

            }
            frmMetaMapSetLabel fmMMSL = new frmMetaMapSetLabel(fdLst);
            if (fmMMSL.ShowDialog(this) == DialogResult.OK && axMapControlR.LayerCount>0)
            {
               
                ILabelEngineLayerProperties pLELP = new LabelEngineLayerPropertiesClass();
                pLELP.Expression = fmMMSL.LabelExpression;
                IAnnotateLayerPropertiesCollection pALPC = pGFL.AnnotationProperties;
                pALPC.Clear();
                pALPC.Add(pLELP as IAnnotateLayerProperties);
                pGFL.DisplayAnnotation = true;
                axMapControlR.Refresh();
 
            }
        }

        private void axMapControlR_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {

        }


    }
}
