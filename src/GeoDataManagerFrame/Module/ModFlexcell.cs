//*********************************************************************************
//** 文件名：ModFlexcell.cs
//** CopyRight (c) 2000-2007 武汉吉奥信息工程技术有限公司工程部
//** 创建人：chulili
//** 日  期：2011-03
//** 修改人：
//** 日  期：
//** 描  述：用于报表输出
//**
//** 版  本：1.0
//*********************************************************************************
using System;
using System.Collections.Generic;
using System.Text;

using System.Data.OleDb;
namespace GeoDataManagerFrame
{
    class ModFlexcell
    {
        public static int m_startCol;       //起始列
        public static int m_startRow;       //起始行
        public static int m_SpecialRow;     //特殊行（需要单独修改某一行的显示时候用到，通常情况下都是整表输出）
        public static int m_SpecialRow_ex;  //特殊行（需要单独修改某一行的显示时候用到，通常情况下都是整表输出）
        public static int m_SpecialRow_ex2; //特殊行（需要单独修改某一行的显示时候用到，通常情况下都是整表输出）
        //added by chulili
        //函数功能：将数据写到表格中（数据存在于数据库的一张表中）
        //输入参数：数据库连接串，报表对话框标题，数据源表名，指定输出的字段，排序字段（仅限于升序排列），报表模板全路径，起始行，起始列，字体，字号
        public static FormFlexcell  SendDataToFlexcell(string connstr,string caption,string TableName,string Fieldstr,string OrderByField,string TemplateFile,int startrow,int startcol,string FontName,int FontSize)
        {
            //根据连接串连接数据库
            OleDbConnection conn = new OleDbConnection(connstr );
            if (conn == null)
                return null;
            conn.Open();
            //判断数据源表是否存在
            if (ModTableFun.isExist(conn, TableName) == false)
                return null;
            //设置起始行，起始列
            m_startCol = startcol;
            m_startRow = startrow;
            //初始化报表对话框
            FormFlexcell frm = new FormFlexcell();
            //获取数据源记录数（OleDbDataReader对象的一个缺点是没有recordcount属性或方法，需要单独取记录数）
            int recordcount=0;
            OleDbDataReader myreader;
            OleDbCommand mycomm = conn.CreateCommand();
            mycomm.CommandText = "select count(*) from " + TableName;
            myreader = mycomm.ExecuteReader();
            if (myreader.Read())
                recordcount = (int)myreader.GetValue(0);
            myreader.Close();
            //判断是否输出全部字段
            if (Fieldstr.Equals(""))
                Fieldstr="*";
            //判断排序字段是否存在
            if (OrderByField.Equals("")==false)
                if (ModTableFun.isFieldExist(conn,TableName,OrderByField)==false)
                    OrderByField="";
            string sqlstr;
            //构造获取数据的sql语句
            if (OrderByField.Equals (""))
            {
                sqlstr="select "+Fieldstr +" from " + TableName ;
            }
            else
            {
                sqlstr="select "+Fieldstr +" from " + TableName +" order by "+OrderByField ;
            }
            mycomm.CommandText=sqlstr;
            myreader=mycomm.ExecuteReader();
            //向报表对话框输入数据
            frm.SetTextFromRS(startrow,startcol,myreader,TemplateFile,recordcount,FontName,FontSize);
            if (caption.Equals("") )
                caption=TableName;
            frm.Text = caption;
            myreader.Close();

            conn.Close();
            return frm;
        }
        //函数重载
        public static FormFlexcell SendDataToFlexcell(string connstr, string caption, string TableName, string Fieldstr, string OrderByField, string TemplateFile, int startrow, int startcol)
        {
            return SendDataToFlexcell(connstr,caption,TableName,Fieldstr,OrderByField, TemplateFile,startrow,startcol,"宋体",9);
        }

    }
}
