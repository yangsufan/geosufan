using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
//ZQ  20110802   add



using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;

namespace GeoUtilities.Gis.Form
{
    public partial class frmExportDataByMapNO : DevComponents.DotNetBar.Office2007Form
    {
        //ZQ  20110801    Modify
        private string strDLGSrcPaht = Application.StartupPath + "\\..\\OrderTask\\DLGSrcPath.txt";
        private string strDEMSrcPaht = Application.StartupPath + "\\..\\OrderTask\\DEMSrcPath.txt";
        private string strDOMSrcPaht = Application.StartupPath + "\\..\\OrderTask\\DOMSrcPath.txt";
        //ZQ  20110801    Modify
        private IMapControlDefault m_MapControl; //被查询的地图对象 
        private IWorkspace m_WS = null;        //系统维护库连接工作空间
        public frmExportDataByMapNO(IList<string> strMapNOs, bool pVisble)
        {
            InitializeComponent();
            m_strMapNOs = strMapNOs;
          
            if(!pVisble)
            {
                chckBoxDDBH.Visible = false;
                txtDDBH.Visible = false;
            }
        }
        //ZQ  20110801    Modify
        public frmExportDataByMapNO(IList<string> strMapNOs, bool pVisble, bool bDLG, bool bDEM, bool bDOM, IMapControlDefault pMapControl, IWorkspace pWs)
        {
            InitializeComponent();
            m_strMapNOs = strMapNOs;
            m_MapControl = pMapControl;
            m_WS = pWs;
            if (!pVisble)
            {
                chckBoxDDBH.Visible = false;
                txtDDBH.Visible = false;
            }
            chkBoxDLG.Checked = bDLG;
            chkBoxDEM.Checked = bDEM;
            chkBoxDOM.Checked = bDOM;
        }
        //ZQ  20110801    Modify
        private void frmExportDataByMapNO_Load(object sender, EventArgs e)
        {
            this.txtDLGSrcPath.Text = GetSrcPath(strDLGSrcPaht);
            this.txtDEMSrcPath.Text = GetSrcPath(strDEMSrcPaht);//GetSrcPath(strDLGSrcPaht);
            this.txtDOMSrcPath.Text = GetSrcPath(strDOMSrcPaht);
            if (m_strMapNOs == null) return;

            for (int i = 0; i < m_strMapNOs.Count; i++)
            {
                this.lstMapNOs.Items.Add(m_strMapNOs[i],true);
            }
          
        }

        private string GetSrcPath(string strPath)
        {
            string strSrcPaht = "";
            if (File.Exists(strPath))
            {
                StreamReader sr = new StreamReader(strPath);
                strSrcPaht = sr.ReadLine();
                sr.Close();
            }

            return strSrcPaht;
        }

