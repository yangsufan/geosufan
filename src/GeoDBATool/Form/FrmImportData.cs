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
    public partial class FrmImportData : DevComponents.DotNetBar.Office2007Form
    {
        private Plugin.Application.IAppGISRef m_AppGIS;               //主功能应用APP
        private EnumOperateType m_OpeType;                            //数据操作类型：数据入库
        private XmlElement m_DbProjectElement;                        //选中的数据库节点

        private clsDataThread m_DataThread;
        private System.Windows.Forms.Timer _timer;

        public FrmImportData(string strFrmName, EnumOperateType pType, Plugin.Application.IAppGISRef pAppGIS, XmlElement dbProjectElement)
        {
            InitializeComponent();
            this.Text = strFrmName;
            m_OpeType = pType;
            m_AppGIS = pAppGIS;
            m_DbProjectElement = dbProjectElement;

            //cyf 20110626 modify
            //object[] TagDBType = new object[] { "GDB", "SDE", "PDB" ,"SHP"};
            object[] TagDBType = new object[] { "ArcSDE(For Oracle)", "ESRI文件数据库(*.gdb)", "ESRI个人数据库(*.mdb)" };   
            //end
            comboBoxOrgType.Items.AddRange(TagDBType);
            comboBoxOrgType.SelectedIndex = 0;
        }


        //选择源数据
        private void btnOrg_Click(object sender, EventArgs e)
        {
            switch (comboBoxOrgType.Tag.ToString())   //cyf 20110626 modify:
            {
                case "SDE":
                    //frmDBPropertySet frmSet = new frmDBPropertySet("设置SDE连接", "SDE");
                    frmXZDBPropertySet frmSet = new frmXZDBPropertySet();
                    frmSet.ShowDialog();
                    if (frmSet.Res == true)
                    {
                        if (!listViewEx.Items.ContainsKey(frmSet.GetPropertySetStr))
                        {
                            ListViewItem aItem = listViewEx.Items.Add(frmSet.GetPropertySetStr, frmSet.GetPropertySetStr, "");
                            aItem.Tag = "SDE";
                            aItem.Checked = true;
                            aItem.ToolTipText = frmSet.GetPropertySetStr;
                        }
                        else
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "列表中已添加该项!");
                        }
                    }
                    break;

                case "GDB":
                    FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                    if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        if (pFolderBrowser.SelectedPath.EndsWith(".gdb"))
                        {
                            if (!listViewEx.Items.ContainsKey(pFolderBrowser.SelectedPath))
                            {
                                ListViewItem aItem = listViewEx.Items.Add(pFolderBrowser.SelectedPath, pFolderBrowser.SelectedPath, "");
                                aItem.Tag = "GDB";
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
                    break;
                case "SHP":
                    FolderBrowserDialog pFolderBrowser1 = new FolderBrowserDialog();
                    if (pFolderBrowser1.ShowDialog() == DialogResult.OK)
                    {
                        if (!listViewEx.Items.ContainsKey(pFolderBrowser1.SelectedPath))
                        {
                            ListViewItem aItem = listViewEx.Items.Add(pFolderBrowser1.SelectedPath, pFolderBrowser1.SelectedPath, "");
                            aItem.Tag = "SHP";
                            aItem.Checked = true;
                            aItem.ToolTipText = pFolderBrowser1.SelectedPath;
                        }
                        else
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "列表中已添加该项!");
                        }

                    }
                    break;
                case "PDB":
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
                                aItem.Tag = "PDB";
                                aItem.Checked = true;
                                aItem.ToolTipText = OpenFile.FileNames[i];
                            }
                            else
                            {
                                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "列表中已添加该项!");
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        //全选
        private void btnSelAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem aItem in listViewEx.Items)
            {
                aItem.Checked = true;
            }
        }

        //反选
        private void btnSelReverse_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem aItem in listViewEx.Items)
            {
                aItem.Checked = !aItem.Checked;
            }
        }

        //清除
        private void btnDel_Click(object sender, EventArgs e)
        {
            listViewEx.Items.Clear();
        }

        //取消
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //确定
        private void btnOk_Click(object sender, EventArgs e)
        {
            //Exception err = null;
            DataTable pDataTable = null;
            SysCommon.DataBase.SysDataBase pSysDB = null;
            string pObjDBType = "";                              //目标数据库类型
            string pObjServer = "";                              //目标服务器
            string pObjInstance = "";                            //目标实例名
            string pObjDataBase = "";                            //目标数据库
            string pObjUser = "";                                //用户
            string pObjPassword = "";                            //密码
            string pObjVersion = "";                             //版本


            //源数据连接设置检查
            if (listViewEx.Items.Count == 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请设置源数据连接!");
                return;
            }
            //cyf 20110625 modify：根据数据集节点获得数据库节点
            //DevComponents.AdvTree.Node pDtNode = m_AppGIS.ProjectTree.SelectedNode;      //数据集树节点
            //string pFeaDtName = pDtNode.Text;                      //数据集名称
            //try { m_DbProjectElement = pDtNode.Parent.Tag as XmlElement; }     //数据库节点
            //catch { }
            //if (m_DbProjectElement == null) return;
            //end
            //目标数据库连接信息
            if(m_DbProjectElement!=null)
            {
                try
                {
                    //cyf 20110625 modify:改为从数据库节点读取连接信息（以前是从数据库工程节点读取连接信息）
                    XmlElement aElement = m_DbProjectElement.SelectSingleNode(".//连接信息") as XmlElement;
                    pObjDBType = aElement.GetAttribute("类型");
                    pObjServer = aElement.GetAttribute("服务器");
                    pObjInstance = aElement.GetAttribute("服务名");
                    pObjDataBase = aElement.GetAttribute("数据库");
                    pObjVersion = aElement.GetAttribute("版本");
                    pObjUser = aElement.GetAttribute("用户");
                    pObjPassword = aElement.GetAttribute("密码");
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

                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统数据库工程XML内容破坏,请检查!");
                    return;
                }
            }

            //目标数据库连接设置检查
            if (pObjDBType.ToUpper().Trim() == "SDE")
            {
                if (pObjUser.Trim() == "" || pObjPassword.Trim() == "" )
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请设置目标数据连接!");
                    return;
                }
            }
            else
            {
                if (pObjDataBase.Trim() == "")
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请设置目标数据连接!");
                    return;
                }
                if (pObjDBType.ToUpper().Trim() == "GDB")
                {
                    if (!pObjDataBase.Trim().Contains(".gdb"))
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "目标数据连接，请选择ESRI文件数据库！");
                        return;
                    }
                }
            }

            //映射规则文件检查
            if (!File.Exists(ModData.DBImportPath))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "映射关系文件‘批量数据入库.xml’不存在，请检查！");
                return;
            }
            

            Plugin.Application.IAppFormRef pAppFormRef = m_AppGIS as Plugin.Application.IAppFormRef;
            pAppFormRef.OperatorTips = "获取数据信息...";

            //根据窗体设置形成数据操作xml内容
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<数据移植></数据移植>");
            XmlNode sourceNode = ModDBOperator.SelectNode(xmlDoc.DocumentElement as XmlNode, "源数据连接");
            foreach (ListViewItem aItem in listViewEx.Items)
            {
                if (aItem.Checked)
                {
                    XmlElement dataElement = xmlDoc.CreateElement("源数据");
                    dataElement.SetAttribute("类型", aItem.Tag.ToString());
                    if (aItem.Tag.ToString() != "SDE")
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
                        string[] strTemp = aItem.Text.Split('|');
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
            //设置目标数据连接xml
            XmlElement objElementTemp = ModDBOperator.SelectNode(xmlDoc.DocumentElement as XmlNode, "目标数据连接") as XmlElement;
            objElementTemp.SetAttribute("类型", pObjDBType.Trim());
            objElementTemp.SetAttribute("服务器", pObjServer.Trim());
            objElementTemp.SetAttribute("服务名", pObjInstance.Trim());
            objElementTemp.SetAttribute("数据库", pObjDataBase.Trim());
            objElementTemp.SetAttribute("用户", pObjUser.Trim());
            objElementTemp.SetAttribute("密码", pObjPassword.Trim());
            objElementTemp.SetAttribute("版本", pObjVersion.Trim());
            //设置映射规则XML
            XmlElement RuleElement = ModDBOperator.SelectNode(xmlDoc.DocumentElement as XmlNode, "规则") as XmlElement;
            RuleElement.SetAttribute("路径", ModData.DBImportPath);

            //初始化数据处理树图
            pAppFormRef.OperatorTips = "初始化数据处理树图...";
            InitialDBTree newInitialDBTree = new InitialDBTree();
            newInitialDBTree.OnCreateDataTree(m_AppGIS.DataTree, xmlDoc);
            if ((bool)m_AppGIS.DataTree.Tag == false) return;

            //*******************************************************************
            //guozheng added  数据入库系统运行日志 
            List<string> Pra = new List<string>();
            Pra.Add(" Target: " + pObjDBType.ToUpper().Trim());
            if (pObjDBType.ToUpper().Trim() == "SDE")
            {
                Pra.Add(pObjServer.Trim());
                Pra.Add(pObjInstance.Trim());
                Pra.Add(pObjDataBase.Trim());
                Pra.Add(pObjUser.Trim());
                Pra.Add(pObjVersion.Trim());
            }
            else
            {
                Pra.Add(pObjDataBase.Trim());
            }
            for (int i = 0; i < this.listViewEx.Items.Count; i++)
            {
                if (this.listViewEx.Items[i].Checked)
                {
                    Pra.Add(" Source: " + this.listViewEx.Items[i].ToolTipText);
                }
            }
            if (ModData.SysLog != null)
            {
                ModData.SysLog.Write("矢量数据入库", Pra, DateTime.Now);
            }
            else
            {
                ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModData.SysLog.Write("矢量数据入库", Pra, DateTime.Now);
            }
            //********************************************************************


            //进行数据移植
            this.Visible = false;
            this.Hide();
            pAppFormRef.OperatorTips = "进行" + this.Text + "...";
            //--------------------------------------------------
          
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
                    //SaveToXML();
                    this.Close();
                }
                else
                    this.Show();

                m_DataThread = null;
                _timer.Enabled = false;
            }
        }

        private void FrmImportData_Load(object sender, EventArgs e)
        {

        }

        private void comboBoxOrgType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cyf 20110626 add:设置数据类型的值
            if (comboBoxOrgType.Text.Trim() == "ArcSDE(For Oracle)")
            {
                comboBoxOrgType.Tag = "SDE";
            }
            else if (comboBoxOrgType.Text.Trim() == "ESRI文件数据库(*.gdb)")
            {
                comboBoxOrgType.Tag = "GDB";
            }
            else if (comboBoxOrgType.Text.Trim() == "ESRI个人数据库(*.mdb)")
            {
                comboBoxOrgType.Tag = "PDB";
            }
        }
    }
}