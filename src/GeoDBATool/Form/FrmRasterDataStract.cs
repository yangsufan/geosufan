using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.SpatialAnalyst;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.GeoAnalyst;

namespace GeoDBATool
{
    /// <summary>
    /// 栅格数据提取。 陈亚飞编写

    /// 源数据分为：栅格编目数据提取、栅格数据集提取；

    /// 目标数据分为：geodatabase型（栅格编目、栅格数据集）、文件型（例如：tif、img)
    /// 目标数据若为geodatabase，则存储类型（栅格编目、栅格数据集）与源数据相同；步骤为：创建库体、入库

    /// 目标数据若为文件型，统一转化为栅格数据集进行操作，用ISaveAs接口
    /// </summary>
    public partial class FrmRasterDataStract : DevComponents.DotNetBar.Office2007Form
    {

        private Plugin.Application.IAppGISRef _Hook;
        //IWorkspace m_WorkSpace = null;//源栅格数据工作空间

        string m_dbType = "";           //栅格数据存储类型：栅格编目、栅格数据集
        string m_DesRasterCollName = "";//栅格数据集名称  
        ILayer m_Layer = null;          //栅格图层

        private string v_Scale = "";
        public FrmRasterDataStract(Plugin.Application.IAppGISRef pHook, ILayer pLayer)
        {
            InitializeComponent();

            _Hook = pHook;
            m_Layer = pLayer;
            if (m_Layer == null) return;

            //初始化界面控件

            cmbDataFormat.Items.AddRange(new object[] { "TIF", "IMG" });//"PDB","GDB",
            cmbDataFormat.SelectedIndex = 0;

            cmbScale.Items.AddRange(new object[] { "500", "1000", "2000", "5000", "10000", "20000" });
            cmbScale.SelectedIndex = 0;

            checkBoxClip.Checked = true;

            v_Scale = cmbScale.SelectedItem.ToString().Trim();

            //初始化图幅选择列表
            IntialDgMap();

            //初始化行政区列表
            IntialDgCounty();

            //初始化任意多边形列表
            IntialDgRange();


            //Exception eError = null;
            ///获得树图上选择的工程节点

            DevComponents.AdvTree.Node pCurNode = _Hook.ProjectTree.SelectedNode;
            //cyf 20110608 modify
            //string pProjectname = pCurNode.Name;
            string pProjectname = pCurNode.Text;
            System.Xml.XmlNode Projectnode = _Hook.DBXmlDocument.SelectSingleNode("工程管理/工程[@名称='" + pProjectname + "']");
            //获得栅格数据库类型

            System.Xml.XmlElement DbTypeElem = Projectnode.SelectSingleNode(".//栅格数据库") as System.Xml.XmlElement;
            m_dbType = DbTypeElem.GetAttribute("存储类型");   //栅格编目、栅格数据集
            if (m_dbType == "栅格数据集")
            {
                txtWhereStr.Enabled = false;
                btnAttrSetting.Enabled = false;
            }
            else
            {
                txtWhereStr.Enabled = true;
                btnAttrSetting.Enabled = true;
            }

            //获得栅格目录，栅格数据集名称
            System.Xml.XmlElement DbcataLogNameElem = Projectnode.SelectSingleNode(".//栅格数据库/连接信息/库体") as System.Xml.XmlElement;
            if (DbcataLogNameElem == null) return;
            m_DesRasterCollName = DbcataLogNameElem.GetAttribute("名称");    //栅格数据名称
            if (m_DesRasterCollName == "") return;
        }


        #region 初始化函数

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

        //设置行政区范围

        private void btnSelCounty_Click(object sender, EventArgs e)
        {
            string pScale = v_Scale;
            FrmCountySheet pFrmCountySheet = new FrmCountySheet(pScale, "行政区范围选择");

            if (pFrmCountySheet.ShowDialog() == DialogResult.OK)
            {
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

        private void btnDelRange_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dvRow in dgCounty.SelectedRows)
            {
                dgCounty.Rows.Remove(dvRow);
            }
            dgCounty.Update();
        }

        //设置图幅范围
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
        }

