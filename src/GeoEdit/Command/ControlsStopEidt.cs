using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using System.Windows.Forms;
namespace GeoEdit
{
    /// <summary>
    /// 结束编辑
    /// 
    /// </summary>
    public class ControlsStopEidt : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        public ControlsStopEidt()
        {
            base._Name = "GeoEdit.ControlsStopEidt";
            base._Caption = "结束编辑";
            base._Tooltip = "结束编辑";
            base._Checked = false;
            base._Enabled = false;
            base._Visible = true;

            base._Message = "结束编辑";
        }

        /// <summary>
        /// 确定是否有打开的数据，并控件显示
        /// </summary>
        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.MapControl == null) return false;
                if (MoData.v_CurWorkspaceEdit == null) return false;
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

        public override void OnClick()
        {
            if (!MoData.v_CurWorkspaceEdit.IsBeingEdited()) return;
            bool bHasEdits = false;
            DateTime SaveTime = DateTime.Now;////////////////////////////////////结束编辑的时间
            Dictionary<string, Dictionary<int, List<IRow>>> ChangeFea = null;///更新变化的要素
            MoData.v_CurWorkspaceEdit.HasEdits(ref bHasEdits);
            bool bSave = false;
            if (bHasEdits == true)   //如果进行了编辑则提示是否保存
            {
                if (SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "是否对编辑进行保存") == true)
                {
                    bSave = true;
                }
            }

            if (bSave)
            {
                //bool bLock = true;                      //true获得锁
                //bool bebortIfConflict = false;           //检测到冲突时，版本协调是否停止，true停止，false不停止
                bool bChildWin = false;                 //true替换上一个版本（冲突版本）,false用上一个版本（冲突版本）
                //bool battributeCheck = false;          //false若为true则只有修改同一个属性时才检测出冲突
                bool beMerge = true;                   //对于产生冲突的要素是否融合

                Exception eError = null;

                frmConflictSet pfrmConflictSet = new frmConflictSet();
                if (pfrmConflictSet.ShowDialog() == DialogResult.OK)
                {
                    bChildWin = pfrmConflictSet.BECHILDWIM;
                    beMerge = pfrmConflictSet.BEMERGE;
                }

                string pDefVerName = "";              //默认版本名称
                Dictionary<string, Dictionary<int, List<int>>> conflictFeaClsDic = null;  //产生冲突的要素类信息
                Dictionary<string, Dictionary<int, List<IRow>>> feaChangeDic = null;       //更新变化的要素
                //Dictionary<string, Dictionary<int, List<IRow>>> feaChangeSaveDic = null;   //保存编辑后更新变化的要素 

                ClsVersionReconcile pClsVersionReconcile = new ClsVersionReconcile(MoData.v_CurWorkspaceEdit);
                //获得默认版本名称
                pDefVerName = pClsVersionReconcile.GetDefautVersionName(out eError);
                if (eError != null || pDefVerName == "")
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                    return;
                }

                //进行版本协调时产生冲突的要素类信息
                conflictFeaClsDic = pClsVersionReconcile.ReconcileVersion(pDefVerName, bChildWin, beMerge, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                    return;
                }

                //获得发生更新变化的要素信息
                feaChangeDic = pClsVersionReconcile.GetModifyClsInfo(out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", eError.Message);
                    return;
                }
                ChangeFea = feaChangeDic;

            }

            MoData.v_CurWorkspaceEdit.StopEditing(bSave);
            if (bSave)
            {
                //更新日志记录表等
                //*******************************************************************************************
                //guozheng 2010-11-4 added
                //编辑完成，冲突处理完成后，写入更新日志表
                #region 编辑完成，冲突处理完成后，写入更新日志表
                Exception ex = null;
                if (ChangeFea != null)
                {
                    //////////远程更新日志处理对象
                    clsUpdataEnvironmentOper UpLogOper = new clsUpdataEnvironmentOper();
                    bool SaveLOG = true;
                    UpLogOper.HisWs = (IWorkspace)MoData.v_CurWorkspaceEdit;
                    foreach (KeyValuePair<string, Dictionary<int, List<IRow>>> item in ChangeFea)
                    {
                        string sLayerName = item.Key;
                        Dictionary<int, List<IRow>> FeatureDic = item.Value;
                        if (FeatureDic != null)
                        {
                            foreach (KeyValuePair<int, List<IRow>> item2 in FeatureDic)
                            {
                                int iState = item2.Key;
                                List<IRow> ListFea = item2.Value;
                                for (int count = 0; count < ListFea.Count; count++)
                                {
                                    IRow getRow = ListFea[count];
                                    UpLogOper.RecordLOG(getRow, iState, SaveTime, iState, sLayerName, out ex);
                                    if (ex != null)
                                    {
                                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "更新编辑记录失败。\n原因：" + ex.Message);
                                        SaveLOG = false;
                                        break;
                                    }
                                }
                                if (SaveLOG == false) break;
                            }
                        }
                    }
                    if (SaveLOG)
                    {
                        UpLogOper.WriteDBVersion(MoData.DBVersion, SaveTime, out ex);
                        if (ex != null)
                        {
                            SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "版本信息写入失败，\n原因：" + ex.Message);
                            // return;
                        }
                    }
                }
                #endregion
                //*******************************************************************************************
                //写入版本信息
                //*******************************************************************************************
            }

            MoData.v_CurWorkspaceEdit = null;




            ////保存日志记录表的修改
            //MoData.v_LogTable.EndTransaction(bSave);
            //MoData.v_LogTable.CloseDbConnection();
            //MoData.v_LogTable = null;

            m_Hook.CurrentTool = "";
            m_Hook.MapControl.CurrentTool = null;
            m_Hook.MapControl.Map.ClearSelection();
            m_Hook.MapControl.ActiveView.Refresh();

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }
    }
}
