using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;



using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using SysCommon.Gis;
using System.Diagnostics;
//ZQ  20111010  成果数据上传
namespace GeoDataManagerFrame
{
    public partial class FrmResultDataUploading : DevComponents.DotNetBar.Office2007Form
    {
        IWorkspace m_Workspace = null;
        public FrmResultDataUploading(IWorkspace pWorkspace)
        {
            InitializeComponent();
            m_Workspace = pWorkspace;

            //初始化文件夹树
            InitTree();
        }

        //初始化树
        public void InitTree()
        {
            ////创建根节点
            treeView1.Nodes.Clear();
            TreeNode tparent;
            TreeNode childNode;
            tparent = new TreeNode();
            tparent.Text = "成果数据目录";
            tparent.Tag = 0;
            tparent.ImageIndex = 0;
            tparent.Expand();
            tparent.ExpandAll();
            treeView1.Nodes.Add(tparent);

            Exception exError0 = null;
            Exception exError = null;
            //获取数据源中的RESULTDIR中的PATHFILE
          
            if(m_Workspace != null)
            {
                SysGisTable sysTable = new SysGisTable(m_Workspace);

                ///获得对应成果数据上传的目标路径
                if (sysTable.ExistData("RESULTDIR", null))
                {
                    //获取每一行
                    List<Dictionary<string, object>> pLst = sysTable.GetRows("RESULTDIR", null, out exError);

                    List<object> pListFileName = sysTable.GetFieldValues("RESULTDIR", "PATHNAME", null, out exError0);
                    if (pListFileName.Count > 0)
                    {
                        //获取每一列
                        for (int ii = 0; ii < pLst.Count; ii++)
                        {
                            childNode = new TreeNode();
                            childNode.Text = pListFileName[ii].ToString();

                            childNode.Expand();
                            childNode.Tag = 1;
                            childNode.ImageIndex = 1;
                            childNode.SelectedImageIndex = 2;
                            tparent.Nodes.Add(childNode);
                        } 
                    }
                 
                }
            }

            treeView1.Refresh();

        }
        #region    数据源设置
        private void bttOpenFile_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog sOpenFileD = new System.Windows.Forms.OpenFileDialog();
            sOpenFileD.CheckFileExists = true;
            sOpenFileD.CheckPathExists = true;
            sOpenFileD.Multiselect = true;
            sOpenFileD.Title = "选择数据源";
            sOpenFileD.Filter = "Excel 97-2003 工作薄 (*.xls)|*.xls|Excel 工作薄(*.xlsx)|*.xlsx|JPEG (*.jpg)|*.jpg";
        
            int m = 1;
            if (sOpenFileD.ShowDialog() == DialogResult.OK)
            {
                string[] strFileName = sOpenFileD.FileNames;
                for (int j = 0; j < strFileName.Length; j++)
                {
                    for (int i = 0; i < dataGrid.RowCount ; i++)
                    {
                        if (strFileName[j].ToString() == dataGrid.Rows[i].Cells["CmnPath"].Value.ToString())
                        {
                            if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                break;
                            }
                            else
                            {
                                m = 0;
                                break;
                            }
                        }

                    }
                    if (m == 1)
                    {
                        dataGrid.Rows.Add(true, strFileName[j].ToString(),"未上传");
                    }
                }
            }

