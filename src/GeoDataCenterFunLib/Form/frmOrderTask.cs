using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Xml;


using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;

//订单生成功能       ZQ    20110726   add
namespace GeoDataCenterFunLib
{
    public partial class frmOrderTask : DevComponents.DotNetBar.Office2007Form
    {
        //订单存储的数据库地址
        private string DDanPath = Application.StartupPath + "\\..\\OrderTask\\订单数据库.mdb";
        //打印订单模板
        private object wordPath = Application.StartupPath + "\\..\\OrderTask\\订单模板.doc";
        private string _layerTreePath = Application.StartupPath + "\\..\\res\\xml\\展示图层树.xml";
        private IMapControlDefault m_MapControl; //被查询的地图对象 
        private IWorkspace m_WS = null;        //系统维护库连接工作空间
        public frmOrderTask(IMapControlDefault pMapControl, IWorkspace pWs)
        {
            InitializeComponent();
            m_MapControl = pMapControl;
            m_WS = pWs;
            initialization();
        }
        #region 界面初始化    ZQ   20110726
        private void initialization()
        {
            object[] TagDBType = new object[] { "未分级", "内部", "秘密", "机密", "绝密" };
            cmboxMJ.Items.AddRange(TagDBType);
            cmboxMJ.SelectedIndex = 3;
            object[] TagZTType = new object[]{"未批准","已批准"};
            cmbZT.Items.AddRange(TagZTType);
            cmbZT.SelectedIndex = 0;
            dateTimeZB.Value = DateTime.Now.Date;
          
        }
        //ZQ   20110801   modify
        public frmOrderTask(IMapControlDefault pMapControl, IWorkspace pWs,bool bDLG,bool bDEM,bool bDOM,string strSize,string strBH,string strMapNo)
        {
            InitializeComponent();
            m_MapControl = pMapControl;
            m_WS = pWs;
            initialization();
            chkBoxDLG.Checked = bDLG;
            chkBoxDEM.Checked = bDEM;
            chkBoxDOM.Checked = bDOM;
       
            
        }
        public frmOrderTask(IMapControlDefault pMapControl, string TileName, List<string> strValue, IWorkspace pWs)
        {
            InitializeComponent();
            try
            {
                m_MapControl = pMapControl;
                m_WS = pWs;
                this.Text = TileName;
                button1.Visible = true;
                bttRL.Visible = false;
                txtBH.Enabled = false;
                dateTimeZB.Enabled = false;
                object[] TagDBType = new object[] { "未分级", "内部", "秘密", "机密", "绝密" };
                cmboxMJ.Items.AddRange(TagDBType);
                object[] TagZTType = new object[] { "未批准", "已批准" };
                cmbZT.Items.AddRange(TagZTType);
                txtBH.Text = strValue[0]; dateTimeZB.Text = strValue[1]; txtRN.Text = strValue[2]; txtYJ.Text = strValue[3];
                txtDW.Text = strValue[4]; txtBM.Text = strValue[5]; txtXY.Text = strValue[6]; txtSM.Text = strValue[7];
                txtFW.Text = strValue[8]; txtYT.Text = strValue[9]; txtDX.Text = strValue[10]; cmboxMJ.SelectedItem = strValue[11];
                txt.Text = strValue[12]; txtPN.Text = strValue[13];txtGS.Text= strValue[14]; txtSL.Text = strValue[15];
                txtTFS.Text = strValue[16]; txtFY.Text = strValue[17]; txtYHQR.Text = strValue[18];
                if (strValue[19] == "1899-12-30 0:00:00"||strValue[19] == "1899/12/30 0:00:00") dateTimeQR.LockUpdateChecked = false;
                else
                {
                    dateTimeQR.Text = strValue[19];
                }
                txtYHJS.Text = strValue[20];
                if (strValue[21] == "1899-12-30 0:00:00" || strValue[21] == "1899/12/30 0:00:00") dateTimeJS.LockUpdateChecked = false;
                else
                {
                    dateTimeJS.Text = strValue[21];
                }
                txtLXR.Text = strValue[22]; txtDH.Text = strValue[23];
                txtYZBM.Text = strValue[24]; txtEmail.Text = strValue[25]; txtDZ.Text = strValue[26]; txtBZ.Text = strValue[27];
                txtFZR.Text = strValue[28];
                if (strValue[29] == "1899-12-30 0:00:00" || strValue[29] == "1899/12/30 0:00:00")
                {
                    dateTimeBM.LockUpdateChecked = false;
                }
                else
                {
                    dateTimeBM.Text = strValue[29];
                } txtJGR.Text = strValue[30];
                if (strValue[31] == "1899-12-30 0:00:00" || strValue[31] == "1899/12/30 0:00:00")
                {
                    dateTimeJSJG.LockUpdateChecked = false;
                }
                else
                {
                    dateTimeJSJG.Text = strValue[31];
                }
                txtJCR.Text = strValue[32];
                if (strValue[33] == "1899-12-30 0:00:00" || strValue[33] == "1899/12/30 0:00:00")
                {
                    dateTimeJCR.LockUpdateChecked = false;

                }
                else
                { dateTimeJCR.Text = strValue[33]; } txtJSR.Text = strValue[34];
                if (strValue[35] == "1899-12-30 0:00:00" || strValue[35] == "1899/12/30 0:00:00")
                {
                    dateTimeJSR.LockUpdateChecked = false;
                }
                else { dateTimeJSR.Text = strValue[35]; } if (strValue[36] == "") cmbZT.SelectedIndex = 0;
                else
                {

                    cmbZT.SelectedItem = strValue[36];
                } txtZBR.Text = strValue[37];
                if(strValue[38]!=""&&strValue[38].ToUpper()=="TRUE")
                {
                    chkBoxDLG.Checked = true;
                }
                if (strValue[39] != "" && strValue[39].ToUpper() == "TRUE")
                {
                    chkBoxDEM.Checked = true;
                }
                if (strValue[40] != "" && strValue[40].ToUpper() == "TRUE")
                {
                    chkBoxDOM.Checked = true;
                }
                if (strValue[41] == "") return;
                else
                {
                    string[] strMapNo = strValue[41].Split(new char[] { ',' });
                    if (strMapNo.Length == 0) return;
                    for (int i = 0; i < strMapNo.Length; i++)
                    {
                        checkedBoxMapNO.Items.Add(strMapNo[i], true);
                    }
                }
                

            }
            catch
            {
            }
        }
        #endregion 
        #region 数据入库    ZQ   20110726
        private void bttRL_Click(object sender, EventArgs e)
        {
            try
            {
                errorProvider.Clear();
                if (txtBH.Text == "" )
                {
                    errorProvider.SetError(txtBH, "编号信息不能为空！");
                    return;
                }
                if (txtZBR.Text == "")
                {
                    errorProvider.SetError(txtZBR, "制表人信息不能为空！");
                    return;
                }
                if (GetMapNo() == "")
                {
                    errorProvider.SetError(txtMapNo, "图幅号信息不能为空！");
                    return;
                }
                if (dateTimeZB.Value.ToString() == "")
                {
                    errorProvider.SetError(dateTimeZB, "制表日期信息不能为空！");
                    return;
                }
                OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DDanPath);

                OleDbCommand pOleDbCommand = con.CreateCommand();
                pOleDbCommand.CommandText = "select * from 订单表";
                try
                {
                    con.Open();
                }
                catch { MessageBox.Show("数据库连接失败！", "提示！"); }
                OleDbDataAdapter pOleDbDataAdapter = new OleDbDataAdapter(pOleDbCommand.CommandText, con);
                OleDbCommandBuilder myBuilder = new OleDbCommandBuilder(pOleDbDataAdapter);
                DataSet pDataSet = new DataSet();
                pOleDbDataAdapter.Fill(pDataSet, "订单表");
                DataTable pDataTable = pDataSet.Tables["订单表"];
                DataRow pDataRow = pDataTable.NewRow();
                //pDataRow["ID"] = 1;
                pDataRow = SetRow(pDataRow);
                pDataTable.Rows.Add(pDataRow);
                pOleDbDataAdapter.InsertCommand = myBuilder.GetInsertCommand();
                string insertSQL = pOleDbDataAdapter.InsertCommand.CommandText;
                try
                {
                    pOleDbDataAdapter.Update(pDataSet, "订单表");
                }
                catch
                {
                    pOleDbCommand = new OleDbCommand(insertSQL, con);
                    pOleDbDataAdapter = new OleDbDataAdapter(pOleDbCommand);
                    pOleDbDataAdapter.Update(pDataSet, "订单表");
                }
                pDataSet.AcceptChanges();

                con.Close();
                MessageBox.Show("入录完成！", "提示！");
            }
            catch
            {
                MessageBox.Show("请查看该编号信息是否已经存在！","提示！");
            }
        }

