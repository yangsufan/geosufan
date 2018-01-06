using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Plugin;

using System.IO;
using ESRI.ArcGIS.Geodatabase;
using SysCommon.Gis;
using ESRI.ArcGIS.esriSystem;
//using GeoGISBase;

namespace GeoSysSetting.SubControl
{
    public partial class UCSysSetting : UserControl
    {
        private IWorkspace _TmpWorkSpace = null;    //业务库工作空间
        private ListViewItem _CurListItem = null;   //当前编辑行
        private bool _isAdd = false;    //是添加参数还是修改参数
        public UCSysSetting(IWorkspace pTmpWorkSpace)
        {
            InitializeComponent();
            _TmpWorkSpace = pTmpWorkSpace;
            
        }

        private void UCSysSetting_Load(object sender, EventArgs e)
        {
            //数据类型下拉框
            comboBoxDataType.Items.Add("");
            comboBoxDataType.Items.Add("String");
            comboBoxDataType.Items.Add("Double");
            comboBoxDataType.Items.Add("Integer");
            comboBoxDataType.Items.Add("XmlFile");
            comboBoxDataType.Items.Add("MxdFile");
            //最初 不可导入、导出参数
            buttonXImport.Enabled = false;
            buttonXExport.Enabled = false;
            ///Zq 20111118  add
            buttonModify.Enabled = false;
            ///end
            SysGisTable mSystable = new SysCommon.Gis.SysGisTable(_TmpWorkSpace);
            Exception err = null;
            //打开系统参数表
            List<Dictionary<string, object>> listdic = mSystable.GetRows("SYSSETTING", "", out err);
            if (listdic != null)
            {
                for (int i = 0; i < listdic.Count; i++)
                {
                    Dictionary <string,object> pDic=listdic[i];
                    ListViewItem pListViewItem = new ListViewItem();
                    //参数名
                    if (pDic.ContainsKey("SETTINGNAME"))
                    {
                        if (pDic["SETTINGNAME"] != null)
                        {
                            pListViewItem.SubItems[0].Text=pDic["SETTINGNAME"].ToString();//比较特殊，默认已经有一个子项
                        }
                        //else
                        //{
                        //    pListViewItem.SubItems.Add("");
                        //}
                    }
                    //else
                    //{ pListViewItem.SubItems.Add(""); }
                    //参数值
                    if (pDic.ContainsKey("SETTINGVALUE"))
                    {
                        if (pDic["SETTINGVALUE"] != null)
                        {
                            pListViewItem.SubItems.Add(pDic["SETTINGVALUE"].ToString());
                        }
                        else
                        {
                            pListViewItem.SubItems.Add("");
                        }
                    }
                    else
                    { pListViewItem.SubItems.Add(""); }
                    //参数数据类型
                    if (pDic.ContainsKey("DATATYPE"))
                    {
                        if (pDic["DATATYPE"] != null)
                        {
                            pListViewItem.SubItems.Add(pDic["DATATYPE"].ToString());
                        }
                        else
                        {
                            pListViewItem.SubItems.Add("");
                        }
                    }
                    else
                    { pListViewItem.SubItems.Add(""); }
                    //参数描述
                    if (pDic.ContainsKey("DESCRIPTION"))
                    {
                        if (pDic["DESCRIPTION"] != null)
                        {
                            pListViewItem.SubItems.Add(pDic["DESCRIPTION"].ToString());
                        }
                        else
                        {
                            pListViewItem.SubItems.Add("");
                        }
                    }
                    else
                    { pListViewItem.SubItems.Add(""); }
                    //如果BLOB类型的字段有值
                    if (pDic.ContainsKey("SETTINGVALUE2"))
                    {
                        if (pDic["SETTINGVALUE2"] != null)
                        {
                            if (pListViewItem.SubItems[2].Text.Contains("File"))
                            {
                                pListViewItem.SubItems[1].Text ="*";
                            }
                        }
                    }
                    listView.Items.Add(pListViewItem);
                }
            }
            //判断导入、导出参数按钮的可用状态
            if (comboBoxDataType.Text.Contains("File"))
            {
                buttonXImport.Enabled = true;
                buttonXExport.Enabled = true;
            }
            else
            {
                buttonXImport.Enabled = false;
                buttonXExport.Enabled = false;
            }
            mSystable = null;
        }

