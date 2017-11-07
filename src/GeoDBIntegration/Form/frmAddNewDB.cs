using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using SysCommon;
using System.IO;
using System.Xml;
using System.Net;
using System.Data.OracleClient;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;

namespace GeoDBIntegration
{
    public partial class frmAddNewDB : DevComponents.DotNetBar.Office2007Form
    {
        /*郭正 2010-9-28 添加 实现在系统维护库中添加一条新的数据库信息
         * 需要获取数据库类型、数据库平台信息、数据库名称信息
         */
        //private SysCommon.DataBase.SysTable m_DBOper;
        //cyf 20110615 add:系统维护库工作空间
        private IWorkspace m_WS=null;        //系统维护库连接工作空间
        private IFeatureWorkspace m_FeaWS=null; //系统维护库连接工作空间
        string m_UpdateType = "";   //是新增还是连接已有的
        string m_RasterType = "";     //栅格数据类型  cyf 201106009 add
        Dictionary<string, string> DicRasterInfo = new Dictionary<string, string>();//栅格数据图层名和类型（栅格编目、栅格数据集）//cyf 20110629

        // *****************************************************************************************
        // *功能函数：修改构造函数，修改数据库的连接方式
        // *修改日期：20110602
        // *修 改 者：陈亚飞
        #region 修改后的构造函数  陈亚飞 add
        /// </summary>
        /// <param name="DBConType">系统维护库连接类型</param>
        /// <param name="DBType">系统维护库数据库类型</param>
        /// <param name="sConInfo">系统维护库连接信息</param>
        public frmAddNewDB(IWorkspace pWs, string updateType)
        {
            InitializeComponent();

            //将按钮和菜单进行关联 cyf 20110626 add
            //int x=this.btnHistoryCon.Location.X;      //按钮左上角的坐标x
            //int y=this.btnHistoryCon.Location.Y;      //按钮左上角的坐标Y
            //System.Drawing.Point pPnt = new System.Drawing.Point(x, y);
            //this.contextMenuStrip1.Show((Control)this.btnHistoryCon,pPnt);
            this.btnHistoryCon.ContextMenuStrip = this.contextMenuStrip1;
            //end

            m_UpdateType = updateType;
            //cyf 20110615 add:
            m_WS=pWs;
            m_FeaWS=pWs as IFeatureWorkspace;
            if(m_WS==null||m_FeaWS==null) return;
            //end
            Exception ex = null;
            #region 初始化界面
            IFeatureWorkspace pFeaWs = pWs as IFeatureWorkspace;
            if (pFeaWs != null)
            {
                //查询系统维护库表格
                IQueryDef pQueryDef = pFeaWs.CreateQueryDef();
                // ////////////在系统维护库中获取数据库的类型信息//////////
                try
                {
                    //cyf 20110627 
                    DataTable mTable = new DataTable();
                    mTable = CreateTempTable();
                    //end
                    pQueryDef.Tables = "DATABASETYPEMD";
                    pQueryDef.SubFields = "ID,DATABASETYPE";
                    ICursor pCursor = pQueryDef.Evaluate();
                    if (pCursor != null)
                    {
                        IRow pRow = pCursor.NextRow();
                        while (pRow != null)
                        {
                            long lID = Convert.ToInt64(pRow.get_Value(0).ToString().Trim());
                            string sType = pRow.get_Value(1).ToString().Trim();
                            //cyf 20110627 modify:添加行
                            DataRow dtRow = mTable.NewRow();
                            dtRow[0] = lID.ToString();
                            dtRow[1] = sType.ToString();
                            mTable.Rows.Add(dtRow);
                            //int index = this.combox_DBType.Items.Add(sType);
                            //end
                            pRow = pCursor.NextRow();
                        }
                        //cyf 20110627 
                        if (mTable.Rows.Count > 0)
                        {
                            this.combox_DBType.DataSource = mTable;
                            this.combox_DBType.DisplayMember = "Name";
                            this.combox_DBType.ValueMember = "ID";
                        }
                        //cyf 20110615 modify:保护，释放游标
                        if (combox_DBType.Items.Count > 0)
                        {
                            this.combox_DBType.SelectedIndex = 1;
                        }
                        //end
                    }
                    //cyf 20110615 add:释放游标
                     System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                    //end
                } catch (Exception eError)
                {
                    //****************************************************
                    if (ModuleData.v_SysLog != null)
                        ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
                    //****************************************************
                    return;
                }
             
                // ////////////在系统维护库中获取数据库的类型信息、平台信息//////////
                try
                {
                    //cyf 20110627 
                    DataTable pTable = new DataTable();
                    pTable = CreateTempTable();
                    //end
                    pQueryDef = pFeaWs.CreateQueryDef();
                    pQueryDef.Tables = "DATABASEFORMATMD";
                    pQueryDef.SubFields = "ID ,DATABASEFORMAT";
                    ICursor pCursor = pQueryDef.Evaluate();
                    if (pCursor != null)
                    {
                        IRow pRow = pCursor.NextRow();
                        while (pRow != null)
                        {
                            long lID = Convert.ToInt64(pRow.get_Value(0).ToString().Trim());
                            string sType = pRow.get_Value(1).ToString().Trim();
                            //cyf 20110627 modify:添加行
                            DataRow dtRow = pTable.NewRow();
                            dtRow[0] = lID.ToString();
                            dtRow[1] = sType.ToString();
                            pTable.Rows.Add(dtRow);
                            //int index = this.combox_DBFormat.Items.Add(sType);
                            //end
                            pRow = pCursor.NextRow();
                        }
                        //cyf 20110627 
                        if (pTable.Rows.Count > 0)
                        {
                            this.combox_DBFormat.DataSource = pTable;
                            this.combox_DBFormat.DisplayMember ="Name";
                            this.combox_DBFormat.ValueMember = "ID";
                        }
                        //end
                        //cyf 20110615 modify:保护，释放游标
                        if (combox_DBFormat.Items.Count > 0)
                        {
                            //默认pdb格式
                            this.combox_DBFormat.SelectedIndex = 0;
                            //初次设置下拉框的值，不会触发SelectedIndexChanged事件
                            //added by chulili 20110705
                            #region 根据数据格式初始化界面 
                            this.txtUser.Enabled = false;
                            this.txtPassWord.Enabled = false;
                            this.txtVersion.Enabled = false;
                            if (m_UpdateType == EnumUpdateType.New.ToString())
                            {
                                this.btn_test.Enabled = false;
                            }
                            else
                            {
                                this.btn_test.Enabled = true;
                            }
                            this.txt_servername.Enabled = false;
                            this.txtServer.Enabled = false;
                            #endregion
                        }
                        //end
                    }
                    //cyf 20110615 add:释放游标
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                    //end
                } catch (Exception eError)
                {
                    //****************************************************
                    if (ModuleData.v_SysLog != null)
                        ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
                    //****************************************************
                    return;
                }
            }
            cmbScale.Items.Clear();
            //cyf 20110615 添加国标比例尺
            cmbScale.Items.AddRange(new object[] { 500,1000, 2000,5000, 10000, 50000,250000,1000000});
            //end
            cmbScale.SelectedIndex = 0;
            #endregion
            // OpenFTPPanel(false);
            this.groupPanel1.Enabled = false;
            this.groupPanel3.Text = "数据库连接设置";
            if (m_UpdateType == EnumUpdateType.New.ToString())
            {
                //添加数据库工程
                this.Text = "添加数据库工程";
                cmbScale.Enabled = false;
                //cyf 20110623 modify
                //cmbDataset.Enabled = false;
                lvDataset.Enabled = false;
                //end
            }
            else if (m_UpdateType == EnumUpdateType.Update.ToString())
            {
                //连接已有的数据库
                this.Text = "连接已有的数据库";
                cmbScale.Enabled = true;
                //cyf 20110623 modify
                //cmbDataset.Enabled = true;
                lvDataset.Enabled = true;
                //end
            }
        }
        #endregion

        //cyf 20110627 add:创建临时表，用来作为下拉列表框的数据源
        private DataTable CreateTempTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", System.Type.GetType("System.String"));
            dt.Columns.Add("Name", System.Type.GetType("System.String"));
            return dt;
        }

