using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using SysCommon.Gis;
using SysCommon.Error;
///ZQ  20111010  add 成果数据目录配置
namespace GeoDataManagerFrame
{
    public partial class FrmSetResultFileDir : DevComponents.DotNetBar.Office2007Form
    {

        IWorkspace m_Workspace = null;
        public FrmSetResultFileDir(IWorkspace pWorkspace)
        {
            InitializeComponent();
            m_Workspace = pWorkspace;
        }

        private void btn_openXlsDir_Click(object sender, EventArgs e)
        {
            textBox_XLS.Text = GetFolderBrowserDialogPath();
        }

        private void btn_openJpgDir_Click(object sender, EventArgs e)
        {
            textBox_Jpg.Text = GetFolderBrowserDialogPath();
        }
        private void buttonXQuit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose(true);
        }
        private void buttonXOK_Click(object sender, EventArgs e)
        {
            if (textBox_Jpg.Text == "" || textBox_XLS.Text == "") { MessageBox.Show("请设置成果数据目录信息！","提示！"); return; }
            Exception exError = null;
            SysGisTable sysTable = new SysGisTable(m_Workspace);
            Dictionary<string, object> dicData = new Dictionary<string, object>();
            dicData.Add("DATATYPE", "文档成果数据");
            dicData.Add("DATAPATH", textBox_XLS.Text);
            //判断是更新还是添加
            //不存在则添加，已存在则更新
            if (!sysTable.ExistData("RESULTDIR", "DATATYPE='文档成果数据'"))
            {
                if (!sysTable.NewRow("RESULTDIR", dicData, out exError))
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", "添加失败！");
                  
                }
            }
            else
            {
                if (MessageBox.Show("已经存在文档成果数据目录信息，是否进行更新", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (!sysTable.UpdateRow("RESULTDIR", "DATATYPE='文档成果数据'", dicData, out exError))
                    {
                        ErrorHandle.ShowFrmErrorHandle("提示", "更新失败！");
                    }
                }
            }
            dicData = new Dictionary<string, object>();
            dicData.Add("DATATYPE", "图件成果数据");
            dicData.Add("DATAPATH", textBox_Jpg.Text);
            //判断是更新还是添加
            //不存在则添加，已存在则更新
            if (!sysTable.ExistData("RESULTDIR", "DATATYPE='图件成果数据'"))
            {
                if (!sysTable.NewRow("RESULTDIR", dicData, out exError))
                {
                    ErrorHandle.ShowFrmErrorHandle("提示", "添加失败！");
                    return;
                }
            }
            else
            {
                if (MessageBox.Show("已经存在图件成果数据目录信息，是否进行更新", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (!sysTable.UpdateRow("RESULTDIR", "DATATYPE='图件成果数据'", dicData, out exError))
                    {
                        ErrorHandle.ShowFrmErrorHandle("提示", "更新失败！");
                        return;
                    }
                }
            }
            sysTable =null;
            this.Close();
            this.Dispose(true);
        }
        /// <summary>
        /// 获取上传目标文件夹路径
        /// </summary>
        /// <returns></returns>
        private string GetFolderBrowserDialogPath()
        {
            string strFilePath = "";
            FolderBrowserDialog pFolderBrowserDialog = new FolderBrowserDialog();
            if (pFolderBrowserDialog.ShowDialog() != DialogResult.OK)
                return strFilePath = "";
                strFilePath = pFolderBrowserDialog.SelectedPath;
                return strFilePath ;
        }

       

      
    }
}
