using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using System.IO;

namespace GeoSysUpdate
{
    public partial class FrmQueryAll : DevComponents.DotNetBar.Office2007Form
    {
        //private Plugin.Application.IAppGISRef _Hook;
        private Plugin.Application.IAppArcGISRef _Hook;
        private Dictionary<string, ILayer> m_cunrrentLayerNames;//当前地图中的图层名

        private Dictionary<string, string> m_pDicFieldname;//存放用户选中的图层的字段
        private Dictionary<string, string> m_pDBDicFieldName;//存放全库的字段
        private Dictionary<string, ILayer> m_Fcnames = new Dictionary<string, ILayer>();//用于存放图层中数据的地物类名字。
        private List<IDataset> m_LstDataset = null;
        private IMap m_Map = null;
        public bool mSuceed{set;get;}

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
        public FrmQueryAll(Plugin.Application.IAppArcGISRef mHook)
        {
            InitializeComponent();
            mSuceed = false;
            _Hook = mHook;
            if (_Hook == null) return;
            IMap mMap = mHook.MapControl.Map;
            if (mMap == null) return;
            m_Map = mMap;
            Exception eError = null;
            InitialDB(out eError);
            if (m_LstDataset == null || m_LstDataset.Count==0)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                return;
            }

            IntialComBox(mMap);
            IntialGrid();
            mSuceed = true;

        }

        

