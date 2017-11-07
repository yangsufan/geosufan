using System;
using System.Collections.Generic;
using System.Text;
using GeoDataCenterFunLib;
using System.Data.OleDb;
using System.Xml;
using System.IO;
using System.Windows.Forms;
//编辑命名规则
namespace GeoDBConfigFrame
{
    public class InitConfig : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
        public InitConfig()
        {
            base._Name = "GeoDBConfigFrame.InitConfig";
            base._Caption = "初始化配置";
            base._Tooltip = "初始化配置";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "初始化配置";
        }
        public override void OnClick()
        {
            if (MessageBox.Show("确定要初始化配置吗？本操作将清空部分业务表。", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string xmlpath = dIndex.m_strInitXmlPath;
            XmlDocument xmldoc = new XmlDocument();
            XmlNode xmlNode=null;
            if (xmldoc != null)
            {
                if (File.Exists(xmlpath))
                {
                    xmldoc.Load(xmlpath);
                    string strSearch = "//InitConfig";
                    xmlNode = xmldoc.SelectSingleNode(strSearch);
                    if (xmlNode == null) return;

                }
                else
                    return;
            }
            else
                return;

            
            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);
                if (log != null)
                {
                    log.Writelog("初始化配置");

                }
            }
            if (m_Hook.GridCtrl == null)
                return;
            //获取数据库连接串和表名
            string dbpath = dIndex.GetDbInfo();
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dbpath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串

            OleDbConnection mycon = new OleDbConnection(connstr);   //定义OleDbConnection对象实例并连接数据库
            mycon.Open();
            OleDbCommand aCommand =mycon.CreateCommand();
            XmlNodeList xmlNdList = xmlNode.ChildNodes;
            foreach (XmlNode xmlChild in xmlNdList)
            {
                XmlElement xmlElent = (XmlElement)xmlChild;
                string strTblName = xmlElent.GetAttribute("ItemName");
                if (isExist(mycon, strTblName) == true)
                {
                    //执行删除语句
                    aCommand.CommandText = "delete from " + strTblName;
                    aCommand.ExecuteNonQuery();
                }
            }                       
            mycon.Close();
            MessageBox.Show("初始化配置完毕。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information );
        }
        //函数功能：判断表是否存在  
        //输入参数：数据库连接 表名  输出参数：布尔型
        public static bool isExist(OleDbConnection conn, string TableName)
        {
            OleDbCommand comm = conn.CreateCommand();
            comm.CommandText = "select * from " + TableName + " where 1=0";
            OleDbDataReader myreader;
            //根据错误保护判断表是否存在
            try
            {
                myreader = comm.ExecuteReader();
                myreader.Close();
                return true;
            }
            //报错则表示不存在
            catch (System.Exception e)
            {
                e.Data.Clear();
                return false;
            }

        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
        }
    }
}
