using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using Fan.Common;
using Fan.DataBase;
using Fan.DataBase.Module;

namespace Fan.SysSetting
{
    public partial class FromInitializeSet : BaseMdiChild
    {

        public FromInitializeSet(BaseRibbonForm parentForm):base(parentForm)
        {
            InitializeComponent();
            m_sysConfig = parentForm.GetMainSysConfig();
        }
        private SysConfig m_sysConfig = null;
        private Dictionary<string,DBConfig> m_dicTemplateDb = new Dictionary<string, DBConfig>();
        /// <summary>
        ///设置正式库连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSetOffcalDbConnect_Click(object sender, EventArgs e)
        {
            frmDBSet dBSet = new frmDBSet(string.Format("设置正式数据库连接"),string.Format("正式数据主要是指成果数据存储位置"));
            if (dBSet.ShowDialog() != DialogResult.OK) return;
            DBConfig OffcalDbConfig = dBSet.DbConfig;
            txtOfficalDbSet.Text = OffcalDbConfig.GetConfigName();
            //将连接信息存储到业务库中
            string strMessage = m_sysConfig.UpdateSysConfig(ColumnName.OfficDbConfigCode, OffcalDbConfig.GetConfigStr(), string.Format("临时库配置"));
            if (!string.IsNullOrEmpty(strMessage))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strMessage,"提示",MessageBoxButtons.OK,MessageBoxIcon.Error);   
            }
        }
        /// <summary>
        /// 设置临时库连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSetTempDbConnect_Click(object sender, EventArgs e)
        {
            frmDBSet dBSet = new frmDBSet(string.Format("设置临时数据库连接"),string.Format("临时数据库主要是临时存储数据"));
            if (dBSet.ShowDialog() != DialogResult.OK) return;
            DBConfig TempDbConfig = dBSet.DbConfig;
            txtTempDbSet.Text = TempDbConfig.GetConfigName();
            string strMessage = m_sysConfig.UpdateSysConfig(ColumnName.TempDbConfigCode, TempDbConfig.GetConfigStr(), string.Format("临时库配置"));
            if (string.IsNullOrEmpty(strMessage))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 设置历史库连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSetHisDbConnect_Click(object sender, EventArgs e)
        {
            frmDBSet dBSet = new frmDBSet(string.Format("设置历史数据库连接"), string.Format("历史数据库主要是存储历史数据"));
            if (dBSet.ShowDialog() != DialogResult.OK) return;
            DBConfig HisConfig = dBSet.DbConfig;
            txtHisDbSet.Text = HisConfig.GetConfigName();
            string strMessage = m_sysConfig.UpdateSysConfig(ColumnName.HisDbConfigCode, HisConfig.GetConfigStr(), string.Format("历史库配置"));
            if (string.IsNullOrEmpty(strMessage))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strMessage,"提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }
        /// <summary>
        /// 选择数据库模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSelectDbTempLate_Click(object sender, EventArgs e)
        {
            frmDBSet dBSet = new frmDBSet(string.Format("选取数据库模板"),string.Format("数据库存储结构，用于初始化正式库、临时库和历史库"));
            if (dBSet.ShowDialog() != DialogResult.OK) return;
            DBConfig dbTemplateConfig = dBSet.DbConfig;
            string strConfigName = dbTemplateConfig.GetConfigName();
            if (m_dicTemplateDb.ContainsKey(strConfigName))
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(string.Format("当前模板列表已存在"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                m_dicTemplateDb.Add(strConfigName, dbTemplateConfig);
                CheckedListBoxItem item = new CheckedListBoxItem();
                item.Value = strConfigName;
                item.CheckState = CheckState.Unchecked;
                listDbTemplate.Items.Add(item);
            }
        }
        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInitailDb_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 删除选中的模板项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteSelect_Click(object sender, EventArgs e)
        {
            foreach (CheckedListBoxItem item in listDbTemplate.Items)
            {
                if (item.CheckState == CheckState.Checked)
                {
                    listDbTemplate.Items.Remove(item);
                    if(m_dicTemplateDb.ContainsKey(item.Value.ToString()))
                    m_dicTemplateDb.Remove(item.Value.ToString());
                }
            }
        }
        /// <summary>
        /// 清空所有数据库模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            listDbTemplate.Items.Clear();
            m_dicTemplateDb.Clear();
        }
        private void FromInitializeSet_Load(object sender, EventArgs e)
        {
            listDbTemplate.Items.Clear();
        }

    }
}
