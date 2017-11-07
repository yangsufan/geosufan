//*********************************************************************************
//** 文件名：TableForm.cs
//** CopyRight (c) 2000-2007 武汉吉奥信息工程技术有限公司工程部
//** 创建人：chulili
//** 日  期：2011-02
//** 修改人：
//** 日  期：
//** 描  述：用于添加修改记录
//**
//** 版  本：1.0
//*********************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

using SysCommon.Gis;
using SysCommon.Error;
using SysCommon.Authorize;
using ESRI.ArcGIS.Geodatabase;
namespace GeoDBConfigFrame
{
    public partial class TableForm : DevComponents.DotNetBar.Office2007Form
    {

        private int m_FieldCount = 0;
        private string m_connstr;
        private string m_TableName;
        private IRow pRow=null;//新产生的row；
        
        DevComponents.DotNetBar.LabelX[] m_LabelX = new DevComponents.DotNetBar.LabelX[9];
        Label[] m_LabelY = new Label[9];
        DevComponents.DotNetBar.Controls.TextBoxX[] m_TextBoxX = new DevComponents.DotNetBar.Controls.TextBoxX[9];
        DataGridView m_gridView;

        string m_EditMode;
        //初始化表格 输入参数：DataGridView控件，数据库连接串，表名
        public TableForm(DataGridView gridview,string connstr,string Tablename)
        {
            InitializeComponent();
            if (gridview != null)
                m_gridView = gridview;
            m_connstr = connstr;
            m_TableName = Tablename;
        }
        //初始化label内容  changed by xisheng 添加自动调整label的宽度，然后调整
        private void initLableButton(int labcount)
        {
            m_FieldCount = labcount;

            int Top = 17;
            for (int i = 0; i < labcount; i++)
            {
                m_LabelY[i] = new Label();
                m_LabelY[i].Top = Top;
                m_LabelY[i].Left = 23;
                m_LabelY[i].AutoSize = true;
                this.Controls.Add(m_LabelY[i]);
                Top += 30;
            }

            Top = 17;
            for (int i = 0; i < labcount; i++)
            {
                m_TextBoxX[i] = new DevComponents.DotNetBar.Controls.TextBoxX();
                m_TextBoxX[i].Top = Top;
                m_TextBoxX[i].Left = 75;
                m_TextBoxX[i].Width = 142;
                m_TextBoxX[i].Height = 20;
                this.Controls.Add(m_TextBoxX[i]);
                Top += 30;
            }
            this.buttonXOK.Top = m_LabelY[labcount - 1].Top + 40;
            this.buttonXCancel.Top = m_LabelY[labcount - 1].Top + 40;
            this.Height = this.buttonXOK.Top + 60;

        }
        //初始化对话框
        //输入参数：编辑模式（添加记录、修改记录两种）  输出参数：无
        public void InitForm(string EditMode)
        {
            m_EditMode=EditMode;
            int i = 0;
            int maxwidth=0;
            initLableButton(m_gridView.ColumnCount);
            if (EditMode == "MODIFY")
                this.Text = "修改记录";
            else
            {
                this.Text = "添加记录";
                Add();
            }
            for (i = 0; i < m_gridView.ColumnCount ; i++)  //从1开始
            {
                //添加记录只需根据字段名称初始化标签内容
                m_LabelY[i].Text = m_gridView.Columns[i].Name+":";
                if (m_LabelY[i].Width > maxwidth) maxwidth=m_LabelY[i].Width;
                if (m_gridView.Columns[i].Name.ToUpper() == "ID")
                    m_TextBoxX[i].Enabled = false;
                //修改记录需要根据字段值初始化编辑框内容
                if (EditMode == "MODIFY")
                    m_TextBoxX[i].Text = m_gridView.SelectedRows[0].Cells[i].Value.ToString();
            }
            for (i = 0; i < m_gridView.ColumnCount; i++)
            {
                m_TextBoxX[i].Left = m_LabelY[i].Left + maxwidth;
            }
        }


        private bool Add_Ex()
        {
            //SysGisTable sysTable = new SysGisTable(ModData.gisDb);
            ////判断是更新还是添加
            //try
            //{
            //    if (!sysTable.ExistData("user_info", "NAME='" + txtUser.Text.Trim().ToLower() + "'"))
            //    {
            //        if (!sysTable.NewRow("user_info", dicData, out exError))
            //        {
            //            ErrorHandle.ShowFrmErrorHandle("提示", "添加失败！" + exError.Message);
            //            return;
            //        }
            //    } 
            //    this.DialogResult=DialogResult.OK;
            //}
            //catch (Exception ex)
            //{
            //    exError = ex;
            //    ModData.gisDb.EndTransaction(false, out exError);
            //    ErrorHandle.ShowFrmErrorHandle("提示", exError.Message);
            //}
            return false;
        }


