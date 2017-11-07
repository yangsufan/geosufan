using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Display;

namespace GeoDBATool
{
    /// <summary>
    /// 矢量数据提取
    /// </summary>
    public partial class FrmDataStract : DevComponents.DotNetBar.Office2007Form
    {
        private Plugin.Application.IAppGISRef _Hook;

        private string v_Scale = "";

        //画范围


        //private IGeometry mGeo = null;
        //public IGeometry DrawGeometry
        //{
        //    get 
        //    {
        //        return mGeo;
        //    }
        //    set 
        //    {
        //        mGeo = value;
        //    }
        //}


        public FrmDataStract(Plugin.Application.IAppGISRef mHook)
        {
            InitializeComponent();

            radioCurrent.Checked = true;
            btnAddData.Enabled = false;
            _Hook = mHook;

            IntialDgPropertis(mHook);
            cmbDataFormat.Items.AddRange(new object[] { "ESRI文件数据库(*.gdb)", "ESRI个人数据库(*.mdb)" });//, "PDB", "GDB""SHP"
            cmbDataFormat.SelectedIndex = 0;

            cmbScale.Items.AddRange(new object[] {"0", "500", "1000", "2000", "5000", "10000", "20000" });
            cmbScale.SelectedIndex = 0;

            v_Scale = cmbScale.SelectedItem.ToString().Trim();

            //XmlNode ProNode = mHook.ProjectTree.SelectedNode.Tag as XmlNode;
            //v_Scale = (ProNode as XmlElement).GetAttribute("比例尺");
            //if(v_Scale=="")
            //{
            //    v_Scale = "500";
            //}

            //初始化图幅选择列表
            IntialDgMap();

            //初始化行政区列表
            IntialDgCounty();

            //初始化任意多边形列表
            IntialDgRange();
        }

        private void btnAddData_Click(object sender, EventArgs e)
        {
            //ICommand cmd = new ControlsAddDataCommand();
            //cmd.OnCreate(_Hook.MapControl);
            //if (cmd == null) return;
            //cmd.OnClick();
        }

        private void radioNonCurrent_CheckedChanged(object sender, EventArgs e)
        {
            //if(radioNonCurrent.Checked)
            //{
            //    btnAddData.Enabled = true;
            //}
            //else 
            //{
            //    btnAddData.Enabled = false;
            //}
        }

        private void dgPropertiesSetting_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            List<IField> fieldList = new List<IField>();//字段信息集合

            string feaclsName = dgPropertiesSetting.Rows[dgPropertiesSetting.CurrentCell.RowIndex].Cells["源图层名"].FormattedValue.ToString();//获得源图层名
            IFeatureClass pFeatureClass = dgPropertiesSetting.Rows[dgPropertiesSetting.CurrentCell.RowIndex].Cells["源图层名"].Tag as IFeatureClass;//源要素类
            if (pFeatureClass == null) return;

            if (dgPropertiesSetting.CurrentCell.ColumnIndex == 4)
            {
                #region 属性表达式条件获取
                fieldList = dgPropertiesSetting.CurrentCell.Tag as List<IField>;
                if (fieldList == null) return;

                FrmSQLQuery pFrmSQLQuery = new FrmSQLQuery(pFeatureClass, fieldList);
                if (pFrmSQLQuery.ShowDialog() == DialogResult.OK)
                {
                    dgPropertiesSetting.CurrentCell.Value = pFrmSQLQuery.WhereClause;
                }
                #endregion
            }
            //else if (dgPropertiesSetting.CurrentCell.ColumnIndex == 5)
            //{

            //    if (Convert.ToBoolean(dgPropertiesSetting.Rows[dgPropertiesSetting.CurrentCell.RowIndex].Cells[5].Value.ToString()))
            //    {
            //        //采用源字段


            //        dgPropertiesSetting.Rows[dgPropertiesSetting.CurrentCell.RowIndex].Cells["字段对应关系"].ReadOnly = true;
            //    }
            //    else
            //    {
            //        //不采用源字段，进行设置


            //        dgPropertiesSetting.Rows[dgPropertiesSetting.CurrentCell.RowIndex].Cells["字段对应关系"].ReadOnly = false;
            //    }

            //}
            else if (dgPropertiesSetting.CurrentCell.ColumnIndex == 5)
            {
                //字段对应关系
                fieldList = dgPropertiesSetting.Rows[dgPropertiesSetting.CurrentCell.RowIndex].Cells["属性表达式"].Tag as List<IField>;
                if (fieldList == null) return;

                FrmFieldSetting pFrmSQLQuery = new FrmFieldSetting(pFeatureClass, fieldList, dgPropertiesSetting.CurrentCell.Tag as Dictionary<string, string>);
                if (pFrmSQLQuery.ShowDialog() == DialogResult.OK)
                {
                    dgPropertiesSetting.CurrentCell.Tag = pFrmSQLQuery.FIELDNAME;
                }
            }
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            //cyf 20110628 modify:界面统一
            if (cmbDataFormat.Tag.ToString().ToUpper() == "PDB")
            {
                saveDialog.Title = "保存ESRI个人数据库";
                saveDialog.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
            }
            else if (cmbDataFormat.Tag.ToString().ToUpper() == "GDB")
            {
                saveDialog.Title = "保存ESRI文件数据库";
                saveDialog.Filter = "ESRI文件数据库(*.gdb)|*.gdb";
            }
            else if (cmbDataFormat.Tag.ToString().ToUpper() == "SHP")
            {
                saveDialog.Title = "保存SHP数据";
                //saveDialog.Filter = "SHP数据(*.SHP)|*.SHP";
            }
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                //if (File.Exists(saveDialog.FileName))
                //{
                //    if (SysCommon.Error.ErrorHandle.ShowFrmInformation("确定", "取消", "该文件已存在，是否替换文件?"))
                //    {
                //        File.Delete(saveDialog.FileName);
                //    }
                //    else
                //    {
                //        txtSavePath.Text = "";
                //        return;
                //    }
                //}
                txtSavePath.Text = saveDialog.FileName;
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Exception eError = null;
            //_Hook.MapControl.CurrentTool = null;
            //界面控制判断和检查


