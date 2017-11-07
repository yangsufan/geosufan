using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoDataChecker
{
   public class ControlsLineCoveredByAreaBoundaryCheck:Plugin.Interface.CommandRefBase
    {
       private Plugin.Application.IAppGISRef _AppHk;

        public ControlsLineCoveredByAreaBoundaryCheck()
        {
            base._Name = "GeoDataChecker.ControlsLineCoveredByAreaBoundaryCheck";
            base._Caption = "线面边界重合检查";
            base._Tooltip = "检查线是否被多边形要素的边界覆盖";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "线面边界重合检查";
        }

        public override bool Enabled
        {
            get
            {
                return true;
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
            //Exception eError = null;

            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;

            //执行线面重合检查
            FrmMathematicsCheck mFrmMathematicsCheck = new FrmMathematicsCheck(_AppHk, enumErrorType.线面边界重合检查);
            mFrmMathematicsCheck.ShowDialog();


            //string oriFeaClsName = "GB500_RES_LN";
            //string desFeaClsName="GB500_RES_PY";

            //SysCommon.Gis.SysGisDataSet pGisDT = new SysCommon.Gis.SysGisDataSet();
            //pGisDT.SetWorkspace(TopologyCheckClass.DataCheckPath, SysCommon.enumWSType.PDB , out eError );
            //if (eError != null)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接数据库出错！");
            //    return;
            //}
            //List<string> feaDatasetNameLst = pGisDT.GetAllFeatureDatasetNames();
            //for (int i = 0; i < feaDatasetNameLst.Count; i++)
            //{
            //    IFeatureDataset pFeaDataset = pGisDT.GetFeatureDataset(feaDatasetNameLst[i], out eError);
            //    if (eError != null)
            //    {
            //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据集失败,数据集名称为:"+feaDatasetNameLst[i]);
            //        continue;
            //    }

            //    //执行线面重合检查
            //    DataCheckClass dataCheckCls = new DataCheckClass(_AppHk);
            //    dataCheckCls.OrdinaryTopoCheck( pFeaDataset, oriFeaClsName,desFeaClsName,esriTopologyRuleType.esriTRTLineCoveredByAreaBoundary,out eError);
            //    if (eError != null)
            //    {
            //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "线面重合检查失败！"+eError.Message );
            //        return;
            //    }
            //}

            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "线面重合检查完成!");
        }


       public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;
        }
    }
}

