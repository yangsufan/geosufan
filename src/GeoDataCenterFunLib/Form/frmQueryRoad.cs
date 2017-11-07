using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Controls;

namespace GeoDataCenterFunLib
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110730
    /// 说明：道路和河流查询窗体
    /// </summary>
    public partial class frmQueryRoad : DevComponents.DotNetBar.Office2007Form
    {
        private string strSqlWhere = "";
        private string _strLayerName = "";//图层名，显示在查询结果窗口中 added by chulili 20110825
        private string _strFieldName = "";//地名字段
        private string _strFieldCode = "";//地名编码字段
        private Form _OwnerFrm = null;
        private IMapControlDefault _MapControl = null;
        private IFeatureClass  _QueryFeaClass = null;
        private frmQuerytoTable m_frmQuery = null;

        private IQueryFilter _QueryFilterAll = null;
        private IQueryFilter _QueryFilterPart1 = null;
        private IQueryFilter _QueryFilterPart2 = null;

        private bool _Writelog = true;  //added by chulili 2012-09-07 是否写日志
        public bool WriteLog
        {
            get
            {
                return _Writelog;
            }
            set
            {
                _Writelog = value;
            }
        }
        public frmQueryRoad(Form pFrm,IMapControlDefault pMapControl,IFeatureClass pFeatureClass,string strLayerName, string strFieldName,string strFieldCode,string labelFieldName,string labelFieldCode,string wndTitle)
        {
            InitializeComponent();
            _OwnerFrm = pFrm;
            _MapControl = pMapControl;
            _QueryFeaClass = pFeatureClass;
            _strLayerName = strLayerName;
            _strFieldName = strFieldName;
            _strFieldCode = strFieldCode;
            this.labelCode.Text = labelFieldCode;
            this.labelName.Text = labelFieldName;
            this.Text = wndTitle;
        }
        public string SqlWhere
        {
            get
            {
                return strSqlWhere;
            }
        }    

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (_QueryFeaClass == null)
            {
                return;
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(this.Text);//xisheng 日志记录 0928;
            }
            //获得数据库类型，根据数据库类型书写where条件
            string strWKSdescrip = ModQuery.GetDescriptionOfWorkspace((_QueryFeaClass as IDataset).Workspace);

            int indexName=_QueryFeaClass.Fields.FindField(_strFieldName);
            int indexCode = _QueryFeaClass.Fields.FindField(_strFieldCode);
            if (indexName < 0)
            {
                MessageBox.Show("找不到名称字段，请检查配置文件!","提示",MessageBoxButtons.OK,MessageBoxIcon.Exclamation );
                return;
            }
            if (indexCode < 0)
            {
                MessageBox.Show("找不到编码字段，请检查配置文件!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            esriFieldType typeName= _QueryFeaClass.Fields.get_Field(indexName).Type;
            esriFieldType typeCode = _QueryFeaClass.Fields.get_Field(indexCode).Type ;
            string strLike = "";
            switch (strWKSdescrip)
            {
                case "Personal Geodatabase"://mdb数据库 使用*作匹配符
                    strLike = "*";
                    break;
                case "File Geodatabase"://gdb数据库 使用%作匹配符
                    strLike = "%";
                    break;
                case "Spatial Database Connection"://sde(oracle数据库) 使用%作匹配符(sql server数据库，现在暂未考虑)
                    strLike = "%";
                    break;
                default:
                    strLike = "%";
                    break;
            }
            //构造where条件语句
            string strSqlWhere2 = "";
            string strSqlWhere3 = "";
            if (txtName.Text == "" && this.textCode.Text == "")
            {
                strSqlWhere = "";   //全部结果的查询条件
                strSqlWhere2 = "";  //查询结果中排序在前
                strSqlWhere3 = "1=0";   //查询结果中排序在后
            }
            else if (this.textCode.Text == "")
            {
                switch (strWKSdescrip)
                {
                    case "Personal Geodatabase"://mdb数据库 使用*作匹配符
                        strLike = "*";
                        strSqlWhere = _strFieldName + " LIKE '" + strLike + txtName.Text.Trim() + strLike + "'";
                        break;
                    case "File Geodatabase"://gdb数据库 使用%作匹配符
                        strLike = "%";
                        strSqlWhere = _strFieldName + " LIKE '" + strLike + txtName.Text.Trim() + strLike + "'";
                        break;
                    case "Spatial Database Connection"://sde(oracle数据库) 使用%作匹配符(sql server数据库，现在暂未考虑)
                        strLike = "%";
                        strSqlWhere ="contains("+ _strFieldName + ",'" + txtName.Text.Trim() + "')>0";
                        break;
                    default:
                        strLike = "%";
                        strSqlWhere = _strFieldName + " LIKE '" + strLike + txtName.Text.Trim() + strLike + "'";
                        break;
                }
                
                if (typeName.Equals(esriFieldType.esriFieldTypeString))
                {
                    strSqlWhere2 = _strFieldName + " = '" + txtName.Text.Trim() + "'";
                    strSqlWhere3 = _strFieldName + "<>'" + txtName.Text.Trim() + "'";
                }
                else
                {
                    strSqlWhere2 = _strFieldName + " = " + txtName.Text.Trim();
                    strSqlWhere3 = _strFieldName + " <> " + txtName.Text.Trim();
                }
            }
            else if (this.txtName.Text == "")
            {
                switch (strWKSdescrip)
                {
                    case "Personal Geodatabase"://mdb数据库 使用*作匹配符
                        strLike = "*";
                        strSqlWhere = _strFieldCode + " LIKE '" + strLike + textCode.Text.Trim() + strLike + "'";
                        break;
                    case "File Geodatabase"://gdb数据库 使用%作匹配符
                        strLike = "%";
                        strSqlWhere = _strFieldCode + " LIKE '" + strLike + textCode.Text.Trim() + strLike + "'";
                        break;
                    case "Spatial Database Connection"://sde(oracle数据库) 使用%作匹配符(sql server数据库，现在暂未考虑)
                        strLike = "%";
                        strSqlWhere = "contains(" + _strFieldCode + ",'" + textCode.Text.Trim() + "')>0";
                        break;
                    default:
                        strLike = "%";
                        strSqlWhere = _strFieldName + " LIKE '" + strLike + txtName.Text.Trim() + strLike + "'";
                        break;
                }
                
                if (typeCode.Equals(esriFieldType.esriFieldTypeString))
                {
                    strSqlWhere2 = _strFieldCode + " = '" + textCode.Text.Trim() + "'";
                    strSqlWhere3 = _strFieldCode + "<>'" + textCode.Text.Trim() + "'";
                }
                else
                {
                    strSqlWhere2 = _strFieldCode + " = " + textCode.Text.Trim();
                    strSqlWhere3 = _strFieldCode + "<>" + textCode.Text.Trim();
                }
            }
            else
            {
                switch (strWKSdescrip)
                {
                    case "Personal Geodatabase"://mdb数据库 使用*作匹配符
                        strLike = "*";
                        strSqlWhere = _strFieldCode + " LIKE '" + strLike + textCode.Text.Trim() + strLike + "' and " + _strFieldName + " LIKE '" + strLike + txtName.Text.Trim() + strLike + "'";
                        break;
                    case "File Geodatabase"://gdb数据库 使用%作匹配符
                        strLike = "%";
                        strSqlWhere = _strFieldCode + " LIKE '" + strLike + textCode.Text.Trim() + strLike + "' and " + _strFieldName + " LIKE '" + strLike + txtName.Text.Trim() + strLike + "'";
                        break;
                    case "Spatial Database Connection"://sde(oracle数据库) 使用%作匹配符(sql server数据库，现在暂未考虑)
                        strLike = "%";
                        strSqlWhere = "contains(" + _strFieldCode + ",'" + textCode.Text.Trim() + "') and contains(" + _strFieldName + ",'" + txtName.Text.Trim() + "')>0";
                        break;
                    default:
                        strLike = "%";
                        strSqlWhere = _strFieldName + " LIKE '" + strLike + txtName.Text.Trim() + strLike + "'";
                        break;
                }
                if (typeName.Equals(esriFieldType.esriFieldTypeString))
                {
                    strSqlWhere2 = _strFieldName + " = '" + txtName.Text.Trim() + "'";
                    strSqlWhere3 = _strFieldName + "<>'" + txtName.Text.Trim() + "'";
                }
                else
                {
                    strSqlWhere2 = _strFieldName + " = " + txtName.Text.Trim();
                    strSqlWhere3 = _strFieldName + " <> " + txtName.Text.Trim();
                }
            }
            this.Hide();
            if (_QueryFilterAll==null) _QueryFilterAll = new QueryFilterClass();
            if (_QueryFilterPart1 == null) _QueryFilterPart1 = new QueryFilterClass();
            if (_QueryFilterPart2 == null) _QueryFilterPart2 = new QueryFilterClass();
            //deleted by chulili 20110802 查询道路地物类,不再查询自然地名注记,就不必增加查询条件
            ////added by chulili 20110801 
            //if (RoadWhere.Equals(""))
            //{
            //    RoadWhere = "CLASS ='DI' OR CLASS='DG'";
            //}
            //else
            //{
            //    RoadWhere = RoadWhere + " AND (CLASS ='DI' OR CLASS='DG')";
            //}

            ////end added by chulili
            _QueryFilterAll.WhereClause = strSqlWhere;
            _QueryFilterPart1.WhereClause = strSqlWhere2;
            _QueryFilterPart2.WhereClause = strSqlWhere3;
            
            if (m_frmQuery == null)
            {
                m_frmQuery = new frmQuerytoTable(_MapControl);
            }
            //用查询地物类,查询条件填充查询窗体
            
            m_frmQuery.FillData(_QueryFeaClass,_strLayerName, _QueryFilterAll, _QueryFilterPart1, _QueryFilterPart2, true);
            m_frmQuery.Show(_OwnerFrm);
            m_frmQuery.FormClosed += new FormClosedEventHandler(frmQuery_FormClosed);
          
        }
        private void frmQuery_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_frmQuery = null;
            _QueryFilterAll = null;
            _QueryFilterPart1 = null;
            _QueryFilterPart2 = null;
            this.Dispose();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
