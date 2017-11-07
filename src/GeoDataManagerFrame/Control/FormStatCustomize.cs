using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using GeoDataCenterFunLib;
namespace GeoDataManagerFrame
{
    public partial class FormStatCustomize : DevComponents.DotNetBar.Office2007Form
    {
        //private string m_connstr;
        //private string m_FeaClassName;
        private IFeatureLayer _FeatureLayer;
        private IMap _Map;
        private Dictionary<int, ILayer> _LayerDic = new Dictionary<int, ILayer>();//added by chulili 20110709由于现在有图层组，查找图层不方便，现在使用字典存储图层，key值是在下拉框中的索引号

        public FormStatCustomize()
        {
            InitializeComponent();
        }
        public void InitForm(IMap pMap,IFeatureLayer pFeaLayer)
        {
            
            _Map = pMap;
            _FeatureLayer = pFeaLayer;
            if (pMap == null)
                return;
            _LayerDic.Clear();
            int iLayerCount = pMap.LayerCount;
            string layername = "";
            this.comboBoxLayer.Items.Clear();
            for (int ii = 0; ii < iLayerCount; ii++)
            {
                ILayer pLayer = pMap.get_Layer(ii);
                IGroupLayer pGroupLayer = pLayer as IGroupLayer;
                //若该图层是图层组
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer comlayer = pLayer as ICompositeLayer;
                    for (int j = 0; j < comlayer.Count; j++)
                    {
                        ILayer pTmpLayer = comlayer.get_Layer(j);
                        if (pTmpLayer is IFeatureLayer)
                        {
                            layername = pTmpLayer.Name;
                            this.comboBoxLayer.Items.Add(layername);
                            _LayerDic.Add(this.comboBoxLayer.Items.Count - 1, pTmpLayer);
                        }
                    }

                }
                //若该图层是单独图层
                if (pLayer is IFeatureLayer)
                {
                    layername = pLayer.Name;
                    this.comboBoxLayer.Items.Add(layername);
                    _LayerDic.Add(this.comboBoxLayer.Items.Count - 1, pLayer);
                }
            }
            this.comboBoxLayer.Text = _FeatureLayer.Name;
        }
        public void InitForm(IMap pMap)
        {
            
            _Map = pMap;
            if (pMap == null)
                return;
            _LayerDic.Clear();
            int iLayerCount = pMap.LayerCount;
            string layername = "";
            this.comboBoxLayer.Items.Clear();
            for (int ii = 0; ii < iLayerCount; ii++)
            {
                ILayer pLayer = pMap.get_Layer(ii);
                IGroupLayer pGroupLayer = pLayer as IGroupLayer;
                //若该图层是图层组
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer comlayer=pLayer as ICompositeLayer;
                    for (int j = 0; j < comlayer.Count; j++)
                    {
                        ILayer pTmpLayer = comlayer.get_Layer(j);
                        if (pTmpLayer is IFeatureLayer)
                        {
                            layername = pTmpLayer.Name;
                            this.comboBoxLayer.Items.Add(layername);
                            _LayerDic.Add(this.comboBoxLayer.Items.Count -1,pTmpLayer);
                        }
                    }
 
                }
                //若该图层是单独图层
                if (pLayer is IFeatureLayer )
                {
                    layername = pLayer.Name;
                    this.comboBoxLayer.Items.Add(layername);
                    _LayerDic.Add(this.comboBoxLayer.Items.Count-1,pLayer );
                }
            }
        }
        //public Boolean InitForm(IFeatureLayer pFeatureLayer)
        //{
        //    if (pFeatureLayer == null) return false;
        //    IFeatureClass pFeaClass = pFeatureLayer.FeatureClass;
        //    if (pFeaClass == null) return false;
        //    m_FeatureLayer = pFeatureLayer;
        //    this.comboBoxLayer.Text = pFeatureLayer.Name;
        //    IFields pFields = pFeaClass.Fields as IFields;
        //    for (int i = 0; i < pFields.FieldCount; i++)
        //    {
        //        //this.comboBoxExGroupF.Items.Add(pFields.get_Field(i).AliasName );
        //        this.comboBoxExSumF.Items.Add(pFields.get_Field(i).AliasName );
        //    }
        //    return true;
        //}
        private void buttonXStatic_Click(object sender, EventArgs e)
        {
            //判断分组字段是否设置
            if (this.listSelectColumns.Items.Count ==0)
            {
                MessageBox.Show("请选择分组字段!","提示",MessageBoxButtons.OK,MessageBoxIcon.Information );
                return;
            }
            //判断汇总字段是否设置
            if (this.comboBoxExSumF.Text.Equals(""))
            {
                MessageBox.Show("请选择汇总字段!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //初始化进度条
            IFeatureClass pFeatureClass = _FeatureLayer.FeatureClass;
            if (pFeatureClass.FeatureCount(null) == 0)
            {
                MessageBox.Show("图层中没有可供统计的数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IFields pFields = pFeatureClass.Fields;
            int iFieldsindex = 0;

            iFieldsindex = pFields.FindFieldByAliasName(this.comboBoxExSumF.Text);
            string sSumField = pFields.get_Field(iFieldsindex).Name;
            IField pSumField = pFields.get_Field(iFieldsindex);
            esriFieldType pType = pFields.get_Field(iFieldsindex).Type;
            if (pType != esriFieldType.esriFieldTypeDouble && pType != esriFieldType.esriFieldTypeInteger && pType != esriFieldType.esriFieldTypeSingle && pType != esriFieldType.esriFieldTypeSmallInteger)
            {
                MessageBox.Show("汇总字段须是数值型!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            List<IField> listGroupbyFields = new List<IField>();
            for (int i = 0; i < listSelectColumns.Items.Count; i++)
            {
                string strFieldname = listSelectColumns.Items[i] as string ;
                int tmpFieldindex = pFields.FindFieldByAliasName(strFieldname );
                IField   pField = pFields.get_Field(tmpFieldindex);
                if (strFieldname.Equals(this.comboBoxExSumF.Text))
                {
                    continue;
                }
                listGroupbyFields.Add(pField);
            }
            if (listGroupbyFields.Count == 0)
            {
                MessageBox.Show("请选择有效地分组字段!","提示",MessageBoxButtons.OK,MessageBoxIcon.Information );
                return;
            }
            this.Hide();
            
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
            vProgress.SetProgress("创建临时成果数据库");
                try
                {
                string workpath = Application.StartupPath + "\\..\\Temp";
                string workSpaceName = Application.StartupPath + "\\..\\Temp\\TmpStatistic.mdb";
                if (File.Exists(workSpaceName))
                {
                    File.Delete(workSpaceName);
                }
                //判断结果目录是否存在，不存在则创建
                if (System.IO.Directory.Exists(workpath) == false)
                    System.IO.Directory.CreateDirectory(workpath);
                //创建一个新的mdb数据库,并打开工作空间
                IWorkspace pOutWorkSpace = ChangeJudge.CreatePDBWorkSpace(workpath, "TmpStatistic.mdb");
                string sFeatureClassName = "";
                vProgress.SetProgress("分析目标图层数据");
                if (_FeatureLayer != null)
                {
                    IDataset pDataSet = _FeatureLayer.FeatureClass as IDataset;
                    IWorkspace pWorkSpace = pDataSet.Workspace;
                    sFeatureClassName = pDataSet.Name;
                    CopyPasteGDBData.CopyPasteGeodatabaseData(pWorkSpace, pOutWorkSpace, sFeatureClassName, esriDatasetType.esriDTFeatureClass);
                }
                pOutWorkSpace = null;
                if (sFeatureClassName.Contains("."))
                {
                    sFeatureClassName = sFeatureClassName.Substring(sFeatureClassName.IndexOf(".") + 1);
                }
                ModStatReport.DoCustomizeStat(workSpaceName, _FeatureLayer.Name, sFeatureClassName, listGroupbyFields, pSumField as Field, vProgress);
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                vProgress.Close();
            }
        }

        private void buttonXCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void listViewSelectColumns_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //所选图层改变
        private void comboBoxLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.comboBoxLayer.Text.Equals(""))
            {
                IFeatureLayer ptmpLayer = null;
                int layerindex = this.comboBoxLayer.SelectedIndex;
                //changed by chulili 20110709用字典存储图层
                if (_LayerDic.ContainsKey(layerindex))
                {
                    ptmpLayer = _LayerDic[layerindex] as IFeatureLayer ;

                }
                //change end
                //找到图层
                _FeatureLayer = ptmpLayer;
            }
            if (_FeatureLayer == null) return;
            IFeatureClass pFeaClass = _FeatureLayer.FeatureClass;
            if (pFeaClass == null) return;
            IFields pFields = pFeaClass.Fields as IFields;
            //重新初始化可选分组字段  已选分组字段列表
            listColumns.Items.Clear();
            listSelectColumns.Items.Clear();
            comboBoxExSumF.Items.Clear();
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                //汇总字段过滤，仅列举数值型字段
                esriFieldType pType = pFields.get_Field(i).Type;
                if (pType == esriFieldType.esriFieldTypeDouble || pType == esriFieldType.esriFieldTypeInteger || pType == esriFieldType.esriFieldTypeSingle || pType == esriFieldType.esriFieldTypeSmallInteger)
                {
                    comboBoxExSumF.Items.Add(pFields.get_Field(i).AliasName);
                }                
                
                listColumns.Items.Add(pFields.get_Field(i).AliasName);
            }
        }
        //添加可选字段
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (listColumns.SelectedItems.Count == 0)
            {
                MessageBox.Show("没有选中的可用字段!", "提示");
                return;
            }
            for (int i = 0; i < listColumns.SelectedItems.Count;i++ )
            {
                string strFieldname = listColumns.SelectedItems[i] as string ;
                if (!listSelectColumns.Items.Contains(strFieldname))
                {
                    listSelectColumns.Items.Add(strFieldname);
                }
            }
        }
        //删除分组字段
        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (listSelectColumns.SelectedItems.Count == 0)
            {
                //MessageBox.Show("没有选中的分组字段!", "提示");
                return;
            }
            //倒序删除，以防出错
            int cnt = listSelectColumns.SelectedItems.Count;
            for (int i = cnt; i > 0;i-- )
            {
                int Fieldindex = listSelectColumns.SelectedIndices[i-1];
                listSelectColumns.Items.RemoveAt(Fieldindex);
            }
        }
        //左栏双击添加
        private void listColumns_DoubleClick(object sender, EventArgs e)
        {
            buttonAdd_Click(sender,e);
        }
        //右栏双击删除
        private void listSelectColumns_DoubleClick(object sender, EventArgs e)
        {
            buttonDel_Click(sender,e);
        }
    }
}