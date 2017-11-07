using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Xml;
using System.IO;
using ESRI.ArcGIS.esriSystem;
using SysCommon.Gis;
using ESRI.ArcGIS.DataSourcesGDB;

namespace GeoUtilities
{
	 public static class GeoModule
    {
        public static IList<string> GetMapNOsByGeometry(IMap pMap, string strMapCls, IGeometry pGeometry)
        {
            IList<string> lstMapNOs=new List<string>();
            IFeatureClass pFeaClsMapNO = null;

            List<ILayer> lstLyrs = new List<ILayer>();
            SysCommon.Gis.ModGisPub.GetLayersByMap(pMap, ref lstLyrs);

            //获得要素类
            for (int i = 0; i < lstLyrs.Count; i++)
            {
                ILayer pLyr = lstLyrs[i];
                if (!(pLyr is IFeatureLayer)) continue;

                IFeatureLayer pFeaLyr=pLyr as IFeatureLayer;
                IFeatureClass pFeaCls = pFeaLyr.FeatureClass;
                if (pFeaCls == null) continue;
                if (pFeaCls.FeatureType != esriFeatureType.esriFTSimple) continue;

                IDataset pDataSet = pFeaCls as IDataset;
                string strFeaClsName = pDataSet.Name;
                strFeaClsName=strFeaClsName.Substring(strFeaClsName.IndexOf('.')+1);
                if (strFeaClsName.ToUpper() == strMapCls.ToUpper())
                {
                    pFeaClsMapNO = pFeaCls;
                    break;
                } 
            }

            if (pFeaClsMapNO == null) return lstMapNOs;
            int intMapFieldIndex=pFeaClsMapNO.Fields.FindField("MAP");//注意这里的图幅号字段名是固定的
            if (intMapFieldIndex < 0) return lstMapNOs;

            //获得图幅号
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.Geometry = pGeometry;
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            //查询图幅好
            IFeatureCursor pFeaCursor = pFeaClsMapNO.Search(pSpatialFilter, false);
            IFeature pFea = pFeaCursor.NextFeature();
            while (pFea != null)
            {
                string strMapNO = "";
                object obj = pFea.get_Value(intMapFieldIndex);
                if (obj != null && obj != DBNull.Value) strMapNO = obj.ToString();

                if (strMapNO.Trim() != "" && !lstMapNOs.Contains(strMapNO.Trim())) lstMapNOs.Add(strMapNO);

                pFea = pFeaCursor.NextFeature();
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);
            pFeaCursor = null;

            return lstMapNOs;
        }
        private static string _layerTreePath =System.Windows.Forms.Application.StartupPath + "\\..\\res\\xml\\展示图层树.xml";//added by chulili 20110802 褚丽丽添加变量,展示图层树路径,专门用来查找道路,河流,地名地物类
         //added by chulili 20110802褚丽丽添加函数,根据nodeKey获取地物类,直接读取数据源连接信息,读取地物类
        public static IFeatureClass GetFeatureClassByNodeKey(string strNodeKey)
        {
            if (strNodeKey.Equals(""))
            {
                return null;
            }
            //目录树路径变量:_layerTreePath
            XmlDocument pXmldoc = new XmlDocument();
            if (!File.Exists(_layerTreePath))
            {
                return null;
            }
            //打开展示图层树,获取图层节点
            pXmldoc.Load(_layerTreePath);
            string strSearch = "//Layer[@NodeKey=" + "'" + strNodeKey + "'" + "]";
            XmlNode pNode = pXmldoc.SelectSingleNode(strSearch);
            if (pNode == null)
            {
                return null;
            }
            //获取图层名,数据源id
            string strFeaClassName = "";
            string strDBSourceID = "";
            try
            {
                strFeaClassName = pNode.Attributes["Code"].Value;
                strDBSourceID = pNode.Attributes["ConnectKey"].Value;
            }
            catch
            { }
            //根据数据源id,获取数据源信息
            SysGisTable sysTable = new SysGisTable(Plugin.ModuleCommon.TmpWorkSpace);
            Exception eError = null;
            object objConnstr = sysTable.GetFieldValue("DATABASEMD", "CONNECTIONINFO", "ID=" + strDBSourceID, out eError);
            string conninfostr = "";
            if (objConnstr != null)
            {
                conninfostr = objConnstr.ToString();
            }
            object objType = sysTable.GetFieldValue("DATABASEMD", "DATAFORMATID", "ID=" + strDBSourceID, out eError);
            int type = -1;
            if (objType != null)
            {
                type = int.Parse(objType.ToString());
            }
            //根据数据源连接信息,获取数据源连接
            IWorkspace pWorkspace = GetWorkSpacefromConninfo(conninfostr, type);
            if (pWorkspace == null)
            {
                return null;
            }
            //打开地物类
            IFeatureWorkspace pFeaWorkSpace = pWorkspace as IFeatureWorkspace;
            IFeatureClass pFeaClass = null;
            try
            {
                pFeaClass = pFeaWorkSpace.OpenFeatureClass(strFeaClassName);
            }
            catch
            { }
            return pFeaClass;

        }
        //根据连接字符串获取工作空间
        //此处连接字符串是固定格式的连接串 Server|Service|Database|User|Password|Version
        private static IWorkspace GetWorkSpacefromConninfo(string conninfostr, int type)
        {
            //added by chulili 20111109 添加保护
            if (conninfostr == "")
            {
                return null;
            }
            if (type < 0)
            {
                return null;
            }
            //end added by chulili 20111109
            int index1 = conninfostr.IndexOf("|");
            int index2 = conninfostr.IndexOf("|", index1 + 1);
            int index3 = conninfostr.IndexOf("|", index2 + 1);
            int index4 = conninfostr.IndexOf("|", index3 + 1);
            int index5 = conninfostr.IndexOf("|", index4 + 1);
            int index6 = conninfostr.IndexOf("|", index5 + 1);
            IPropertySet pPropSet = new PropertySetClass();
            IWorkspaceFactory pWSFact = null;
            string sServer = ""; string sService = ""; string sDatabase = "";
            string sUser = ""; string sPassword = ""; string strVersion = "";
            switch (type)
            {
                case 1://mdb
                    pWSFact = new AccessWorkspaceFactoryClass();
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    break;
                case 2://gdb
                    pWSFact = new FileGDBWorkspaceFactoryClass();
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    break;
                case 3://sde
                    pWSFact = new SdeWorkspaceFactoryClass();
                    sServer = conninfostr.Substring(0, index1);
                    sService = conninfostr.Substring(index1 + 1, index2 - index1 - 1);
                    sDatabase = conninfostr.Substring(index2 + 1, index3 - index2 - 1);
                    sUser = conninfostr.Substring(index3 + 1, index4 - index3 - 1);
                    sPassword = conninfostr.Substring(index4 + 1, index5 - index4 - 1);
                    strVersion = conninfostr.Substring(index5 + 1, index6 - index5 - 1);
                    break;
            }

            pPropSet.SetProperty("SERVER", sServer);
            pPropSet.SetProperty("INSTANCE", sService);
            pPropSet.SetProperty("DATABASE", sDatabase);
            pPropSet.SetProperty("USER", sUser);
            pPropSet.SetProperty("PASSWORD", sPassword);
            pPropSet.SetProperty("VERSION", strVersion);
            try
            {

                IWorkspace pWorkspace = pWSFact.Open(pPropSet, 0);
                return pWorkspace;
            }
            catch
            {
                return null;
            }
        }
    }
    
}
