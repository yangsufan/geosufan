using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;

namespace GeoDataCenterFunLib
{
    public partial class frmAnalyseInLibMap : DevComponents.DotNetBar.Office2007Form
    {
        public frmAnalyseInLibMap()
        {
            InitializeComponent();
        }

        string m_con;//连接字符串变量
        //string m_path;//路径
        List<string> m_list = new List<string>();//返回列表
        public static TreeNode Node;//数据单元树返回的节点
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //以行政区为主
        private void checkBoxArea_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxArea.Checked)
            {
                groupBoxArea.Enabled = true;
                //string str_Exp = "select 行政名称 from 数据单元表";
                //GeoDataCenterDbFun dbfun = new GeoDataCenterDbFun();
                //List<string> list = dbfun.GetDataReaderFromMdb(m_con, str_Exp);
                //for (int i = 0; i < list.Count; i++)
                //{
                //    comboBoxArea.Items.Add(list[i]);
                //}
                //if (comboBoxArea.Items.Count > 0)
                //    comboBoxArea.SelectedIndex = 0;
            }
            else
            { 
                groupBoxArea.Enabled = false;
                comboBoxArea.Items.Clear();
                comboBoxArea.Text = "";
            }
        }

        //以年度为主
        private void checkBoxYear_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBoxYear.Checked)
            {
                groupBoxYear.Enabled = true;
                string str_Exp = "select 年度 from 数据编码表";
                GeoDataCenterDbFun dbfun = new GeoDataCenterDbFun();
                List<string> list = dbfun.GetDataReaderFromMdb(m_con, str_Exp);
                for (int i = 0; i < list.Count; i++)
                {
                    if (!comboBoxYear.Items.Contains(list[i]))
                    comboBoxYear.Items.Add(list[i]);
                    
                }
                if (comboBoxYear.Items.Count > 0)
                    comboBoxYear.SelectedIndex = 0;
            }
            else
            {
                groupBoxYear.Enabled = false;
                comboBoxYear.Items.Clear();
                comboBoxYear.Text = "";
            }
        }

        //以比例尺为主
        private void checkBoxScale_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBoxScale.Checked)
            {
                groupBoxScale.Enabled = true;
                string str_Exp = "select 描述,代码 from 比例尺代码表";
                GeoDataCenterDbFun dbfun = new GeoDataCenterDbFun();
               DataTable dt=dbfun.GetDataTableFromMdb(m_con,str_Exp);
               for (int i = 0; i < dt.Rows.Count;i++)
               {
                   comboBoxScale.Items.Add(dt.Rows[i][0]+"("+dt.Rows[i][1]+")");
               }
                if (comboBoxScale.Items.Count > 0)
                    comboBoxScale.SelectedIndex = 0;
            }

            else
            { 
                groupBoxScale.Enabled = false;
                
                comboBoxScale.Items.Clear();
                comboBoxScale.Text = "";
            }
        }

        //窗口加载
        private void frmAnalyseInLibMap_Load(object sender, EventArgs e)
        {
            checkBoxDelold.Checked = true;
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            m_con = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string strExp = "select 数据源名称 from 物理数据源表";
            List<string> list = db.GetDataReaderFromMdb(m_con, strExp);
            for (int i = 0; i < list.Count; i++)
            {
                comboBoxSource.Items.Add(list[i]);//加载数据源列表框
            }
            if (list.Count > 0)
            {
                comboBoxSource.SelectedIndex = 0;//默认选择第一个
            }
            //m_path = GetSourcePath(comboBoxSource.Text.Trim());

        }

        //得到数据源地址
        private string GetSourcePath(string str)
        {
            try
            {
                GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                string mypath = dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                string strExp = "select 数据库 from 物理数据源表 where 数据源名称='" + str + "'";
                GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                string strname = db.GetInfoFromMdbByExp(strCon, strExp);
                return strname;
            }
            catch { return ""; }
        }
        /// <summary>
        /// 得到数据库空间 Added by xisheng 2011.04.28
        /// </summary>
        /// <param name="str">数据源名称</param>
        /// <returns>工作空间</returns>
        private IWorkspace GetWorkspace(string str)
        {
            try
            {
                IWorkspace pws = null;
                GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                string mypath = dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                string strExp = "select * from 物理数据源表 where 数据源名称='" + str + "'";
                GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                DataTable dt = db.GetDataTableFromMdb(strCon, strExp);
                string type = dt.Rows[0]["数据源类型"].ToString();
                if (type.Trim() == "GDB")
                {
                    IWorkspaceFactory pWorkspaceFactory;
                    pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                    pws = pWorkspaceFactory.OpenFromFile(dt.Rows[0]["数据库"].ToString(), 0);
                }
                else if (type.Trim() == "SDE")
                {
                    IWorkspaceFactory pWorkspaceFactory;
                    pWorkspaceFactory = new SdeWorkspaceFactoryClass();

                    //PropertySet
                    IPropertySet pPropertySet;
                    pPropertySet = new PropertySetClass();
                    pPropertySet.SetProperty("Server", dt.Rows[0]["服务器"].ToString());
                    pPropertySet.SetProperty("Database", dt.Rows[0]["数据库"].ToString());
                    pPropertySet.SetProperty("Instance", "5151");//"port:" + txtService.Text
                    pPropertySet.SetProperty("user", dt.Rows[0]["用户"].ToString());
                    pPropertySet.SetProperty("password", dt.Rows[0]["密码"].ToString());
                    pPropertySet.SetProperty("version", "sde.DEFAULT");
                    pws = pWorkspaceFactory.Open(pPropertySet, 0);

                }
                return pws;
            }
            catch
            {
                return null;
            }
        }
 
        //开始分析
        private void btn_Analys_Click(object sender, EventArgs e)
        {
            SysCommon.CProgress vProgress = new SysCommon.CProgress("开始分析数据");
            vProgress.EnableCancel = false;
            vProgress.ShowDescription = true;
            vProgress.FakeProgress = true;
            vProgress.TopMost = true;
            vProgress.ShowProgress();
            try
            {
                string strArea = "";
                string strArea2 = "";
                List<string> strAreaChild = new List<string>();
                string strYear = "";
                string strScale = "";
                string strScale2 = "";
                string strExp = "";
                bool flat = false;//选了比例尺与否的状态
                int index = 0;
                int ifinish=0;//显示分析了多少条数据
                GeoDataCenterDbFun dbfun = new GeoDataCenterDbFun();
                m_list.Clear();

                //选择框的8种状态
                #region
                if (comboBoxScale.Text != "" && comboBoxYear.Text != "" && comboBoxArea.Text != "")
                    index = 1;
                else if (comboBoxArea.Text != "" && comboBoxYear.Text != "")
                    index = 2;
                else if (comboBoxArea.Text != "" && comboBoxScale.Text != "")
                    index = 3;
                else if (comboBoxYear.Text != "" && comboBoxScale.Text != "")
                    index = 4;
                else if (comboBoxArea.Text != "")
                    index = 5;
                else if (comboBoxYear.Text != "")
                    index = 6;
                else if (comboBoxScale.Text != "")
                    index = 7;
                else
                    index = 0;
                #endregion

                if (Node != null && comboBoxArea.Text!="")
                {
                    string[] arrr = comboBoxArea.Text.Split('(', ')');
                    strArea2 = arrr[1];
                    //得到所有该行政区的辖区
                    switch (Convert.ToInt32(Node.Tag))
                    {
                        case 1:
                            strArea = "行政代码 like '" + strArea2.Substring(0, 3).Trim() + "*' and ";
                            strExp = "select 行政代码 from 数据单元表 where 行政代码 like '" + strArea2.Substring(0, 3).Trim() + "*'";
                            break;
                        case 2:
                            strArea = "行政代码 like '" + strArea2.Substring(0, 4).Trim() + "*' and";
                            strExp = "select 行政代码 from 数据单元表 where 行政代码 like '" + strArea2.Substring(0, 4).Trim() + "*'";
                            break;
                        case 3:
                            strArea = "行政代码='" + strArea2 + "' and ";
                            strExp = "select 行政代码 from 数据单元表 where 行政代码 = '" + strArea2 + "*'";
                            break;
                    }
                    strAreaChild = dbfun.GetDataReaderFromMdb(m_con, strExp);

                }
                if (comboBoxYear.Text != "")
                {
                    strYear = "年度='" + comboBoxYear.Text + "' and ";
                }
                if (comboBoxScale.Text != "")
                {

                    string []arrr = comboBoxScale.Text.Split('(', ')');
                    strScale2 = arrr[1];
                    strScale = "比例尺='" + strScale2 + "' and ";
                    //flat = true;
                }
                //else
                //    flat = false;
                //if (flat)//选了比例尺，不用去掉and
                //    strExp = "select ID from 数据编码表 where " + strArea + strYear + strScale + " and 数据源名称='" + GetSourceName(m_path) + "'";
                //else if (comboBoxYear.Text != "" ||comboBoxArea.Text != "")//比例尺没有选择，而年度或者行政区划选了，要去掉and
                //{
                    //strExp = "select ID from 数据编码表 where " + strArea + strYear + "  数据源名称='" + GetSourceName(m_path) + "'";
                //}
                //else//都没有选择
                //    strExp = "select ID from 数据编码表 where  数据源名称='" + GetSourceName(m_path) + "'";

                strExp = "select ID from 数据编码表 where " + strArea + strYear + strScale + " 数据源名称='" +comboBoxSource.Text.Trim() + "'";  
                List<string> list = new List<string>();
                list = dbfun.GetDataReaderFromMdb(m_con, strExp);
              
               // m_path = GetSourcePath(comboBoxSource.Text);
                for (int i = 0; i < list.Count; i++)
                {
                    strExp = "delete * from 数据编码表 where ID=" + Convert.ToInt32(list[i]);
                    dbfun.ExcuteSqlFromMdb(m_con, strExp);//从数据编码表删除符合ID条件的行
                }
                m_list = new List<string>();
                IWorkspace pWorkspace = GetWorkspace(comboBoxSource.Text);
                //遍历数据库中数据并存在m_list列表中
                if (pWorkspace!=null)
                {
                   
                    IEnumDataset enumDataset = pWorkspace.get_Datasets(esriDatasetType.esriDTFeatureClass) as IEnumDataset;
                    IDataset dataset = enumDataset.Next();
                    //遍历mdb的每一个独立要素类
                    while (dataset != null)
                    {
                        IFeatureClass pFeatureClass = dataset as IFeatureClass;
                        m_list.Add(pFeatureClass.AliasName);
                        dataset = enumDataset.Next();
                    }
                }
                else
                {
                    vProgress.Close();
                    MessageBox.Show("数据源空间不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Activate();
                    return;

                }
                //从m_list中读取数据并分析
                string[] array = new string[6];
                bool boolarea = false;

                for (int i = 0; i < m_list.Count; i++)
                {
                    flat = false;
                    if (m_list[i].Contains("."))
                        m_list[i] = m_list[i].Substring(m_list[i].LastIndexOf(".")+1);
                    strExp = "select 字段名称 from 图层命名规则表";
                    string strname =dbfun.GetInfoFromMdbByExp(m_con, strExp);
                    string[] arrName = strname.Split('+');//分离字段名称
                    for (int ii = 0; ii < arrName.Length; ii++)
                    {
                        switch (arrName[ii])
                        {
                            case "业务大类代码":
                                array[0] = m_list[i].Substring(0, 2);//业务大类代码
                                m_list[i] = m_list[i].Remove(0, 2);
                                break;
                            case "年度":
                                array[1] = m_list[i].Substring(0, 4);//年度
                                m_list[i] = m_list[i].Remove(0, 4);
                                break;
                            case "业务小类代码":
                                array[2] = m_list[i].Substring(0, 2);//业务小类代码
                                m_list[i] = m_list[i].Remove(0, 2);
                                break;
                            case "行政代码":
                                array[3] = m_list[i].Substring(0, 6);//行政代码
                                m_list[i] = m_list[i].Remove(0, 6);
                                break;
                            case "比例尺":
                                array[4] = m_list[i].Substring(0, 1);//比例尺
                                m_list[i] = m_list[i].Remove(0, 1);
                                break;
                        }
                    }
                    array[5] = m_list[i];//图层组成
                    for (int j = 0; j < strAreaChild.Count; j++)//判断是否包含该行政区的辖区
                    {
                        if (strAreaChild[j] == array[3])
                        { 
                            boolarea = true; 
                            break; 
                        }
                    }
                    //判断情况条件是否符合
                    #region
                    switch (index)
                    {
                        case 0:
                            flat = true;
                            break;
                        case 1:
                            if (array[1] == comboBoxYear.Text && array[4] == strScale2 && boolarea)
                                flat = true;
                            break;
                        case 2:
                            if (boolarea && array[1] == comboBoxYear.Text)
                                flat = true;
                            break;
                        case 3:
                            if (boolarea && array[4] == strScale2)
                                flat = true;
                            break;
                        case 4:
                            if (array[1] == comboBoxYear.Text && array[4] == strScale2)
                                flat = true;
                            break;
                        case 5:
                            if (boolarea)
                                flat = true;
                            break;
                        case 6:
                            if (array[1] == comboBoxYear.Text)
                                flat = true;
                            break;
                        case 7:
                            if (array[4] == strScale2)
                                flat = true;
                            break;
                    }
                    #endregion

                    string sourecename = comboBoxSource.Text.Trim();
                    if (flat)
                    {
                        ifinish++;
                        
                        strExp = string.Format("insert into 数据编码表(业务大类代码,年度,业务小类代码,行政代码,比例尺,图层代码,数据源名称) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
                        array[0], array[1], array[2], array[3], array[4], array[5],sourecename);
                       
                        string strdata = GetLayerName(array[0], array[1], array[2], array[3], array[4]) + array[5];//组织数据
                        string logpath = Application.StartupPath + "\\..\\Log\\DataManagerLog.txt";
                        LogFile log = new LogFile(null,logpath);
                        string strLog = "开始分析数据源" +sourecename+"中"+ strdata+"数据";
                        if (log != null)
                        {
                            log.Writelog(strLog);
                        }
                        vProgress.SetProgress(strLog);
                        dbfun.ExcuteSqlFromMdb(m_con, strExp); //更新数据编码表
                        dbfun.UpdateMdbInfoTable(array[0], array[1], array[2], array[3], array[4]);//更新地图入库信息表
                    }
                  
                }
                ifinish = list.Count >= ifinish ? list.Count : ifinish;
                vProgress.Close();
                this.Activate();
                if (ifinish == 0)
                    MessageBox.Show("没有符合条件的数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("分析完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.Close();
            }
            //重新编号
            //strExp = "alter table 数据编码表 alter column ID counter(1,1)";
            // dbfun.ExcuteSqlFromMdb(m_con,strExp);
            catch (System.Exception ex)
            {
                vProgress.Close();
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        /// <summary>
        /// 组织前缀　added by xs 20110415
        /// </summary>
        /// <param name="str1">业务大类</param>
        /// <param name="str2">年度</param>
        /// <param name="str3">业务小类</param>
        /// <param name="str4">行政代码</param>
        /// <param name="str5">比例尺</param>
        /// <returns></returns>
        public string GetLayerName(string str1, string str2, string str3, string str4, string str5)
        {
            string layername = "";
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            string strExp = "select 字段名称 from 图层命名规则表";
            GeoDataCenterDbFun db = new GeoDataCenterDbFun();
            string strname = db.GetInfoFromMdbByExp(strCon, strExp);
            string[] arrName = strname.Split('+');//分离字段名称
            for (int i = 0; i < arrName.Length; i++)
            {
                switch (arrName[i])
                {
                    case "业务大类代码":
                        layername += str1;
                        break;
                    case "年度":
                        layername += str2;
                        break;
                    case "业务小类代码":
                        layername += str3;
                        break;
                    case "行政代码":
                        layername += str4;
                        break;
                    case "比例尺":
                        layername += str5;
                        break;
                }
            }
            return layername;
        }

        //得到数据源名称
        private string GetSourceName(string str)
        {
            try
            {
                GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                string mypath = dIndex.GetDbInfo();
                string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
                string strExp = "select 数据源名称 from 物理数据源表 where 数据库='" + str + "'";
                GeoDataCenterDbFun db = new GeoDataCenterDbFun();
                string strname = db.GetInfoFromMdbByExp(strCon, strExp);
                return strname;
            }
            catch
            {
                return str;
            }
        }
        //点击行政区域
        private void comboBoxArea_Click(object sender, EventArgs e)
        {
            GeoDataCenterDbFun dbfun = new GeoDataCenterDbFun();
            
            frmDataUnitTree frm = new frmDataUnitTree();//初始化数据单元树窗体
            frm.Location = new Point(this.Location.X + 147, this.Location.Y + 80);
            frm.flag=0;
            frm.ShowDialog();
            if (Node != null)//传回的Node不是NULL
            {
                if (Convert.ToInt32(Node.Tag) != 0)
                {
                    
                    string strExp = "select 行政代码 from 数据单元表 where 行政名称='" + Node.Text + "' and 数据单元级别='"+Node.Tag+"'";
                    string code = dbfun.GetInfoFromMdbByExp(m_con, strExp);
                    comboBoxArea.Text = Node.Text + "(" + code + ")";//为数据单元box显示数据
                }
            }
            
           
        }
    }
}