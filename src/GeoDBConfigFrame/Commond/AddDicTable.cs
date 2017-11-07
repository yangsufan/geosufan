using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using GeoDataCenterFunLib;
using ESRI.ArcGIS.Geodatabase;
using System.Xml;
using System.IO;
//添加记录
namespace GeoDBConfigFrame
{
    public class AddDicTable : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppPrivilegesRef m_Hook;
     //   private Plugin.Application.IAppFormRef m_frmhook;
        public AddDicTable()
        {
            base._Name = "GeoDBConfigFrame.AddDicTable";
            base._Caption = "添加字典";
            base._Tooltip = "添加字典";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "添加字典";
        }
        //添加记录菜单响应
        public override void OnClick()
        {

            if (m_Hook.GridCtrl  == null)
                return;
            FaceControl pfacecontrol = (FaceControl)m_Hook.MainUserControl;
            DataGridView pGridControl = m_Hook.GridCtrl;

            FaceControl pFaceControl =( FaceControl )(m_Hook.MainUserControl);

            FrmAddDicTable pFrm = new FrmAddDicTable();
            pFrm.ShowDialog();
            string strTableName = pFrm.TableName;
            if (strTableName != "")
            {
                AddTable(Plugin.ModuleCommon.TmpWorkSpace, strTableName);
                pfacecontrol.InitIndexTree();
            }
           if (this.WriteLog)
           {
               Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
           }            
        }
        private void AddTable(ESRI.ArcGIS.Geodatabase.IWorkspace pWks, string strTableName)
        {
            SysCommon.Gis.SysGisTable pGisTable = new SysCommon.Gis.SysGisTable(pWks);

            Exception eError = null;

            IFields pFields = new FieldsClass();
            IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
            IField pField = new FieldClass();
            IFieldEdit pEdit = pField as IFieldEdit;
            pEdit.Name_2 = "ID";
            pEdit.AliasName_2 = "ID";
            pEdit.Type_2 = esriFieldType.esriFieldTypeOID;
            //pEdit.Length_2 = 50;
            pFieldsEdit.AddField(pField);

            pField = new FieldClass();
            pEdit = pField as IFieldEdit;
            pEdit.Name_2 = "Code";
            pEdit.AliasName_2 = "编码";
            pEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pEdit.Length_2 = 50;
            pFieldsEdit.AddField(pField);

            pField = new FieldClass();
            pEdit = pField as IFieldEdit;
            pEdit.Name_2 = "Name";
            pEdit.AliasName_2 = "名称";
            pEdit.Type_2 = esriFieldType.esriFieldTypeString;
            pEdit.Length_2 = 255;
            pFieldsEdit.AddField(pField);
            try
            {
                pGisTable.CreateTable(strTableName, pFields, out eError);
            }
            catch
            { }
            pGisTable = null;

            string strInitXmlPath = Application.StartupPath + "\\..\\Res\\Xml\\DataTreeInitIndex.xml";
            XmlDocument xmldoc = new XmlDocument();

            if (File.Exists(strInitXmlPath))
            {
                xmldoc.Load(strInitXmlPath);
                string strSearch = "//Main/Childset/Itemset[@ItemName=" + "'数据字典'" + "]";
                XmlNode xmlNode = xmldoc.SelectSingleNode(strSearch);
                if (xmlNode != null)
                {
                    XmlElement childele = xmldoc.CreateElement("Layer");

                    childele.SetAttribute("tblName", strTableName);
                    childele.SetAttribute("ItemName", strTableName);
                    childele.SetAttribute("Caption", "");
                    xmlNode.AppendChild(childele as XmlNode);
                    xmldoc.Save(strInitXmlPath);
                }
            }
            xmldoc = null;

        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppPrivilegesRef;
         //   m_frmhook = hook as Plugin.Application.IAppFormRef;
        }
    }
}
