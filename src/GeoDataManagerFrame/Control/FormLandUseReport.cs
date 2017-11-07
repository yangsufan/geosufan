using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using GeoDataCenterFunLib;
namespace GeoDataManagerFrame
{
    public partial class FormLandUseReport : DevComponents.DotNetBar.Office2007Form
    {
        public FormLandUseReport()
        {
            InitializeComponent();
            InitDataSource();
        }
        private void InitDataSource()
        {

            string path = Application.StartupPath + "\\..\\OutputResults\\汇总成果\\森林资源现状汇总" ;
            DirectoryInfo pathinfo = new DirectoryInfo(path);
            if (Directory.Exists(path))
            {
                foreach (FileInfo Finfo in pathinfo.GetFiles("*.mdb"))
                {
                    string DataSourceName = Finfo.Name.Substring(0, Finfo.Name.IndexOf("."));
                    this.comboBoxExDataSource.Items.Add(DataSourceName);
                }
            }

        }
        private void buttonXOK_Click(object sender, EventArgs e)
        {
            if (this.comboBoxExAreaName.Text.Equals(""))
            {
                MessageBox.Show("请选择面积单位！");
                return;
            }
            if (this.comboBoxExDLGrade.Text.Equals(""))
            {
                MessageBox.Show("请选择地类级别！");
                return;
            }
            if (this.comboBoxExDataSource.Text.Equals(""))
            {
                MessageBox.Show("请选择汇总数据源！");
                return;
            }
            string DataSourceName = this.comboBoxExDataSource.Text;
            int index1=DataSourceName.IndexOf("(");
            int index2=DataSourceName.IndexOf(")");
            string xzqcode = DataSourceName.Substring(index1+1,index2-index1-1);
            string DataSourcePath =Application.StartupPath + "\\..\\OutputResults\\汇总成果\\森林资源现状汇总";
            string AreaName = this.comboBoxExAreaName.Text;
            string dlGrade = this.comboBoxExDLGrade.Text;
            int iDLJB=1;
            switch (dlGrade)
            {
                case "一级":
                    iDLJB = 1;
                    break;
                case "二级":
                    iDLJB = 2;
                    break;
                default:
                    break;
            }
            //初始化进度条
            SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
            vProgress.SetProgress("统计农村森林资源现状分类面积汇总表");
            ModStatReport.LandUseCurReport(DataSourcePath, DataSourceName + ".mdb", xzqcode, AreaName,2, iDLJB,"",vProgress);
            this.DialogResult = DialogResult.OK;
        }

        private void buttonXQuit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}