        /// <summary>
        /// 初始化数据库连接
        /// </summary>
        private void InitialDB(out Exception err)
        {
            err = null;
            //cyf 20110625 modify
            List<IDataset> lstDataset = new List<IDataset>();
            XmlDocument xml = new XmlDocument();
            if(!File.Exists(ModData.v_projectDetalXML))
            {
                err = new Exception(ModData.v_projectDetalXML+"不存在");
                return ;
            }
            xml.Load(ModData.v_projectDetalXML);
            XmlElement elementTemp = xml.SelectSingleNode("//现势库/连接信息[@类型!='']") as XmlElement;//取得矢量数据
            //end
            IWorkspace TempWorkSpace = ModDBOperator.GetDBInfoByXMLNode(elementTemp, "") as IWorkspace;
            if (TempWorkSpace == null)
            {
                err = new Exception("连接数据库失败!");
                return;
            }
            XmlElement elementTemp2 = xml.SelectSingleNode("//现势库/数据集[@名称!='']") as XmlElement;
            string datasetname = elementTemp2.GetAttribute("名称");
            SysCommon.Gis.SysGisDataSet sysGisDataset = new SysCommon.Gis.SysGisDataSet(TempWorkSpace);
            //cyf 20110625 modify:
            IFeatureDataset dataset = sysGisDataset.GetFeatureDataset(datasetname,out err);
            //end
            if (err != null)
            {
                err = new Exception("获取数据失败!");
                return ;
            }
            
                lstDataset = sysGisDataset.GetFeatureClass(dataset);
                if (lstDataset == null || lstDataset.Count == 0)
                {
                    err = new Exception("库体要素类为空！");
                }
                m_LstDataset = lstDataset;
             


    }
        /// <summary>
        /// 初始化下拉列表框
        /// </summary>
        /// <param name="mMap"></param>
        private void IntialComBox(IMap mMap)
        {

            //初始化匹配方式下拉列表框
            cmbMatch.Items.Clear();
            cmbMatch.Items.AddRange(new object[] { "完全匹配", "首字母匹配", "任意匹配" });
            cmbMatch.SelectedIndex = 0;
            //初始化检索范围下拉列表框
            cmbRange.Items.Clear();
            cmbRange.Items.AddRange(new object[] { "全数据库", "当前地图", "可见图层" });
            //初始化图层名称

            m_cunrrentLayerNames = new Dictionary<string, ILayer>();
            for (int i = 0; i < mMap.LayerCount; i++)
            {
                ILayer pLayer = mMap.get_Layer(i);
                if (pLayer is IGroupLayer)
                {
                    ICompositeLayer pComLayer = pLayer as ICompositeLayer;
                    for (int k = 0; k < pComLayer.Count; k++)
                    {
                        ILayer mLayer = pComLayer.get_Layer(k);
                        IDataset mDataset = mLayer as IDataset;
                        if (!(pLayer is IFeatureLayer))
                            continue;
                        if (!m_cunrrentLayerNames.ContainsKey(mDataset.Name))
                        {
                            m_cunrrentLayerNames.Add(mDataset.Name, mLayer);
                            m_Fcnames.Add(((pLayer as IFeatureLayer).FeatureClass as IDataset).Name,mLayer);
                            cmbRange.Items.Add(mDataset.Name);
                        }
                    }
                }
                if (pLayer is IFeatureLayer)
                {
                    IDataset pDataset = pLayer as IDataset;
                    if (!m_cunrrentLayerNames.ContainsKey(pDataset.Name))
                    {
                        m_cunrrentLayerNames.Add(pDataset.Name, pLayer);
                        m_Fcnames.Add(((pLayer as IFeatureLayer).FeatureClass as IDataset).Name,pLayer);
                        cmbRange.Items.Add(pDataset.Name);
                    }

                }
            }
            if (mMap.LayerCount > 0)
            {
                cmbRange.SelectedIndex = 1;
            }
            else
            {
                cmbRange.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 初始化DataGrid
        /// </summary>
        private void IntialGrid()
        {
            dgResult.Columns.Add("字段值", "字段值");
            dgResult.Columns.Add("图层名称", "图层名称");
            dgResult.Columns.Add("字段名称", "字段名称");

            DataGridViewCheckBoxColumn checkBoxCol = new DataGridViewCheckBoxColumn();
            checkBoxCol.Name = "是否位于当前地图";
            checkBoxCol.HeaderText = "是否位于当前地图";
            dgResult.Columns.Add(checkBoxCol);
            dgResult.Columns.Add("OID", "OID");
            dgResult.Columns.Add("工作区路径", "工作区路径");
            dgResult.Columns.Add("数据集名称", "数据集名称");
            //cyf 20110705 modify:检索结果显示
            dgResult.Columns[4].Visible = true;
            dgResult.Columns[5].Visible = true;
            dgResult.Columns[6].Visible = false;
            //end
            dgResult.ReadOnly = true;
            dgResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            for (int j = 0; j < dgResult.Columns.Count-3; j++)  //cyf 20110705 modify
            {
                dgResult.Columns[j].Width = (dgResult.Width - 20) / (dgResult.Columns.Count-3);
            }
            dgResult.RowHeadersWidth = 20;

        }

        /// <summary>
        /// 获得图层的字段

        /// </summary>
        /// <param name="pFeaCls"></param>
        /// <param name="isAddField"></param>
        /// <param name="isDB"></param>
        private void GetFieldofOneLayer(IFeatureClass pFeaCls, bool isAddField, bool isDB)
        {
            IFields pFields = pFeaCls.Fields;
            for (int i = 0; i < pFields.FieldCount; i++)
            {
                IField pField = pFields.get_Field(i);
                if (pField.Type != esriFieldType.esriFieldTypeBlob && pField.Type != esriFieldType.esriFieldTypeGeometry && pField.Type != esriFieldType.esriFieldTypeRaster)
                {
                    if (!m_pDicFieldname.ContainsKey(pField.Name))
                    {
                        //添加图层字段
                        m_pDicFieldname.Add(pField.Name, pField.AliasName);
                        if (isAddField)
                        {
                            if (pField.Name.ToUpper() == "SHAPE.LEN" || pField.Name.ToUpper() == "SHAPE.AREA")//不查自动生成的字段 xisheng 20110914
                                continue;
                            cmbField.Items.Add(pField.Name);
                        }
                    }
                    if (isDB)
                    {
                        //添加全库字段
                        if (!m_pDBDicFieldName.ContainsKey(pField.Name))
                        {
                            m_pDBDicFieldName.Add(pField.Name, pField.AliasName);
                        }
                    }

                }
            }
        }
        /// <summary>
        /// 初始化字段下拉列表框
        /// </summary>
        /// <param name="feaRange"></param>
        /// <param name="isAddField"></param>
        private void GetFields(string feaRange, bool isAddField)
        {
            m_pDicFieldname = new Dictionary<string, string>();
            if (feaRange == "全数据库")
            {
                #region 加载全数据库的字段

                if (m_pDBDicFieldName == null || m_pDBDicFieldName.Count == 0)
                {
                    m_pDBDicFieldName = new Dictionary<string, string>();
                    if (m_LstDataset != null)
                    {
                        foreach (IDataset pDataset in m_LstDataset)
                        {
                            IFeatureClass pfeaCls = pDataset as IFeatureClass;
                            if (pfeaCls != null)
                            {
                                GetFieldofOneLayer(pfeaCls, isAddField, true);
                            }
                        }
                    }
                }
                else
                {
                    m_pDicFieldname = m_pDBDicFieldName;
                    if (isAddField)
                    {
                        foreach (KeyValuePair<string, string> fieldname in m_pDBDicFieldName)
                        {
                            if (fieldname.Key.ToUpper() == "SHAPE.LEN" || fieldname.Key.ToUpper() == "SHAPE.AREA")//不查自动生成的字段 xisheng 20110914
                                continue;
                            cmbField.Items.Add(fieldname.Key);
                        }
                    }
                }
                #endregion
            }
            else if (feaRange == "当前地图")
            {
                #region 加载当前地图的字段

                if (m_cunrrentLayerNames != null)
                {
                    foreach (KeyValuePair<string, ILayer> currentLayer in m_cunrrentLayerNames)
                    {
                        ILayer pLayer = currentLayer.Value;
                        IFeatureLayer pFeaLayer = pLayer as IFeatureLayer;
                        if (pFeaLayer != null)
                        {
                            IFeatureClass mFeaCls = pFeaLayer.FeatureClass;
                            GetFieldofOneLayer(mFeaCls, isAddField, false);
                        }
                    }
                }
                #endregion
            }
            else if (feaRange == "可见图层")
            {
                #region 加载可见图层的字段

                if (m_cunrrentLayerNames != null)
                {
                    foreach (KeyValuePair<string, ILayer> currentLayer in m_cunrrentLayerNames)
                    {
                        ILayer pLayer = currentLayer.Value;
                        IFeatureLayer pFeaLayer = pLayer as IFeatureLayer;
                        if (pFeaLayer != null)
                        {
                            if (pFeaLayer.Visible)
                            {
                                IFeatureClass mFeaCls = pFeaLayer.FeatureClass;
                                GetFieldofOneLayer(mFeaCls, isAddField, false);
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region 加载特定的某个图层

                if (m_cunrrentLayerNames.ContainsKey(feaRange))
                {
                    ILayer mLayer = m_cunrrentLayerNames[feaRange];
                    IFeatureLayer nFeaLayer = mLayer as IFeatureLayer;
                    if (nFeaLayer != null)
                    {
                        IFeatureClass nFeaCls = nFeaLayer.FeatureClass;
                        GetFieldofOneLayer(nFeaCls, isAddField, false);
                    }
                }
                #endregion

            }
        }

        /// <summary>
        /// 获得图层的所有字段的查询条件
        /// </summary>
        /// <param name="pFeaCls">要查询的要素类</param>
        /// <param name="pMatchType">匹配类型</param>
        /// <param name="pValue">值</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private string GetSQL(IFeatureClass pFeaCls, string pMatchType, string pValue, out Exception eError)
        {
            eError = null;
            string pSQL = "";                  //查询语句
            string pMatchChar = "";          //匹配通配符


            if (m_pDicFieldname != null)
            {
                IDataset pDataset = pFeaCls as IDataset;
                if (pDataset.Workspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
                {
                    pMatchChar = "%";
                }
                else
                {
                    pMatchChar = "*";
                }
                #region 查询特定图层的所有字段信息

                foreach (KeyValuePair<string, string> fieldInfo in m_pDicFieldname)
                {
                    string fieldName = fieldInfo.Key;
                    int index = pFeaCls.Fields.FindField(fieldName);
                    if (index != -1)
                    {
                        if (pSQL == "")
                        {
                            switch (pMatchType)
                            {
                                case "完全匹配":
                                    pSQL = fieldName + " like '" + pValue + "'";
                                    break;
                                case "首字母匹配":
                                    pSQL = fieldName + " like '" + pValue + pMatchChar + "'";
                                    break;
                                case "任意匹配":
                                    pSQL = fieldName + " like '" + pMatchChar + pValue + pMatchChar + "'";
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (pMatchType)
                            {
                                case "完全匹配":
                                    pSQL = pSQL + " or (" + fieldName + " like '" + pValue + "')";
                                    break;
                                case "首字母匹配":
                                    pSQL = pSQL + " or (" + fieldName + " like '" + pValue + pMatchChar + "')";
                                    break;
                                case "任意匹配":
                                    pSQL = pSQL + " or (" + fieldName + " like '" + pMatchChar + pValue + pMatchChar + "')";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                #endregion
            }
            return pSQL;
        }

        /// <summary>
        /// 获取图层指定字段的查询条件

        /// </summary>
        /// <param name="pFeaCls">要查询的图层</param>
        /// <param name="pMatchType">匹配类型</param>
        /// <param name="pValue">值</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="eError"></param>
        /// <returns></returns>
        private string GetSQL(IFeatureClass pFeaCls, string pMatchType, string pValue, string fieldName)
        {
            string pSQL = "";
            string pMatchChar = "";          //匹配通配符

            IDataset pDataset = pFeaCls as IDataset;
            if (pDataset.Workspace.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
            {
                //sde
                pMatchChar = "%";
            }
            else if (pDataset.Workspace.Type == esriWorkspaceType.esriLocalDatabaseWorkspace)
            {
                //pdb.gdb
                pMatchChar = "*";
            }
            else if (pDataset.Workspace.Type == esriWorkspaceType.esriFileSystemWorkspace)
            {
                //Shapefiles and ArcInfo
                pMatchChar = "%";
            }


            int index = pFeaCls.Fields.FindField(fieldName);
            if (index != -1)
            {
                try
                {
                    if (pDataset.Workspace.WorkspaceFactory.GetClassID().Value.ToString() == "{71FE75F0-EA0C-4406-873E-B7D53748AE7E}")
                    {
                        //File Geodatabase Workspace 
                        IField pField = pFeaCls.Fields.get_Field(index);
                        if (pField.Type != esriFieldType.esriFieldTypeString)
                        {
                            //GDB只能完全匹配。比较特殊

                            if (pField.Type != esriFieldType.esriFieldTypeString)
                            {
                                fieldName = "CAST (\"" + fieldName + "\" AS character)";
                            }

                        }

                    }
                    else
                    {
                        //Personal Geodatabase = "{DD48C96A-D92A-11D1-AA81-00C04FA33A15}" 

                    }

                    switch (pMatchType)
                    {
                        case "完全匹配":
                            pSQL = fieldName + " like '" + pValue + "'";
                            break;
                        case "首字母匹配":

                            pSQL = fieldName + " like '" + pValue + pMatchChar + "'";

                            break;
                        case "任意匹配":

                            pSQL = fieldName + " like '" + pMatchChar + pValue + pMatchChar + "'";

                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
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

                }
            }
            return pSQL;
        }

        /// <summary>
        /// 将结果填充到表格中

        /// </summary>
        /// <param name="pFeaCls"></param>
        /// <param name="pValue"></param>
        /// <param name="pMatchType"></param>
        /// <param name="fieldName"></param>
        /// <param name="bCurrent"></param>
        /// <param name="eError"></param>
        private long FillResultToGrid(IFeatureClass pFeaCls, string pValue, string pMatchType, string fieldName, bool bCurrent, out Exception eError)
        {
            eError = null;
            long pCount = 0;
            try
            {
                string pSQL = "";
                IDataset pDataset = pFeaCls as IDataset;
                //****************************************************************
                //guozheng 2010-11-9 处理SDE图层带用户名的问题
                string SlayerName = string.Empty;
                IWorkspace getWs = pFeaCls.FeatureDataset.Workspace;
                //if (getWs.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
                //{
                //    try
                //    {
                //        SlayerName = pDataset.Name.Substring(pDataset.Name.LastIndexOf('.') + 1);
                //    }
                //    catch
                //    {
                //        SlayerName = pDataset.Name;
                //    }
                //}
                //else
                //{
                SlayerName = pDataset.Name;
                //}
                //****************************************************************

                int index = pFeaCls.Fields.FindField(fieldName);
                if (index != -1)
                {
                    pSQL = GetSQL(pFeaCls, pMatchType, pValue, fieldName);
                    if (eError != null) return pCount;

                    IQueryFilter pFilter = new QueryFilterClass();
                    pFilter.WhereClause = pSQL;
                    //IFeatureLayer pFeaLayer = new FeatureLayerClass();
                    //pFeaLayer.FeatureClass = pFeaCls;
                    IFeatureCursor pCusor = pFeaCls.Search(pFilter, false);
                    if (pCusor == null) return pCount;
                    IFeature pFea = pCusor.NextFeature();
                    labelX5.Text = "正在搜索图层" + pDataset.Name + "  ......";
                    labelX5.Update();
                    while (pFea != null)
                    {
                        //将查出来的结果添加在GRID中

                        DataGridViewRow aRow = new DataGridViewRow();
                        aRow.CreateCells(dgResult);
                        aRow.Cells[0].Value = pFea.get_Value(index).ToString();
                        aRow.Cells[1].Value = SlayerName;
                        aRow.Cells[2].Value = fieldName;
                        aRow.Cells[3].Value = bCurrent;
                        aRow.Cells[4].Value = pFea.OID;
                        aRow.Cells[5].Value = pDataset.Workspace.PathName;
                        aRow.Cells[6].Value = "";
                        dgResult.Rows.Add(aRow);
                        dgResult.Update();
                        pFea = pCusor.NextFeature();
                        pCount++;
                    }
                    Marshal.ReleaseComObject(pCusor);
                }
                return pCount;
            }
            catch (Exception ex)
            {
                //*******************************************************************
                //guozheng added
                if (ModData.SysLog != null)
                {
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                else
                {
                    ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                    ModData.SysLog.Write(ex, null, DateTime.Now);
                }
                //********************************************************************
                eError = ex;
                return pCount;
            }
        }

        private void cmbRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbField.Items.Clear();
            cmbField.Items.Add("所有字段");
            string feaRange = cmbRange.SelectedItem.ToString().Trim();
            GetFields(feaRange, true);
            cmbField.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //GDB数据只进行了完全匹配搜索

            dgResult.Rows.Clear();
            Exception eError = null;
            string pMatchType = "";          //匹配类型
            string pValue = "";              //值

            string pRange = "";               //检索范围

            string fieldName = "";              //字段名


            pMatchType = cmbMatch.Text.Trim();
            pValue = txtValue.Text.Trim();
            fieldName = cmbField.Text.Trim();
            if (pValue == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请填写检索值！");
                return;
            }
            pRange = cmbRange.Text.Trim();
            //*****************************************************
            //guozheng added 系统运行日志
            List<string> Pra = new List<string>();
            Pra.Add(this.txtValue.Text);
            Pra.Add(this.cmbRange.Text);
            Pra.Add(this.cmbMatch.Text);
            Pra.Add(this.cmbField.Text);
            if (ModData.SysLog != null)
            {
                ModData.SysLog.Write("数据检索", Pra, DateTime.Now);
            }
            else
            {
                ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModData.SysLog.Write("数据检索", Pra, DateTime.Now);
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog(this.Text + ",检索值为:" + txtValue.Text + ",检索范围:" + pRange);//xisheng 日志记录 0928;
            }
            //*****************************************************
            labelX5.Text = "正在搜索  ......";
            labelX5.Update();
            long totalCount = 0;
            if (pRange == "全数据库")
            {
                #region 检索全库要素类
                foreach (IDataset pDataset in m_LstDataset)
                {
                    IFeatureClass pFeaCls = pDataset as IFeatureClass; 
                    bool bCurrent = true;   //是否当前图层
                    if (m_Fcnames.ContainsKey(pDataset.Name))
                    {
                        bCurrent = true;
                    }
                    else
                    {
                        bCurrent = false;

                    }
                    if (fieldName == "所有字段")
                    {
                        foreach (KeyValuePair<string, string> pName in m_pDBDicFieldName)
                        {
                            string fName = pName.Key;
                            if (fName.ToUpper() == "SHAPE.LEN" || fName.ToUpper() == "SHAPE.AREA")//不查自动生成的字段 xisheng 20110914
                                continue;
                            long pCount = FillResultToGrid(pFeaCls, pValue, pMatchType, fName, bCurrent, out eError);
                            if (eError != null)
                            {
                                //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                //return;
                                continue;
                            }
                            totalCount += pCount;
                        }
                    }
                    else
                    {
                        long pCount = FillResultToGrid(pFeaCls, pValue, pMatchType, fieldName, bCurrent, out eError);
                        if (eError != null)
                        {
                            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                            //return;
                            continue;
                        }
                        totalCount += pCount;
                    }
                }
                #endregion
            }
            else if (pRange == "当前地图")
            {
                #region 检索当前地图

                foreach (KeyValuePair<string, ILayer> lItem in m_cunrrentLayerNames)
                {
                    ILayer pLayer = lItem.Value;
                    IFeatureLayer pFeaLayer = pLayer as IFeatureLayer;
                    if (pFeaLayer != null)
                    {
                        IFeatureClass mFeaCls = pFeaLayer.FeatureClass;
                        if (fieldName == "所有字段")
                        {
                            foreach (KeyValuePair<string, string> pName in m_pDicFieldname)
                            {
                                string fName = pName.Key;
                                if (fName.ToUpper() == "SHAPE.LEN" || fName.ToUpper() == "SHAPE.AREA")//不查自动生成的字段 xisheng 20110914
                                    continue;
                                long pCount = FillResultToGrid(mFeaCls, pValue, pMatchType, fName, true, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    //return;
                                    continue;
                                }
                                totalCount += pCount;
                            }
                        }
                        else
                        {
                            long pCount = FillResultToGrid(mFeaCls, pValue, pMatchType, fieldName, true, out eError);
                            if (eError != null)
                            {
                                //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                //return;
                                continue;
                            }
                            totalCount += pCount;
                        }
                    }
                }
                #endregion
            }
            else if (pRange == "可见图层")
            {
                #region 检索可见地图

                foreach (KeyValuePair<string, ILayer> lItem in m_cunrrentLayerNames)
                {
                    ILayer pLayer = lItem.Value;
                    IFeatureLayer pFeaLayer = pLayer as IFeatureLayer;
                    if (pFeaLayer != null && pFeaLayer.Visible)
                    {
                        IFeatureClass mFeaCls = pFeaLayer.FeatureClass;
                        if (fieldName == "所有字段")
                        {
                            foreach (KeyValuePair<string, string> pName in m_pDicFieldname)
                            {
                                string fName = pName.Key;
                                if (fName.ToUpper() == "SHAPE.LEN" || fName.ToUpper() == "SHAPE.AREA")//不查自动生成的字段 xisheng 20110914
                                    continue;
                                long pCount = FillResultToGrid(mFeaCls, pValue, pMatchType, fName, true, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    //return;
                                    continue;
                                }
                                totalCount += pCount;
                            }
                        }
                        else
                        {
                            long pCount = FillResultToGrid(mFeaCls, pValue, pMatchType, fieldName, true, out eError);
                            if (eError != null)
                            {
                                //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                //return;
                                continue;
                            }
                            totalCount += pCount;
                        }

                    }
                }
                #endregion
            }
            else
            {
                #region 检索指定图层

                if (m_cunrrentLayerNames.ContainsKey(pRange))
                {
                    ILayer pLayer = m_cunrrentLayerNames[pRange];
                    IFeatureLayer pFeaLayer = pLayer as IFeatureLayer;
                    if (pFeaLayer != null)
                    {
                        IFeatureClass pFeaCls = pFeaLayer.FeatureClass;
                        if (fieldName == "所有字段")
                        {
                            foreach (KeyValuePair<string, string> pName in m_pDicFieldname)
                            {
                                string fName = pName.Key;
                                if (fName.ToUpper() == "SHAPE.LEN" || fName.ToUpper() == "SHAPE.AREA")//不查自动生成的字段 xisheng 20110914
                                    continue;
                                long pCount = FillResultToGrid(pFeaCls, pValue, pMatchType, fName, true, out eError);
                                if (eError != null)
                                {
                                    //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                    return;
                                }
                                totalCount += pCount;
                            }
                        }
                        else
                        {
                            long pCount = FillResultToGrid(pFeaCls, pValue, pMatchType, fieldName, true, out eError);
                            if (eError != null)
                            {
                                //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                                return;
                            }
                            totalCount += pCount;
                        }
                    }
                }
                #endregion
            }
            labelX5.Text = "搜索完毕。共搜索到" + totalCount + "个要素！";
            labelX5.Update();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgResult_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //双击定位
            DevComponents.DotNetBar.Controls.DataGridViewX dtView = new DevComponents.DotNetBar.Controls.DataGridViewX();
            dtView = sender as DevComponents.DotNetBar.Controls.DataGridViewX;
            if (dtView.SelectedRows.Count == 1)
            {
                string feaClsName = dtView.SelectedRows[0].Cells["图层名称"].FormattedValue.ToString();
                if(!m_Fcnames.ContainsKey(feaClsName))//if (!m_cunrrentLayerNames.ContainsKey(feaClsName))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "图层" + feaClsName + "并未加载，无法进行定位，请检查！");
                    return;
                }
                else
                {
                    ILayer pLayer = m_Fcnames[feaClsName];
                    int pOID = Convert.ToInt32(dtView.SelectedRows[0].Cells["OID"].FormattedValue.ToString());
                    if (pOID <= 0) return;
                    try
                    {
                        IFeatureLayer pFeatLay = pLayer as IFeatureLayer;
                        if (pFeatLay == null) return;
                        IFeatureClass pFeatCls = pFeatLay.FeatureClass;
                        IQueryFilter pQueryFilter = new QueryFilterClass();
                        pQueryFilter.WhereClause = "OBJECTID=" + pOID;
                        IFeatureCursor pFeatCursor = pFeatCls.Search(pQueryFilter, false);
                        IFeature pFeature = pFeatCursor.NextFeature();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatCursor);
                        if (pFeature == null) return;
                        IGeoDataset pGeoDt = pFeatCls as IGeoDataset;
                        ISpatialReference pSpatialRef = null;
                        if (pGeoDt != null)
                        {
                            pSpatialRef = pGeoDt.SpatialReference;
                        }
                        _Hook.MapControl.Map.ClearSelection();
                        _Hook.MapControl.Map.SelectFeature(pLayer, pFeature);
                        SysCommon.Gis.ModGisPub.ZoomToFeature(_Hook.MapControl, pFeature, pSpatialRef);
                    }
                    catch (Exception er)
                    {
                        //*******************************************************************
                        //guozheng added
                        if (ModData.SysLog != null)
                        {
                            ModData.SysLog.Write(er, null, DateTime.Now);
                        }
                        else
                        {
                            ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                            ModData.SysLog.Write(er, null, DateTime.Now);
                        }
                        //********************************************************************

                    }
                }
            }
        }
    }
}