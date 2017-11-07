//*********************************************************************************
//** 文件名：ChangeJudge.cs
//** CopyRight (c) 2000-2007 武汉吉奥信息工程技术有限公司工程部
//** 创建人：chulili
//** 日  期：2011-03-21
//** 修改人：
//** 日  期：
//** 描  述：用于图斑研判功能
//**
//** 版  本：1.0
//*********************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using SysCommon.Gis;
using System.Xml;
using System.IO;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using System.Data.OleDb;

using GeoDataCenterFunLib;

namespace GeoDataManagerFrame
{
    class ChangeJudge
    {
        public const string g_DLBM = "DLBM"; //地类编码
        public const string g_DLMC = "DLMC";//地类名称
        public const string g_XZQHDM = "XZQHDM";//行政区划代码
        public const string g_TBBH = "TBBH";//图斑编号
        public const string g_MJ = "MJ";//面积----建议为变化图斑增加一个属性(面积)
        public const string g_PC = "PCMC";//批次
        public const string g_PCBH = "PCBH";//批次编号
        public const string g_SGTJPFWH = "SGTJPFWH";//市林业局批复文号
        public const string g_SGTTPFWH = "SGTTPZWH";//省林业厅批准文号
        public const string g_GWYPFWH = "GWYPFWH";//国务院批复文号
        public const string g_TDYTFQDM = "TDYTFQDM";//森林用途分区代码
        public const string g_TDYTFQBH = "TDYTFQBH";//森林用途分区编号
        public const string g_TBMJ = "TBMJ";//图斑面积
        public const string g_XZDWMJ = "XZDWMJ";//线状地物面积
        public const string g_LXDWMJ = "LXDWMJ";//零星地物面积
        public const string g_TBDLMJ = "TBDLMJ";//图斑地类面积
        public const string g_ZB = "ZB";//占比
        private static string m_WorkPath = "";
        //added by chulili 
        //函数功能：图斑研判主函数
        //传入参数：变化图斑图层 批次报批图层 森林用途图层 地类图斑图层 进度条  传出参数：无

