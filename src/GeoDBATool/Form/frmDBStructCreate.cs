using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Runtime.InteropServices;

namespace GeoDBATool
{
    public partial class frmDBStructCreate : DevComponents.DotNetBar.Office2007Form
    {

        private Plugin.Application.IAppGISRef m_Hook = null;

        public frmDBStructCreate()
        {
            InitializeComponent();
        }

        public frmDBStructCreate(Plugin.Application.IAppGISRef Hook)
        {
            InitializeComponent();
            m_Hook = Hook as Plugin.Application.IAppGISRef;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 初始化窗体控件状态与赋值

        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmDBStructCreate_Load(object sender, EventArgs e)
        {
            object[] TagDBType = new object[] { "GDB", "SDE", "PDB" };
            comBoxType.Items.AddRange(TagDBType);
            comBoxType.SelectedIndex = 0;
        }

        /// <summary>
        /// 选择本地库文件路径

        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnServer_Click(object sender, EventArgs e)
        {
            switch (comBoxType.Text)
            {
                case "SDE":

                    break;

                case "GDB":
                    FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                    if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        DirectoryInfo dir = new DirectoryInfo(pFolderBrowser.SelectedPath);
                        string name = dir.Name;
                        if (dir.Parent == null)
                        {
                            name = dir.Name.Substring(0,dir.Name.Length-2);
                        }
                        txtDataBase.Text = dir.FullName + "\\" + name + ".gdb";
                    }
                    break;

                case "PDB":
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.Title = "保存为ESRI个人数据库";
                    saveFile.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        txtDataBase.Text = saveFile.FileName;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 选择空间参考文件

        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProj_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "选择空间参考";
            OpenFile.Filter = "空间参考文件(*.prj)|*.prj";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                txtProjFilePath.Text = OpenFile.FileName;
                btnProj.Tooltip = OpenFile.FileName;
            }
        }

        /// <summary>
        /// 选择库体配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRuleFile_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "选择库体配置文件";
            OpenFile.Filter = "库体配置文件(*.mdb)|*.mdb|库体配置文件(*.gosch)|*.gosch";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                textRuleFilePath.Text = OpenFile.FileName;
                btnRuleFile.Tooltip = OpenFile.FileName;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            btnOk.Enabled = false;
            try
            {
                #region 检查设置是否完备

                if (comBoxType.SelectedIndex==1)
                {
                    if (txtServer.Text.Length == 0 || txtInstance.Text.Length == 0 || txtUser.Text.Length == 0 || txtPassWord.Text.Length == 0 || txtVersion.Text.Length == 0)
                    {
                        labelXErr.Text = "请完整设置SDE服务器访问参数！";
                        btnOk.Enabled = true;
                        return;
                    }
                }
                else
                {
                    if (txtDataBase.Text.Length == 0)
                    {
                        labelXErr.Text = "请完整设置本地数据库路径！";
                        btnOk.Enabled = true;
                        return;
                    }
                }

                if (txtProjFilePath.Text.Length == 0 || textRuleFilePath.Text.Length == 0)
                {
                    labelXErr.Text = "请完整设置空间参考与库体配置文件访问路径！";
                    btnOk.Enabled = true;
                    return;
                }
                #endregion

                SysCommon.Gis.ICreateGeoDatabase pCreateGeoDatabase = new SysCommon.Gis.CreateArcGISGeoDatabase();

                if (!pCreateGeoDatabase.LoadDBShecmaDocument(textRuleFilePath.Text))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "读取库体配置文件失败,请检查配置文件是否正确！");
                    return;
                }
                pCreateGeoDatabase.LoadSpatialReference(txtProjFilePath.Text);

                #region 设置数据库连接

                List<string> Pra = new List<string>();

                if (this.comBoxType.SelectedIndex == 2)    //PDB库体
                {
                    pCreateGeoDatabase.SetDestinationProp("PDB", txtDataBase.Text, "", "", "", "");
                    Pra.Add("PDB");
                    Pra.Add(txtDataBase.Text);
                }
                else if (this.comBoxType.SelectedIndex == 1)    //SDE库体
                {
                    pCreateGeoDatabase.SetDestinationProp("SDE", txtServer.Text, txtInstance.Text, txtUser.Text, txtPassWord.Text, txtVersion.Text);
                    Pra.Add("SDE");
                    Pra.Add(txtServer.Text);
                    Pra.Add(txtInstance.Text);
                    Pra.Add(txtUser.Text);
                    Pra.Add(txtVersion.Text);
                }
                else if (this.comBoxType.SelectedIndex == 0)   //GDB库体
                {
                    pCreateGeoDatabase.SetDestinationProp("GDB", txtDataBase.Text, "", "", "", "");
                    Pra.Add("GDB");
                    Pra.Add(txtDataBase.Text);
                }
                #endregion

