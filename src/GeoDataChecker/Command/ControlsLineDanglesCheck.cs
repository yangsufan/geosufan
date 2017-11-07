using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace GeoDataChecker
{
    //还需手动写拓扑算法进行改进和完善
    public class ControlsLineDanglesCheck:Plugin.Interface.CommandRefBase
    {
       private Plugin.Application.IAppGISRef _AppHk;

        public ControlsLineDanglesCheck()
        {
            base._Name = "GeoDataChecker.ControlsLineDanglesCheck";
            base._Caption = "线悬挂点检查";
            base._Tooltip = "检查线要素中是否有悬结点，即每一条线段的端点是否孤立";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "线悬挂点检查";
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

            //string oriFeaClsName = "GB500_PIP_LN";
            //string desFeaClsName = "GB500_RES_PY";
            ////string desFeaClsName = "GB500_RES_LN";
            ////string desFeaClsName = "GB500_PIP_LN";

            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;

            //执行线悬挂点检查
            FrmMathematicsCheck mFrmMathematicsCheck = new FrmMathematicsCheck(_AppHk, enumErrorType.线存在悬挂点);
            mFrmMathematicsCheck.ShowDialog();

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

            //    //执行线悬挂点检查
            //    DataCheckClass dataCheckCls = new DataCheckClass(_AppHk);
            //    if (oriFeaClsName == desFeaClsName)
            //    {
            //        //同层面悬挂点检查
            //        dataCheckCls.OrdinaryTopoCheck(pFeaDataset, oriFeaClsName, esriTopologyRuleType.esriTRTLineNoDangles, out eError);
            //        if (eError != null)
            //        {
            //            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "线悬挂点检查失败！" + eError.Message);
            //            return;
            //        }
            //    }
            //    else
            //    {
            //        string oriStr = "GISID='1'";//1,2
            //        string desStr = "GISID='31090030'";//38020520,31090030
            //        double tolerence = 0.5;
            //        dataCheckCls.LineDangleCheck(pFeaDataset, oriFeaClsName, oriStr, desFeaClsName, desStr, tolerence, out eError);
            //        if (eError != null)
            //        {
            //            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "线悬挂点检查失败！" + eError.Message);
            //            return;
            //        }
            //    }
            //}

            //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "线悬挂点检查完成!");
        }


       public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;
        }
    }
}
