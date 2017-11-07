using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;

namespace GeoDBATool
{
    public partial class FrmFieldSetting : DevComponents.DotNetBar.Office2007Form
    {
        //源字段名和目标字段名字段信息对(只包含用户自定义的字段)
        private Dictionary<string, string> fieldNameInfo = new Dictionary<string, string>();
        public Dictionary<string, string> FIELDNAME
        {
            get
            {
                return fieldNameInfo;
            }
            set
            {
                fieldNameInfo = value;
            }
        }

        public FrmFieldSetting(IFeatureClass orgFeaCls, List<IField> orgFieldList, Dictionary<string, string> SaveState)
        {
            InitializeComponent();
            checkBoxField.Checked = true;
            dgFieldSeeting.Columns.Clear();

            initalDGridField(orgFeaCls, orgFieldList, SaveState);
        }
        /// <summary>
        /// 初始化dgFieldSeeting
        /// </summary>
        /// <param name="orgFeaCls"></param>
        /// <param name="orgFieldList"></param>
        private void initalDGridField(IFeatureClass orgFeaCls, List<IField> orgFieldList, Dictionary<string, string> SaveState)
        {
            //初始化dgFieldSeeting结构
            //特殊列

            DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
            checkBoxCol.Name = "是否导出";
            checkBoxCol.HeaderText = "是否导出";
            dgFieldSeeting.Columns.Add(checkBoxCol);//.Add("是否导出", "是否导出");

            dgFieldSeeting.Columns.Add("源字段名", "源字段名");
            dgFieldSeeting.Columns.Add("目标字段名", "目标字段名");
            dgFieldSeeting.Columns[1].ReadOnly = true;

            for (int j = 0; j < dgFieldSeeting.Columns.Count; j++)
            {
                dgFieldSeeting.Columns[j].Width = (dgFieldSeeting.Width - 20) / dgFieldSeeting.Columns.Count;
            }
            dgFieldSeeting.RowHeadersWidth = 20;

            #region 初始化dgFieldSeeting行，只添加用户定义的字段信息
            for (int i = 0; i < orgFieldList.Count; i++)
            {
                //源字段名
                string orgFieldName = orgFieldList[i].Name;
                if (orgFieldName == "") return;

                #region 将特殊的字段排除掉

                if (orgFieldList[i].Type == esriFieldType.esriFieldTypeGeometry) continue;
                if (orgFieldList[i].Type == esriFieldType.esriFieldTypeOID) continue;
                //if (orgFieldList[i].Type == esriFieldType.esriFieldTypeBlob) continue;
                if (orgFieldList[i].Editable == false) continue;
                if (orgFeaCls.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    //注记

                    bool annofield = false;//在判断注记的必要字段时，作为标志跳出循环
                    //判断如果该字段是注记的必要字段，则排除

                    IObjectClassDescription pOCDesc = new AnnotationFeatureClassDescription();
                    IFields pfields = pOCDesc.RequiredFields;
                    for (int k = 0; k < pfields.FieldCount; k++)
                    {

                        if (pfields.get_Field(k).Name.ToLower() == orgFieldName.ToLower())
                        {
                            annofield = true;
                            break;
                        }
                    }
                    if (annofield == true)
                    {
                        continue;
                    }
                }
                #endregion
                //添加行信息

                DataGridViewRow dgRow = new DataGridViewRow();
                dgRow.CreateCells(dgFieldSeeting);
                //第一列

                //************************************************************************************************
                //guozheng 2010-12-28 added 对已保存的状态进行判断，解决再打开窗口时，所有的字段都为选中的状态的问题
                if (SaveState != null)
                {
                    if (SaveState.ContainsKey(orgFieldName)) dgRow.Cells[0].Value = true;
                    else dgRow.Cells[0].Value = false;
                }
                else
                    dgRow.Cells[0].Value = true;// 是否导出

                //************************************************************************************************
                //第二列

                dgRow.Cells[1].Value = orgFieldName;  //源字段名
                //第三列

                dgRow.Cells[2].Value = orgFieldName;  //目标字段名

                dgFieldSeeting.Rows.Add(dgRow);
            }
            #endregion
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgFieldSeeting.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgFieldSeeting.Rows[i].Cells[0].FormattedValue.ToString()) == true)
                {
                    string desFieldName = dgFieldSeeting.Rows[i].Cells[2].FormattedValue.ToString();//目标字段名

                    if (desFieldName == "")
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "目标字段名称不能为空！");
                        return;
                    }
                }
            }
            //将字段信息对保存起来
            Dictionary<string, string> fieldInfoDic = new Dictionary<string, string>();
            for (int i = 0; i < dgFieldSeeting.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgFieldSeeting.Rows[i].Cells[0].FormattedValue.ToString()) == true)
                {
                    //勾选中
                    string orgFieldName = dgFieldSeeting.Rows[i].Cells[1].FormattedValue.ToString();//源字段名
                    string desFieldName = dgFieldSeeting.Rows[i].Cells[2].FormattedValue.ToString();//目标字段名

                    if (orgFieldName == "") continue;

                    if (!fieldInfoDic.ContainsKey(orgFieldName))
                    {
                        fieldInfoDic.Add(orgFieldName, desFieldName);
                    }
                }
            }
            fieldNameInfo = fieldInfoDic;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void checkBoxField_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxField.Checked)
            {
                //勾选

                for (int i = 0; i < dgFieldSeeting.Rows.Count; i++)
                {
                    dgFieldSeeting.Rows[i].Cells["目标字段名"].Value = dgFieldSeeting.Rows[i].Cells["源字段名"].FormattedValue.ToString();
                }
            }

        }

        private void dgFieldSeeting_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgFieldSeeting.CurrentCell.ColumnIndex == 2)
            {
                //目标字段
                checkBoxField.Checked = false;
            }
        }
    }
}