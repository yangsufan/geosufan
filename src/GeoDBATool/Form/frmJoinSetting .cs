using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace GeoDBATool
{
    public partial class frmJoinSetting : DevComponents.DotNetBar.Office2007Form
    {


        public frmJoinSetting()
        {
            InitializeComponent();
            this.com_jointype.Items.Add("标准图幅");
            this.com_jointype.Items.Add("非标准图幅");
            this.com_MergeAtrSet.Items.Add("添加到源要素");
            this.com_MergeAtrSet.Items.Add("覆盖源要素");
        }

        private void frmConSet_Load(object sender, EventArgs e)
        {
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(ModData.v_JoinSettingXML);
            if (null == XmlDoc)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取接边参数配置文件失败！");
                return;
            }
            XmlElement ele = XmlDoc.SelectSingleNode(".//接边设置") as XmlElement;
            string sDisTo = ele.GetAttribute("距离容差");
            string sSeacherTo = ele.GetAttribute("搜索容差");
            string sAngleTo = ele.GetAttribute("角度容差");
            string sLengthTo = ele.GetAttribute("长度容差");
            string sjoinType = ele.GetAttribute("接边类型");
            string sIsRemovePnt = ele.GetAttribute("删除多边形多余点").Trim();
            string sIsSimplify = ele.GetAttribute("简单化要素").Trim();

            ele = XmlDoc.SelectSingleNode(".//融合设置") as XmlElement;
            string sMergeAtrSet = ele.GetAttribute("属性覆盖").Trim();
            ele = XmlDoc.SelectSingleNode(".//日志设置") as XmlElement;
            string sLogPath = ele.GetAttribute("日志路径").Trim();
            double dDisTo = -1;
            double dSeacherTo = -1;
            double dAngleTo = -1;
            double dLengthTo = -1;
            try
            {
                dDisTo = Convert.ToDouble(sDisTo);
                dSeacherTo = Convert.ToDouble(sSeacherTo);
                dAngleTo = Convert.ToDouble(sAngleTo);
                dLengthTo = Convert.ToDouble(sLengthTo);
            }
            catch (Exception er)
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
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "接边参数配置文件中参数不正确！");
                return;
            }
            this.con_AngleTo.Value = dAngleTo;
            this.con_DisTo.Value = dDisTo;
            this.con_LengthTo.Value = dLengthTo;
            this.con_SearchTo.Value = dSeacherTo;
            if (sjoinType == "标准图幅")
            {
                this.com_jointype.SelectedIndex = 0;
            }
            else if (sjoinType == "非标准图幅")
            {
                this.com_jointype.SelectedIndex = 1;
            }
            if (sIsRemovePnt == "Y")
                this.check_RemovePoPnt.Checked = true;
            else
                this.check_RemovePoPnt.Checked = false;

            if (sIsSimplify == "Y")
                this.check_Simplify.Checked = true;
            else
                this.check_Simplify.Checked = false;
            if (sMergeAtrSet == "Y")
                this.com_MergeAtrSet.SelectedIndex = 1;
            else
                this.com_MergeAtrSet.SelectedIndex = 0;
            if (!string.IsNullOrEmpty(sLogPath))
            {
                this.logcheck.Checked = true;
                this.label_LogPath.Visible = true;
                this.label_LogPath.Text = sLogPath;
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.com_jointype.Text))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择一个接边类型！");
                return;
            }
            if (string.IsNullOrEmpty(this.com_MergeAtrSet.Text))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择要素融合属性数据处理类型！");
                return;
            }
            double dDisTo = this.con_DisTo.Value; ;
            double dSeacherTo = this.con_SearchTo.Value;
            double dAngleTo = this.con_AngleTo.Value;
            double dLengthTo = this.con_LengthTo.Value;
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(ModData.v_JoinSettingXML);
            if (null == XmlDoc)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取接边参数配置文件失败！");
                return;
            }
            XmlElement ele = XmlDoc.SelectSingleNode(".//接边设置") as XmlElement;
            ele.SetAttribute("距离容差", dDisTo.ToString());
            ele.SetAttribute("搜索容差", dSeacherTo.ToString());
            ele.SetAttribute("角度容差", dAngleTo.ToString());
            ele.SetAttribute("长度容差", dLengthTo.ToString());
            ele.SetAttribute("接边类型", this.com_jointype.Text.Trim());
            if (this.check_RemovePoPnt.Checked == true)
                ele.SetAttribute("删除多边形多余点", "Y");
            else
                ele.SetAttribute("删除多边形多余点", "N");

            if (this.check_Simplify.Checked == true)
                ele.SetAttribute("简单化要素", "Y");
            else
                ele.SetAttribute("简单化要素", "N");

            XmlElement ele2 = XmlDoc.SelectSingleNode(".//融合设置") as XmlElement;
            if (this.com_MergeAtrSet.Text.Trim() == "添加到源要素")
            {
                ele2.SetAttribute("属性覆盖", "N");
            }
            else
            {
                ele2.SetAttribute("属性覆盖", "Y");
            }
            XmlElement ele3 = XmlDoc.SelectSingleNode(".//日志设置") as XmlElement;
            if (this.logcheck.Checked == true)
            {
                if (!string.IsNullOrEmpty(this.label_LogPath.Text))
                {
                    ele3.SetAttribute("日志路径", this.label_LogPath.Text.Trim());
                }
            }
            else
            {
                ele3.SetAttribute("日志路径", string.Empty);
            }

            try
            {
                XmlDoc.Save(ModData.v_JoinSettingXML);
            }
            catch (Exception er)
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
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "接边配置文件参数写入失败！\n 请确认文件是否只读：\n" + ModData.v_JoinSettingXML);
                return;
            }
            if (this.logcheck.Checked == true)
            {
                IJoinLOG JoinLog = new ClsJoinLog();
                Exception ex = null;
                JoinLog.InitialLog(out ex);
                if (null != ex)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                    return;
                }
            }
            this.Close();
        }

        private void logcheck_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void logcheck_Click(object sender, EventArgs e)
        {
            if (logcheck.Checked == false)
            {
                SaveFileDialog saveDia = new SaveFileDialog();
                saveDia.Filter = "xml 日志文件 (*.xml)|*.xml";
                if (saveDia.ShowDialog() != DialogResult.No || saveDia.ShowDialog() != DialogResult.Cancel)
                {
                    this.label_LogPath.Text = saveDia.FileName;
                    this.label_LogPath.Visible = true;
                }
            }
            else
            {
                this.label_LogPath.Visible = false;
                this.label_LogPath.Text = string.Empty;
            }
        }

    }
}