        private void UCSysSetting_Resize(object sender, EventArgs e)
        {
            this.splitContainer1.SplitterDistance = this.splitContainer1.Width / 5;
            listView.Columns[0].Width = listView.Width / 4;
            listView.Columns[1].Width = listView.Width / 4;
            listView.Columns[2].Width = listView.Width / 4;
            listView.Columns[3].Width = listView.Width / 4;
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
                return;
            
            _isAdd = false;//选中了参数中的某一行，那就不是添加新参数状态
            ListViewItem pItem = listView.SelectedItems[0];
            _CurListItem = pItem;
            if (pItem != null)
            {
                if (pItem.SubItems[0] != null)  //参数名称
                {
                    this.textBoxSettingName.Text  = pItem.SubItems[0].Text;
                    textBoxSettingName.Enabled = false;
                }
                if (pItem.SubItems[1] != null)  //参数值
                {
                    this.textBoxSettingValue.Text  = pItem.SubItems[1].Text;
                }
                if (pItem.SubItems[2] != null)  //参数类型
                {
                    string strType = pItem.SubItems[2].Text;
                    for (int i = 0; i < comboBoxDataType.Items.Count; i++)
                    {
                        if (strType == comboBoxDataType.Items[i].ToString())
                        {
                            comboBoxDataType.SelectedIndex = i;
                        }
                    }
                }
                if (pItem.SubItems[3] != null)//参数描述
                {
                    this.textBoxSettingDescription.Text  = pItem.SubItems[3].Text;
                }
            }
        }
        //更新系统参数表
        private void buttonXOK_Click(object sender, EventArgs e)
        {
            if (textBoxSettingName.Text.Trim() == "")
            {
                MessageBox.Show("请输入参数名");
                return;
            }
            SysGisTable mSystable = new SysCommon.Gis.SysGisTable(_TmpWorkSpace);
            Dictionary<string, object> pDic = new Dictionary<string, object>();
            //参数名
            pDic.Add("SETTINGNAME", textBoxSettingName.Text  );
            //参数数据类型
            pDic.Add("DATATYPE", comboBoxDataType.Text );
            //参数描述
            pDic.Add("DESCRIPTION", textBoxSettingDescription.Text );
            //参数值（分简单参数值和文件型参数值）
            if (!comboBoxDataType.Text.Contains("File"))
            {
                pDic.Add("SETTINGVALUE", textBoxSettingValue.Text);
            }
            else
            {
                if (File.Exists(textBoxSettingValue.Text))
                {
                    IMemoryBlobStream pBlobStream = new MemoryBlobStreamClass();

                    pBlobStream.LoadFromFile(textBoxSettingValue.Text);
                    pDic.Add("SETTINGVALUE2", pBlobStream);
                }
            }
            Exception err = null;
            bool bRes = false;
            if (!_isAdd)
            {
                bRes = mSystable.UpdateRow("SYSSETTING", "SETTINGNAME='" +this.textBoxSettingName.Text + "'", pDic, out err);
            }
            else
            {
                bRes = mSystable.NewRow("SYSSETTING", pDic, out err);
            }
            if (!bRes)
            {
                MessageBox.Show(err.Message);
                mSystable = null;
                return;
            }
            if (!_isAdd)    //修改参数
            {
                _CurListItem.SubItems[2].Text = comboBoxDataType.Text;
                _CurListItem.SubItems[3].Text = textBoxSettingDescription.Text;
                if (!comboBoxDataType.Text.Contains("File"))
                {
                    _CurListItem.SubItems[1].Text = textBoxSettingValue.Text;
                }
            }
            else    //添加参数
            {
                ListViewItem pItem = new ListViewItem();
                pItem.SubItems[0].Text = textBoxSettingName.Text;
                pItem.SubItems.Add(""); pItem.SubItems.Add(""); pItem.SubItems.Add("");

                pItem.SubItems[2].Text = comboBoxDataType.Text;                
                pItem.SubItems[3].Text = textBoxSettingDescription.Text;
                if (!comboBoxDataType.Text.Contains("File"))
                {
                    pItem.SubItems[1].Text = textBoxSettingValue.Text;
                }
                listView.Items.Add(pItem);
                _CurListItem = pItem;
            }
            _isAdd = false; //添加完以后，添加状态消失
            mSystable = null;
        }
        //导入文件作为当前系统参数的值
        private void buttonXImport_Click(object sender, EventArgs e)
        {
            if(!comboBoxDataType.Text.Contains("File"))
                return;
            OpenFileDialog pOpenFileDlg = new OpenFileDialog();
            pOpenFileDlg.Title = "选择文件";
            switch(comboBoxDataType.Text)   //目前仅支持两种，可以扩展
            {
                case "XmlFile":
                    //弹出对话框供用户选择导入的xml文件          
                    
                    pOpenFileDlg.Filter = "XML数据(*.xml)|*.xml";
                    break;
                case "MxdFile":
                    pOpenFileDlg.Filter = "mxd文件(*.mxd)|*.mxd";
                    break;
            }
            if (pOpenFileDlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string strPath = pOpenFileDlg.FileName;
            textBoxSettingValue.Text = strPath;
            pOpenFileDlg = null;
           // textBoxSettingValue.Text =       ;
            //ygc 2012-12-17  将文件写如数据库
          //  ModuleDatabase .UpdateRow 

        }
        //当初当前系统参数的值，放在指定路径（限定该参数值是BLOB类型，并且存入时是选取文件存入的）
        private void buttonXExport_Click(object sender, EventArgs e)
        {
            if (!comboBoxDataType.Text.Contains("File"))
                return;
            SaveFileDialog pOpenFileDlg = new SaveFileDialog();
            pOpenFileDlg.Title = "选择文件";
            switch (comboBoxDataType.Text)
            {
                case "XmlFile":
                    //弹出对话框供用户设置导出的xml文件          

                    pOpenFileDlg.Filter = "XML数据(*.xml)|*.xml";
                    break;
                case "MxdFile":
                    pOpenFileDlg.Filter = "mxd文件(*.mxd)|*.mxd";
                    break;
            }
            if (pOpenFileDlg.ShowDialog() == DialogResult.OK)
            {   //获得系统参数表的BLOB字段值，并导出到指定文件中
                string strPath = pOpenFileDlg.FileName;
                SysGisTable mSystable = new SysCommon.Gis.SysGisTable(_TmpWorkSpace);
                Exception err=null;
                Dictionary<string,object> pDic=mSystable.GetRow("SYSSETTING","SETTINGNAME='"+this.textBoxSettingName.Text +"'",out err);
                if (pDic != null)
                {
                    if (pDic.ContainsKey("SETTINGVALUE2"))
                    {
                        if (pDic["SETTINGVALUE2"] != null)  //这里仅能成功导出当初以文件类型导入的BLOB字段 
                        {
                            object tempObj = pDic["SETTINGVALUE2"];
                            IMemoryBlobStreamVariant pMemoryBlobStreamVariant = tempObj as IMemoryBlobStreamVariant;
                            IMemoryBlobStream pMemoryBlobStream = pMemoryBlobStreamVariant as IMemoryBlobStream;
                            if (pMemoryBlobStream != null)
                            {
                                pMemoryBlobStream.SaveToFile(strPath);
                                MessageBox.Show("将文件" + strPath+"导出成功", "提示",MessageBoxButtons .OK,MessageBoxIcon.Information);//ygc 2012-12-17 新增提示信息
                            }
                        }
                    }
                }
                mSystable = null;
                
            }
        }
        //点击添加参数按钮
        private void buttonXAdd_Click(object sender, EventArgs e)
        {
            _isAdd = true;
            _CurListItem = null;    //当前修改行为空
            textBoxSettingName.Enabled = true;
            textBoxSettingName.Text = "";
            textBoxSettingValue.Text = "";
            comboBoxDataType.SelectedIndex = 0;
            textBoxSettingDescription.Text = "";
        }

        private void comboBoxDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ///ZQ  20111118 add
            switch(comboBoxDataType.SelectedItem.ToString())
            {
                case "XmlFile":
                    buttonModify.Enabled =true;
                    break;
                default:
                    buttonModify.Enabled =false;
                    break;
            }
            ///end
            if (comboBoxDataType.Text.Contains("File"))//文件类型，不允许用户编辑参数值
            {
                buttonXImport.Enabled = true;
                buttonXExport.Enabled = true;
                textBoxSettingValue.Enabled = false;
            }
            else //非文件类型，不允许用户编辑参数值
            {
                buttonXImport.Enabled = false;
                buttonXExport.Enabled = false;
                textBoxSettingValue.Enabled = true;
            }
            switch (textBoxSettingName.Text)
            {
                case "X最小值":
                case "X最大值":
                case "Y最小值":
                case "Y最大值":
                    buttonModify.Enabled = true;
                    break;
            }
        }

