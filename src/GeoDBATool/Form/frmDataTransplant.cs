using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Threading;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.ADF.CATIDs;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;

namespace GeoDBATool
{
    public partial class frmDataTransplant : DevComponents.DotNetBar.Office2007Form
    {
        private Plugin.Application.IAppGISRef m_AppGIS;               //主功能应用APP
        private IGeometry m_Geometry;
        private EnumOperateType m_OpeType;
        private XmlElement m_DbProjectElement;

        public IGeometry DrawGeometry
        {
            set
            {
                m_Geometry=value;
            }
        }

        private clsDataThread m_DataThread;
        private System.Windows.Forms.Timer _timer;

        public frmDataTransplant(string strFrmName,EnumOperateType pType, Plugin.Application.IAppGISRef pAppGIS)
        {
            InitializeComponent();
            this.Text = strFrmName;
            m_OpeType = pType;
            m_AppGIS = pAppGIS;
            InitializeForm(strFrmName);
        }

        public frmDataTransplant(string strFrmName, EnumOperateType pType, Plugin.Application.IAppGISRef pAppGIS,XmlElement dbProjectElement)
        {
            InitializeComponent();
            this.Text = strFrmName;
            m_OpeType = pType;
            m_AppGIS = pAppGIS;
            m_DbProjectElement = dbProjectElement;
            InitializeForm(strFrmName);
        }

        private void InitializeForm(string strFrmName)
        {
            // cyf 20110628 modify :修改界面统一问题
            object[] TagDBType = new object[] { "ESRI文件数据库(*.gdb)", "ESRI个人数据库(*.mdb)" };//"GDB","SDE", "PDB", "ArcSDE(For Oracle)"
            comboBoxOrgType.Items.AddRange(TagDBType);
            comboBoxOrgType.SelectedIndex = 0;
            comBoxType.Items.AddRange(TagDBType);
            comBoxType.SelectedIndex = 0;

            TagDBType = new object[] { "ORACLE", "ACCESS", "SQL" };
            comboBoxParaDBType.Items.AddRange(TagDBType);
            comboBoxParaDBType.SelectedIndex = 1;

            TagDBType = new object[] { "相交包含", "包含", "穿越", "重叠", "相接", "在内", "相交" };
            comboBoxRel.Items.AddRange(TagDBType);
            comboBoxRel.SelectedIndex = 0;

            checkBoxXDelData.Visible = false;

            checkBoxRange.Checked = false;
            groupPanelRange.Enabled = false;
            RadioBtnDrawRange.Checked = true;
            btnDrawRange.Enabled = true;
            btnInputRange.Enabled = false;

            checkBoxFID.Checked = false;
            groupPanelFID.Enabled = false;
            groupPanelOut.Enabled = false;

            groupPanel3.Visible = false;
            
            //设置控件的可用性

            switch (m_OpeType)
            {
                case EnumOperateType.Input:
                    comBoxType.Enabled = false;
                    expandablePanel.Visible = false;

                    if (m_DbProjectElement != null)
                    {
                        try
                        {
                            XmlElement aElement = m_DbProjectElement.SelectSingleNode(".//内容//图幅工作库//连接信息") as XmlElement;
                            FillObjDB(aElement);

                            XmlElement ruleElement = m_DbProjectElement.SelectSingleNode(".//内容//数据操作规则//规则[@类型='图幅数据入工作库']") as XmlElement;
                            //txtXML.Text = ruleElement.GetAttribute("路径");
                        }
                        catch(Exception e)
                        {
                            //*******************************************************************
                            //guozheng added
                            if (ModData.SysLog != null)
                            {
                                ModData.SysLog.Write(e, null, DateTime.Now);
                            }
                            else
                            {
                                ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                                ModData.SysLog.Write(e, null, DateTime.Now);
                            }
                            //********************************************************************

                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统数据库工程XML内容破坏,请检查!");
                            return;
                        }
                    }

                    break;
                case EnumOperateType.UserDBInput:
                    comBoxType.Enabled = true;
                    expandablePanel.Visible = false;

                    if (m_DbProjectElement != null)
                    {
                        try
                        {
                            XmlElement aElement = m_DbProjectElement.SelectSingleNode(".//内容//现势库//连接信息") as XmlElement;
                            FillObjDB(aElement);

                            XmlElement ruleElement = m_DbProjectElement.SelectSingleNode(".//内容//数据操作规则//规则[@类型='批量数据入库']") as XmlElement;
                            //txtXML.Text = ruleElement.GetAttribute("路径");
                        }
                        catch(Exception e)
                        {
                            //*******************************************************************
                            //guozheng added
                            if (ModData.SysLog != null)
                            {
                                ModData.SysLog.Write(e, null, DateTime.Now);
                            }
                            else
                            {
                                ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                                ModData.SysLog.Write(e, null, DateTime.Now);
                            }
                            //********************************************************************

                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统数据库工程XML内容破坏,请检查!");
                            return;
                        }
                    }

                    break;
                case EnumOperateType.Stract:
                    comBoxType.Enabled = false;
                    expandablePanel.Visible = false;
                    //groupPanel.Enabled = false;
                    //groupPanel.Enabled = false;
                    //comboBoxRel.SelectedIndex = 6;
                    //comboBoxRel.Enabled = false;

                    if (m_DbProjectElement != null)
                    {
                        try
                        {
                            XmlElement aElement = m_DbProjectElement.SelectSingleNode(".//内容//图幅工作库//连接信息") as XmlElement;
                            FillObjDB(aElement);

                            aElement = m_DbProjectElement.SelectSingleNode(".//内容//现势库//连接信息") as XmlElement;
                            FillOrgDB(aElement);

                            XmlElement ruleElement = m_DbProjectElement.SelectSingleNode(".//内容//数据操作规则//规则[@类型='图幅接边数据提取']") as XmlElement;
                            //txtXML.Text = ruleElement.GetAttribute("路径");
                        }
                        catch(Exception e)
                        {
                            //*******************************************************************
                            //guozheng added
                            if (ModData.SysLog != null)
                            {
                                ModData.SysLog.Write(e, null, DateTime.Now);
                            }
                            else
                            {
                                ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                                ModData.SysLog.Write(e, null, DateTime.Now);
                            }
                            //********************************************************************

                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统数据库工程XML内容破坏,请检查!");
                            return;
                        }
                    }

                    break;
                case EnumOperateType.OutputUpdateData:
                    checkBoxXDelData.Visible = true;
                    checkBoxXDelData.Checked = true;
                    checkBoxFID.Checked = true;

                    if (m_DbProjectElement != null)
                    {
                        try
                        {
                            XmlElement aElement = m_DbProjectElement.SelectSingleNode(".//内容//更新库//数据库//连接信息") as XmlElement;
                            FillObjDB(aElement);

                            aElement = m_DbProjectElement.SelectSingleNode(".//内容//现势库//连接信息") as XmlElement;
                            FillOrgDB(aElement);

                            aElement = m_DbProjectElement.SelectSingleNode(".//内容//FID记录表//连接信息") as XmlElement;
                            FillFIDInfo(aElement);

                            aElement = m_DbProjectElement.SelectSingleNode(".//内容//更新库//配置库//连接信息") as XmlElement;
                            txtOutFID.Text = aElement.GetAttribute("数据库");

                            XmlElement ruleElement = m_DbProjectElement.SelectSingleNode(".//内容//数据操作规则//规则[@类型='数据更新导出']") as XmlElement;
                            //txtXML.Text = ruleElement.GetAttribute("路径");
                        }
                        catch(Exception e)
                        {
                            //*******************************************************************
                            //guozheng added
                            if (ModData.SysLog != null)
                            {
                                ModData.SysLog.Write(e, null, DateTime.Now);
                            }
                            else
                            {
                                ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                                ModData.SysLog.Write(e, null, DateTime.Now);
                            }
                            //********************************************************************
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统数据库工程XML内容破坏,请检查!");
                            return;
                        }
                    }

                    break;
                default :
                    break;
            }
        }

