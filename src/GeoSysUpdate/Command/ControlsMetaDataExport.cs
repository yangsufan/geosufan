using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Data.OracleClient;
using System.IO;

namespace GeoSysUpdate
{
   public class ControlsMetaDataExport:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        public ControlsMetaDataExport()
        {
            base._Name = "GeoSysUpdate.ControlsMetaDataExport";
            base._Caption = "元数据导出";
            base._Tooltip = "元数据导出";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "元数据导出";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
                return true;
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            try
            {
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("执行元数据导出操作");
                }
                ///存储元数据对应关系表
                string strFile = Application.StartupPath + "\\..\\MDxmlData\\Res\\metachange.xml";
                TreeNode pSelNode = ModData.v_AppGisUpdate.MetadataTree.SelectedNode;///获取元数据查询结果树目录
                string strXMLFile = pSelNode.Text;
                ///链接oracle数据库并查询相关数据
                string sqlStr = "select t.metadataxml.getclobval() metadataxml from metadata_xml t WHERE 图幅号='" + strXMLFile + "'AND 数据类型='" + pSelNode.Parent.Parent.Text + "'";
                string pOracleConn = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + Plugin.Mod.Server + ") (PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=" + Plugin.Mod.Database + ")));Persist Security Info=True;User Id=" + Plugin.Mod.User + "; Password=" + Plugin.Mod.Password + "";
                OracleConnection con = new OracleConnection();
                DataTable dt = new DataTable();
                con.ConnectionString = pOracleConn;
                if (con.State == ConnectionState.Closed)
                {
                    try
                    {
                        con.Open();
                    }
                    catch
                    {
                        MessageBox.Show("数据库连接失败！", "提示！");
                        if (this.WriteLog)
                        {
                            Plugin.LogTable.Writelog("数据库连接失败，元数据导出失败");
                        }
                    }
                }
                OracleDataAdapter da = new OracleDataAdapter(sqlStr, con);
                da.Fill(dt);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                XmlDocument pXmlDocument = new XmlDocument();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    pXmlDocument.InnerXml = dt.Rows[i]["metadataxml"].ToString();
                }
                //DeleteFolder(Application.StartupPath + "\\..\\MDxmlData\\XMLData\\");
                //string strXMLPath = Application.StartupPath + "\\..\\MDxmlData\\XMLData\\" + pSelNode.Parent.Parent.Text + "_" + strXMLFile + ".xml";
                //pXmlDocument.Save(strXMLPath);
                string strPath = GetSaveFilePath(strXMLFile);
                if(strPath=="")
                {
                    return;
                }
                if (System.IO.File.Exists(strPath))
                {
                    try
                    {
                        System.IO.File.Delete(strPath);
                    }
                    catch { }
                }
                System.Reflection.Missing miss = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Excel.Application myExcel = new Microsoft.Office.Interop.Excel.Application();
                if (myExcel == null)
                {
                    MessageBox.Show("无法创建Excel对象，可能您的机器未安装Excel");
                    if (this.WriteLog)
                    {
                        Plugin.LogTable.Writelog("无法创建Excel对象，可能您的机器未安装Excel。元数据导出失败");
                    }
                    return;
                }

                Microsoft.Office.Interop.Excel._Workbook xBk;//工作薄   
                Microsoft.Office.Interop.Excel._Worksheet xSt;//工作Sheet  

