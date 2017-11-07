using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data;
using SysCommon;
using System.IO;
using System.Xml;
using System.Data.OracleClient;
using ESRI.ArcGIS.Geodatabase;


namespace GeoDBIntegration
{
    public partial class frmCreateDB : DevComponents.DotNetBar.Office2007Form
    {
        /*
         * guozheng 2010-9-29 添加
         * 该类实现各类型数据库的库体初始化，需要在主界面获取数据库的工程树的节点ProjectNode
         * 在节点中获取Tag属性挂接的数据库信息进行操作

         * 库体初始化完成后将连接信息挂接到ProjectNode上，并修改数据库的状态

         */
        private SysCommon.DataBase.SysTable m_DBOper;
        private string m_sDBFormat;
        private long lDBid = -1;
        private string sDBName = string.Empty;
        private string sDBType = string.Empty;
        private string sDBFormat = string.Empty;
        private string sDBState = string.Empty;
        //cyf 20110628 add
        private string sDBTypeID = string.Empty;
        private string sDBFormatID = string.Empty;
        private string sDBStateID = string.Empty;
        //end
        private string m_sDBConnectStr = string.Empty;/////////////////////用于记录文件库的连接信息字符串（元信息库除外）
        private string connectInfo = string.Empty;/////数据库连接字符串
        private enumInterDBFormat m_DBFormat;
        private enumInterDBType m_DBType;
        DevComponents.AdvTree.Node m_CurProNode = null;