        // *end
        // *************************************************************************************************
        //cyf 20110627 modify:将类型中通过名称进行判断改为通过ID来进行判断this.combox_DBType.SelectedValue.ToString()/this.combox_DBFormat.SelectedValue.ToString()
        private void btn_OK_Click(object sender, EventArgs e)
        {
            Exception pError = null;
            if (string.IsNullOrEmpty(this.txt_DBName.Text))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入数据库工程名称");
                this.txt_DBName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.combox_DBType.Text))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择一个数据库类型");
                this.combox_DBType.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.combox_DBFormat.Text))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择一个数据库平台");
                this.combox_DBFormat.Focus();
                return;
            }
            //cyf 20110627 modify :将类型中通过名称进行判断改为通过ID来进行判断
            //if (this.combox_DBFormat.Text == "ARCGISSDE" || this.combox_DBFormat.Text == "GEOSTARORACLE" || this.combox_DBFormat.Text == "GEOSTARSQLSERVER" || this.combox_DBFormat.Text == "ORACLESPATIAL" || this.combox_DBFormat.Text == "FTP")
            if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.GEOSTARORACLE.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.GEOSTARORSQLSERVER.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ORACLESPATIAL.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.FTP.GetHashCode().ToString())
            {
                if (!Test_Server_Only(out pError))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "服务连接失败！详细信息：" + pError.Message);
                    return;
                }
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
            {
                if (string.IsNullOrEmpty(this.txtDataBase.Text.Trim()))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入数据库的本地路径");
                    this.txtDataBase.Focus();
                    return;
                }
            }
            //end
            //cyf 20110627 modify:将类型中通过名称进行判断改为通过ID来进行判断
            //cyf 20110615 add:测试栅格ftp服务连接,不管是连接已有的库体还新增的库体，都需要测试FTP连接测试
            if (combox_DBType.SelectedValue.ToString() == enumInterDBType.高程数据库.GetHashCode().ToString() || combox_DBType.SelectedValue.ToString() == enumInterDBType.影像数据库.GetHashCode().ToString())
            {
                //cyf 20110620  add
                //if (this.txtRootPath.Text.Trim() == "")
                //{
                //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请填写栅格数据存储的ftp目录！");
                //    return;
                //}
                //end
                //cyf 20110629 add
                //if (lvRootPath.CheckedItems.Count == 0)
                //{
                //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择栅格数据存储的ftp目录！");
                //    return;
                //}
                //end
                 // shduan add 20110629

                /*不要FTP模块测试
                if ( this.chkDBIsUpdate.Checked)//cyf 20110704 modify m_UpdateType == EnumUpdateType.Update.ToString() &&
                {

                }
                else
                {
                    if (!TestFtpServer(this.txt_metaServer.Text.Trim(), this.txt_metauser.Text.Trim(), this.txt_metapassword.Text.Trim()))
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "栅格目录数据的文件服务器连接失败！");
                        return;
                    }
                }
               不要FTP模块测试deleted by xisheng 20110914   end */
            }
            //cyf 20110609   必须要选择一个数据集  cyf 20110627 
            if (m_UpdateType == EnumUpdateType.Update.ToString() && this.combox_DBType.SelectedValue.ToString() == enumInterDBType.框架要素数据库.GetHashCode().ToString())
            {
                //连接已经有的库体
                //cyf 20110623 modify:
                bool beCheckOne = false;  //标志是否选中一个。一个都没有选中则为false
                if (lvDataset.Items.Count == 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择要连接的数据集");
                    this.lvDataset.Focus();
                    return;
                }
                else
                {
                    for (int i = 0; i < lvDataset.Items.Count; i++)
                    {
                        if (lvDataset.Items[i].Checked)
                        {
                            beCheckOne = true;
                            break;
                        }
                    }
                }
                if (!beCheckOne)
                {
                    //一个都没有选中
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择要连接的数据集");
                    this.lvDataset.Focus();
                    return;
                }
                //if (cmbDataset.Text.Trim() == "")
                //{
                //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择要连接的数据集");
                //    this.cmbDataset.Focus();
                //    return;
                //}
                //
            }
            //end

#region cyf 20110626 add:将数据库连接信息记录下来
            XmlDocument xmlDoc = new XmlDocument();
            if (!File.Exists(ModuleData.v_HistoryConnXML))
            {
                //创建xml文档
                xmlDoc.LoadXml("<ROOT></ROOT>");
                xmlDoc.Save(ModuleData.v_HistoryConnXML);
            }
            //加载文档
            xmlDoc.Load(ModuleData.v_HistoryConnXML);
            //创建历史连接节点
            XmlElement hisConnElem = null;   //历史连接节点
            string pConnStr = combox_DBFormat.Text.Trim() + "," + txtServer.Text.Trim() + "," + txt_servername.Text.Trim() + "," + txtDataBase.Text.Trim() + "," + txtUser.Text.Trim() + "," + txtVersion.Text.Trim();  //连接信息
            try { hisConnElem = xmlDoc.SelectSingleNode(".//历史连接[@连接信息='" + pConnStr + "']") as XmlElement; }
            catch { }
            if (hisConnElem == null)
            {
                //该节点为空，则创建
                hisConnElem = xmlDoc.CreateElement("历史连接");
                xmlDoc.DocumentElement.AppendChild(hisConnElem as XmlNode);
                //设置属性
                hisConnElem.SetAttribute("数据源", txt_DBName.Text.Trim());
                hisConnElem.SetAttribute("类型", combox_DBFormat.Text.Trim());
                hisConnElem.SetAttribute("连接信息", pConnStr);
                hisConnElem.SetAttribute("服务器", txtServer.Text.Trim());
                hisConnElem.SetAttribute("服务名", txt_servername.Text.Trim());
                hisConnElem.SetAttribute("数据库", txtDataBase.Text.Trim());
                hisConnElem.SetAttribute("用户", txtUser.Text.Trim());
                hisConnElem.SetAttribute("密码", "");
                hisConnElem.SetAttribute("版本", txtVersion.Text.Trim());
                //保存起来
                hisConnElem.OwnerDocument.Save(ModuleData.v_HistoryConnXML);
            }
