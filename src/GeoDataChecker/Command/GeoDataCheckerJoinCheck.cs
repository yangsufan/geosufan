using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Data;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Display;

namespace GeoDataChecker
{
    /// <summary>
    /// 功能：接边检查 查出在一定范围内应该要接边的两个要素而没接上
    /// 编写人：王冰
    /// 重新整理：王晶晶 
    /// 再次整理:王冰
    /// 
    /// 接边检查的思想：
    /// 总体思想：应查出应当接边而没有接上的两个要素，如是已接上而没有融合的，则不作为错误查出。
    /// 接边的开发思想：（以图幅为参照）
    /// 1、首先取得单个图幅的外边框
    /// 2、针对边框进行缓冲
    /// 3、将得到的这个缓冲范围和原来的图幅边框进行求交，最终得到的是在图幅内的一部分缓冲区
    /// 4、取出在这个部分缓冲区内的所有要素
    /// 5、针对每一个要素进行取点集合，确定在部分缓冲区内的点，将这些点组织在一起作为查询的对象
    /// 6、以X，Y为参照，确定两个点是不是接上，其中取这个点的参照是以边框为参照的最近点
    /// 7、用点集合里的每一个点和边框进行求差，得到非边框内的要素，然后再配合我们选择的属性，确定应该要接边的要素而没有接上，显示这些要素
    /// 
    /// </summary>

    public class JoinCheck : Plugin.Interface.CommandRefBase
    {
        
        private Plugin.Application.IAppGISRef _AppHk;
        public JoinCheck()
        {
            base._Name = "GeoDataChecker.JoinCheck";
            base._Caption = "接边检查";
            base._Tooltip = "接边检查";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "接边检查";

        }
       
        public override bool Enabled
        {
            get
            {
                try
                {
                    //if (_AppHk.MapControl.LayerCount == 0)
                    //{
                    //    base._Enabled = false;
                    //    return false;
                    //}
                    //else
                    //{
                    //    if (SetCheckState.CheckState)
                    //    {
                    //        base._Enabled = true;
                    //        return true;
                    //    }
                    //    else
                    //    {
                    //        base._Enabled = false;
                    //        return false;
                    //    }
                    //}
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
            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            //执行等高线注记检查
            //FrmMathematicsCheck mFrmMathematicsCheck = new FrmMathematicsCheck(_AppHk, enumErrorType.接边检查);
            SetJoinChecks frmSetJoinCheck = new SetJoinChecks(_AppHk);
            frmSetJoinCheck.ShowDialog();

            //FromJoinCheck = new SetJoinChecks(_AppHk);//实例设置接边属性的窗体
            //FromJoinCheck.Show();
            //Overridfunction.name = "接边检查";
            ////高亮显示处理结果栏
            //DevComponents.DotNetBar.PanelDockContainer PanelTip = _AppHk.DataTree.Parent as DevComponents.DotNetBar.PanelDockContainer;
            //if (PanelTip != null)
            //{
            //    PanelTip.DockContainerItem.Selected = true;
            //}
            

        }
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;

        }


    }
}