        public static void DoJudgeBySelect(IFeatureLayer pChangeFeaLayer, IFeatureLayer pcbpFeaLayer, IFeatureLayer tdytFeaLayer, IFeatureLayer dltbFeaLayer,SysCommon.CProgress vProgress)
        {
            if (pChangeFeaLayer == null)
                return;
            IFeatureClass pChangeFeaClass=pChangeFeaLayer.FeatureClass;
            IFeatureSelection pSelect = pChangeFeaLayer as IFeatureSelection ;
            //若变化图斑层不存在选择集则退出
            if (pSelect.SelectionSet.Count < 1)
                return;
            if (pcbpFeaLayer == null)
                return;
            if (tdytFeaLayer==null)
                return;
            if (dltbFeaLayer==null)
                return;
            string workpath = Application.StartupPath + @"\..\OutputResults\文档成果\" + System.DateTime.Now.ToString("yyyyMMddhhmmss");
            m_WorkPath = workpath;
            string workSpaceName = workpath + "\\" + "JudgeRes.mdb";
            //判断结果目录是否存在，不存在则创建
            if (System.IO.Directory.Exists(workpath) == false)
                System.IO.Directory.CreateDirectory(workpath);
            //创建一个新的mdb数据库,并打开工作空间
            IWorkspace pOutWorkSpace = CreatePDBWorkSpace(workpath, "JudgeRes.mdb");
            //进度条提示
            if (vProgress!=null) vProgress.SetProgress("正在生成监测图斑报批情况表");
            pcbpJudge(pChangeFeaLayer, pcbpFeaLayer, pOutWorkSpace, false ,vProgress );
            //进度条提示
            if (vProgress != null) vProgress.SetProgress("正在生成监测图斑规划情况表");
            tdytJudge(pChangeFeaLayer, tdytFeaLayer, pOutWorkSpace, false, vProgress);
            //进度条提示
            if (vProgress != null) vProgress.SetProgress("正在生成监测图斑地类情况表");
            dltbJudge(pChangeFeaLayer, dltbFeaLayer, pOutWorkSpace, false, vProgress);

        }
        //森林用途研判
        private static void tdytJudge(IFeatureLayer pChangeFeaLayer, IFeatureLayer tdytFeaLayer, IWorkspace pOutWorkSpace, bool useSelection,SysCommon.CProgress vProgress)
        {
            ITable pChangeTable = pChangeFeaLayer as ITable;
            ITable tdytTable = tdytFeaLayer as ITable;
            double rol = 0.0001;
            IFeatureClass tdytFeaClass = tdytFeaLayer.FeatureClass;
            //创建结果地物类名称
            IFeatureClassName pResFeaClassName = new FeatureClassNameClass();
            String fcName = tdytFeaClass.AliasName.Trim().Substring(tdytFeaClass.AliasName.Trim().IndexOf(".") + 1)+"_res";
            IDataset pOutDataset = (IDataset)pOutWorkSpace;
            IDatasetName pOutDatasetName = (IDatasetName)pResFeaClassName;
            pOutDatasetName.WorkspaceName = (IWorkspaceName)pOutDataset.FullName;
            pOutDatasetName.Name = fcName;
            IBasicGeoprocessor pGeoProcessor = new BasicGeoprocessorClass();
            //叠置分析
            pGeoProcessor.Intersect(pChangeTable, useSelection, tdytTable, false, rol, pResFeaClassName);

            //从叠置结果生成报表
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pOutDatasetName.WorkspaceName.PathName;
            OleDbConnection oledbconn = new OleDbConnection(connstr);
            oledbconn.Open();
            ModTableFun.DropTable(oledbconn,"tmprel");
            string sqlstr = "select "+g_XZQHDM+","+g_TBBH+","+g_MJ+" as jctbmj,"+g_TDYTFQBH+","+g_TDYTFQDM+",shape_area as jsmj,shape_area as mj,shape_area as zb into tmprel from " + fcName;
                            //行政区划代码，编号，监测图斑面积，规划图斑编号，规划用地用途代码
            OleDbCommand oledbcomm = oledbconn.CreateCommand();
            oledbcomm.CommandText = sqlstr;
            oledbcomm.ExecuteNonQuery();

            oledbcomm.CommandText = "update tmprel set zb=mj/jctbmj*100";
            oledbcomm.ExecuteNonQuery();

            ModTableFun.DropTable(oledbconn ,"森林用途字典");
            CopyTdytDictionary(oledbconn);//从业务库里面拷贝森林用途字典过来
            
            //根据森林用途字典更新森林用途名称
            oledbcomm.CommandText = "alter table tmprel add tdytmc text(30)";
            oledbcomm.ExecuteNonQuery();
            oledbcomm.CommandText = "update tmprel set tdytmc=" + g_TDYTFQDM;
            oledbcomm.ExecuteNonQuery();
            if (ModTableFun.isExist(oledbconn,"森林用途字典"))
            {               
                oledbcomm.CommandText = "update tmprel a,森林用途字典 b set a.tdytmc=b.森林用途分区类型 where a." + g_TDYTFQDM + "=b.代码";
                oledbcomm.ExecuteNonQuery();
            }
            //报表模板路径
            string Templatepath = Application.StartupPath + "\\..\\Template\\森林资源规划研判模板.cel";
            //生成报表对话框
            oledbconn.Close();
            FormFlexcell frm;
            ModFlexcell.m_SpecialRow = -1;
            ModFlexcell.m_SpecialRow_ex = -1;
            ModFlexcell.m_SpecialRow_ex2 = -1;
            //规划图斑没有图斑编号怎么办？？暂时使用森林用途分区编号
            frm = ModFlexcell.SendDataToFlexcell(connstr, "监测图斑规划情况表", "tmprel", g_XZQHDM + "," + g_TBBH + ",jctbmj,TDYTFQBH,tdytmc," + g_MJ + "," + g_ZB, "", Templatepath, 4, 2);
            //弹出报表对话框

            AxFlexCell.AxGrid pGrid = frm.GetGrid();
            string excelPath = m_WorkPath + "\\监测图斑规划情况表.xls";
            pGrid.ExportToExcel(excelPath);

            //frm.SaveFile(m_WorkPath + "\\监测图斑规划情况表.cel");
            ModStatReport.OpenExcelFile(excelPath);
            
        }
        //批次报批研判
        private static void pcbpJudge(IFeatureLayer pChangeFeaLayer, IFeatureLayer pcbpFeaLayer, IWorkspace pOutWorkSpace, bool useSelection,SysCommon.CProgress vProgress)
        {
            ITable pChangeTable = pChangeFeaLayer as ITable;
            ITable pcbpTable = pcbpFeaLayer as ITable;
            double rol = 0.0001;
            IFeatureClass pcbpFeaClass = pcbpFeaLayer.FeatureClass;
            //创建结果地物类名称
            IFeatureClassName pResFeaClassName = new FeatureClassNameClass();
            String fcName = pcbpFeaClass.AliasName.Trim().Substring(pcbpFeaClass.AliasName.Trim().IndexOf(".") + 1)+"_res";
            IDataset pOutDataset = (IDataset)pOutWorkSpace;
            IDatasetName pOutDatasetName = (IDatasetName)pResFeaClassName;
            pOutDatasetName.WorkspaceName = (IWorkspaceName)pOutDataset.FullName;
            pOutDatasetName.Name = fcName;
            IBasicGeoprocessor pGeoProcessor = new BasicGeoprocessorClass();
            //叠置分析
            pGeoProcessor.Intersect(pChangeTable, useSelection, pcbpTable, false, rol, pResFeaClassName);

            //从叠置结果生成报表
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pOutDatasetName.WorkspaceName.PathName;
            OleDbConnection oledbconn = new OleDbConnection(connstr);
            oledbconn.Open();
            ModTableFun.DropTable(oledbconn, "tmprel");
            string sqlstr = "select "+g_XZQHDM+","+g_TBBH+","+g_MJ+" as jctbmj,"+g_PC  +",shape_area as " +g_MJ+",shape_area as zb,"+g_GWYPFWH+" as bpwh,"+g_SGTTPFWH+","+g_SGTJPFWH+" into tmprel from " + fcName;
                            //行政区划代码，编号，监测图斑面积，报批名称，计算面积，占比，批准文号（三个批准文号的备选字段）
            OleDbCommand oledbcomm = oledbconn.CreateCommand();
            oledbcomm.CommandText = sqlstr;
            oledbcomm.ExecuteNonQuery();

            oledbcomm.CommandText = "update tmprel set zb="+g_MJ+"/jctbmj*100";
            oledbcomm.ExecuteNonQuery();
            //报批文号为空，则取省林业厅批复文号
            oledbcomm.CommandText = "update tmprel set bpwh=" + g_SGTTPFWH + " where bpwh is null and " + g_SGTTPFWH + "  is not null";
            oledbcomm.ExecuteNonQuery();
            //报批文号为空，则取省市林业局批复文号
            oledbcomm.CommandText = "update tmprel set bpwh=" + g_SGTJPFWH + " where bpwh is null and " + g_SGTJPFWH + " is not null";
            oledbcomm.ExecuteNonQuery();
            //报表模板路径
            string Templatepath = Application.StartupPath + "\\..\\Template\\批次报批研判模板.cel";
            oledbconn.Close();
            //生成报表对话框
            FormFlexcell frm;
            ModFlexcell.m_SpecialRow = -1;
            ModFlexcell.m_SpecialRow_ex = -1;
            ModFlexcell.m_SpecialRow_ex2 = -1;
            frm = ModFlexcell.SendDataToFlexcell(connstr, "监测图斑报批情况表", "tmprel", g_XZQHDM + "," + g_TBBH + ",jctbmj," + g_PC + "," + g_MJ + "," + g_ZB + ",bpwh", "", Templatepath, 4, 2);

            AxFlexCell.AxGrid pGrid = frm.GetGrid();
            string excelPath = m_WorkPath + "\\监测图斑报批情况表.xls";
            pGrid.ExportToExcel(excelPath);
            
            //frm.SaveFile(m_WorkPath + "\\监测图斑报批情况表.cel");
            //弹出报表
            ModStatReport.OpenExcelFile(excelPath);
            
        }
        //森林资源/地类图斑研判
        private static void dltbJudge(IFeatureLayer pChangeFeaLayer, IFeatureLayer dltbFeaLayer, IWorkspace pOutWorkSpace, bool useSelection,SysCommon.CProgress vProgress)
        {
            ITable pChangeTable = pChangeFeaLayer as ITable;
            ITable pcbpTable = dltbFeaLayer as ITable;
            IFeatureClass dltbFeaClass = dltbFeaLayer.FeatureClass;
            double rol = 0.0001;
            IFeatureClassName pResFeaClassName = new FeatureClassNameClass();
            //创建结果地物类名称
            String fcName = dltbFeaClass.AliasName.Trim().Substring(dltbFeaClass.AliasName.Trim().IndexOf(".") + 1)+"_res";
            IDataset pOutDataset = (IDataset)pOutWorkSpace;
            IDatasetName pOutDatasetName = (IDatasetName)pResFeaClassName;
            pOutDatasetName.WorkspaceName = (IWorkspaceName)pOutDataset.FullName;
            pOutDatasetName.Name = fcName;
            IBasicGeoprocessor pGeoProcessor = new BasicGeoprocessorClass();
            //叠置分析
            pGeoProcessor.Intersect(pChangeTable, useSelection, pcbpTable, false, rol, pResFeaClassName);

            //从叠置结果生成报表
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pOutDatasetName.WorkspaceName.PathName ;
            OleDbConnection oledbconn = new OleDbConnection(connstr);
            oledbconn.Open();
            ModTableFun.DropTable(oledbconn, "tmprel");
            string sqlstr = "select "+g_XZQHDM+","+g_TBBH+","+g_MJ+" as jctbmj,"+g_TBBH+"_1,"+g_DLBM+","+g_TBMJ+","+g_TBDLMJ+",shape_area as jsmj,shape_area as mj,shape_area as zb into tmprel from " + fcName;
                            //行政区划代码，编号，面积，图斑编号，地类编码，图斑面积，图斑地类面积，计算面积，占比
            OleDbCommand oledbcomm = oledbconn.CreateCommand();
            oledbcomm.CommandText = sqlstr;
            oledbcomm.ExecuteNonQuery();
            //叠置结果地类面积计算方法：  面积=叠置结果计算面积*地类图斑地类面积/地类图斑总面积
            oledbcomm.CommandText = "update tmprel set mj=jsmj*"+g_TBDLMJ+"/"+g_TBMJ+"";
            oledbcomm.ExecuteNonQuery();
            //计算占比
            oledbcomm.CommandText = "update tmprel set zb=mj/jctbmj*100";
            oledbcomm.ExecuteNonQuery();
            //报表模板路径
            oledbconn.Close();
            string Templatepath = Application.StartupPath + "\\..\\Template\\森林资源现状研判模板.cel";
            //生成报表对话框
            FormFlexcell frm;
            ModFlexcell.m_SpecialRow = -1;
            ModFlexcell.m_SpecialRow_ex = -1;
            ModFlexcell.m_SpecialRow_ex2 = -1;
            frm = ModFlexcell.SendDataToFlexcell(connstr, "监测图斑地类情况表", "tmprel", g_XZQHDM + "," + g_TBBH + ",jctbmj," + g_TBBH + "_1,"+g_DLBM+"," + g_MJ + "," + g_ZB, "", Templatepath, 4, 2);

            AxFlexCell.AxGrid pGrid = frm.GetGrid();
            string excelPath = m_WorkPath + "\\监测图斑地类情况表.xls";
            pGrid.ExportToExcel(excelPath);
            
            //frm.SaveFile(m_WorkPath + "\\监测图斑地类情况表.cel");
            //弹出报表
            ModStatReport.OpenExcelFile(excelPath);
            
        }
        //函数功能：拷贝森林用途字典  传入参数：数据库连接  传出参数：无
        //森林用途字典表名称已知
        private static void CopyTdytDictionary(OleDbConnection conn)
        {
            if (conn==null)
                return;
            //if (conn.State!=1)
            //    return;
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            //string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串

            OleDbCommand mycomm = conn.CreateCommand();
            
            mycomm.CommandText = "select * into 森林用途字典 from [" + mypath + "].森林用途字典";
            try
            {
                mycomm.ExecuteNonQuery();
            }
            catch (System.Exception e)
            {
                e.Data.Clear();
                return;
            }


        }
        //added by chulili 
        //函数功能：创建一个新的地物类 传入参数：地物类名称  参考的featurelayer 地物类所在工作空间  CLSID CLSEXT 地物类几何类型
        //传出参数：地物类
        //函数来源：借鉴同事代码  
        private static IFeatureClass CreateFeatureClass(string name, IFeatureLayer pfeaturelayer, IWorkspace pWorkspace, UID uidCLSID, UID uidCLSEXT, esriGeometryType GeometryType)
        {
            try
            {
                if (uidCLSID == null)
                {
                    uidCLSID = new UIDClass();
                    switch (pfeaturelayer.FeatureClass.FeatureType)
                    {
                        case (esriFeatureType.esriFTSimple):
                            uidCLSID.Value = "{52353152-891A-11D0-BEC6-00805F7C4268}";
                            break;
                        case (esriFeatureType.esriFTSimpleJunction):
                            GeometryType = esriGeometryType.esriGeometryPoint;
                            uidCLSID.Value = "{CEE8D6B8-55FE-11D1-AE55-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTComplexJunction):
                            uidCLSID.Value = "{DF9D71F4-DA32-11D1-AEBA-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTSimpleEdge):
                            GeometryType = esriGeometryType.esriGeometryPolyline;
                            uidCLSID.Value = "{E7031C90-55FE-11D1-AE55-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTComplexEdge):
                            GeometryType = esriGeometryType.esriGeometryPolyline;
                            uidCLSID.Value = "{A30E8A2A-C50B-11D1-AEA9-0000F80372B4}";
                            break;
                        case (esriFeatureType.esriFTAnnotation):
                            GeometryType = esriGeometryType.esriGeometryPolygon;
                            uidCLSID.Value = "{E3676993-C682-11D2-8A2A-006097AFF44E}";
                            break;
                        case (esriFeatureType.esriFTDimension):
                            GeometryType = esriGeometryType.esriGeometryPolygon;
                            uidCLSID.Value = "{496764FC-E0C9-11D3-80CE-00C04F601565}";
                            break;
                    }
                }

                // 设置 uidCLSEXT (if Null)
                if (uidCLSEXT == null)
                {
                    switch (pfeaturelayer.FeatureClass.FeatureType)
                    {
                        case (esriFeatureType.esriFTAnnotation):
                            uidCLSEXT = new UIDClass();
                            uidCLSEXT.Value = "{24429589-D711-11D2-9F41-00C04F6BC6A5}";
                            break;
                        case (esriFeatureType.esriFTDimension):
                            uidCLSEXT = new UIDClass();
                            uidCLSEXT.Value = "{48F935E2-DA66-11D3-80CE-00C04F601565}";
                            break;
                    }
                }

                IFeatureWorkspace pFeatureWorkspace = (IFeatureWorkspace)pWorkspace;
                IFields pFields = new FieldsClass();
                IFieldsEdit pFieldsEdit = pFields as IFieldsEdit;
                //为地物类添加字段
                for (int i = 0; i < pfeaturelayer.FeatureClass.Fields.FieldCount; i++)
                {
                    IClone pClone = pfeaturelayer.FeatureClass.Fields.get_Field(i) as IClone;
                    IField pTempField = pClone.Clone() as IField;
                    IFieldEdit pTempFieldEdit = pTempField as IFieldEdit;
                    string strFieldName = pTempField.Name;
                    string[] strFieldNames = strFieldName.Split('.');

                    if (pFieldsEdit.FindField(strFieldNames[strFieldNames.GetLength(0) - 1]) > -1) continue;

                    pTempFieldEdit.Name_2 = strFieldNames[strFieldNames.GetLength(0) - 1];
                    pFieldsEdit.AddField(pTempField);
                }

                string strShapeFieldName = pfeaturelayer.FeatureClass.ShapeFieldName;
                string[] strShapeNames = strShapeFieldName.Split('.');
                strShapeFieldName = strShapeNames[strShapeNames.GetLength(0) - 1];


                IFeatureClass targetFeatureclass = pFeatureWorkspace.CreateFeatureClass("" + name + "", pFields, uidCLSID, uidCLSEXT, pfeaturelayer.FeatureClass.FeatureType, strShapeFieldName, "");

                return targetFeatureclass;
            }
            catch (Exception ex)
            {
                if (ex.Message == "Cannot create a low precision dataset in a high precision database.")
                {
                    MessageBox.Show("数据必须是ArcGis9.3的数据，请将数据处理成ArcGis9.2的数据！");
                }
            }
            IFeatureClass featureclass = null;
            return featureclass;
        }
        //added by chulili
        //函数功能：创建PDB工作空间  传入参数：工作空间所在文件夹路径  工作空间名称 传出参数：工作空间
        //代码来源：借鉴同事代码
        public static IWorkspace CreatePDBWorkSpace(string path,string filename)
        {
            IWorkspaceFactory pWorkspaceFactory = new ESRI.ArcGIS.DataSourcesGDB.AccessWorkspaceFactoryClass();
            if (System.IO.File.Exists(filename))
            {
                if (pWorkspaceFactory.IsWorkspace(filename))
                {
                    IWorkspace pTempWks = pWorkspaceFactory.OpenFromFile(filename, 0);
                    pWorkspaceFactory = null;
                    return pTempWks;
                }
            }

            IWorkspaceName pWorkspaceName = pWorkspaceFactory.Create("" + path + "", "" + filename + "", null, 0);
            IName name = (ESRI.ArcGIS.esriSystem.IName)pWorkspaceName;
            IWorkspace PDB_workspace = (IWorkspace)name.Open();
            pWorkspaceFactory = null;
            return PDB_workspace;

        }
    }
}
