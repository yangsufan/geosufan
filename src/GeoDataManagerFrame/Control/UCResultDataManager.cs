using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
using System.Diagnostics;

namespace GeoDataManagerFrame
{
    public partial class UCResultDataManager : UserControl
    {
        //用于存储修改后的文件名
        public string m_ModifyName = "";
        public UCResultDataManager()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 获取业务库
        /// </summary>
        IWorkspace m_Workspace = null;
        public IWorkspace pWorkspace
        {
            set
             {
                 m_Workspace = value;
             }
        }
        //获取数据库的数据类型（ORACLE MDB GDB）
        public static string GetDescriptionOfWorkspace(IWorkspace pWorkspace)
        {
            string strLike = "%";
            if (pWorkspace == null)
            {
                return strLike = "%";
            }
            IWorkspaceFactory pWorkSpaceFac = pWorkspace.WorkspaceFactory;
            if (pWorkSpaceFac == null)
            {
                return strLike = "%";
            }
            string strDescrip = pWorkSpaceFac.get_WorkspaceDescription(false);
            switch (strDescrip)
            {
                case "Personal Geodatabase"://mdb数据库 使用*作匹配符
                    strLike = "*";
                    break;
                case "File Geodatabase"://gdb数据库 使用%作匹配符
                    strLike = "%";
                    break;
                case "Spatial Database Connection"://sde(oracle数据库) 使用%作匹配符(sql server数据库，现在暂未考虑)
                    strLike = "%";
                    break;
                default:
                    strLike = "%";
                    break;
            }
            return strLike;
        }
        /// <summary>
        /// 获得查询结果游标
        /// </summary>
        /// <param name="pWorkspace">工作空间</param>
        /// <param name="plstField">查询的字段列表</param>
        /// <param name="strKey">关键字</param>
        /// <param name="strTableName">表名</param>
        /// <param name="strLike">模糊查询通配符</param>
        /// <returns></returns>
        private ICursor GetCursor(IWorkspace pWorkspace, List<string> plstField, string strKey, string strTableName, string strLike)
        {
            
            ICursor pCursor = null;
            try
            {
                if (plstField.Count == 0) { return pCursor; }
                IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                ITable pTable = pFeatureWorkspace.OpenTable(strTableName);
                IQueryFilter pQueryFilter = new QueryFilterClass();
                for (int i = 0; i < plstField.Count; i++)
                {
                    int pIndex = pTable.FindField(plstField[i].ToString());
                    if (pIndex == -1) { continue; }
                    if (pQueryFilter.WhereClause == "")
                    {
                        pQueryFilter.WhereClause = plstField[i].ToString() + " Like '" + strLike + strKey + strLike + "'";
                    }
                    else
                    {
                        pQueryFilter.WhereClause = pQueryFilter.WhereClause + " or " + plstField[i].ToString() + " Like '" + strLike + strKey + strLike + "'";
                    }
                }
                pCursor = pTable.Search(pQueryFilter, false);
                return pCursor;
            }
            catch { return pCursor; }
          
        }
        private void Query()
        {
            dataGridVRe.Rows.Clear();
            string strLik = GetDescriptionOfWorkspace(m_Workspace);
            List<string> LstField = new List<string>();
            LstField.Add("DATANAME"); LstField.Add("DATATYPE");
            ICursor pCursor = GetCursor(m_Workspace, LstField, txtKeys.Text.Trim(), "RESULTLIST", strLik);
            try
            {
                IRow pRow = pCursor.NextRow();
                while (pRow != null)
                {
                    int iName = pRow.Fields.FindField("DATANAME");
                    int iType = pRow.Fields.FindField("DATATYPE");
                    int iPath = pRow.Fields.FindField("DATAPATH");
                    if (iName == -1 || iName == -1 || iPath == -1)
                    {

                        MessageBox.Show("系统表被损坏！", "提示！");
                        return;
                    }
                    else
                    {
                        dataGridVRe.Rows.Add(pRow.get_Value(iName), pRow.get_Value(iType), pRow.get_Value(iPath));
                    }
                    pRow = pCursor.NextRow();
                }
            }
            catch { }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                pCursor = null;
            }

        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="Src"></param>
        public void DeleteFile(string Src)
        {
            try
            {
                if (File.Exists(Src))
                {
                    File.Delete(Src);
                }    
            }
            catch { }
        }
        /// <summary>
        /// 从系统表中删除该条记录
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <param name="condition">查询条件</param>
        /// <returns></returns>
        public bool DeleteTableRow(string strTableName,string condition)
        {
            Exception exError = null;
            bool Bdelete = true;
            SysCommon.Gis.SysGisTable sysTable = new SysCommon.Gis.SysGisTable(m_Workspace);
            if (sysTable.ExistData(strTableName, condition))
            {
                if (!sysTable.DeleteRows(strTableName, condition,out  exError))
                {
                    return Bdelete = false;
                }
            }
            sysTable = null;
            return Bdelete;
        }
        /// <summary>
        /// 更新系统表的数据记录
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="condition"></param>
        /// <param name="dicValues"></param>
        /// <returns></returns>
        public bool UpTableRow(string tablename, string condition, Dictionary<string, object> dicValues)
        {
            Exception exError = null;
            bool Bdelete = true;
            SysCommon.Gis.SysGisTable sysTable = new SysCommon.Gis.SysGisTable(m_Workspace);
            if (sysTable.ExistData(tablename, condition))
            {
                return Bdelete = sysTable.UpdateRow(tablename, condition, dicValues, out  exError);
            }
            else
            {
                Bdelete = false;
            }
            sysTable = null;
            return Bdelete;
        }
        private void bttQuery_Click(object sender, EventArgs e)
        {
            Query();
        }
        private void txtKeys_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Query();
            }
        }
        private void bttOpen_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection pSelectedRowC = dataGridVRe.SelectedRows;
            if (pSelectedRowC.Count != 1) { MessageBox.Show("请选中一条相关的记录再进行查看操作！", "提示！"); return; }
            string strPath = dataGridVRe.Rows[pSelectedRowC[0].Index].Cells["CumFilePath"].Value.ToString();
            string DataType = dataGridVRe.Rows[pSelectedRowC[0].Index].Cells["CmnType"].Value.ToString();
            string ShardPath = GetSharedPath(DataType);
            if (ShardPath==null) {MessageBox .Show ("业务库中不存在该共享目录！","提示"); return ;}
            SysCommon.Gis.SysGisTable sysTable = new SysCommon.Gis.SysGisTable(m_Workspace);
            Exception ex=null ;
            if (!Directory.Exists(ShardPath))
            {
                Dictionary<string, object> dicDataRow = new Dictionary<string, object>();
                dicDataRow=sysTable.GetRow("RESULTDIR", "PATHNAME='" + DataType + "'", out ex);
                bool t = Connect(dicDataRow["COMPUTERIP"].ToString(), dicDataRow["USER_"].ToString(), dicDataRow["PASSWORD_"].ToString());
            }

            if (!Directory.Exists(ShardPath))
            {
                MessageBox.Show("无法连接到成果目录","提示");
                return;
            }

            if (!File.Exists(strPath)) { MessageBox.Show("成果数据目录已被损坏！", "提示！"); return; }
            System.Diagnostics.Process.Start(strPath);
        }

        private void bttModify_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection pSelectedRowC = dataGridVRe.SelectedRows;
            if (pSelectedRowC.Count != 1) { MessageBox.Show("请选中一条相关的记录再进行修改操作！", "提示！"); return; }
            string strPath = dataGridVRe.Rows[pSelectedRowC[0].Index].Cells["CumFilePath"].Value.ToString();
            string DataType = dataGridVRe.Rows[pSelectedRowC[0].Index].Cells["CmnType"].Value.ToString();
            string ShardPath = GetSharedPath(DataType);
            if (ShardPath == null) { MessageBox.Show("业务库中不存在该共享目录！", "提示"); return; }
            SysCommon.Gis.SysGisTable sysTable = new SysCommon.Gis.SysGisTable(m_Workspace);
            Exception ex = null;
            if (!Directory.Exists(ShardPath))
            {
                Dictionary<string, object> dicDataRow = new Dictionary<string, object>();
                dicDataRow = sysTable.GetRow("RESULTDIR", "PATHNAME='" + DataType + "'", out ex);
                bool t = Connect(dicDataRow["COMPUTERIP"].ToString(), dicDataRow["USER_"].ToString(), dicDataRow["PASSWORD_"].ToString());
            }

            if (!Directory.Exists(ShardPath))
            {
                MessageBox.Show("无法连接到成果目录", "提示");
                return;
            }
            if (!File.Exists(strPath)) { MessageBox.Show("成果数据目录已被损坏！", "提示！"); return; }
            string strName = dataGridVRe.Rows[pSelectedRowC[0].Index].Cells["CumFileName"].Value.ToString();
            string strType = dataGridVRe.Rows[pSelectedRowC[0].Index].Cells["CmnType"].Value.ToString();

            FrmModifyName pFrmModifyName = new FrmModifyName(this,strPath);
            pFrmModifyName.strModifyValue = Path.GetFileNameWithoutExtension(strPath);
            if (pFrmModifyName.ShowDialog() == DialogResult.Cancel) { return; }
            ///重组修改后的文件名和文件路径
            m_ModifyName = m_ModifyName + Path.GetExtension(strPath);
            strPath = Path.GetDirectoryName(strPath) + "\\" + m_ModifyName;
            Dictionary<string, object> dicData = new Dictionary<string, object>();
            dicData.Add("DATATYPE", strType);
            dicData.Add("DATANAME", m_ModifyName);
            dicData.Add("DATAPATH", strPath);
            
            ///更新系统表中得数据
            if (UpTableRow("RESULTLIST", "DATANAME='" + strName + "'", dicData))
            {
               ///文件进行重命名
                File.Move(dataGridVRe.Rows[pSelectedRowC[0].Index].Cells["CumFilePath"].Value.ToString(),strPath);
                dataGridVRe.Rows[pSelectedRowC[0].Index].Cells["CumFilePath"].Value = strPath;
                dataGridVRe.Rows[pSelectedRowC[0].Index].Cells["CumFileName"].Value = m_ModifyName;
                MessageBox.Show("修改成功！", "提示！");
            }
            else
            {
                MessageBox.Show("成果数据：" + dataGridVRe.Rows[pSelectedRowC[0].Index].Cells["CumFileName"].Value.ToString() + "修改失败！", "提示！");
            }

        }

        private void bttDelete_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection pSelectedRowC = dataGridVRe.SelectedRows;
            if (pSelectedRowC.Count == 0) { MessageBox.Show("请选中相关的记录再进行删除操作！","提示！"); return; }
            try
            {
                if (MessageBox.Show("确定要删除选中的记录吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                /////删除用户选中的项
                for (int i = 0; i < pSelectedRowC.Count; i++)
                { 
                    string strPath = dataGridVRe.Rows[pSelectedRowC[i].Index].Cells["CumFilePath"].Value.ToString();
                    //if (!File.Exists(Path.GetDirectoryName(strPath))) { MessageBox.Show("成果数据目录已被损坏！","提示！"); return; }
                    string strName = dataGridVRe.Rows[pSelectedRowC[i].Index].Cells["CumFileName"].Value.ToString();
                    if (DeleteTableRow("RESULTLIST", "DATANAME='" + strName + "'"))
                    {
                        DeleteFile(strPath);
                        dataGridVRe.Rows.RemoveAt(pSelectedRowC[i].Index);
                    }
                    else { MessageBox.Show("成果数据："+strName+"删除失败！","提示！"); }
                }
            }
            catch { }
        }
     //根据成果类型获取共享目录  ygc 2012-9-6
        private string GetSharedPath(string DataType)
        {
            if (m_Workspace == null) return null;
            SysCommon.Gis.SysGisTable sysTable = new SysCommon.Gis.SysGisTable(m_Workspace);
            Exception ex=null ;
            Dictionary<string, object> newdic = new Dictionary<string, object>();
            try
            {
              newdic= sysTable.GetRow("RESULTDIR", "PATHNAME='" + DataType + "'", out ex);
            }
            catch
            {
 
            }
            if (newdic ==null) return null;
            if (newdic.Count == 0) return null;
            return newdic["DATADIR"].ToString ();
        }

        public static bool Connect(string remoteHost, string userName, string passWord)
        {
            if (!Ping(remoteHost))
            {
                return false;
            }
            bool Flag = true;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe"; //设定程序名
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true; //重定向标准输入
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true; //重定向错误输出
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = "";
                if (userName != "")
                {
                    dosLine = @"net use \\" + remoteHost + " " + passWord + " " + " /user:" + userName + ">NUL";
                }
                else
                {
                    dosLine = @"net use \\" + remoteHost + " >NUL";
                }
                proc.StandardInput.WriteLine(dosLine);  //执行的命令

                proc.StandardInput.WriteLine("exit");
                while (proc.HasExited == false)
                {
                    proc.WaitForExit(100);
                }
                string errormsg = proc.StandardError.ReadToEnd();
                if (errormsg != "")
                {
                    Flag = false;
                }
                proc.StandardError.Close();
            }
            catch (Exception ex)
            {
                Flag = false;
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
        public static bool Ping(string remoteHost)
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
    }
}
