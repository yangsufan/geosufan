using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using SysCommon;
using SysCommon.Gis;
using System.IO;
using ESRI.ArcGIS.esriSystem;

//ygc 2012-12-17 制图方案 将配置好的地图当成模板，存入数据库中

namespace GeoPageLayout
{
    public partial class FrmManagerPageLayout : DevComponents.DotNetBar.Office2007Form
    {
        public FrmManagerPageLayout()
        {
            InitializeComponent();
        }
        public IWorkspace m_pWorkspace
        {
            get;
            set;
        }
        public string m_MXDFileName
        {
            get;
            set;
        }

        //退出本窗体
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog openMXD = new OpenFileDialog();
            openMXD.Title = "打开地图文档";
            openMXD.Filter = "*.mxd|*.mxd";
            openMXD .Multiselect=true ;
            if (openMXD.ShowDialog() != DialogResult.OK) return;
            string[] fileName  = openMXD.FileNames;
            Dictionary<string, string> dicFiles = GetFileDic(fileName);
            bool flag = AddFileToDataBase(m_pWorkspace, dicFiles, "PAGELAYOUTSOLUTION");
            if (flag)
            {
                MessageBox.Show("成功导入制图方案!", "提示", MessageBoxButtons.OK);
                IntialDataGridView(m_pWorkspace, "PAGELAYOUTSOLUTION");
            }
            else
            {
                MessageBox.Show("导入制图方案出错！","提示",MessageBoxButtons .OK ,MessageBoxIcon.Error);
            }
            
        }
        private DataTable GetPageSolutionTale(IFeatureWorkspace pWorkspace,string tableName,string Contion)
        {
            if (pWorkspace == null || tableName == "") return null;
            DataTable newtable = null;
            try
            {
                ITable pTable = pWorkspace.OpenTable(tableName);
                newtable =ITableToDataTable(pTable, "制图方案",Contion);
            }
            catch (Exception ex)
            {
                return null;
            }
            return newtable;
        }
        //获取制图方案表
        private DataTable ITableToDataTable(ITable pTable, string sTableName,string condtion)
        {
            DataTable pDataTable = new DataTable(sTableName);
            try
            {
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = condtion;
                ICursor pCursor = pTable.Search(pQueryFilter, false);
                IRow pRow = pCursor.NextRow();
                if (pRow != null)
                {
                    for (int i = 0; i < pRow.Fields.FieldCount; i++)
                    {
                        pDataTable.Columns.Add(pRow.Fields.get_Field(i).Name);
                    }
                    while (pRow != null)
                    {
                        DataRow pDataRow = pDataTable.NewRow();
                        for (int j = 0; j < pCursor.Fields.FieldCount; j++)
                            pDataRow[j] = pRow.get_Value(j);
                        pDataTable.Rows.Add(pDataRow);
                        pRow = pCursor.NextRow();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return pDataTable;
        }
        private void FrmManagerPageLayout_Load(object sender, EventArgs e)
        {
            m_pWorkspace = Plugin.ModuleCommon.TmpWorkSpace;
            string tableName = "PAGELAYOUTSOLUTION";
            IntialDataGridView(m_pWorkspace, tableName);
        }
        private bool AddFileToDataBase(IWorkspace pWorkspace, Dictionary<string, string> dicFile, string TableName)
        {
            bool flag = false;
            try
            {
                SysGisTable sysTable = new SysGisTable(Plugin.ModuleCommon.TmpWorkSpace);
                ITable pTable = (pWorkspace as IFeatureWorkspace).OpenTable(TableName);
                if (pTable == null) return flag;
                Exception ex=null ;
                foreach (string key in dicFile.Keys)
                {
                    System.IO.FileStream vFileStream = new System.IO.FileStream(key, FileMode.Open, FileAccess.Read);
                    Byte[] byteData = new Byte[vFileStream.Length];
                    vFileStream.Read(byteData, 0, byteData.Length);
                    vFileStream.Close();
                    Dictionary<string, object> newdic = new Dictionary<string, object>();
                    newdic.Add("SOLUTIONNAME",dicFile [key ]);
                    newdic.Add("DOCMXD",byteData);
                    flag= NewRow(pTable, newdic, out ex);
                }
            }
            catch (Exception ex)
            {
                return flag;
            }
            return flag;
        }
        //初始化表格控件
        private void IntialDataGridView(IWorkspace pWorkspace, string tableName)
        {
            if (m_pWorkspace == null)
            {
                this.Close();
                return;
            }
            DataTable pageTable = GetPageSolutionTale(m_pWorkspace as IFeatureWorkspace, tableName, null);
            if (pageTable == null)
            {
                MessageBox.Show("无制图方案！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            dgvShowPagelayoutSolution.DataSource = pageTable;
        }
        private Dictionary<string, string> GetFileDic(string[] fileNames)
        {
            Dictionary<string, string> newdic = new Dictionary<string, string>();
            for (int i = 0; i < fileNames.Length; i++)
            {
                string tempStr = fileNames[i];
                //获取文件名
                tempStr = tempStr.Substring(tempStr .LastIndexOf ("\\")+1,tempStr .Length -tempStr .LastIndexOf ("\\")-5);
                if (newdic.ContainsKey(fileNames[i]))
                { }
                else
                {
                    newdic.Add(fileNames[i], tempStr);
                }
            }
            return newdic;
        }
        //将地图文档存入数据库中
        private  bool NewRow(ITable pTable, Dictionary<string, object> dicvalues, out Exception eError)
        {
            eError = null;

            ICursor pCursor = pTable.Insert(false);
            IRowBuffer pRowBuffer = pTable.CreateRowBuffer();
            foreach (KeyValuePair<string, object> keyValue in dicvalues)
            {
                int index = pRowBuffer.Fields.FindField(keyValue.Key);
                if (index == -1)
                {
                    eError = new Exception("字段" + keyValue.Key + "不存在");
                    return false;
                }

                try
                {
                    if (pRowBuffer.Fields.get_Field(index).Editable)
                    {
                        if (keyValue.Value == null)
                        {
                            pRowBuffer.set_Value(index, System.DBNull.Value);
                        }
                        else
                        {
                            if (keyValue.Value.GetType().FullName == "System.String")
                            {
                                pRowBuffer.set_Value(index, keyValue.Value);
                            }
                            else
                            {
                                pRowBuffer.set_Value(index, ConvertByte2Object((byte[])keyValue.Value));
                            }
                        }
                    }
                }
                catch (Exception eX)
                {
                    eError = new Exception("字段" + keyValue.Key + "类型与值不匹配");
                    //********************************
                    //guozheng added  system exception log
                    if (SysCommon.Log.Module.SysLog == null)
                        SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    SysCommon.Log.Module.SysLog.Write(eX);
                    SysCommon.Log.Module.SysLog.Write(eError);
                    //********************************
                    return false;
                }
            }

            try
            {
                pCursor.InsertRow(pRowBuffer);
            }
            catch (Exception eR)
            {
                //********************************
                //guozheng added  system exception log
                if (SysCommon.Log.Module.SysLog == null)
                    SysCommon.Log.Module.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                SysCommon.Log.Module.SysLog.Write(eR);
                //********************************
                eError = eR;
                return false;
            }

            return true;
        }
        private object ConvertByte2Object(byte[] objValue)
        {
            if (objValue == null || objValue.Length == 0) return null;
            IMemoryBlobStream pMemory = new MemoryBlobStreamClass();
            pMemory.ImportFromMemory(ref objValue[0], (uint)objValue.GetLength(0));
            return pMemory;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvShowPagelayoutSolution.CurrentRow == null)
            {
                MessageBox.Show("请选择要删除的方案！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("确定删除改制图方案？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.OK) return;
           string oid = dgvShowPagelayoutSolution.CurrentRow.Cells["OBJECTID"].Value .ToString();
           Exception ex = null;
           try
           {
               ITable pTable = (m_pWorkspace as IFeatureWorkspace).OpenTable("PAGELAYOUTSOLUTION");
               string condition = "objectid='" + oid + "'";
              bool flag= ModGisPub.DelRow(pTable ,condition ,out ex);
              if (flag)
              {
                  MessageBox.Show("成功删除改方案!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                  IntialDataGridView(m_pWorkspace, "PAGELAYOUTSOLUTION");
              }
              else
              {
                  MessageBox.Show("删除制图方案失败！","提示",MessageBoxButtons.OK ,MessageBoxIcon.Error);
              }
           }
           catch
           {
 
           }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvShowPagelayoutSolution.CurrentRow == null) return;
            string Pname = dgvShowPagelayoutSolution.CurrentRow.Cells["PageName"].Value.ToString();
            string sRemark = dgvShowPagelayoutSolution.CurrentRow.Cells["remark"].Value.ToString();
            string oid=dgvShowPagelayoutSolution .CurrentRow .Cells ["OBJECTID"].Value .ToString ();
            FrmUpdateSolution newfrm = new FrmUpdateSolution();
            newfrm.m_Name = Pname;
            newfrm.m_Remark = sRemark;
            if (newfrm.ShowDialog() != DialogResult.OK) return;
            Pname = newfrm.m_Name;
            sRemark = newfrm.m_Remark;
            Exception ex=null ;
            try
            {
                ITable pTable=(m_pWorkspace as IFeatureWorkspace ) .OpenTable ("PAGELAYOUTSOLUTION");
                string con="objectid='"+oid+"'";
                Dictionary <string ,object > newdic=new Dictionary<string,object> ();
                newdic .Add ("SOLUTIONNAME",Pname);
                newdic .Add ("REMARK",sRemark);
               bool flag= ModGisPub.UpdateRow(pTable, con, newdic, out ex);
               if (flag)
               {
                   MessageBox.Show("成功更新制图方案！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   IntialDataGridView(m_pWorkspace, "PAGELAYOUTSOLUTION");
               }
               else
               {
                   MessageBox.Show("更新制图方案失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
            }
            catch
            {
                MessageBox.Show("更新制图方案失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            string FilePath = Application.StartupPath + "\\..\\Temp";
            if (dgvShowPagelayoutSolution.CurrentRow == null) return;
            string pName = dgvShowPagelayoutSolution.CurrentRow.Cells["PageName"].Value.ToString();
            string oid = dgvShowPagelayoutSolution.CurrentRow.Cells["OBJECTID"].Value.ToString();
            string strQuery = "OBJECTID='" + oid + "'";
            string FileName = FilePath +"\\"+ pName + ".mxd";
            bool flag=SaveFile (FileName ,ReadByteFromBlob("PAGELAYOUTSOLUTION",strQuery ,"DOCMXD"));
            if (flag)
            {
                m_MXDFileName = FileName;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("读取方案出错！","提示",MessageBoxButtons .OK ,MessageBoxIcon.Information);
                return;
            }



        }
        //从数据库中读取MXD文档
        private byte[] ReadByteFromBlob(string TableName, string QuerStr, string fieldName)
        {
            ICursor pCursor = null;
            byte[] newbyte = null;
            try
            {
                ITable pTable = (m_pWorkspace as IFeatureWorkspace).OpenTable(TableName);
                IQueryFilter pQueryFilter = new QueryFilterClass();
                pQueryFilter.WhereClause = QuerStr;
                pQueryFilter.SubFields = fieldName;
                pCursor = pTable.Search(pQueryFilter, false);
                if (pCursor == null) return null;
                int FieldIndex = pCursor.Fields.FindField(fieldName);
                if (FieldIndex < 0) return null;
                IRow pRow = pCursor.NextRow();
                if (pRow == null) return null;
                object resultValue = pRow.get_Value(FieldIndex);
                if (resultValue == null) return null;
                if (resultValue is System.DBNull) return null;
                IMemoryBlobStreamVariant pMemoryBlob = resultValue as IMemoryBlobStreamVariant;
                  object outValue;
                  pMemoryBlob.ExportToVariant(out outValue);
                newbyte = (byte[])outValue;
            }
            catch
            {
                return null;
            }
            return newbyte;
        }
        private bool SaveFile(string FilePat,byte[] byteValue)
        {
            try
            {
                if(File.Exists (FilePat))
                {
                    File.Delete(FilePat);
                }
                System.IO.FileStream vFileStream = File.Create(FilePat);
                vFileStream.Write(byteValue, 0, byteValue.Length);
                vFileStream.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
