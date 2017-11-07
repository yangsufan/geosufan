using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Data;

namespace FileDBTool
{
    public class clsProject
    {
        private string _ProName;////////工程名
        private string _ftpIP;//////////连接ip
        private string _ftpUSER;///////连接用户id
        private string _ftpPassWord;////连接用户密码
        private FTP_Class ftp;/////////
        #region 构造函数
        /// <summary>
        /// 构造函数，初始化工程名，ip,用户id，密码
        /// </summary>
        /// <param name="ProName">工程名</param>
        /// <param name="ftpIP">ip</param>
        /// <param name="ftpUser">用户id</param>
        /// <param name="PassWord">密码</param>
        public clsProject(string ProName,string ftpIP,string ftpUser,string PassWord)
        {
            this._ProName = ProName;
            this._ftpIP = ftpIP;
            this._ftpUSER = ftpUser;
            this._ftpPassWord = PassWord;
            ftp = new FTP_Class(_ftpIP, _ftpUSER, _ftpPassWord);
            ftp.FtpUpDown(_ftpIP, _ftpUSER, _ftpPassWord);          
        }
        #endregion
        #region 建立工程
        /// <summary>
        /// 建立工程，成功返回true,失败返回false
        /// </summary>
        /// <returns></returns>
        public bool Create()
        {
            if (null == _ProName || "" == _ProName)
                return false;
            try
            {               
                XmlDocument Doc = new XmlDocument();
                Doc.Load(ModData.v_ProjectInfoXML);
                XmlNodeList Nodelist = Doc.SelectSingleNode("项目目录").ChildNodes;
                if (null == Nodelist)
                    return false;
                StringBuilder result = new StringBuilder();
                String[] Floders;
                foreach (XmlNode Node1 in Nodelist)
                {
                    result.Append(Node1.InnerText);
                    result.Append("\n");
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                Floders = result.ToString().Split('\n');
                string err;
                ftp.MakeDir(_ProName,out  err);
                if (null == Floders)
                    return false;
                foreach (string FloderName in Floders)
                {
                    string errs="";
                    ftp.MakeDir(_ProName +"/"+ FloderName.Trim(),out errs);
                    if("Succeed"!=err)
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (null != ftp)
                    ftp = null;
            }
        }
        #endregion
        #region 删除一个工程
        /// <summary>
        /// 删除一个工程
        /// </summary>
        /// <returns></returns>
        public bool DeleteProject(DevComponents.AdvTree.Node ProjectNode, string ConStr,long Projiectid　,out string err)
        {
            Exception ex = null;
            err = "";
            if (string.IsNullOrEmpty(this._ProName))
            {
                err = "没有指定项目名称！";
                return false;
            }
            if (string.IsNullOrEmpty(ConStr))
            {
                err = "没有指定元信息库连接信息！";
                return false;
            }
            int value = 0;
            FrmProcessBar DelBar = new FrmProcessBar(10);
            DelBar.Show();
            DelBar.SetFrmProcessBarText("正在连接元信息库");
            Application.DoEvents();
            SysCommon.DataBase.SysTable pSysDB = new SysCommon.DataBase.SysTable();    //属性库连接类
            pSysDB.SetDbConnection(ConStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out ex);
            if (ex != null)
            {
                err = "元信息库连接失败！连接地址为：" + ConStr;
                pSysDB.CloseDbConnection();
                DelBar.Dispose();
                DelBar.Close();
                return false;
            }    
            long ProjectID = Projiectid;
            #region 删除项目的所有产品
            string Sql = "SELECT ID,产品名称,存储位置 FROM ProductMDTable WHERE 项目ID=" + ProjectID;
            DataTable GetTable = pSysDB.GetSQLTable(Sql,out ex);
            if (null != ex)
            {
                err = "获取项目的产品信息失败！";
                pSysDB.CloseDbConnection();
                DelBar.Dispose();
                DelBar.Close();
                return false;
            }
            if (null != GetTable)
            {
                for (int i = 0; i < GetTable.Rows.Count; i++)
                {
                   
                    long ProducetId = -1;
                    string ProductName = string.Empty;
                    string ProductPath = string.Empty;
                    ProducetId = long.Parse(GetTable.Rows[i][0].ToString());
                    ProductName = GetTable.Rows[i][1].ToString().Trim();
                    ProductPath = GetTable.Rows[i][2].ToString().Trim();
                    DelBar.SetFrmProcessBarValue(value);
                    DelBar.SetFrmProcessBarText(" 正在删除产品：" + ProductName);
                    value += 1;
                    if (value == 10)
                        value = 0;
                    Application.DoEvents();
                    DevComponents.AdvTree.Node ProductNode= ModDBOperator.GetTreeNode(ProjectNode,ProductPath, ProductName , EnumTreeNodeType.PRODUCT.ToString(), ProducetId, out ex);
                    if (null!=ex)
                    {
                        err = "获取项目的产品:‘" + ProductName + "’树节点失败！";
                        pSysDB.CloseDbConnection();
                        DelBar.Dispose();
                        DelBar.Close();
                        return false;
                    }
                    if (null != ProductNode)
                        if (!ModDBOperator.DelProduct(ProductNode, out ex))
                        {
                            err = "删除项目的产品:‘" + ProductName + "’失败！";
                            pSysDB.CloseDbConnection();
                            DelBar.Dispose();
                            DelBar.Close();
                            return false;
                        }
                }
            }
            #endregion
            #region 删除项目的文件夹
            DelBar.SetFrmProcessBarText("正在删除工程项目目录");
            Application.DoEvents();
            if (!ModDBOperator.DelDirtory(this._ftpIP, this._ftpUSER, this._ftpPassWord, this._ProName, out ex))
            {
                err = "工程项目项目目录删除失败！" ;
                pSysDB.CloseDbConnection();
                DelBar.Dispose();
                DelBar.Close();
                return false;
            }
            #endregion
            DelBar.Dispose();
            DelBar.Close();
            pSysDB.CloseDbConnection();
            return true;

        }
        #endregion
    }
}
