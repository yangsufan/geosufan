using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.IO;
using System.Collections;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;

namespace GeoDBATool
{
    public partial class frmInitCaseEnviroment : DevComponents.DotNetBar.Office2007Form
    {
        private string m_Path = null;
        private IWorkspace _UpadateDesWorkspace = null;
        private string _DesDSName = null;
        private Plugin.Application.IAppGISRef m_Hook = null;

        public frmInitCaseEnviroment()
        {
            InitializeComponent();
        }

        public frmInitCaseEnviroment(Plugin.Application.IAppGISRef Hook)
        {
            InitializeComponent();
            m_Hook = Hook as Plugin.Application.IAppGISRef;
        }

        private void frmInitUpdateEnviroment_Load(object sender, EventArgs e)
        {
            object[] TagDBType = new object[] { "GDB", "SDE", "PDB" };
            comBoxType.Items.AddRange(TagDBType);
            comBoxType.SelectedIndex = 0;
            this.comboBoxEx1.SelectedIndex = 0;
            if (this.comboBoxEx1.Text == "ACCESS")
            {
                this.buttonXFile.Visible = true;

            }
            else
            {
                this.buttonXFile.Visible = false;
            }

            //buttonXOK.Enabled = false;
            buttonXGenerateFID.Enabled = false;
            //InitDBConnIdentify();

            ///填充现势库信息
            ///
            DevComponents.AdvTree.Node pCurNode = m_Hook.ProjectTree.SelectedNode; ///获得树图上选择的工程节点
            string pProjectname = pCurNode.Name;

            System.Xml.XmlNode Projectnode = m_Hook.DBXmlDocument.SelectSingleNode("工程管理/工程[@名称='" + pProjectname + "']");
            System.Xml.XmlElement ProjectNodeElement = Projectnode as System.Xml.XmlElement;

            System.Xml.XmlElement ProjectUserDBConnEle = ProjectNodeElement.SelectSingleNode(".//现势库/连接信息") as System.Xml.XmlElement;
            string DBType = ProjectUserDBConnEle.GetAttribute("类型");

            if (DBType == "PDB")
            {
                comBoxType.SelectedIndex = 2;
                string path = ProjectUserDBConnEle.GetAttribute("数据库");

                this.txtDataBase.Text = path;
            }
            else if (DBType=="GDB")
            {
                comBoxType.SelectedIndex = 0;
                string path = ProjectUserDBConnEle.GetAttribute("数据库");

                this.txtDataBase.Text = path;
            }
            else if(DBType=="SDE")
            {
                comBoxType.SelectedIndex = 1;
                this.txtServer.Text = ProjectUserDBConnEle.GetAttribute("服务器");
                this.txtInstance.Text = ProjectUserDBConnEle.GetAttribute("服务名");
                this.txtDataBase.Text = ProjectUserDBConnEle.GetAttribute("数据库");
                this.txtUser.Text = ProjectUserDBConnEle.GetAttribute("用户");
                this.txtPassWord.Text = ProjectUserDBConnEle.GetAttribute("密码");
                this.txtVersion.Text = ProjectUserDBConnEle.GetAttribute("版本");
            }
        }

