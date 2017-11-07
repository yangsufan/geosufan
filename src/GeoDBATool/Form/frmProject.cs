using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace GeoDBATool
{
    public partial class frmProject : DevComponents.DotNetBar.Office2007Form
    {
        private DevComponents.AdvTree.AdvTree ProjectTree;
        private bool IsNew;
        private bool bHasEdit;

        public frmProject(DevComponents.AdvTree.AdvTree projectTree, bool isNew)
        {
            InitializeComponent();

            object[] scale = new object[] { "500", "1000", "2000", "5000", "10000" };
            comBoxScale.Items.AddRange(scale);
            comBoxScale.SelectedIndex = 0;

            if (!isNew)
            {
                XmlElement aElement = projectTree.SelectedNode.Tag as XmlElement;
                txtProjectName.Text = aElement.GetAttribute("名称");
                comBoxScale.Text = aElement.GetAttribute("比例尺");
            }

            ProjectTree = projectTree;
            IsNew = isNew;
            bHasEdit = false;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!CheckProjectName())
            {
                return;
            }
            ////////////
            if (!File.Exists(ModData.v_projectXML))
            {
                if (!File.Exists(ModData.v_projectXMLTemp))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "缺失模板文件,请检查!");
                    return;
                }
                XmlDocument xml = new XmlDocument();
                xml.LoadXml("<工程管理></工程管理>");
                xml.Save(ModData.v_projectXML);
                xml.Load(ModData.v_projectXML);
                ModData.v_AppGIS.DBXmlDocument = xml;
            }
            ///////////
            if (!IsNew)
            {
                if (ModData.v_AppGIS.DBXmlDocument.DocumentElement.SelectSingleNode(".//工程[@名称='" + txtProjectName.Text + "']") != null)
                {
                    if (bHasEdit == true)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "已存在同名节点!");
                        return;
                    }
                    else
                    {
                        this.Close();
                        return;
                    }
                }
                else
                {
                    if (bHasEdit == false)
                    {
                        this.Close();
                        return;
                    }
                    XmlElement aElement = ProjectTree.SelectedNode.Tag as XmlElement;
                    aElement.SetAttribute("名称", txtProjectName.Text);
                    aElement.SetAttribute("比例尺", comBoxScale.Text);
                    ProjectTree.SelectedNode.Name = txtProjectName.Text;
                    ProjectTree.SelectedNode.Text = txtProjectName.Text;

                    aElement.OwnerDocument.Save(ModData.v_projectXML);
                    //**********************************************
                    //guozheng added System Function log
                    List<string> Pra = new List<string>();
                    Pra.Add("名称:" + txtProjectName.Text);
                    Pra.Add("比例尺:" + comBoxScale.Text);
                    if (ModData.SysLog != null)
                    {
                        ModData.SysLog.Write("修改工程名称", Pra, DateTime.Now);
                    }
                    else
                    {
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                        ModData.SysLog.Write("修改工程名称", Pra, DateTime.Now);
                    }
                    //*********************************************
                    this.Close();

                    return;
                }
            }
            else
            {
                if (ModData.v_AppGIS.DBXmlDocument.DocumentElement.SelectSingleNode(".//工程[@名称='" + txtProjectName.Text + "']") != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "已存在同名节点!");
                    return;
                }
            }

            if (txtProjectName.Text.Trim() == string.Empty)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入节点名称!");
                return;
            }



            //if (ModData.v_AppGIS.DBXmlDocument.DocumentElement.SelectSingleNode(".//工程[@名称='" + txtProjectName.Text + "']") != null)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "已存在同名节点!");
            //    return;
            //}
            //**********************************************
            //guozheng added System Function log
            List<string> Pra2 = new List<string>();
            Pra2.Add("名称:" + txtProjectName.Text);
            Pra2.Add("比例尺:" + comBoxScale.Text);
            if (ModData.SysLog != null)
            {
                ModData.SysLog.Write("新建数据库项目", Pra2, DateTime.Now);
            }
            else
            {
                ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModData.SysLog.Write("新建数据库项目", Pra2, DateTime.Now);
            }
            //*********************************************
            ProjectXml.AddTreeNode(ModData.v_AppGIS.DBXmlDocument, ModData.v_AppGIS.ProjectTree, txtProjectName.Text, comBoxScale.Text, ModData.v_projectXMLTemp);

            ModData.v_AppGIS.DBXmlDocument.Save(ModData.v_projectXML);

            this.Close();
        }

        private void txtProjectName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ("'".Contains(e.KeyChar.ToString()))
            {
                e.KeyChar = Char.MinValue;
                DevComponents.DotNetBar.SuperTooltipInfo info = new DevComponents.DotNetBar.SuperTooltipInfo();
                info.HeaderText = "不能包含字符 '";
                superTooltip.SetSuperTooltip(txtProjectName, info);
            }
        }

        private void txtProjectName_TextChanged(object sender, EventArgs e)
        {
            this.bHasEdit = true;
        }
        /// <summary>
        /// 检查工程名称名称

        /// </summary>
        /// <returns></returns>
        private bool CheckProjectName()
        {
            string ProjectName = this.txtProjectName.Text;
            if (null == ProjectName)
                return false;
            string s_nonlicet = string.Empty;
            if (ProjectName.Contains("<")) s_nonlicet += "<,";
            if (ProjectName.Contains("\"")) s_nonlicet += "\",";
            if (ProjectName.Contains("%")) s_nonlicet += "%,";
            if (ProjectName.Contains("?")) s_nonlicet += "?,";
            if (ProjectName.Contains("^")) s_nonlicet += "^,";
            if (ProjectName.Contains("\'")) s_nonlicet += "\',";
            if (ProjectName.Contains("&")) s_nonlicet += "&,";
            if (ProjectName.Contains("$")) s_nonlicet += "$,";
            if (ProjectName.Contains("#")) s_nonlicet += "#,";
            if (ProjectName.Contains("@")) s_nonlicet += "@,";
            if (ProjectName.Contains("~")) s_nonlicet += "~,";
            if (ProjectName.Contains("*")) s_nonlicet += "*,";
            if (ProjectName.Contains("!")) s_nonlicet += "!,";
            if (ProjectName.Contains(">")) s_nonlicet += ">,";
            if (ProjectName.Contains("/")) s_nonlicet += "/,";
            if (ProjectName.Contains("\\")) s_nonlicet += "\\,";
            if (ProjectName.Contains(";")) s_nonlicet += ";,";
            if (s_nonlicet != string.Empty)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "非法字符：" + s_nonlicet);
                return false;
            }
            else
                return true;

        }
    }
}