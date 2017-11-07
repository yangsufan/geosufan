using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using DevComponents.DotNetBar.Controls;
using System.Xml;
using ESRI.ArcGIS.Geometry;
using System.IO;
using ESRI.ArcGIS.DataSourcesFile;
using System.Threading;

namespace GeoCustomExport
{
    public partial class frmCustomExport : DevComponents.DotNetBar.Office2007Form
    {
        public frmCustomExport()
        {
            InitializeComponent();
            this.listViewFeaCls.ListViewItemSorter = lvwColumnSorter;
            this.listViewField.ListViewItemSorter = lvwColumnSorter;

        }
        private IWorkspace m_Workspace = null;//工作库工作空间

        public frmCustomExport(IWorkspace pWorkspace)
        {
            m_Workspace = pWorkspace;
            InitializeComponent();
            this.listViewFeaCls.ListViewItemSorter = lvwColumnSorter;
            this.listViewField.ListViewItemSorter = lvwColumnSorter;

        }
        private bool m_IsAdd = false;//是否在做添加操作
        private bool m_IsMutiSelect = false;//是否做批量选择勾选
        private string m_Connset = "";//字符串连接信息;
        private XmlDocument m_XmlDoc = new XmlDocument();//主xml
        private Dictionary<ListViewItem, XmlElement> m_dicElement = new Dictionary<ListViewItem, XmlElement>();//记录每个featureclass对应属性结构
        //private Dictionary<ListViewItem, List<string>> m_dicList = new Dictionary<ListViewItem, List<string>>();//记录每个featureclass删除掉的字段
        private XmlElement m_TempElement = null;//作为拷贝的临时节点
        private ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();//排序的类

        private void frmCustomExport_Load(object sender, EventArgs e)
        {
            cb_Range.SelectedIndex = 0;
        }

        /// <summary>
        /// 勾选状态改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewFeaCls_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (!m_IsAdd)//如果是做添加，则不作此操作
            {
                if (e.Item.Checked == false)//当取消勾选时。移除第二列的值
                {
                    //if (e.Item.SubItems.Count == 2)
                    //    e.Item.SubItems.Remove(e.Item.SubItems[1]);
                    //改变勾选状态为不勾选时需要实时清空对应的字段信息
                    listViewField.Items.Clear();
                    txtSQL.Text = "";
                }
                else//反之。，勾选时，添加第二列的值
                {
                    if (e.Item.SubItems.Count == 1)
                    {
                        e.Item.SubItems.Add(e.Item.Text);
                    }


                    //改变勾选状态为勾选时需要实时显示对应的字段信息
                    if (m_IsMutiSelect) return;
                    if (listViewFeaCls.SelectedItems.Count != 1) return;
                    listViewField.Items.Clear();
                    ListViewItem pSelItem = listViewFeaCls.SelectedItems[0];
                    if (!m_dicElement.Keys.Contains(pSelItem))//判断是否已存储字段
                    {
                        IFeatureClass pFeatureClass = pSelItem.Tag as IFeatureClass;
                        IFields pFields = pFeatureClass.Fields;
                        ListViewItem item = null;
                        for (int i = 0; i < pFields.FieldCount; i++)
                        {
                            //自带的字段不用显示出来所以continue
                            //if (pFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry || pFields.get_Field(i).Type == esriFieldType.esriFieldTypeOID || pFields.get_Field(i).Name.Contains("Shape.")) continue;
                            item = new ListViewItem(pFields.get_Field(i).Name);
                            item.Checked = true;
                            listViewField.Items.Add(item);
                        }
                    }
                    else
                    {
                        //显示字段
                        XmlNodeList tempnodelist = m_dicElement[pSelItem].SelectNodes(".//字段");
                        ListViewItem item = null;
                        foreach (XmlElement element in tempnodelist)
                        {
                            item = new ListViewItem();
                            string[] strFields = element.InnerText.Split('|');
                            item.Text = strFields[0];
                            item.SubItems.Add(strFields[1]);
                            if (element.GetAttribute("IsChecked") == "true")
                            {
                                item.Checked = true;
                            }
                            else
                            {
                                item.Checked = false;
                            }
                            m_IsAdd = true;
                            listViewField.Items.Add(item);
                            m_IsAdd = false;
                        }

                        //显示过滤条件
                        XmlNode pelement = m_dicElement[pSelItem].SelectSingleNode(".//过滤条件");
                        if (pelement == null) return;
                        txtSQL.Text = pelement.InnerText;
                    }


                }
            }
        }
        private void listViewField_ItemChecked(object sender, ItemCheckedEventArgs e)
        {


            if (!m_IsAdd)//如果是做添加，则不作此操作
            {
                if (e.Item.Checked == false)//当取消勾选时。移除第二列的值
                {
                    //if (e.Item.SubItems.Count == 2)
                    //    e.Item.SubItems.Remove(e.Item.SubItems[1]);
                }
                else//反之。，勾选时，添加第二列的值
                {
                    if (e.Item.SubItems.Count == 1)
                    {
                        e.Item.SubItems.Add(e.Item.Text);
                    }
                }
            }

        }

