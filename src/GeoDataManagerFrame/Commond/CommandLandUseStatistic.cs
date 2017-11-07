using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using System.IO;
using GeoDataCenterFunLib;
using System.Xml;
namespace GeoDataManagerFrame
{
    public class CommandLandUseStatistic : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        public CommandLandUseStatistic()
        {
            base._Name = "GeoDataManagerFrame.CommandLandUseStatistic";
            base._Caption = "县级森林资源现状统计";
            base._Tooltip = "县级森林资源现状统计";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "县级森林资源现状统计";
        }

        public override void OnClick()
        {
            if (m_Hook == null)
                return;
            if (m_Hook.ArcGisMapControl == null)
                return;
            if (m_Hook.ArcGisMapControl.Map == null)
                return;
            IMap pMap = m_Hook.ArcGisMapControl.Map;
            if (pMap.LayerCount == 0)
                return;

            string XmlPath = Application.StartupPath + "\\..\\Res\\Xml\\展示图层树0.xml";
            SysCommon.ModSysSetting.CopyLayerTreeXmlFromDataBase(Plugin.ModuleCommon.TmpWorkSpace, XmlPath);
            FrmLandUseStatistic pFrm = null;
            DialogResult pResult = DialogResult.No ;
            try
            {
                pFrm = new FrmLandUseStatistic(XmlPath);
                pResult = pFrm.ShowDialog();
            }
            catch
            { }
            if (pResult == DialogResult.OK)   //行政区代码改为从数据中自动获取，不再由用户设置 20110919
            {

                string strXZQcode = pFrm._XZQcode;
                string strXZQname = pFrm._XZQmc;
                string strYear = pFrm._Year;
                string strAreaUnit = pFrm._AreaUnit;
                int FractionNum = pFrm._FractionNum;
                string ResPath = pFrm._ResultPath;
                pFrm = null;
                SysCommon.CProgress pProgress = new SysCommon.CProgress("森林资源现状分类面积统计");
                pProgress.EnableCancel = false;
                pProgress.ShowDescription = true;
                pProgress.FakeProgress = true;
                pProgress.TopMost = true;
                pProgress.ShowProgress();
                try
                {
                    DoStatistic(XmlPath, strXZQcode,strXZQname, strYear, strAreaUnit, FractionNum, ResPath,pProgress);
                }
                catch(Exception err) 
                {}
                pProgress.Close();
            }
            System.IO.File.Delete(XmlPath);
        }
        private void DoStatistic(string XmlPath, string XZQcode,string XZQName, string strYear,string strAreaUnit,int FractionNum,string ResultPath, SysCommon.CProgress vProgress)
        {
            string DLTBNodeKey = "";
            string XZDWNodeKey = "";
            string LXDWNodeKey = "";
            //SysCommon.Log.SysLocalLog pLog = new SysCommon.Log.SysLocalLog();
            //pLog.CreateLogFile(Application.StartupPath+"\\..\\Log\\统计.log");
            ModStatReport.WriteStaticLog("查找森林资源现状数据");
            //pLog.WriteLocalLog("查找森林资源现状数据");
            ModStatReport.GetTDLYLayerKey(XmlPath, XZQcode, strYear, out DLTBNodeKey, out XZDWNodeKey, out LXDWNodeKey);
            if (DLTBNodeKey == "")
            {
                vProgress.Close();
                MessageBox.Show("找不到该行政区的森林资源现状数据!");                
                return;
            }
            IFeatureClass pDLTBFeaCls = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, XmlPath, DLTBNodeKey);
            IFeatureClass pXZDWFeaCls = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, XmlPath, XZDWNodeKey);
            IFeatureClass pLXDWFeaCls = SysCommon.ModSysSetting.GetFeatureClassByNodeKey(Plugin.ModuleCommon.TmpWorkSpace, XmlPath, LXDWNodeKey);
            if (pDLTBFeaCls == null)
            {
                vProgress.Close();
                MessageBox.Show("找不到该行政区的森林资源现状数据!");                
                return;
            }
            ModStatReport.WriteStaticLog("创建临时成果数据库");
            //pLog.WriteLocalLog("创建临时成果数据库");
            //string workpath = Application.StartupPath + @"\..\OutputResults\统计成果\" + System.DateTime.Now.ToString("yyyyMMddHHmmss");
            string strMDBName = XZQName+strYear +"年LandUse"+System.DateTime.Now.ToString("yyyyMMddHHmmss")+".mdb";
            string workSpaceName = ResultPath + "\\" + strMDBName;
            //判断结果目录是否存在，不存在则创建
            if (System.IO.Directory.Exists(ResultPath) == false)
                System.IO.Directory.CreateDirectory(ResultPath);
            //创建一个新的mdb数据库,并打开工作空间
            if (vProgress != null)
            {
                vProgress.SetProgress("创建结果库...");
            }
            IWorkspace pOutWorkSpace = ChangeJudge.CreatePDBWorkSpace(ResultPath, strMDBName);


            string strDLTBname = "";
            string strXZDWname = "";
            string strLXDWname = "";
            if (pDLTBFeaCls != null)
            {
                ModStatReport.WriteStaticLog("拷贝地类图斑数据");
                CopyPasteGDBData.CopyPasteGeodatabaseData((pDLTBFeaCls as IDataset).Workspace, pOutWorkSpace, (pDLTBFeaCls as IDataset).Name, esriDatasetType.esriDTFeatureClass);

                strDLTBname = (pDLTBFeaCls as IDataset).Name;
                strDLTBname = strDLTBname.Substring(strDLTBname.IndexOf(".") + 1);//added by chulili 20110922 去掉oracle中用户名
            }
            else
            {
                strDLTBname = "";
            }
            if (pXZDWFeaCls != null)
            {
                ModStatReport.WriteStaticLog("拷贝线状地物数据");
                CopyPasteGDBData.CopyPasteGeodatabaseData((pXZDWFeaCls as IDataset).Workspace, pOutWorkSpace, (pXZDWFeaCls as IDataset).Name, esriDatasetType.esriDTFeatureClass);
                strXZDWname = (pXZDWFeaCls as IDataset).Name;
                strXZDWname = strXZDWname.Substring(strXZDWname.IndexOf(".") + 1);//added by chulili 20110922 去掉oracle中用户名
            }
            else
            {
                strXZDWname = "";
            }
            if (pLXDWFeaCls != null)
            {
                ModStatReport.WriteStaticLog("拷贝零星地物数据");
                CopyPasteGDBData.CopyPasteGeodatabaseData((pLXDWFeaCls as IDataset).Workspace, pOutWorkSpace, (pLXDWFeaCls as IDataset).Name, esriDatasetType.esriDTFeatureClass);
                strLXDWname = (pLXDWFeaCls as IDataset).Name;
                strLXDWname = strLXDWname.Substring(strLXDWname.IndexOf(".") + 1);//added by chulili 20110922 去掉oracle中用户名
            }
            else
            {
                strLXDWname = "";
            }


            ModStatReport.WriteStaticLog("拷贝行政区字典表");
            CopyPasteGDBData.CopyPasteGeodatabaseData(Plugin.ModuleCommon.TmpWorkSpace, pOutWorkSpace, Plugin.ModuleCommon.TmpWorkSpace.ConnectionProperties.GetProperty("User").ToString() + ".行政区字典表", esriDatasetType.esriDTTable);
            string strExcelName = XZQName + strYear + "年森林资源现状统计表.xls";
            ModStatReport.WriteStaticLog("拷贝森林资源现状统计模板");
            if (File.Exists(ResultPath + "\\" + strExcelName))
            {
                File.Delete(ResultPath + "\\" + strExcelName);
            }
            File.Copy(Application.StartupPath + "\\..\\Template\\森林资源现状统计模板.xls", ResultPath + "\\" + strExcelName);
            
            ModStatReport.WriteStaticLog("生成基础统计表");
             
            ModStatReport.DoLandUseStatic(workSpaceName, strDLTBname, strXZDWname, strLXDWname, vProgress);

            ModStatReport.WriteStaticLog("生成森林资源现状一级分类面积统计表");
            ModStatReport.LandUseCurReport(ResultPath, strMDBName, XZQcode, strAreaUnit, FractionNum, 1, ResultPath + "\\一级分类面积.xls", vProgress);
           
            ModStatReport.WriteStaticLog("生成森林资源现状二级分类面积统计表");
            ModStatReport.LandUseCurReport(ResultPath, strMDBName, XZQcode, strAreaUnit, FractionNum, 2, ResultPath + "\\二级分类面积.xls", vProgress);
            

            ModStatReport.WriteStaticLog("拷贝EXCEL中的工作区表");
            ModStatReport.CopyExcelSheet(ResultPath + "\\一级分类面积.xls", "Sheet1", ResultPath + "\\" + strExcelName, "森林资源现状一级分类面积");
            ModStatReport.CopyExcelSheet(ResultPath + "\\二级分类面积.xls", "Sheet1", ResultPath + "\\" + strExcelName, "森林资源现状二级分类面积");
            File.Delete(ResultPath + "\\一级分类面积.xls");
            File.Delete(ResultPath + "\\二级分类面积.xls");
            ModStatReport.WriteStaticLog("打开森林资源现状统计表");
            ModStatReport.OpenExcelFile(ResultPath + "\\" + strExcelName);
            vProgress.Close();
            ModStatReport.WriteStaticLog("统计结束");
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pOutWorkSpace);
            pOutWorkSpace = null;
            try
            {
                File.Delete(workSpaceName);
            }
            catch (Exception err)
            { }
            //pLog = null;
        }
       

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
        }
    }
}