        /// <summary>
        /// 获取oracle连接字符串，初始化下拉框
        /// </summary>
        private void InitDBConnIdentify()
        {

            //获取本机上Oracle的连接字符串，并初始化下拉列表中的内容
            #region 获取Oracle数据库的连接字符串信息
            RegistryKey regLocalMachine = Registry.LocalMachine;
            RegistryKey regSYSTEM = regLocalMachine.OpenSubKey("SYSTEM", true);//打开HKEY_LOCAL_MACHINE下的SYSTEM
            RegistryKey regControlSet001 = regSYSTEM.OpenSubKey("ControlSet001", true);//打开ControlSet001 
            RegistryKey regControl = regControlSet001.OpenSubKey("Control", true);//打开Control
            RegistryKey regManager = regControl.OpenSubKey("Session Manager", true);//打开Control

            RegistryKey regEnvironment = regManager.OpenSubKey("Environment", true);//打开MSSQLServer下的MSSQLServer
            m_Path = regEnvironment.GetValue("path").ToString();//读取path的值

            //根据分号截取字符串获得路径
            string[] Path = m_Path.Split(';');

            //tns文件路径
            string TnsnamesFilePath = null;

            //遍历数组查找有效的tnsnames.ora文件路径
            #region 查找获得tnsnames.ora文件路径
            for (int i = 0; i < Path.Length - 1; i++)
            {
                if (System.IO.File.Exists(Path[i] + "\\.." + @"\NETWORK\ADMIN\tnsnames.ora"))
                {
                    TnsnamesFilePath = Path[i] + "\\.." + @"\NETWORK\ADMIN\tnsnames.ora";
                }

            }

            #region 如果获得文件路径则读取里面的网络连接标识符，并初始化控件下拉框

            if (TnsnamesFilePath != null)
            {
                #region 读取文本中的内容
                StreamReader objReader = new StreamReader(TnsnamesFilePath);

                string sLine = "";
                string pTxt = "";
                ArrayList arrText = new ArrayList();

                while (sLine != null)
                {
                    sLine = objReader.ReadLine();

                    if (sLine != null)
                    {
                        //“#”“（”“）”开头的行去掉
                        if (!(sLine.IndexOf("#") > -1 || (sLine.IndexOf("(") > -1) || (sLine.IndexOf(")") > -1)))
                        {
                            pTxt = pTxt + sLine;

                            if (sLine != null)
                            {
                                arrText.Add(sLine);
                            }
                        }
                    }

                }
                objReader.Close();

                #endregion


                #region 正则表达式分离解析字符
                string[] pStringSegment = new string[100];
                //int[] pMatchposition = new int[];
                //通过正则表达式分离空格，解析每行字符
                Regex re = new Regex("\\S+", RegexOptions.None);
                MatchCollection mc = re.Matches(pTxt);
                int j = 0;

                for (int i = 0; i < mc.Count; i++) //在输入字符串中找到所有匹配
                {
                    if (!(mc[i].Value.StartsWith("=")))
                    {
                        pStringSegment[j] = mc[i].Value;
                    }
                    else if (!(mc[i].Value == "="))
                    {
                        pStringSegment[j] = mc[i].Value;
                        pStringSegment[j] = pStringSegment[j].Substring(1, pStringSegment[j].Length - 1);
                    }

                    j++;
                    //pStringSegment[i] = mc[i].Value; //将匹配的字符串添在字符串数组中
                    //pMatchposition[i] = mc[i].Index; //记录匹配字符的位置
                }
                #endregion
                //填充数据库连接标识符下拉框
                this.comboBoxEx1.Items.Clear();
                for (int i = 0; i < 99; i++)
                {

                    if (pStringSegment[i] != null)
                    {
                        this.comboBoxEx1.Items.Add(pStringSegment[i]);
                    }
                }
                if (pStringSegment[0] != null)
                {
                    this.comboBoxEx1.Text = pStringSegment[0];
                }
            }

            TnsnamesFilePath = null;
            #endregion

            #endregion

            #endregion


        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXCancel_Click(object sender, EventArgs e)
        {
            //释放类成员
            if (_UpadateDesWorkspace != null)
            { System.Runtime.InteropServices.Marshal.ReleaseComObject(_UpadateDesWorkspace); }
            this.Close();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            switch (comBoxType.Text)
            {
                case "SDE":

                    break;

                case "GDB":
                    FolderBrowserDialog pFolderBrowser = new FolderBrowserDialog();
                    if (pFolderBrowser.ShowDialog() == DialogResult.OK)
                    {
                        if (!pFolderBrowser.SelectedPath.EndsWith(".gdb"))
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择GDB");
                            return;
                        }
                        txtDataBase.Text = pFolderBrowser.SelectedPath;
                    }
                    break;

                case "PDB":
                    OpenFileDialog OpenFile = new OpenFileDialog();
                    OpenFile.Title = "选择ESRI个人数据库";
                    OpenFile.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
                    if (OpenFile.ShowDialog() == DialogResult.OK)
                    {
                        txtDataBase.Text = OpenFile.FileName;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 连接更新目标数据库，获得工作空间与数据集
        /// </summary>
        /// <param name="DesWorkspace">输出的工作空间</param>
        /// <returns></returns>
        private bool GetDBDataset(out IWorkspace DesWorkspace)
        {
            IWorkspaceFactory pWorkspaceFactory = null;
            IPropertySet pPropertySet = new PropertySetClass();

            comboBoxEx2.Items.Clear();

            try
            {
                if (comBoxType.SelectedIndex==1)  //SDE库
                {
                    pWorkspaceFactory = new SdeWorkspaceFactoryClass();

                    pPropertySet.SetProperty("SERVER", txtServer.Text);
                    pPropertySet.SetProperty("INSTANCE", txtInstance.Text);
                    pPropertySet.SetProperty("USER", txtUser.Text);
                    pPropertySet.SetProperty("PASSWORD", txtPassWord.Text);
                    pPropertySet.SetProperty("VERSION", txtVersion.Text);

                }
                else if (comBoxType.SelectedIndex == 0)   //GDB库
                {
                    pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                    pPropertySet.SetProperty("DATABASE", txtDataBase.Text);
                }
                else if (comBoxType.SelectedIndex == 2)  //PDB库体
                {
                    pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                    pPropertySet.SetProperty("DATABASE", txtDataBase.Text);
                }

                DesWorkspace = pWorkspaceFactory.Open(pPropertySet, 0);
                if (DesWorkspace == null)
                {
                    return false;
                }

                IEnumDatasetName pEnumDatasetName = DesWorkspace.get_DatasetNames(esriDatasetType.esriDTFeatureDataset);

                if (pEnumDatasetName == null)
                {
                    _DesDSName = null;
                    return true;
                }

                IDatasetName pDatasetName = pEnumDatasetName.Next();

                while (pDatasetName != null)
                {

                    comboBoxEx2.Items.Add(pDatasetName.Name);

                    pDatasetName = pEnumDatasetName.Next();
                }

                if (comboBoxEx2.Items.Count > 0)
                {
                    comboBoxEx2.SelectedIndex = 0;
                }

                return true;
            }
            catch(Exception e)
            {

                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************

                DesWorkspace = null;
                return false;
            }

        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            bool Success = GetDBDataset(out this._UpadateDesWorkspace);

            if (!Success)
            {
                //buttonXOK.Enabled = false;
                buttonXGenerateFID.Enabled = false;
            }
            else
            {
                //buttonXOK.Enabled = true;
                buttonXGenerateFID.Enabled = true;
            }
        }

        /// <summary>
        /// 创建FID表填充信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXGenerateFID_Click(object sender, EventArgs e)
        {
            this._DesDSName = comboBoxEx2.Text;

            //构建FID
            if (comboBoxEx1.SelectedIndex == 0)   //本地辅助库
            {
                if (File.Exists(textBoxXServer.Text))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据文件已存在!\n"+ textBoxXServer.Text);
                    return;
                }
                //创建mdb文件
                ADOX.Catalog AdoxCatalog = new ADOX.Catalog();
                AdoxCatalog.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + textBoxXServer.Text + ";");
                ADOX.TableClass　tbl = new ADOX.TableClass();
                tbl.ParentCatalog = AdoxCatalog;
                tbl.Name = "FID记录表";

                ADOX.ColumnClass　col = new ADOX.ColumnClass();
                col.ParentCatalog = AdoxCatalog;
                col.Type = ADOX.DataTypeEnum.adInteger; // 必须先设置字段类型
                col.Name = "GOFID";
                col.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
                col.Properties["AutoIncrement"].Value = true;
                tbl.Columns.Append(col, ADOX.DataTypeEnum.adInteger, 0);
                
                col = new ADOX.ColumnClass();
                col.ParentCatalog = AdoxCatalog;
                col.Type = ADOX.DataTypeEnum.adLongVarWChar;
                col.Name = "FCNAME";
                col.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
                tbl.Columns.Append(col, ADOX.DataTypeEnum.adInteger, 0);

                col = new ADOX.ColumnClass();//增加一个本地OID字段
                col.ParentCatalog = AdoxCatalog;
                col.Type = ADOX.DataTypeEnum.adInteger;
                col.Name = "OID";
                col.Properties["Jet OLEDB:Allow Zero Length"].Value = false;
                tbl.Columns.Append(col, ADOX.DataTypeEnum.adInteger, 0);
                AdoxCatalog.Tables.Append(tbl);

                string pConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + textBoxXServer.Text + ";Persist Security Info=False;";
                GeoFIDCreator FIDCreator = new GeoFIDCreator(this._UpadateDesWorkspace, pConn, FIDDBType.access);
                FIDCreator.DatasetName = this._DesDSName;
                if (FIDCreator.CreateFID("FID记录表") == false)
                {
                    SysCommon.Error.ErrorHandle.ShowInform("提示", "创建业务信息表失败！");
                    return;
                }
            }
            else if (comboBoxEx1.SelectedIndex == 1)    //远程辅助库
            {
                string Conn = "Data Source=" + textBoxXServer.Text + ";User Id=" + textBoxXUser.Text + ";Password=" + textBoxXPassword.Text + ";";
                GeoFIDCreator FIDCreator = new GeoFIDCreator(this._UpadateDesWorkspace, Conn, FIDDBType.oracle);
                FIDCreator.DatasetName = this._DesDSName;
                if (FIDCreator.CreateFID("FID记录表") == false)
                {
                    SysCommon.Error.ErrorHandle.ShowInform("提示", "创建业务信息表失败！");
                    return;
                }
            }

            ///将FID记录表的访问方式写入xml文档对象
            ///
            DevComponents.AdvTree.Node pCurNode = m_Hook.ProjectTree.SelectedNode; ///获得树图上选择的工程节点
            string pProjectname = pCurNode.Name;

            System.Xml.XmlNode Projectnode = m_Hook.DBXmlDocument.SelectSingleNode("工程管理/工程[@名称='"+pProjectname+"']");
            System.Xml.XmlElement ProjectNodeElement = Projectnode as System.Xml.XmlElement;

            System.Xml.XmlElement ProjectConnEle = ProjectNodeElement.SelectSingleNode(".//FID记录表/连接信息") as System.Xml.XmlElement;

            System.Xml.XmlElement ProjectDBDSConnEle = ProjectNodeElement.SelectSingleNode(".//现势库/连接信息/库体") as System.Xml.XmlElement;
            ProjectDBDSConnEle.SetAttribute("名称", comboBoxEx2.Text);

            ///设置数据库连接类型
            ///
            if (this.comboBoxEx1.SelectedIndex==0)
            {
                ProjectConnEle.SetAttribute("类型","Access");
            }
            else if (this.comboBoxEx1.SelectedIndex == 1)
            {
                ProjectConnEle.SetAttribute("类型","Oracle");
            }

            ///设置具体连接方式
            ///
            if (this.comboBoxEx1.SelectedIndex == 0)
            {
                string text = textBoxXServer.Text;
                ProjectConnEle.SetAttribute("数据库", text);
            }
            else if (this.comboBoxEx1.SelectedIndex == 1)
            {

                ProjectConnEle.SetAttribute("数据库", textBoxXServer.Text);
                ProjectConnEle.SetAttribute("用户", textBoxXUser.Text);
                ProjectConnEle.SetAttribute("密码", textBoxXPassword.Text);
            }

            m_Hook.DBXmlDocument.Save(ModData.v_projectXML);           

            //释放类成员
            if (_UpadateDesWorkspace != null)
            { System.Runtime.InteropServices.Marshal.ReleaseComObject(_UpadateDesWorkspace); }


            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "业务维护信息初始化完成!");
            this.Close();
        }