#endregion
            ///////////////////在系统维护库中插入这一数据库信息////////////
            Exception ex = null;
            clsDBAdd DBAddOper = new clsDBAdd();
            int pScale = -1;    //比例尺
            if (m_UpdateType == EnumUpdateType.Update.ToString())
            {
                try
                {
                    pScale = Convert.ToInt32(cmbScale.Text.Trim());
                }
                catch
                { }
                if (pScale == -1)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "比例尺分母必须为数字！");
                    this.cmbScale.Focus();
                    return;
                }
            }
            #region cyf 20110627 delete
            //DBAddOper.DataBaseOper = this.m_DBOper;
            //enumInterDBType enumDbtype = enumDbtype = enumInterDBType.成果文件数据库;
            //if (this.combox_DBType.Text.Trim() == enumInterDBType.成果文件数据库.ToString()) enumDbtype = enumInterDBType.成果文件数据库;
            //else if (this.combox_DBType.Text.Trim() == enumInterDBType.地理编码数据库.ToString()) enumDbtype = enumInterDBType.地理编码数据库;
            //else if (this.combox_DBType.Text.Trim() == enumInterDBType.地名数据库.ToString()) enumDbtype = enumInterDBType.地名数据库;
            //else if (this.combox_DBType.Text.Trim() == enumInterDBType.高程数据库.ToString()) enumDbtype = enumInterDBType.高程数据库;
            //else if (this.combox_DBType.Text.Trim() == enumInterDBType.框架要素数据库.ToString()) enumDbtype = enumInterDBType.框架要素数据库;
            //else if (this.combox_DBType.Text.Trim() == enumInterDBType.影像数据库.ToString()) enumDbtype = enumInterDBType.影像数据库;
            //else
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不支持的数据库类型");
            //    return;
            //}
            //enumInterDBFormat enumDbformat = enumInterDBFormat.ARCGISGDB;
            //if (this.combox_DBFormat.Text.Trim() == enumInterDBFormat.ARCGISGDB.ToString()) enumDbformat = enumInterDBFormat.ARCGISGDB;
            //else if (this.combox_DBFormat.Text.Trim() == enumInterDBFormat.ARCGISPDB.ToString()) enumDbformat = enumInterDBFormat.ARCGISPDB;
            //else if (this.combox_DBFormat.Text.Trim() == enumInterDBFormat.ARCGISSDE.ToString()) enumDbformat = enumInterDBFormat.ARCGISSDE;
            //else if (this.combox_DBFormat.Text.Trim() == enumInterDBFormat.GEOSTARACCESS.ToString()) enumDbformat = enumInterDBFormat.GEOSTARACCESS;
            //else if (this.combox_DBFormat.Text.Trim() == enumInterDBFormat.GEOSTARORACLE.ToString()) enumDbformat = enumInterDBFormat.GEOSTARORACLE;
            //else if (this.combox_DBFormat.Text.Trim() == enumInterDBFormat.GEOSTARORSQLSERVER.ToString()) enumDbformat = enumInterDBFormat.GEOSTARORSQLSERVER;
            //else if (this.combox_DBFormat.Text.Trim() == enumInterDBFormat.FTP.ToString()) enumDbformat = enumInterDBFormat.FTP;
            //else if (this.combox_DBFormat.Text.Trim() == enumInterDBFormat.ORACLESPATIAL.ToString()) enumDbformat = enumInterDBFormat.ORACLESPATIAL;
            //else
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不支持的数据库平台");
            //    return;
            //}
            #endregion
            long NewDbID = -1;
            /////在数据库中增加新建的数据库信息，返回新建的数据库的ID号///////
            SysCommon.Gis.SysGisDataSet pSysDataset = new SysCommon.Gis.SysGisDataSet();   //连接ArcGIS库的变量
            string sConnectInfo = string.Empty;                        //数据库连接信息
            //cyf 20110615 add
            string ftpConnInfo = string.Empty;                         //栅格目录文件服务器ftp连接信息
            //string ftpRootPath = string.Empty;                         //栅格目录文件服务器ftp的根目录
            //end
            //cyf 20110627 modify
            if (this.combox_DBType.SelectedValue.ToString() == enumInterDBType.成果文件数据库.GetHashCode().ToString())/////若是文件库，将元信息库的连接字符串挂接在连接信息的末端
            {
                sConnectInfo = this.txtServer.Text + "|" + this.txt_servername.Text + "|" + this.txtDataBase.Text + "|" + this.txtUser.Text + "|" + this.txtPassWord.Text + "|" + this.txtVersion.Text;
                string sServer = this.txt_metaServer.Text;
                string sUser = this.txt_metauser.Text;
                string sPassword = this.txt_metapassword.Text;
                OracleConnectionStringBuilder ConnectBuilder = new OracleConnectionStringBuilder();
                ConnectBuilder.UserID = sUser;
                ConnectBuilder.Password = sPassword;
                ConnectBuilder.DataSource = sServer;
                string sConnect = ConnectBuilder.ConnectionString;
                sConnectInfo += "|" + sConnect;
            }
            else
            {
                //if (m_UpdateType == EnumUpdateType.New.ToString())
                //{
                sConnectInfo = this.txtServer.Text + "|" + this.txt_servername.Text + "|" + this.txtDataBase.Text + "|" + this.txtUser.Text + "|" + this.txtPassWord.Text + "|" + this.txtVersion.Text + "|";
                //}
                if (m_UpdateType == EnumUpdateType.Update.ToString())
                {
                    //连接已有的数据库,框架要素库选择要素集  cyf 20110627
                    if (this.combox_DBType.SelectedValue.ToString() == enumInterDBType.框架要素数据库.GetHashCode().ToString())
                    {
                        if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
                        {
                            #region 传递存储数据集名称
                            #region delete
                            //cyf 20110609 modify
                            //if (enumDbformat == enumInterDBFormat.ARCGISGDB)
                            //{
                            //    //数据集的名称也要写进去
                            //    //获得数据集的名称
                            //    pSysDataset.SetWorkspace(txtDataBase.Text.Trim(), enumWSType.GDB, out ex);
                            //    if (ex != null)
                            //    {
                            //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据库失败！原因：" + ex.Message);
                            //        return;
                            //    }
                            //}
                            //else if (enumDbformat == enumInterDBFormat.ARCGISPDB)
                            //{
                            //    //数据集的名称也要写进去
                            //    //获得数据集的名称
                            //    pSysDataset.SetWorkspace(txtDataBase.Text.Trim(), enumWSType.PDB, out ex);
                            //    if (ex != null)
                            //    {
                            //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据库失败！原因：" + ex.Message);
                            //        return;
                            //    }
                            //}
                            //else if (enumDbformat == enumInterDBFormat.ARCGISSDE)
                            //{
                            //    //数据集的名称也要写进去
                            //    //获得数据集的名称
                            //    pSysDataset.SetWorkspace(txtServer.Text.Trim(), txt_servername.Text.Trim(), txtDataBase.Text.Trim(), txtUser.Text.Trim(), txtPassWord.Text.Trim(), txtVersion.Text.Trim(), out ex);
                            //    if (ex != null)
                            //    {
                            //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据库失败！原因：" + ex.Message);
                            //        return;
                            //    }
                            //}

                            //List<string> LstDtName = pSysDataset.GetAllFeatureDatasetNames();
                            //if (LstDtName.Count == 0)
                            //{
                            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "该数据库下不存在数据集，请检查！");
                            //    return;
                            //}
                            //遍历数据集名称，将数据集的名称写到数据库去
                            //foreach (string item in LstDtName)
                            //{
                            //    if (item.ToLower().EndsWith("_GOH"))
                            //        continue;
                            //    //**********************//
                            //    //guozheng altered 增加数据集的用户判断
                            //    if (!item.ToUpper().StartsWith(this.txtUser.Text.Trim().ToUpper())) continue;
                            //    //去掉用户名
                            //    string GetDataSetName = item;
                            //    if (item.Contains("."))
                            //    {
                            //        GetDataSetName = item.Substring(item.LastIndexOf('.') + 1);
                            //    }
                            //    sConnectInfo += GetDataSetName;
                            //    break;
                            //}
                            #endregion
                            //将数据集的名称写到数据库去
                            //cyf 20110623 modify:
                            for (int i = 0; i < lvDataset.CheckedItems.Count; i++)
                            {
                                sConnectInfo += this.lvDataset.CheckedItems[i].Text.Trim() + ",";
                              
                            }
                            sConnectInfo = sConnectInfo.Substring(0, sConnectInfo.Length - 1);
                            //sConnectInfo += this.cmbDataset.Text.Trim();
                            //end
                            //end
                            #endregion
                        }
                    }
                    //cyf 201100609 modify :将栅格编目文件名称写进去
                    else if (this.combox_DBType.SelectedValue.ToString() == enumInterDBType.高程数据库.GetHashCode().ToString() || this.combox_DBType.SelectedValue.ToString() == enumInterDBType.影像数据库.GetHashCode().ToString())
                    {
                        //将数据集的名称写到数据库去
                        //cyf 20110623 modify:
                        m_RasterType = "";
                        for (int i = 0; i < lvDataset.CheckedItems.Count; i++)
                        {
                            sConnectInfo += this.lvDataset.CheckedItems[i].Text.Trim() + ",";
                            if (DicRasterInfo.ContainsKey(this.lvDataset.CheckedItems[i].Text.Trim()))
                            {
                                m_RasterType += DicRasterInfo[this.lvDataset.CheckedItems[i].Text.Trim()] + ",";  //cyf 20110629 
                            }
                        }
                        sConnectInfo = sConnectInfo.Substring(0, sConnectInfo.Length - 1);
                        //shduan 20110630 增加判断
                        if (m_RasterType != "")
                        {
                            m_RasterType = m_RasterType.Substring(0, m_RasterType.Length - 1);
                        }
                        //sConnectInfo += this.cmbDataset.Text.Trim();
                        //end
                        //end
                        //将栅格类型（栅格编目、栅格数据集）的信息写进去
                    }
                }
                //cyf 20110615 add:将栅格目录ftp地址和文件存放的根路径存储在数据库当中
                if (this.combox_DBType.SelectedValue.ToString() == enumInterDBType.高程数据库.GetHashCode().ToString() || this.combox_DBType.SelectedValue.ToString() == enumInterDBType.影像数据库.GetHashCode().ToString())
                {
                    ftpConnInfo = this.txt_metaServer.Text.Trim() + "|" + this.txt_metauser.Text.Trim() + "|" + this.txt_metapassword.Text.Trim();
                    //ftpRootPath = this.txtRootPath.Text.Trim();
                }
            }
            //*****************************************************
            //guozheng  写日志
            List<string> Pra = new List<string>();
            Pra.Add(this.txt_DBName.Text.Trim());
            Pra.Add(this.combox_DBFormat.Text.Trim());   //cyf 20110627 modifys
            Pra.Add(this.combox_DBType.Text.Trim());//cyf 20110627 modify
            Pra.Add(sConnectInfo);
            if (ModuleData.v_SysLog != null)
                ModuleData.v_SysLog.Write("加载数据库", Pra, DateTime.Now);
            //*****************************************************
            //cyf 20110627 add
            long pDBTypeID = -1;              //数据库类型ID(高程、框架等)
            long pDBFormatID = -1;            //数据库格式ID(pdb、gdb等)
            try { pDBTypeID = Convert.ToInt64(this.combox_DBType.SelectedValue.ToString()); }
            catch { }
            try { pDBFormatID = Convert.ToInt64(this.combox_DBFormat.SelectedValue.ToString()); }
            catch { }
            if (pDBTypeID == -1 || pDBFormatID == -1)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据类型ID或数据格式ID出错！");
                return;
            }
            DBAddOper.AddNewDB(this.txt_DBName.Text.Trim(), pDBTypeID, pDBFormatID, sConnectInfo, ftpConnInfo, m_RasterType, out NewDbID, m_UpdateType, pScale, out ex);
            if (ex != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                return;
            }

            #region 若连接数据库，还需要初始化两张日志记录表
            if (m_UpdateType == EnumUpdateType.Update.ToString())
            {
                if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
                {
                    //ArcGIS库
                    if (this.combox_DBType.SelectedValue.ToString() == enumInterDBType.框架要素数据库.GetHashCode().ToString())
                    {
                        if (pSysDataset != null)
                        {
                            #region 创建日志表
                            if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString())
                            {
                                CreateSQLTable(pSysDataset.WorkSpace, "GDB", out ex);
                            }
                            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
                            {
                                CreateSQLTable(pSysDataset.WorkSpace, "PDB", out ex);
                            }
                            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
                            {
                                CreateSQLTable(pSysDataset.WorkSpace, "SDE", out ex);
                            }
                            #endregion
                        }
                    }
                }
                else
                { }
            }
            #endregion
            // shduan 20110630 屏蔽,成功的时候建议不采用弹出窗口提示
            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库增加成功！");
            //////////////////接下来应该使用返回的NewDbID刷新界面信息//////////////////////
            clsRefurbishDBinfo RefOper = new clsRefurbishDBinfo();
            //cyf 20110602 
            //RefOper.DBOper = this.m_DBOper;
            //end
            DevComponents.AdvTree.Node ProjectNode = null;
            RefOper.RefurbishDBinfo(NewDbID, out ProjectNode, out ex);
            #region cyf 20110609 delete
            //*****************************************************************************************//
            //guozheng 2011-5-10 added 若是连接到栅格库体上，则需要选择栅格数据集，以及栅格数据集的参数//
            //if (m_UpdateType == EnumUpdateType.Update.ToString() && (enumDbtype == enumInterDBType.影像数据库 || enumDbtype == enumInterDBType.高程数据库))
            //{
            //    /////////////////栅格数据集选择窗体////////////////////////
            //    FrmSetRasterDB SetRasterDB = new FrmSetRasterDB(NewDbID, enumDbtype.ToString());
            //    if (SetRasterDB.ShowDialog() == DialogResult.OK)
            //    {
            //        string dbtypeStr = SetRasterDB.s1dbtypeStr;                  //栅格数据类型
            //        string pResampleStr = SetRasterDB.s2pResampleStr;            //重采样
            //        string pCompressionStr = SetRasterDB.s3pCompressionStr;      //压缩类型
            //        string pPyramidStr = SetRasterDB.s4pPyramidStr;              //金字塔 
            //        string pTileHStr = SetRasterDB.s5pTileHStr;                  //瓦片高度
            //        string pTileWStr = SetRasterDB.s6pTileWStr;                  //瓦片宽度
            //        string pBandStr = SetRasterDB.s7pBandStr;                    //波段
            //        string pDTName = SetRasterDB.s8pDTName;                      //数据集名称
            //        string sNewConInfo = sConnectInfo + pDTName;
            //        string sRasterPara = dbtypeStr + "|" + pResampleStr + "|" + pCompressionStr + "|" + pPyramidStr + "|" + pTileHStr + "|" + pTileWStr + "|" + pBandStr;
            //        DBAddOper.UpDataDBInfo(NewDbID, "CONNECTIONINFO", "'" + sNewConInfo + "'", out ex);
            //        DBAddOper.UpDataDBInfo(NewDbID, "DBPARA", "'" + sRasterPara + "'", out ex);
            //    }
            //}

            //*****************************************************************************************//
            #endregion
            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示","数据库添加成功!");
            this.Close();
        }

        /// <summary>
        /// 创建远程日志表 陈亚飞添加
        /// </summary>
        /// <param name="pDBType">工作空间类型：PDB、GDB、SDE</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        public bool CreateSQLTable(IWorkspace m_WorkSpace,string pDBType, out Exception eError)
        {
            //创建远程日志表
            eError = null;

           string netLogPath=Application.StartupPath + @"\..\Template\Network_Log.mdb";       //远程日志模板路径
            //检查远程日志表是否存在，只要有一张表格存在，就返回
            ITable pTable = null;
            ITable mTable = null;
            IFeatureWorkspace pFeaWS = m_WorkSpace as IFeatureWorkspace;
            if (pFeaWS == null) return false;
            try
            {
                pTable = pFeaWS.OpenTable("GO_DATABASE_UPDATELOG");
                if (pTable != null)
                {
                    eError = new Exception("远程日志表'GO_DATABASE_UPDATELOG'已经存在，请检查！");
                    return false;
                }
            }
            catch
            {
                try
                {
                    mTable = pFeaWS.OpenTable("go_database_version");
                    if (mTable != null)
                    {
                        eError = new Exception("远程日志表'go_database_version'已经存在，请检查！");
                        return false;
                    }
                }
                catch
                {

                }
            }

            //创建表格
            try
            {
                if (pDBType.ToUpper() == "PDB")
                {
                    m_WorkSpace.ExecuteSQL("create table GO_DATABASE_UPDATELOG (OID  integer,STATE integer,LAYERNAME varchar(50),USERNAME varchar(255),LASTUPDATE date,VERSION integer,XMIN float,XMAX float,YMIN float,YMAX float)");
                    m_WorkSpace.ExecuteSQL("create table go_database_version (VERSION  integer,USERNAME varchar(255),VERSIONTIME date,DES varchar(255))");

                }
                else if (pDBType.ToUpper() == "SDE")
                {
                    m_WorkSpace.ExecuteSQL("create table GO_DATABASE_UPDATELOG (OID  INTEGER,STATE INTEGER,LAYERNAME NVARCHAR2(50),USERNAME NVARCHAR2(255),LASTUPDATE DATE,VERSION INTEGER,XMIN FLOAT,XMAX FLOAT,YMIN FLOAT,YMAX FLOAT)");
                    m_WorkSpace.ExecuteSQL("create table go_database_version (VERSION  INTEGER,USERNAME NVARCHAR2(255),VERSIONTIME DATE,DES NVARCHAR2(255))");
                }
                else if (pDBType.ToUpper() == "GDB")
                {
                    string tempFile = netLogPath;
                    FileInfo pFI = new FileInfo(tempFile);
                    string fName = pFI.Name;  //远程日志表名

                    //日志存储路径
                    string dbPath = m_WorkSpace.PathName;
                    int index = dbPath.LastIndexOf('\\');
                    if (index == -1) return false;
                    string FileDic = dbPath.Substring(0, index);
                    string FileName = FileDic + "\\" + fName;

                    if (File.Exists(FileName))
                    {
                        if (!SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "日志文件'" + fName + "'已存在,\n是否替换？"))
                        {
                            return true;
                        }
                        else
                        {
                            File.Delete(FileName);
                        }
                    }
                    File.Copy(tempFile, FileName);
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaWS);
                return true;
            }
            catch (System.Exception ex)
            {
                eError = ex;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaWS);
                return false;
            }

        }

        /// <summary>
        /// 将连接信息写入XML,针对框架要素库  陈亚飞添加 20100929
        /// </summary>
        /// <param name="proID">数据库工程ID</param>
        /// <param name="proName">数据库工程名称</param>
        /// <param name="strConType">数据库连接类型:PDB、GDB、SDE</param>
        /// <param name="strConn">数据库连接信息</param>
        private void AddProXml(string proID, string proName, string strConType, string strConn, out Exception eError)
        {
            eError = null;
            string pServer = "";       //服务器

            string pInstance = "";     //服务名

            string pDB = "";           //数据库

            string pUser = "";         //用户
            string pPassword = "";     //密码
            string pVersion = "";      //版本

            //检查工程xml文件和工程xml模板文件是否存在
            if (!File.Exists(ModuleData.v_feaProjectXMLTemp))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "缺失模板文件,请检查!");
                return;
            }
            //加载工程xml文件
            XmlDocument xmlDoc = new XmlDocument();
            if (!File.Exists(ModuleData.v_feaProjectXML))
            {
                //若工程xml文件不存在，则创建xml文件
                xmlDoc.LoadXml("<工程管理></工程管理>");
                xmlDoc.Save(ModuleData.v_feaProjectXML);
            }
            xmlDoc.Load(ModuleData.v_feaProjectXML);

            //获得数据库连接参数

            string[] strArr = strConn.Split(new char[] { '|' });
            if (strArr.Length != 6)
            {
                eError = new Exception("连接字符串设置有误！");
                return;
            }
            pServer = strArr[0];
            pInstance = strArr[1];
            pDB = strArr[2];
            pUser = strArr[3];
            pPassword = strArr[4];
            pVersion = strArr[5];


            //创建工程管理节点下面的子节点 “工程”节点

            XmlElement proElement = xmlDoc.CreateElement("工程");
            xmlDoc.DocumentElement.AppendChild(proElement as XmlNode);

            //加载工程模板xml文件
            XmlDocument xmlDocTemple = new XmlDocument();
            xmlDocTemple.Load(ModuleData.v_feaProjectXMLTemp);
            //获得xml模板文件中的“工程”节点

            XmlNode nodeTemple = xmlDocTemple.SelectSingleNode(".//工程管理//工程");
            //将模板文件中的“工程”节点引入到新创建的xml文件中

            XmlNode DBXmlNodeNew = xmlDoc.ImportNode(nodeTemple, true);
            //设置工程节点的属性信息

            (DBXmlNodeNew as XmlElement).SetAttribute("编号", proID);
            (DBXmlNodeNew as XmlElement).SetAttribute("名称", proName);
            //(DBXmlNodeNew as XmlElement).SetAttribute("比例尺", strScale);
            //用设置好的节点替换原有的节点
            xmlDoc.DocumentElement.ReplaceChild(DBXmlNodeNew, proElement as XmlNode);


            XmlNode contextNode = DBXmlNodeNew.FirstChild;
            //遍历数据库子节点集合，设置数据库的连接属性

            foreach (XmlNode subNode in contextNode.ChildNodes)
            {
                string sVisible = (subNode as XmlElement).GetAttribute("是否显示");
                if (sVisible == bool.FalseString.ToLower()) continue;

                //连接信息节点
                XmlElement subElement = subNode.FirstChild as XmlElement;

                //设置连接信息属性

                if (strConType == "ARCGISPDB")
                {
                    subElement.SetAttribute("类型", "PDB");
                    subElement.SetAttribute("数据库", pDB);

                }
                else if (strConType == "ARCGISGDB")
                {
                    subElement.SetAttribute("类型", "GDB");
                    subElement.SetAttribute("数据库", pDB);
                }
                else if (strConType == "ARCGISSDE")
                {
                    subElement.SetAttribute("类型", "SDE");
                    subElement.SetAttribute("服务器", pServer);
                    subElement.SetAttribute("服务名", pInstance);
                    subElement.SetAttribute("数据库", pDB);
                    subElement.SetAttribute("用户", pUser);
                    subElement.SetAttribute("密码", pPassword);
                    subElement.SetAttribute("版本", pVersion);
                }
            }

            //保存设置
            xmlDoc.Save(ModuleData.v_feaProjectXML);
        }
        private void btn_cancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void combox_DBFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cyf 20110627 modyf
            this.txtServer.Enabled = true;
            this.txtDataBase.Enabled = true;
            this.txtUser.Enabled = true;
            this.txtPassWord.Enabled = true;
            this.txtVersion.Enabled = true;
            this.btn_test.Enabled = true;
            this.btnServer.Enabled = true;
            this.txt_servername.Enabled = true;
            this.txtServer.WatermarkText = "服务器IP地址或服务名";
            //OpenFTPPanel(false);
            //cyf 20110615 add:  
            if (this.combox_DBType.SelectedValue.ToString() == enumInterDBType.影像数据库.GetHashCode().ToString() || this.combox_DBType.SelectedValue.ToString() == enumInterDBType.高程数据库.GetHashCode().ToString())
            {
                this.groupPanel1.Enabled = true;
            }
            else
            {
                this.groupPanel1.Enabled = false;
            }
            //end
            this.groupPanel3.Text = "数据库连接设置";
            ////cyf 20110609 modify
            //if (this.combox_DBType.Text == enumInterDBType.框架要素数据库.ToString())
            //{
            //    if (m_UpdateType == EnumUpdateType.Update.ToString())
            //    {
            //        this.btn_test.Enabled = true;
            //    }
            //}
            //else
            //{
            //    if (m_UpdateType == EnumUpdateType.Update.ToString())
            //    {
            //        this.btn_test.Enabled = false;
            //    }
            //}
            ////end
            //cyf 20110626 add:清空界面信息
            this.txtServer.Text = "";
            this.txt_servername.Text = "";
            this.txtDataBase.Text = "";
            this.txtUser.Text = "";
            this.txtPassWord.Text = "";
            this.txtVersion.Text = "";
            this.lvDataset.Items.Clear();
            //end
            if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
            {
                
                this.txtUser.Enabled = false;
                this.txtPassWord.Enabled = false;
                this.txtVersion.Enabled = false;
                if (m_UpdateType == EnumUpdateType.New.ToString())
                {
                    this.btn_test.Enabled = false;
                }
                else
                {
                    this.btn_test.Enabled = true;
                }
                this.txt_servername.Enabled = false;
                this.txtServer.Enabled = false;
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString())
            {
                this.txtUser.Enabled = false;
                this.txtPassWord.Enabled = false;
                this.txtVersion.Enabled = false;
                if (m_UpdateType == EnumUpdateType.New.ToString())
                {
                    this.btn_test.Enabled = false;
                }
                else
                {
                    this.btn_test.Enabled = true;
                }
                this.txt_servername.Enabled = false;
                this.txtServer.Enabled = false;
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
            {
                this.txtVersion.Text = "SDE.DEFAULT";
                this.btnServer.Enabled = false;
                this.btn_test.Enabled = true;
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.GEOSTARACCESS.GetHashCode().ToString())
            {
                this.txtUser.Enabled = false;
                this.txtPassWord.Enabled = false;
                this.txtVersion.Enabled = false;
                this.btn_test.Enabled = false;
                this.txt_servername.Enabled = false;
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.GEOSTARORACLE.GetHashCode().ToString())
            {
                this.btnServer.Enabled = false;
                this.txtServer.Enabled = false;
                this.txt_servername.Enabled = false;
                this.txtVersion.Enabled = false;
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.GEOSTARORSQLSERVER.GetHashCode().ToString())
            {
                this.btnServer.Enabled = false;
                this.txt_servername.Enabled = false;
                this.txtVersion.Enabled = false;
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ORACLESPATIAL.GetHashCode().ToString())
            {
                this.txtDataBase.Enabled = false;
                this.btnServer.Enabled = false;
                this.txtVersion.Enabled = false;
                this.txt_servername.Enabled = false;
                this.txtServer.WatermarkText = "Oracle服务名";
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.FTP.GetHashCode().ToString())
            {
                this.txtDataBase.Enabled = false;
                this.btnServer.Enabled = false;
                this.txtVersion.Enabled = false;
                this.txt_servername.Enabled = false;
                //this.combox_DBType.Text = enumInterDBType.成果文件数据库.ToString();
                this.combox_DBType.SelectedValue = enumInterDBType.成果文件数据库.GetHashCode();
                //OpenFTPPanel(true);
                this.groupPanel1.Enabled = true;
                this.groupPanel3.Text = "FTP服务器连接设置";
            }
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            //cyf 20110627 modify
            if (combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
            { }
            else if (combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString())// "ARCGISGDB":
            {
                FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                {
                    if (m_UpdateType == EnumUpdateType.New.ToString())
                    {
                        DirectoryInfo dir = new DirectoryInfo(pFolderBrowser.SelectedPath);
                        string name = dir.Name;
                        if (dir.Parent == null)
                        {
                            name = dir.Name.Substring(0, dir.Name.Length - 2);
                        }
                        txtDataBase.Text = dir.FullName + "\\" + name + ".gdb";
                    }
                    else
                    {

                        if (pFolderBrowser.SelectedPath.EndsWith(".gdb"))
                        {
                            txtDataBase.Text = pFolderBrowser.SelectedPath;
                        }
                        else
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择GDB格式文件!");
                        }
                    }
                }
            }
            else if (combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())// "ARCGISPDB":
            {
                if (m_UpdateType == EnumUpdateType.New.ToString())
                {
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.Title = "保存为PDB数据";
                    saveFile.Filter = "PDB数据(*.mdb)|*.mdb";
                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        txtDataBase.Text = saveFile.FileName;
                    }
                }
                else
                {
                    OpenFileDialog OpenFile = new OpenFileDialog();
                    OpenFile.CheckFileExists = true;
                    OpenFile.CheckPathExists = true;
                    OpenFile.Title = "选择PDB数据";
                    OpenFile.Filter = "PDB数据(*.mdb)|*.mdb";
                    OpenFile.Multiselect = false;
                    if (OpenFile.ShowDialog() == DialogResult.OK)
                    {
                        txtDataBase.Text = OpenFile.FileName;
                    }
                }
            }
            else if (combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.GEOSTARACCESS.GetHashCode().ToString())// "GEOSTARACCESS":
            {
                if (m_UpdateType == EnumUpdateType.New.ToString())
                {
                    OpenFileDialog open = new OpenFileDialog();
                    open.Title = "打开ACCESS数据";
                    open.Filter = "ACCESS数据(*.mdb)|*.mdb";
                    if (open.ShowDialog() == DialogResult.OK)
                    {
                        txtDataBase.Text = open.FileName;
                    }
                }
                else
                { }
            }
        }
        /// <summary>
        /// 测试服务器连接

        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_test_Click(object sender, EventArgs e)
        {
            Exception pError = null;
            //cyf 20110609 modify  cyf 20110627 modify

            if (string.IsNullOrEmpty(this.txt_DBName.Text))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入数据库工程名称");
                this.txt_DBName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.combox_DBType.Text))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择一个数据库类型");
                this.combox_DBType.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.combox_DBFormat.Text))
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择一个数据库平台");
                this.combox_DBFormat.Focus();
                return;
            }
            //cyf 20110627 modify :将类型中通过名称进行判断改为通过ID来进行判断
            //if (this.combox_DBFormat.Text == "ARCGISSDE" || this.combox_DBFormat.Text == "GEOSTARORACLE" || this.combox_DBFormat.Text == "GEOSTARSQLSERVER" || this.combox_DBFormat.Text == "ORACLESPATIAL" || this.combox_DBFormat.Text == "FTP")
            if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.GEOSTARORACLE.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.GEOSTARORSQLSERVER.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ORACLESPATIAL.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.FTP.GetHashCode().ToString())
            {

                if (string.IsNullOrEmpty(this.txtServer.Text))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "服务器地址不能为空");
                    this.txtServer.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(this.txt_servername.Text))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "服务端口号不能为空");
                    this.txt_servername.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(this.txtDataBase.Text))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库名不能为空");
                    this.txtDataBase.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(this.txtUser.Text))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "用户名不能为空");
                    this.txtUser.Focus();
                    return;

                }
                if (string.IsNullOrEmpty(this.txtPassWord.Text))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "密码不能为空");
                    this.txtPassWord.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(this.txtVersion.Text))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "版本号不能为空");
                    this.txtVersion.Focus();
                    return;
                }
                if (!Test_Server_Only(out pError))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "服务连接失败！详细信息：" + pError.Message);
                    return;
                }
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
            {
                if (string.IsNullOrEmpty(this.txtDataBase.Text.Trim()))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请输入数据库的本地路径");
                    this.txtDataBase.Focus();
                    return;
                }
            }
            if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.GEOSTARORACLE.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.GEOSTARORSQLSERVER.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ORACLESPATIAL.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.FTP.GetHashCode().ToString())
            {
                if (!Test_Server(out pError))
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接失败！详细信息：" + pError.Message);
                else
                {
                    //if (lvDataset.Items.Count == 0)
                    //{
                    //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接成功！");//cyf 20110629
                    //}

                    //cyf 20110623 modify
                    if (lvDataset.Items.Count >= 0)
                    {
                        //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库连接成功！");//xisheng 20110727
                        //遍历所有的数据集名称，默认全部选中
                        for (int i = 0; i < lvDataset.Items.Count; i++)
                        {
                            lvDataset.Items[i].Checked = true;
                        }
                    }
                    //cyf  20110612 add
                    //if (cmbDataset.Items.Count > 0)
                    //{
                    //    cmbDataset.SelectedIndex = 0;
                    //}
                    //end
                }
            }
        }

        private void combox_DBType_SelectedIndexChanged(object sender, EventArgs e)
        {
               Exception outError=null;          //cyf 20110629 add
            //cyf 20110615 add
            this.groupPanel1.Text = "元信息库连接设置";
            txt_metatype.Text = "Oracle";
            this.txt_metaServer.WatermarkText = "Oracle服务名";
            //cyf 20110629 modify
            //txtRootPath.Enabled=false;
            //清空控件上的列表
            //lvRootPath.Items.Clear();
            //lvRootPath.Enabled = false;
            //end
            //end
            //cyf 20110627 modify
            if (combox_DBType.SelectedValue.ToString() == enumInterDBType.成果文件数据库.GetHashCode().ToString())
            {
                //this.combox_DBFormat.Text = enumInterDBFormat.FTP.ToString();
                this.combox_DBFormat.SelectedValue = enumInterDBFormat.FTP.GetHashCode();
                //OpenFTPPanel(true);
                this.groupPanel1.Enabled = true;
                this.groupPanel3.Text = "FTP服务器连接设置";
            }
            else
            {
                this.groupPanel3.Text = "数据库连接设置";
                //cyf 20110627 modify
                if (this.combox_DBFormat.SelectedValue != null)
                {
                    if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.FTP.GetHashCode().ToString())
                    {
                        this.combox_DBFormat.SelectedValue = enumInterDBFormat.ARCGISPDB.GetHashCode();
                        //this.combox_DBFormat.Text = enumInterDBFormat.ARCGISPDB.ToString();
                    }

                    /*不要FTP模块测试 deleted by xisheng 20110914

                    if (combox_DBType.SelectedValue.ToString() == enumInterDBType.影像数据库.GetHashCode().ToString() || combox_DBType.SelectedValue.ToString() == enumInterDBType.高程数据库.GetHashCode().ToString())
                    {
                        // shduan add 20110629 连接已有库体时，可不填FTP地址，说明此库不能用于入库更新 cyf 20110704 modify
                        //if (m_UpdateType == EnumUpdateType.New.ToString())
                        //{
                        //    //添加数据库工程
                        //    chkDBIsUpdate.Visible = false;
                        //}
                        //else if (m_UpdateType == EnumUpdateType.Update.ToString())
                        //{
                        //    //连接已有的数据库
                        //    chkDBIsUpdate.Visible = true ;
                        //}
                        //end *************************************************************************

                        //cyf 20110615 add
                        //若为影像库或者高程库，则添加ftp链接，以便将原始数据上传到ftp上
                        this.groupPanel1.Text = "FTP服务器连接设置";
                        txt_metatype.Text = "FTP";
                        this.txt_metaServer.WatermarkText = "FTP地址";
                        //txtRootPath.Enabled = true;
                        //lvRootPath.Enabled = true;
                        this.groupPanel1.Enabled = true;
                        
                        //将栅格目录ftp的根目录从数据库中读取出来
                        //if (m_FeaWS != null)
                        //{
                        //    //cyf 20110629 modify :修改栅格数据根目录加载
                        //    ICursor pCursor = ModDBOperate.GetCursor(m_FeaWS, "RasterFilePathInfo", "RasterFilePath", "", out outError);
                        //    if (outError != null || pCursor == null)
                        //    {
                        //        return;
                        //    }
                        //    IRow pRow = pCursor.NextRow();
                        //    while (pRow != null)
                        //    {
                        //        //this.txtRootPath.Text = pRow.get_Value(0).ToString().Trim();
                        //        lvRootPath.Items.Add(pRow.get_Value(0).ToString().Trim());
                        //        pRow = pCursor.NextRow();
                        //    }
                        //    //释放游标
                        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                        //}
                        //end
                    }
                    else
                    {
                        this.groupPanel1.Enabled = false;
                        //this.txtRootPath.Text = "";
                    }
                    不要FTP模块测试 deleted by xisheng 20110914   end */
                    this.groupPanel1.Enabled = false;//不要FTP模块测试,所以默认为false added by xisheng 20110914   end */
                }
            }
               
        }
        /// <summary>
        /// 展开文件库元信息库连接信息设置面板

       
        /// <summary>
        /// FTP元信息库连接测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_metatest_Click(object sender, EventArgs e)
        {
            //cyf 20110627 modify
            string sServer = this.txt_metaServer.Text;      //服务器
            string sUser = this.txt_metauser.Text;          //用户
            string sPassword = this.txt_metapassword.Text;  //密码
            if (combox_DBType.SelectedValue.ToString() == enumInterDBType.影像数据库.GetHashCode().ToString() || combox_DBType.SelectedValue.ToString() == enumInterDBType.高程数据库.GetHashCode().ToString())
            {
                //cyf 20110615 add:测试ftp连接
                //测试ftp连接
                if (TestFtpServer(sServer, sUser, sPassword))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接成功！");
                    return;
                }
                else
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接失败！");
                    return;
                }
                //end
            }
            else
            {
                //测试oracle连接
                OracleConnectionStringBuilder ConnectBuilder = new OracleConnectionStringBuilder();
                ConnectBuilder.UserID = sUser;
                ConnectBuilder.Password = sPassword;
                ConnectBuilder.DataSource = sServer;
                string sConnect = ConnectBuilder.ConnectionString;
                ClsDatabase OracleCon = new ClsDatabase(sConnect);
                try
                {
                    OracleCon.Open();
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接成功！");
                }
                catch
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接失败！");
                    return;
                }
                finally
                {
                    OracleCon.Close();
                }
            }
        }
        private bool Test_Server_Only(out Exception pError)
        {
            //cyf 20110627 modify
            pError = null;
            //cyf 20110623 modify
            //cmbDataset.Items.Clear();//added by  xisheng 06.17
            //lvDataset.Items.Clear();
            //cyf 20110627 modify
            if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
            {
                //cyf 20110609 测试数据库连接 包括SDE、PDB、gdb
                #region 更新后的代码
                SysCommon.Gis.SysGisDataSet pSysDt = new SysCommon.Gis.SysGisDataSet();
                if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString())
                {
                    pSysDt.SetWorkspace(this.txtDataBase.Text, enumWSType.GDB, out pError);
                }
                else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
                {
                    pSysDt.SetWorkspace(this.txtServer.Text, this.txt_servername.Text, this.txtDataBase.Text, this.txtUser.Text, this.txtPassWord.Text, this.txtVersion.Text, out pError);
                    #region SDEOracle连接测试，原有代码
                    //IPropertySet pPropSet = new PropertySetClass();
                    //IWorkspaceFactory pSdeFact = new SdeWorkspaceFactoryClass();
                    //pPropSet.SetProperty("SERVER", this.txtServer.Text);
                    //pPropSet.SetProperty("INSTANCE", this.txt_servername.Text);
                    //pPropSet.SetProperty("DATABASE", this.txtDataBase.Text);
                    //pPropSet.SetProperty("USER", this.txtUser.Text);
                    //pPropSet.SetProperty("PASSWORD", this.txtPassWord.Text);
                    //pPropSet.SetProperty("VERSION", this.txtVersion.Text);

                    //try
                    //{
                    //    IWorkspace _Workspace = pSdeFact.Open(pPropSet, 0);
                    //    pPropSet = null;
                    //    pSdeFact = null;
                    //    return true;
                    //}
                    //catch
                    //{
                    //    return false;
                    //}
                    #endregion
                }
                else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
                {
                    pSysDt.SetWorkspace(this.txtDataBase.Text, enumWSType.PDB, out pError);
                }
                if (pError != null)
                {
                    //pError = new Exception("连接数据库失败！");
                    return false;
                }
                IWorkspace pWs = pSysDt.WorkSpace;
                if (pWs == null) return false;
                #endregion
                return true;
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.GEOSTARORACLE.GetHashCode().ToString())
            {
                #region GeostarOracle连接测试
                string sConinfo = "Data Source=" + this.txtServer.Text + ";Persist Security Info=True;User ID=" + this.txtUser.Text + ";Password=" + this.txtPassWord.Text + ";Unicode=True";
                ClsDatabase OracleCon = new ClsDatabase(sConinfo);
                try
                {
                    OracleCon.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    OracleCon.Close();
                }
                #endregion
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.GEOSTARORSQLSERVER.GetHashCode().ToString())
            {
                #region GeostarSqlServer连接测试
                string sConinfo = "server=" + this.txtServer.Text + ";database=" + this.txtDataBase.Text + ";uid=" + this.txtUser.Text + ";pwd=" + this.txtPassWord.Text + ";";
                SqlConnection SqlCon = new SqlConnection(sConinfo);
                try
                {
                    SqlCon.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (SqlCon.State == ConnectionState.Open)
                        SqlCon.Close();
                }
                #endregion
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ORACLESPATIAL.GetHashCode().ToString())
            {
                #region OracleSpatial连接测试
                string sConinfo = "Data Source=" + this.txtServer.Text + ";Persist Security Info=True;User ID=" + this.txtUser.Text + ";Password=" + this.txtPassWord.Text + ";Unicode=True";
                ClsDatabase OracleCon = new ClsDatabase(sConinfo);
                try
                {
                    OracleCon.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    pError = ex;//add by xisheng 06.17
                    return false;
                }
                finally
                {
                    OracleCon.Close();
                }
                #endregion
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.FTP.GetHashCode().ToString())
            {
                #region ftp连接测试
                FtpWebRequest reqFTP = null;
                try
                {
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + this.txtServer.Text + "/"));
                    reqFTP.UseBinary = true;
                    // ftp用户名和密码
                    reqFTP.Credentials = new NetworkCredential(this.txtUser.Text, this.txtPassWord.Text);
                    reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                    reqFTP.EnableSsl = false;///////////////是否使用加密（true为使用加密）代理模式下无法使用加密

                    WebResponse response = reqFTP.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);//中文文件名

                    string line = reader.ReadLine();
                    return true;
                }
                catch (Exception ex)
                {
                    pError = ex;//add by xisheng 06.17
                    return false;
                }
                finally
                {
                    reqFTP = null;
                }
                #endregion

            }
            else return true;
        }
        private bool Test_Server(out Exception pError)
        {
            //cyf 20110627 modify
            pError = null;
            //cyf 20110623 modify
            //cmbDataset.Items.Clear();//added by  xisheng 06.17
            lvDataset.Items.Clear();
            //cyf 20110627 modify
            if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString() || this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
            {
                //cyf 20110609 测试数据库连接 包括SDE、PDB、gdb
                #region 更新后的代码
                SysCommon.Gis.SysGisDataSet pSysDt = new SysCommon.Gis.SysGisDataSet();
                if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString())
                {
                    if (!Directory.Exists(txtDataBase.Text))
                    {
                        pError = new Exception("数据库文件路径不存在！");
                        return false;
                    }
                    pSysDt.SetWorkspace(this.txtDataBase.Text, enumWSType.GDB, out pError);
                }
                else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
                {
                    pSysDt.SetWorkspace(this.txtServer.Text, this.txt_servername.Text, this.txtDataBase.Text, this.txtUser.Text, this.txtPassWord.Text, this.txtVersion.Text, out pError);
                    #region SDEOracle连接测试，原有代码
                    //IPropertySet pPropSet = new PropertySetClass();
                    //IWorkspaceFactory pSdeFact = new SdeWorkspaceFactoryClass();
                    //pPropSet.SetProperty("SERVER", this.txtServer.Text);
                    //pPropSet.SetProperty("INSTANCE", this.txt_servername.Text);
                    //pPropSet.SetProperty("DATABASE", this.txtDataBase.Text);
                    //pPropSet.SetProperty("USER", this.txtUser.Text);
                    //pPropSet.SetProperty("PASSWORD", this.txtPassWord.Text);
                    //pPropSet.SetProperty("VERSION", this.txtVersion.Text);

                    //try
                    //{
                    //    IWorkspace _Workspace = pSdeFact.Open(pPropSet, 0);
                    //    pPropSet = null;
                    //    pSdeFact = null;
                    //    return true;
                    //}
                    //catch
                    //{
                    //    return false;
                    //}
                    #endregion
                }
                else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString())
                {
                    if (!File.Exists(txtDataBase.Text))
                    {pError = new Exception("数据库路径不存在！");
                        return false;
                    }

                    pSysDt.SetWorkspace(this.txtDataBase.Text, enumWSType.PDB, out pError);
                }
                if (pError != null)
                {
                    pError = new Exception("连接数据库失败！");
                    return false;
                }
                IWorkspace pWs = pSysDt.WorkSpace;
                if (pWs == null) return false;
                if (m_UpdateType == EnumUpdateType.Update.ToString())
                {
                    //连接成功后，将数据集加载到下拉列表框中
                    if (this.combox_DBType.SelectedValue.ToString() == enumInterDBType.框架要素数据库.GetHashCode().ToString())
                    {
                        #region 框架要素库中数据集名称
                        List<string> LstDtName = pSysDt.GetAllFeatureDatasetNames();
                        #region 过滤用户，仅保留本用户数据集
                        //added by chulili 20110714 如果是sde用户，则对数据集进行过滤，仅保留本用户的数据集
                        //if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
                        //{
                        //    string strUsername=this.txtUser.Text;
                        //    //倒叙循环删除，以免漏删
                        //    for (int i = LstDtName.Count-1; i >=0; i--)
                        //    {//changed by chulili 20110721 支持大小写不同
                        //        if (!LstDtName[i].ToUpper().Contains(strUsername.ToUpper()+"."))
                        //        {
                        //            LstDtName.RemoveAt(i);
                        //        }
                        //    }
                        //}
                        //end add
                        #endregion
                        if (LstDtName.Count == 0)
                        {
                            pError = new Exception("该数据库下不存在数据集，请检查！");
                            return false;
                        }
                        //遍历数据集名称，将数据集加载在下拉列表框中
                        foreach (string item in LstDtName)
                        {
                            //历史数据集，不添加
                            if (item.ToLower().EndsWith("_GOH"))
                                continue;
                            string GetDataSetName = item;//数据集名称
                            //changed by chulili 20110704 带上用户名比较好，容易区分数据集
                            ////去掉用户名
                            //if (item.Contains("."))
                            //{
                            //    //**********************//
                            //    //guozheng altered 增加数据集的用户判断
                            //    //if (!item.ToUpper().StartsWith(this.txtUser.Text.Trim().ToUpper())) continue;
                            //    GetDataSetName = item.Substring(item.LastIndexOf('.') + 1);
                            //}
                            //cyf 20110623 modify:
                            //end changed by chulili
                            //将数据集添加在下拉列表框中
                            if (!lvDataset.Items.ContainsKey(GetDataSetName))
                            {
                                lvDataset.Items.Add(GetDataSetName);
                            }
                            //if (!cmbDataset.Items.Contains(GetDataSetName))
                            //{
                            //    cmbDataset.Items.Add(GetDataSetName);
                            //}
                            //end
                        }
                        //if (cmbDataset.Items.Count > 0)
                        //{
                        //    cmbDataset.SelectedIndex = 0;
                        //}
                        #endregion
                    }
                    else if (this.combox_DBType.SelectedValue.ToString() == enumInterDBType.影像数据库.GetHashCode().ToString() || this.combox_DBType.SelectedValue.ToString() == enumInterDBType.高程数据库.GetHashCode().ToString())
                    {
                        string pRasterType = "";   //cyf 20110629 栅格数据类型
                        IEnumDataset pEnumDataset = null;

                        pEnumDataset = pWs.get_Datasets(esriDatasetType.esriDTRasterDataset);
                        if (pEnumDataset == null)
                        {
                            pError = new Exception("获取栅格数据名称出错！");
                            return false;
                        }
                        pEnumDataset.Reset();
                        IDataset pDt = pEnumDataset.Next();
                        if (pDt != null)
                        {
                            pRasterType = "栅格数据集" + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|";
                        }
                        DicRasterInfo.Clear();  //cyf 20110629
                        #region 栅格数据库中Rd的名称和类型
                        //遍历栅格数据集
                        while (pDt != null)
                        {
                            string rasteName = ""; //栅格数据名称
                            rasteName = pDt.Name;
                            #region 过滤用户，仅保留本用户数据集
                            //added by chulili 20110714 如果是sde用户，则对数据集进行过滤，仅保留本用户的数据集
                            //if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
                            //{
                            //    string strUsername = this.txtUser.Text;
                            //    //倒叙循环删除，以免漏删
                            //    //changed by chulili 20110721 支持大小写不同
                            //    if (!rasteName.ToUpper().Contains(strUsername.ToUpper()+"."))
                            //    {
                            //        pDt = pEnumDataset.Next();
                            //        continue;
                            //    }

                            //}
                            //end add
                            #endregion
                            //changed by chulili 20110704 带上用户名比较好，容易区分数据集
                            //去掉用户名
                            //if (rasteName.Contains("."))
                            //{
                            //    rasteName = rasteName.Substring(rasteName.LastIndexOf('.') + 1);
                            //}
                            //cyf 20110623 modify:
                            //end changed by chulili
                            //将栅格数据名称添加在下拉列表框中
                            if (!lvDataset.Items.ContainsKey(rasteName))
                            {
                                lvDataset.Items.Add(rasteName);
                            }
                            //end
                            if (!DicRasterInfo.ContainsKey(rasteName))
                            {
                                DicRasterInfo.Add(rasteName, pRasterType);
                            }
                            pDt = pEnumDataset.Next();
                        }
                        #endregion

                        pEnumDataset = pWs.get_Datasets(esriDatasetType.esriDTRasterCatalog);
                        if (pEnumDataset == null)
                        {
                            pError = new Exception("获取栅格数据名称出错！");
                            return false;
                        }
                        pEnumDataset.Reset();
                        pDt = pEnumDataset.Next();
                        if (pDt == null)
                        {
                            pError = new Exception("获取栅格数据名称出错！");
                            return false;
                        }
                        else
                        {
                            pRasterType = "栅格编目" + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|";
                        }
                        #region 栅格数据库中Rd的名称和类型
                        //遍历栅格编目
                        while (pDt != null)
                        {
                            string rasteName = ""; //栅格数据名称
                            rasteName = pDt.Name;
                            #region 过滤用户，仅保留本用户数据集
                            //added by chulili 20110714 如果是sde用户，则对数据集进行过滤，仅保留本用户的数据集
                            //if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
                            //{
                            //    string strUsername = this.txtUser.Text;
                            //    //倒叙循环删除，以免漏删
                            //    //changed by chulili 20110721 支持大小写不同
                            //    if (!rasteName.ToUpper().Contains(strUsername.ToUpper() + "."))
                            //    {
                            //        pDt = pEnumDataset.Next();
                            //        continue;
                            //    }

                            //}
                            //end add
                            #endregion
                            //added by chulili 20110715过滤掉原本存在的历史库
                            if (!rasteName.ToLower().EndsWith("_GOH"))
                            {
                                //end added by chulili 
                                //陈亚飞  20110619  add；若为栅格编目，连接已有库体时，需要创建历史RasterCatalog
                                if (m_UpdateType == EnumUpdateType.Update.ToString() && pRasterType.StartsWith("栅格编目"))
                                {
                                    //cyf 20110619  add:添加创建历史RasterCatalog
                                    IRasterWorkspaceEx pRasterWSEx = pWs as IRasterWorkspaceEx;
                                    if (pRasterWSEx != null)
                                    {
                                        try
                                        {
                                            IRasterCatalog tempRasterCatalog = pRasterWSEx.OpenRasterCatalog(rasteName + "_GOH");
                                            if (tempRasterCatalog != null)
                                            {
                                                // MessageBox.Show("栅格数据'" + rasteName + "_GOH" + "'已经存在，请检查！");//打开已有的数据目录，存在的不必提示，不用创建。 changed by xisheng 
                                            }
                                        }
                                        catch (System.Exception ex)
                                        {
                                            //创建栅格目录,(未设置几何列空间参考和栅格列空间参考)
                                            CreateCatalog(pRasterWSEx, rasteName + "_GOH", "Raster", "Shape", null, null, "", false, out pError);
                                        }
                                    }
                                }
                                //changed by chulili 20110704 带上用户名比较好，容易区分数据集
                                //去掉用户名
                                //if (rasteName.Contains("."))
                                //{
                                //    rasteName = rasteName.Substring(rasteName.LastIndexOf('.') + 1);
                                //}
                                //cyf 20110623 modify:
                                //end changed by chulili 
                                //将栅格数据名称添加在下拉列表框中
                                if (!lvDataset.Items.ContainsKey(rasteName))
                                {
                                    lvDataset.Items.Add(rasteName);
                                }
                                //end
                                if (!DicRasterInfo.ContainsKey(rasteName))
                                {
                                    DicRasterInfo.Add(rasteName, pRasterType);
                                }
                            }
                            pDt = pEnumDataset.Next();
                        }
                        #endregion
                    }
                #endregion
                    //end
                }
                return true;
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.GEOSTARORACLE.GetHashCode().ToString())
            {
                #region GeostarOracle连接测试
                string sConinfo = "Data Source=" + this.txtServer.Text + ";Persist Security Info=True;User ID=" + this.txtUser.Text + ";Password=" + this.txtPassWord.Text + ";Unicode=True";
                ClsDatabase OracleCon = new ClsDatabase(sConinfo);
                try
                {
                    OracleCon.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    OracleCon.Close();
                }
                #endregion
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.GEOSTARORSQLSERVER.GetHashCode().ToString())
            {
                #region GeostarSqlServer连接测试
                string sConinfo = "server=" + this.txtServer.Text + ";database=" + this.txtDataBase.Text + ";uid=" + this.txtUser.Text + ";pwd=" + this.txtPassWord.Text + ";";
                SqlConnection SqlCon = new SqlConnection(sConinfo);
                try
                {
                    SqlCon.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (SqlCon.State == ConnectionState.Open)
                        SqlCon.Close();
                }
                #endregion
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.ORACLESPATIAL.GetHashCode().ToString())
            {
                #region OracleSpatial连接测试
                string sConinfo = "Data Source=" + this.txtServer.Text + ";Persist Security Info=True;User ID=" + this.txtUser.Text + ";Password=" + this.txtPassWord.Text + ";Unicode=True";
                ClsDatabase OracleCon = new ClsDatabase(sConinfo);
                try
                {
                    OracleCon.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    pError = ex;//add by xisheng 06.17
                    return false;
                }
                finally
                {
                    OracleCon.Close();
                }
                #endregion
            }
            else if (this.combox_DBFormat.SelectedValue.ToString() == enumInterDBFormat.FTP.GetHashCode().ToString())
            {
                #region ftp连接测试
                FtpWebRequest reqFTP = null;
                try
                {
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + this.txtServer.Text + "/"));
                    reqFTP.UseBinary = true;
                    // ftp用户名和密码
                    reqFTP.Credentials = new NetworkCredential(this.txtUser.Text, this.txtPassWord.Text);
                    reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                    reqFTP.EnableSsl = false;///////////////是否使用加密（true为使用加密）代理模式下无法使用加密

                    WebResponse response = reqFTP.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);//中文文件名

                    string line = reader.ReadLine();
                    return true;
                }
                catch (Exception ex)
                {
                    pError = ex;//add by xisheng 06.17
                    return false;
                }
                finally
                {
                    reqFTP = null;
                }
                #endregion

            }
            else return true;
        }
        #region 创建栅格数据相关函数
        /// <summary>
        /// 在Geodatabase中创建栅格数据编目

        /// </summary>
        /// <param name="pRasterWSEx">目标Geodatabase工作区</param>
        /// <param name="pCatalogName">栅格编目的名称</param>
        /// <param name="pRasterFielsName">栅格列的名称</param>
        /// <param name="pShapeFieldName">几何要素列名称(Shape)</param>
        /// <param name="pRasterSpatialRef">几何要素列空间参考</param>
        /// <param name="pGeoSpatialRef">栅格列空间参考</param>
        /// <param name="pKeyword"> 栅格编目表的字段</param>
        /// <param name="eError">ArcSDE 适用, 表示configuration keyword</param>
        /// <returns></returns>
        private IRasterCatalog CreateCatalog(IRasterWorkspaceEx pRasterWSEx, string pCatalogName, string pRasterFielsName, string pShapeFieldName, ISpatialReference pRasterSpatialRef, ISpatialReference pGeoSpatialRef, string pKeyword, bool ismanaged, out Exception eError)
        {
            eError = null;
            IRasterCatalog pRasterCat = null;
            try
            {
                #region 创建字段
                IFields pFields = new FieldsClass();
                IFieldsEdit pFeildsEdit = pFields as IFieldsEdit;
                IField pField = null;

                pField = CreateCommonField("Name", esriFieldType.esriFieldTypeString);
                if (pField == null)
                {
                    eError = new Exception("创建'name'字段出错！");
                    return null;
                }
                pFeildsEdit.AddField(pField);

                IField2 pField2 = CreateRasterField(pRasterFielsName, pRasterSpatialRef, ismanaged);
                if (pField == null)
                {
                    eError = new Exception("创建栅格字段出错！");
                    return null;
                }
                pFeildsEdit.AddField(pField2);
                pField = CreateShapeField(pShapeFieldName, pGeoSpatialRef);
                if (pField == null)
                {
                    eError = new Exception("创建几何字段出错！");
                    return null;
                }
                pFeildsEdit.AddField(pField);
                pField = CreateOIDField("OBJECTID");
                if (pField == null)
                {
                    eError = new Exception("创建OID字段出错！");
                    return null;
                }
                pFeildsEdit.AddField(pField);
                pField = null;
                if (pKeyword.Trim() == "")
                {
                    pKeyword = "defaults";
                }
                pFields = pFeildsEdit as IFields;

                //创建用户自定义字段

                #endregion

                pRasterCat = pRasterWSEx.CreateRasterCatalog(pCatalogName, pFields, pShapeFieldName, pRasterFielsName, pKeyword);

                return pRasterCat;

            }
            catch (System.Exception ex)
            {
                eError = new Exception("创建栅格编目出错！\n" + ex.Message);
                return null;
            }

        }

        /// <summary>
        /// 创建栅格字段
        /// </summary>
        /// <param name="pRasterFielsName">栅格字段名</param>
        /// <param name="pSpatialRes">栅格空间参考</param>
        /// <param name="eError"></param>
        /// <returns>返回字段</returns>
        private IField2 CreateRasterField(string pRasterFielsName, ISpatialReference pSpatialRes, bool isManaged)
        {
            IField2 pField = new FieldClass();
            IFieldEdit2 pFieldEdit = pField as IFieldEdit2;
            pFieldEdit.Name_2 = pRasterFielsName;
            pFieldEdit.AliasName_2 = pRasterFielsName;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeRaster;

            IRasterDef pRasterDef = new RasterDefClass();
            pRasterDef.Description = "this is Raster catalog";
            if (pSpatialRes == null)
            {
                //如果空间参考为空，则设置为UnknownCoordinateSystemClass
                pSpatialRes = new UnknownCoordinateSystemClass();
            }
            //only for PGDB
            pRasterDef.IsManaged = isManaged;
            pRasterDef.SpatialReference = pSpatialRes;
            pFieldEdit.RasterDef = pRasterDef;
            pField = pFieldEdit as IField2;

            return pField;
        }

        /// <summary>
        /// 创建shape字段
        /// </summary>
        /// <param name="pShapeFielsName">shape字段名</param>
        /// <param name="pSpatialRes">空间参考</param>
        /// <returns>返回字段</returns>
        private IField CreateShapeField(string pShapeFielsName, ISpatialReference pSpatialRes)
        {
            IField pField = new FieldClass();
            IFieldEdit pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Name_2 = pShapeFielsName;
            pFieldEdit.AliasName_2 = pShapeFielsName;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry;

            IGeometryDef pGeoDef = new GeometryDefClass();
            pGeoDef = CreateGeoDef(pSpatialRes);
            pFieldEdit.GeometryDef_2 = pGeoDef;
            pField = pFieldEdit as IField;
            return pField;
        }
      
        /// <summary>
        /// 创建用户自定义字段

        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldType"></param>
        /// <returns></returns>
        private IField CreateCommonField(string fieldName, esriFieldType fieldType)
        {
            IField pField = new FieldClass();
            IFieldEdit pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Name_2 = fieldName;
            pFieldEdit.AliasName_2 = fieldName;
            pFieldEdit.Type_2 = fieldType;
            pFieldEdit.Length_2 = 50;
            pField = pFieldEdit as IField;
            return pField;
        }

        /// <summary>
        /// 创建OID字段
        /// </summary>
        /// <param name="pOIDFieldName">OID字段名</param>
        /// <returns></returns>
        private IField CreateOIDField(string pOIDFieldName)
        {
            IField pField = new FieldClass();
            IFieldEdit pFieldEdit = pField as IFieldEdit;
            pFieldEdit.Name_2 = pOIDFieldName;
            pFieldEdit.AliasName_2 = pOIDFieldName;
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            pField = pFieldEdit as IField;
            return pField;
        }

        /// <summary>
        /// 设置几何空间参考定义

        /// </summary>
        /// <param name="pSpatialRes"></param>
        /// <returns></returns>
        private IGeometryDef CreateGeoDef(ISpatialReference pSpatialRes)
        {
            IGeometryDef pGeoDef = new GeometryDefClass();
            IGeometryDefEdit pGeoDefEdit = pGeoDef as IGeometryDefEdit;

            pGeoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
            pGeoDefEdit.AvgNumPoints_2 = 4;
            pGeoDefEdit.GridCount_2 = 1;
            pGeoDefEdit.set_GridSize(0, 1000);

            if (pSpatialRes == null)
            {
                pSpatialRes = new UnknownCoordinateSystemClass();
            }
            pGeoDefEdit.SpatialReference_2 = pSpatialRes;
            pGeoDef = pGeoDefEdit as IGeometryDef;
            return pGeoDef;
        }


        #endregion
        //cyf 20110615 add:测试ftp连接
        private bool TestFtpServer(string pServer,string pUser,string pPassword)
        {
            FtpWebRequest reqFTP = null;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + pServer + "/"));
                reqFTP.UseBinary = true;
                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(pUser, pPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.EnableSsl = false;///////////////是否使用加密（true为使用加密）代理模式下无法使用加密

                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);//中文文件名

                string line = reader.ReadLine();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                reqFTP = null;
            }
        }

        private void btnHistoryCon_MouseClick(object sender, MouseEventArgs e)
        {
            //cyf 20110626 add:添加历史连接按钮下拉菜单
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (contextMenuStrip1.Items.Count > 0)
                {
                    contextMenuStrip1.Items.Clear();
                }
                XmlDocument xmlDoc = new XmlDocument();
                if (!File.Exists(ModuleData.v_HistoryConnXML)) return;
                xmlDoc.Load(ModuleData.v_HistoryConnXML);
                XmlNodeList pHisNodeLst = null;      //历史连接信息节点
                try { pHisNodeLst = xmlDoc.DocumentElement.SelectNodes(".//历史连接"); }
                catch { }
                if (pHisNodeLst == null) return;
                List<string> LstConn = new List<string>();//用来进行唯一值过滤
                foreach (XmlNode pHisNode in pHisNodeLst)
                {
                    XmlElement pHisElem = pHisNode as XmlElement;
                    if (pHisElem == null) continue;
                    string pHisConn = "";               //历史连接信息
                    pHisConn = pHisElem.GetAttribute("连接信息");
                    string pName = pHisElem.GetAttribute("数据源").ToString();

                    //ToolStripItem pItem = new toolstripit
                    //pItem.Name = pHisConn;
                    //pItem.Text = pHisConn;
                    if (!LstConn.Contains(pHisConn))
                    {
                        //if (!contextMenuStrip1.Items.ContainsKey(pHisConn))
                        //{
                        contextMenuStrip1.Items.Add(pName);
                            LstConn.Add(pHisConn);
                        //}
                    }
                }
                //读取下拉菜单按钮
                this.contextMenuStrip1.Show(btnHistoryCon, new System.Drawing.Point(e.X, e.Y));
            } 
            //end
        }
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //cyf 20110626 add:从xml中读取连接信息，加载在控件上
           //string pHisConn= e.ClickedItem.Text;  //连接信息
           string pName = e.ClickedItem.Text;  //名称
           if (File.Exists(ModuleData.v_HistoryConnXML))
           {

               XmlDocument xmlDoc = new XmlDocument();
               xmlDoc.Load(ModuleData.v_HistoryConnXML);
               XmlElement pChickHisElem = null;   //点击的历史连接
               try { pChickHisElem = xmlDoc.DocumentElement.SelectSingleNode(".//历史连接[@数据源='" + pName + "']") as XmlElement; }
               catch { }
               if (pChickHisElem == null) return;
               //给控件上赋值
               combox_DBFormat.Text = pChickHisElem.GetAttribute("类型").Trim();
               txtServer.Text = pChickHisElem.GetAttribute("服务器").Trim();
               txt_servername.Text = pChickHisElem.GetAttribute("服务名").Trim();
               txtDataBase.Text = pChickHisElem.GetAttribute("数据库").Trim();
               txtUser.Text = pChickHisElem.GetAttribute("用户").Trim();
               txtVersion.Text = pChickHisElem.GetAttribute("版本").Trim();
           }
            //end
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.lvDataset.Items.Count; i++)
            {
                this.lvDataset.Items[i].Checked = true;
            }
        }

        private void btnSelectNot_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.lvDataset.Items.Count; i++)
            {
                if (this.lvDataset.Items[i].Checked)
                {
                    this.lvDataset.Items[i].Checked = false;
                }
                else
                {
                    this.lvDataset.Items[i].Checked = true;
                }
            }
        }

    }
}