        private DataRow SetRow(DataRow pDataRow)
        {
            pDataRow["编号"] = txtBH.Text;
            pDataRow["制表日期"] = dateTimeZB.Value;
            pDataRow["数据内容"] = txtRN.Text;
            pDataRow["提供依据"] = txtYJ.Text;
            pDataRow["索取单位"] = txtDW.Text;
            pDataRow["处理部门"] = txtBM.Text;
            pDataRow["协议编号"] = txtXY.Text;
            pDataRow["特殊说明"] = txtSM.Text;
            pDataRow["数据范围"] = txtFW.Text;
            pDataRow["主要用途"] = txtYT.Text;
            pDataRow["国地信"] = txtDX.Text;
            pDataRow["密级"] = cmboxMJ.Text;
            pDataRow["SN"] = txt.Text;
            pDataRow["PN"] = txtPN.Text;
            pDataRow["格式"] = txtGS.Text;
            pDataRow["数据量"] = txtSL.Text;
            //获取涉及的图幅号
            string strMapNO = GetMapNo();
            pDataRow["图幅号"] = strMapNO.ToString();
            pDataRow["涉及图幅数"] = txtTFS.Text;
            pDataRow["技术服务费用"] = txtFY.Text;
            pDataRow["用户确认签字"] = txtYHQR.Text;
            pDataRow["用户确认签字日期"] = dateTimeQR.Value;
            pDataRow["用户接收签字"] = txtJSR.Text;
            pDataRow["用户接收签字日期"] = dateTimeJS.Value;
            pDataRow["联系人"] = txtLXR.Text;
            pDataRow["联系电话"] = txtDH.Text;
            pDataRow["邮政编码"] = txtYZBM.Text;
            pDataRow["Email"] = txtEmail.Text;
            pDataRow["联系地址"] = txtDZ.Text;
            pDataRow["备注"] = txtBZ.Text;
            pDataRow["部门负责人"] = txtFZR.Text;
            pDataRow["部门负责人签字日期"] = dateTimeBM.Value;
            pDataRow["技术加工人"] = txtJGR.Text;
            pDataRow["技术加工人签字日期"] = dateTimeJSJG.Value;
            pDataRow["成果检查人"] = txtJCR.Text;
            pDataRow["成果检查人签字日期"] = dateTimeJCR.Value;
            pDataRow["成果接收人"] = txtJSR.Text;
            pDataRow["成果接收人签字日期"] = dateTimeJSR.Value;
            pDataRow["订单状态"] = cmbZT.Text;
            pDataRow["制表人"] = txtZBR.Text;
            pDataRow["DLG"] = chkBoxDLG.Checked.ToString();
            pDataRow["DEM"] = chkBoxDEM.Checked.ToString();
            pDataRow["DOM"] = chkBoxDOM.Checked.ToString();
            return pDataRow;
        }
        #endregion