                xBk = myExcel.Workbooks.Add(true);
                xSt = (Microsoft.Office.Interop.Excel._Worksheet)xBk.ActiveSheet;
                ///工作Sheet名称设为元数据图幅号
                xSt.Name = strXMLFile;
                ///用于存储已经添加的字段名
                List<string> LstFieldName = new List<string>();
                //读取对照关系
                XmlDocument vXmlDoc = new XmlDocument();
                string strTableName =pSelNode.Parent.Parent.Text.ToString();
                vXmlDoc.Load(strFile);
                XmlNode vCurNode = vXmlDoc.SelectSingleNode("/zjMetadata/table[@name='" + strTableName.ToLower() + "']");
                //***模板文件****
                XmlNode vModelXMLNode = vCurNode.FirstChild;
                ///读取导出的元数据xml
                XmlNamespaceManager vXMLNSM = new XmlNamespaceManager(pXmlDocument.NameTable);
                vXMLNSM.AddNamespace("smmd", "http://data.sbsm.gov.cn/smmd/2007");
                int j = 1;
                foreach (XmlNode vSubItemNode in vModelXMLNode.ChildNodes)//都取metachange.xml中item项中的DLG、DOM、DEM等值来选取模板
                {
                    XmlNode vXmlNodeComparison = vSubItemNode.ParentNode.ParentNode.LastChild;
                    foreach (XmlNode vSubNode in vXmlNodeComparison.ChildNodes)
                    {
                        try
                        {
                            ///获取元数据对应关系表中项名代表：元数据Xml的路径
                            string strXMLNode = vSubNode.Attributes["name"].Value;
                            ///对应的字段名
                            string strFieldName = vSubNode.Attributes["fieldname"].Value;
                            if (strFieldName != ""&&!IsValueExists(LstFieldName, strFieldName))
                            {
                                XmlNode pXmlNodeExport = pXmlDocument.SelectSingleNode(strXMLNode, vXMLNSM);
                                string strNodeName = pXmlNodeExport.Name.ToString();
                                string strNodeText = pXmlNodeExport.InnerText.ToString();
                                ///处理路径相同的元数据项的问题
                                if (strNodeName == "smmd:refDate" || strNodeName == "smmd:rpOrgName")
                                {
                                    strNodeText = GetSamePathValue(ref strNodeText, pXmlNodeExport, vSubNode);
                                }
                                ///根据查找的值插入到excel中
                                myExcel.Cells[j, 1] = strFieldName;
                                myExcel.Cells[j, 2] = strNodeText;
                                LstFieldName.Add(strFieldName);
                                j++;
                            }
                        }
                        catch { }
                    }
                }
                myExcel.Visible = false;
                ///保存到本地
                xBk.SaveAs(strPath, miss, miss,
                miss, miss, miss, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared,
                miss, miss, miss, miss, miss);
                myExcel.Quit();
                ///释放资源
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xBk);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(myExcel);
                GC.Collect();
                MessageBox.Show("数据导出成功！","提示！");
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("元数据导出操作完成");
                }
            }
            catch
            {
                 MessageBox.Show("数据导出失败！","提示！");
                 if (this.WriteLog)
                 {
                     Plugin.LogTable.Writelog("元数据导出失败");
                 }
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
       /// <summary>
       /// 获得相同路径下不同的值
       /// </summary>
       /// <param name="strNodeText"></param>
       /// <param name="pXmlNodeExport"></param>
       /// <returns></returns>
        private string GetSamePathValue(ref string strNodeText, XmlNode pXmlNodeExport, XmlNode vSubNode)
        {
            try
            {
                ///当type值与其下一个节点的值相同则获取该节点的值
                if (vSubNode.Attributes["type"].Value.ToString() == pXmlNodeExport.NextSibling.InnerText)
                {
                    return strNodeText;
                }
                ///不同则遍历下一个节点直到获取到type值与其下一个节点的值相同的节点
                else 
                {
                    strNodeText = SamePathVale(ref strNodeText, pXmlNodeExport,vSubNode);
                    return strNodeText;
                }

            }
            catch
            {
                return strNodeText;
            }
        }
       /// <summary>
       /// 递归循环获取
       /// </summary>
       /// <param name="strNodeText"></param>
       /// <param name="vModelSubNode"></param>
       /// <param name="vSubNode"></param>
       /// <returns></returns>
        private string SamePathVale(ref string strNodeText,XmlNode vModelSubNode , XmlNode vSubNode)
        {

            try
            {
                 ///获取当前节点父节点的下一个节点
                XmlNode pOneXmlNode = vModelSubNode.ParentNode.NextSibling;
                XmlNodeList pOneXmlNodeList = pOneXmlNode.ChildNodes;
                for (int i = 0; i < pOneXmlNodeList.Count; i++)
                {
                    XmlNode pXmlNode = pOneXmlNodeList.Item(i);
                    ///遍历该节点与其输入节点的名称相同，且type值与其下一个节点的值相同则获取该节点的值
                    if (pXmlNode.Name == vModelSubNode.Name && vSubNode.Attributes["type"].Value.ToString() == pXmlNode.NextSibling.InnerText)
                    {
                        return strNodeText =pXmlNode.InnerText.ToString();
                    }
                    else
                    {
                        strNodeText = SamePathVale(ref strNodeText, pXmlNode, vSubNode);
                        return strNodeText;
                    }
                }
                return strNodeText;
            }
            catch {
                return strNodeText;
            }
        }
        private string GetSaveFilePath(string strDefault)
        {
            string strPath=""; 
            SaveFileDialog pSaveFileDialog = new SaveFileDialog();
            pSaveFileDialog.Title = "元数据导出";
            pSaveFileDialog.FileName = strDefault;
            pSaveFileDialog.Filter = "Excel 工作薄(*.xlsx)|*.xlsx|Excel 97-2003 工作薄 (*.xls)|*.xls";
            if (pSaveFileDialog.ShowDialog() != DialogResult.OK)
            { return strPath; }
            strPath = pSaveFileDialog.FileName;
            return strPath; 
        }
        private bool IsValueExists(List<string> LstFieldName, string strFieldName)
        {
            bool bIsExists = false;
            if (LstFieldName.Count == 0) { return bIsExists; }
            for (int i = 0; i < LstFieldName.Count; i++)
            {
                ///判断字典中是否已经该字符
                if (LstFieldName[i].ToString() == strFieldName)
                {
                    return bIsExists = true;
                }
            }
            return bIsExists;
        }
      
    }
}