        private void FillObjDB(XmlElement aElement)
        {
            //cyf 20110628 modify:界面统一
            if (aElement.GetAttribute("类型").Trim() == "PDB")
            {
                comBoxType.Text = "ESRI个人数据库(*.mdb)";
            }
            else if (aElement.GetAttribute("类型").Trim() == "GDB")
            {
                comBoxType.Text = "ESRI文件数据库(*.gdb)";
            }
            else if (aElement.GetAttribute("类型").Trim() == "SDE")
            {
                comBoxType.Text = "ArcSDE(For Oracle)";
            }
            //comBoxType.Text = aElement.GetAttribute("类型");
            txtServer.Text = aElement.GetAttribute("服务器");
            txtInstance.Text = aElement.GetAttribute("服务名");
            txtDB.Text = aElement.GetAttribute("数据库");
            txtVersion.Text = aElement.GetAttribute("版本");
            txtUser.Text = aElement.GetAttribute("用户");
            txtPassword.Text = aElement.GetAttribute("密码");
        }

        private void SaveObjDB(XmlElement aElement)
        {
            aElement.SetAttribute("类型", comBoxType.Tag.ToString().Trim());  //cyf 20110628
            aElement.SetAttribute("服务器", txtServer.Text);
            aElement.SetAttribute("服务名", txtInstance.Text);
            aElement.SetAttribute("数据库", txtDB.Text);
            aElement.SetAttribute("版本", txtVersion.Text);
            aElement.SetAttribute("用户", txtUser.Text);
            aElement.SetAttribute("密码", txtPassword.Text);
        }

        private void FillOrgDB(XmlElement aElement)
        {
            //cyf 20110628 modify:界面统一
            if (aElement.GetAttribute("类型").Trim() == "PDB")
            {
                comboBoxOrgType.Text = "ESRI个人数据库(*.mdb)";
            }
            else if (aElement.GetAttribute("类型").Trim() == "GDB")
            {
                comboBoxOrgType.Text = "ESRI文件数据库(*.gdb)";
            }
            else if (aElement.GetAttribute("类型").Trim() == "SDE")
            {
                comboBoxOrgType.Items.Add("ArcSDE(For Oracle)");
                comboBoxOrgType.Text = "ArcSDE(For Oracle)";
            }
            //comboBoxOrgType.Text = aElement.GetAttribute("类型");
            //end
            //cyf 20110628 modify:界面统一控制
            if (comboBoxOrgType.Text == "ArcSDE(For Oracle)")
            {
                if (aElement.GetAttribute("用户") != "" && aElement.GetAttribute("密码") != "")
                {
                    string strTemp = aElement.GetAttribute("服务器") + "|" + aElement.GetAttribute("服务名") + "|" + aElement.GetAttribute("数据库") + "|" + aElement.GetAttribute("用户") + "|" + aElement.GetAttribute("密码") + "|" + aElement.GetAttribute("版本");
                    ListViewItem aItem = listViewEx.Items.Add(strTemp, strTemp, "");
                    aItem.Tag = comboBoxOrgType.Tag.ToString().Trim(); //cyf 20110628
                    aItem.Checked = true;
                    aItem.ToolTipText = strTemp;
                }
            }
            else
            {
                if (aElement.GetAttribute("数据库") != "")
                {
                    ListViewItem aItem = listViewEx.Items.Add(aElement.GetAttribute("数据库"), aElement.GetAttribute("数据库"), "");
                    aItem.Tag = comboBoxOrgType.Tag.ToString().Trim();//cyf 20110628
                    aItem.Checked = true;
                    aItem.ToolTipText = aElement.GetAttribute("数据库");
                }
            }
        }

        private void FillFIDInfo(XmlElement aElement)
        {
            //cyf 20110628 modify:界面统一
            if (aElement.GetAttribute("类型").Trim() == "PDB")
            {
                comboBoxParaDBType.Text = "ESRI个人数据库(*.mdb)";
            }
            else if (aElement.GetAttribute("类型").Trim() == "GDB")
            {
                comboBoxParaDBType.Text = "ESRI文件数据库(*.gdb)";
            }
            else if (aElement.GetAttribute("类型").Trim() == "SDE")
            {
                comboBoxParaDBType.Text = "ArcSDE(For Oracle)";
            }
            //comboBoxParaDBType.Text=aElement.GetAttribute("类型");
            textBoxXServer.Text = aElement.GetAttribute("数据库");
            textBoxXInstance.Text = aElement.GetAttribute("服务名");
            textBoxXUser.Text = aElement.GetAttribute("用户");
            textBoxXPassword.Text = aElement.GetAttribute("密码");
            textBoxXTableName.Text = (aElement.FirstChild as XmlElement).GetAttribute("名称");
        }

