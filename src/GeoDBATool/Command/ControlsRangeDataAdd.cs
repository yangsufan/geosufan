using System;
using System.Collections.Generic;
using System.Text;
using SysCommon.Authorize;
using System.Xml;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;

namespace GeoDBATool
{
    /// <summary>
    /// 加载任务范围,只有管理员才具备加载任务的资格
    /// </summary>
    public class ControlsRangeDataAdd: Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;                //传入的窗体对象
        private User m_AppUser;                             //当前登陆的用户
        private XmlElement m_DBConInfoele = null;   //数据库连接信息Xmlelement
        private IWorkspace m_SDEWs = null;////////////SDE的Workspace


        public ControlsRangeDataAdd()
        {
            base._Name = "GeoDBATool.ControlsRangeDataAdd";
            base._Caption = "加载任务范围";
            base._Tooltip = "加载任务范围";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "加载任务范围数据";
        }

        public override bool Enabled
        {
            get
            {
                ////用户是管理员身份时，按钮才可用
                //if (ModuleData.m_User == null) return false;
                //if (ModuleData.m_User.RoleTypeID!= EnumRoleType.管理员.GetHashCode())
                //    return false;

                if (ModData.v_AppGIS.ProjectTree.SelectedNode == null) return false;
                if (ModData.v_AppGIS.ProjectTree.SelectedNode.Tag == null) return false;
                XmlElement DBInfoEle = ModData.v_AppGIS.ProjectTree.SelectedNode.Tag as XmlElement;
                if (DBInfoEle == null) return false;
                this.m_DBConInfoele = DBInfoEle;

                //cyf 20110602 modify
                //若没有登录系统，则按钮不可用
                if ((m_Hook as Plugin.Application.IAppFormRef).LstRoleInfo == null) return false;
                //若用户不是管理员，则按钮不可用
                bool beAdmin = false;
                foreach (Role pRole in (m_Hook as Plugin.Application.IAppFormRef).LstRoleInfo)
                {
                    if (pRole.TYPEID == EnumRoleType.管理员.GetHashCode().ToString())
                    {
                        beAdmin = true;
                        break;
                    }
                }
                return true;
                //end
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

        public override void OnClick()
        {
            Exception outError = null;
            ////////guozheng 2011-3-14 added 
            if (this.m_DBConInfoele == null) return;
            string sDBConStr = this.m_DBConInfoele.GetAttribute("数据库连接信息");
            string sDBID = this.m_DBConInfoele.GetAttribute("数据库ID");
            string sDBType = this.m_DBConInfoele.GetAttribute("数据库类型");
            string sDBFormate = this.m_DBConInfoele.GetAttribute("数据库平台");
            //cyf 20110603 delete
            //if (sDBFormate != "ARCGISSDE") return;
            //end
            if (string.IsNullOrEmpty(sDBConStr)) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据库工程的连接信息失败"); return; }
            //////////不为SDE数据库不进行任务分配///////
            /////////首先获取SDE上的Workspace///////////

            //cyf 20110603 modify:连接geodatabase
            //IPropertySet pPropSet = new PropertySetClass();
            SysCommon.Gis.SysGisDB pSysDb = new SysCommon.Gis.SysGisDB();
            //IWorkspaceFactory pSdeFact = new SdeWorkspaceFactoryClass();
            string[] SDEConnectInfo = null;
            try
            {
                SDEConnectInfo = sDBConStr.Split('|');
                if (SDEConnectInfo.Length < 7) { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据库工程用户库数据集失败"); return; }
                //pPropSet.SetProperty("SERVER", SDEConnectInfo[0]);
                //pPropSet.SetProperty("INSTANCE", SDEConnectInfo[1]);
                //pPropSet.SetProperty("DATABASE", SDEConnectInfo[2]);
                //pPropSet.SetProperty("USER", SDEConnectInfo[3]);
                //pPropSet.SetProperty("PASSWORD", SDEConnectInfo[4]);
                //pPropSet.SetProperty("VERSION", SDEConnectInfo[5]);
                //this.m_SDEWs = pSdeFact.Open(pPropSet, 0);
                if (sDBFormate == enumInterDBFormat.ARCGISGDB.ToString())
                {
                    pSysDb.SetWorkspace(SDEConnectInfo[2], SysCommon.enumWSType.GDB,out outError);
                }
                else if (sDBFormate == enumInterDBFormat.ARCGISPDB.ToString())
                {
                    pSysDb.SetWorkspace(SDEConnectInfo[2], SysCommon.enumWSType.PDB, out outError);
                }
                else if (sDBFormate == enumInterDBFormat.ARCGISPDB.ToString())
                {
                    pSysDb.SetWorkspace(SDEConnectInfo[0], SDEConnectInfo[1], SDEConnectInfo[2], SDEConnectInfo[3], SDEConnectInfo[4], SDEConnectInfo[5], out outError);
                }
                if (outError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接Geodatabase库体失败，" + outError.Message);
                    return;
                }
                this.m_SDEWs = pSysDb.WorkSpace;
            }
            catch(Exception eError)
            {
                /////系统运行日志
                if (null ==ModData.SysLog) ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModData.SysLog.Write(eError);
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接Geodatabase库体失败，" + eError.Message);
                return;
            }
            //end
            /////////获取本地的任务分配图层，转储至SDE中////////
            IDataset pUserDataSet = null;///////////////////////用户库数据集
            IFeatureClass pTaskRangeCls = null;////////////////Range图层
            try
            {
                if (SDEConnectInfo[6].Trim() == "")
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "目标库数据集不存在，请先创建数据库！");
                    return;
                }
                pUserDataSet = (this.m_SDEWs as IFeatureWorkspace).OpenFeatureDataset(SDEConnectInfo[6]);
                Exception ex=null;
                pTaskRangeCls = CreateTaskRangeLayerInSDE("RANGE", this.m_SDEWs, pUserDataSet, out ex);
                if (ex != null )
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "SDE中建立任务范围图层失败，" + ex.Message);
                    return;
                }

            }
            catch (Exception eError)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                if (null == ModData.SysLog) ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModData.SysLog.Write(eError);
                return;
            }
            ////////获取本地的数据库图层，导入至SDE图层中///////////////
            FrmGetTaskLayerGuide GetTaskLayerGuideFrm = new FrmGetTaskLayerGuide(m_Hook,this.m_SDEWs, sDBID);
            GetTaskLayerGuideFrm.ShowDialog();
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppGISRef;
            m_AppUser = (m_Hook as Plugin.Application.IAppFormRef).ConnUser;
        }

        /// <summary>
        /// 在SDE库体上建立一个任务的范围图层
        /// </summary>
        /// <param name="in_sTaskLayerName"></param>
        /// <param name="in_pSDEWs"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private IFeatureClass CreateTaskRangeLayerInSDE(string in_sTaskLayerName,IWorkspace in_pSDEWs,IDataset in_UserDBDataSet, out Exception ex)
        {
            IFeatureClass ResFeaCls = null;
            ex = null;
            /////////首先在SDE中查找该图层，找到即返回//////
            try
            {
                ResFeaCls = (in_pSDEWs as IFeatureWorkspace).OpenFeatureClass(in_sTaskLayerName);
                if (ResFeaCls != null) return ResFeaCls;
            }
            catch (Exception eError)
            {
            }
            try
            {

                /////////没有找到则建立这一图层//////////
                IFields fields = new FieldsClass();
                IFieldsEdit fsEdit = fields as IFieldsEdit;
                //添加Object字段
                IField newfield2 = new FieldClass();
                IFieldEdit fieldEdit2 = newfield2 as IFieldEdit;
                fieldEdit2.Name_2 = "OBJECTID";
                fieldEdit2.Type_2 = esriFieldType.esriFieldTypeOID;
                fieldEdit2.AliasName_2 = "OBJECTID";
                newfield2 = fieldEdit2 as IField;
                fsEdit.AddField(newfield2);

                //添加Geometry字段
                IField newfield1 = new FieldClass();
                IFieldEdit fieldEdit1 = newfield1 as IFieldEdit;
                fieldEdit1.Name_2 = "SHAPE";
                fieldEdit1.Type_2 = esriFieldType.esriFieldTypeGeometry;

                IGeometryDef geoDef = new GeometryDefClass();
                IGeometryDefEdit geoDefEdit = geoDef as IGeometryDefEdit;
                geoDefEdit.GeometryType_2 = esriGeometryType.esriGeometryPolygon;
                geoDefEdit.SpatialReference_2 = (in_UserDBDataSet as IGeoDataset).SpatialReference;
                fieldEdit1.GeometryDef_2 = geoDefEdit as GeometryDef;
                newfield1 = fieldEdit1 as IField;
                fsEdit.AddField(newfield1);
                fields = fsEdit as IFields;

                //任务相关字段//范围号
                IField newfield = new FieldClass();
                IFieldEdit fieldEdit = newfield as IFieldEdit;
                fieldEdit.Name_2 = "RANGEID";
                fieldEdit.AliasName_2 = "RANGEID";
                fieldEdit.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit.Length_2 = 255;
                newfield = fieldEdit as IField;
                fsEdit.AddField(newfield);
                //范围名称
                IField newfield3 = new FieldClass();
                IFieldEdit fieldEdit3 = newfield3 as IFieldEdit;
                fieldEdit3.Name_2 = "RANGENAME";
                fieldEdit3.AliasName_2 = "RANGENAME";
                fieldEdit3.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit3.Length_2 = 255;
                newfield = fieldEdit3 as IField;
                fsEdit.AddField(newfield3);
                //分配状态
                IField newfield4 = new FieldClass();
                IFieldEdit fieldEdit4= newfield4 as IFieldEdit;
                fieldEdit4.Name_2 = "assign";
                fieldEdit4.AliasName_2 = "assign";
                fieldEdit4.Type_2 = esriFieldType.esriFieldTypeInteger;
                fieldEdit4.Length_2 = 255;
                newfield = fieldEdit4 as IField;
                fieldEdit4.DefaultValue_2 = 0;
                fsEdit.AddField(newfield4);
                //分配的用户
                IField newfield5 = new FieldClass();
                IFieldEdit fieldEdit5 = newfield5 as IFieldEdit;
                fieldEdit5.Name_2 = "USERNAME";
                fieldEdit5.AliasName_2 = "USERNAME";
                fieldEdit5.Type_2 = esriFieldType.esriFieldTypeString;
                fieldEdit5.Length_2 = 255;
                newfield = fieldEdit5 as IField;
                fieldEdit5.DefaultValue_2 = 0;
                fsEdit.AddField(newfield5);

                ResFeaCls = (in_pSDEWs as IFeatureWorkspace).CreateFeatureClass(in_sTaskLayerName, fields, null, null, esriFeatureType.esriFTSimple, "SHAPE", "");
                return ResFeaCls;
            }
            catch (Exception eError)
            {
                if (null == ModData.SysLog) ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                ModData.SysLog.Write(eError);
                ex = eError;
                return null;
            }
        }
    }
}