        //函数功能：根据用户填写向表中添加记录
        //输入参数：无  输出参数：无
        private bool Addpre()
        {
            int i = 0;
            DataTable t = (DataTable)(m_gridView.DataSource);

            OleDbConnection mycon = new OleDbConnection(m_connstr);   //定义OleDbConnection对象实例并连接数据库
            string strExp = "";
            //先查询表中是否存在同样的记录
            strExp = "select * from " + m_TableName + " where";
            for (i = 0; i < m_gridView.ColumnCount; i++)
            {
                if (m_TextBoxX[i].Text.Equals(""))
                    strExp = strExp + " (" + m_LabelY[i].Text + "='' or " + m_LabelY[i].Text + " is null) and";
                else 
                    strExp = strExp + " " + m_LabelY[i].Text + "='" + m_TextBoxX[i].Text + "' and";
            }
            strExp = strExp.Substring(0, strExp.Length - 3);
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            mycon.Open();
            OleDbDataReader aReader = aCommand.ExecuteReader();
            //存在同样的记录则退出函数
            if (aReader.Read())
            {
                aReader.Close();
                mycon.Close();
                return false;
            }
            aReader.Close();
            mycon.Close();

            DataRow tmprow;
            tmprow = t.NewRow();
            //给添加的新记录赋属性
            for (i = 0; i < m_gridView.ColumnCount; i++)  
            {
                if (m_LabelY[i].Text.ToUpper().Equals("ID"))
                    continue;
                tmprow[m_LabelY[i].Text ] = m_TextBoxX[i].Text;
            }
            //添加记录
            t.Rows.Add(tmprow);
            m_gridView.DataSource = t;
            return true;
        }
           //函数功能：修改记录
        //输入参数：无  输出参数：布尔型
        //函数功能：根据用户填写向表中添加记录
        //输入参数：无  输出参数：bool
        private bool Add()
        {
            try
            {
                if (GeoDataCenterFunLib.LogTable.m_sysTable == null)
                    return false;
                Dictionary<string, object> pDic = new Dictionary<string, object>();
                Exception ex=null;
                ITable pTable = GeoDataCenterFunLib.LogTable.m_sysTable.OpenTable(m_TableName, out ex);
                if (pTable == null)
                    return false;
                pRow = pTable.CreateRow();
                for (int i = 0; i < m_gridView.ColumnCount; i++)  //显示新产生的记录ID给用户
                {
                    if (m_gridView.Columns[i].Name.ToUpper() == "ID")
                    {
                        m_TextBoxX[i].Text = pRow.OID.ToString();
                        ModDBOperate.oid = pRow.OID;
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "修改记录失败！" + ex.Message);
                return false;
            }
            return true;
        }
        //函数功能：修改记录for add
        //输入参数：无  输出参数：布尔型
        private bool ModifyAfterAdd()
        {
            if (GeoDataCenterFunLib.LogTable.m_sysTable == null)
                return false;
             Exception ex=null;
            ITable pTable = GeoDataCenterFunLib.LogTable.m_sysTable.OpenTable(m_TableName, out  ex);
            if (pTable == null)
                return false;
            string whereClause = pTable.OIDFieldName;
            Dictionary<string, object> pDic = new Dictionary<string, object>();
            for (int i = 0; i < m_gridView.ColumnCount; i++)  //从1开始，第一列是ID
            {
                if (m_gridView.Columns[i].Name.ToUpper() == "ID")
                {
                    whereClause += " = " + m_TextBoxX[i].Text;
                    continue;
                }
                //if (m_LabelX[i].Text.Contains(":"))
                //{
                //    m_LabelX[i].Text = m_LabelX[i].Text.Substring(0, m_LabelX[i].Text.LastIndexOf(":"));
                //}
                pDic.Add(m_gridView.Columns[i].Name, m_TextBoxX[i].Text);
            }
            Exception ex1 = null;
            try
            {
                return GeoDataCenterFunLib.LogTable.m_sysTable.UpdateRowByAliasName(m_TableName,whereClause,pDic, out ex1);//更新记录
            }
            catch (Exception err)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "添加记录失败！" + err.Message);
                return false;
            }


        }
        //函数功能：修改记录
        //输入参数：无  输出参数：布尔型
        private bool Modify()
        {
            if (GeoDataCenterFunLib.LogTable.m_sysTable == null)
                return false;
            Exception err;
            ITable pTable = GeoDataCenterFunLib.LogTable.m_sysTable.OpenTable(m_TableName, out err);
            if (pTable == null)
                return false;
            string whereClause = pTable.OIDFieldName;
            Dictionary<string, object> pDic = new Dictionary<string, object>();
            for (int i = 0; i < m_gridView.ColumnCount; i++)  //从1开始，第一列是ID
            {
                if (m_gridView.Columns[i].Name.ToUpper() == "ID")
                {
                    whereClause += " = " + m_TextBoxX[i].Text;
                    continue;
                }
                pDic.Add(m_gridView.Columns[i].Name, m_TextBoxX[i].Text);
            }
            Exception ex = null;
            try
            {
                return GeoDataCenterFunLib.LogTable.m_sysTable.UpdateRowByAliasName(m_TableName, whereClause, pDic, out ex);
            }
            catch (Exception err1)
            {
                ErrorHandle.ShowFrmErrorHandle("提示", "修改记录失败！" + err1.Message);
                return false;
            }


        }
        //函数功能：修改记录
        //输入参数：无  输出参数：布尔型
        private bool Modifypre()
        {
            
            //int i = 0;  writed by others commented by yjl begin 2011-6-11
            ////for (i = 0; i < m_gridView.ColumnCount ; i++)  //从1开始，第一列是ID
            ////{
            ////    m_gridView.SelectedRows[0].Cells[i].Value =m_TextBoxX[i].Text ;
            ////}
            ////OleDbConnection mycon = new OleDbConnection(m_connstr);   //定义OleDbConnection对象实例并连接数据库
            //string strExp = "";
            ////查找表中有没有与修改后相同的记录
            //strExp = "select * from " + m_TableName +" where";
            //for (i = 0; i < m_gridView.ColumnCount; i++) 
            //{
            //    if (m_LabelX[i].Text.ToUpper().Equals("ID"))
            //        continue;
            //    if (m_TextBoxX[i].Text.Equals(""))
            //        strExp = strExp + " (" + m_LabelX[i].Text + "='' or " + m_LabelX[i].Text + " is null) and";
            //    else 
            //        strExp =strExp+ " " + m_LabelX[i].Text + "='" + m_TextBoxX[i].Text+ "' and";
            //}
            //strExp = strExp.Substring(0,strExp.Length - 3);
            //OleDbCommand aCommand = new OleDbCommand(strExp, mycon);     
            //mycon.Open();
            //OleDbDataReader aReader = aCommand.ExecuteReader();
            ////如果表中存在与修改后相同的记录，则退出函数
            //if(aReader.Read())
            //{
            //    aReader.Close ();
            //    mycon.Close();
            //    return false;
            //}
            //aReader.Close();
            ////构造更新sql语句（由于业务表中没有id列，则不能使用dataview自带的功能进行更新）
            //strExp="update " + m_TableName +" set ";
            //for (i = 0; i < m_gridView.ColumnCount; i++) 
            //{
            //    if (m_LabelX[i].Text.ToUpper().Equals("ID"))
            //        continue;
            //    strExp =strExp+ " " + m_LabelX[i].Text + "='" + m_TextBoxX[i].Text+ "',";
            //}
            //strExp = strExp.Substring(0,strExp.Length - 1);
            //strExp = strExp + " where ";
            //for (i = 0; i < m_gridView.ColumnCount; i++)
            //{
            //    if (m_gridView.Columns[i].Name.ToUpper().Equals("ID"))
            //        continue;
            //    if (m_gridView.SelectedRows[0].Cells[i].Value.ToString().Equals(""))
            //        strExp = strExp + " (" + m_gridView.Columns[i].Name + "='' or " + m_gridView.Columns[i].Name + " is null) and";
            //    else 
            //        strExp = strExp + " " + m_gridView.Columns[i].Name + "='" + m_gridView.SelectedRows[0].Cells[i].Value.ToString() + "' and";
            //}
            //strExp = strExp.Substring(0,strExp.Length - 3);
            ////执行更新语句
            //aCommand.CommandText=strExp;
            //aCommand.ExecuteNonQuery ();
            //mycon.Close();  
            //writed by others commented by yjl end 
            return true;
        }
        //单击确定按钮时根据编辑模式选择执行的功能函数
        private void buttonXOK_Click(object sender, EventArgs e)
        {
            if (m_EditMode == "ADD")
                if (ModifyAfterAdd())
                {
                    MessageBox.Show("添加成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    return;
            else
                if (Modify())
                    MessageBox.Show("修改成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    return;
            this.DialogResult = DialogResult.OK;
            
        }
        //单击取消按钮时候的响应
        private void buttonXCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

        }
    }
}