using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
//ygc 2013-01-24  临时库导入成功库

namespace GeoDBATool
{
    public partial class FrmInputTempToProduction : DevComponents.DotNetBar.Office2007Form
    {
        public FrmInputTempToProduction()
        {
            InitializeComponent();
        }

 
        private void FrmInputTempToProduction_Load(object sender, EventArgs e)
        {
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
                cbProNameList.Items.Add(xmlNodeProList[i].Attributes["名称"].Value.ToString());
            }
            if (cbProNameList.Items.Count > 0)
            {
                cbProNameList.SelectedIndex = 0;
            }
        }

        private void btnInputTempData_Click(object sender, EventArgs e)
        {
            if (cbProNameList.SelectedItem == null)
            {
                MessageBox.Show("请选择要入库的工程！","提示",MessageBoxButtons.OK ,MessageBoxIcon.Error);
                return;
            }
            #region 获取库中要素
            string ProName = cbProNameList.SelectedItem.ToString();
            //通过配置文件获取成果库图层集合
            XmlDocument xmlPro = new XmlDocument();
            xmlPro.Load(ModData.v_projectDetalXML);
            XmlElement xmlCHGConElement = xmlPro.SelectSingleNode("//工程[@名称='" + ProName + "']//内容//现势库//连接信息") as XmlElement;
            IWorkspace pCHGWorkspace = ModDBOperator.GetDBInfoByXMLNode(xmlCHGConElement, "") as IWorkspace;
            //获得成果库数据集名称
            string CHGDatasetName = xmlPro.SelectSingleNode("//工程[@名称='" + ProName + "']//内容//现势库//数据集").Attributes["名称"].Value .ToString ();
            //获得成果库图层数据
            List<IFeatureClass> listCHGFeatureClass = clsLogicCheck.GetFeatureClass(pCHGWorkspace, CHGDatasetName);

            //获得历史库图层数据
            XmlElement pLSConElement=xmlPro.SelectSingleNode("//工程[@名称='" + ProName + "']//内容//历史库//连接信息") as XmlElement;
            IWorkspace pLSWorkspace=ModDBOperator.GetDBInfoByXMLNode(pLSConElement, "") as IWorkspace;
            string LSDatasetName = xmlPro.SelectSingleNode("//工程[@名称='" + ProName + "']//内容//历史库//数据集").Attributes["名称"].Value.ToString();
            List<IFeatureClass> listLSFeatureClass = clsLogicCheck.GetFeatureClass(pLSWorkspace, LSDatasetName);

            //获得临时库图层数据
            XmlElement pTempConElement = xmlPro.SelectSingleNode("//工程[@名称='" + ProName + "']//内容//临时库//连接信息") as XmlElement;
            IWorkspace LTempWorkspace = ModDBOperator.GetDBInfoByXMLNode(pTempConElement, "") as IWorkspace;
            string TempDatasetName = xmlPro.SelectSingleNode("//工程[@名称='" + ProName + "']//内容//临时库//数据集").Attributes["名称"].Value.ToString();
            List<IFeatureClass> listTempFeatureClass = clsLogicCheck.GetFeatureClass(LTempWorkspace, TempDatasetName);

            if (listCHGFeatureClass.Count == 0 )
            {
                MessageBox.Show("无法获取成果库图层信息!","提示",MessageBoxButtons.OK ,MessageBoxIcon.Error);
                return;
            }
            #endregion
            Exception ErrorMSG;
            prbInputTempData.Maximum = listCHGFeatureClass.Count;
            prbInputTempData.Minimum = 0;
            prbInputTempData.Step = 1;
            prbInputTempData.Value = 0;
            //进行入库操作
            for (int i = 0; i < listCHGFeatureClass.Count; i++)
            {
                lblState.Text = "正在导入图层" + listCHGFeatureClass[i].AliasName;
                prbInputTempData.Value = i;
                //将成果库数据导入到历史库中
                IFeatureClass pCHGFeatureClass = listCHGFeatureClass[i];
                string newFeatureName = GetNewString(pCHGFeatureClass.AliasName );
                IFeatureClass pLSFeatureClass = GetFeatureClassByName(listLSFeatureClass, newFeatureName + "_GOH");
                clsInputTempDate.CopySourceFeatureClass(pCHGFeatureClass, pLSFeatureClass, null, "", out ErrorMSG);
               if (ErrorMSG!=null)
               {
                   MessageBox.Show("写入历史库出错！","提示",MessageBoxButtons .OK ,MessageBoxIcon.Error);
                   return;
               }
                //将成果库中数据删除
               clsInputTempDate.DeleteFeatureClass(pCHGFeatureClass, "", pCHGFeatureClass.AliasName, out ErrorMSG);
                if (ErrorMSG != null)
                {
                    MessageBox.Show("删除成果库出错！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //将临时库中数据导入到成果库中
                IFeatureClass pTempFeatureClass = GetFeatureClassByName(listTempFeatureClass, newFeatureName + "_GOT");
                clsInputTempDate.CopySourceFeatureClass(pTempFeatureClass, pCHGFeatureClass, null, "", out ErrorMSG);
                if (ErrorMSG!=null)
                {
                    MessageBox.Show("写入成果库出错！","提示",MessageBoxButtons.OK ,MessageBoxIcon.Error);
                    return;
                }
                //将临时库中数据删除
                clsInputTempDate.DeleteFeatureClass(pTempFeatureClass, "",pTempFeatureClass.AliasName , out ErrorMSG);
                if (ErrorMSG != null)
                {
                    MessageBox.Show("删除临时库出错！","提示",MessageBoxButtons .OK ,MessageBoxIcon.Error );
                    return;
                }
            }
            MessageBox.Show("成功导入临时库！","提示",MessageBoxButtons .OK ,MessageBoxIcon.Information);
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