        private void buttonModify_Click(object sender, EventArgs e)
        {
            switch (textBoxSettingName.Text)
            {
                case "查询配置":
                    if (this.comboBoxDataType.Text.Contains("XmlFile"))
                    {
                        FrmQueryConfig pFrm = new FrmQueryConfig(_TmpWorkSpace, "查询配置");
                        pFrm.ShowDialog();
                    }
                    break;
                case "最大林斑号":
                    if (this.comboBoxDataType.Text.Contains("XmlFile"))
                    {
                        FrmQueryConfig pFrm = new FrmQueryConfig(_TmpWorkSpace, "最大林斑号");
                        pFrm.ShowDialog();
                    }
                    break;
                case"统计配置":
                    if (this.comboBoxDataType.Text.Contains("XmlFile"))
                    {
                        FrmStatisticsConfig pFrmStatisticsConfig = new FrmStatisticsConfig(_TmpWorkSpace);
                        pFrmStatisticsConfig.ShowDialog();
                    }
                    break;
                case "X最小值":
                case "X最大值":
                case "Y最小值":
                case "Y最大值":
                    FormFullMapConfig pFrmFullMapConfig = new FormFullMapConfig(_TmpWorkSpace);
                    DialogResult pR= pFrmFullMapConfig.ShowDialog();
                    if (pR == DialogResult.OK)
                    {
                        RefreshDataView();
                    }
                    break;


            }
        }
        private void RefreshDataView()
        {
            listView.Items.Clear();
            SysGisTable mSystable = new SysCommon.Gis.SysGisTable(_TmpWorkSpace);
            Exception err = null;
            //打开系统参数表
            List<Dictionary<string, object>> listdic = mSystable.GetRows("SYSSETTING", "", out err);
            if (listdic != null)
            {
                for (int i = 0; i < listdic.Count; i++)
                {
                    Dictionary<string, object> pDic = listdic[i];
                    ListViewItem pListViewItem = new ListViewItem();
                    //参数名
                    if (pDic.ContainsKey("SETTINGNAME"))
                    {
                        if (pDic["SETTINGNAME"] != null)
                        {
                            pListViewItem.SubItems[0].Text = pDic["SETTINGNAME"].ToString();//比较特殊，默认已经有一个子项
                        }
                        //else
                        //{
                        //    pListViewItem.SubItems.Add("");
                        //}
                    }
                    //else
                    //{ pListViewItem.SubItems.Add(""); }
                    //参数值
                    if (pDic.ContainsKey("SETTINGVALUE"))
                    {
                        if (pDic["SETTINGVALUE"] != null)
                        {
                            pListViewItem.SubItems.Add(pDic["SETTINGVALUE"].ToString());
                        }
                        else
                        {
                            pListViewItem.SubItems.Add("");
                        }
                    }
                    else
                    { pListViewItem.SubItems.Add(""); }
                    //参数数据类型
                    if (pDic.ContainsKey("DATATYPE"))
                    {
                        if (pDic["DATATYPE"] != null)
                        {
                            pListViewItem.SubItems.Add(pDic["DATATYPE"].ToString());
                        }
                        else
                        {
                            pListViewItem.SubItems.Add("");
                        }
                    }
                    else
                    { pListViewItem.SubItems.Add(""); }
                    //参数描述
                    if (pDic.ContainsKey("DESCRIPTION"))
                    {
                        if (pDic["DESCRIPTION"] != null)
                        {
                            pListViewItem.SubItems.Add(pDic["DESCRIPTION"].ToString());
                        }
                        else
                        {
                            pListViewItem.SubItems.Add("");
                        }
                    }
                    else
                    { pListViewItem.SubItems.Add(""); }
                    //如果BLOB类型的字段有值
                    if (pDic.ContainsKey("SETTINGVALUE2"))
                    {
                        if (pDic["SETTINGVALUE2"] != null)
                        {
                            if (pListViewItem.SubItems[2].Text.Contains("File"))
                            {
                                pListViewItem.SubItems[1].Text = "*";
                            }
                        }
                    }
                    listView.Items.Add(pListViewItem);
                }
            }
            mSystable = null;
        }

        //新增删除参数按钮 ygc 2012-8-31
        private void btnDelete_Click(object sender, EventArgs e)
        {
            
            if (textBoxSettingName.Text.Trim() == "")
            {
                DevComponents.DotNetBar.MessageBoxEx.Show("请选择要删除的参数名");
                return;
            }
            DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("确定要删除该配置信息吗?","提示",MessageBoxButtons .OKCancel ,MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                return;
            }
            SysGisTable mSystable = new SysCommon.Gis.SysGisTable(_TmpWorkSpace);
            Exception err = null;
               bool t= mSystable.DeleteRows("SYSSETTING", "SETTINGNAME='" + textBoxSettingName.Text + "'", out err);
               if (t)
               {
                   DevComponents.DotNetBar.MessageBoxEx.Show("成功删除该配置信息!", "提示");
               }
               else
               {
                   DevComponents.DotNetBar.MessageBoxEx.Show("删除该配置信息失败！"+err.ToString(),"提示");
                   return;
               }
               textBoxSettingDescription.Text = "";
               textBoxSettingName.Text = "";
               textBoxSettingValue.Text = "";
               comboBoxDataType.Text = "";
               RefreshDataView();
        }

    }
}