            if (dataGrid.RowCount > 0)
            {
              bttRemove.Enabled = true;
             bttAllRemove.Enabled = true;
            }
        }

        private void bttOpenFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog pFolderBrowserDialog = new FolderBrowserDialog();
            int m = 1;
            if (pFolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string[] pFilePath =null;
                //pFilePath = Directory.GetFiles(pFolderBrowserDialog.SelectedPath.ToString(), "*.xlsx", SearchOption.TopDirectoryOnly);
                
                //if (pFilePath.Length == 0)
                //{
                //    for (int i = 0; i < dataGrid.RowCount; i++)
                //    {
                //        if (pFolderBrowserDialog.SelectedPath.ToString() == dataGrid.Rows[i].Cells["CmnPath"].Value.ToString())
                //        {
                //            if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                //            {
                //                break;
                //            }
                //            else
                //            {
                //                m = 0;
                //                break;
                //            }
                //        }
                //    }
                //    if (m == 1)
                //    {
                //        dataGrid.Rows.Add(true, pFolderBrowserDialog.SelectedPath.ToString(), "未上载");
                //    }
                //}
                //else
                //{
                //    for (int j = 0; j < pFilePath.Length; j++)
                //    {
                //        for (int i = 0; i < dataGrid.RowCount; i++)
                //        {
                //            if (pFilePath[j].ToString() == dataGrid.Rows[i].Cells["CmnPath"].Value.ToString())
                //            {
                //                if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                //                {
                //                    break;
                //                }
                //                else
                //                {
                //                    m = 0;
                //                    break;
                //                }
                //            }
                //        }
                //        if (m == 1)
                //        {
                //            dataGrid.Rows.Add(true, pFilePath[j].ToString(), "未上载");
                //        }
                //    }
                //}

                pFilePath = Directory.GetFiles(pFolderBrowserDialog.SelectedPath.ToString(), "*.xls", SearchOption.TopDirectoryOnly);
                m = 1;
                if (pFilePath.Length == 0)
                {
                    for (int i = 0; i < dataGrid.RowCount; i++)
                    {
                        if (pFolderBrowserDialog.SelectedPath.ToString() == dataGrid.Rows[i].Cells["CmnPath"].Value.ToString())
                        {
                            if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                break;
                            }
                            else
                            {
                                m = 0;
                                break;
                            }
                        }
                    }
                    if (m == 1)
                    {
                        dataGrid.Rows.Add(true, pFolderBrowserDialog.SelectedPath.ToString(), "未上传");
                    }
                }
                else
                {
                    for (int j = 0; j < pFilePath.Length; j++)
                    {
                        for (int i = 0; i < dataGrid.RowCount; i++)
                        {
                            if (pFilePath[j].ToString() == dataGrid.Rows[i].Cells["CmnPath"].Value.ToString())
                            {
                                if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                {
                                    break;
                                }
                                else
                                {
                                    m = 0;
                                    break;
                                }
                            }
                        }
                        if (m == 1)
                        {
                            dataGrid.Rows.Add(true, pFilePath[j].ToString(), "未上传");
                        }
                    }
                }
                pFilePath = Directory.GetFiles(pFolderBrowserDialog.SelectedPath.ToString(), "*.jpg", SearchOption.TopDirectoryOnly);
                m = 1;
                if (pFilePath.Length == 0)
                {
                    for (int i = 0; i < dataGrid.RowCount; i++)
                    {
                        if (pFolderBrowserDialog.SelectedPath.ToString() == dataGrid.Rows[i].Cells["CmnPath"].Value.ToString())
                        {
                            if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                break;
                            }
                            else
                            {
                                m = 0;
                                break;
                            }
                        }
                    }
                    if (m == 1)
                    {
                        dataGrid.Rows.Add(true, pFolderBrowserDialog.SelectedPath.ToString(), "未上传");
                    }
                }
                else
                {
                    for (int j = 0; j < pFilePath.Length; j++)
                    {
                        for (int i = 0; i < dataGrid.RowCount; i++)
                        {
                            if (pFilePath[j].ToString() == dataGrid.Rows[i].Cells["CmnPath"].Value.ToString())
                            {
                                if (MessageBox.Show("该数据已经存在，是否再次添加", "提示！", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                {
                                    break;
                                }
                                else
                                {
                                    m = 0;
                                    break;
                                }
                            }
                        }
                        if (m == 1)
                        {
                            dataGrid.Rows.Add(true, pFilePath[j].ToString(), "未上传");
                        }
                    }
                }
            }

            if (dataGrid.RowCount > 0)
            {
                bttRemove.Enabled = true;
                bttAllRemove.Enabled = true;
            }
        }

        private void btnAllSelected_Click(object sender, EventArgs e)
        {
            if (dataGrid.RowCount > 0)
            {
                for (int i = 0; i < dataGrid.RowCount; i++)
                {
                    dataGrid.Rows[i].Cells["CmnSelect"].Value = true;
                }
            }
        }

        private void btnOtherSelected_Click(object sender, EventArgs e)
        {
            if (dataGrid.RowCount > 0)
            {
                for (int i = 0; i < dataGrid.RowCount; i++)
                {
                    if (dataGrid.Rows[i].Cells["CmnSelect"].Value.ToString() == true.ToString())
                    {
                         dataGrid.Rows[i].Cells["CmnSelect"].Value = false;
                    }
                    else
                    {
                        dataGrid.Rows[i].Cells["CmnSelect"].Value = true;
                    }
                }
            }
        }


        private void bttRemove_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection pSelectedRowC = dataGrid.SelectedRows;
            if(pSelectedRowC.Count==0){return;}
            try
            {
                /////删除用户选中的项
                for (int i = 0; i < pSelectedRowC.Count; i++)
                {
                    dataGrid.Rows.RemoveAt(pSelectedRowC[i].Index);
                }
                if (dataGrid.RowCount == 0)
                {
                    bttRemove.Enabled = false;
                    bttAllRemove.Enabled = false;
                }
            }
            catch { }
        }

        private void bttAllRemove_Click(object sender, EventArgs e)
        {
            dataGrid.Rows.Clear();
            bttRemove.Enabled = false;
            bttAllRemove.Enabled = false;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose(true);

        }
        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGrid.Rows.Clear();
            bttRemove.Enabled = false;
            bttAllRemove.Enabled = false;
        }
        #endregion

        #region 数据上传
        /// <summary>
        /// 数据拷贝
        /// </summary>
        /// <param name="Src"></param>
        /// <param name="Dst"></param>
        public  void copyDirectory(string Src, string Dst)
        {
            try
            {
                if (File.Exists(Src))
                    File.Copy(Src, Dst, true);
            }
            catch { }
        }
        /// <summary>
        /// 添加或更新一条记录
        /// </summary>
        /// <param name="pSysTable"></param>
        /// <param name="strType"></param>
        /// <param name="strFilePath"></param>
        /// <param name="eError"></param>
        private void CreateRow(SysGisTable pSysTable, string strType, string strFilePath, out Exception exError)
        {
            Dictionary<string, object> dicData = new Dictionary<string, object>();
            dicData.Add("DATATYPE", strType);
            dicData.Add("DATANAME", System.IO.Path.GetFileName(strFilePath));
            dicData.Add("DATAPATH", strFilePath);
            //判断是更新还是添加
            //不存在则添加，已存在则更新
            if (!pSysTable.ExistData("RESULTLIST", "DATANAME='"+ System.IO.Path.GetFileName(strFilePath) +"'"))
            {
                if (!pSysTable.NewRow("RESULTLIST", dicData, out exError))
                {
                   return;
                }
            }
            else
            {
                if (!pSysTable.UpdateRow("RESULTLIST", "DATANAME='" + System.IO.Path.GetFileName(strFilePath) + "'", dicData, out exError))
                {
                    return;
                }
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            ///获得上传成果数据的类型
            if (treeView1.SelectedNode == null)
            {
                MessageBox.Show("请在左边目录树中选择目标文件夹！", "提示！"); 
            }
             string strcmbType = treeView1.SelectedNode.Text;
            if (dataGrid.Rows.Count == 0)
            {
                MessageBox.Show("请添加"+strcmbType+"！","提示！"); 
                return;
            }
            Exception exError = null;
            SysGisTable sysTable = new SysGisTable(m_Workspace);
            ///获得对应成果数据上传的目标路径
            string strUploading = null;
            if (sysTable.ExistData("RESULTDIR", "PATHNAME='" + strcmbType + "'"))
            {
                strUploading = sysTable.GetFieldValue("RESULTDIR", "DATADIR", "PATHNAME='" + strcmbType + "'", out exError).ToString();
            }
           if(strUploading==null||File.Exists(strUploading))
           {
               MessageBox.Show("请先配置成果数据目录！","提示！"); 
             return;
           }
          
           SysCommon.CProgress vProgress = new SysCommon.CProgress("进度条");
           vProgress.EnableCancel = false;//设置进度条
           vProgress.ShowDescription = true;
           vProgress.FakeProgress = true;
           vProgress.TopMost = true;
           vProgress.MaxValue = dataGrid.RowCount;
           vProgress.ShowProgress();
           try
           {
               for (int i = 0; i < dataGrid.RowCount; i++)
               {
                   vProgress.ProgresssValue = i + 1;
                   if (dataGrid.Rows[i].Cells["CmnSelect"].Value.ToString() == true.ToString())
                   {
                       string strSource = dataGrid.Rows[i].Cells["CmnPath"].Value.ToString();
                       string strUpPath = strUploading + "\\" + System.IO.Path.GetFileName(strSource);
                       ///判断上传目标路径下是否已存在同名的文件
                       if (!File.Exists(strUpPath))
                       {
                           ////判断上传的文件是否存在
                           if (System.IO.File.Exists(strSource))
                           {
                               vProgress.SetProgress("正在上传数据：" + System.IO.Path.GetFileName(strSource));
                               ///文件拷贝
                               copyDirectory(strSource, strUpPath);
                               dataGrid.Rows[i].Cells["CmnState"].Value = "已上传";
                               dataGrid.Rows[i].Cells["CmnSelect"].Value = false;
                               CreateRow(sysTable, strcmbType, strUpPath, out exError);
                           }
                           else
                           {
                               dataGrid.Rows[i].Cells["CmnState"].Value = "源文件不存在";
                           }
                       }
                       else
                       {
                           dataGrid.Rows[i].Cells["CmnState"].Value = "已存在同名文件";
                       }

                   }
               }
               vProgress.Close();
               sysTable = null;
           }
           catch { vProgress.Close(); }

        }
       
        #endregion



 

        //新增
        private void toolStripMenuAdd_Click(object sender, EventArgs e)
        {
            FrmAddResultDir frm = new FrmAddResultDir(m_Workspace);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                 InitTree();
                //新增添加成功提示框 ygc 2012-9-6
                 MessageBox.Show("增加成果目录成功！","提示");
            }
           

        }

        private void toolStripMenuOpen_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuDel_Click(object sender, EventArgs e)
        {
            try
            {
                //删除记录
                Exception eError = null;
                string strTreeText = treeView1.SelectedNode.Text;
                if (m_Workspace != null)
                {
                    //增加删除提示 ygc 2012-9-6
                   DialogResult result= MessageBox.Show("该操作将会删除该成果目录及该成果目录下所有成果文件，确定删除？","提示",MessageBoxButtons.OKCancel ,MessageBoxIcon.Information );
                   if (result != DialogResult.OK)
                   { return; }
                    //判断名称是否存在
                    string strPath = "";
                    SysGisTable sysTable = new SysGisTable(m_Workspace);
                    if (sysTable.ExistData("RESULTDIR", null))
                    {

                        string strComputerIp = sysTable.GetFieldValue("RESULTDIR", "COMPUTERIP", "PATHNAME='" + strTreeText + "'", out eError).ToString();
                        if (PingEx(strComputerIp))
                        {
                            //删除实际文件夹  获取 DATADIR
                            strPath = sysTable.GetFieldValue("RESULTDIR", "DATADIR", "PATHNAME='" + strTreeText + "'", out eError).ToString();
                            if (Directory.Exists(strPath))
                            {
                                Directory.Delete(strPath, true);
                            }
                            //获取成果文件 并将其从数据库中删除 ygc 2012-9-6
                            List<object> fileList = sysTable.GetFieldValues("RESULTLIST", "DATATYPE", "DATATYPE='" + strTreeText + "'", out eError);
                            if (fileList.Count >0)
                            {
                                sysTable.DeleteRows("RESULTLIST","DATATYPE='"+fileList[0].ToString ()+"'",out eError);
                                
                            }//end
                            sysTable.DeleteRows("RESULTDIR", "PATHNAME='" + strTreeText + "'", out eError);
                            InitTree();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString ());
                return;
            }
            //新增操作成功提示 ygc 2012-9-6
            MessageBox.Show("删除该成果目录成功！","提示");
        }

        public static bool PingEx(string remoteHost)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                //string dosLine = @"ping -n 1 " + remoteHost;
                string dosLine = @"ping -n 1 " + remoteHost;
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (proc.HasExited == false)
                {
                    proc.WaitForExit(500);
                }
                string pingResult = proc.StandardOutput.ReadToEnd();
                if (pingResult.IndexOf("(0% loss)") != -1 || pingResult.IndexOf("(0% 丢失)") != -1)
                {
                    Flag = true;
                }
                proc.StandardOutput.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                try
                {
                    proc.Close();
                    proc.Dispose();
                }
                catch
                {
                }
            }
            return Flag;
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode = e.Node;
        }


    }
}
