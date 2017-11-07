using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDBIntegration
{
    /// <summary>
    /// 陈亚飞 20110216  add   ：修改数据库工程名
    /// </summary>
    public partial class frmProject : DevComponents.DotNetBar.Office2007Form
    {
        private DevComponents.AdvTree.AdvTree ProjectTree;  //数据库工程树
        private bool bHasEdit;  //标志是否进行了编辑

        public frmProject(DevComponents.AdvTree.AdvTree projectTree)
        {
            InitializeComponent();

            if (projectTree.SelectedNode == null) return;
            txtProjectName.Text = projectTree.SelectedNode.Text.Trim();  //工程名

            ProjectTree = projectTree;

            bHasEdit = false;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Exception pError=null;

            //首先检查工程名臣是否符合规范
            if (!CheckProjectName())
            {
                return;
            }
            if (ProjectTree.SelectedNode == null) return;
            int projectID=-1;   //数据库工程ID
            string projectType = "";  //数据库工程类型
            if (ProjectTree.SelectedNode.DataKey.ToString().Trim() != "")
            {
                projectID = Convert.ToInt32(ProjectTree.SelectedNode.DataKey.ToString().Trim());
            }
            XmlElement pElem = null;  //数据库工程参数信息元素
            if (ProjectTree.SelectedNode.Tag != null)
            {
                pElem = ProjectTree.SelectedNode.Tag as XmlElement;
                if (pElem != null)
                {
                    projectType = pElem.GetAttribute("数据库类型").Trim();
                }
            }

            DevComponents.AdvTree.Node rootNode=ProjectTree.SelectedNode;  //树图的根节点
            //遍历父节点，从而获得根节点
            while(rootNode.Parent!=null)
            {
                rootNode = rootNode.Parent;
            }
            //cyf 20110602 modify :修改数据库连接信息
            #region 原有代码
            //string pConnStr = ""; //系统维护库连接字符串
            ////获得系统维护库的连接字符串
            //if (rootNode.Tag == null)
            //{
            //    //从xml中获得连接信息
            //    if (File.Exists(ModuleData.v_AppDBConectXml))
            //    {
            //        XmlDocument XmlDoc = new XmlDocument();
            //        XmlDoc.Load(ModuleData.v_AppDBConectXml);
            //        XmlElement ele = XmlDoc.SelectSingleNode(".//系统维护库连接信息") as XmlElement;
            //        if (ele != null)
            //        {
            //            pConnStr = ele.GetAttribute("连接字符串");
            //        }
            //    }
            //}
            //else
            //{
            //    //从树节点获得连接信息
            //    pConnStr = rootNode.Tag.ToString().Trim();
            //}

            //连接数据库
            //SysCommon.DataBase.SysTable pDbTable = new SysCommon.DataBase.SysTable();
            //pDbTable.SetDbConnection(pConnStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out pError);
            //if (pError != null)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接系统维护库出错！");
            //    return;
            //}
            #endregion 
            if (bHasEdit)
            {
                //数据库工程名进行了更改

                //更新数据库工程名
                if (projectID != -1)
                {
                     #region 原有代码
                    string updateStr = "update DATABASEMD set DATABASENAME='" + txtProjectName.Text.Trim() + "' where ID=" + projectID;
                   //pDbTable.UpdateTable(updateStr, out pError);
                    //if (pError != null)
                    //{
                    //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "更新数据库出错！");
                    //    return;
                    //}
                    #endregion

                    //cyf 20110602 modify
                    if ( ModuleData.TempWks  == null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统维护库连接失败！");
                        return;
                    }
                    try
                    {
                        ModuleData.TempWks.ExecuteSQL(updateStr);
                    } catch
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "更新数据库出错！");
                        return;
                    }
                    //end
                    //刷新界面
                    //刷新树节点
                    ProjectTree.SelectedNode.Text = txtProjectName.Text.Trim();
                    ProjectTree.Refresh();
                    //cyf 20110627 add:刷新游标面板中的label控件
                    ModuleData.v_DataBaseProPanel.EditLabel(projectType,projectID.ToString(), txtProjectName.Text.Trim());
                    //end
                    //刷新Combox
                    //clsRefurbishDBinfo pclsRefurbishDBinfo = new clsRefurbishDBinfo(pDbTable);
                    //if (projectType != "")
                    //{
                    //    pclsRefurbishDBinfo.UpDataComBox(projectType, out pError);
                    //    if (pError != null)
                    //    {
                    //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "刷新界面ComboBox出错！");
                    //        return;
                    //    }
                    //}
                    //刷新XML
                    //cyf 20110627 delete
                    //RefreshStartXml(projectType, projectID, txtProjectName.Text.Trim(), pError);
                    //if (pError != null)
                    //{
                    //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", pError.Message);
                    //    return;
                    //}
                    //end
                }
            }
            this.Close();
        }

        private void RefreshStartXml(string pDbType,int projectID,string projectName,Exception pError)
        {
            pError = null;
            string xmlPath = "";   //需要刷新的xml路径
            //获得数据库工程的xml
            if (pDbType == enumInterDBType.成果文件数据库.ToString())
            {
                
            }
            else if (pDbType == enumInterDBType.地理编码数据库.ToString())
            {
               
            }
            else if (pDbType == enumInterDBType.地名数据库.ToString())
            {
               
            }
            else if (pDbType == enumInterDBType.高程数据库.ToString())
            {
                xmlPath = ModuleData.v_DEMProjectXml;
            }
            else if (pDbType == enumInterDBType.框架要素数据库.ToString())
            {
                //cyf 20110627 delete:
                xmlPath = ModuleData.v_feaProjectXML;
                //end
            }
            else if (pDbType == enumInterDBType.影像数据库.ToString())
            {
                xmlPath = ModuleData.v_ImageProjectXml;
            }
            //加载数据库工程xml
            XmlDocument xmlDoc = new XmlDocument();
            if (!File.Exists(xmlPath))
            {
                pError = new Exception("xml文件不存在！");
                return;
            }
            xmlDoc.Load(xmlPath);

            XmlNode pNode = xmlDoc.SelectSingleNode(".//工程管理//工程[@编号='" + projectID + "']");
            if (pNode != null)
            {
                XmlElement mElem = pNode as XmlElement;
                mElem.SetAttribute("名称", projectName);
                xmlDoc.Save(xmlPath);
            }
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
        /// <returns>返回工程名称是否合法</returns>
        private bool CheckProjectName()
        {
            string ProjectName = this.txtProjectName.Text.Trim();
            if (""== ProjectName)
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