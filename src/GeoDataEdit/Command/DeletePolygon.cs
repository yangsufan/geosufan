using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.ADF.BaseClasses;

namespace GeoDataEdit
{
    public class DeletePolygon : Plugin.Interface.ToolRefBase
    {

        private Plugin.Application.IAppArcGISRef _AppHk;


        public DeletePolygon()
        {
            base._Name = "GeoDataEdit.DeletePolygon";
            base._Caption = "删除面";
            base._Tooltip = "删除面";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "删除面";
            //base._Image = "";
            //base._Category = "";
        }





        public override bool Checked
        {
            get
            {
                if (_AppHk.CurrentTool != this.Name) return false;
                return true;
            }
        }

        public override bool Enabled
        {
            get
            {
                try
                {
                    if (_AppHk.MapControl.Map.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }

                    base._Enabled = true;
                    return true;
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
            }
        }

        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        }

        public override void OnClick()
        {
            if ( _AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            IFeatureLayer tmpFeatureLayer = getEditLayer.isExistLayer(_AppHk.MapControl.Map) as IFeatureLayer;
            if (tmpFeatureLayer == null || tmpFeatureLayer.FeatureClass.ShapeType != esriGeometryType.esriGeometryPolygon)
            {
                MessageBox.Show("请设置编辑图层为面图层！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            IWorkspaceEdit iWE = (tmpFeatureLayer.FeatureClass as IDataset).Workspace as IWorkspaceEdit;
            if (!iWE.IsBeingEdited())
                iWE.StartEditing(false);
            ISelectionSet iSS = (tmpFeatureLayer as IFeatureSelection).SelectionSet;
            ICursor iCursor;
            iSS.Search(null, false, out iCursor);
            IFeatureCursor iFC = iCursor as IFeatureCursor;
            IFeature tmpFeature = iFC.NextFeature();
            if (tmpFeature == null)
                return;
            if (tmpFeature.ShapeCopy.GeometryType != esriGeometryType.esriGeometryPolygon)
            {
                MessageBox.Show("请选择面要素！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("是否删除选择的面！", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
                return;
            while (tmpFeature != null)
            {
                tmpFeature.Delete();
                tmpFeature = iFC.NextFeature();

            }
            iWE.StopEditing(true);
            iWE = null;
            _AppHk.MapControl.ActiveView.Refresh();
        

            
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;

            Plugin.Application.IAppFormRef pAppForm = hook as Plugin.Application.IAppFormRef;
        


        }

        private IFeatureLayer layerCurSeleted()
        {
            IMap pMap = _AppHk.MapControl.Map;
            int iLayerCount = pMap.LayerCount;
            IFeatureLayer pFeatureLayer;
            IFeatureLayer pCurSeLayer = null;

            int countSelectable = 0;
            for (int n = 0; n < iLayerCount; n++)
            {
                if (countSelectable >= 2) break;
                ILayer layer = pMap.get_Layer(n);
                if (layer is IGroupLayer)
                {
                    ICompositeLayer Comlayer = layer as ICompositeLayer;//将一个具有组的层进行转换成一个组合层，使它可以遍历
                    for (int c = 0; c < Comlayer.Count; c++)
                    {
                        pFeatureLayer = Comlayer.get_Layer(c) as IFeatureLayer;
                        if (pFeatureLayer != null && pFeatureLayer.Selectable)
                        {
                            countSelectable++;
                            pCurSeLayer = pFeatureLayer;

                        }

                    }
                }
            }//for
            if (countSelectable != 1)
                return null;
            else
                return pCurSeLayer;




        }

    }
}
