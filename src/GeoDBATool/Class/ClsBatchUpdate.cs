using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geometry;

namespace GeoDBATool
{
    class ClsBatchUpdate
    {
        public DevComponents.AdvTree.Node GetNodeOfProjectTree(DevComponents.AdvTree.AdvTree pTree, string strDataKey, string strNodeText)
        {
            for (int i = 0; i < pTree.Nodes.Count; i++)
            {
                DevComponents.AdvTree.Node pTmpnode = pTree.Nodes[i];
                if (pTmpnode.DataKeyString == strDataKey && pTmpnode.Text == strNodeText)
                {
                    return pTmpnode;
                }
                if (pTmpnode.Nodes.Count > 0)
                {
                    DevComponents.AdvTree.Node pResNode = GetNodeOfProjectTree(pTmpnode, strDataKey, strNodeText);
                    if (pResNode != null)
                    {
                        return pResNode;
                    }
                }
            }
            return null;
        }
        private DevComponents.AdvTree.Node GetNodeOfProjectTree(DevComponents.AdvTree.Node pNode, string strDataKey, string strNodeText)
        {
            for (int i = 0; i < pNode.Nodes.Count; i++)
            {
                DevComponents.AdvTree.Node pTmpnode = pNode.Nodes[i];
                if (pTmpnode.DataKeyString == strDataKey && pTmpnode.Text == strNodeText)
                {
                    return pTmpnode;
                }
                if (pTmpnode.Nodes.Count > 0)
                {
                    DevComponents.AdvTree.Node pResNode = GetNodeOfProjectTree(pTmpnode, strDataKey, strNodeText);
                    if (pResNode != null)
                    {
                        return pResNode;
                    }
                }
            }
            return null;
        }
        private DevComponents.AdvTree.Node GetHisNodeOfProjectTree(DevComponents.AdvTree.Node pNode, string strDataKey, string strNodeText)
        {
            for (int i = 0; i < pNode.Nodes.Count; i++)
            {
                DevComponents.AdvTree.Node pTmpnode = pNode.Nodes[i];
                if (pTmpnode.DataKeyString == strDataKey && pTmpnode.Text == strNodeText + "_GOH")
                {
                    return pTmpnode;
                }
                if (pTmpnode.Nodes.Count > 0)
                {
                    DevComponents.AdvTree.Node pResNode = GetNodeOfProjectTree(pTmpnode, strDataKey, strNodeText);
                    if (pResNode != null)
                    {
                        return pResNode;
                    }
                }
            }
            return null;
        }
        public void DoBatchUpdate(IWorkspace pCurWorkSpace, IWorkspace pHisWorkSpace, IWorkspace pUptWorkSpace, IGeometry pUptGeometry, DevComponents.AdvTree.Node pCurNode, DevComponents.AdvTree.Node pHisNode, FrmProcessBar frmbar)
        {
            IFeatureWorkspace pCurFeaWKS = pCurWorkSpace as IFeatureWorkspace;
            IFeatureWorkspace pHisFeaWKS = pHisWorkSpace as IFeatureWorkspace;
            IFeatureWorkspace pUptFeaWKS = pUptWorkSpace as IFeatureWorkspace;
            int barcnt = 0;
            frmbar.SetFrmProcessBarMax(100);
            frmbar.SetFrmProcessBarText("正在执行范围更新");
            int FDcnt = pCurNode.Nodes.Count;
            for (int i = 0; i < pCurNode.Nodes.Count; i++)
            {
                DevComponents.AdvTree.Node pTmpNode = pCurNode.Nodes[i];
                if (pTmpNode.DataKeyString == "FD")
                {
                    DevComponents.AdvTree.Node pTmpHisNode = GetHisNodeOfProjectTree(pHisNode, "FD", pTmpNode.Text);
                    int FCcnt = pTmpNode.Nodes.Count;
                    for (int j = 0; j < pTmpNode.Nodes.Count; j++)
                    {
                        DevComponents.AdvTree.Node pFCnode = pTmpNode.Nodes[j];
                        if (pFCnode.DataKeyString == "FC")
                        {
                            DevComponents.AdvTree.Node pHisFCnode = GetHisNodeOfProjectTree(pTmpHisNode, "FC", pFCnode.Text);
                            IFeatureClass pCurFeatureClass = pCurFeaWKS.OpenFeatureClass(pFCnode.Text);
                            IFeatureClass pHisFeatureClass = null;
                            if (pHisFCnode != null)
                            {
                                pHisFeatureClass = pHisFeaWKS.OpenFeatureClass(pHisFCnode.Text);
                            }
                            IFeatureClass pUptFeatureClass = null;
                            pUptFeatureClass = pUptFeaWKS.OpenFeatureClass(pFCnode.Text);
                            if (pHisFeatureClass != null && pUptFeatureClass != null)
                            {
                                double dValue = (i + 1) / FDcnt * (j + 1) / FCcnt * 100;
                                int value = (int)(Math.Floor(dValue));
                                if (value > barcnt)
                                {
                                    frmbar.SetFrmProcessBarValue(value);
                                    Application.DoEvents();
                                    barcnt = value;
                                }
                                frmbar.SetFrmProcessBarText("写历史库："+pFCnode.Text);
                                WriteHisOfFeatureClass(pCurFeatureClass, pHisFeatureClass, pUptGeometry);
                                frmbar.SetFrmProcessBarText("更新现状数据："+pFCnode.Text);
                                DoUpdateOfFeatureClass(pCurFeatureClass,pUptFeatureClass , pUptGeometry);
                            }
                        }

                    }
                }
            }
        }
        //写历史
        private void WriteHisOfFeatureClass(IFeatureClass pCurFeatureClass, IFeatureClass pHisFeatureClass, IGeometry pGeometry)
        {
            if (pCurFeatureClass == null)
            {
                return;
            }
            if (pHisFeatureClass == null)
            {
                return;
            }
            IWorkspace pTagetWorkspace = null;
            try
            {
                pTagetWorkspace = (pHisFeatureClass as IDataset).Workspace;
            }
            catch
            { }
            IWorkspaceEdit wsEdit = pTagetWorkspace as IWorkspaceEdit;
            if (wsEdit.IsBeingEdited() == true)
            {
                wsEdit.StopEditing(true);
            }
            wsEdit.StartEditing(false);
            wsEdit.StartEditOperation();
            ISpatialFilter pFilter = new SpatialFilterClass();
            pFilter.Geometry = pGeometry;
            pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;
            IFeatureCursor pFeaCursor = pCurFeatureClass.Search(pFilter, false);
            if (pFeaCursor != null)
            {
                Exception err0 = null;
                System.DateTime pDate = DateTime.Today;
                NewFeatures(pHisFeatureClass, pFeaCursor, "ZZRQ", DateTime.Today.ToShortDateString() as object , true, null, out err0);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);
                pFeaCursor = null;
                if (err0 == null)
                {
                    wsEdit.StopEditOperation();
                    wsEdit.StopEditing(true);
                }
                else
                {
                    wsEdit.StopEditOperation();
                    wsEdit.StopEditing(false);
                }
            }
            else
            {
                wsEdit.StopEditOperation();
                wsEdit.StopEditing(false);
            }
            

        }
        //删除现状库中原有数据，将更新数据写到现状数据库中
        private void DoUpdateOfFeatureClass(IFeatureClass pCurFeatureClass, IFeatureClass pUptFeatureClass, IGeometry pGeometry)
        {
            if (pCurFeatureClass == null)
            {
                return;
            }
            if (pUptFeatureClass == null)
            {
                return;
            }
            ISpatialFilter pFilter = new SpatialFilterClass();
            pFilter.Geometry = pGeometry;
            pFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains;  //更新范围包含的地物

            IWorkspace pTagetWorkspace = null;
            try
            {
                pTagetWorkspace = (pCurFeatureClass as IDataset).Workspace;
            }
            catch
            { }
            //(pTagetWorkspace as IWorkspaceEdit).StartEditing(false);
            IWorkspaceEdit wsEdit = pTagetWorkspace as IWorkspaceEdit;
            if (wsEdit.IsBeingEdited() == true)
            {
                wsEdit.StopEditing(true);
            }
            wsEdit.StartEditing(false);
            wsEdit.StartEditOperation();


           
            //删除现状库中地物
            
            IFeatureCursor pFeaCursor = pCurFeatureClass.Update(pFilter, false);
            if (pFeaCursor != null)
            {                
                IFeature pFea = pFeaCursor.NextFeature();
                while (pFea != null)
                {
                    pFeaCursor.DeleteFeature();
                    pFea = pFeaCursor.NextFeature();
                }
                //pFeaCursor.Flush();    
                
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeaCursor);
                pFeaCursor = null;
                
            }
            //从更新库向现状库写地物
            IFeatureCursor pUptFeacursor = pUptFeatureClass.Search(pFilter, false);
            if (pUptFeacursor != null)
            {
                Exception err0 = null;
                NewFeatures(pCurFeatureClass, pUptFeacursor,"QSRQ",DateTime.Today.ToShortDateString() as object, true, null, out err0);
                if (err0 == null)
                {
                    wsEdit.StopEditOperation();
                    wsEdit.StopEditing(true);
                }
                else
                {
                    wsEdit.StopEditOperation();
                    wsEdit.StopEditing(false);

                }
            }
            else
            {
                wsEdit.StopEditOperation();
                wsEdit.StopEditing(true);
            }
        }
        //参数bIngore：出错了是否忽略，继续写下一个地物
        private bool NewFeatures(IFeatureClass ObjFeatureCls, IFeatureCursor pfeacursor, string FieldName,object FieldValue, bool bIngore,DevComponents.DotNetBar.ProgressBarItem progressBar, out Exception eError)
        {
            eError = null;
            if (ObjFeatureCls == null) return false;
            int FieldIndex = -1;
            FieldIndex = ObjFeatureCls.Fields.FindField(FieldName);
            IFeatureBuffer pFeatureBuffer = ObjFeatureCls.CreateFeatureBuffer();
            IFeatureCursor pObjFeaCursor = ObjFeatureCls.Insert(true);
            IFeature pFeature = null;
            if (pObjFeaCursor != null)
            {
                pFeature = pfeacursor.NextFeature();
            }
            else
            {
                return false;
            }
            while (pFeature != null)
            {

                try
                {
                    //***************************************
                    //guozheng 2011-4-11 added 对空要素的保护
                    //空要素不需要
                    if (pFeature.Shape != null)  //wgf 20111109 死机
                    {
                        if (pFeature.Shape.IsEmpty)
                        {
                            pFeature = pfeacursor.NextFeature();
                            continue;
                        }
                    }
                    else
                    {
                        pFeature = pfeacursor.NextFeature();
                        continue;
                    }
                    //****************************************

                        for (int i = 0; i < pFeature.Fields.FieldCount; i++)
                        {
                            IField aField = pFeature.Fields.get_Field(i);
                            if (aField.Type != esriFieldType.esriFieldTypeGeometry && aField.Type != esriFieldType.esriFieldTypeOID && aField.Editable)
                            {
                                int ObjIndex = pFeatureBuffer.Fields.FindField(aField.Name);
                                if (ObjIndex == -1) continue;

                                pFeatureBuffer.set_Value(ObjIndex, pFeature.get_Value(i));
                            }
                        }
                        if (FieldIndex > 0)
                        {
                            try
                            {
                                pFeatureBuffer.set_Value(FieldIndex, FieldValue);
                            }
                            catch(Exception err)
                            { }
                        }

                    pFeatureBuffer.Shape = pFeature.ShapeCopy;
                    pObjFeaCursor.InsertFeature(pFeatureBuffer);

                    pFeature = pfeacursor.NextFeature();
                    if (progressBar != null)
                    {
                        progressBar.Value++;
                    }
                    Application.DoEvents();
                }
                catch (Exception eX)
                {
                    //*******************************************************************
                    //guozheng added
                    if (ModData.SysLog != null)
                    {
                        ModData.SysLog.Write(eX, null, DateTime.Now);
                    }
                    else
                    {
                        ModData.SysLog = new SysCommon.Log.clsWriteSystemFunctionLog();
                        ModData.SysLog.Write(eX, null, DateTime.Now);
                    }
                    //********************************************************************
                    eError = eX;
                    if (bIngore == false)
                        break;
                    else
                        pFeature = pfeacursor.NextFeature();
                    if (progressBar != null)
                    {
                        progressBar.Value++;
                    }
                    Application.DoEvents();
                    continue;
                }
            }

            pObjFeaCursor.Flush();
            Marshal.ReleaseComObject(pObjFeaCursor);
            return true;
        }
    }
}
