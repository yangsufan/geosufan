using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.Xml;
using System.IO;

//2013-1-26 ygc 清空临时库 
namespace GeoDBATool
{
    public partial class FrmDeleteTempData : DevComponents.DotNetBar.Office2007Form
    {
        public FrmDeleteTempData()
        {
            InitializeComponent();
        }
        private  string m_City = "";
        private List<string> listCity;
        private  List<string> listCounty;
        private string m_CountryCode = "";
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmDeleteTempData_Load(object sender, EventArgs e)
        {
            InitializecbCity();
            //初始化工程下拉框
            XmlDocument xml = new XmlDocument();
            if (!File.Exists(ModData.v_projectDetalXML))
            {
                MessageBox.Show("无法获取工程配置文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            xml.Load(ModData.v_projectDetalXML);
            XmlNodeList xmlNodeProList = xml.SelectNodes("//工程");
            for (int i = 0; i < xmlNodeProList.Count; i++)
            {
                cbProName.Items.Add(xmlNodeProList[i].Attributes["名称"].Value.ToString());
            }
            if (cbProName.Items.Count > 0)
            {
                cbProName.SelectedIndex = 0;
            }
        }
        //初始化市级行政区列表
        //ygc 2012-8-21
        private void InitializecbCity()
        {
            listCity = new List<string>();
            IFeatureWorkspace pFW = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
            IWorkspace2 pW2 = Plugin.ModuleCommon.TmpWorkSpace as IWorkspace2;
            if (pFW == null) return;
            if (!pW2.get_NameExists(esriDatasetType.esriDTTable, "行政区字典表")) return;
            ITable pTable = pFW.OpenTable("行政区字典表");

            int ndx = pTable.FindField("NAME"),
             cdx = pTable.FindField("CODE");

            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = "XZJB='" + 2 + "' and substr(code,1,3)<>'149'";

            ICursor pCursor = pTable.Search(pQueryFilter, false);
            if (pCursor == null) return;

            IRow pRow = pCursor.NextRow();

            listCity.Clear();
            while (pRow != null)
            {
                cbShiName.Items.Add(pRow.get_Value(ndx).ToString());
                listCity.Add(pRow.get_Value(cdx).ToString());
                pRow = pCursor.NextRow();
            }
            if (listCity.Count <= 0)
            {
                MessageBox.Show("无市级行政区数据！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            cbShiName.SelectedIndex = 0;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        }

        private void cbShiName_SelectedIndexChanged(object sender, EventArgs e)
        {
            listCounty = new List<string>();
            cbXianName.Items.Clear();
            cbXianName.Text = "";

            int cityIndex = cbShiName.SelectedIndex;
            m_City = listCity[cityIndex];

            IFeatureWorkspace pFW = Plugin.ModuleCommon.TmpWorkSpace as IFeatureWorkspace;
            IWorkspace2 pW2 = Plugin.ModuleCommon.TmpWorkSpace as IWorkspace2;
            if (pFW == null) return;
            if (!pW2.get_NameExists(esriDatasetType.esriDTTable, "行政区字典表")) return;
            ITable pTable = pFW.OpenTable("行政区字典表");

            int ndx = pTable.FindField("NAME"),
            cdx = pTable.FindField("CODE");

            IQueryFilter pQueryFilter = new QueryFilterClass();
            pQueryFilter.WhereClause = "XZJB='" + 3 + "' and substr(code,1,4)='" + m_City + "' and substr(code,1,3)<>'149'";

            ICursor pCursor = pTable.Search(pQueryFilter, false);
            if (pCursor == null) return;

            IRow pRow = pCursor.NextRow();


            while (pRow != null)
            {
                cbXianName.Items.Add(pRow.get_Value(ndx).ToString());
                listCounty.Add(pRow.get_Value(cdx).ToString());
                pRow = pCursor.NextRow();
            }
            if (listCounty.Count <= 0)
            {
                MessageBox.Show("无县级行政区数据！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            cbXianName.SelectedIndex = 0; 
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
        }

        private void cbLayerName_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void cbProName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbProName.SelectedItem.ToString() == "") return;
            XmlDocument xml = new XmlDocument();
            if (!File.Exists(ModData.v_projectDetalXML))
            {
                MessageBox.Show("无法获取工程配置文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            xml.Load(ModData.v_projectDetalXML);
            XmlNode xmlNodeLayer = xml.SelectSingleNode("//工程[@名称='" + cbProName.SelectedItem.ToString() + "']//内容//临时库//数据集//图层名");
            string fcNames = xmlNodeLayer.Attributes["名称"].Value.ToString().Trim();
            string[] FcArr = fcNames.Split(',');
            if (FcArr.Length == 0) return;
            cbLayerName.Items.Clear();
            cbLayerName.Text = "";
            for (int i = 0; i < FcArr.Length; i++)
            {
                cbLayerName.Items.Add(FcArr[i]);
            }
            if (cbLayerName.Items.Count == 0) return;
            cbLayerName.SelectedIndex = 0;
        }

        //清除选中县
        private void btnClear_Click(object sender, EventArgs e)
        {
        
            if(cbXianName .SelectedItem ==null)
            {
                MessageBox .Show ("请选择要清除的县级行政区！","提示",MessageBoxButtons .OK ,MessageBoxIcon.Error);
                return ;
            }
            if(cbLayerName .SelectedItem ==null)
            {
                MessageBox .Show ("请选择要清除的图层！","提示",MessageBoxButtons.OK ,MessageBoxIcon.Error);
                return ;
            }
            int xianIndex=cbXianName .SelectedIndex;
            string CountryCode=listCounty[xianIndex];
            //获取选中的图层信息
             string layerName=cbLayerName .SelectedItem .ToString ();
            //获得临时库图层数据
             string ProName = cbProName.SelectedItem.ToString();
             XmlDocument xmlPro = new XmlDocument();
             xmlPro.Load(ModData.v_projectDetalXML);
            XmlElement pTempConElement = xmlPro.SelectSingleNode("//工程[@名称='" + ProName + "']//内容//临时库//连接信息") as XmlElement;
            IWorkspace LTempWorkspace = ModDBOperator.GetDBInfoByXMLNode(pTempConElement, "") as IWorkspace;
            string TempDatasetName = xmlPro.SelectSingleNode("//工程[@名称='" + ProName + "']//内容//临时库//数据集").Attributes["名称"].Value.ToString();
            List<IFeatureClass> listTempFeatureClass = clsLogicCheck.GetFeatureClass(LTempWorkspace, TempDatasetName);
            IFeatureClass pTempFeatureClass=GetFeatureClassByName(listTempFeatureClass,layerName);
            Exception errorMsg=null ;
            clsInputTempDate.DeleteFeatureClass(pTempFeatureClass, "xian='" + CountryCode + "'", layerName, out errorMsg);
            if(errorMsg !=null )
            {
                MessageBox .Show ("清除临时库图层失败！"+errorMsg .Message .ToString (),"提示",MessageBoxButtons.OK ,MessageBoxIcon.Error);
                return ;
            }
            else 
            {
                MessageBox .Show ("成功清除符合条件的要素！","提示",MessageBoxButtons.OK ,MessageBoxIcon.Information);
            }
        }
        //清除选中图层
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (cbLayerName.SelectedItem == null)
            {
                MessageBox.Show("请选择要清除的图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //获取选中的图层信息
            string layerName = cbLayerName.SelectedItem.ToString();
            //获得临时库图层数据
            string ProName = cbProName.SelectedItem.ToString();
            XmlDocument xmlPro = new XmlDocument();
            xmlPro.Load(ModData.v_projectDetalXML);
            XmlElement pTempConElement = xmlPro.SelectSingleNode("//工程[@名称='" + ProName + "']//内容//临时库//连接信息") as XmlElement;
            IWorkspace LTempWorkspace = ModDBOperator.GetDBInfoByXMLNode(pTempConElement, "") as IWorkspace;
            string TempDatasetName = xmlPro.SelectSingleNode("//工程[@名称='" + ProName + "']//内容//临时库//数据集").Attributes["名称"].Value.ToString();
            List<IFeatureClass> listTempFeatureClass = clsLogicCheck.GetFeatureClass(LTempWorkspace, TempDatasetName);
            IFeatureClass pTempFeatureClass = GetFeatureClassByName(listTempFeatureClass, layerName);
            Exception errorMsg = null;
            clsInputTempDate.DeleteFeatureClass(pTempFeatureClass, "", layerName, out errorMsg);
            if (errorMsg != null)
            {
                MessageBox.Show("清除临时库图层失败！" + errorMsg.Message.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("成功清除临时库图层:" + layerName, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
               //根据名称来获取FeatureClass
        private IFeatureClass GetFeatureClassByName(List<IFeatureClass> listFeatureClass, string FeatureClassName)
        {
            IFeatureClass pFeatureClass = null;
            if (listFeatureClass == null || listFeatureClass.Count == 0)
            {
                return pFeatureClass;
            }
            for (int i = 0; i < listFeatureClass.Count; i++)
            {
                string tempName = GetNewString(listFeatureClass[i].AliasName);
                    if(tempName ==FeatureClassName)
                    {
                        pFeatureClass =listFeatureClass [i];
                        break ;
                    }
            }
              return pFeatureClass;
        }
        
        private string GetNewString(string FeatureClassName)
        {
            if(FeatureClassName .Contains ("."))
            {
                string temp = FeatureClassName.Substring(FeatureClassName.IndexOf(".")+1, FeatureClassName.Length - FeatureClassName.IndexOf(".")-1);
                return temp;
            }
            else 
            {
                return FeatureClassName ;
            }
        }


    }
}
