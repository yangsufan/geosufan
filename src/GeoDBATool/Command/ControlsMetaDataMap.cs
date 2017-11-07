using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.OleDb;
// zhangqi    add   
using SysCommon.Authorize;
using SysCommon.Gis;
using ESRI.ArcGIS.Geodatabase;
using System.Data;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using SysCommon.Error;

namespace GeoDBATool
{
    /// <summary>
    /// yjl 20110727 ：元数据 出图
    /// </summary>
    public class ControlsMetaDataMap : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        private string g_conn = "";
        public ControlsMetaDataMap()
        {
            base._Name = "GeoDBATool.ControlsMetaDataMap";
            base._Caption = "元数据出图";
            base._Tooltip = "元数据出图";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "元数据出图";
            
        }
        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.MapControl == null) return false;
                return true;
                
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = m_Hook as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
        //测试链接信息是否可用
        private bool CanOpenConnect(SysCommon.Gis.SysGisDB vgisDb, string strType, string strServer, string strService, string strDatabase, string strUser, string strPassword, string strVersion)
        {
            bool blnOpen = false;

            Exception Err;

            if (strType.ToUpper() == "ORACLE" || strType.ToUpper() == "SQLSERVER")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, strService, strDatabase, strUser, strPassword, strVersion, out Err);
            }
            else if (strType.ToUpper() == "ACCESS")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.PDB, out Err);
            }
            else if (strType.ToUpper() == "FILE")
            {
                blnOpen = vgisDb.SetWorkspace(strServer, SysCommon.enumWSType.GDB, out Err);
            }

            return blnOpen;

        }
        public override void OnClick()
        {
            //   zhangqi Add  
            //判断配置文件是否存在
            bool blnCanConnect = false;
            SysCommon.Gis.SysGisDB vgisDb = new SysGisDB();
            if (File.Exists(GeoDBIntegration.ModuleData.v_ConfigPath))
            {
                //获得系统维护库连接信息
                SysCommon.Authorize.AuthorizeClass.GetConnectInfo(GeoDBIntegration.ModuleData.v_ConfigPath, out GeoDBIntegration.ModuleData.Server, out GeoDBIntegration.ModuleData.Instance, out GeoDBIntegration.ModuleData.Database, out GeoDBIntegration.ModuleData.User, out GeoDBIntegration.ModuleData.Password, out GeoDBIntegration.ModuleData.Version, out GeoDBIntegration.ModuleData.dbType);
                //连接系统维护库
                blnCanConnect = CanOpenConnect(vgisDb, GeoDBIntegration.ModuleData.dbType, GeoDBIntegration.ModuleData.Server, GeoDBIntegration.ModuleData.Instance, GeoDBIntegration.ModuleData.Database, GeoDBIntegration.ModuleData.User, GeoDBIntegration.ModuleData.Password, GeoDBIntegration.ModuleData.Version);
            }
            else
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "缺失系统维护库连接信息文件：" + GeoDBIntegration.ModuleData.v_ConfigPath + "/n请重新配置");
                return;
            }
            if (!blnCanConnect)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "系统能够维护库连接失败，请检查!");
                return;
            }
            //zhangqi   add  end
           GeoDBIntegration.ModuleData.TempWks = vgisDb.WorkSpace;
           SysCommon.CProgress pgss = new SysCommon.CProgress("正在加载元数据出图界面，请稍候...");
           try
           {

               
               pgss.EnableCancel = false;
               pgss.ShowDescription = false;
               pgss.FakeProgress = true;
               pgss.TopMost = false;
               pgss.ShowProgress();
               Application.DoEvents();
               createMatadataMap(vgisDb.WorkSpace);
               pgss.Close();
           }
           catch (Exception ex)
           {
               pgss.Close();
               ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
               return;
           }
           frmMetaMap FrmMetaConv = new frmMetaMap(vgisDb.WorkSpace);
           FrmMetaConv.ShowDialog();
            
        }
        private void createMatadataMap(IWorkspace inF)
        {
            g_conn = "Provider=OraOLEDB.Oracle;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + Plugin.Mod.Server + ") (PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=orcl)));Persist Security Info=True;User Id=" + Plugin.Mod.User + "; Password=" + Plugin.Mod.Password + "";
            //g_conn = "Provider=OraOLEDB.Oracle;Data Source=orc;Persist Security Info=True;User Id=" + Plugin.Mod.User + "; Password=" + Plugin.Mod.Password + "";
            OleDbConnection oeCon = new OleDbConnection(g_conn);
            oeCon.Open();
            string pTname = "metadata_map_tmp";
            ModTableFun.DropTable(oeCon, pTname);
            if (!ModTableFun.isExist(oeCon, "metadata_xml")||!ModTableFun.isExist(oeCon,"JHTB"))
                return;
            string sql = "create table metadata_map_tmp as select * from metadata_xml";
            ModTableFun.ExecuteDDL(oeCon, sql);//根据元数据表创建中间表
            ModTableFun.DropColumn(oeCon, pTname, "XMLID,METADATAXML");//删掉不支持的类型的列
            if (!ModTableFun.isExist(oeCon, pTname))//中间表未创建  返回
                return;
            
            IFeatureWorkspace pFW = inF as IFeatureWorkspace;
            IFeatureClass pFC_jhtb = pFW.OpenFeatureClass("JHTB");
            IFeatureClass pFC = null;//元数据地图图层
            if (!(inF as IWorkspace2).get_NameExists(esriDatasetType.esriDTFeatureClass, "METADATA_MAP"))//若不存在就创建
            {
                IGeoDataset pGD_jhtb = pFC_jhtb as IGeoDataset;
                pFC=CreateFeatureClass("METADATA_MAP", pFW, pGD_jhtb.SpatialReference);
                IClassSchemaEdit pCSE = pFC as IClassSchemaEdit;
                pCSE.AlterAliasName("元数据地图");
                ITable pT = pFW.OpenTable(pTname);
                for (int i = 0; i < pT.Fields.FieldCount; i++)
                {
                    IField pField = pT.Fields.get_Field(i);
                    pFC.AddField(pField);

                }
            }
            else//否则直接打开
            {
                //打开前删除全部的记录
                sql = "truncate table METADATA_MAP";
                ModTableFun.ExecuteDDL(oeCon, sql);
                pFC = pFW.OpenFeatureClass("METADATA_MAP");
            }
            if (pFC.FeatureCount(null) != 0)//确认元数据地图层记录为空
                return;
            IFields pFds = pFC.Fields;

            IFeatureCursor insertCsr = pFC.Insert(true);//插入要素的游标
           
          
            sql = "select * from "+pTname;
            OleDbDataReader oeDR = ModTableFun.GetReader(oeCon, sql);
            if (oeDR == null)//元数据为空，返回
                return;
            DataTable pDT = oeDR.GetSchemaTable();
            while (oeDR.Read())//遍历记录
            {
                IFeatureBuffer pFB = pFC.CreateFeatureBuffer();
                IFeature pF = pFB as IFeature;
                string tfh = oeDR["图幅号"].ToString();
                IQueryFilter pQF = new QueryFilterClass();
                pQF.WhereClause = "MAPNO = '" + tfh + "'";
                IFeatureCursor fCursor_jhtb = pFC_jhtb.Search(pQF, false);
                IFeature f_jhtb = fCursor_jhtb.NextFeature();
                if (f_jhtb == null)//若元数据表中的图幅号未在结合图表找到，则不加入元数据地图图层
                    continue;
                pF.Shape = f_jhtb.ShapeCopy;//写结合图表的图形
                foreach (DataRow dr in pDT.Rows)//遍历写入列值
                {
                   
                    int fdix = pFds.FindField(dr["ColumnName"].ToString());
                    IField pFld = pFds.get_Field(fdix);
                    if ((fdix > -1) && (pFld.Editable == true) && pFld.Type != esriFieldType.esriFieldTypeGeometry && pFld.Type != esriFieldType.esriFieldTypeOID)
                    {
                        pF.set_Value(fdix, oeDR[dr["ColumnName"].ToString()]);
                    }

                    
                }//foreach

                insertCsr.InsertFeature(pFB);//将缓存要素插入游标
 
            }//while
            insertCsr.Flush();
            
        }
        //using a class description object创建一个新要素类，并赋予空间参考和要素类型
        private IFeatureClass CreateFeatureClass(String featureClassName, IFeatureWorkspace featureWorkspace,  ISpatialReference spatialReference)
        {  //实例化要素类描述对象，获得默认字段 
            IFeatureClassDescription fcDescription = new FeatureClassDescriptionClass(); 
            IObjectClassDescription ocDescription = (IObjectClassDescription)fcDescription; 
            IFields fields = ocDescription.RequiredFields;   
            // 找到图形字段定义空间参考和类型 
            int shapeFieldIndex = fields.FindField(fcDescription.ShapeFieldName);  
            IField field = fields.get_Field(shapeFieldIndex);  
            IGeometryDef geometryDef = field.GeometryDef;  
            IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;  
            geometryDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;  
            geometryDefEdit.SpatialReference_2 = spatialReference;   
          
            IFeatureClass featureClass = featureWorkspace.CreateFeatureClass(featureClassName, fields,    ocDescription.InstanceCLSID, ocDescription.ClassExtensionCLSID, 
                esriFeatureType.esriFTSimple,    fcDescription.ShapeFieldName, "");  
            return featureClass;}
    }
}