                //**********************************************
                //guozheng added System Function log
                Pra.Add(this.txtProjFilePath.Text);
                Pra.Add(this.textRuleFilePath.Text);
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write("构建空间数据库库体", Pra, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write("构建空间数据库库体", Pra, DateTime.Now);
                }
                //*********************************************

                List<string> DSName = new List<string>();
                if (!pCreateGeoDatabase.CreateDBStruct(DSName))
                {
                    return;
                }

                string pDSName = DSName[0];


                //================================================================================================================
                //创建远程日志表

                Exception err = null;
                labelXErr.Text = "创建远程日志表....";
                string mDbType = "";
                if (this.comBoxType.SelectedIndex == 2)
                {
                    mDbType = "PDB";
                }
                else if (this.comBoxType.SelectedIndex == 1)
                {
                    mDbType = "SDE";
                }
                else if (this.comBoxType.SelectedIndex == 0)
                {
                    mDbType = "GDB";
                }
                if (!pCreateGeoDatabase.CreateSQLTable(mDbType, out err))//!CreateTable(pTagetWorkspace,out err)
                {

                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建远程日志表失败！\n" + err.Message);
                    btnOk.Enabled = true;
                    return;
                }

                ///将现势库信息写入配置文件（bin目录下的Admin.xml）

                ///
                DevComponents.AdvTree.Node pCurNode = m_Hook.ProjectTree.SelectedNode; ///获得树图上选择的工程节点

                string pProjectname = pCurNode.Text ;

                System.Xml.XmlNode Projectnode = m_Hook.DBXmlDocument.SelectSingleNode("工程管理/工程[@名称='"+pProjectname+"']");
                System.Xml.XmlElement ProjectNodeElement = Projectnode as System.Xml.XmlElement;

                System.Xml.XmlElement ProjectConnEle = ProjectNodeElement.SelectSingleNode(".//现势库/连接信息") as System.Xml.XmlElement;
                ///设置数据库连接类型

                ///
                if (this.comBoxType.SelectedIndex == 2)
                {
                    ProjectConnEle.SetAttribute("类型", "PDB");
                    ProjectConnEle.SetAttribute("数据库", txtDataBase.Text);
                    
                }
                else if (this.comBoxType.SelectedIndex == 0)
                {
                    ProjectConnEle.SetAttribute("类型", "GDB");
                    ProjectConnEle.SetAttribute("数据库", txtDataBase.Text);
                }
                else if(this.comBoxType.SelectedIndex==1)
                {
                    ProjectConnEle.SetAttribute("类型", "SDE");
                    ProjectConnEle.SetAttribute("服务器", txtServer.Text);
                    ProjectConnEle.SetAttribute("服务名", txtInstance.Text);
                    ProjectConnEle.SetAttribute("数据库", txtDataBase.Text);
                    ProjectConnEle.SetAttribute("用户", txtUser.Text);
                    ProjectConnEle.SetAttribute("密码", txtPassWord.Text);
                    ProjectConnEle.SetAttribute("版本", txtVersion.Text);
                }

                ///设置数据集名称

                ///
                System.Xml.XmlElement ProjectUserDSEle = ProjectConnEle.SelectSingleNode(".//库体") as System.Xml.XmlElement;
                ProjectUserDSEle.SetAttribute("名称", pDSName);

                m_Hook.DBXmlDocument.Save(ModData.v_projectXML);

                labelXErr.Text = "创建完成！";

                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建库体成功!");
                this.Close();