        private void comBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtServer.Text = "";
            txtInstance.Text = "";
            txtDB.Text = "";
            txtVersion.Text = "";
            txtUser.Text = "";
            txtPassword.Text = "";
            //cyf 20110628 modify :统一界面控制  "ESRI文件数据库(FGDB)", "ESRI个人数据库(PGDB)", "ArcSDE(For Oracle)" 
            if (comBoxType.Text == "ESRI文件数据库(*.gdb)")
            {
                comBoxType.Tag = "GDB";
            }
            else if (comBoxType.Text == "ESRI个人数据库(*.mdb)")
            {
                comBoxType.Tag = "PDB";
            }
            else if (comBoxType.Text == "ArcSDE(For Oracle)")
            {
                comBoxType.Tag = "SDE";
            }
            //end
            if (comBoxType.Text!= "ArcSDE(For Oracle)")
            {
                btnDB.Visible = true;
                txtDB.Size = new Size(txtServer.Size.Width - btnDB.Size.Width, txtDB.Size.Height);
                txtServer.Enabled = false;
                txtInstance.Enabled = false;
                txtUser.Enabled = false;
                txtPassword.Enabled = false;
                txtVersion.Enabled = false;
            }
            else
            {
                btnDB.Visible = false;
                txtDB.Size = new Size(txtServer.Size.Width, txtDB.Size.Height);
                txtServer.Enabled = true;
                txtInstance.Enabled = true;
                txtUser.Enabled = true;
                txtPassword.Enabled = true;
                txtVersion.Enabled = true;
               
            }
        }

        private void checkBoxRange_CheckedChanged(object sender, EventArgs e)
        {
            groupPanelRange.Enabled = !groupPanelRange.Enabled;
        }

        private void checkBoxFID_CheckedChanged(object sender, EventArgs e)
        {
            groupPanelFID.Enabled = !groupPanelFID.Enabled;
            groupPanelOut.Enabled = !groupPanelOut.Enabled;
        }