            if (txtSavePath.Text == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择保存路径！");
                return;
            }
            if (dgPropertiesSetting.Rows.Count == 0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "没有要导出的数据，请加载矢量数据！");
                return;
            }
            for (int i = 0; i < dgPropertiesSetting.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgPropertiesSetting.Rows[i].Cells[1].FormattedValue.ToString()) == true)
                {
                    string desFName = dgPropertiesSetting.Rows[i].Cells["目标图层名"].FormattedValue.ToString();
                    if (desFName == "")
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "目标图层名不能为空，请填写！");
                        return;
                    }
                }
            }

            #region 创建工作空间
            IWorkspace pWS = null;//定义工作空间
            IFeatureWorkspace pFeaWS = null;//要素类工作空间


            //如果存在同名库体则删除


            if (File.Exists(txtSavePath.Text.Trim()))
            {
                File.Delete(txtSavePath.Text.Trim());
            }
            //cyf 20110628 modify:界面统一
            if (cmbDataFormat.Tag.ToString().Trim().ToUpper() == "PDB")
            {
                pWS = CreateWorkspace(txtSavePath.Text.Trim(), "PDB", out eError);

            }
            else if (cmbDataFormat.Tag.ToString().Trim().ToUpper() == "GDB")
            {
                pWS = CreateWorkspace(txtSavePath.Text.Trim(), "GDB", out eError);
            }
            else if (cmbDataFormat.Tag.ToString().Trim().ToUpper() == "SHP")
            {
                pWS = CreateWorkspace(txtSavePath.Text.Trim(), "SHP", out eError);
            }

            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建工作空间失败！");
                return;
            }
            pFeaWS = pWS as IFeatureWorkspace;
            if (pFeaWS == null) return;
            #endregion

            #region 创建工作空间下的要素类


            for (int i = 0; i < dgPropertiesSetting.Rows.Count; i++)
            {
                bool b = Convert.ToBoolean(dgPropertiesSetting.Rows[i].Cells["是否导出"].FormattedValue.ToString());
                if (b)
                {
                    IFeatureClass orgFeaCls = dgPropertiesSetting.Rows[i].Cells["源图层名"].Tag as IFeatureClass;
                    string desFeaClsName = dgPropertiesSetting.Rows[i].Cells["目标图层名"].FormattedValue.ToString();
                    List<IField> orgFieldList = dgPropertiesSetting.Rows[i].Cells["属性表达式"].Tag as List<IField>;
                    if (orgFieldList == null)
                    {
                        return;
                    }
                    Dictionary<string, string> fieldNameDic = dgPropertiesSetting.Rows[i].Cells["字段对应关系"].Tag as Dictionary<string, string>;
                    if (fieldNameDic == null)
                    {
                        //采用源字段


                        #region 将字段对应关系保存起来


                        fieldNameDic = new Dictionary<string, string>();
                        List<IField> mFieldList = dgPropertiesSetting.Rows[i].Cells["属性表达式"].Tag as List<IField>;
                        for (int j = 0; j < mFieldList.Count; j++)
                        {
                            //源字段名
                            string orgFieldName = mFieldList[j].Name;
                            if (orgFieldName == "") return;

                            #region 将特殊的字段排除掉


                            if (mFieldList[j].Type == esriFieldType.esriFieldTypeGeometry) continue;
                            if (mFieldList[j].Type == esriFieldType.esriFieldTypeOID) continue;
                            if (mFieldList[j].Editable == false) continue;
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
                            if (!fieldNameDic.ContainsKey(orgFieldName))
                            {
                                fieldNameDic.Add(orgFieldName, orgFieldName);
                            }
                            #endregion
                        }
                        if (fieldNameDic == null)
                        {
                            return;
                        }
                        dgPropertiesSetting.Rows[i].Cells["字段对应关系"].Tag = fieldNameDic;
                        #endregion
                    }
                    //创建要素类


                    if (!CreateFeatureClass(pFeaWS, orgFeaCls, desFeaClsName, orgFieldList, fieldNameDic, out eError))
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                        return;
                    }
                }
            }
            # endregion

            #region 将数据入库


            int max = 0;
            for (int i = 0; i < dgPropertiesSetting.Rows.Count; i++)
            {
                bool b = Convert.ToBoolean(dgPropertiesSetting.Rows[i].Cells["是否导出"].FormattedValue.ToString());
                if (b)
                {
                    max++;
                }
            }

            int pValue = 0;
            progressBarX1.Maximum = max;
            progressBarX1.Minimum = 0;
            progressBarX1.Value = pValue;

            for (int i = 0; i < dgPropertiesSetting.Rows.Count; i++)
            {
                bool b = Convert.ToBoolean(dgPropertiesSetting.Rows[i].Cells["是否导出"].FormattedValue.ToString());
                if (b)
                {
                    IFeatureClass orgFeaCls = dgPropertiesSetting.Rows[i].Cells["源图层名"].Tag as IFeatureClass;//源要素类
                    if (orgFeaCls == null) continue;
                    IFeatureClass desFeaCls = null;//目标要素类


                    string desFeaClsName = dgPropertiesSetting.Rows[i].Cells["目标图层名"].FormattedValue.ToString();//目标要素类名
                    string whereStr = dgPropertiesSetting.Rows[i].Cells["属性表达式"].FormattedValue.ToString();//属性条件


                    List<IField> orgFieldList = dgPropertiesSetting.Rows[i].Cells["属性表达式"].Tag as List<IField>; //源要素类字段集合
                    if (orgFieldList == null)
                    {
                        return;
                    }
                    //源要素类和目标要素类字段名对应关系


                    Dictionary<string, string> fieldNameDic = dgPropertiesSetting.Rows[i].Cells["字段对应关系"].Tag as Dictionary<string, string>;
                    if (fieldNameDic == null)
                    {
                        return;
                    }
                    //打开目标要素类


                    try
                    {
                        desFeaCls = pFeaWS.OpenFeatureClass(desFeaClsName);
                    }
                    catch (Exception eex)
                    {
                        eError = eex;
                        return;
                    }

                    //源要素类和目标要素类字段做引对应关系（只针对用户定义的字段）
                    Dictionary<int, int> fieldIndexDic = GetFIndexPair(orgFieldList, orgFeaCls, desFeaCls, fieldNameDic, out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                        return;
                    }
                    if (fieldIndexDic == null) return;

                    #region 获得几何限制条件
                    IGeometry pGeoUnio = null;
                    IGeometry pGeo1 = null;
                    IGeometry pGeo2 = null;
                    IGeometry pGeo3 = null;
                    if (checkBoxMap.Checked)
                    {
                        pGeo1 = GetUnionGeo(dgMap);
                    }
                    if (checkBoxCounty.Checked)
                    {
                        pGeo2 = GetUnionGeo(dgCounty);
                    }
                    if (checkBoxGeo.Checked)
                    {
                        pGeo3 = GetUnionGeo(dgRange);
                    }
                    ITopologicalOperator pTop = pGeoUnio as ITopologicalOperator;
                    if (pGeo1 != null)
                    {
                        if (pGeoUnio == null)
                        {
                            pGeoUnio = pGeo1;
                        }
                        else
                        {
                            pGeoUnio = pTop.Union(pGeo1);
                        }

                    }
                    pTop = pGeoUnio as ITopologicalOperator;
                    if (pGeo2 != null)
                    {
                        if (pGeoUnio == null)
                        {
                            pGeoUnio = pGeo2;
                        }
                        else
                        {
                            pGeoUnio = pTop.Union(pGeo2);
                        }

                    }
                    pTop = pGeoUnio as ITopologicalOperator;
                    if (pGeo3 != null)
                    {
                        if (pGeoUnio == null)
                        {
                            pGeoUnio = pGeo3;
                        }
                        else
                        {
                            pGeoUnio = pTop.Union(pGeo3);
                        }

                    }
                    if (pGeoUnio != null)
                    {
                        pTop = pGeoUnio as ITopologicalOperator;
                        pTop.Simplify();
                        pGeoUnio = pTop as IGeometry;
                    }

                    #endregion

                    //是否切割checkBoxClip

                    //进行数据入库
                    if (!DataImport(orgFeaCls, whereStr, pGeoUnio, checkBoxClip.Checked, desFeaCls, fieldIndexDic, out eError))
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据入库失败。要素类名称为：'" + desFeaClsName + "'");
                        return;
                    }
                    pValue++;
                    progressBarX1.Value = pValue;
                }
            }

            #endregion

            this.Close();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelMap_Click(object sender, EventArgs e)
        {
            string pScale = v_Scale;
            FrmCountySheet pFrmMapSheet = new FrmCountySheet(pScale, "图幅选择");

            if (pFrmMapSheet.ShowDialog() == DialogResult.OK)
            {

                Dictionary<string, IGeometry> rangeFeaInfo = new Dictionary<string, IGeometry>();
                rangeFeaInfo = pFrmMapSheet.RANGEFEADIC;

                //将范围要素信息在dgCounty中显示出来


                foreach (KeyValuePair<string, IGeometry> rangeItem in rangeFeaInfo)
                {
                    //添加行


                    DataGridViewRow dgRow = new DataGridViewRow();
                    dgRow.CreateCells(dgMap);
                    //第一列


                    dgRow.Cells[0].Value = true;// 是否选用
                    //第二列


                    dgRow.Cells[1].Value = rangeItem.Key;
                    //第二列


                    dgRow.Cells[1].Tag = rangeItem.Value;

                    dgMap.Rows.Add(dgRow);
                }
            }
            /* dgMap.Rows.Clear();
             string pScale = v_Scale;

             string mMapPath = ModData.MapPath;
             frmMapSheet MapSheet = new frmMapSheet(mMapPath);
             MapSheet.MapNumCheck = false;

             MapSheet.StrScale = pScale;
             string mFrameName = GetMapFrameName(mMapPath, pScale);
             if (mFrameName == "")
             {
                 SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("信息提示", "没有比例尺为" + pScale + "的图幅结合表，请检查！");
                 return;
             }
             MapSheet.MapFrameName = mFrameName;
             MapSheet.ShowDialog();

             if (MapSheet.Res == true)
             {
                 dgMap.Rows.Clear();
                 //listViewRange.Items.Clear();
                 dgMap.Tag = MapSheet.GEOMETRY;
                 //返回选择的图幅号
                 int ArrLength = 0;
                 //返回选择的图幅号
                 string[] SelectedMapNo = null;
                 if (MapSheet.MapNumSelected != null)
                 {
                     if (MapSheet.MapNumSelected.Length > 0)
                     {
                         ArrLength = MapSheet.MapNumSelected.Length;
                         SelectedMapNo = new string[ArrLength];
                         SelectedMapNo = MapSheet.MapNumSelected;
                         for (int i = 0; i < SelectedMapNo.Length; i++)
                         {
                             //添加行

                             DataGridViewRow dgRow = new DataGridViewRow();
                             dgRow.CreateCells(dgMap);
                             //第一列

                             dgRow.Cells[0].Value = true;// 是否选用
                             //第三列

                             dgRow.Cells[1].Value = SelectedMapNo.GetValue(i).ToString();  //源图层名

                             dgMap.Rows.Add(dgRow);
                         }
                     }
                 }
             }
             //MapFrameName = MapSheet.MapSheetName;
             * */
        }

        private void btnSelCounty_Click(object sender, EventArgs e)
        {
            //dgCounty.Rows.Clear();
            string pScale = v_Scale;
            FrmCountySheet pFrmCountySheet = new FrmCountySheet(pScale, "行政区范围选择");

            if (pFrmCountySheet.ShowDialog() == DialogResult.OK)
            {
                //dgCounty.Rows.Clear();
                //IGeometry pGeo = pFrmCountySheet.CountyGEOMETRY ;
                //dgCounty.Tag = pGeo;
                Dictionary<string, IGeometry> rangeFeaInfo = new Dictionary<string, IGeometry>();
                rangeFeaInfo = pFrmCountySheet.RANGEFEADIC;

                //将范围要素信息在dgCounty中显示出来


                foreach (KeyValuePair<string, IGeometry> rangeItem in rangeFeaInfo)
                {
                    //添加行


                    DataGridViewRow dgRow = new DataGridViewRow();
                    dgRow.CreateCells(dgCounty);
                    //第一列


                    dgRow.Cells[0].Value = true;// 是否选用
                    //第二列


                    dgRow.Cells[1].Value = rangeItem.Key;
                    //第二列


                    dgRow.Cells[1].Tag = rangeItem.Value;

                    dgCounty.Rows.Add(dgRow);
                }
            }
        }

        private void btnSelRange_Click(object sender, EventArgs e)
        {
            //dgRange.Rows.Clear();
            DrawPolygonToolClass drawPolygon = new DrawPolygonToolClass(true, this);
            drawPolygon.OnCreate(_Hook.MapControl);
            _Hook.MapControl.CurrentTool = drawPolygon as ITool;
        }


        //===========================================================================================================================
        #region 初始化函数



        /// <summary>
        /// 初始化dgPropertiesSetting列表
        /// </summary>
        private void IntialDgPropertis(Plugin.Application.IAppGISRef mHook)
        {
            dgPropertiesSetting.Rows.Clear();

            //初始化dgPropertiesSetting字段信息
            dgPropertiesSetting.Columns.Add("序号", "序号");
            //特殊列


            DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
            checkBoxCol.Name = "是否导出";
            checkBoxCol.HeaderText = "是否导出";
            dgPropertiesSetting.Columns.Add(checkBoxCol);//.Add("是否导出", "是否导出");

            dgPropertiesSetting.Columns.Add("源图层名", "源图层名");
            dgPropertiesSetting.Columns.Add("目标图层名", "目标图层名");

            //特殊列


            dgPropertiesSetting.Columns.Add("属性表达式", "属性表达式");
            //字段一一对应
            //DataGridViewCheckBoxColumn checkBoxCol2 = new DataGridViewCheckBoxColumn();
            //checkBoxCol2.Name = "采用源字段";
            //checkBoxCol2.HeaderText = "采用源字段";
            //dgPropertiesSetting.Columns.Add(checkBoxCol2);
            //特殊列


            DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
            btnCol.Name = "字段对应关系";
            btnCol.HeaderText = "字段对应关系";
            dgPropertiesSetting.Columns.Add(btnCol);//"字段对应关系", "字段对应关系");

            dgPropertiesSetting.Columns[0].ReadOnly = true;
            dgPropertiesSetting.Columns[2].ReadOnly = true;
            dgPropertiesSetting.Visible = true;
            for (int j = 0; j < dgPropertiesSetting.Columns.Count; j++)
            {
                //cyf 20110706 modify
                //if (j == 0)
                //{
                //    dgPropertiesSetting.Columns[j].Width = (dgPropertiesSetting.Width - 20) / dgPropertiesSetting.Columns.Count - 60;
                //}
                //else if (j == 1)
                //{
                //    dgPropertiesSetting.Columns[j].Width = (dgPropertiesSetting.Width - 20) / dgPropertiesSetting.Columns.Count - 50;
                //}
                //else if (j == 2 || j == 3)
                //{
                //    dgPropertiesSetting.Columns[j].Width = (dgPropertiesSetting.Width + 100) / dgPropertiesSetting.Columns.Count - 20;
                //}
                //else
                //{
                //    dgPropertiesSetting.Columns[j].Width = (dgPropertiesSetting.Width + 220) / dgPropertiesSetting.Columns.Count + 5;
                //}
                dgPropertiesSetting.Columns[j].Width = (dgPropertiesSetting.Width - 20) / dgPropertiesSetting.Columns.Count;
                //end
            }
            dgPropertiesSetting.RowHeadersWidth = 20;

            #region 初始化dgPropertiesSetting里面的数据


            int rowindex = 0;
            List<IFeatureLayer> feaLayers = new List<IFeatureLayer>();
            feaLayers = GetFeaLayerFromMap(mHook);
            if (feaLayers == null) return;
            for (int i = 0; i < feaLayers.Count; i++)
            {
                IFeatureLayer mFeaLayer = feaLayers[i];
                IFeatureClass mFeaCls = mFeaLayer.FeatureClass;
                string feaclsName = (mFeaCls as IDataset).Name;

                List<IField> fieldInfoList = new List<IField>();//字段信息集合
                fieldInfoList = GetFieldInfo(mFeaCls);
                if (fieldInfoList == null) return;
                //添加行


                DataGridViewRow dgRow = new DataGridViewRow();
                dgRow.CreateCells(dgPropertiesSetting);
                //第一列


                dgRow.Cells[0].Value = rowindex + 1;  //序号
                //第二列


                dgRow.Cells[1].Value = true;// 是否导出
                //第三列


                dgRow.Cells[2].Value = feaclsName;  //源图层名
                dgRow.Cells[2].Tag = mFeaCls;//源要素类
                //第四列

                string sDesLayerName = string.Empty;
                sDesLayerName = mFeaLayer.Name;
                if (sDesLayerName.Contains(".")) sDesLayerName = sDesLayerName.Substring(sDesLayerName.LastIndexOf('.')+1);
                dgRow.Cells[3].Value = sDesLayerName;// feaclsName;  //目标图层名


                //第五列


                dgRow.Cells[4].Value = "";  //属性表达式
                dgRow.Cells[4].Tag = fieldInfoList;//存储字段信息
                //第六列


                //dgRow.Cells[5].Value = true;//采用源字段


                //第七列


                dgRow.Cells[5].Value = "设置";

                dgPropertiesSetting.Rows.Add(dgRow);
                rowindex++;
            }

            //}

            #endregion
        }

        /// <summary>
        /// 初始化dgMap列表
        /// </summary>
        private void IntialDgMap()
        {
            dgMap.Rows.Clear();

            DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
            checkBoxCol.Name = "是否选用";
            checkBoxCol.HeaderText = "是否选用";
            dgMap.Columns.Add(checkBoxCol);//.Add("是否导出", "是否导出");
            dgMap.Columns.Add("图幅号", "图幅号");

            //dgMap.Columns[0].ReadOnly = true;
            dgMap.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgMap.Columns[1].ReadOnly = true;
            dgMap.Visible = true;
            for (int j = 0; j < dgMap.Columns.Count; j++)
            {
                dgMap.Columns[j].Width = (dgMap.Width - 20) / dgMap.Columns.Count;
            }
            dgMap.RowHeadersWidth = 20;
        }

        /// <summary>
        /// 初始化行政区范围列表
        /// </summary>
        private void IntialDgCounty()
        {
            dgCounty.Rows.Clear();

            DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
            checkBoxCol.Name = "是否选用";
            checkBoxCol.HeaderText = "是否选用";
            dgCounty.Columns.Add(checkBoxCol);//.Add("是否导出", "是否导出");
            dgCounty.Columns.Add("范围名称", "范围名称");

            //dgCounty.Columns[0].ReadOnly = true;
            dgCounty.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgCounty.Columns[1].ReadOnly = true;
            dgCounty.Visible = true;
            for (int j = 0; j < dgCounty.Columns.Count; j++)
            {
                dgCounty.Columns[j].Width = (dgCounty.Width - 20) / dgCounty.Columns.Count;
            }
            dgCounty.RowHeadersWidth = 20;
        }

        /// <summary>
        /// 初始化任意多边形列表
        /// </summary>
        private void IntialDgRange()
        {
            dgRange.Rows.Clear();

            DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
            checkBoxCol.Name = "是否选用";
            checkBoxCol.HeaderText = "是否选用";
            dgRange.Columns.Add(checkBoxCol);
            dgRange.Columns.Add("任意多边形", "任意多边形");

            //dgRange.Columns[0].ReadOnly = true;
            dgRange.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgRange.Columns[1].ReadOnly = true;
            dgRange.Visible = true;
            for (int j = 0; j < dgRange.Columns.Count; j++)
            {
                dgRange.Columns[j].Width = (dgRange.Width - 20) / dgRange.Columns.Count;
            }
            dgRange.RowHeadersWidth = 20;
        }

        #endregion


        #region 创建库体
        /// <summary>
        /// 创建几何字段
        /// </summary>
        /// <param name="orgField">源几何字段</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private IField GetGeometryField(IField orgField, out Exception eError)
        {
            eError = null;
            try
            {
                IField newfield1 = new FieldClass();
                IFieldEdit fieldEdit1 = newfield1 as IFieldEdit;
                fieldEdit1.Name_2 = "SHAPE";
                fieldEdit1.AliasName_2 = "SHAPE";
                fieldEdit1.Type_2 = orgField.Type;
                fieldEdit1.GeometryDef_2 = orgField.GeometryDef;
                newfield1 = fieldEdit1 as IField;
                return newfield1;
            }

            catch
            {
                eError = new Exception("创建几何shape字段失败！");
                return null;
            }
        }

        /// <summary>
        /// 创建OBJECT字段
        /// </summary>
        /// <param name="eError"></param>
        /// <returns></returns>
        private IField GetObjectField(out Exception eError)
        {
            eError = null;
            try
            {
                IField newfield2 = new FieldClass();
                IFieldEdit fieldEdit2 = newfield2 as IFieldEdit;
                fieldEdit2.Name_2 = "OBJECTID";
                fieldEdit2.AliasName_2 = "OBJECTID";
                fieldEdit2.Type_2 = esriFieldType.esriFieldTypeOID;
                fieldEdit2.AliasName_2 = "OBJECTID";
                fieldEdit2.IsNullable_2 = false;
                fieldEdit2.Required_2 = true;
                fieldEdit2.Editable_2 = false;
                newfield2 = fieldEdit2 as IField;
                return newfield2;
            }
            catch
            {
                eError = new Exception("创建几何OBJECT字段失败！");
                return null;
            }
        }

        /// <summary>
        /// 创建普通字段


        /// </summary>
        /// <param name="orgField">源字段</param>
        /// <param name="desFieldName">目标字段名</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private IField GetNormalField(IField orgField, string desFieldName, out Exception eError)
        {
            eError = null;
            try
            {
                IField newfield = new FieldClass();
                IFieldEdit fieldEdit = newfield as IFieldEdit;
                fieldEdit.Name_2 = desFieldName;
                fieldEdit.AliasName_2 = desFieldName;
                fieldEdit.Type_2 = orgField.Type;
                fieldEdit.IsNullable_2 = orgField.IsNullable;
                fieldEdit.Length_2 = orgField.Length;
                fieldEdit.Precision_2 = orgField.Precision;
                fieldEdit.Scale_2 = orgField.Scale;
                fieldEdit.Required_2 = orgField.Required;
                fieldEdit.Editable_2 = orgField.Editable;
                fieldEdit.DomainFixed_2 = orgField.DomainFixed;
                fieldEdit.DefaultValue_2 = orgField.DefaultValue;
                newfield = fieldEdit as IField;
                return newfield;
            }
            catch
            {
                eError = new Exception("创建几何普通字段失败，源字段名为：'" + orgField.Name + "'！");
                return null;
            }
        }

        /// <summary>
        /// 创建要保存的要素类的工作空间
        /// </summary>
        /// <param name="sFilePath">路径</param>
        /// <param name="strType">工作空间类型</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private IWorkspace CreateWorkspace(string sFilePath, string strType, out Exception eError)
        {
            try
            {
                eError = null;
                IWorkspace TempWorkSpace = null;
                IWorkspaceFactory pWorkspaceFactory = null;
                if (strType.Trim().ToUpper() == "PDB")
                {
                    pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                }
                else if (strType.Trim().ToUpper() == "GDB")
                {
                    pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                }
                else if (strType.Trim().ToUpper() == "SHP")
                {
                    pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();
                }

                if (!File.Exists(sFilePath))
                {
                    FileInfo finfo = new FileInfo(sFilePath);
                    string outputDBPath = finfo.DirectoryName;
                    string outputDBName = finfo.Name;
                    //if (outputDBName.LastIndexOf('.') != -1)
                    //{
                    //    outputDBName = outputDBName.Substring(0, outputDBName.LastIndexOf('.'));
                    //}

                    IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create(outputDBPath, outputDBName, null, 0);
                    IName pName = (IName)pWorkspaceName;
                    TempWorkSpace = (IWorkspace)pName.Open();
                }
                else
                {
                    TempWorkSpace = pWorkspaceFactory.OpenFromFile(sFilePath, 0);
                }
                return TempWorkSpace;
            }
            catch (Exception eX)
            {
                eError = eX;
                return null;
            }
        }


        /// <summary>
        /// 创建要素类


        /// </summary>
        /// <param name="feaworkspace"></param>
        /// <param name="orgFeaCls"></param>
        /// <param name="desFeaClsName"></param>
        /// <param name="orgFieldList"></param>
        /// <param name="filedNameInfoDic">用户自定义字段集合</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private bool CreateFeatureClass(IFeatureWorkspace feaworkspace, IFeatureClass orgFeaCls, string desFeaClsName, List<IField> orgFieldList, Dictionary<string, string> filedNameInfoDic, out Exception eError)
        {
            eError = null;
            try
            {

                //遍历字段信息，创建字段结构


                IFields fields = new FieldsClass();
                IFieldsEdit fsEdit = fields as IFieldsEdit;
                IField geoField = null;//定义几何字段；



                //创建用户自定义的字段（普通字段）
                for (int i = 0; i < orgFieldList.Count; i++)
                {
                    if (orgFieldList[i].Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        geoField = orgFieldList[i];
                    }
                    if (!filedNameInfoDic.ContainsKey(orgFieldList[i].Name)) continue;

                    IField desField = GetNormalField(orgFieldList[i], filedNameInfoDic[orgFieldList[i].Name], out eError);
                    if (eError != null)
                    {
                        return false;
                    }
                    fsEdit.AddField(desField);
                }

                if (geoField == null)
                {
                    eError = new Exception("获取源几何字段失败！");
                    return false;
                }

                //创建特殊字段
                if (orgFeaCls.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    //注记
                    //int pScale = 500;
                    if (!CreateAnnoFeatureClass(feaworkspace, fsEdit, desFeaClsName, geoField, out eError))
                    {
                        return false;
                    }
                }
                else
                {
                    # region 创建普通featureClass的特殊字段


                    //添加Object字段
                    IField objfield = new FieldClass();
                    objfield = GetObjectField(out eError);
                    if (eError != null)
                    {
                        return false;
                    }
                    fsEdit.AddField(objfield);

                    //添加Geometry字段
                    IField desGeofield = new FieldClass();
                    desGeofield = GetGeometryField(geoField, out eError);
                    if (eError != null)
                    {
                        return false;
                    }
                    fsEdit.AddField(desGeofield);

                    fields = fsEdit as IFields;

                    #endregion
                    feaworkspace.CreateFeatureClass(desFeaClsName, fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                }
                return true;
            }
            catch
            {
                eError = new Exception("创建图层结构失败！");
                return false;
            }
        }

        /// <summary>
        /// 创建注记层


        /// </summary>
        /// <param name="feaworkspace"></param>
        /// <param name="fsEditAnno">字段集合</param>
        /// <param name="desFeaName">目标图层名</param>
        /// <param name="orgGeoField">源几何字段</param>
        /// <param name="intScale">比例尺</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private bool CreateAnnoFeatureClass(IFeatureWorkspace feaworkspace, IFieldsEdit fsEditAnno, string desFeaName, IField orgGeoField, out Exception eError)
        {
            eError = null;
            //创建注记的特殊字段


            try
            {
                int intScale = Convert.ToInt32(v_Scale);
                //注记的workSpace
                IFeatureWorkspaceAnno pFWSAnno = feaworkspace as IFeatureWorkspaceAnno;
                if (pFWSAnno == null)
                {
                    eError = new Exception("创建注记层失败！");
                    return false;
                }
                IGraphicsLayerScale pGLS = new GraphicsLayerScaleClass();
                pGLS.Units = esriUnits.esriMeters;
                pGLS.ReferenceScale = Convert.ToDouble(intScale);//创建注记必须要设置比例尺

                IFormattedTextSymbol myTextSymbol = new TextSymbolClass();
                ISymbol pSymbol = (ISymbol)myTextSymbol;
                //Anno要素类必须有的缺省符号


                ISymbolCollection2 pSymbolColl = new SymbolCollectionClass();
                ISymbolIdentifier2 pSymID = new SymbolIdentifierClass();
                pSymbolColl.AddSymbol(pSymbol, "Default", out pSymID);

                //Anno要素类的必要属性


                IAnnotateLayerProperties pAnnoProps = new LabelEngineLayerPropertiesClass();
                pAnnoProps.CreateUnplacedElements = true;
                pAnnoProps.CreateUnplacedElements = true;
                pAnnoProps.DisplayAnnotation = true;
                pAnnoProps.UseOutput = true;

                ILabelEngineLayerProperties pLELayerProps = (ILabelEngineLayerProperties)pAnnoProps;
                pLELayerProps.Symbol = pSymbol as ITextSymbol;
                pLELayerProps.SymbolID = 0;
                pLELayerProps.IsExpressionSimple = true;
                pLELayerProps.Offset = 0;
                pLELayerProps.SymbolID = 0;

                IAnnotationExpressionEngine aAnnoVBScriptEngine = new AnnotationVBScriptEngineClass();
                pLELayerProps.ExpressionParser = aAnnoVBScriptEngine;
                pLELayerProps.Expression = "[DESCRIPTION]";
                IAnnotateLayerTransformationProperties pATP = (IAnnotateLayerTransformationProperties)pAnnoProps;
                pATP.ReferenceScale = pGLS.ReferenceScale;
                pATP.ScaleRatio = 1;

                IAnnotateLayerPropertiesCollection pAnnoPropsColl = new AnnotateLayerPropertiesCollectionClass();
                pAnnoPropsColl.Add(pAnnoProps);

                IObjectClassDescription pOCDesc = new AnnotationFeatureClassDescription();
                IFields fields = pOCDesc.RequiredFields;
                IFeatureClassDescription pFDesc = pOCDesc as IFeatureClassDescription;

                for (int j = 0; j < pOCDesc.RequiredFields.FieldCount; j++)
                {
                    IField tempField = pOCDesc.RequiredFields.get_Field(j);
                    if (tempField.Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        continue;
                    }
                    fsEditAnno.AddField(tempField);
                }
                //根据xml文件，Geometry字段可能带有空间参考，因此单独添加Geometry字段
                //添加Geometry字段
                IField newfield1 = new FieldClass();
                newfield1 = GetGeometryField(orgGeoField, out eError);
                if (eError != null)
                {
                    return false;
                }

                fsEditAnno.AddField(newfield1);

                fields = fsEditAnno as IFields;
                pFWSAnno.CreateAnnotationClass(desFeaName, fields, pOCDesc.InstanceCLSID, pOCDesc.ClassExtensionCLSID, pFDesc.ShapeFieldName, "", null, null, pAnnoPropsColl, pGLS, pSymbolColl, true);
            }
            catch
            {
                eError = new Exception("创建注记层失败！");
                return false;
            }
            return true;
        }

        #endregion

        #region  数据入库
        /// <summary>
        /// 用户自定义属性字段配对（注记层须做特殊处理，注记层的必须字段须赋值）
        /// </summary>
        /// <param name="orgFieldLst"></param>
        /// <param name="orgFeaCls"></param>
        /// <param name="desFeaCls"></param>
        /// <param name="fNamePair"></param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private Dictionary<int, int> GetFIndexPair(List<IField> orgFieldLst, IFeatureClass orgFeaCls, IFeatureClass desFeaCls, Dictionary<string, string> fNamePair, out Exception eError)
        {
            eError = null;
            try
            {
                Dictionary<int, int> indexPairDic = new Dictionary<int, int>();
                for (int i = 0; i < orgFieldLst.Count; i++)
                {
                    if (!fNamePair.ContainsKey(orgFieldLst[i].Name)) continue;
                    string desFName = fNamePair[orgFieldLst[i].Name];//目标字段名


                    int orgIndex = orgFeaCls.Fields.FindField(orgFieldLst[i].Name);//源子段索引


                    int desIndex = desFeaCls.Fields.FindField(desFName);//目标字段索引
                    if (orgIndex == -1 || desIndex == -1)
                    {
                        eError = new Exception("源字段或目标字段不存在！");
                        return null;
                    }
                    if (!indexPairDic.ContainsKey(desIndex))
                    {
                        indexPairDic.Add(desIndex, orgIndex);
                    }
                }
                if (orgFeaCls.FeatureType == esriFeatureType.esriFTAnnotation)
                {
                    //注记的必须字段加上 
                    IObjectClassDescription pOCDesc = new AnnotationFeatureClassDescription();
                    IFields fields = pOCDesc.RequiredFields;
                    for (int j = 0; j < fields.FieldCount; j++)
                    {
                        IField mField = fields.get_Field(j);
                        if (mField.Type == esriFieldType.esriFieldTypeGeometry || mField.Type == esriFieldType.esriFieldTypeOID) continue;
                        if (mField.Editable == false) continue;

                        string fieldName = mField.Name;
                        int oIndex = orgFeaCls.Fields.FindField(fieldName);
                        int dIndex = desFeaCls.Fields.FindField(fieldName);
                        if (oIndex == -1 || dIndex == -1)
                        {
                            eError = new Exception("源字段或目标字段不存在！");
                            return null;
                        }
                        if (!indexPairDic.ContainsKey(dIndex))
                        {
                            indexPairDic.Add(dIndex, oIndex);
                        }
                    }
                }
                return indexPairDic;
            }
            catch (Exception ex)
            {
                eError = ex;
                return null;
            }
        }

        /// <summary>
        /// 数据入库
        /// </summary>
        /// <param name="orgFeaCls"></param>
        /// <param name="desFeaCls"></param>
        /// <param name="indexPair">索引对</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private bool DataImport(IFeatureClass orgFeaCls, string whereStr, IGeometry pGeo, bool beClip, IFeatureClass desFeaCls, Dictionary<int, int> indexPair, out Exception eError)
        {
            eError = null;
            try
            {
                ISpatialFilter pFilter = new SpatialFilterClass();
                pFilter.WhereClause = whereStr;
                if (pGeo != null)
                {
                    pFilter.Geometry = pGeo;
                    pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;
                }
                IFeatureCursor orgCusor = orgFeaCls.Search(pFilter, false);

                if (orgCusor == null) return false;
                IFeature orgFeature = orgCusor.NextFeature();
                //遍历源要素类的要素，给目标要素赋值


                while (orgFeature != null)
                {
                    IFeature desFeature = desFeaCls.CreateFeature();

                    //给用户自定义的字段赋值


                    foreach (KeyValuePair<int, int> indexItem in indexPair)
                    {
                        int desIndex = indexItem.Key;//目标字段索引
                        int orgIndex = indexItem.Value;//源字段索引


                        if (orgFeature.get_Value(orgIndex).ToString() != "")
                        {
                            try
                            {
                                desFeature.set_Value(desIndex, orgFeature.get_Value(orgIndex));
                            }
                            catch
                            { }
                        }

                    }
                    //给特殊字段赋值（shape）


                    IGeometry newGeo = orgFeature.Shape;
                    IGeometry outGeo = null;
                    if (beClip&&pGeo!=null)
                    {
                        //切割
                        ITopologicalOperator pTopo = newGeo as ITopologicalOperator;
                        //若为提取，则求差
                        if (newGeo.GeometryType == esriGeometryType.esriGeometryPoint)
                        {
                            outGeo = pTopo.Intersect(pGeo, esriGeometryDimension.esriGeometry0Dimension);
                        }
                        else if (newGeo.GeometryType == esriGeometryType.esriGeometryPolyline)
                        {
                            outGeo = pTopo.Intersect(pGeo, esriGeometryDimension.esriGeometry1Dimension);
                        }
                        else if (newGeo.GeometryType == esriGeometryType.esriGeometryPolygon)
                        {
                            outGeo = pTopo.Intersect(pGeo, esriGeometryDimension.esriGeometry2Dimension);
                        }
                        ITopologicalOperator pTopo2 = outGeo as ITopologicalOperator;
                        pTopo2.Simplify();
                        outGeo = pTopo2 as IGeometry;
                    }
                    else
                    {
                        //不切割


                        outGeo = newGeo;
                    }
                    desFeature.Shape = outGeo;

                    desFeature.Store();
                    orgFeature = orgCusor.NextFeature();
                }

                //释放cursor
                System.Runtime.InteropServices.Marshal.ReleaseComObject(orgCusor);
                return true;
            }
            catch (Exception ex)
            {
                eError = ex;
                return false;
            }
        }

        #endregion
        /// <summary>
        /// 判断是否存在着相应比利尺的图副
        /// </summary>
        /// <param name="MapPath">图副路径</param>
        /// <param name="StrScale">比例尺</param>
        /// <returns></returns>
        private string GetMapFrameName(string MapPath, string StrScale)
        {
            IWorkspaceFactory pWorkSpaceFactory = new AccessWorkspaceFactoryClass();
            IWorkspace pWorkSpace = null;
            IEnumDataset pEnumDataSet = null;
            IDataset pDataSet = null;
            IFeatureDataset pFeatureDataSet = null;
            IEnumDataset pEnumFC = null;
            IDataset pFC = null;
            string LayerName = "";
            string MapFrameName = "";
            if (MapPath != "")
            {
                pWorkSpace = pWorkSpaceFactory.OpenFromFile(MapPath, 0);
                pEnumDataSet = pWorkSpace.get_Datasets(esriDatasetType.esriDTFeatureClass);
                pEnumDataSet.Reset();
                pDataSet = pEnumDataSet.Next();
                while (pDataSet != null)
                {
                    if (pDataSet is IFeatureClass)
                    {
                        LayerName = pDataSet.Name;
                        if (LayerName.Contains(StrScale))
                        {
                            MapFrameName = LayerName;
                        }
                    }
                    else if (pDataSet is IFeatureDataset)
                    {
                        pFeatureDataSet = pDataSet as IFeatureDataset;
                        pEnumFC = pFeatureDataSet.Subsets;
                        pEnumFC.Reset();
                        pFC = pEnumFC.Next();
                        while (pFC != null)
                        {
                            LayerName = pFC.Name;
                            if (LayerName.Contains(StrScale))
                            {
                                MapFrameName = LayerName;
                            }
                        }
                    }
                    else
                    {
                        MapFrameName = "";
                    }
                    pDataSet = pEnumDataSet.Next();
                }
            }
            return MapFrameName;
        }

        /// <summary>
        /// 获得MapControl上所有的图层
        /// </summary>
        /// <param name="mHook"></param>
        /// <returns></returns>
        private List<IFeatureLayer> GetFeaLayerFromMap(Plugin.Application.IAppGISRef mHook)
        {
            List<IFeatureLayer> feaLayers = new List<IFeatureLayer>();
            for (int i = 0; i < mHook.MapControl.LayerCount; i++)
            {
                ILayer mLayer = mHook.MapControl.get_Layer(i);
                if (mLayer is IGroupLayer)
                {
                    //将栅格数据排除

                    if (mLayer.Name == "栅格数据库") continue;
                    ICompositeLayer pComLayer = mLayer as ICompositeLayer;
                    for (int k = 0; k < pComLayer.Count; k++)
                    {
                        ILayer pLayer = pComLayer.get_Layer(k);
                        IFeatureLayer pfeaLayer = pLayer as IFeatureLayer;
                        if (pfeaLayer != null)
                        {
                            if (!feaLayers.Contains(pfeaLayer))
                            {
                                feaLayers.Add(pfeaLayer);
                            }
                        }
                    }
                }
                else
                {
                    IFeatureLayer mFeaLayer = mLayer as IFeatureLayer;
                    if (mFeaLayer != null)
                    {
                        if (!feaLayers.Contains(mFeaLayer))
                        {
                            feaLayers.Add(mFeaLayer);
                        }
                    }
                }
            }
            return feaLayers;
        }

        /// <summary>
        /// 从控件上获得指定名称的要素类
        /// </summary>
        /// <param name="feaClsName"></param>
        /// <returns></returns>
        private IFeatureClass GetFeaLayerFromMap(string feaClsName)
        {
            IFeatureClass pFeaCls = null;
            for (int i = 0; i < _Hook.MapControl.LayerCount; i++)
            {
                ILayer mLayer = _Hook.MapControl.get_Layer(i);
                if (mLayer is IGroupLayer)
                {
                    ICompositeLayer pComLayer = mLayer as ICompositeLayer;
                    for (int k = 0; k < pComLayer.Count; k++)
                    {
                        ILayer pLayer = pComLayer.get_Layer(k);
                        if (pLayer.Name == feaClsName)
                        {
                            IFeatureLayer pFeaLayer = pLayer as IFeatureLayer;
                            if (pFeaLayer == null) continue;
                            pFeaCls = pFeaLayer.FeatureClass;
                            return pFeaCls;
                        }
                    }
                }
                else
                {
                    if (mLayer.Name == feaClsName)
                    {
                        IFeatureLayer mFeaLayer = mLayer as IFeatureLayer;
                        if (mFeaLayer == null) continue;
                        pFeaCls = mFeaLayer.FeatureClass;
                        return pFeaCls;
                    }
                }
            }
            return pFeaCls;

        }

        /// <summary>
        /// 获得要素类的字段信息
        /// </summary>
        /// <param name="mFeaCls"></param>
        /// <returns></returns>
        private List<IField> GetFieldInfo(IFeatureClass mFeaCls)
        {
            if (mFeaCls == null) return null;
            List<IField> fieldInfoList = new List<IField>();//字段信息集合
            for (int j = 0; j < mFeaCls.Fields.FieldCount; j++)
            {
                IField pField = mFeaCls.Fields.get_Field(j);
                switch (pField.Type)
                {
                    case esriFieldType.esriFieldTypeSmallInteger:
                    case esriFieldType.esriFieldTypeInteger:
                    case esriFieldType.esriFieldTypeSingle:
                    case esriFieldType.esriFieldTypeDouble:
                    case esriFieldType.esriFieldTypeString:
                    case esriFieldType.esriFieldTypeDate:
                    case esriFieldType.esriFieldTypeOID:
                    case esriFieldType.esriFieldTypeGeometry:
                    case esriFieldType.esriFieldTypeBlob:
                        if (!fieldInfoList.Contains(pField))
                        {
                            fieldInfoList.Add(pField);
                        }
                        break;
                    default:
                        break;
                }
            }
            return fieldInfoList;
        }

        /// <summary>
        /// 获得总范围


        /// </summary>
        /// <param name="dgView"></param>
        /// <returns></returns>
        private IGeometry GetUnionGeo(DevComponents.DotNetBar.Controls.DataGridViewX dgView)
        {
            IGeometry UnionGeo = null;
            for (int i = 0; i < dgView.RowCount; i++)
            {
                if (dgView.Rows[i].Cells[0].FormattedValue.ToString() == "") continue;
                bool b = Convert.ToBoolean(dgView.Rows[i].Cells[0].FormattedValue.ToString());
                if (b)
                {
                    IGeometry pGeo = dgView.Rows[i].Cells[1].Tag as IGeometry;
                    if (pGeo == null) continue;
                    if (UnionGeo == null)
                    {
                        UnionGeo = pGeo;
                    }
                    else
                    {
                        ITopologicalOperator pTop = UnionGeo as ITopologicalOperator;
                        UnionGeo = pTop.Union(pGeo);
                        pTop.Simplify();
                    }
                }
            }
            return UnionGeo;
        }

        /// <summary>
        /// 序列化(将对象序列化成字符串)
        /// </summary>
        /// <param name="xmlByte">序列化字节</param>
        /// <param name="obj">序列化对象</param>
        /// <returns></returns>
        public static byte[] xmlSerializer(object obj)
        {
            try
            {
                byte[] xmlByte = null;//保存序列化后的字节


                //判断是否支持IPersistStream接口,只有支持该接口的对象才能进行序列化


                if (obj is ESRI.ArcGIS.esriSystem.IPersistStream)
                {
                    ESRI.ArcGIS.esriSystem.IPersistStream pStream = obj as ESRI.ArcGIS.esriSystem.IPersistStream;

                    ESRI.ArcGIS.esriSystem.IXMLStream xmlStream = new ESRI.ArcGIS.esriSystem.XMLStreamClass();

                    pStream.Save(xmlStream as ESRI.ArcGIS.esriSystem.IStream, 0);

                    xmlByte = xmlStream.SaveToBytes();
                }
                return xmlByte;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 将xmlByte解析为obj
        /// </summary>
        /// <param name="xmlByte"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool XmlDeSerializer(byte[] xmlByte, object obj)
        {
            try
            {
                //判断字符串是否为空


                if (xmlByte != null)
                {
                    ESRI.ArcGIS.esriSystem.IPersistStream pStream = obj as ESRI.ArcGIS.esriSystem.IPersistStream;

                    ESRI.ArcGIS.esriSystem.IXMLStream xmlStream = new ESRI.ArcGIS.esriSystem.XMLStreamClass();

                    xmlStream.LoadFromBytes(ref xmlByte);
                    pStream.Load(xmlStream as ESRI.ArcGIS.esriSystem.IStream);

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private void btnDelMap_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dvRow in dgMap.SelectedRows)
            {
                dgMap.Rows.Remove(dvRow);
            }
            dgMap.Update();
        }

        private void btnDelRange_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dvRow in dgCounty.SelectedRows)
            {
                dgCounty.Rows.Remove(dvRow);
            }
            dgCounty.Update();
        }

        private void btnDelDraw_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dvRow in dgRange.SelectedRows)
            {
                dgRange.Rows.Remove(dvRow);
            }
            dgRange.Update();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //将设置条件保存起来


            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = "保存过滤条件";
            saveDialog.Filter = "xml文件(*.xml)|*.xml";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                string saveFileName = saveDialog.FileName;

                //声明一个xml的文件，用来保存设置
                XmlDocument saveXmlDoc = new XmlDocument();

                string xmlStr = "<过滤条件><属性条件></属性条件><空间条件></空间条件></过滤条件>";
                saveXmlDoc.LoadXml(xmlStr);

                #region 添加属性条件的字节点


                //遍历属性条件表格


                for (int i = 0; i < dgPropertiesSetting.Rows.Count - 1; i++)
                {
                    bool b = Convert.ToBoolean(dgPropertiesSetting.Rows[i].Cells["是否导出"].FormattedValue.ToString());
                    string orgFeaClsName = dgPropertiesSetting.Rows[i].Cells["源图层名"].FormattedValue.ToString();
                    IFeatureClass orgFeaCls = dgPropertiesSetting.Rows[i].Cells["源图层名"].Tag as IFeatureClass;
                    if (orgFeaCls == null) continue;
                    string desFeaClsName = dgPropertiesSetting.Rows[i].Cells["目标图层名"].FormattedValue.ToString();
                    string whereStr = dgPropertiesSetting.Rows[i].Cells["属性表达式"].FormattedValue.ToString();

                    Dictionary<string, string> fieldNameDic = dgPropertiesSetting.Rows[i].Cells["字段对应关系"].Tag as Dictionary<string, string>;
                    if (fieldNameDic == null)
                    {
                        //采用源字段


                        #region 将字段对应关系保存起来


                        fieldNameDic = new Dictionary<string, string>();
                        List<IField> mFieldList = dgPropertiesSetting.Rows[i].Cells["属性表达式"].Tag as List<IField>;
                        if (mFieldList == null) return;
                        for (int j = 0; j < mFieldList.Count; j++)
                        {
                            //源字段名
                            string orgFieldName = mFieldList[j].Name;
                            if (orgFieldName == "") return;

                            #region 将特殊的字段排除掉


                            if (mFieldList[j].Type == esriFieldType.esriFieldTypeGeometry) continue;
                            if (mFieldList[j].Type == esriFieldType.esriFieldTypeOID) continue;
                            if (mFieldList[j].Editable == false) continue;
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
                            if (!fieldNameDic.ContainsKey(orgFieldName))
                            {
                                fieldNameDic.Add(orgFieldName, orgFieldName);
                            }
                            #endregion
                        }
                        if (fieldNameDic == null)
                        {
                            return;
                        }
                        dgPropertiesSetting.Rows[i].Cells["字段对应关系"].Tag = fieldNameDic;
                        #endregion
                    }
                    //创建"条件"子节点


                    XmlNode codiNode = saveXmlDoc.CreateElement("条件");
                    XmlNode proCodiNode = saveXmlDoc.SelectSingleNode(".//过滤条件//属性条件");
                    proCodiNode.AppendChild(codiNode);
                    XmlElement codiElem = codiNode as XmlElement;
                    codiElem.SetAttribute("是否导出", b.ToString());
                    codiElem.SetAttribute("源数据", orgFeaClsName);
                    codiElem.SetAttribute("目标数据", desFeaClsName);
                    codiElem.SetAttribute("属性条件", whereStr);

                    //遍历字段对应管关系子节点
                    foreach (KeyValuePair<string, string> fieldItem in fieldNameDic)
                    {
                        string orgFieldName = fieldItem.Key;         //源字段名
                        string desFieldName = fieldItem.Value;     //目标字段名



                        //创建字段对应关系子节点


                        XmlNode fieldDicNode = saveXmlDoc.CreateElement("字段对应关系");
                        codiNode.AppendChild(fieldDicNode);
                        XmlElement fieldDicElem = fieldDicNode as XmlElement;
                        fieldDicElem.SetAttribute("是否导出", "true");
                        fieldDicElem.SetAttribute("源字段", orgFieldName);
                        fieldDicElem.SetAttribute("目标字段", desFieldName);
                    }
                }
                #endregion

                #region 添加空间条件子节点


                bool cMap = checkBoxMap.Checked;
                bool cCounty = checkBoxCounty.Checked;
                bool cGeo = checkBoxGeo.Checked;
                bool cClip = checkBoxClip.Checked;

                //创建空间条件子节点


                XmlNode boolNode1 = saveXmlDoc.CreateElement("按图幅提取");
                XmlNode boolNode2 = saveXmlDoc.CreateElement("按行政区提取");
                XmlNode boolNode3 = saveXmlDoc.CreateElement("任意多边形提取");
                XmlNode boolNode4 = saveXmlDoc.CreateElement("是否切割");
                boolNode1.InnerText = cMap.ToString();
                boolNode2.InnerText = cCounty.ToString();
                boolNode3.InnerText = cGeo.ToString();
                boolNode4.InnerText = cClip.ToString();
                XmlNode spaialNode = saveXmlDoc.SelectSingleNode(".//过滤条件//空间条件");
                spaialNode.AppendChild(boolNode1);
                spaialNode.AppendChild(boolNode2);
                spaialNode.AppendChild(boolNode3);
                spaialNode.AppendChild(boolNode4);
                #region  按范围提取


                if (cMap)
                {
                    //按图幅提取


                    XmlNode mapNode = saveXmlDoc.CreateElement("图幅");
                    spaialNode.AppendChild(mapNode);
                    //创建图幅的子节点
                    for (int i = 0; i < dgMap.Rows.Count; i++)
                    {
                        bool b = Convert.ToBoolean(dgMap.Rows[i].Cells[0].FormattedValue.ToString());
                        string mapName = dgMap.Rows[i].Cells[1].FormattedValue.ToString();
                        IGeometry pGeo = dgMap.Rows[i].Cells[1].Tag as IGeometry;
                        if (pGeo == null) continue;
                        byte[] btGeo = xmlSerializer(pGeo);
                        string base64String = Convert.ToBase64String(btGeo);

                        XmlElement mapGeoElem = saveXmlDoc.CreateElement("条件");
                        mapGeoElem.SetAttribute("是否选用", b.ToString());
                        mapGeoElem.SetAttribute("范围名称", mapName);
                        mapGeoElem.SetAttribute("几何范围", base64String);
                        mapNode.AppendChild(mapGeoElem);
                    }
                }
                if (cCounty)
                {
                    //按行政区提取
                    XmlNode mapNode = saveXmlDoc.CreateElement("行政区");
                    spaialNode.AppendChild(mapNode);
                    //创建图幅的子节点
                    for (int i = 0; i < dgCounty.Rows.Count; i++)
                    {
                        bool b = Convert.ToBoolean(dgCounty.Rows[i].Cells[0].FormattedValue.ToString());
                        string mapName = dgCounty.Rows[i].Cells[1].FormattedValue.ToString();
                        IGeometry pGeo = dgCounty.Rows[i].Cells[1].Tag as IGeometry;
                        if (pGeo == null) continue;
                        byte[] btGeo = xmlSerializer(pGeo);
                        string base64String = Convert.ToBase64String(btGeo);

                        XmlElement mapGeoElem = saveXmlDoc.CreateElement("条件");
                        mapGeoElem.SetAttribute("是否选用", b.ToString());
                        mapGeoElem.SetAttribute("范围名称", mapName);
                        mapGeoElem.SetAttribute("几何范围", base64String);
                        mapNode.AppendChild(mapGeoElem);
                    }
                }
                if (cClip)
                {
                    //按任意多边形提取
                    XmlNode mapNode = saveXmlDoc.CreateElement("任意多边形");
                    spaialNode.AppendChild(mapNode);
                    //创建图幅的子节点
                    for (int i = 0; i < dgRange.Rows.Count; i++)
                    {
                        bool b = Convert.ToBoolean(dgRange.Rows[i].Cells[0].FormattedValue.ToString());
                        string mapName = dgRange.Rows[i].Cells[1].FormattedValue.ToString();
                        IGeometry pGeo = dgRange.Rows[i].Cells[1].Tag as IGeometry;
                        if (pGeo == null) continue;
                        byte[] btGeo = xmlSerializer(pGeo);
                        string base64String = Convert.ToBase64String(btGeo);

                        XmlElement mapGeoElem = saveXmlDoc.CreateElement("条件");
                        mapGeoElem.SetAttribute("是否选用", b.ToString());
                        mapGeoElem.SetAttribute("范围名称", mapName);
                        mapGeoElem.SetAttribute("几何范围", base64String);
                        mapNode.AppendChild(mapGeoElem);
                    }
                }
                #endregion
                #endregion

                saveXmlDoc.Save(saveFileName);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Title = "选择上传的数据";
            OpenFile.Filter = "过滤条件配置文件(*.xml)|*.xml";

            OpenFile.Multiselect = false;
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                string fileName = OpenFile.FileName;
                XmlDocument configDoc = new XmlDocument();
                configDoc.Load(fileName);

                //清空列表
                dgPropertiesSetting.Rows.Clear();
                dgMap.Rows.Clear();
                dgCounty.Rows.Clear();
                dgRange.Rows.Clear();

                #region 属性条件还原


                XmlNodeList propeNodeList = configDoc.SelectNodes(".//过滤条件//属性条件//条件");
                if (propeNodeList == null || propeNodeList.Count == 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择正确的条件设置文件！");
                    return;
                }
                foreach (XmlNode condiNode in propeNodeList)
                {
                    bool bOut;                                                                  //是否导出
                    string orgFeaClsName;                                                       //源图层名
                    string desFeaClsName;                                                       //目标图层名


                    string CondiStr;                                                            //属性表达式条件
                    IFeatureClass orgFeaCls = null;                                             //源要素类
                    Dictionary<string, string> fieldDic = new Dictionary<string, string>();     //字段对应关系
                    List<IField> fieldLst = new List<IField>();
                    //字段列表
                    if (condiNode == null) return;

                    XmlElement condiElem = condiNode as XmlElement;
                    bOut = Convert.ToBoolean(condiElem.GetAttribute("是否导出").ToString().Trim());
                    orgFeaClsName = condiElem.GetAttribute("源数据").ToString().Trim();
                    desFeaClsName = condiElem.GetAttribute("目标数据").ToString().Trim();
                    CondiStr = condiElem.GetAttribute("属性条件").ToString().Trim();

                    //字段对应关系
                    XmlNodeList fieldNodeLst = condiNode.SelectNodes(".//字段对应关系");
                    foreach (XmlNode fieldNode in fieldNodeLst)
                    {
                        XmlElement fieldElem = fieldNode as XmlElement;
                        string orgFieldName = fieldElem.GetAttribute("源字段");
                        string desFieldName = fieldElem.GetAttribute("目标字段");
                        if (!fieldDic.ContainsKey(orgFieldName))
                        {
                            fieldDic.Add(orgFieldName, desFieldName);
                        }
                    }
                    if (fieldDic == null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "导入文件出错！");
                        return;
                    }

                    //获得源要素图层名
                    orgFeaCls = GetFeaLayerFromMap(orgFeaClsName);
                    if (orgFeaCls == null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请加载图层名为：'" + orgFeaClsName + "'的图层！");
                        return;
                    }

                    //获得源要素字段列表


                    fieldLst = GetFieldInfo(orgFeaCls);
                    if (fieldLst == null) return;


                    //添加行


                    int rowindex = 0;
                    DataGridViewRow dgRow = new DataGridViewRow();
                    dgRow.CreateCells(dgPropertiesSetting);
                    //第一列


                    dgRow.Cells[0].Value = rowindex + 1;  //序号
                    //第二列


                    dgRow.Cells[1].Value = bOut;// 是否导出
                    //第三列


                    dgRow.Cells[2].Value = orgFeaClsName;  //源图层名
                    dgRow.Cells[2].Tag = orgFeaCls;//源要素类
                    //第四列


                    dgRow.Cells[3].Value = desFeaClsName;  //目标图层名


                    //第五列


                    dgRow.Cells[4].Value = CondiStr;  //属性表达式
                    dgRow.Cells[4].Tag = fieldLst;//存储字段信息
                    //第六列


                    dgRow.Cells[5].Value = "设置";
                    dgRow.Cells[5].Tag = fieldDic;
                    dgPropertiesSetting.Rows.Add(dgRow);
                    rowindex++;
                }
                dgPropertiesSetting.Update();
                #endregion

                #region 空间条件还原

                XmlNode spatialNode = configDoc.SelectSingleNode(".//过滤条件//空间条件");
                if (spatialNode == null)
                {
                    return;
                }
                bool bMap;                                      //按图幅提取


                bool bCounty;                                   //按行政区提取
                bool bRange;                                    //按任意多边形提取
                bool bClip;                                     //是否切割
                bool bselect;                                   //是否选用
                string geoName;                                 //范围名称
                IGeometry pGeo = new PolygonClass();             //几何范围

                XmlNode mapNode = spatialNode.SelectSingleNode(".//按图幅提取");
                XmlNode countyNode = spatialNode.SelectSingleNode(".//按行政区提取");
                XmlNode rangeNode = spatialNode.SelectSingleNode(".//任意多边形提取");
                XmlNode clipNode = spatialNode.SelectSingleNode(".//是否切割");
                if (mapNode == null || countyNode == null || rangeNode == null || clipNode == null)
                {
                    return;
                }

                bMap = Convert.ToBoolean(mapNode.InnerText);
                bCounty = Convert.ToBoolean(countyNode.InnerText);
                bRange = Convert.ToBoolean(rangeNode.InnerText);
                bClip = Convert.ToBoolean(clipNode.InnerText);
                checkBoxMap.Checked = bMap;
                checkBoxCounty.Checked = bCounty;
                checkBoxGeo.Checked = bRange;
                checkBoxClip.Checked = bClip;
                if (bMap)
                {
                    XmlNodeList mapNodeLst = spatialNode.SelectNodes(".//图幅//条件");
                    foreach (XmlNode mNode in mapNodeLst)
                    {
                        XmlElement mElem = mNode as XmlElement;
                        bselect = Convert.ToBoolean(mElem.GetAttribute("是否选用").ToString().Trim());
                        geoName = mElem.GetAttribute("范围名称").ToString().Trim();
                        string pGeoStr = mElem.GetAttribute("几何范围").ToString().Trim();
                        byte[] xmlByte = Convert.FromBase64String(pGeoStr);
                        if (XmlDeSerializer(xmlByte, pGeo) == false)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "解析几何范围出错！");
                            return;
                        }

                        //添加行


                        DataGridViewRow dgRow = new DataGridViewRow();
                        dgRow.CreateCells(dgMap);
                        //第一列


                        dgRow.Cells[0].Value = bselect;// 是否选用
                        //第二列


                        dgRow.Cells[1].Value = geoName;
                        //第二列


                        dgRow.Cells[1].Tag = pGeo;
                        dgMap.Rows.Add(dgRow);
                    }
                    dgMap.Update();
                }
                if (bCounty)
                {
                    XmlNodeList mapNodeLst = spatialNode.SelectNodes(".//行政区//条件");
                    foreach (XmlNode mNode in mapNodeLst)
                    {
                        XmlElement mElem = mNode as XmlElement;
                        bselect = Convert.ToBoolean(mElem.GetAttribute("是否选用").ToString().Trim());
                        geoName = mElem.GetAttribute("范围名称").ToString().Trim();
                        string pGeoStr = mElem.GetAttribute("几何范围").ToString().Trim();
                        byte[] xmlByte = Convert.FromBase64String(pGeoStr);
                        if (XmlDeSerializer(xmlByte, pGeo) == false)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "解析几何范围出错！");
                            return;
                        }

                        //添加行


                        DataGridViewRow dgRow = new DataGridViewRow();
                        dgRow.CreateCells(dgCounty);
                        //第一列


                        dgRow.Cells[0].Value = bselect;// 是否选用
                        //第二列


                        dgRow.Cells[1].Value = geoName;
                        //第二列


                        dgRow.Cells[1].Tag = pGeo;
                        dgCounty.Rows.Add(dgRow);
                    }
                    dgCounty.Update();
                }
                if (bRange)
                {
                    XmlNodeList mapNodeLst = spatialNode.SelectNodes(".//任意多边形//条件");
                    foreach (XmlNode mNode in mapNodeLst)
                    {
                        XmlElement mElem = mNode as XmlElement;
                        bselect = Convert.ToBoolean(mElem.GetAttribute("是否选用").ToString().Trim());
                        geoName = mElem.GetAttribute("范围名称").ToString().Trim();
                        string pGeoStr = mElem.GetAttribute("几何范围").ToString().Trim();
                        byte[] xmlByte = Convert.FromBase64String(pGeoStr);
                        if (XmlDeSerializer(xmlByte, pGeo) == false)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "解析几何范围出错！");
                            return;
                        }

                        //添加行


                        DataGridViewRow dgRow = new DataGridViewRow();
                        dgRow.CreateCells(dgRange);
                        //第一列


                        dgRow.Cells[0].Value = bselect;// 是否选用
                        //第二列


                        dgRow.Cells[1].Value = geoName;
                        //第二列


                        dgRow.Cells[1].Tag = pGeo;
                        dgRange.Rows.Add(dgRow);
                    }
                    dgRange.Update();
                }
                #endregion
            }
        }

        private void dgMap_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
           
        }

        private void dgPropertiesSetting_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemSelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgPropertiesSetting.RowCount; i++)
            {
                dgPropertiesSetting.Rows[i].Cells[1].Value = true;
            }
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemSelRef_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgPropertiesSetting.RowCount; i++)
            {
                if (dgPropertiesSetting.Rows[i].Cells[1].FormattedValue == null)
                    dgPropertiesSetting.Rows[i].Cells[1].Value = true;
                else
                {
                    if (Convert.ToBoolean(dgPropertiesSetting.Rows[i].Cells[1].FormattedValue.ToString()) == true)
                        dgPropertiesSetting.Rows[i].Cells[1].Value = false;
                    else
                        dgPropertiesSetting.Rows[i].Cells[1].Value = true;
                }
            }
        }

        /// <summary>
        /// 清除选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemSelClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgPropertiesSetting.RowCount; i++)
            {
                dgPropertiesSetting.Rows[i].Cells[1].Value = false;
            }
        }

        private void cmbDataFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cyf 20110628 add
            if (cmbDataFormat.Text == "ESRI文件数据库(*.gdb)")
            {
                cmbDataFormat.Tag = "GDB";
            }
            else if (cmbDataFormat.Text == "ESRI个人数据库(*.mdb)")
            {
                cmbDataFormat.Tag = "PDB";
            }
            //end
        }

        private void FrmDataStract_SizeChanged(object sender, EventArgs e)
        {
            //cyf 20110706 add
            for (int j = 0; j < dgPropertiesSetting.Columns.Count; j++)
            {
                dgPropertiesSetting.Columns[j].Width = (dgPropertiesSetting.Width - 20) / dgPropertiesSetting.Columns.Count;
            }
            dgPropertiesSetting.RowHeadersWidth = 20;
            //end
        }
    }
}