        //选择不同项的时候 右边字段列表显示不同的字段
        private void listViewFeaCls_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewFeaCls.SelectedItems.Count != 1)
            {
                return;//选择0项返回
            }
            txtSQL.Text = "";
            listViewField.Items.Clear();
            // m_ListField.Clear();
            ListViewItem pSelItem = listViewFeaCls.SelectedItems[0];
            if (pSelItem.Checked == false) return;
            if (!m_dicElement.Keys.Contains(pSelItem))//判断是否已存储字段
            {
                IFeatureClass pFeatureClass = pSelItem.Tag as IFeatureClass;
                IFields pFields = pFeatureClass.Fields;
                ListViewItem item = null;
                for (int i = 0; i < pFields.FieldCount; i++)
                {
                    //自带的字段不用显示出来所以continue
                    // if (pFields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry || pFields.get_Field(i).Type == esriFieldType.esriFieldTypeOID || pFields.get_Field(i).Name.Contains("Shape.")) continue;
                    item = new ListViewItem(pFields.get_Field(i).Name);
                    item.Checked = true;
                    listViewField.Items.Add(item);
                }
            }
            else
            {
                //显示字段
                XmlNodeList tempnodelist = m_dicElement[pSelItem].SelectNodes(".//字段");
                ListViewItem item = null;
                foreach (XmlElement element in tempnodelist)
                {
                    item = new ListViewItem();
                    string[] strFields = element.InnerText.Split('|');
                    item.Text = strFields[0];
                    item.SubItems.Add(strFields[1]);
                    if (element.GetAttribute("IsChecked") == "true")
                    {
                        item.Checked = true;
                    }
                    else
                    {
                        item.Checked = false;
                    }
                    m_IsAdd = true;
                    listViewField.Items.Add(item);
                    m_IsAdd = false;
                }

                //显示过滤条件
                XmlNode pelement = m_dicElement[pSelItem].SelectSingleNode(".//过滤条件");
                txtSQL.Text = pelement.InnerText;
            }
        }
        /// <summary>
        /// 添加过滤条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddCondition_Click(object sender, EventArgs e)
        {
            if (listViewFeaCls.SelectedItems.Count == 0) return;
            IFeatureClass pFeatureclass = null;
            if (listViewFeaCls.SelectedItems[0].Tag != null)
            {
                pFeatureclass = listViewFeaCls.SelectedItems[0].Tag as IFeatureClass;
            }
            else
            {
                if (m_Workspace == null)
                {
                    MessageBox.Show("请先连接数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                pFeatureclass = (m_Workspace as IFeatureWorkspace).OpenFeatureClass(listViewFeaCls.SelectedItems[0].Text);
            }

            SQLfrm frm = new SQLfrm(pFeatureclass, txtSQL.Text);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtSQL.Text = frm.SqlTxt;
            }
        }

        private void btn_DelCondition_Click(object sender, EventArgs e)
        {
            txtSQL.Text = "";
        }

        //双击图层列表时显示修改
        private void listViewFeaCls_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GeneralMethod("Edit", listViewFeaCls);
        }

        //双击字段列表时显示修改
        private void listViewField_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GeneralMethod("Edit", listViewField);
        }

        /// <summary>
        /// 添加图层对应关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddLayer_Click(object sender, EventArgs e)
        {
            GeneralMethod("Add", listViewFeaCls);
        }

        /// <summary>
        /// 修改图层对应关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ModifyLayer_Click(object sender, EventArgs e)
        {
            GeneralMethod("Edit", listViewFeaCls);
        }

        /// <summary>
        /// 删除图层对应关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DelLayer_Click(object sender, EventArgs e)
        {
            GeneralMethod("Del", listViewFeaCls);
        }

        /// <summary>
        /// 禁止双击的时候改变列表框勾选的状态。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewFeaCls_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks > 1)
            {

                ListViewItem lvi = listViewFeaCls.GetItemAt(e.X, e.Y);
                if (null != lvi)
                {
                    lvi.Checked = !lvi.Checked;
                }
            }
        }

        /// <summary>
        /// 增加，删除、修改、全选、反选
        /// </summary>
        private void GeneralMethod(string type, ListViewEx listview)
        {
            switch (type)
            {
                case "Add":
                    {
                        FrmEditLabel frm = null;
                        ListViewItem item = null;
                        if (listview.Name == "listViewFeaCls")
                        {
                            frm = new FrmEditLabel(true, listview);
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                item = new ListViewItem();
                                item.Text = (frm.objItem as ListViewItem).Text;
                                item.Tag = (frm.objItem as ListViewItem).Tag;
                                item.Checked = true;
                                item.SubItems.Add(frm.strTarget);
                                m_IsAdd = true;
                                listview.Items.Add(item);
                                m_IsAdd = false;
                            }
                        }
                        else
                        {
                            frm = new FrmEditLabel(false, listview);
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                item = new ListViewItem();
                                item.Text = frm.objItem.ToString();
                                item.Checked = true;
                                item.SubItems.Add(frm.strTarget);
                                m_IsAdd = true;
                                listview.Items.Add(item);
                                m_IsAdd = false;
                            }
                        }

                        break;
                    }
                case "Del":

                    if (listview.SelectedItems.Count == 0) return;
                    if (listview.Name == "listViewField")//删除的是字段
                    {
                        //List<string> list = null;
                        //if (m_dicList.Keys.Contains(listViewFeaCls.SelectedItems[0]))//可添加字段列表是否包含该项
                        //{
                        //    list = m_dicList[listViewFeaCls.SelectedItems[0]];
                        //    m_dicList.Remove(listViewFeaCls.SelectedItems[0]);
                        //}
                        //else
                        //{
                        //    list = new List<string>();
                        //}
                        //if (!list.Contains(listview.SelectedItems[0].Text))
                        //{
                        //    list.Add(listview.SelectedItems[0].Text);
                        //}
                        //m_dicList.Add(listViewFeaCls.SelectedItems[0],list);
                    }

                    foreach (ListViewItem item in listview.SelectedItems)
                    {
                        item.Remove();
                        if (listview.Name == "listViewFeaCls")//删除的是图层
                        {
                            //m_dicList.Remove(listview.SelectedItems[0]);//移除该图层对应的可添加字段列表
                            m_dicElement.Remove(item);//移除该图层对应的xml属性结构
                        }
                    }

                    break;
                case "Edit":
                    {
                        if (listview.SelectedItems.Count != 1) return;//选中的不是一个返回
                        FrmEditLabel frm = null;
                        if (listview.Name == "listViewFeaCls")
                        {
                            frm = new FrmEditLabel(true, listview.SelectedItems[0].Text, listview.SelectedItems[0].SubItems[1].Text);
                        }
                        else
                        {
                            frm = new FrmEditLabel(false, listview.SelectedItems[0].Text, listview.SelectedItems[0].SubItems[1].Text);
                        }
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            listview.SelectedItems[0].SubItems[1].Text = frm.strTarget;
                        }
                        break;
                    }
                case "AllCheck":
                    m_IsMutiSelect = true;
                    foreach (ListViewItem item in listview.Items)
                    {
                        item.Checked = true;
                    }
                    m_IsMutiSelect = false;
                    break;
                case "ReCheck":
                    m_IsMutiSelect = true;
                    foreach (ListViewItem item in listview.Items)
                    {
                        if (item.Checked) item.Checked = false;
                        else item.Checked = true;
                    }
                    m_IsMutiSelect = false;
                    break;

            }
        }
        /// <summary>
        /// 全选图层对应关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AllcheckLayer_Click(object sender, EventArgs e)
        {
            GeneralMethod("AllCheck", listViewFeaCls);
        }

        /// <summary>
        /// 反选图层对应关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RecheckLayer_Click(object sender, EventArgs e)
        {
            GeneralMethod("ReCheck", listViewFeaCls);
        }


        /// <summary>
        /// 全选字段对应关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AllcheckField_Click(object sender, EventArgs e)
        {
            GeneralMethod("AllCheck", listViewField);
        }

        /// <summary>
        /// 反选字段对应关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RecheckField_Click(object sender, EventArgs e)
        {
            GeneralMethod("ReCheck", listViewField);
        }

        /// <summary>
        /// 添加字段对应关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AddField_Click(object sender, EventArgs e)
        {
            GeneralMethod("Add", listViewField);
        }

        /// <summary>
        /// 删除字段对应关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DelField_Click(object sender, EventArgs e)
        {
            GeneralMethod("Del", listViewField);
        }

        /// <summary>
        /// 修改字段对应关系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ModifyField_Click(object sender, EventArgs e)
        {
            if (listViewField.SelectedItems.Count > 1)
            {
                MessageBox.Show("只能选择一项进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            GeneralMethod("Edit", listViewField);
        }

        /// <summary>
        /// 禁止双击改变勾选状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewField_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks > 1)
            {
                ListViewItem lvi = listViewField.GetItemAt(e.X, e.Y);
                if (null != lvi)
                {
                    lvi.Checked = !lvi.Checked;
                }
            }
        }

        /// <summary>
        /// 保存的方法
        /// </summary>
        private void SaveXml()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "XML文档|*.xml";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                progressBarLayer.Maximum = listViewFeaCls.Items.Count;
                progressBarLayer.Minimum = 0;
                progressBarLayer.Value = 0;

                XmlElement pRuleElement = m_XmlDoc.CreateElement("规则");
                XmlAttribute pcaijianatt = m_XmlDoc.CreateAttribute("是否裁剪");
                pcaijianatt.Value = checkBox_Cut.Checked.ToString();
                XmlElement peleWs = m_XmlDoc.CreateElement("连接信息");
                XmlElement peleGeo = m_XmlDoc.CreateElement("范围");
                XmlAttribute pgeopath = m_XmlDoc.CreateAttribute("路径");
                pgeopath.Value = txt_Range.Text;
                peleGeo.InnerText = pGeometry == null ? "" : MouduleCommon.XmlSerializer(pGeometry, "范围");
                peleWs.InnerText = m_Connset;
                peleGeo.Attributes.Append(pgeopath);
                pRuleElement.Attributes.Append(pcaijianatt);
                pRuleElement.AppendChild(peleGeo);
                pRuleElement.AppendChild(peleWs);
                XmlElement pElement = null;
                for (int i = 0; i < listViewFeaCls.Items.Count; i++)
                {
                    progressBarLayer.Value++;
                    labelDetail.Text = "正在处理第" + (i + 1) + "个图层：“" + listViewFeaCls.Items[i].Text + "”";
                    Application.DoEvents();

                    progressBarField.Minimum = 0;
                    progressBarField.Value = 0;
                    if (m_dicElement.Keys.Contains(listViewFeaCls.Items[i]))
                    {
                        progressBarField.Maximum = 1;
                        progressBarField.Value = 1;
                        pElement = m_dicElement[listViewFeaCls.Items[i]];
                    }
                    else
                    {

                        pElement = m_XmlDoc.CreateElement("对应关系");
                        //写入字段对应关系
                        if (listViewFeaCls.Items[i].Tag != null)
                        {
                            IFeatureClass pFeatureclass = listViewFeaCls.Items[i].Tag as IFeatureClass;
                            progressBarField.Maximum = pFeatureclass.Fields.FieldCount;
                            for (int j = 0; j < pFeatureclass.Fields.FieldCount; j++)
                            {
                                IField pField = pFeatureclass.Fields.get_Field(j);

                                progressBarField.Value++;
                                Application.DoEvents();
                                //自带的字段不用显示出来所以continue
                                // if (pField.Type == esriFieldType.esriFieldTypeGeometry || pField.Type == esriFieldType.esriFieldTypeOID || pField.Name.Contains("Shape.")) continue;
                                XmlElement pFieldsElement = m_XmlDoc.CreateElement("字段");
                                XmlAttribute pFieldAtt = m_XmlDoc.CreateAttribute("IsChecked");//勾选状态
                                pFieldAtt.Value = "true";
                                pFieldsElement.Attributes.Append(pFieldAtt);
                                pFieldsElement.InnerText = pField.Name + "|" + pField.Name;
                                pElement.AppendChild(pFieldsElement);
                            }
                        }

                        //写入过滤条件对应关系
                        XmlElement pSqlElement = m_XmlDoc.CreateElement("过滤条件");
                        pElement.AppendChild(pSqlElement);

                    }

                    //插入图层信息
                    XmlElement pSourceLayerEle = m_XmlDoc.CreateElement("源图层");
                    XmlElement pTargetLayerEle = m_XmlDoc.CreateElement("目标图层");
                    pSourceLayerEle.InnerText = listViewFeaCls.Items[i].Text;
                    if (listViewFeaCls.Items[i].SubItems.Count > 1)
                    {
                        pTargetLayerEle.InnerText = listViewFeaCls.Items[i].SubItems[1].Text;
                    }
                    pElement.InsertBefore(pTargetLayerEle, pElement.FirstChild);
                    pElement.InsertBefore(pSourceLayerEle, pElement.FirstChild);

                    //插入ID属性
                    XmlAttribute patt = m_XmlDoc.CreateAttribute("ID");
                    patt.Value = (i + 1).ToString();
                    pElement.Attributes.Append(patt);

                    //插入图层是否勾选
                    XmlAttribute patt2 = m_XmlDoc.CreateAttribute("IsChecked");
                    if (listViewFeaCls.Items[i].Checked == true)
                    {
                        patt2.Value = "true";
                    }
                    else
                    {
                        patt2.Value = "false";
                    }
                    pElement.Attributes.Append(patt2);

                    pRuleElement.AppendChild(pElement);
                }
                if (m_XmlDoc.ChildNodes.Count == 1)
                    m_XmlDoc.RemoveChild(m_XmlDoc.FirstChild);
                m_XmlDoc.AppendChild(pRuleElement as XmlNode);
                m_XmlDoc.Save(dlg.FileName);
                labelDetail.Text = "处理完成";
                MessageBox.Show("保存成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// 保存XML的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveXml_Click(object sender, EventArgs e)
        {
            SaveXml();
        }

        int m_Selectindex = -1;
        /// <summary>
        ///判断是不是右键，显示右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewFeaCls_MouseClick(object sender, MouseEventArgs e)
        {
            contextMenuLayer.Visible = false;

            if (e.Button == MouseButtons.Right)//右键菜单
            {
                if (listViewFeaCls.SelectedItems.Count == 0) return;

                contextMenuLayer.Show(e.Location);
                if (m_TempElement != null && m_Selectindex != listViewFeaCls.SelectedIndices[0])//判断是否已经复制或者是否是复制本身的那项
                {
                    ToolStripMenuItemPast.Enabled = true;
                }
                else
                {
                    ToolStripMenuItemPast.Enabled = false;
                }
            }
        }

        //保存featureclass的属性结构XML节点
        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (listViewFeaCls.SelectedItems.Count == 0) return;
            XmlElement pElement = m_XmlDoc.CreateElement("对应关系");

            //写入字段对应关系
            for (int i = 0; i < listViewField.Items.Count; i++)
            {
                XmlElement pFieldsElement = m_XmlDoc.CreateElement("字段");
                XmlAttribute pFieldAtt = m_XmlDoc.CreateAttribute("IsChecked");//勾选状态
                if (listViewField.Items[i].Checked)
                {
                    pFieldAtt.Value = "true";

                }
                else
                {
                    pFieldAtt.Value = "false";
                }
                if (listViewField.Items[i].SubItems.Count > 1)
                {
                    pFieldsElement.InnerText = listViewField.Items[i].Text + "|" + listViewField.Items[i].SubItems[1].Text;
                }
                else
                {
                    pFieldsElement.InnerText = listViewField.Items[i].Text + "|" + listViewField.Items[i].Text;
                }
                pFieldsElement.Attributes.Append(pFieldAtt);

                pElement.AppendChild(pFieldsElement);
            }

            //写入过滤条件对应关系

            XmlElement pSqlElement = m_XmlDoc.CreateElement("过滤条件");
            if (txtSQL.Text != "")
            {
                pSqlElement.InnerText = txtSQL.Text;
            }
            pElement.AppendChild(pSqlElement);
            if (!m_dicElement.Keys.Contains(listViewFeaCls.SelectedItems[0]))//判断是否已经存在
            {
                m_dicElement.Add(listViewFeaCls.SelectedItems[0], pElement);//保存
            }
            else
            {
                m_dicElement.Remove(listViewFeaCls.SelectedItems[0]);
                m_dicElement.Add(listViewFeaCls.SelectedItems[0], pElement);//保存
            }

        }

        //拷贝属性结构和过滤条件
        private void ToolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            if (listViewFeaCls.SelectedItems.Count == 0)
            {
                return;
            }
            if (!m_dicElement.Keys.Contains(listViewFeaCls.SelectedItems[0]))//判断是否存在，如果不存在，则提示不可复制
            {
                MessageBox.Show("请确认该项属性结构是否已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            m_TempElement = m_dicElement[listViewFeaCls.SelectedItems[0]];
            m_Selectindex = listViewFeaCls.SelectedIndices[0];
        }

        //粘贴属性结构到列表
        private void ToolStripMenuItemPast_Click(object sender, EventArgs e)
        {
            if (m_TempElement == null) return;
            if (listViewFeaCls.SelectedItems.Count == 0) return;
            listViewField.Items.Clear();

            //粘贴字段
            XmlNodeList tempnodelist = m_TempElement.SelectNodes("//字段");
            ListViewItem item = null;
            foreach (XmlElement element in tempnodelist)
            {
                item = new ListViewItem();
                string[] strFields = element.InnerText.Split('|');
                item.Text = strFields[0];
                if (strFields.Length == 2)
                {
                    item.SubItems.Add(strFields[1]);
                }
                if (element.GetAttribute("IsChecked") == "true")
                {
                    item.Checked = true;
                }
                m_IsAdd = true;
                listViewField.Items.Add(item);
                m_IsAdd = false;
            }

            //粘贴可添加字段的列表
            //List<string> list = null;
            //if (!m_dicList.Keys.Contains(listViewFeaCls.SelectedItems[0])) return;
            //list=m_dicList[listViewFeaCls.SelectedItems[0]];
            //if (m_dicList.Keys.Contains(listViewFeaCls.Items[m_Selectindex]))
            //{
            //  //  m_dicList.Remove()
            //}

        }

        /// <summary>
        /// 导入配置xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ImportXml_Click(object sender, EventArgs e)
        {
            if (listViewFeaCls.Items.Count > 0)
            {
                if (MessageBox.Show("是否保存当前配置", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    SaveXml();
                }
            }
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XML文档|*.xml";
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            m_XmlDoc = new XmlDocument();
            m_XmlDoc.Load(dlg.FileName);
            this.Text = "自定义导出--" + dlg.FileName;
            //清空现有图层、字段、过滤条件
            listViewFeaCls.Items.Clear();
            listViewField.Items.Clear();
            txtSQL.Text = "";
            m_dicElement.Clear();

            //开始读取XML
            //读取是否裁剪 
            if ((m_XmlDoc.FirstChild as XmlElement).GetAttribute("是否裁剪") == "True")
                checkBox_Cut.Checked = true;
            else
                checkBox_Cut.Checked = false;

            //读取连接信息
            XmlNode connnode = m_XmlDoc.SelectSingleNode("//连接信息");
            m_Connset = connnode.InnerText;
            GetWorkSpaceFromStr(m_Connset);

            //读取范围信息
            XmlElement rangenode = m_XmlDoc.SelectSingleNode("//范围") as XmlElement;
            string str = rangenode.InnerText;
            if (str.Trim() != "")
            {
                pGeometry = MouduleCommon.XmlDeSerializer(str) as IGeometry;
                txt_Range.Text = rangenode.GetAttribute("路径");
                cb_Range.SelectedIndex = 1;
            }

            XmlNodeList pNodelist = m_XmlDoc.SelectNodes("//对应关系");
            ListViewItem item;
            foreach (XmlElement element in pNodelist)
            {
                item = new ListViewItem();
                item.Text = element.ChildNodes[0].InnerText;
                item.SubItems.Add(element.ChildNodes[1].InnerText);
                if (element.GetAttribute("IsChecked") == "true")//判断是否勾选
                {
                    item.Checked = true;
                }
                else
                {
                    item.Checked = false;
                }
                m_IsAdd = true;
                listViewFeaCls.Items.Add(item);
                m_IsAdd = false;
                element.RemoveChild(element.FirstChild);//移除源图层
                element.RemoveChild(element.FirstChild);//移除目标图层
                m_dicElement.Add(item, element);


            }

        }
        /// <summary>
        /// 得到工作空间通过连接字符串
        /// </summary>
        /// <param name="str"></param>
        private void GetWorkSpaceFromStr(string str)
        {
            frmDBPropertySet frm = new frmDBPropertySet();
            Exception err = null;
            string[] arr = str.Split(';');
            switch (arr[0])
            {
                case "SDE":
                    string[] conn = arr[1].Split('|');
                    frm.SetWorkspace(conn[0], conn[1], conn[2], conn[3], conn[4], conn[5], out err);
                    m_Workspace = frm.Workspace;
                    break;
                case "GDB":
                case "PDB":
                    frm.SetWorkspace(arr[1], arr[0], out err);
                    m_Workspace = frm.Workspace;
                    break;

            }
        }

        private void Waiting()
        {
            //局部变量，在此线程创建，可以直接操作其成员
            frmWait frmTip = new frmWait("查找工作空间下的所有数据集...");
            frmTip.ShowDialog();
        }
        private void Waiting2()
        {
            //局部变量，在此线程创建，可以直接操作其成员
            frmWait frmTip = new frmWait("正在连接数据并加载到列表中...");
            frmTip.ShowDialog();
        }

        /// <summary>
        /// 实际工作函数
        /// </summary>
        private void DoWork(Thread thread, FrmDataset frm2)
        {
            thread.Start();
            string user = "";
            ListViewItem dtitem = null;
            IEnumDataset pEnumtempDataset = m_Workspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);
            if (m_Connset.Split(';')[0] == "SDE")
            {
                user = m_Connset.Split('|')[3].ToLower();
            }
            IDataset ptempdataset = pEnumtempDataset.Next();
            while (ptempdataset != null)
            {
                if (ptempdataset.Name.Contains("."))
                {
                    if (ptempdataset.Name.ToLower().Split('.')[0] != user)
                    {
                        ptempdataset = pEnumtempDataset.Next();
                        continue;
                    }
                }
                dtitem = new ListViewItem(ptempdataset.Name);
                dtitem.Tag = ptempdataset;
                frm2.ListDataset.Items.Add(dtitem);
                ptempdataset = pEnumtempDataset.Next();
            }

            if (thread.ThreadState == ThreadState.Running)
            {
                thread.Abort();//销毁线程，自动回收托管资源
            }
            Application.DoEvents();
            GC.Collect();
        }
        /// <summary>
        /// 实际工作函数
        /// </summary>
        private void DoWork(Thread thread, ListViewEx listDataset)
        {

            thread.Start();
            foreach (ListViewItem temitem in listDataset.CheckedItems)
            {
                if (cb_Range.SelectedItem.ToString() == "全库范围")
                {
                    //pEnumDataset.Reset();
                    IEnumDataset pEnumDataset = (temitem.Tag as IFeatureDataset).Subsets;
                    IDataset pDataset = pEnumDataset.Next();
                    ListViewItem item;
                    string name = "";
                    while (pDataset != null)
                    {
                        name = pDataset.Name;
                        if (name.Contains("."))//如果包含用户名，去掉用户名
                        {
                            name = name.Substring(name.LastIndexOf('.') + 1);
                        }
                        item = new ListViewItem(name);
                        item.Tag = pDataset as IFeatureClass;
                        // item.Tag = pDataset.Name;
                        item.Checked = true;
                        listViewFeaCls.Items.Add(item);
                        pDataset = pEnumDataset.Next();
                    }
                }
                else if (cb_Range.SelectedItem.ToString() == "导入范围")
                {
                    if (txt_Range.Text == "")
                    {
                        if (thread.ThreadState == ThreadState.Running)
                        {
                            thread.Abort();//销毁线程，自动回收托管资源
                        }
                        MessageBox.Show("请先导入范围！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        return;
                    }
                    IEnumDataset pEnumDataset = (temitem.Tag as IFeatureDataset).Subsets;
                    //pEnumDataset.Reset();
                    IDataset pDataset = pEnumDataset.Next();
                    ListViewItem item;
                    string name = "";
                    while (pDataset != null)
                    {
                        ISpatialFilter pSpatialFilter = new SpatialFilterClass();
                        pSpatialFilter.Geometry = pGeometry;
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;//yjl修改为只添加范围内图层
                        IFeatureCursor pFeaCursor = (pDataset as IFeatureClass).Search(pSpatialFilter, false);
                        if (pFeaCursor.NextFeature() == null)
                        {
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);
                            pDataset = pEnumDataset.Next();
                            continue;
                        }
                        name = pDataset.Name;
                        if (name.Contains("."))//如果包含用户名，去掉用户名
                        {
                            name = name.Substring(name.LastIndexOf('.') + 1);
                        }
                        item = new ListViewItem(name);
                        item.Tag = pDataset as IFeatureClass;
                        // item.Tag = pDataset.Name;
                        item.Checked = true;
                        listViewFeaCls.Items.Add(item);
                        pDataset = pEnumDataset.Next();
                    }

                }
            }

            if (thread.ThreadState == ThreadState.Running)
            {
                thread.Abort();//销毁线程，自动回收托管资源
            }
            GC.Collect();
        }
        /// <summary>
        /// 按照范围加载数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LoadData_Click(object sender, EventArgs e)
        {
            ////清空现有图层、字段、过滤条件
            //listViewFeaCls.Items.Clear();
            //listViewField.Items.Clear();
            //txtSQL.Text = "";
            //m_dicElement.Clear();

            frmDBPropertySet frm = new frmDBPropertySet("连接源工作空间", m_Connset);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                m_Workspace = frm.Workspace;
                m_Connset = frm.ConnSet;
            }
            Application.DoEvents();
            if (m_Workspace == null) return;
            if (listViewFeaCls.Items.Count > 0) return;
            FrmDataset frm2 = new FrmDataset();
            Thread thread = new Thread(Waiting);
            DoWork(thread, frm2);

            if (frm2.ListDataset.Items.Count == 0)
            {
                MessageBox.Show("数据源下没找到数据集", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (frm2.ShowDialog() == DialogResult.OK)
            {
                Thread thread2 = new Thread(new ThreadStart(Waiting2));
                DoWork(thread2, frm2.ListDataset);
            }



        }

        /// <summary>
        /// 范围类型改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_Range_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_Range.SelectedIndex != 1)
            {
                pGeometry = null;
                txt_Range.Text = "";
                btn_SelectRange.Enabled = false;
            }
            else
            {
                btn_SelectRange.Enabled = true;
            }
        }

        /// <summary>
        /// 输出数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ImportData_Click(object sender, EventArgs e)
        {
            if (saveworkspace == "")
            {
                MessageBox.Show("请设置存放路径", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (m_Workspace == null)//源工作空间
            {
                MessageBox.Show("请先连接数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IWorkspace pWorkspace = null;

            switch (savefiletype)
            {
                case "mdb":
                    pWorkspace = CreatePDBWorkSpace(savefilename);
                    break;
                case "gdb":
                    pWorkspace = CreateFileGDBWorkSpace(savefilename);
                    break;
                case "shp":
                    pWorkspace = CreateShapeFileWorkSpace(savefilename);
                    break;
            }
            DataExport(pWorkspace);


        }

        string saveworkspace = "";
        string savefilename = "";
        string savefiletype = "";
        public IGeometry pGeometry;
        /// <summary>
        /// 设置导出数据的范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutPath_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "(shp文件夹)|*.shp|(*.mdb)|*.mdb|(*.gdb)|*.gdb";
            dlg.AddExtension = true;
            if (dlg.ShowDialog() != DialogResult.OK) return;


            string filepath = dlg.FileName;
            switch (dlg.FilterIndex)
            {
                case 1:
                    savefiletype = "shp";
                    if (!filepath.EndsWith("shp")) filepath += ".shp";
                    break;
                case 2:
                    savefiletype = "mdb";
                    if (!filepath.EndsWith("mdb")) filepath += ".mdb";
                    break;
                case 3:
                    savefiletype = "gdb";
                    if (!filepath.EndsWith("gdb")) filepath += ".gdb";
                    break;
            }

            string[] savearry = new string[100];
            string[] savearry1 = new string[100];
            string str = "";
            savearry = filepath.Split(new char[] { '\\' });
            for (int i = 0; i < savearry.Length - 1; i++)
            {
                str = str + savearry[i] + "\\\\";
            }
            string str1 = savearry[savearry.Length - 1];
            saveworkspace = str;
            savefilename = str1;


            if (dlg.FilterIndex == 1)
            {
                txtPath.Text = filepath.Substring(0, filepath.LastIndexOf("."));
            }
            else
            {
                txtPath.Text = filepath;
            }
        }

        /// <summary>
        /// 创建PDB工作空间
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private IWorkspace CreatePDBWorkSpace(string filename)
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

            IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create("" + saveworkspace + "", "" + filename + "", null, 0);
            IName name = (ESRI.ArcGIS.esriSystem.IName)pWorkspaceName;
            IWorkspace PDB_workspace = (IWorkspace)name.Open();
            return PDB_workspace;

        }
        /// <summary>
        /// 创建GDB工作空间
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public IWorkspace CreateFileGDBWorkSpace(string filename)
        {
            IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.FileGDBWorkspaceFactoryClass();
            if (System.IO.File.Exists(filename))
            {
                if (pWorkspaceFactory.IsWorkspace(filename))
                {
                    IWorkspace pTempWks = pWorkspaceFactory.OpenFromFile(filename, 0);
                    return pTempWks;
                }
            }

            IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create("" + saveworkspace + "", "" + filename + "", null, 0);
            IName name = (ESRI.ArcGIS.esriSystem.IName)pWorkspaceName;

            IWorkspace fileGDB_workspace = (IWorkspace)name.Open();
            return fileGDB_workspace;
        }

        /// <summary>
        /// 创建SHP文件工作空间
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
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

            IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create("" + saveworkspace + "", "" + filename + "", null, 0);
            IName name = (ESRI.ArcGIS.esriSystem.IName)pWorkspaceName;

            IWorkspace shapefile_workspace = (IWorkspace)name.Open();
            return shapefile_workspace;
        }

        /// <summary>
        /// 根据XML内容数据输出
        /// </summary>
        /// <param name="xmldoc">配置文件XML</param>
        /// <param name="pWorkspace">输出工作空间</param>
        private void DataExportByXML(XmlDocument xmldoc)
        {
            string strCheckorNo = "";
            if (checkBox_Cut.Checked)
            {
                strCheckorNo = "True";
            }
            else
            {
                strCheckorNo = "False";
            }

            //读取连接信息
            XmlNode connnode = m_XmlDoc.SelectSingleNode("//连接信息");
            m_Connset = connnode.InnerText;
            GetWorkSpaceFromStr(m_Connset);

            //读取范围信息
            XmlElement rangenode = m_XmlDoc.SelectSingleNode("//范围") as XmlElement;
            string str = rangenode.InnerText;
            if (str.Trim() != "")
            {
                pGeometry = MouduleCommon.XmlDeSerializer(str) as IGeometry;
            }
            ImportToFC.progressStep = this.progressBarField;
            IFeatureClass pFeatureclass = null;
            string strCondtion = "";

            XmlNodeList layerNodelist = xmldoc.SelectNodes("//对应关系[@IsChecked='true']");
            progressBarLayer.Maximum = layerNodelist.Count;
            progressBarLayer.Minimum = 0;
            progressBarLayer.Value = 0;


            for (int k = 0; k < layerNodelist.Count; k++)
            {
                progressBarLayer.Value++;
                XmlElement element = layerNodelist[k] as XmlElement;
                labelDetail.Text = "正在处理第" + (k + 1) + "个图层：“" + element.FirstChild.InnerText + "”";
                Application.DoEvents();

                pFeatureclass = (m_Workspace as IFeatureWorkspace).OpenFeatureClass(element.FirstChild.InnerText);

                if (pFeatureclass == null) continue;
                string name = element.ChildNodes[1].InnerText;//输出目标图层名

                strCondtion = element.SelectSingleNode(".//过滤条件").InnerText;


                //要素输出
                //if (pGeometry != null)
                //{
                ///zq 2011-1219 当地图的空间参考与图层的不一致时进行转换
                IGeometry pTempGeometry = null;
                if (pGeometry != null)
                {
                    IClone pClone = (IClone)pGeometry;
                    pTempGeometry = pClone.Clone() as IGeometry;
                    try
                    {
                        if (pTempGeometry.SpatialReference != (pFeatureclass as IGeoDataset).SpatialReference)
                        {
                            pTempGeometry.Project((pFeatureclass as IGeoDataset).SpatialReference);
                        }
                    }
                    catch { }
                }
                //end
                if (savefiletype != "shp")
                {
                    IFeatureClass targetFeatureclass = CreateFeatureClass(name, pFeatureclass, m_Workspace, null, null, pFeatureclass.ShapeType, element);
                    if (targetFeatureclass == null) continue;



                    ImportToFC.CopyFeatureToFeatureClass(pFeatureclass, targetFeatureclass, pTempGeometry, strCheckorNo, strCondtion);
                }
                else
                {
                    IFeatureClass targetFeatureclass = CreateFeatureClassSHP(name, pFeatureclass, m_Workspace, null, null, pFeatureclass.ShapeType, element);
                    if (targetFeatureclass == null) continue;



                    ImportToFC.CopyFeatureToFeatureClassShp(pFeatureclass, targetFeatureclass, pTempGeometry, strCheckorNo, strCondtion);

                }

            }
            labelDetail.Text = "处理完成";
            Application.DoEvents();
            MessageBox.Show("导出数据完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //if (MessageBox.Show("导出数据完成，是否保存配置文件?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            //{
            //    SaveXml();
            //}
        }

        /// <summary>
        /// 数据输出
        /// </summary>
        /// <param name="pWorkspace">目标工作空间</param>
        private void DataExport(IWorkspace pWorkspace)
        {
            string strCheckorNo = "";
            if (checkBox_Cut.Checked)
            {
                strCheckorNo = "True";
            }
            else
            {
                strCheckorNo = "False";
            }
            ImportToFC.progressStep = this.progressBarField;
            IFeatureClass pFeatureclass = null;
            string strCondtion = "";

            progressBarLayer.Maximum = listViewFeaCls.CheckedItems.Count;
            progressBarLayer.Minimum = 0;
            progressBarLayer.Value = 0;

            for (int k = 0; k < this.listViewFeaCls.CheckedItems.Count; k++)
            {
                strCondtion = "";
                ImportToFC.m_DicFields.Clear();
                progressBarLayer.Value++;

                if (listViewFeaCls.CheckedItems[k].Tag != null)
                {
                    pFeatureclass = listViewFeaCls.CheckedItems[k].Tag as IFeatureClass;
                }
                else
                {
                    pFeatureclass = (m_Workspace as IFeatureWorkspace).OpenFeatureClass(listViewFeaCls.CheckedItems[k].Text);
                }
                this.labelDetail.Text = "正在导出数据：" + pFeatureclass.AliasName;
                this.labelDetail.Refresh();
                Application.DoEvents();

                if (pFeatureclass == null) continue;
                string name = listViewFeaCls.CheckedItems[k].SubItems[1].Text;//输出目标图层名

                XmlElement Element = null;
                if (m_dicElement.Keys.Contains(listViewFeaCls.CheckedItems[k]))
                {
                    Element = m_dicElement[listViewFeaCls.CheckedItems[k]];//字段和过滤条件的XML节点
                    strCondtion = Element.SelectSingleNode(".//过滤条件").InnerText;
                }

                //要素输出
                //if (pGeometry != null)
                //{
                ///zq 2011-1219 当地图的空间参考与图层的不一致时进行转换
                IGeometry pTempGeometry = null;
                if (pGeometry != null)
                {
                    IClone pClone = (IClone)pGeometry;
                    pTempGeometry = pClone.Clone() as IGeometry;
                    try
                    {
                        if (pTempGeometry.SpatialReference != (pFeatureclass as IGeoDataset).SpatialReference)
                        {
                            pTempGeometry.Project((pFeatureclass as IGeoDataset).SpatialReference);
                        }
                    }
                    catch { }
                }
                //end
                if (savefiletype != "shp")
                {
                    IFeatureClass targetFeatureclass = CreateFeatureClass(name, pFeatureclass, pWorkspace, null, null, pFeatureclass.ShapeType, Element);
                    if (targetFeatureclass == null) continue;



                    ImportToFC.CopyFeatureToFeatureClass(pFeatureclass, targetFeatureclass, pTempGeometry, strCheckorNo, strCondtion);
                }
                else
                {
                    IFeatureClass targetFeatureclass = CreateFeatureClassSHP(name, pFeatureclass, pWorkspace, null, null, pFeatureclass.ShapeType, Element);
                    if (targetFeatureclass == null) continue;



                    ImportToFC.CopyFeatureToFeatureClassShp(pFeatureclass, targetFeatureclass, pTempGeometry, strCheckorNo, strCondtion);

                }

            }
            labelDetail.Text = "处理完成";
            Application.DoEvents();
            if (MessageBox.Show("导出数据完成，是否保存配置文件?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                SaveXml();
            }
        }

        //创建featureclass
        private IFeatureClass CreateFeatureClass(string name, IFeatureClass pFeatureClass, IWorkspace pWorkspace, UID uidCLSID, UID uidCLSEXT, esriGeometryType GeometryType, XmlElement Element)
        {
            bool HasGeometryType = false;//是否包含geometry
            try
            {
                if (uidCLSID == null)
                {
                    uidCLSID = new UIDClass();
                    switch (pFeatureClass.FeatureType)
                    {
                        case (esriFeatureType.esriFTSimple):
                            uidCLSID.Value = "{52353152-891A-11D0-BEC6-00805F7C4268}";
                            break;
                        case (esriFeatureType.esriFTSimpleJunction):
                            GeometryType = esriGeometryType.esriGeometryPoint;
                            uidCLSID.Value = "{CEE8D6B8-55FE-11D1-AE55-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTComplexJunction):
                            uidCLSID.Value = "{DF9D71F4-DA32-11D1-AEBA-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTSimpleEdge):
                            GeometryType = esriGeometryType.esriGeometryPolyline;
                            uidCLSID.Value = "{E7031C90-55FE-11D1-AE55-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTComplexEdge):
                            GeometryType = esriGeometryType.esriGeometryPolyline;
                            uidCLSID.Value = "{A30E8A2A-C50B-11D1-AEA9-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTAnnotation):
                            GeometryType = esriGeometryType.esriGeometryPolygon;
                            uidCLSID.Value = "{E3676993-C682-11D2-8A2A-006097AFF44E}";
                            break;
                        case (esriFeatureType.esriFTDimension):
                            GeometryType = esriGeometryType.esriGeometryPolygon;
                            uidCLSID.Value = "{496764FC-E0C9-11D3-80CE-00C04F601565}";
                            break;
                    }
                }

                // 设置 uidCLSEXT (if Null)
                if (uidCLSEXT == null)
                {
                    switch (pFeatureClass.FeatureType)
                    {
                        case (esriFeatureType.esriFTAnnotation):
                            uidCLSEXT = new UIDClass();
                            uidCLSEXT.Value = "{24429589-D711-11D2-9F41-00C04F6BC6A5}";
                            break;
                        case (esriFeatureType.esriFTDimension):
                            uidCLSEXT = new UIDClass();
                            uidCLSEXT.Value = "{48F935E2-DA66-11D3-80CE-00C04F601565}";
                            break;
                    }
                }

                IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                IFields pFields = new FieldsClass();
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
                if (Element == null)
                {
                    for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
                    {
                        IClone pClone = pFeatureClass.Fields.get_Field(i) as IClone;
                        IField pTempField = pClone.Clone() as IField;
                        IFieldEdit pTempFieldEdit = pTempField as IFieldEdit;
                        if (pTempField.Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            HasGeometryType = true;
                        }
                        string strFieldName = pTempField.Name;
                        string[] strFieldNames = strFieldName.Split('.');

                        if (pFieldsEdit.FindField(strFieldNames[strFieldNames.GetLength(0) - 1]) > -1) continue;

                        pTempFieldEdit.Name_2 = strFieldNames[strFieldNames.GetLength(0) - 1];
                        pFieldsEdit.AddField(pTempField);
                    }
                }
                else
                {
                    XmlNodeList pNodelist = Element.SelectNodes(".//字段[@IsChecked='true']");
                    foreach (XmlElement element in pNodelist)
                    {
                        string[] str = element.InnerText.Split('|');
                        IClone pClone = pFeatureClass.Fields.get_Field(pFeatureClass.Fields.FindField(str[0])) as IClone;
                        IField pTempField = pClone.Clone() as IField;
                        IFieldEdit pTempFieldEdit = pTempField as IFieldEdit;
                        if (pTempField.Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            HasGeometryType = true;
                        }
                        //string strFieldName = pTempField.Name;
                        //string[] strFieldNames = strFieldName.Split('.');
                        //if (pFieldsEdit.FindField(strFieldNames[strFieldNames.GetLength(0) - 1]) > -1) continue;
                        pTempFieldEdit.Name_2 = str[1];//用目标字段
                        pFieldsEdit.AddField(pTempField);
                        ImportToFC.m_DicFields.Add(str[0], str[1]);
                    }
                }
                if (!HasGeometryType)//此处自己创建Geometry字段，不然创建不成功
                {
                    IClone pClone = pFeatureClass.Fields.get_Field(pFeatureClass.Fields.FindField("SHAPE")) as IClone;
                    IField pTempField = pClone.Clone() as IField;
                    IFieldEdit pTempFieldEdit = pTempField as IFieldEdit;
                    pFieldsEdit.AddField(pTempField);
                }
                string strShapeFieldName = pFeatureClass.ShapeFieldName;
                string[] strShapeNames = strShapeFieldName.Split('.');
                strShapeFieldName = strShapeNames[strShapeNames.GetLength(0) - 1];


                IFeatureClass targetFeatureclass = pFeatureWorkspace.CreateFeatureClass("" + name + "", pFields, uidCLSID, uidCLSEXT, pFeatureClass.FeatureType, strShapeFieldName, "");
                //如果是注记要素类 则需要拷贝注记样式
                if (targetFeatureclass.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    ImportToFC.CopyAnnoPropertyToFC(pFeatureClass, targetFeatureclass);
                }

                return targetFeatureclass;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Cannot create a low precision dataset in a high precision database.")
                {
                    MessageBox.Show("创建要素类失败");
                }
            }
            IFeatureClass featureclass = null;
            return featureclass;
        }

        //创建featureclass针对shp，因其字段长度最长限制为10...yjl630
        private IFeatureClass CreateFeatureClassSHP(string name, IFeatureClass pFeatureClass, IWorkspace pWorkspace, UID uidCLSID, UID uidCLSEXT, esriGeometryType GeometryType, XmlElement Element)
        {
            bool HasGeometryType = false;//是否包含geometry
            try
            {
                if (uidCLSID == null)
                {
                    uidCLSID = new UIDClass();
                    switch (pFeatureClass.FeatureType)
                    {
                        case (esriFeatureType.esriFTSimple):
                            uidCLSID.Value = "{52353152-891A-11D0-BEC6-00805F7C4268}";
                            break;
                        case (esriFeatureType.esriFTSimpleJunction):
                            GeometryType = esriGeometryType.esriGeometryPoint;
                            uidCLSID.Value = "{CEE8D6B8-55FE-11D1-AE55-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTComplexJunction):
                            uidCLSID.Value = "{DF9D71F4-DA32-11D1-AEBA-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTSimpleEdge):
                            GeometryType = esriGeometryType.esriGeometryPolyline;
                            uidCLSID.Value = "{E7031C90-55FE-11D1-AE55-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTComplexEdge):
                            GeometryType = esriGeometryType.esriGeometryPolyline;
                            uidCLSID.Value = "{A30E8A2A-C50B-11D1-AEA9-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTAnnotation):
                            GeometryType = esriGeometryType.esriGeometryPolygon;
                            uidCLSID.Value = "{E3676993-C682-11D2-8A2A-006097AFF44E}";
                            break;
                        case (esriFeatureType.esriFTDimension):
                            GeometryType = esriGeometryType.esriGeometryPolygon;
                            uidCLSID.Value = "{496764FC-E0C9-11D3-80CE-00C04F601565}";
                            break;
                    }
                }

                // 设置 uidCLSEXT (if Null)
                if (uidCLSEXT == null)
                {
                    switch (pFeatureClass.FeatureType)
                    {
                        case (esriFeatureType.esriFTAnnotation):
                            uidCLSEXT = new UIDClass();
                            uidCLSEXT.Value = "{24429589-D711-11D2-9F41-00C04F6BC6A5}";
                            break;
                        case (esriFeatureType.esriFTDimension):
                            uidCLSEXT = new UIDClass();
                            uidCLSEXT.Value = "{48F935E2-DA66-11D3-80CE-00C04F601565}";
                            break;
                    }
                }

                IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                IFields pFields = new FieldsClass();
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
                //toShpField = new Dictionary<string, string>();
                if (Element == null)
                {
                    for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
                    {
                        IClone pClone = pFeatureClass.Fields.get_Field(i) as IClone;
                        IField pTempField = pClone.Clone() as IField;
                        IFieldEdit pTempFieldEdit = pTempField as IFieldEdit;
                        if (pTempField.Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            HasGeometryType = true;
                        }
                        string strFieldName = pTempField.Name;
                        string[] strFieldNames = strFieldName.Split('.');

                        if (pFieldsEdit.FindField(strFieldNames[strFieldNames.GetLength(0) - 1]) > -1) continue;
                        string fdName = strFieldNames[strFieldNames.GetLength(0) - 1];
                        pTempFieldEdit.Name_2 = fdName;
                        pFieldsEdit.AddField(pTempField);
                    }
                }
                else
                {

                    XmlNodeList pNodelist = Element.SelectNodes(".//字段[@IsChecked='true']");
                    foreach (XmlElement element in pNodelist)
                    {
                        string[] str = element.InnerText.Split('|');
                        IClone pClone = pFeatureClass.Fields.get_Field(pFeatureClass.Fields.FindField(str[0])) as IClone;
                        IField pTempField = pClone.Clone() as IField;
                        IFieldEdit pTempFieldEdit = pTempField as IFieldEdit;
                        if (pTempField.Type == esriFieldType.esriFieldTypeGeometry)
                        {
                            HasGeometryType = true;
                        }
                        //string strFieldName = pTempField.Name;
                        //string[] strFieldNames = strFieldName.Split('.');
                        //if (pFieldsEdit.FindField(strFieldNames[strFieldNames.GetLength(0) - 1]) > -1) continue;
                        pTempFieldEdit.Name_2 = str[1];//用目标字段
                        pFieldsEdit.AddField(pTempField);
                        ImportToFC.m_DicFields.Add(str[0], str[1]);
                    }
                }

                if (!HasGeometryType)//此处自己创建Geometry字段，不然创建不成功
                {
                    IClone pClone = pFeatureClass.Fields.get_Field(pFeatureClass.Fields.FindField("SHAPE")) as IClone;
                    IField pTempField = pClone.Clone() as IField;
                    IFieldEdit pTempFieldEdit = pTempField as IFieldEdit;
                    pFieldsEdit.AddField(pTempField);
                }
                string strShapeFieldName = pFeatureClass.ShapeFieldName;
                string[] strShapeNames = strShapeFieldName.Split('.');
                strShapeFieldName = strShapeNames[strShapeNames.GetLength(0) - 1];

                IFieldChecker fdCheker = new FieldCheckerClass();
                IEnumFieldError enumFdError = null;
                IFields validFds = null;
                fdCheker.ValidateWorkspace = pWorkspace;
                fdCheker.Validate(pFields, out enumFdError, out validFds);

                if (File.Exists(pWorkspace.PathName + "\\" + name + ".shp"))//如果不判断，则创建重名的要素会失败且将其删除，原因未知
                    return null;

                IFeatureClass targetFeatureclass = pFeatureWorkspace.CreateFeatureClass("" + name + "", validFds, uidCLSID, uidCLSEXT, pFeatureClass.FeatureType, strShapeFieldName, "");

                return targetFeatureclass;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Cannot create a low precision dataset in a high precision database.")
                {
                    MessageBox.Show("创建要素类失败");
                }
            }
            IFeatureClass featureclass = null;
            return featureclass;
        }

        //快捷键设置
        private void listViewFeaCls_KeyDown(object sender, KeyEventArgs e)
        {
            if (listViewFeaCls.SelectedItems.Count != 1) return;
            if (e.Modifiers.CompareTo(Keys.Control) == 0 && e.KeyCode == Keys.C)//ctrl+c快捷键复制
            {
                if (!m_dicElement.Keys.Contains(listViewFeaCls.SelectedItems[0]))//判断是否存在，如果不存在，则提示不可复制
                {
                    MessageBox.Show("请确认该项属性结构是否已保存", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                m_TempElement = m_dicElement[listViewFeaCls.SelectedItems[0]];
                m_Selectindex = listViewFeaCls.SelectedIndices[0];
            }

            if (e.Modifiers.CompareTo(Keys.Control) == 0 && e.KeyCode == Keys.V)//ctrl+V快捷键粘贴
            {
                if (m_TempElement == null) return;
                if (m_Selectindex == listViewFeaCls.SelectedIndices[0]) return;
                listViewField.Items.Clear();

                //粘贴字段
                XmlNodeList tempnodelist = m_TempElement.SelectNodes("//字段");
                ListViewItem item = null;
                foreach (XmlElement element in tempnodelist)
                {
                    item = new ListViewItem();
                    string[] strFields = element.InnerText.Split('|');
                    item.Text = strFields[0];
                    if (strFields.Length == 2)
                    {
                        item.SubItems.Add(strFields[1]);
                    }
                    if (element.GetAttribute("IsChecked") == "true")
                    {
                        item.Checked = true;
                    }
                    m_IsAdd = true;
                    listViewField.Items.Add(item);
                    m_IsAdd = false;
                }
            }
        }

        /// <summary>
        /// 导入范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectRange_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "shp数据|*.shp|个人数据库'范围面'层(*.mdb)|*.mdb|文件数据库'范围面'层(*.gdb)|gdb|CAD文件'0'层(*.dwg)|*.dwg";
            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;
            IPolygon pGon = new PolygonClass();
            pGon = GetPolyGonFromFile(dlg.FileName);
            if (pGon == null) return;
            pGeometry = pGon as IGeometry;
            txt_Range.Text = dlg.FileName;
        }

        /// <summary>
        /// 从文件路径获得一个PolyGon
        /// </summary>
        /// <param name="path">文件全路径</param>
        /// <returns></returns>
        private IPolygon GetPolyGonFromFile(string path)
        {
            IPolygon pGon = null;
            if (path.EndsWith(".mdb"))
            {
                string errmsg = "";
                IWorkspaceFactory pwf = new AccessWorkspaceFactoryClass();
                IWorkspace pworkspace = pwf.OpenFromFile(path, 0);
                IEnumDataset pEnumdataset = pworkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                pEnumdataset.Reset();
                IDataset pDataset = pEnumdataset.Next();
                while (pDataset != null)
                {
                    IFeatureClass pFeatureclass = pDataset as IFeatureClass;
                    //if (pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolygon && pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolyline)
                    if (pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolygon)
                    {
                        pDataset = pEnumdataset.Next();
                        continue;
                    }
                    else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                        pGon = GetUnionFromCursor(pCursor) as IPolygon;
                    }
                    //else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    //{
                    //    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                    //    IFeature pFeature = pCursor.NextFeature();
                    //    if (pFeature != null)
                    //    {

                    //        IPolyline pPolyline = pFeature.Shape as IPolyline;
                    //        pGon = GetPolygonFormLine(pPolyline);
                    //        if (pGon.IsClosed == false)
                    //        {
                    //            errmsg = "选择的要素不能构成封闭多边形！";
                    //            pGon = null;
                    //            pDataset = pEnumdataset.Next();
                    //            continue;
                    //        }
                    //        else
                    //        {
                    //            break;
                    //        }
                    //    }
                    //}
                    pDataset = pEnumdataset.Next();
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
                                //if (pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolygon && pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolyline)
                                if (pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolygon)
                                {
                                    pDataset1 = pEnumDataset2.Next();
                                    continue;
                                }
                                else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
                                {
                                    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                                    pGon = GetUnionFromCursor(pCursor) as IPolygon;
                                }
                                //else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
                                //{
                                //    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                                //    IFeature pFeature = pCursor.NextFeature();
                                //    if (pFeature != null)
                                //    {

                                //        IPolyline pPolyline = pFeature.Shape as IPolyline;
                                //        pGon = GetPolygonFormLine(pPolyline);
                                //        if (pGon.IsClosed == false)
                                //        {
                                //            errmsg = "选择的要素不能构成封闭多边形！";
                                //            pGon = null;
                                //            pDataset1 = pEnumDataset2.Next();
                                //            continue;
                                //        }
                                //        else
                                //        {
                                //            break;
                                //        }
                                //    }
                                //}
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
                        MessageBox.Show(errmsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("请选择一个包含面要素的文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return pGon;
                }
            }
            else if (path.EndsWith(".shp"))
            {
                IWorkspaceFactory pwf = new ShapefileWorkspaceFactoryClass();
                string filepath = System.IO.Path.GetDirectoryName(path);
                string filename = path.Substring(path.LastIndexOf("\\") + 1);
                IFeatureWorkspace pFeatureworkspace = (IFeatureWorkspace)pwf.OpenFromFile(filepath, 0);
                IFeatureClass pFeatureclass = pFeatureworkspace.OpenFeatureClass(filename);
                if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
                {
                    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                    pGon = GetUnionFromCursor(pCursor) as IPolygon;
                }
                //else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
                //{
                //    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                //    IFeature pFeature = pCursor.NextFeature();
                //    if (pFeature != null)
                //    {

                //        IPolyline pPolyline = pFeature.Shape as IPolyline;
                //        pGon = GetPolygonFormLine(pPolyline);
                //        if (pGon.IsClosed == false)
                //        {
                //            MessageBox.Show("选择的线要素不能构成封闭多边形！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //            return null;
                //        }
                //    }
                //}
                else
                {
                    MessageBox.Show("请选择一个面要素文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return null;
                }


            }
            else if (path.EndsWith("gdb"))
            {
                string errmsg = "";
                IWorkspaceFactory pwf = new FileGDBWorkspaceFactoryClass();
                IWorkspace pworkspace = pwf.OpenFromFile(path.Substring(0, path.LastIndexOf("\\")), 0);
                IEnumDataset pEnumdataset = pworkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                pEnumdataset.Reset();
                IDataset pDataset = pEnumdataset.Next();
                while (pDataset != null)
                {
                    IFeatureClass pFeatureclass = pDataset as IFeatureClass;
                    //if (pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolygon && pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolyline)
                    if (pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolygon)
                    {
                        pDataset = pEnumdataset.Next();
                        continue;
                    }
                    else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                        pGon = GetUnionFromCursor(pCursor) as IPolygon;
                    }
                    //else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    //{
                    //    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                    //    IFeature pFeature = pCursor.NextFeature();
                    //    if (pFeature != null)
                    //    {

                    //        IPolyline pPolyline = pFeature.Shape as IPolyline;
                    //        pGon = GetPolygonFormLine(pPolyline);
                    //        if (pGon.IsClosed == false)
                    //        {
                    //            errmsg = "选择的要素不能构成封闭多边形！";
                    //            pGon = null;
                    //            pDataset = pEnumdataset.Next();
                    //            continue;
                    //        }
                    //        else
                    //        {
                    //            break;
                    //        }
                    //    }
                    //}

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
                                //if (pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolygon && pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolyline)
                                if (pFeatureclass.ShapeType != esriGeometryType.esriGeometryPolygon)
                                {
                                    pDataset1 = pEnumDataset2.Next();
                                    continue;
                                }
                                else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolygon)
                                {
                                    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                                    pGon = GetUnionFromCursor(pCursor) as IPolygon;

                                }
                                //else if (pFeatureclass.ShapeType == esriGeometryType.esriGeometryPolyline)
                                //{
                                //    IFeatureCursor pCursor = pFeatureclass.Search(null, false);
                                //    IFeature pFeature = pCursor.NextFeature();
                                //    if (pFeature != null)
                                //    {

                                //        IPolyline pPolyline = pFeature.Shape as IPolyline;
                                //        pGon = GetPolygonFormLine(pPolyline);
                                //        if (pGon.IsClosed == false)
                                //        {
                                //            errmsg = "选择的要素不能构成封闭多边形！";
                                //            pGon = null;
                                //            pDataset1 = pEnumDataset2.Next();
                                //            continue;
                                //        }
                                //        else
                                //        {
                                //            break;
                                //        }
                                //    }
                                //}
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
                        MessageBox.Show(errmsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("请选择一个包含面要素的文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return pGon;
                }
            }
            else if (path.EndsWith("dwg"))
            {
                pGon = GetLyrUnionPlygon("0", path) as IPolygon;
            }
            return pGon;
        }

        private IGeometry GetUnionFromCursor(IFeatureCursor pFeatureCursor)
        {
            //构造
            IGeometryBag pGeometryBag = new GeometryBagClass();
            IGeometryCollection pGeometryCol = pGeometryBag as IGeometryCollection;

            object obj = System.Type.Missing;
            //获得所有图形
            IFeature pFeature = pFeatureCursor.NextFeature();
            while (pFeature != null)
            {

                if (pFeature.Shape != null && !pFeature.Shape.IsEmpty)
                    pGeometryCol.AddGeometry(pFeature.ShapeCopy, ref obj, ref obj);
                pFeature = pFeatureCursor.NextFeature();
            }

            //构造合并
            ITopologicalOperator pTopo = new PolygonClass();
            pTopo.ConstructUnion(pGeometryCol as IEnumGeometry);

            IGeometry pGeometry = pTopo as IGeometry;
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
            pFeatureCursor = null;

            return pGeometry;
        }

        #region 通过dwg得到geometry
        /// <summary>
        /// 获得指定图层的合并范围 为本次加的一个函数
        /// </summary>
        /// <param name="strLyrName"></param>
        /// <param name="strWhere"></param>
        /// <param name="eFeatureType"></param>
        /// <param name="eGeometryType"></param>
        /// <returns></returns>
        public IGeometry GetLyrUnionPlygon(string strLyrName, string strProjectDwgPath)
        {
            string strCADFielPath = CopyDwgToTempDIR(strProjectDwgPath);
            IList<IFeature> vFeaList = GetLyrFeas(strLyrName, strCADFielPath);
            if (vFeaList == null) return null;
            if (vFeaList.Count < 1) return null;
            //构造
            IGeometryBag pGeometryBag = new GeometryBagClass();
            IGeometryCollection pGeometryCol = pGeometryBag as IGeometryCollection;

            object obj = System.Type.Missing;
            //获得所有图形
            for (int i = 0; i < vFeaList.Count; i++)
            {
                if (vFeaList[i].Shape != null && !vFeaList[i].Shape.IsEmpty) pGeometryCol.AddGeometry(vFeaList[i].ShapeCopy, ref obj, ref obj);
            }

            //构造合并
            ITopologicalOperator pTopo = new PolygonClass();
            pTopo.ConstructUnion(pGeometryCol as IEnumGeometry);

            IGeometry pGeometry = pTopo as IGeometry;


            IZAware pZAware = pGeometry as IZAware;
            pZAware.ZAware = false;


            return pGeometry;

        }
        //将dwg文件存放到临时英文目录下
        private string CopyDwgToTempDIR(string strProjectDwgPath)
        {
            //首先需要将dwg文件存放到临时英文目录下
            string sPath = System.IO.Path.GetTempPath() + "NJTDTManager";
            string sFileName = DateTime.Now.ToString("yyyyMMddhhmmss");
            sFileName = sFileName + ".dwg";
            string sDesFileName = sPath + "\\" + sFileName;
            if (!System.IO.Directory.Exists(sPath))
            {
                System.IO.Directory.CreateDirectory(sPath);
            }
            //ZCL:*********************************
            System.IO.File.Copy(strProjectDwgPath, sDesFileName);
            //*********************************
            return sDesFileName;
        }

        /// <summary>
        /// 根据图层 要素类型 获得查询要素
        /// </summary>
        /// <param name="strLyrName"></param>
        /// <param name="strWhere"></param>
        /// <param name="eFeatureType"></param>
        /// <param name="eGeometryType"></param>
        /// <returns></returns>
        public IList<IFeature> GetLyrFeas(string strLyrName, string strCADFielPath)
        {
            IList<IFeature> vFeaList = new List<IFeature>();

            string strPath = "";
            if (!System.IO.File.Exists(strCADFielPath))
            {
                return null;
            }

            strPath = System.IO.Path.GetDirectoryName(strCADFielPath);

            IWorkspaceFactory2 pWksFac2 = new ESRI.ArcGIS.DataSourcesFile.CadWorkspaceFactoryClass();
            if (!pWksFac2.IsWorkspace(strPath))
            {
                return null;
            }
            IFeatureWorkspace pFeaWks = pWksFac2.OpenFromFile(strPath, 0) as IFeatureWorkspace;
            if (pFeaWks == null) return vFeaList;
            //CAD文件名
            string strCADFileName = System.IO.Path.GetFileName(strCADFielPath);

            IDataset pFeaDataset = pFeaWks.OpenFeatureDataset(strCADFileName);

            IEnumDataset pEnumSubs = pFeaDataset.Subsets;
            IDataset pDataset = pEnumSubs.Next();

            //获得所有要素类
            while (pDataset != null)
            {
                //判断类型
                IFeatureClass pFeaCls = pDataset as IFeatureClass;
                if (pFeaCls == null || pFeaCls.FeatureType != esriFeatureType.esriFTSimple || pFeaCls.ShapeType != esriGeometryType.esriGeometryPolygon)
                {
                    pDataset = pEnumSubs.Next();
                    continue;
                }

                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = "";

                IFeatureCursor pFeaCursor = pFeaCls.Search(pQueryFilter, false);
                IFeature pFea = pFeaCursor.NextFeature();
                while (pFea != null)
                {
                    //处理一个Fea
                    string strCADLyrName = pFea.get_Value(pFea.Fields.FindField("Layer")).ToString();

                    //图层匹配
                    if (strCADLyrName != strLyrName)
                    {
                        pFea = pFeaCursor.NextFeature();
                        continue;
                    }

                    //添加返回值
                    vFeaList.Add(pFea);

                    pFea = pFeaCursor.NextFeature();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);
                pFeaCursor = null;

                pDataset = pEnumSubs.Next();
            }

            return vFeaList;
        }

        #endregion

        //通过封闭线构成面 xisheng 20110926 
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

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //点击列排序
        private void listViewFeaCls_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 检查被按下的数据行是否就是刚刚被排序过的数据行。
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // 逆转此数据行的排序顺序。
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // 设定要排序哪一个数据行，预设采用递增排序。
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // 以上述的排序选项设定来加以排序。
            this.listViewFeaCls.Sort();

        }

        /// <summary>
        /// 点击字段列表排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewField_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 检查被按下的数据行是否就是刚刚被排序过的数据行。
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // 逆转此数据行的排序顺序。
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // 设定要排序哪一个数据行，预设采用递增排序。
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // 以上述的排序选项设定来加以排序。
            this.listViewField.Sort();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (m_Workspace == null)
            {
                MessageBox.Show("请先连接数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IWorkspace2 pW2 = m_Workspace as IWorkspace2;
            string sRes = "";
            IFeatureClass pFC = null;
            IFeatureWorkspace pFW = m_Workspace as IFeatureWorkspace;
            progressBarLayer.Maximum = listViewFeaCls.Items.Count;
            progressBarLayer.Value = 0;
            labelDetail.Text = "开始检查";
            foreach (ListViewItem lvi in listViewFeaCls.Items)
            {
                labelDetail.Text = "检查"+lvi.Text;
                if (!pW2.get_NameExists(esriDatasetType.esriDTFeatureClass, lvi.Text))
                {
                    sRes += "图层：" + lvi.Text + "\r\n";
                }
                else
                {
                    pFC = pFW.OpenFeatureClass(lvi.Text);
                    if (pFC != null)
                    {
                        string sFRes = "";
                        if (m_dicElement.Keys.Contains(lvi))//判断是否已存储字段
                        {
                            //显示字段
                            XmlNodeList tempnodelist = m_dicElement[lvi].SelectNodes(".//字段");
                            ListViewItem item = null;
                            progressBarField.Maximum = tempnodelist.Count;
                            progressBarField.Value = 0;
                            foreach (XmlElement element in tempnodelist)
                            {
                                string[] strFields = element.InnerText.Split('|');
                                int idx = pFC.FindField(strFields[0]);
                                if (idx == -1)
                                {
                                    sFRes += "\t字段：" + strFields[0] + "\r\n";
                                }
                                progressBarField.PerformStep();
                            }
                        }
                        if (sFRes != "")
                        {
                            sRes += "图层：" + lvi.Text + "\r\n";
                            sRes +=sFRes;
                        }
                    }

                }
                progressBarLayer.PerformStep();
            }
            progressBarField.Value = 0;
            progressBarLayer.Value = 0;
            labelDetail.Text = "";
            FrmCheck fmCheck = new FrmCheck(sRes);
            fmCheck.ShowDialog();
        }
    }

}