        private void comboBoxParaDBType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxParaDBType.Text == "ACCESS")
            {
                btnFile.Visible = true;
                textBoxXServer.Size = new Size(textBoxXUser.Size.Width - btnFile.Size.Width, textBoxXServer.Size.Height);
                labelX12.Text= "数 据 库：";
                textBoxXInstance.Enabled = false;
                textBoxXUser.Enabled = false;
                textBoxXPassword.Enabled = false;
            }
            else
            {
                btnFile.Visible = false;
                textBoxXServer.Size = new Size(textBoxXUser.Size.Width, textBoxXServer.Size.Height);
                labelX12.Text = "服 务 名：";
                textBoxXInstance.Enabled = true;
                textBoxXUser.Enabled = true;
                textBoxXPassword.Enabled = true;

            }
        }

        private void RadioBtnDrawRange_CheckedChanged(object sender, EventArgs e)
        {
            btnDrawRange.Enabled = RadioBtnDrawRange.Checked;
        }

        private void RadioBtnInputRange_CheckedChanged(object sender, EventArgs e)
        {
            btnInputRange.Enabled = RadioBtnInputRange.Checked;
        }


        //选择源数据

        private void btnOrg_Click(object sender, EventArgs e)
        {
            //cyf 2011068 modify:界面统一控制
            if (comboBoxOrgType.Text== "ArcSDE(For Oracle)")
            {
               // frmDBPropertySet frmSet = new frmDBPropertySet("设置SDE连接", comboBoxOrgType.Text.Trim());
                frmXZDBPropertySet frmSet = new frmXZDBPropertySet();
                frmSet.ShowDialog();
                if (frmSet.Res == true)
                {
                    if (!listViewEx.Items.ContainsKey(frmSet.GetPropertySetStr))
                    {
                        ListViewItem aItem = listViewEx.Items.Add(frmSet.GetPropertySetStr, frmSet.GetPropertySetStr, "");
                        aItem.Tag = comboBoxOrgType.Tag.ToString().Trim();  //cyf 20110628
                        aItem.Checked = true;
                        aItem.ToolTipText = frmSet.GetPropertySetStr;
                    }
                    else
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "列表中已添加该项!");
                    }
                }
            }

            else if (comboBoxOrgType.Text == "ESRI文件数据库(*.gdb)")
            {
                FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    if (pFolderBrowser.SelectedPath.EndsWith(".gdb"))
                    {
                        if (!listViewEx.Items.ContainsKey(pFolderBrowser.SelectedPath))
                        {
                            ListViewItem aItem = listViewEx.Items.Add(pFolderBrowser.SelectedPath, pFolderBrowser.SelectedPath, "");
                            aItem.Tag = comboBoxOrgType.Tag.ToString().Trim();// "ESRI文件数据库(FGDB)"; cyf 20110628
                            aItem.Checked = true;
                            aItem.ToolTipText = pFolderBrowser.SelectedPath;
                        }
                        else
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "列表中已添加该项!");
                        }
                    }
                    else
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择GDB格式文件!");
                    }
                }
            }
            else if (comboBoxOrgType.Text == "ESRI个人数据库(*.mdb)")
            {
                OpenFileDialog OpenFile = new OpenFileDialog();
                OpenFile.CheckFileExists = true;
                OpenFile.CheckPathExists = true;
                OpenFile.Title = "选择ESRI个人数据库";
                OpenFile.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
                OpenFile.Multiselect = true;
                if (OpenFile.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < OpenFile.FileNames.Length; i++)
                    {
                        if (!listViewEx.Items.ContainsKey(OpenFile.FileNames[i]))
                        {
                            ListViewItem aItem = listViewEx.Items.Add(OpenFile.FileNames[i], OpenFile.FileNames[i], "");
                            aItem.Tag = comboBoxOrgType.Tag.ToString().Trim();// "ESRI个人数据库(PGDB)";cyf 20110628
                            aItem.Checked = true;
                            aItem.ToolTipText = OpenFile.FileNames[i];
                        }
                        else
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "列表中已添加该项!");
                        }
                    }
                }
            }
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem aItem in listViewEx.Items)
            {
                aItem.Checked = true;
            }
        }

        private void btnSelReverse_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem aItem in listViewEx.Items)
            {
                aItem.Checked = !aItem.Checked;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            listViewEx.Items.Clear();
        }

        private void btnXML_Click(object sender, EventArgs e)
        {
            //OpenFileDialog OpenFile = new OpenFileDialog();
            //OpenFile.CheckFileExists = true;
            //OpenFile.CheckPathExists = true;
            //OpenFile.Title = "选择映射文件";
            //OpenFile.Filter = "映射文件(*.xml)|*.xml";
            //if (OpenFile.ShowDialog() == DialogResult.OK)
            //{
            //    txtXML.Text = OpenFile.FileName;
            //}
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //cyf 20110628 modify:界面统一控制
            if (comBoxType.Text== "ArcSDE(For Oracle)")
            {
                if (txtUser.Text == "" || txtPassword.Text == "")
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请设置目标数据连接!");
                    return;
                }
            }
            else
            {
                if (txtDB.Text == "")
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请设置目标数据连接!");
                    return;
                }
                if (comBoxType.Text == "ESRI文件数据库(*.gdb)")
                {
                    if (!txtDB.Text.Contains(".gdb"))
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "目标数据连接，请选择ESRI文件数据库！");
                        return;
                    }
                }
            }

            if (listViewEx.Items.Count == 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请设置源数据连接!");
                return;
            }

            //=======================================================
            //chenyafei   20100106  modify :    图幅批量规则设置
            string fileXml = "";
            if (m_OpeType == EnumOperateType.Input)
            {
                //图幅数据入库
                fileXml = ModData.DBTufuInputXml;
            }
            else if (m_OpeType == EnumOperateType.Stract)
            {
                //获取图幅接边要素
                fileXml = ModData.DBTufuStractXml;

                ModData.v_AppGIS.ArcGisMapControl.ClearLayers();
                //将GDB数据加载到地图
                IWorkspaceFactory pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                //打开工作空间并遍历数据集
                IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(txtDB.Text, 0);
                IEnumDataset pEnumDataset = pWorkspace.get_Datasets(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureDataset);
                pEnumDataset.Reset();
                IDataset pDataset = pEnumDataset.Next();
                //如果数据集是IFeatureDataset,则遍历它下面的子类
                if (pDataset is IFeatureDataset)
                {
                    IFeatureDataset pFeatureDataset = pDataset as IFeatureDataset;
                    IEnumDataset pEnumDataset1 = pFeatureDataset.Subsets;
                    pEnumDataset1.Reset();
                    IDataset pDataset1 = pEnumDataset1.Next();
                    //如果子类是FeatureClass，则添加到axMapControl1中
                    while(pDataset1!=null)
                    {
                        IFeatureLayer pFeatureLayer = new FeatureLayerClass();
                        pFeatureLayer.FeatureClass = pDataset1 as IFeatureClass;
                        pFeatureLayer.Name = pFeatureLayer.FeatureClass.AliasName;
                        ModData.v_AppGIS.ArcGisMapControl.Map.AddLayer(pFeatureLayer);
                        pDataset1 = pEnumDataset1.Next();
                    }
                }
                ModData.v_AppGIS.ArcGisMapControl.ActiveView.Refresh();
            }

            //===============================================================

            if (!File.Exists(fileXml))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "对照关系文件不存在!\n"+fileXml);
                return;
            }

            if (m_OpeType == EnumOperateType.OutputUpdateData)
            {
                //更新数据导出
                if (!checkBoxFID.Checked)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请设置业务维护信息表连接!");
                    return;
                }
            }

            //判断FID记录表是否连接上
            Exception err=null;
            DataTable pDataTable = null;
            SysCommon.DataBase.SysDataBase pSysDB = null;
            IWorkspace TempWorkSpace = null;
            if (m_OpeType == EnumOperateType.OutputUpdateData)
            {
                string connectionStr = "";
                pSysDB = new SysCommon.DataBase.SysDataBase();
                connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + txtOutFID.Text + ";Mode=Share Deny None;Persist Security Info=False";
                pSysDB.SetDbConnection(connectionStr, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out err);
                if(err!=null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择业务维护信息内容输出路径!");
                    return;
                }

                SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();
               
                switch(comboBoxParaDBType.Text)
                {
                    case "ORACLE":
                        connectionStr = "Data Source=" + textBoxXServer.Text + ";Persist Security Info=True;User ID=" + textBoxXUser.Text + ";Password=" + textBoxXPassword.Text + ";Unicode=True";
                        pSysTable.SetDbConnection(connectionStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out err);
                        break;
                    case "ACCESS":
                        connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + textBoxXServer.Text + ";Mode=Share Deny None;Persist Security Info=False";
                        pSysTable.SetDbConnection(connectionStr, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out err);
                        break;
                    case "SQL":
                        connectionStr = "Data Source=" + textBoxXServer.Text + ";Initial Catalog=" + textBoxXInstance.Text + ";User ID=" + textBoxXUser.Text + ";Password=" + textBoxXPassword.Text;
                        pSysTable.SetDbConnection(connectionStr, SysCommon.enumDBConType.SQL, SysCommon.enumDBType.SQLSERVER, out err);
                        break;
                }

                pDataTable=pSysTable.GetTable(textBoxXTableName.Text, out err);
                pSysTable.CloseDbConnection();
                if (err != null || pDataTable == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接业务维护信息表失败!");
                    return;
                }

                pDataTable.TableName = textBoxXTableName.Text;
            }

            Plugin.Application.IAppFormRef pAppFormRef = m_AppGIS as Plugin.Application.IAppFormRef;
            pAppFormRef.OperatorTips = "获取数据信息...";

            //根据窗体设置形成数据操作xml内容
            XmlDocument xmlDoc=new XmlDocument();
            xmlDoc.LoadXml("<数据移植></数据移植>");
            XmlNode sourceNode = ModDBOperator.SelectNode(xmlDoc.DocumentElement as XmlNode, "源数据连接");
            foreach (ListViewItem aItem in listViewEx.Items)
            {
                if (aItem.Checked)
                {
                    XmlElement dataElement = xmlDoc.CreateElement("源数据");
                    dataElement.SetAttribute("类型", aItem.Tag.ToString());
                    if (aItem.Tag.ToString() != "SDE")//"ArcSDE(For Oracle)"  cyf 20110628
                    {
                        dataElement.SetAttribute("服务器", "");
                        dataElement.SetAttribute("服务名", "");
                        dataElement.SetAttribute("数据库", aItem.Text);
                        dataElement.SetAttribute("用户", "");
                        dataElement.SetAttribute("密码", "");
                        dataElement.SetAttribute("版本", "");
                    }
                    else
                    {
                        string[] strTemp=aItem.Text.Split('|');
                        dataElement.SetAttribute("服务器", strTemp[0]);
                        dataElement.SetAttribute("服务名", strTemp[1]);
                        dataElement.SetAttribute("数据库", strTemp[2]);
                        dataElement.SetAttribute("用户", strTemp[3]);
                        dataElement.SetAttribute("密码", strTemp[4]);
                        dataElement.SetAttribute("版本", strTemp[5]);
                    }
                    sourceNode.AppendChild((XmlNode)dataElement);
                }
            }

            XmlElement objElementTemp = ModDBOperator.SelectNode(xmlDoc.DocumentElement as XmlNode, "目标数据连接") as XmlElement;
            objElementTemp.SetAttribute("类型", comBoxType.Tag.ToString().Trim());  //cyf 20110628
            objElementTemp.SetAttribute("服务器", txtServer.Text.Trim());
            objElementTemp.SetAttribute("服务名", txtInstance.Text.Trim());
            objElementTemp.SetAttribute("数据库", txtDB.Text.Trim());
            objElementTemp.SetAttribute("用户", txtUser.Text.Trim());
            objElementTemp.SetAttribute("密码", txtPassword.Text.Trim());
            objElementTemp.SetAttribute("版本", txtVersion.Text.Trim());

            XmlElement RuleElement = ModDBOperator.SelectNode(xmlDoc.DocumentElement as XmlNode, "规则") as XmlElement;
            RuleElement.SetAttribute("路径", fileXml);

            //检查规则是否需要创建图层

            XmlDocument rulexmlDoc = new XmlDocument();
            try
            {
                rulexmlDoc.Load(fileXml);
            }
            catch(Exception er)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(er, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(er, null, DateTime.Now);
                }
                //********************************************************************

                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "选择的对照关系文件无法加载,请检查!");
                return;
            }
            XmlElement targetNode = rulexmlDoc.SelectSingleNode(".//目标库体结构") as XmlElement;
            if (targetNode != null)
            {
                TempWorkSpace = ModDBOperator.GetDBInfoByXMLNode(objElementTemp, "") as IWorkspace;
                if (TempWorkSpace == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接目标数据失败!");
                    return;
                }

                IFeatureWorkspace pFeatureWorkSpace = TempWorkSpace as IFeatureWorkspace;
                int intScale = -1;
                try
                {
                    intScale = Convert.ToInt32(targetNode.GetAttribute("比例尺").Trim());
                }
                catch
                {
                   
                }

                if (intScale == -1)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "映射规则中目标库体结构未设置比例尺!");
                    return;
                }
                
                if (!ModDBOperator.createFeatureClass(rulexmlDoc, pFeatureWorkSpace, intScale))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建目标数据图层失败!");
                    return;
                }
            }
            
            //数据导出用于更新时需先删除现势库中数据和FID记录表内容

            if (m_OpeType == EnumOperateType.OutputUpdateData && pSysDB != null && checkBoxXDelData.Checked == true)
            {
                TempWorkSpace = ModDBOperator.GetDBInfoByXMLNode(objElementTemp, "") as IWorkspace;
                if (TempWorkSpace == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接更新环境库失败!");
                    return;
                }

                pAppFormRef.OperatorTips = "删除更新环境相关数据...";
                SysCommon.Gis.SysGisDataSet pSysGisDataSet = new SysCommon.Gis.SysGisDataSet(TempWorkSpace);
                pSysGisDataSet.StartWorkspaceEdit(false, out err);
                if (err != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("删除更新环境相关数据出错", "出错原因:" + err.Message);
                    return;
                }
                List<string> listFC=pSysGisDataSet.GetAllFeatureClassNames(false);
                foreach (string strFc in listFC)
                {
                    if (pSysGisDataSet.DeleteRows(strFc,"", out err) == false) break;
                }
                if (err != null)
                {
                    pSysGisDataSet.EndWorkspaceEdit(false, out err);
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("删除更新环境相关数据出错", "出错原因:" + err.Message);
                    return;
                }
                pSysGisDataSet.EndWorkspaceEdit(true, out err);
                pSysGisDataSet.Dispose();
                if (err != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("删除更新环境相关数据出错", "出错原因:" + err.Message);
                    return;
                }

                pSysDB.UpdateTable("delete from FID记录表", out err);
                if (err != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("删除更新环境相关数据出错", "出错原因:" + err.Message);
                    return;
                }
            }

            pAppFormRef.OperatorTips = "初始化数据处理树图...";
            //初始化数据处理树图

            InitialDBTree newInitialDBTree = new InitialDBTree();
            newInitialDBTree.OnCreateDataTree(m_AppGIS.DataTree, xmlDoc);
            if ((bool)m_AppGIS.DataTree.Tag == false) return;

            //进行数据移植
            this.Visible = false;
            this.Hide();
            pAppFormRef.OperatorTips = "进行"+this.Text+"...";
            //--------------------------------------------------
            # region 读取xml获取Geometry用于相交要素提取
            if (m_OpeType == EnumOperateType.Stract)
            {
                XmlNode ProNode =m_AppGIS.ProjectTree.SelectedNode.Tag as XmlNode;
                XmlElement workDBElem = ProNode.SelectSingleNode(".//内容//图幅工作库//范围信息") as XmlElement;
                string rangeStr = workDBElem.GetAttribute("范围").Trim();
                byte[] xmlByte = Convert.FromBase64String(rangeStr);
                object pGeo = new PolygonClass();
                if (XmlDeSerializer(xmlByte, pGeo) == false)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "几何范围解析出错！");
                    return;
                }
                m_Geometry = pGeo as IGeometry;
            }
            #endregion
            if (m_Geometry != null)
            {
                esriSpatialRelEnum RelEnum = esriSpatialRelEnum.esriSpatialRelUndefined;
                string RelDes;
                if (checkBoxRange.Checked)
                {
                    switch (comboBoxRel.Text)
                    {
                        case "相交包含":
                            RelEnum = esriSpatialRelEnum.esriSpatialRelIntersects;
                            m_DataThread = new clsDataThread(m_AppGIS, m_Geometry, RelEnum, true, pDataTable, pSysDB, m_OpeType, null, "");
                            break;
                        case "包含":
                            RelEnum = esriSpatialRelEnum.esriSpatialRelContains;
                            m_DataThread = new clsDataThread(m_AppGIS, m_Geometry, RelEnum, true, pDataTable, pSysDB, m_OpeType, null, "");
                            break;
                        case "穿越":
                            RelEnum = esriSpatialRelEnum.esriSpatialRelCrosses;
                            m_DataThread = new clsDataThread(m_AppGIS, m_Geometry, RelEnum, true, pDataTable, pSysDB, m_OpeType, null, "");
                            break;
                        case "重叠":
                            RelEnum = esriSpatialRelEnum.esriSpatialRelOverlaps;
                            m_DataThread = new clsDataThread(m_AppGIS, m_Geometry, RelEnum, true, pDataTable, pSysDB, m_OpeType, null, "");
                            break;
                        case "相接":
                            RelEnum = esriSpatialRelEnum.esriSpatialRelTouches;
                            m_DataThread = new clsDataThread(m_AppGIS, m_Geometry, RelEnum, true, pDataTable, pSysDB, m_OpeType, null, "");
                            break;
                        case "在内":
                            RelEnum = esriSpatialRelEnum.esriSpatialRelWithin;
                            m_DataThread = new clsDataThread(m_AppGIS, m_Geometry, RelEnum, true, pDataTable, pSysDB, m_OpeType, null, "");
                            break;
                        case "相交":
                            RelEnum = esriSpatialRelEnum.esriSpatialRelRelation;
                            RelDes = "TT*******";
                            m_DataThread = new clsDataThread(m_AppGIS, m_Geometry, RelDes, true, pDataTable, pSysDB, m_OpeType);
                            break;
                    }
                }
                else if (m_OpeType == EnumOperateType.Stract)
                {
                    //获取相交要素
                    //RelEnum = esriSpatialRelEnum.esriSpatialRelRelation;
                    //RelDes = "T**T*****";
                    //m_DataThread = new clsDataThread(m_AppGIS, m_Geometry, RelDes, true, pDataTable, pSysDB, m_OpeType);

                    RelEnum = esriSpatialRelEnum.esriSpatialRelIntersects;
                    m_DataThread = new clsDataThread(m_AppGIS, m_Geometry, RelEnum, true, pDataTable, pSysDB, m_OpeType, null, "");
                }
            }
            else
                m_DataThread = new clsDataThread(m_AppGIS, null, "", true, pDataTable, pSysDB, m_OpeType);

            Thread aThread = new Thread(new ThreadStart(m_DataThread.DoBatchWorks));
            m_DataThread.CurrentThread = aThread;
            m_AppGIS.CurrentThread = aThread;
            aThread.Start();

            //利用计时器刷新mapcontrol
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 800;
            _timer.Enabled = true;
            _timer.Tick += new EventHandler(Timer_Tick);
        }

          //利用计时器刷新mapcontrol
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (m_DataThread.CurrentThread.ThreadState == ThreadState.Stopped)
            {
                if (m_DataThread.Res == true)
                {
                    SaveToXML();
                    this.Close();
                }
                else
                    this.Show();

                m_DataThread = null;
                _timer.Enabled = false;
            }
        }

        private void SaveToXML()
        {
            if (m_DbProjectElement == null) return;

            switch (m_OpeType)
            {
                case EnumOperateType.Input:
                    try
                    {
                        XmlElement aElement = m_DbProjectElement.SelectSingleNode(".//内容//图幅工作库//连接信息") as XmlElement;
                        SaveObjDB(aElement);

                        XmlElement ruleElement = m_DbProjectElement.SelectSingleNode(".//内容//数据操作规则//规则[@类型='图幅数据入工作库']") as XmlElement;
                        ruleElement.SetAttribute("路径",ModData.DBTufuInputXml);
                        ruleElement.OwnerDocument.Save(ModData.v_projectXML);
                    }
                    catch
                    {
                        return;
                    }
                    break;
                case EnumOperateType.UserDBInput:
                    try
                    {
                        XmlElement ruleElement = m_DbProjectElement.SelectSingleNode(".//内容//数据操作规则//规则[@类型='批量数据入库']") as XmlElement;
                        ruleElement.SetAttribute("路径", ModData.DBImportPath);
                        ruleElement.OwnerDocument.Save(ModData.v_projectXML);
                    }
                    catch
                    {
                        return;
                    }
                    break;
                case EnumOperateType.Stract:
                    try
                    {
                        XmlElement ruleElement = m_DbProjectElement.SelectSingleNode(".//内容//数据操作规则//规则[@类型='图幅接边数据提取']") as XmlElement;
                        ruleElement.SetAttribute("路径",ModData.DBTufuStractXml);
                        ruleElement.OwnerDocument.Save(ModData.v_projectXML);
                    }
                    catch
                    {
                        return;
                    }
                    break;
                case EnumOperateType.OutputUpdateData:
                    try
                    {
                        XmlElement ruleElement = m_DbProjectElement.SelectSingleNode(".//内容//数据操作规则//规则[@类型='数据更新导出']") as XmlElement;
                        //ruleElement.SetAttribute("路径", txtXML.Text);
                        ruleElement.OwnerDocument.Save(ModData.v_projectXML);
                    }
                    catch
                    {
                        return;
                    }
                    break;
                default:
                    break;
            }
        }


        private void btnDB_Click(object sender, EventArgs e)
        {
            //cyf 20110628 modify :界面统一控制
            if (comBoxType.Text == "ArcSDE(For Oracle)")
            { }
            else if (comBoxType.Text == "ESRI文件数据库(*.gdb)")
            {
                /*FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    //cyf 20110706 modify
                    string pFileName = pFolderBrowser.SelectedPath;
                    pFileName = pFileName.Substring(pFileName.LastIndexOf('\\') + 1);
                    txtDB.Text = pFolderBrowser.SelectedPath +"\\"+ pFileName+".gdb";
                    //end
                }*/
                //changed by xisheng   20110729

                if (this.Text == "获取相交要素")
                {
                    FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                    if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        //cyf 20110706 modify
                        if (pFolderBrowser.SelectedPath.EndsWith(".gdb"))
                        txtDB.Text = pFolderBrowser.SelectedPath;
                    }
                }
                else
                {
                    SaveFileDialog dlg = new SaveFileDialog();
                    dlg.FileName = "";
                    dlg.Filter = "ESRI文件数据库(.gdb)|*.gdb";
                    if (dlg.ShowDialog() == DialogResult.Cancel) return;

                    this.txtDB.Text = dlg.FileName;
                }
				
            
			 }          
			 else if (comBoxType.Text == "ESRI个人数据库(*.mdb)")
            {
                OpenFileDialog OpenFile = new OpenFileDialog();
                OpenFile.Title = "选择ESRI个人数据库";
                OpenFile.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
                if (OpenFile.ShowDialog() == DialogResult.OK)
                {
                    txtDB.Text = OpenFile.FileName;
                }
            }
        }

        private void btnDrawRange_Click(object sender, EventArgs e)
        {
            DrawPolygonToolClass drawPolygon = new DrawPolygonToolClass(true,this);
            drawPolygon.OnCreate(m_AppGIS.MapControl);
            m_AppGIS.MapControl.CurrentTool = drawPolygon as ITool;
            m_AppGIS.CurrentTool = drawPolygon.Caption;
        }

        private void btnInputRange_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.CheckFileExists = true;
            OpenFile.CheckPathExists = true;
            OpenFile.Title = "选择图形范围坐标txt";
            OpenFile.Filter = "图形范围坐标文本(*.txt)|*.txt";
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                try
                {
                    StreamReader sr = new StreamReader(OpenFile.FileName);
                    while (sr.Peek() >= 0) 
                    {
                        string[] strTemp=sr.ReadLine().Split(',');
                        if(sb.Length!=0)
                        {
                            sb.Append(",");
                        }
                        sb.Append(strTemp[0] + "@" + strTemp[1]);
                    }
                }
                catch(Exception er)
                {
                    //*******************************************************************
                    //guozheng added
                    if (ModData.SysLog != null)
                    {
                        ModData.SysLog.Write(er, null, DateTime.Now);
                    }
                    else
                    {
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                        ModData.SysLog.Write(er, null, DateTime.Now);
                    }
                    //********************************************************************

                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "图形范围坐标txt格式不正确!\n文本每行为点坐标且以','分割");
                    return;
                }

                if(sb.Length==0) return;
                m_Geometry=ModDBOperator.GetPolygonByCol(sb.ToString()) as IGeometry;
            }
        }

        private void btnOutFID_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "输出对应FID记录内容到MDB中";
            OpenFile.Filter = "access数据(*.mdb)|*.mdb";
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                txtOutFID.Text = OpenFile.FileName;

                Exception err = null;
                SysCommon.DataBase.SysTable pSysTable = new SysCommon.DataBase.SysTable();
                string connectionStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + txtOutFID.Text + ";Mode=Share Deny None;Persist Security Info=False";
                pSysTable.SetDbConnection(connectionStr, SysCommon.enumDBConType.OLEDB, SysCommon.enumDBType.ACCESS, out err);
                if (err != null)
                {
                    txtOutFID.Text = "";
                    return;
                }
                DataTable pDataTable = pSysTable.GetTable(textBoxXTableName.Text, out err);
                pSysTable.CloseDbConnection();
                if (err != null || pDataTable == null)
                {
                    txtOutFID.Text = "";
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "选择的MDB不存在表" + textBoxXTableName.Text + "!");
                    return;
                }

                
            }
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "选择MDB数据";
            OpenFile.Filter = "MDB数据(*.mdb)|*.mdb";
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                textBoxXServer.Text = OpenFile.FileName;
            }
        }

         /// <summary>
        /// 将xmlByte解析为obj
        /// </summary>
        /// <param name="xmlByte"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool XmlDeSerializer(byte[] xmlByte, object obj)
        {
            try
            {
                //判断字符串是否为空

                if (xmlByte != null)
                {
                    ESRI.ArcGIS.esriSystem.IPersistStream pStream = obj as ESRI.ArcGIS.esriSystem.IPersistStream;

                    ESRI.ArcGIS.esriSystem.IXMLStream xmlStream = new ESRI.ArcGIS.esriSystem.XMLStreamClass();

                    xmlStream.LoadFromBytes(ref xmlByte);
                    pStream.Load(xmlStream as ESRI.ArcGIS.esriSystem.IStream);

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private void comboBoxOrgType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cyf 20110628 modify :统一界面控制  "ESRI文件数据库(FGDB)", "ESRI个人数据库(PGDB)", "ArcSDE(For Oracle)" 
            if (comboBoxOrgType.Text == "ESRI文件数据库(*.gdb)")
            {
                comboBoxOrgType.Tag = "GDB";
            }
            else if (comboBoxOrgType.Text == "ESRI个人数据库(*.mdb)")
            {
                comboBoxOrgType.Tag = "PDB";
            }
            else if (comboBoxOrgType.Text == "ArcSDE(For Oracle)")
            {
                comboBoxOrgType.Tag = "SDE";
            }
            //end
        }
    }


    public class DrawPolygonToolClass : BaseTool
    {
        private IHookHelper m_hookHelper;
        private IMapControlDefault m_MapControl;
        private INewPolygonFeedback m_pNewPolygonFeedback;

        private frmDataTransplant m_mainFrm=null;
        private FrmGetRange m_GetRangeForm=null;
        private FrmDataStract m_FrmDataStract = null;
        private FrmRasterDataStract m_FrmRasterDataStract = null;
        private bool m_DoBuf;
       
        //类的方法
        public DrawPolygonToolClass(bool bDoBuf,frmDataTransplant mainFrm)
        {
            m_mainFrm = mainFrm;
            m_DoBuf = bDoBuf;
            base.m_category = "GeoCommon";
            base.m_caption = "DrawPolygon";
            base.m_message = "画多边形面";
            base.m_toolTip = "画多边形面";
            base.m_name = base.m_category + "_" + base.m_caption;
            try
            {
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), "Resources.draw.cur");
            }
            catch(Exception e)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
            }
        }
        public DrawPolygonToolClass(bool bDoBuf, FrmGetRange mainFrm)
        {
            m_GetRangeForm = mainFrm;
            m_DoBuf = bDoBuf;
            base.m_category = "GeoCommon";
            base.m_caption = "DrawPolygon";
            base.m_message = "画多边形面";
            base.m_toolTip = "画多边形面";
            base.m_name = base.m_category + "_" + base.m_caption;
            try
            {
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), "Resources.draw.cur");
            }
            catch(Exception e)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
            }
        }
        public DrawPolygonToolClass(bool bDoBuf, FrmDataStract mainFrm)
        {
            m_FrmDataStract = mainFrm;
            m_DoBuf = bDoBuf;
            base.m_category = "GeoCommon";
            base.m_caption = "DrawPolygon";
            base.m_message = "画多边形面";
            base.m_toolTip = "画多边形面";
            base.m_name = base.m_category + "_" + base.m_caption;
            try
            {
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), "Resources.draw.cur");
            }
            catch(Exception e)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
            }
        }
        public DrawPolygonToolClass(bool bDoBuf, FrmRasterDataStract mainFrm)
        {
            m_FrmRasterDataStract = mainFrm;
            m_DoBuf = bDoBuf;
            base.m_category = "GeoCommon";
            base.m_caption = "DrawPolygon";
            base.m_message = "画多边形面";
            base.m_toolTip = "画多边形面";
            base.m_name = base.m_category + "_" + base.m_caption;
            try
            {
                base.m_cursor = new System.Windows.Forms.Cursor(GetType(), "Resources.draw.cur");
            }
            catch(Exception e)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
            }
        }
        #region Overriden Class Methods

        /// <summary>
        /// Occurs when this tool is created
        /// </summary>
        /// <param name="hook">Instance of the application</param>
        public override void OnCreate(object hook)
        {
            if (m_hookHelper == null)
                m_hookHelper = new HookHelperClass();

            m_hookHelper.Hook = hook;
            m_MapControl = hook as IMapControlDefault;
            if (m_mainFrm != null)
            {
                m_mainFrm.Hide();
            }
            if(m_FrmDataStract!=null)
            {
                m_FrmDataStract.Hide();
            }
            if(m_FrmRasterDataStract!=null)
            {
                m_FrmRasterDataStract.Hide();
            }
        }

        /// <summary>
        /// Occurs when this tool is clicked
        /// </summary>
        public override void OnClick()
        {
            m_MapControl.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewBackground, null, m_MapControl.ActiveView.Extent);
        }

        public override void OnMouseDown(int Button, int Shift, int X, int Y)
        {
            if (Button != 1) return;

            IPoint pPoint = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
            if (m_pNewPolygonFeedback == null)  //第一次按下

            {
                IRgbColor pRGB = new RgbColorClass();
                ISimpleFillSymbol pSimpleFillSymbol = new SimpleFillSymbolClass();
                pRGB.Red = 15;
                pRGB.Blue = 15;
                pRGB.Green = 0;
                pRGB.Transparency = 80;
                pSimpleFillSymbol.Color = pRGB;
                pSimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid;

                m_pNewPolygonFeedback = new NewPolygonFeedbackClass();
                m_pNewPolygonFeedback.Symbol = pSimpleFillSymbol as ISymbol;
                m_pNewPolygonFeedback.Display = m_MapControl.ActiveView.ScreenDisplay;

                m_pNewPolygonFeedback.Start(pPoint);
            }
            else                            //将点加入
            {
                m_pNewPolygonFeedback.AddPoint(pPoint);
            }
        }

        //鼠标移动， 将工具也移动到点的位置

        public override void OnMouseMove(int Button, int Shift, int X, int Y)
        {
            base.OnMouseMove(Button, Shift, X, Y);

            IPoint pPoint = m_MapControl.ActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);

            m_pNewPolygonFeedback.MoveTo(pPoint);
        }

        //双击则创建该线，并弹出缓冲窗体

        public override void OnDblClick()
        {
            //获取折线 并获取当前视图的屏幕显示
            if (m_pNewPolygonFeedback == null) return;
            IPolygon pPolygon = m_pNewPolygonFeedback.Stop();
            m_pNewPolygonFeedback = null;

            //不存在，为空。尺寸不够均退出

            if (pPolygon == null || pPolygon.IsEmpty) return;
            if (pPolygon.Envelope.Width < 0.01 || pPolygon.Envelope.Height < 0.01) return;

            //创建Topo对象，简化后统一空间参考

            ITopologicalOperator pTopo = (ITopologicalOperator)pPolygon;
            pTopo.Simplify();
            pPolygon.Project(m_MapControl.Map.SpatialReference);

            IGeometry pGeometry = pPolygon as IGeometry;
            if (pGeometry == null) return;

            if (m_DoBuf)
            {
                GeoUtilities.frmBufferSet pFrmBufSet = new GeoUtilities.frmBufferSet(pGeometry, m_MapControl.Map, m_mainFrm);
                pGeometry = pFrmBufSet.GetBufferGeometry();
                if (pGeometry == null || pFrmBufSet.Res == false) return;
            }
            if (m_mainFrm != null)
            {
                m_mainFrm.DrawGeometry = pGeometry;
                m_mainFrm.Show();
            }
            if (m_GetRangeForm != null)
            {
                m_GetRangeForm.DrawGeometry = pGeometry;
                m_GetRangeForm.Show();
            }
            if (m_FrmDataStract != null)
            {
                //m_FrmDataStract.DrawGeometry = pGeometry;

                //解析几何范围成字符串
                //将范围信息解析后写入XML中

                //byte[] xmlByte = xmlSerializer(pGeometry);
                //string base64String = Convert.ToBase64String(xmlByte);

                //添加行

                DataGridViewRow dgRow = new DataGridViewRow();
                dgRow.CreateCells(m_FrmDataStract.dgRange);
                //第一列

                dgRow.Cells[0].Value = true;// 是否选用
                //第二列

                string pCount=(m_FrmDataStract.dgRange.RowCount+1).ToString();//cyf 20110711 modify
                dgRow.Cells[1].Value = "第" + pCount + "个图形";// base64String;
                dgRow.Cells[1].Tag = pGeometry;

                m_FrmDataStract.dgRange.Rows.Add(dgRow);

                //m_FrmDataStract.dgRange.Tag = pGeometry;
                m_FrmDataStract.Show();
                m_MapControl.CurrentTool = null;
            }
            if (m_FrmRasterDataStract != null)
            {
                //添加行
                DataGridViewRow dgRow = new DataGridViewRow();
                dgRow.CreateCells(m_FrmRasterDataStract.dgRange);
                //第一列
                dgRow.Cells[0].Value = true;// 是否选用
                //第二列
                string pCount = m_FrmRasterDataStract.dgRange.RowCount.ToString();
                dgRow.Cells[1].Value = "第" + pCount + "个图形";// base64String;
                dgRow.Cells[1].Tag = pGeometry;
                m_FrmRasterDataStract.dgRange.Rows.Add(dgRow);
                m_FrmRasterDataStract.Show();
                m_MapControl.CurrentTool = null;
            }
        }

        public override void Refresh(int hDC)
        {
            if (m_pNewPolygonFeedback != null)
            {
                m_pNewPolygonFeedback.Refresh(hDC);
            }
        }

        //工具不可用时释放窗体等变量

        public override bool Deactivate()
        {
            return true;
        }
        #endregion

        /// <summary>
        /// 序列化(将对象序列化成字符串)
        /// </summary>
        /// <param name="xmlByte">序列化字节</param>
        /// <param name="obj">序列化对象</param>
        /// <returns></returns>
        private byte[] xmlSerializer(object obj)
        {
            try
            {
                byte[] xmlByte = null;//保存序列化后的字节

                //判断是否支持IPersistStream接口,只有支持该接口的对象才能进行序列化

                if (obj is ESRI.ArcGIS.esriSystem.IPersistStream)
                {
                    ESRI.ArcGIS.esriSystem.IPersistStream pStream = obj as ESRI.ArcGIS.esriSystem.IPersistStream;

                    ESRI.ArcGIS.esriSystem.IXMLStream xmlStream = new ESRI.ArcGIS.esriSystem.XMLStreamClass();

                    pStream.Save(xmlStream as ESRI.ArcGIS.esriSystem.IStream, 0);

                    xmlByte = xmlStream.SaveToBytes();
                }
                return xmlByte;
            }
            catch(Exception e)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
                return null;
            }
        }
       
    }

}