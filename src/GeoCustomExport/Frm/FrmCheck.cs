using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace GeoCustomExport
{
    /// <summary>
    /// 选择数据集
    /// </summary>
    public partial class FrmCheck : DevComponents.DotNetBar.Office2007Form
    {
        public FrmCheck(string sLog)
        {
            InitializeComponent();
            this.Text = "校核结果--不存在的图层和字段";
            richTextBox1.Text = sLog;
        }


   



        private void btn_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "文本文档|*.txt";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SaveFile(dlg.FileName, RichTextBoxStreamType.PlainText);
                MessageBox.Show("保存完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



    }
}