        /// <summary>
        /// 输入数据集返回数据集下的要素类名称
        /// </summary>
        /// <param name="FIDTableName">数据集名称</param>
        /// <param name="FCName">数据集下的要素类名称</param>
        /// <returns></returns>
        private bool GetFCname(string DSName,out ArrayList FCName)
        {
            IEnumDataset pEnumDataset = null;
            IDataset pDT = null;
            FCName = new ArrayList();
            
            try
            {
                //根据名称获得数据集
                if (DSName == null)     //如果空间数据库中没有数据集，只有目标要素类
                {
                    pEnumDataset = this._UpadateDesWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass);

                    pDT = pEnumDataset.Next();

                    while (pDT != null)
                    {
                        FCName.Add(pDT.Name);
                        pDT = pEnumDataset.Next();
                    }

                }
                else     //如果指定了数据集
                {
                    IEnumDataset pSubEnumDataset = null;
                    pEnumDataset = this._UpadateDesWorkspace.get_Datasets(esriDatasetType.esriDTFeatureDataset);

                    IDataset pDataset = pEnumDataset.Next();
                    while (pDataset != null)
                    {
                        //从要素类名称中去掉用户名称（SDE中是带有用户名的），如果存在用户名的话\
                        string pFeatureClassName = pDataset.Name;
                        string[] pTemp = pFeatureClassName.Split('.');

                        if (pTemp.Length == 2)
                        {
                            pFeatureClassName = pTemp[1];
                        }
                        else
                        {
                            pFeatureClassName = pTemp[0];
                        }

                        if (pFeatureClassName == DSName)
                        {
                            pSubEnumDataset = pDataset.Subsets;
                            pDT = pSubEnumDataset.Next();

                            while (pDT != null)
                            {
                                FCName.Add(pDT.Name);
                                pDT = pSubEnumDataset.Next();
                            }
                        }
                        pDataset = pEnumDataset.Next();
                    }


                }

            }
            catch(Exception e)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(e, null, DateTime.Now);
                }
                //********************************************************************
                return false;
            }
            return true;
        }

        private void txtDataBase_TextChanged(object sender, EventArgs e)
        {
            this.btnServer.Tooltip = txtDataBase.Text;
        }

        private void comBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtServer.Text = "";
            txtInstance.Text = "";
            txtDataBase.Text = "";
            txtUser.Text = "";
            txtPassWord.Text = "";
            if (comBoxType.Text != "SDE")
            {
                btnServer.Visible = true;
                txtDataBase.Size = new Size(txtServer.Size.Width - btnServer.Size.Width, txtDataBase.Size.Height);
                txtServer.Enabled = false;
                txtInstance.Enabled = false;
                txtUser.Enabled = false;
                txtPassWord.Enabled = false;
                txtVersion.Enabled = false;
            }
            else
            {
                btnServer.Visible = false;
                txtDataBase.Size = new Size(txtServer.Size.Width, txtDataBase.Size.Height);
                txtServer.Enabled = true;
                txtInstance.Enabled = true;
                txtUser.Enabled = true;
                txtPassWord.Enabled = true;
                txtVersion.Enabled = true;

            }
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.Text == "ACCESS")
            {
                this.buttonXFile.Visible = true;
                textBoxXServer.Size = new Size(textBoxXUser.Size.Width - buttonXFile.Size.Width, textBoxXServer.Size.Height);
                textBoxXInstance.Enabled = false;
                textBoxXUser.Enabled = false;
                textBoxXPassword.Enabled = false;
            }
            else
            {
                textBoxXInstance.Enabled = true;
                textBoxXUser.Enabled = true;
                textBoxXPassword.Enabled = true;
                this.buttonXFile.Visible = false;
                textBoxXServer.Size = new Size(textBoxXUser.Size.Width, textBoxXServer.Size.Height);
            }
        }

        private void buttonXFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.OverwritePrompt = false;
            saveFile.Title = "保存为ESRI个人数据库";
            saveFile.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                textBoxXServer.Text = saveFile.FileName;
            }
        }

    }
}