        private IList<string> m_strMapNOs = null;
        //ZQ  20110801    Modify
        private void btnOK_Click(object sender, EventArgs e)
        {
            List<string> strSrcFolder = new List<string>();
             string ListMapNo ="";
            if(chkBoxDLG.Checked)
            {
                strSrcFolder.Add(this.txtDLGSrcPath.Text.Trim());
            }
            if(chkBoxDEM.Checked)
            {
                strSrcFolder.Add(this.txtDEMSrcPath.Text.Trim());
            }
            if(chkBoxDOM.Checked)
            {
                strSrcFolder.Add(this.txtDOMSrcPath.Text.Trim());
            }
            string strDesPath = this.txtDesPath.Text;
            string strMapNO = this.txtMapNO.Text.Trim();
            double pSize =0;
            for (int pCount = 0; pCount < strSrcFolder.Count; pCount++)
            {
                //测试源路径    ZQ   20110801
                if (!System.IO.Directory.Exists(strSrcFolder[pCount].ToString().Trim()))
                {
                    MessageBox.Show("无法获得：" + strMapNO + "图幅数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //拷贝
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    for (int i = 0; i < this.lstMapNOs.Items.Count; i++)
                    {
                        //ZQ    20110804   add  当用户选中时认为要提取的数据
                        if (lstMapNOs.GetItemChecked(i))
                        {
                            strMapNO = this.lstMapNOs.Items[i].ToString();
                            //ZQ  20110801    add
                            if (i == 0)
                            {
                                ListMapNo = strMapNO;
                            }
                            else
                            {
                                ListMapNo = ListMapNo + "," + strMapNO;
                            }
                            //测试源路径
                            if (!System.IO.Directory.Exists(strSrcFolder[pCount].ToString().Trim() + "\\" + strMapNO))
                            {
                                if (MessageBox.Show("无法获得：" + strMapNO + "图幅数据,是否继续？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                                {
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            copyDirectory(strSrcFolder[pCount] + "\\" + strMapNO, strDesPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(strSrcFolder[pCount]) + "_" + strMapNO);
                            DirectoryInfo pDirectoryInfo = new DirectoryInfo(strDesPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(strSrcFolder[pCount]) + "_" + strMapNO);
                            FileInfo[] pFileInfo = pDirectoryInfo.GetFiles();
                            double isize = 1024 * 1024;
                            foreach (FileInfo f in pFileInfo)
                            {
                                pSize = pSize + Convert.ToDouble(f.Length) / isize;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("数据提取过程失败" + Environment.NewLine + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            //ZQ  20110801    add
                if (chckBoxDDBH.Checked == true)
                {
                    if (txtDDBH.Text == "")
                    {
                        MessageBox.Show("编号信息为空！", " 提示！");
                        this.Cursor = Cursors.Default;
                        this.DialogResult = DialogResult.OK;
                        return;
                    }
                    else
                    {
                        if (MessageBox.Show("数据提取完成，是否创建订单", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            //frmOrderTask pfrmOrderTask = new frmOrderTask(m_MapControl, m_WS, chkBoxDLG.Checked, chkBoxDEM.Checked, chkBoxDOM.Checked, pSize.ToString(), txtDDBH.Text, ListMapNo);
                            //pfrmOrderTask.ShowDialog();
                        }

                    }

                }
                this.Cursor = Cursors.Default;

                this.DialogResult = DialogResult.OK;
          
            
        }

        public static void copyDirectory(string Src, string Dst)
        {
            String[] Files;

            if (Dst[Dst.Length - 1] != Path.DirectorySeparatorChar)
                Dst += Path.DirectorySeparatorChar;
            if (!Directory.Exists(Dst)) Directory.CreateDirectory(Dst);
            Files = Directory.GetFileSystemEntries(Src);
            foreach (string Element in Files)
            {
                // Sub directories

                if (Directory.Exists(Element))
                    copyDirectory(Element, Dst + Path.GetFileName(Element));
                // Files in directory

                else
                    File.Copy(Element, Dst + Path.GetFileName(Element), true);
            }
        }
        #region  ZQ   20110804   add 
        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog vFolder = new FolderBrowserDialog();
            if (vFolder.ShowDialog() != DialogResult.OK) return;

            this.txtDLGSrcPath.Text = vFolder.SelectedPath;
        }
        private void btnOpenDEMFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog vFolder = new FolderBrowserDialog();
            if (vFolder.ShowDialog() != DialogResult.OK) return;

            this.txtDEMSrcPath.Text = vFolder.SelectedPath;
        }

        private void btnOpenDOMFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog vFolder = new FolderBrowserDialog();
            if (vFolder.ShowDialog() != DialogResult.OK) return;

            this.txtDOMSrcPath.Text = vFolder.SelectedPath;
        }
        #endregion
        private void btnSave_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog vFolder = new FolderBrowserDialog();
            if (vFolder.ShowDialog() != DialogResult.OK) return;

            this.txtDesPath.Text = vFolder.SelectedPath;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //if (this.txtMapNO.Text.Trim() == ""&&this.txtMapNO.Text.Length!=10) return;
            //this.lstMapNOs.Items.Add(this.txtMapNO.Text);
            //ZQ  20110801    add    
            OpenFileDialog pOpenFileDialog = new OpenFileDialog();
            pOpenFileDialog.Filter = "*.txt|*.txt";
            if (pOpenFileDialog.ShowDialog()==DialogResult.OK)
            {
                if(File.Exists(pOpenFileDialog.FileName.ToString()))
                {
                    string[] strMapNo = GetSrcPath(pOpenFileDialog.FileName.ToString()).Split(new char[]{','});
                    for (int i = 0; i < strMapNo.Length;i++ )
                    {
                        if (strMapNo[i].Length != 10)
                        {
                            MessageBox.Show("输入的图幅号或图幅号之间逗号格式不正确"," 提示！");
                            break;
                        }
                        this.lstMapNOs.Items.Add(strMapNo[i],true);
                    }

                }
            }
            //end
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            IList<string> lstSels = new List<string>();
            for (int i = 0; i < this.lstMapNOs.SelectedItems.Count; i++)
            {
                if (!lstSels.Contains(this.lstMapNOs.SelectedItems[i].ToString())) lstSels.Add(this.lstMapNOs.SelectedItems[i].ToString());
            }

            for (int i = 0; i < lstSels.Count; i++)
            {
                for (int j = 0; j < this.lstMapNOs.Items.Count; j++)
                {
                    if (this.lstMapNOs.Items[j].ToString() == lstSels[i])
                    {
                        this.lstMapNOs.Items.RemoveAt(j);
                        break;
                    } 
                }
            }
        }

        private void txtMapNO_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if (this.txtMapNO.Text.Trim() == "" && this.txtMapNO.Text.Length != 10) return;
                this.lstMapNOs.Items.Add(this.txtMapNO.Text,true);
            }
        }
       

        #region   订单编号与矢量数据提取联系
        private void chckBoxDDBH_CheckedChanged(object sender, EventArgs e)
        {
            if (chckBoxDDBH.Checked == true)
            {
                txtDDBH.Enabled = true;
            }
            else
            {
                txtDDBH.Enabled = false;
            }

        }
        #endregion
        #region ZQ  20110804 add   提供全选和反选功能
        private void bttAll_Click(object sender, EventArgs e)
        {
            if(lstMapNOs.Items.Count>0)
            {
                for (int i = 0; i < lstMapNOs.Items.Count;i++ )
                {
                 lstMapNOs.SetItemChecked(i,true);
                }
            }
        }

        private void bttinstead_Click(object sender, EventArgs e)
        {
            if (lstMapNOs.Items.Count > 0)
            {
                for (int i = 0; i < lstMapNOs.Items.Count; i++)
                {
                    if (lstMapNOs.GetItemChecked(i))
                    {
                        lstMapNOs.SetItemChecked(i, false);
                    }
                    else
                    {
                        lstMapNOs.SetItemChecked(i, true);
                    }
                    
                }
            }
        }
        #endregion

      
    }
}