        private void bttQX_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #region   涉及图幅号操作
        private void txtMapNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtMapNo.Text.Length != 10)
                {
                    MessageBox.Show("格式不正确", "提示！");
                    return;
                }
                txtTFS.Text = "";
                //此处未处理存在重复的数据
                checkedBoxMapNO.Items.Add(txtMapNo.Text, true);
                int pCount=0;
                for(int i = 0;i<checkedBoxMapNO.Items.Count;i++)
                {
                    if(checkedBoxMapNO.GetItemChecked(i))
                    {
                        pCount++;
                    }
                }
                txtTFS.Text = pCount.ToString() + "幅";
                txtMapNo.Text = "";
            }

        }    
        private void bttclear_Click(object sender, EventArgs e)
        {
            checkedBoxMapNO.Items.Clear();
        }
        /// <summary>
        /// 从checkedBox中获得图幅号字符串
        /// </summary>
        /// <returns></returns>
        private string GetMapNo()
        {
            //获取涉及的图幅号
            string strMapNO ="";
            if (checkedBoxMapNO.Items.Count > 0)
            {
                for (int i = 0; i < checkedBoxMapNO.Items.Count; i++)
                {
                    if (!checkedBoxMapNO.GetItemChecked(i))
                    {
                        continue;
                    }
                    if (strMapNO == "")
                    {
                        strMapNO = checkedBoxMapNO.Items[i].ToString();
                    }
                    else
                    {
                        strMapNO = strMapNO + "," + checkedBoxMapNO.Items[i].ToString();
                    }
                }
            }
            return strMapNO;
        }
    
    
            
      
        #endregion
        #region 通过worde打印功能   ZQ   20110726
        private void bttDY_Click(object sender, EventArgs e)
        {
            try
            {
                if (!System.IO.File.Exists(wordPath.ToString())) return;
                List<string> newValue = GettxtValue();
                //word应用
                Microsoft.Office.Interop.Word.ApplicationClass App = new Microsoft.Office.Interop.Word.ApplicationClass(); //Word应用程序对象
                Microsoft.Office.Interop.Word.Document doc = new Microsoft.Office.Interop.Word.Document();             //Word文档对象
                App.Visible = false;
                String[] bmsTest = { "biaohao", "zhibiaoriqi", "shujuneirong", "tigongdanwei", "suoqudanwei", "chulibumen", "xieyibianhao", "teshushuiming", "shujufanwei", "zhuyaoyongtu", "guodixin", "miji", "txtsn", "txtpn", "geshi", "shijitufushu", "jishufuwufeiyong", "shujuliang", "bumenfuzeren", "bumenqianzi", "jishujiagong", "txtjishujiang", "chenggujianchare", 
                                   "qianziriqi", "jieshouren", "aaaaa", "querenqian","bbbbb","ddddd","ccccc","lianxiren","dianhua","youzheng","emaildizhi","fffff","beizhutxt","zhibiaorenyuan", };
                object oMissing = System.Reflection.Missing.Value;
                object readOnly = false;
                object saveFileName = Application.StartupPath + "\\..\\OrderTask\\teste.doc";

                doc = App.Documents.Open(ref wordPath, ref oMissing, ref readOnly,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                doc.Activate();
                int m = 0;
                //替换
                foreach (String s in bmsTest)
                {
                    Replace(doc, s, newValue[m]);
                    m++;
                }

                doc.SaveAs(ref saveFileName, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                       ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                doc.Close(ref oMissing, ref oMissing, ref oMissing);
                App.Quit(ref oMissing, ref oMissing, ref oMissing);

                //打开
                if (System.IO.File.Exists(saveFileName.ToString()))
                {
                    System.Diagnostics.Process.Start(saveFileName.ToString());
                }
                this.WindowState = FormWindowState.Minimized;
            }
            catch
            {
            }
        }
        private List<string> GettxtValue()
        {
            
            List<string> txtValue = new List<string>();
            txtValue.Add(txtBH.Text); txtValue.Add(dateTimeZB.Value.ToShortDateString()); txtValue.Add(txtRN.Text); 
            txtValue.Add(txtYJ.Text); txtValue.Add(txtDW.Text); txtValue.Add(txtBM.Text); txtValue.Add(txtXY.Text);
            txtValue.Add(txtSM.Text); txtValue.Add(txtFW.Text); txtValue.Add(txtYT.Text); txtValue.Add(txtDX.Text); 
            txtValue.Add(cmboxMJ.Text); txtValue.Add(txt.Text); txtValue.Add(txtPN.Text);
            txtValue.Add(txtGS.Text); txtValue.Add(txtTFS.Text); txtValue.Add(txtFY.Text); txtValue.Add(txtSL.Text); 
            txtValue.Add(txtFZR.Text); txtValue.Add(dateTimeBM.Text); txtValue.Add(txtJGR.Text);
            txtValue.Add(dateTimeJSJG.Text); txtValue.Add(txtJCR.Text); txtValue.Add(dateTimeJCR.Text);
            txtValue.Add(txtJSR.Text); txtValue.Add(dateTimeJSR.Text); txtValue.Add(txtYHQR.Text); txtValue.Add(dateTimeQR.Text);
            txtValue.Add(txtYHJS.Text); txtValue.Add(dateTimeJS.Text); txtValue.Add(txtLXR.Text); txtValue.Add(txtDH.Text); txtValue.Add(txtYZBM.Text);
            txtValue.Add(txtEmail.Text); txtValue.Add(txtDZ.Text); txtValue.Add(txtBZ.Text); txtValue.Add(txtZBR.Text);
            return txtValue;

        }
        ///<summary>
        /// 在word 中查找一个字符串直接替换所需要的文本
        /// </summary>
        /// <param name="strOldText">原文本</param>
        /// <param name="strNewText">新文本</param>
        /// <returns></returns>
        public bool Replace(Microsoft.Office.Interop.Word.Document oDoc, string strOldText, string strNewText)
        {
            oDoc.Content.Find.Text = strOldText;
            object FindText, ReplaceWith, Replace;// 
            object MissingValue = Type.Missing;
            FindText = strOldText;//要查找的文本
            ReplaceWith = strNewText;//替换文本
            Replace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;/*wdReplaceAll - 替换找到的所有项。
                                                      * wdReplaceNone - 不替换找到的任何项。
                                                    * wdReplaceOne - 替换找到的第一项。
                                                    * */
            oDoc.Content.Find.ClearFormatting();//移除Find的搜索文本和段落格式设置
            if (oDoc.Content.Find.Execute(
                ref FindText, ref MissingValue,
                ref MissingValue, ref MissingValue,
                ref MissingValue, ref MissingValue,
                ref MissingValue, ref MissingValue, ref MissingValue,
                ref ReplaceWith, ref Replace,
                ref MissingValue, ref MissingValue,
                ref MissingValue, ref MissingValue))
            {
                return true;
            }
            return false;

        }

        public bool SearchReplace(Microsoft.Office.Interop.Word.Application oWordApplic, string strOldText, string strNewText)
        {
            object replaceAll = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;
            object missing = Type.Missing;

            //首先清除任何现有的格式设置选项，然后设置搜索字符串 strOldText。
            oWordApplic.Selection.Find.ClearFormatting();
            oWordApplic.Selection.Find.Text = strOldText;

            oWordApplic.Selection.Find.Replacement.ClearFormatting();
            oWordApplic.Selection.Find.Replacement.Text = strNewText;

            if (oWordApplic.Selection.Find.Execute(
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing,
                ref replaceAll, ref missing, ref missing, ref missing, ref missing))
            {
                return true;
            }
            return false;
        }
        #endregion
        #region ZQ 20110727    add  当前视图   根据图幅号定位到当前视图
        //根据连接字符串获取工作空间
        //此处连接字符串是固定格式的连接串 Server|Service|Database|User|Password|Version
        private IWorkspace GetWorkSpacefromConninfo(string conninfostr, int type)
        {
            int index1 = conninfostr.IndexOf("|");
            int index2 = conninfostr.IndexOf("|", index1 + 1);
            int index3 = conninfostr.IndexOf("|", index2 + 1);
            int index4 = conninfostr.IndexOf("|", index3 + 1);
            int index5 = conninfostr.IndexOf("|", index4 + 1);
            int index6 = conninfostr.IndexOf("|", index5 + 1);
            IPropertySet pPropSet = new PropertySetClass();
            IWorkspaceFactory pWSFact = null;
            string sServer = ""; string sService = ""; string sDatabase = "";
            string sUser = ""; string sPassword = ""; string strVersion = "";
            switch (type)
            {
                case 1://mdb
                    pWSFact = new AccessWorkspaceFactoryClass();
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    break;
                case 2://gdb
                    pWSFact = new FileGDBWorkspaceFactoryClass();
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    break;
                case 3://sde
                    pWSFact = new SdeWorkspaceFactoryClass();
                    sServer = conninfostr.Substring(0, index1);
                    sService = conninfostr.Substring(index1 + 1, index2 - index1 - 1);
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    sUser = conninfostr.Substring(index3 + 1, index4 - index3 - 1);
                    sPassword = conninfostr.Substring(index4 + 1, index5 - index4 - 1);
                    strVersion = conninfostr.Substring(index5 + 1, index6 - index5 - 1);
                    break;
            }

            pPropSet.SetProperty("SERVER", sServer);
            pPropSet.SetProperty("INSTANCE", sService);
            pPropSet.SetProperty("DATABASE", sDatabase);
            pPropSet.SetProperty("USER", sUser);
            pPropSet.SetProperty("PASSWORD", sPassword);
            pPropSet.SetProperty("VERSION", strVersion);
            try
            {

                IWorkspace pWorkspace = pWSFact.Open(pPropSet, 0);
                return pWorkspace;
            }
            catch
            {
                return null;
            }
        }
        private void bttCK_Click(object sender, EventArgs e)
        {
            try
            {
                string[] strMapNo = GetMapNo().Split(new char[] { ',' });
                IList<IFeature> pListFeature = new List<IFeature>();
                if (strMapNo == null) return;
                Exception eError;
                string NodeKey = "";
                if (strMapNo[0].Length == 3)//1:100万 接图表NodeKey
                {
                    NodeKey = "61073218-927f-4eba-a245-e6e59a121a5e";
                }
                else
                {
                    switch (strMapNo[0].ToString().Substring(3, 1).ToUpper())
                    {
                        case "C"://1:25万 接图表NodeKey
                            NodeKey = "610we218-927f-4eba-a245-e6e59a121a5e";
                            break;
                        case "E"://1:5万 接图表NodeKey
                            NodeKey = "c113ac32-14ce-44f6-83b2-c5e0322ef8f9";//不同比例尺的接图表的ID号
                            break;
                    }
                }
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(_layerTreePath);
                SysCommon.Gis.SysGisTable sysTable = new SysCommon.Gis.SysGisTable(m_WS);
                string strSearch = "//Layer[@NodeKey='" + NodeKey.ToString() + "']";
                XmlNode pxmlnode = xmldoc.SelectSingleNode(strSearch);
                XmlElement pxmlele = pxmlnode as XmlElement;
                if (pxmlnode == null)
                    return;
                string strDataSourceID = pxmlele.GetAttribute("ConnectKey");
                string feaclscode = pxmlele.GetAttribute("Code");
                object objDataSource = sysTable.GetFieldValue("DATABASEMD", "DATABASENAME", "ID=" + strDataSourceID, out eError);
                string DataSourcename = "";
                if (objDataSource != null) DataSourcename = objDataSource.ToString();
                string conninfostr = "";
                int type = -1;
                object objconnstr = sysTable.GetFieldValue("DATABASEMD", "CONNECTIONINFO", "ID=" + strDataSourceID, out eError);
                object objType = sysTable.GetFieldValue("DATABASEMD", "DATAFORMATID", "DATABASENAME='" + DataSourcename + "'", out eError);
                if (objconnstr != null)
                    conninfostr = objconnstr.ToString();
                if (objType != null)
                    type = int.Parse(objType.ToString());
                //连接并获得目录数据库
                IWorkspace pWorkspace = GetWorkSpacefromConninfo(conninfostr, type);
                if (pWorkspace == null) return;
                IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
                IFeatureClass pFeatureClass = pFeatureWorkspace.OpenFeatureClass(feaclscode);
                if (pFeatureClass == null) { return; }
                for(int i=0;i<strMapNo.Length;i++)
                {
                    IQueryFilter pQueryFilter = new QueryFilterClass();
                    IFeature pFeature = null;
                    IFeatureCursor pFeatureCursor = null;
                    pQueryFilter.WhereClause = "MAP='" + strMapNo[i]+"'";
                    pFeatureCursor = pFeatureClass.Search(pQueryFilter,false);
                    pFeature = pFeatureCursor.NextFeature();
                    while (pFeature!=null)
                    {
                       pListFeature.Add(pFeature);
                       pFeature= pFeatureCursor.NextFeature();
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);
                }
              
                 IGeometry pGeometry =GetLyrUnionPlygon(pListFeature);
                 if (pGeometry == null) { MessageBox.Show("不存在相关图幅信息，请认真核对图幅信息！", "提示！"); return; }
                 this.WindowState = FormWindowState.Minimized; 
                 m_MapControl.Extent =pGeometry.Envelope;
                 m_MapControl.ActiveView.Refresh();
            }
            catch { }
         

        }
        /// <summary>
        /// 通过图名获取图层
        /// </summary>
        /// <param name="LayerName"></param>
        /// <returns></returns>
        private ILayer GetLayerByName(string LayerName)
        {
            ILayer pLayer =null;
            if (m_MapControl.LayerCount > 0)
            {
                for (int i = 0; i < m_MapControl.LayerCount; i++)
                {
                    pLayer = m_MapControl.get_Layer(i);
                    if (pLayer.Valid && pLayer.Name == LayerName)
                    {
                        return pLayer;
                    }
                }
            }
         
            return pLayer = null;
        }
        /// <summary>
        /// 获得指定图层的合并范围 为本次加的一个函数
        /// </summary>
        /// <param name="strLyrName"></param>
        /// <param name="strWhere"></param>
        /// <param name="eFeatureType"></param>
        /// <param name="eGeometryType"></param>
        /// <returns></returns>
        public IGeometry GetLyrUnionPlygon(IList<IFeature> vFeaList)
        {
           
            if (vFeaList.Count < 1) return null;
            //构造
            IGeometryBag pGeometryBag = new GeometryBagClass();
            IGeometryCollection pGeometryCol = pGeometryBag as IGeometryCollection;

            object obj = System.Type.Missing;
            //获得所有图形
            for (int i = 0; i < vFeaList.Count; i++)
            {
                if (vFeaList[i].Shape != null && !vFeaList[i].Shape.IsEmpty) pGeometryCol.AddGeometry(vFeaList[i].ShapeCopy, ref obj, ref obj);
            }

            //构造合并
            ITopologicalOperator pTopo = new PolygonClass();
            pTopo.ConstructUnion(pGeometryCol as IEnumGeometry);

            IGeometry pGeometry = pTopo as IGeometry;
            return pGeometry;
        }
        #endregion
        //输出图幅号数据到指定位置    ZQ  20110727    add
        private void bttExport_Click(object sender, EventArgs e)
        {
            string[] strMapNo = GetMapNo().Split(new char[] { ',' });
            List<string> LstMapNo = new List<string>();
            foreach(string MapNo in strMapNo)
            {
                LstMapNo.Add(MapNo);
            }
    
            this.WindowState = FormWindowState.Minimized;
            GeoUtilities.Gis.Form.frmExportDataByMapNO pfrmExportDataByMapNO = new GeoUtilities.Gis.Form.frmExportDataByMapNO(LstMapNo, false, chkBoxDLG.Checked, chkBoxDEM.Checked, chkBoxDOM.Checked, m_MapControl, m_WS);
            pfrmExportDataByMapNO.ShowDialog();
        }
        #region   更新数据    ZQ   20110728   add
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                errorProvider.Clear();
                if (txtBH.Text == "")
                {
                    errorProvider.SetError(txtBH, "编号信息不能为空！");
                    return;
                }
                if (txtZBR.Text == "")
                {
                    errorProvider.SetError(txtZBR, "制表人信息不能为空！");
                    return;
                }
                if (GetMapNo() == "")
                {
                    errorProvider.SetError(txtMapNo, "图幅号信息不能为空！");
                    return;
                }
                if (dateTimeZB.Value.ToString() == "")
                {
                    errorProvider.SetError(dateTimeZB, "制表日期信息不能为空！");
                    return;
                }
                OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DDanPath);

                OleDbCommand pOleDbCommand = con.CreateCommand();
                pOleDbCommand.CommandText = "select * from 订单表 where 编号='"+txtBH.Text+"'";
                try
                {
                    con.Open();
                }
                catch { MessageBox.Show("数据库连接失败！", "提示！"); }
                OleDbDataAdapter pOleDbDataAdapter = new OleDbDataAdapter(pOleDbCommand.CommandText, con);
                OleDbCommandBuilder myBuilder = new OleDbCommandBuilder(pOleDbDataAdapter);
                DataSet pDataSet = new DataSet();
                pOleDbDataAdapter.Fill(pDataSet, "订单表");
                DataTable pDataTable = pDataSet.Tables["订单表"];
                DataRow pDataRow = pDataTable.Rows[0];
                pDataRow = SetRow(pDataRow);
                pOleDbDataAdapter.Update(pDataSet, "订单表");
                pDataSet.AcceptChanges();
                con.Close();
                MessageBox.Show("更新完成！", "提示！");
            }
            catch
            {
                
            }
        }
        #endregion

    }
}
