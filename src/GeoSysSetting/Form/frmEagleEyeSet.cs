using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using SysCommon.Gis;
using System.IO;
using SysCommon.Error;
using ESRI.ArcGIS.Controls;
//zq  20110927    add
namespace GeoSysSetting
{
    public partial class frmEagleEyeSet : DevComponents.DotNetBar.Office2007Form
    {
        public AxMapControl m_AxMapControl;
        public IWorkspace m_Workspace;
        public frmEagleEyeSet(AxMapControl pAxMapControl, IWorkspace pWorkspace)
        {
            InitializeComponent();
            m_AxMapControl = pAxMapControl;
            m_Workspace = pWorkspace;
            txtPath.Text = Application.StartupPath + "\\..\\Template\\MapIndex.mxd";
        }

        private void bttOpen_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog sOpenFileD = new System.Windows.Forms.OpenFileDialog();
            sOpenFileD.Multiselect = false ;
            sOpenFileD.Filter = "mxd文件(*.mxd)|*.mxd";
            sOpenFileD.InitialDirectory = Application.StartupPath + "\\..\\Template";
            if (sOpenFileD.ShowDialog() == DialogResult.OK)
            {
                this.txtPath.Text = sOpenFileD.FileName;
            }
        }

        private void bttImport_Click(object sender, EventArgs e)
        {
            if (txtPath.Text == "") { MessageBox.Show(" 请导入鹰眼图文件位置！","提示！"); return; }
            string strPath =txtPath.Text;
            if (!System.IO.File.Exists(strPath)) { MessageBox.Show("未找到指定路径下的文件！","提示！"); return; }
            if (!m_AxMapControl.CheckMxFile(strPath)) { MessageBox.Show("该mxd文件不是合法文件！","提示！"); return; }
            bool pIsImport = ImportEagleEyset(m_Workspace, strPath);
            if (pIsImport) 
            {
                MessageBox.Show("鹰眼图导入成功！", "提示！"); 
                this.Dispose();
                this.Close(); 
            }
            if (!pIsImport) { MessageBox.Show("鹰眼图导入失败！", "提示！"); return; }
           
        }
        //本地向数据库保存图层目录
         private bool ImportEagleEyset(IWorkspace pWorkspace, string strPath)
        {
            //判断各个参数是否有效
            if (pWorkspace == null)
            {
                return false;
            }
            Exception exError = null;
            ITransactions pTransactions = null;
            //保存图层树（由本地向数据库保存）
            try
            {
                IMemoryBlobStream pBlobStream = new MemoryBlobStreamClass();
                ///读取路径下的mxd文件
               // System.IO.FileStream pFileStream = File.Create(strPath);
               //if (pFileStream == null) { return false; }
               // byte[] bytes = new byte[pFileStream.Length];
               // pBlobStream.ImportFromMemory(ref bytes[0], (uint)bytes.GetLength(0));
               // pFileStream.Close();
               // pFileStream.Dispose();
                ///读取路径下的mxd文件转化成MemoryBlobStreamClass
                pBlobStream.LoadFromFile(strPath);
                //启动事务
                pTransactions = (ITransactions)pWorkspace;
                if (!pTransactions.InTransaction) pTransactions.StartTransaction();
                SysGisTable sysTable = new SysGisTable(pWorkspace);
                Dictionary<string, object> dicData = new Dictionary<string, object>();
                dicData.Add("SETTINGVALUE2", pBlobStream);
                dicData.Add("SETTINGNAME", "鹰眼图");
                //判断是更新还是添加
                //不存在则添加，已存在则更新
                if (!sysTable.ExistData("SYSSETTING", "SETTINGNAME='鹰眼图'"))
                {
                    if (!sysTable.NewRow("SYSSETTING", dicData, out exError))
                    {
                        ErrorHandle.ShowFrmErrorHandle("提示", "添加失败！");
                        return false;
                    }
                }
                else
                {
                    if (!sysTable.UpdateRow("SYSSETTING", "SETTINGNAME='鹰眼图'", dicData, out exError))
                    {
                        ErrorHandle.ShowFrmErrorHandle("提示", "更新失败！");
                        return false;
                    }
                }
                //提交事务
                if (pTransactions.InTransaction) pTransactions.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                //出错则放弃提交
                if (pTransactions.InTransaction) pTransactions.AbortTransaction();
                ErrorHandle.ShowFrmErrorHandle("提示", "更新失败！");
                return false;
            }
        }
    }
}