                return;
            }
            catch (Exception ex)
            {
                //labelXErr.Text = "发生错误！"+ex.Message;

                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                //********************************************************************

                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "发生错误！" + ex.Message);
                btnOk.Enabled = true;
                return;

            }


        }

        private void txtProjFilePath_TextChanged(object sender, EventArgs e)
        {
            //设置"确定"按钮是否可用
            if (this.comBoxType.SelectedIndex == 2 || this.comBoxType.SelectedIndex == 0)
            {  
                if (txtProjFilePath.Text != "" && textRuleFilePath.Text != ""&&txtDataBase.Text!="")
                {
                    btnOk.Enabled = true;
                }
            }
            if (this.comBoxType.SelectedIndex == 1)
            {
                if (txtProjFilePath.Text != "" && textRuleFilePath.Text != "" &&txtServer.Text != "" && txtInstance.Text != ""&&txtUser.Text!=""&&txtPassWord.Text!=""&&txtVersion.Text!="")
                {
                    btnOk.Enabled = true;
                }
            }
                
        }

        private void textRuleFilePath_TextChanged(object sender, EventArgs e)
        {
            //设置"确定"按钮是否可用
            if (this.comBoxType.SelectedIndex == 2 || this.comBoxType.SelectedIndex == 0)
            {
                if (txtProjFilePath.Text != "" && textRuleFilePath.Text != "" && txtDataBase.Text != "")
                {
                    btnOk.Enabled = true;
                }
            }
            if (this.comBoxType.SelectedIndex == 1)
            {
                if (txtProjFilePath.Text != "" && textRuleFilePath.Text != "" && txtServer.Text != "" && txtInstance.Text != "" && txtUser.Text != "" && txtPassWord.Text != "" && txtVersion.Text != "")
                {
                    btnOk.Enabled = true;
                }
            }
        }

        private void txtServer_TextChanged(object sender, EventArgs e)
        {
            //设置"确定"按钮是否可用
            if (this.comBoxType.SelectedIndex == 1)
            {
                if (txtProjFilePath.Text != "" && textRuleFilePath.Text != "" && txtServer.Text != "" && txtInstance.Text != "" && txtUser.Text != "" && txtPassWord.Text != "" && txtVersion.Text != "")
                {
                    btnOk.Enabled = true;
                }
            }
        }

        private void txtInstance_TextChanged(object sender, EventArgs e)
        {
            //设置"确定"按钮是否可用
            if (this.comBoxType.SelectedIndex == 2 || this.comBoxType.SelectedIndex == 0)
            {
                if (txtProjFilePath.Text != "" && textRuleFilePath.Text != "" && txtDataBase.Text != "" && txtInstance.Text != "")
                {
                    btnOk.Enabled = true;
                }
            }
            if (this.comBoxType.SelectedIndex == 1)
            {
                if (txtProjFilePath.Text != "" && textRuleFilePath.Text != "" && txtServer.Text != "" && txtInstance.Text != "" && txtUser.Text != "" && txtPassWord.Text != "" && txtVersion.Text != "")
                {
                    btnOk.Enabled = true;
                }
            }
        }

        private void txtDataBase_TextChanged(object sender, EventArgs e)
        {
            //设置"确定"按钮是否可用
            if (this.comBoxType.SelectedIndex == 2 || this.comBoxType.SelectedIndex == 0)
            {
                if (txtProjFilePath.Text != "" && textRuleFilePath.Text != "" && txtDataBase.Text != "" && txtInstance.Text != "")
                {
                    btnOk.Enabled = true;
                }
            }
            if (this.comBoxType.SelectedIndex == 1)
            {
                if (txtProjFilePath.Text != "" && textRuleFilePath.Text != "" && txtServer.Text != "" && txtInstance.Text != "" && txtUser.Text != "" && txtPassWord.Text != "" && txtVersion.Text != "")
                {
                    btnOk.Enabled = true;
                }
            }
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            //设置"确定"按钮是否可用
            if (this.comBoxType.SelectedIndex == 1)
            {
                if (txtProjFilePath.Text != "" && textRuleFilePath.Text != "" && txtServer.Text != "" && txtInstance.Text != "" && txtUser.Text != "" && txtPassWord.Text != "" && txtVersion.Text != "")
                {
                    btnOk.Enabled = true;
                }
            }
        }

        private void txtPassWord_TextChanged(object sender, EventArgs e)
        {
            //设置"确定"按钮是否可用
            if (this.comBoxType.SelectedIndex == 1)
            {
                if (txtProjFilePath.Text != "" && textRuleFilePath.Text != "" && txtServer.Text != "" && txtInstance.Text != "" && txtUser.Text != "" && txtPassWord.Text != "" && txtVersion.Text != "")
                {
                    btnOk.Enabled = true;
                }
            }
        }

        private void txtVersion_TextChanged(object sender, EventArgs e)
        {
            //设置"确定"按钮是否可用
            if (this.comBoxType.SelectedIndex == 1)
            {
                if (txtProjFilePath.Text != "" && textRuleFilePath.Text != "" && txtServer.Text != "" && txtInstance.Text != "" && txtUser.Text != "" && txtPassWord.Text != "" && txtVersion.Text != "")
                {
                    btnOk.Enabled = true;
                }
            }
        }

        private void comBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtServer.Text = "";
            txtInstance.Text = "";
            txtDataBase.Text = "";
            txtUser.Text = "";
            txtPassWord.Text = "";
            labelXErr.Text = "";
            btnOk.Enabled = true;
            if (comBoxType.Text != "SDE")
            {
                btnServer.Visible = true;
                txtDataBase.Size = new Size(txtServer.Size.Width - btnServer.Size.Width, txtDataBase.Size.Height);
                txtServer.Enabled = false;
                txtInstance.Enabled = false;
                txtUser.Enabled = false;
                txtPassWord.Enabled = false;
                txtVersion.Enabled = false;
            }
            else
            {
                btnServer.Visible = false;
                txtDataBase.Size = new Size(txtServer.Size.Width, txtDataBase.Size.Height);
                txtServer.Enabled = true;
                txtInstance.Enabled = true;
                txtUser.Enabled = true;
                txtPassWord.Enabled = true;
                txtVersion.Enabled = true;

            }
        }

        private void labelXErr_Click(object sender, EventArgs e)
        {

        }
    }
}