        private void btnDelMap_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dvRow in dgMap.SelectedRows)
            {
                dgMap.Rows.Remove(dvRow);
            }
            dgMap.Update();
        }

        //画范围 
        private void btnSelRange_Click(object sender, EventArgs e)
        {
            DrawPolygonToolClass drawPolygon = new DrawPolygonToolClass(true, this);
            drawPolygon.OnCreate(_Hook.MapControl);
            _Hook.MapControl.CurrentTool = drawPolygon as ITool;
        }

        private void btnDelDraw_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dvRow in dgRange.SelectedRows)
            {
                dgRange.Rows.Remove(dvRow);
            }
            dgRange.Update();
        }

        //设置属性条件，针对栅格编目数据
        private void btnAttrSetting_Click(object sender, EventArgs e)
        {
            try
            {
                IFeatureLayer mFealayer = m_Layer as IFeatureLayer;
                if (mFealayer == null) return;
                IFeatureClass pFeaCls = mFealayer.FeatureClass as IFeatureClass;
                if (pFeaCls == null) return;
                //属性表达式集合
                List<IField> fieldList = new List<IField>();//字段信息集合
                //获取所有的字段信息
                fieldList = GetFieldInfo(pFeaCls);
                FrmSQLQuery pFrmSQLQuery = new FrmSQLQuery(pFeaCls, fieldList);
                if (pFrmSQLQuery.ShowDialog() == DialogResult.OK)
                {
                    txtWhereStr.Text = pFrmSQLQuery.WhereClause;
                }
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
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "打开栅格编目数据出错！");
                return;
            }

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Exception eError = null;
            //界面控制判断和检查


            if (txtSavePath.Text == "")
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请选择保存路径！");
                return;
            }
            //栅格数据文件名称
            string pFileName = txtSavePath.Text.Trim();
            pFileName = pFileName.Substring(pFileName.LastIndexOf('\\') + 1);
            //目标栅格数据存储类型
            string pFormat = cmbDataFormat.Text.Trim().ToUpper();

            #region 创建目标工作空间
            IWorkspace pDesWS = null;//定义工作空间

            //如果存在同名库体则删除


            if (File.Exists(txtSavePath.Text.Trim()))
            {
                File.Delete(txtSavePath.Text.Trim());
            }
            if (cmbDataFormat.Text.Trim().ToUpper() == "PDB")
            {
                pDesWS = CreateWorkspace(txtSavePath.Text.Trim(), "PDB", out eError);

            }
            else if (cmbDataFormat.Text.Trim().ToUpper() == "GDB")
            {
                pDesWS = CreateWorkspace(txtSavePath.Text.Trim(), "GDB", out eError);
            }
            else if (cmbDataFormat.Text.Trim().ToUpper() == "TIF")
            {
                pDesWS = CreateWorkspace(txtSavePath.Text.Trim(), "tif", out eError);
            }
            else if (cmbDataFormat.Text.Trim().ToUpper() == "IMG")
            {
                pDesWS = CreateWorkspace(txtSavePath.Text.Trim(), "img", out eError);
            }
            if (eError != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建工作空间失败！");
                return;
            }
            if (pDesWS == null) return;

            #endregion


            //IExtractionOp extraction = new RasterExtractionOpClass();
            IGeometry clipExtent = null;       //几何限制条件
            #region 获得几何限制条件
            IGeometry pGeo1 = null;
            IGeometry pGeo2 = null;
            IGeometry pGeo3 = null;

            pGeo1 = GetUnionGeo(dgMap);

            pGeo2 = GetUnionGeo(dgCounty);
            pGeo3 = GetUnionGeo(dgRange);
            ITopologicalOperator pTop = clipExtent as ITopologicalOperator;
            if (pGeo1 != null)
            {
                if (clipExtent == null)
                {
                    clipExtent = pGeo1;
                }
                else
                {
                    clipExtent = pTop.Union(pGeo1);
                }

            }
            pTop = clipExtent as ITopologicalOperator;
            if (pGeo2 != null)
            {
                if (clipExtent == null)
                {
                    clipExtent = pGeo2;
                }
                else
                {
                    clipExtent = pTop.Union(pGeo2);
                }

            }
            pTop = clipExtent as ITopologicalOperator;
            if (pGeo3 != null)
            {
                if (clipExtent == null)
                {
                    clipExtent = pGeo3;
                }
                else
                {
                    clipExtent = pTop.Union(pGeo3);
                }

            }
            if (clipExtent != null)
            {
                pTop = clipExtent as ITopologicalOperator;
                pTop.Simplify();
                clipExtent = pTop as IGeometry;
            }
            #endregion
            try
            {
                #region 提取数据
                IRaster pOrgRaster = null;           //源栅格数据



                if (m_dbType == "栅格数据集")
                {
                    //源栅格数据图层

                    IRasterLayer mOrgRasterlyer = m_Layer as IRasterLayer;
                    if (mOrgRasterlyer == null) return;

                    //源栅格数据集
                    pOrgRaster = mOrgRasterlyer.Raster;
                    if (pOrgRaster == null) return;
                    //栅格数据集提取

                    StractData(pFileName, clipExtent, pDesWS, pFormat, pOrgRaster, checkBoxClip.Checked, out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                        return;
                    }
                }
                else
                {
                    //源数据为栅格数据编目，步骤入下：
                    //(1) 创建临时的工作空间;
                    //(2) 将符合属性条件的栅格数据拼接成栅格数据集后，存储在临时的工作空间里面
                    //(3) 将临时的工作空间里面的栅格数据集案范围提取出来

                    //=======================================================================================

                    #region 栅格编目数据提取

                    //（1）创建临时工作空间

                    IWorkspace tempWS = CreateWorkspace(ModData.temporaryDBPath, "GDB", out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "创建临时的工作空间出错！");
                        return;
                    }
                    if (tempWS == null) return;

                    //（2）遍历源栅格数据，将数据入库到临时工作空间里面

                    //源栅格数据图层

                    IFeatureLayer mOrgFeaLyer = m_Layer as IFeatureLayer;
                    if (mOrgFeaLyer == null) return;
                    IRasterDataset pResRDataset = null;     //最后生成的栅格数据集


                    IFeatureClass pOrgFeaCls = mOrgFeaLyer.FeatureClass;
                    IQueryFilter pFilter = new QueryFilterClass();
                    pFilter.WhereClause = txtWhereStr.Text.Trim();
                    IFeatureCursor pFeaCursor = pOrgFeaCls.Search(pFilter, false);
                    if (pFeaCursor == null) return;
                    IFeature pORgFea = pFeaCursor.NextFeature();
                    #region 遍历源栅格数据，进行源栅格数据入库（过滤和拼接）
                    while (pORgFea != null)
                    {
                        IRasterCatalogItem pOrgRCItem = pORgFea as IRasterCatalogItem;
                        IRasterDataset pOrgRasterDt = pOrgRCItem.RasterDataset;
                        if (pResRDataset == null)
                        {
                            //第一个数据入库直接将源数据拷贝过去

                            if (pOrgRasterDt.CanCopy())
                            {
                                pResRDataset = pOrgRasterDt.Copy(m_DesRasterCollName, tempWS) as IRasterDataset;
                                pORgFea = pFeaCursor.NextFeature();
                                continue;
                            }
                        }
                        //从第二个栅格数据开始，进行入库和拼接

                        IRaster pOrgRast = pOrgRasterDt.CreateDefaultRaster();
                        IRasterLoader pRasterLoad = new RasterLoaderClass();
                        if (pOrgRast != null)
                        {
                            pRasterLoad.Background = 0;     //background value be ignored when loading
                            pRasterLoad.PixelAlignmentTolerance = 0;     //重采样的容差
                            pRasterLoad.MosaicColormapMode = rstMosaicColormapMode.MM_LAST;  //拼接的颜色采用 last map color

                            pRasterLoad.Load(pResRDataset, pOrgRast);
                        }
                        Marshal.ReleaseComObject(pRasterLoad);

                        pORgFea = pFeaCursor.NextFeature();
                    }

                    //释放cursor 
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);
                    #endregion

                    //（3) 将临时工作空间里面的栅格数据集提取出来

                    if (pResRDataset == null) return;
                    pOrgRaster = pResRDataset.CreateDefaultRaster();
                    StractData(pFileName, clipExtent, pDesWS, pFormat, pOrgRaster, checkBoxClip.Checked, out eError);
                    if (eError != null)
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                        return;
                    }
                    #endregion
                }
                #endregion
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "操作完成！");
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
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "提取栅格数据失败！\n" + er.Message);
                return;
            }
            this.Close();
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            if (cmbDataFormat.Text.ToUpper() == "PDB")
            {
                saveDialog.Title = "保存ESRI个人数据库";
                saveDialog.Filter = "ESRI个人数据库(*.mdb)|*.mdb";
            }
            else if (cmbDataFormat.Text.ToUpper() == "GDB")
            {
                saveDialog.Title = "保存ESRI文件数据库";
                saveDialog.Filter = "ESRI文件数据库(*.gdb)|*.gdb";
            }
            else if (cmbDataFormat.Text.ToUpper() == "TIF")
            {
                saveDialog.Title = "保存tif数据";
                saveDialog.Filter = "tif数据(*.tif)|*.tif";
            }
            else if (cmbDataFormat.Text.ToUpper() == "IMG")
            {
                saveDialog.Title = "保存img数据";
                saveDialog.Filter = "img数据(*.img)|*.img";
            }
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                txtSavePath.Text = saveDialog.FileName;

            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 根据范围提取数据
        /// </summary>
        /// <param name="pFileName">目标数据名称（不包含路径）</param>
        /// <param name="clipExtent">几何范围</param>
        /// <param name="pDesWS">目标数据工作空间</param>
        /// <param name="format">目标数据类型</param>
        /// <param name="pResRatser">源栅格数据</param>
        /// <param name="bIn">true:表明提取几何范围内部的数据；false:提取几何范围外部的数据</param>
        /// <param name="eError"></param>
        private void StractData(string pFileName, IGeometry clipExtent, IWorkspace pDesWS, string format, IRaster pResRatser, bool bIn, out Exception eError)
        {
            eError = null;
            IExtractionOp extraction = new RasterExtractionOpClass();
            #region 进行裁切
            IGeoDataset pOrgGeoDt = pResRatser as IGeoDataset;
            IGeoDataset pDesGeoDt = null;
            IPolygon pPoly = clipExtent as IPolygon;
            ICircularArc pArc = clipExtent as ICircularArc;
            IPointCollection pPointColl = clipExtent as IPointCollection;
            IEnvelope pRec = clipExtent as IEnvelope;
            if (pPoly != null)
            {
                //几何范围是多边形
                pDesGeoDt = extraction.Polygon(pOrgGeoDt, pPoly, checkBoxClip.Checked);
            }
            else if (pArc != null)
            {
                //几何范围是圆形

                pDesGeoDt = extraction.Circle(pOrgGeoDt, pArc, checkBoxClip.Checked);
            }
            else if (pRec != null)
            {
                //几何范围是矩形

                pDesGeoDt = extraction.Rectangle(pOrgGeoDt, pRec, checkBoxClip.Checked);
            }
            else if (pPointColl != null)
            {
                //几何范围是点集

                pDesGeoDt = extraction.Points(pOrgGeoDt, pPointColl, checkBoxClip.Checked);
            }
            #endregion

            #region 进行保存
            IRaster pDesRaster = pDesGeoDt as IRaster;
            ISaveAs pSaveAs = pDesRaster as ISaveAs;
            if (format == "TIF")
            {
                if (!pSaveAs.CanSaveAs("TIFF"))
                {
                    eError = new Exception("该栅格数据不能保存为tiff格式！");
                    return;
                }
                pSaveAs.SaveAs(pFileName, pDesWS, "TIFF");
            }
            else if (format == "IMG")
            {
                if (!pSaveAs.CanSaveAs("IMAGINE Image"))
                {
                    eError = new Exception("该栅格数据不能保存为IMG格式！");
                    return;
                }
                pSaveAs.SaveAs(pFileName, pDesWS, "IMAGINE Image");
            }
            else
            {
                //geodatabase结构数据需要用另外的方式来保存数据：PDB、GDB
                //geodatabase根据已有的rasterDataset生成目标rasterDataset(clipExtent几何限制条件没用上)
                IRaster2 pOrgRaster2 = pResRatser as IRaster2;
                if (pOrgRaster2 == null) return;
                IRasterDataset mOrgRDataset = pOrgRaster2.RasterDataset; //源栅格数据集
                if (mOrgRDataset.CanCopy())
                {
                    mOrgRDataset.Copy(m_DesRasterCollName, pDesWS);
                }
            }
            #endregion
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

                if (File.Exists(sFilePath))
                {
                    if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "存在同名文件，是否替换？"))
                    {
                        File.Delete(sFilePath);
                    }
                    else
                    {
                        return null;
                    }
                }
                if (Directory.Exists(sFilePath))
                {
                    if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "存在同名文件，是否替换？\n文件名为：" + sFilePath))
                    {
                        Directory.Delete(sFilePath, true);
                    }
                    else
                    {
                        return null;
                    }
                }

                FileInfo finfo = new FileInfo(sFilePath);
                string outputDBPath = finfo.DirectoryName;   //路径
                string outputDBName = finfo.Name;            //文件名

                if (outputDBName.LastIndexOf('.') != -1)
                {
                    outputDBName = outputDBName.Substring(0, outputDBName.LastIndexOf('.'));
                }

                if (strType.Trim().ToUpper() == "PDB")
                {
                    pWorkspaceFactory = new AccessWorkspaceFactoryClass();
                }
                else if (strType.Trim().ToUpper() == "GDB")
                {
                    pWorkspaceFactory = new FileGDBWorkspaceFactoryClass();
                }
                else
                {
                    //文件型,栅格数据
                    pWorkspaceFactory = new RasterWorkspaceFactoryClass();
                    TempWorkSpace = pWorkspaceFactory.OpenFromFile(outputDBPath, 0);
                    return TempWorkSpace;
                }
                IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create(outputDBPath, outputDBName, null, 0);
                IName pName = (IName)pWorkspaceName;
                TempWorkSpace = (IWorkspace)pName.Open();

                return TempWorkSpace;
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
                return null;
            }
        }

        /// <summary>
        /// 获得要素类的字段信息
        /// </summary>
        /// <param name="mFeaCls">要素类</param>
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

    }
}