        public frmCreateDB(DevComponents.AdvTree.Node ProjectNode)
        {
            InitializeComponent();
            if (ProjectNode == null) return;
            m_CurProNode = ProjectNode;
            /////////////////初始化系统维护库连接信息////////
            m_sDBFormat = string.Empty;
            Exception ex = null;
            //cyf 20110603 modify
            if (ModuleData.TempWks == null)
            { return; }
            //end
            ////////////////获取数据库信息//////////////////
            try
            {
                XmlElement DBInfoEle = ProjectNode.Tag as XmlElement;
                lDBid = Convert.ToInt64(DBInfoEle.GetAttribute("数据库ID"));
                sDBName = DBInfoEle.GetAttribute("数据库工程名");
                //cyf 20110628 add:
                sDBTypeID = DBInfoEle.GetAttribute("数据库类型ID");
                sDBFormatID = DBInfoEle.GetAttribute("数据库平台ID");
                sDBStateID = DBInfoEle.GetAttribute("数据库状态ID");
                //end
                sDBType = DBInfoEle.GetAttribute("数据库类型");
                sDBFormat = DBInfoEle.GetAttribute("数据库平台");
                sDBState = DBInfoEle.GetAttribute("数据库状态");
                this.TitleText = "初始化：" + sDBName + "库体";
                m_sDBFormat = sDBFormat;
            }
            catch (Exception eError)
            {
                //****************************************************
                if (ModuleData.v_SysLog != null)
                    ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
                //****************************************************
                this.m_DBOper = null;
                return;
            }
            ///////////////判断数据库平台类型，处理界面//////////////////
            #region 判断数据库平台类型，处理界面  cyf 20110628
            this.comBoxType.Text = sDBFormat;
            this.comBoxType.Enabled = false;
            if (sDBFormatID == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
            {
                this.txtServer.Enabled = false;
                this.txtUser.Enabled = false;
                this.txtPassWord.Enabled = false;
                this.txtVersion.Enabled = false;
                this.txtservername.Enabled = false;
                m_DBFormat = enumInterDBFormat.ARCGISPDB;
            }
            else if (sDBFormatID == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString())
            {
                this.txtServer.Enabled = false;
                this.txtUser.Enabled = false;
                this.txtPassWord.Enabled = false;
                this.txtVersion.Enabled = false;
                this.txtservername.Enabled = false;
                m_DBFormat = enumInterDBFormat.ARCGISGDB;
            }
            else if (sDBFormatID == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
            {
                this.txtVersion.Text = "SDE.DEFAULT";
                m_DBFormat = enumInterDBFormat.ARCGISSDE;
            }
            else if (sDBFormatID == enumInterDBFormat.GEOSTARACCESS.GetHashCode().ToString())
            {
                this.txtServer.Enabled = false;
                this.txtUser.Enabled = false;
                this.txtPassWord.Enabled = false;
                this.txtVersion.Enabled = false;
                this.txtservername.Enabled = false;
                m_DBFormat = enumInterDBFormat.GEOSTARACCESS;
            }
            else if (sDBFormatID == enumInterDBFormat.GEOSTARORACLE.GetHashCode().ToString())
            {
                m_DBFormat = enumInterDBFormat.GEOSTARORACLE;
                this.btnServer.Enabled = false;
                this.txtServer.Enabled = false;
                this.txtservername.Enabled = false;
            }
            else if (sDBFormatID == enumInterDBFormat.GEOSTARORSQLSERVER.GetHashCode().ToString())
            {
                m_DBFormat = enumInterDBFormat.GEOSTARORSQLSERVER;
                this.btnServer.Enabled = false;
                this.txtservername.Enabled = false;
            }
            else if (sDBFormatID == enumInterDBFormat.ORACLESPATIAL.GetHashCode().ToString())
            {
                this.txtDataBase.Enabled = false;
                this.btnServer.Enabled = false;
                this.txtVersion.Enabled = false;
                this.txtservername.Enabled = false;
                m_DBFormat = enumInterDBFormat.ORACLESPATIAL;
            }
            else if (sDBFormatID == enumInterDBFormat.FTP.GetHashCode().ToString())
            {
                this.comBoxType.Text = "Oracle";
                this.txtDataBase.Enabled = false;
                this.btnServer.Enabled = false;
                this.txtVersion.Enabled = false;
                this.txtservername.Enabled = false;
                this.txtProjFilePath.Enabled = false;
                this.textRuleFilePath.Enabled = false;
                m_DBFormat = enumInterDBFormat.FTP;
            }

            //if (sDBType == enumInterDBType.成果文件数据库.ToString())
            //{
            //    m_DBType = enumInterDBType.成果文件数据库;
            //}
            //else if (sDBType == enumInterDBType.地理编码数据库.ToString())
            //{
            //    m_DBType = enumInterDBType.地理编码数据库;
            //}
            //else if (sDBType == enumInterDBType.地名数据库.ToString())
            //{
            //    m_DBType = enumInterDBType.地名数据库;
            //}
            //else if (sDBType == enumInterDBType.高程数据库.ToString())
            //{
            //    m_DBType = enumInterDBType.高程数据库;
            //}
            //else if (sDBType == enumInterDBType.框架要素数据库.ToString())
            //{
            //    m_DBType = enumInterDBType.框架要素数据库;
            //}
            //else if (sDBType == enumInterDBType.影像数据库.ToString())
            //{
            //    m_DBType = enumInterDBType.影像数据库;
            //}
            //end
            #endregion
            //////////////获取连接信息显示在窗口上//////////////////////
            if (ProjectNode.Tag != null)
            {
                XmlElement InfoEle = ProjectNode.Tag as XmlElement;
                connectInfo = InfoEle.GetAttribute("数据库连接信息");
                string[] constr = connectInfo.Split('|');
                try
                {
                    //cyf 20110628 modify
                    if (sDBTypeID == enumInterDBType.成果文件数据库.GetHashCode().ToString())
                    {
                        string OracleConstr = constr[6];
                        m_sDBConnectStr = constr[0] + "|" + constr[1] + "|" + constr[2] + "|" + constr[3] + "|" + constr[4] + "|" + constr[5];
                        string User = string.Empty;///////Oracle用户
                        string Server=string.Empty;///////Oracle服务名
                        string Pass = string.Empty;///////Oracle用户密码
                        ExplainOracleConectInfo(OracleConstr, out User, out Server, out Pass, out ex);
                        if (ex != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取元信息库连接信息失败,\n原因：" + ex.Message);
                            return;
                        }
                        this.txtServer.Text = Server;
                        this.txtUser.Text = User;
                        this.txtPassWord.Text = Pass;
                    }
                    else
                    {
                        this.txtServer.Text = constr[0];
                        this.txtservername.Text = constr[1];
                        this.txtDataBase.Text = constr[2];
                        this.txtUser.Text = constr[3];
                        this.txtPassWord.Text = constr[4];
                        this.txtVersion.Text = constr[5];
                    }
                    //end
                }
                catch (Exception eError)
                {
                    //****************************************************
                    if (ModuleData.v_SysLog != null)
                        ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
                    //****************************************************
                }
            }
        }
        //设置数据库路径

        private void btnServer_Click(object sender, EventArgs e)
        {
            //cyf 20110628
            if (sDBFormatID == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())// "ARCGISSDE":
            { }
            else if (sDBFormatID == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString())// "ARCGISGDB":
            {
                FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    DirectoryInfo dir = new DirectoryInfo(pFolderBrowser.SelectedPath);
                    string name = dir.Name;
                    if (dir.Parent == null)
                    {
                        name = dir.Name.Substring(0, dir.Name.Length - 2);
                    }
                    txtDataBase.Text = dir.FullName + "\\" + name + ".gdb";
                }
            }
            else if (sDBFormatID == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())// "ARCGISPDB":
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Title = "保存为PDB数据";
                saveFile.Filter = "PDB数据(*.mdb)|*.mdb";
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    txtDataBase.Text = saveFile.FileName;
                }
            }
            else if (sDBFormatID == enumInterDBFormat.GEOSTARACCESS.GetHashCode().ToString())// "GEOSTARACCESS":
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Title = "打开ACCESS数据";
                open.Filter = "ACCESS数据(*.mdb)|*.mdb";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    txtDataBase.Text = open.FileName;
                }
            }
            //end
        }
        //设置投影文件路径
        private void btnProj_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "选择空间参考";
            OpenFile.Filter = "空间参考文件(*.prj)|*.prj";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                txtProjFilePath.Text = OpenFile.FileName;
                btnProj.Tooltip = OpenFile.FileName;
            }
        }
        //设置数据库配置方案路径

        private void btnRuleFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "选择库体配置文件";
            OpenFile.Filter = "库体配置文件(*.mdb)|*.mdb";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                textRuleFilePath.Text = OpenFile.FileName;
                btnRuleFile.Tooltip = OpenFile.FileName;
            }
        }

        private void btn_cancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //执行库体创建
        private void btn_OK_Click(object sender, EventArgs e)
        {
            #region 界面参数输入完整性控制
            //cf 20110628 modify:
            if (sDBFormatID != enumInterDBFormat.FTP.GetHashCode().ToString())
            {
                if (string.IsNullOrEmpty(textRuleFilePath.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "库体配置文件不能为空!"); return; }
            }

            if (sDBFormatID == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString() || sDBFormatID == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString())
            {
                if (string.IsNullOrEmpty(this.txtDataBase.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "数据库路径不能为空!"); return; }
                if (string.IsNullOrEmpty(this.txtProjFilePath.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "空间参考文件不能为空!"); return; }
            }
            else if (sDBFormatID == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
            {
                if (string.IsNullOrEmpty(this.txtServer.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "服务器不能为空!"); return; }
                if (string.IsNullOrEmpty(this.txtUser.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "用户不能为空!"); return; }
                if (string.IsNullOrEmpty(this.txtPassWord.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "密码不能为空!"); return; }
                if (string.IsNullOrEmpty(this.txtProjFilePath.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "空间参考文件不能为空!"); return; }
            }
            else if (sDBFormatID == enumInterDBFormat.GEOSTARACCESS.GetHashCode().ToString())
            {
                if (string.IsNullOrEmpty(this.txtDataBase.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "数据库路径不能为空!"); return; }
                if (string.IsNullOrEmpty(this.txtProjFilePath.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "空间参考文件不能为空!"); return; }
            }
            else if (sDBFormatID == enumInterDBFormat.GEOSTARORACLE.GetHashCode().ToString())
            {
                if (string.IsNullOrEmpty(this.txtProjFilePath.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "空间参考文件不能为空!"); return; }
                if (string.IsNullOrEmpty(this.txtDataBase.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "数据不能为空!"); return; }
                if (string.IsNullOrEmpty(this.txtUser.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "用户不能为空!"); return; }
                if (string.IsNullOrEmpty(this.txtPassWord.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "密码不能为空!"); return; }
            }
            else if (sDBFormatID == enumInterDBFormat.GEOSTARORSQLSERVER.GetHashCode().ToString())
            {
                if (string.IsNullOrEmpty(this.txtProjFilePath.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "空间参考文件不能为空!"); return; }
                if (string.IsNullOrEmpty(this.txtServer.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "服务器不能为空!"); return; }
                if (string.IsNullOrEmpty(this.txtDataBase.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "数据不能为空!"); return; }
                if (string.IsNullOrEmpty(this.txtUser.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "用户不能为空!"); return; }
                if (string.IsNullOrEmpty(this.txtPassWord.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "密码不能为空!"); return; }
            }
            else if (sDBFormatID == enumInterDBFormat.ORACLESPATIAL.GetHashCode().ToString())
            {
                if (string.IsNullOrEmpty(this.txtServer.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "服务器不能为空!"); return; }
                if (string.IsNullOrEmpty(this.txtUser.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "用户不能为空!"); return; }
                if (string.IsNullOrEmpty(this.txtPassWord.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "密码不能为空!"); return; }
            }
            else if (sDBFormatID == enumInterDBFormat.FTP.GetHashCode().ToString())
            {
                if (string.IsNullOrEmpty(this.txtServer.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "服务器不能为空!"); return; }
                if (string.IsNullOrEmpty(this.txtUser.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "用户不能为空!"); return; }
                if (string.IsNullOrEmpty(this.txtPassWord.Text)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示！", "密码不能为空!"); return; }
            }
            //end
            #endregion
            ///////开始库体创建////// 
            //string sDBFeaClsName = string.Empty;/////当前要素集名称 cyf 20110707 modify
            List<string> sDBClsNameLst = new List<string>();//cyf 20110707 add:数据集名称

            Exception ex = null;
            //cyf 20110622 modify:
            //int iScale = -1;
            string iScale = "";  //当前数据集比例尺信息
            //end

            string pAllScale = "";  //所有数据集比例尺信息，比例尺之间以“，”隔开
            string pAllDTName = ""; //所有数据集名称信息，名称之间以“，”隔开

            this.btn_OK.Enabled = false;
            this.btn_cancle.Enabled = false;
            string sMetaConnect = string.Empty;/////成果文件库元信息库连接字符串
            //*****************************************************
            //guozheng  写日志
            List<string> Pra = new List<string>();
            Pra.Add(sDBName);
            Pra.Add(sDBType);
            Pra.Add(sDBFormat);
            Pra.Add(this.connectInfo);
            Pra.Add(this.txtProjFilePath.Text);
            Pra.Add(this.textRuleFilePath.Text);
            if (ModuleData.v_SysLog != null)
                ModuleData.v_SysLog.Write("创建库体", Pra, DateTime.Now);
            //*****************************************************
            //////////////开始创建库体//////////////////////
            if (sDBTypeID == enumInterDBType.框架要素数据库.GetHashCode().ToString())//////框架要素库  cyf 20110628 
            {
                #region 框架要素库创建
                clsDBAdd DBCreater = new clsDBAdd();
                DBCreater.ProcBar = this.progressBar1;
                DBCreater.DataBaseOper = this.m_DBOper;
                //cyf 20110623 delete  cyf 20110707 modify
                DBCreater.DBCreate(sDBFormatID, sDBTypeID, this.txtServer.Text, this.txtservername.Text, this.txtDataBase.Text, txtUser.Text, txtPassWord.Text, this.txtVersion.Text, txtProjFilePath.Text, textRuleFilePath.Text, out iScale, out sDBClsNameLst, out ex);
                if (ex != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                    this.btn_OK.Enabled = true;
                    this.btn_cancle.Enabled = true;
                    return;
                }
                //end 
                this.btn_cancle.Enabled = true;
                #endregion

                //框架要素库创建完毕后，更新系统维护库信息
                DBCreater.UpdateDB(m_CurProNode, iScale, sDBClsNameLst, out pAllScale, out pAllDTName, out ex); //cyf 20110707 modify
            }
            else if (sDBTypeID == enumInterDBType.成果文件数据库.GetHashCode().ToString())/////////成果文件库  cyf 20110628
            {
                #region 成果文件库创建
                try
                {

                    sMetaConnect = "Data Source=" + this.txtServer.Text.Trim() + ";User ID=" + this.txtUser.Text.Trim() + ";Password=" + this.txtPassWord.Text.Trim();
                    clsFTPMetaDB MetaDBCreater = new clsFTPMetaDB(sMetaConnect);
                    MetaDBCreater.Creat(out ex);
                }
                catch (Exception eError)
                {
                    //****************************************************
                    if (ModuleData.v_SysLog != null)
                        ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
                    //****************************************************
                    ex = new Exception("获取元信息库连接信息失败");
                }

                if (ex != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "库体初始化失败！\n原因：" + ex.Message);
                    this.btn_OK.Enabled = true;
                    this.btn_cancle.Enabled = true;
                    return;
                }
                #endregion
                //更新系统维护库
                string sConnectInfo = m_sDBConnectStr + "|Data Source=" + this.txtServer.Text.Trim() + ";User ID=" + this.txtUser.Text.Trim() + ";Password=" + this.txtPassWord.Text.Trim();
                UpdateDBforFile(m_CurProNode, sConnectInfo);
            }
            else return;
            ////////////刷新界面////////////////////////
            clsRefurbishDBinfo ReOper = new clsRefurbishDBinfo();
            ReOper.DBOper = this.m_DBOper;
            DevComponents.AdvTree.Node projectNode = null;
            ReOper.RefurbishDBinfo(lDBid,out projectNode, out ex);
            if (ex != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库创建已成功，但界面信息刷新失败！\n原因：" + ex.Message);
                this.btn_OK.Enabled = true;
                this.btn_cancle.Enabled = true;
                return;
            }
            else
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库初始化成功！");
            }
            this.Close();

          
            //end
            #region 原有代码

            ////////////创建成功将更新系统维护库信息、刷新界面挂接信息////////
            /*
               string sConnectInfo = string.Empty;
               if (m_DBType == enumInterDBType.框架要素数据库)//////框架要素库在连接信息后增加要素集名称
               {
                   sConnectInfo = this.txtServer.Text + "|" + this.txtservername.Text + "|" + this.txtDataBase.Text + "|" + this.txtUser.Text + "|" + this.txtPassWord.Text + "|" + this.txtVersion.Text + "|" + sDBFeaClsName;
               }
               else if (m_DBType == enumInterDBType.成果文件数据库)////成果文件库在连接信息后增加元信息库连接字符串
               {
                   sConnectInfo = m_sDBConnectStr + "|Data Source=" + this.txtServer.Text.Trim() + ";User ID=" + this.txtUser.Text.Trim() + ";Password="+this.txtPassWord.Text.Trim();
               }
               else
               {
                   sConnectInfo = this.txtServer.Text + "|" + this.txtservername.Text + "|" + this.txtDataBase.Text + "|" + this.txtUser.Text + "|" + this.txtPassWord.Text + "|" + this.txtVersion.Text;
               }   
               //cyf 20110603 modify:修改系统维护库读取方式
               long lStateID = -1;
               IFeatureWorkspace pFeaWS = ModuleData.TempWks as IFeatureWorkspace;
               IQueryDef pQueryDes = pFeaWS.CreateQueryDef();
               pQueryDes.Tables = "DATABASESTATEMD";
               pQueryDes.SubFields = "ID";
               pQueryDes.WhereClause = "DATABASESTATE='库体已初始化'";
               try
               {
                   //查询表格
                   ICursor pCursor = pQueryDes.Evaluate();
                   if (pCursor == null)
                   {
                       SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库状态修改错误");
                       this.btn_OK.Enabled = true;
                       this.btn_cancle.Enabled = true;
                       return;
                   }
                   IRow pRow = pCursor.NextRow();
                   if (pRow != null)
                   {
                       lStateID = Convert.ToInt64(pRow.get_Value(0));
                   }
                   //释放游标
                   System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
               } catch { lStateID = -1; }
               if (lStateID == -1)
               {
                   SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库状态修改错误");
                   this.btn_OK.Enabled = true;
                   this.btn_cancle.Enabled = true;
                   return;
               }
               try///////更新数据库连接信息以及数据库状态ID
               {
                   string sql = "UPDATE DATABASEMD SET CONNECTIONINFO='" + sConnectInfo + "',DATABASSTATEID=" + lStateID.ToString() + ",SCALE=" + iScale+ " WHERE ID=" + lDBid.ToString();
                   ModuleData.TempWks.ExecuteSQL(sql);
                   if (ex != null)
                   {
                       SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接信息更新失败");
                       this.btn_OK.Enabled = true;
                       this.btn_cancle.Enabled = true;
                       return;
                   }
               } catch (Exception eError)
               {
                   //****************************************************
                   if (ModuleData.v_SysLog != null)
                       ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
                   //****************************************************
                   SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接信息更新失败");
                   this.btn_OK.Enabled = true;
                   this.btn_cancle.Enabled = true;
                   return;
               }
               */

            //long lStateID = -1;
            //string sql = "SELECT ID  FROM DATABASESTATEMD WHERE DATABASESTATE='库体已初始化'";
            //try
            //{
            //    DataTable gettable = this.m_DBOper.GetSQLTable(sql, out ex);
            //    lStateID = Convert.ToInt64(gettable.Rows[0][0].ToString());
            //}
            //catch { lStateID = -1; }
            //if (lStateID == -1)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库状态修改错误");
            //    this.btn_OK.Enabled = true;
            //    this.btn_cancle.Enabled = true;
            //    return;
            //}
            //try///////更新数据库连接信息以及数据库状态ID
            //{
            //    sql = "UPDATE DATABASEMD SET CONNECTIONINFO='" + sConnectInfo + "',DATABASSTATEID=" + lStateID.ToString() + ",SCALE=" + iScale.ToString() + " WHERE ID=" + lDBid.ToString();
            //    this.m_DBOper.UpdateTable(sql, out ex);
            //    if (ex != null)
            //    {
            //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接信息更新失败");
            //        this.btn_OK.Enabled = true;
            //        this.btn_cancle.Enabled = true;
            //        return;
            //    }
            //}
            //catch (Exception eError)
            //{
            //    //****************************************************
            //    if (ModuleData.v_SysLog != null)
            //        ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
            //    //****************************************************
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接信息更新失败");
            //    this.btn_OK.Enabled = true;
            //    this.btn_cancle.Enabled = true;
            //    return;
            //}
            //////////////刷新界面////////////////////////
            //clsRefurbishDBinfo ReOper = new clsRefurbishDBinfo();
            //ReOper.DBOper = this.m_DBOper;
            //DevComponents.AdvTree.Node projectNode = null;
            //ReOper.RefurbishDBinfo(lDBid, out projectNode, out ex);
            //if (ex != null)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库创建已成功，但界面信息刷新失败！\n原因：" + ex.Message);
            //    this.btn_OK.Enabled = true;
            //    this.btn_cancle.Enabled = true;
            //    return;
            //}
            //else
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库初始化成功！");
            //}
            //this.Close();
            #endregion
        }

        // *---------------------------------------------------------------------------------------
        // *开 发 者：chenyafei
        // *开发时间：20110623
        // *功能函数：更新系统维护库信息.将比例尺信息、连接字符串信息（数据集信息）、数据库状态信息写入数据库
        // *参    数：当前树节点,数据库连接字段值
        // *返 回 值：无返回值
        private void UpdateDBforFile(DevComponents.AdvTree.Node pCurNode, string sConnectInfo)
        {
            //获取当前数据源ID
            if (pCurNode.DataKey == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据库工程ID失败！");
                return;
            }
            string pDBID = pCurNode.DataKey.ToString();  //当前数据库工程ID
            if (ModuleData.TempWks == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取系统维护库工作空间失败！");
                return;
            }

            //更新数据库状态信息
            string updateStr = "UPDATE DATABASEMD SET CONNECTIONINFO='" + sConnectInfo + "',DATABASSTATEID=" + EnumDBState.库体已初始化.GetHashCode() + "  WHERE ID=" + Convert.ToInt32(pDBID);
            try
            {
                ModuleData.TempWks.ExecuteSQL(updateStr);
            }
            catch
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接信息更新失败");
                this.btn_OK.Enabled = true;
                this.btn_cancle.Enabled = true;
                return;
            }
        }

        //************************************************
        //guozheng 2011-1-6 added 
        //************************************************
        /// <summary>
        /// 解析Oracle连接字符串获取信息
        /// </summary>
        /// <param name="OracleConnectStr">Oracle连接字符串</param>
        /// <param name="User">Oracle用户</param>
        /// <param name="Server">Oracle服务名</param>
        /// <param name="Pass">Oracle密码</param>
        private void ExplainOracleConectInfo(string OracleConnectStr, out string User, out string Server, out string Pass,out Exception ex)
        {
            User = string.Empty;
            Server = string.Empty;
            Pass = string.Empty;
            ex = null;
            try
            {
                OracleConnectionStringBuilder ConStr = new OracleConnectionStringBuilder(OracleConnectStr);
                User = ConStr.UserID;
                Server = ConStr.DataSource;
                Pass = ConStr.Password;
            }
            catch(Exception eError)
            {
                //****************************************************
                if (ModuleData.v_SysLog != null)
                    ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
                //****************************************************
                ex = eError;
            }
        }
    }
}