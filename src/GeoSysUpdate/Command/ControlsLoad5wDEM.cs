using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Carto;

using System.IO;
using System.Xml;
namespace GeoSysUpdate
{
    public class ControlsLoad5wDEM : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        private Plugin.Application.AppGidUpdate _hook;
        
        public ControlsLoad5wDEM()
        {
            base._Name = "GeoSysUpdate.ControlsLoad5wDEM";
            base._Caption = "5万DEM";
            base._Tooltip = "5万DEM";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "5万DEM";
        }

        public override bool Enabled
        {
            get
            {
                if (_AppHk.MapControl == null || _AppHk.TOCControl == null) return false;
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
            if (_hook == null)
                return;
            if (_hook.MainUserControl == null)
                return;

            if (!File.Exists(ModDBOperate._QueryConfigXmlPath))
            {
                return;
            }
            //读取配置文件信息
            XmlDocument pXmlDoc = new XmlDocument();
            pXmlDoc.Load(ModDBOperate._QueryConfigXmlPath);
            string strSearch = "//LoadDataConfig/LoadItem[@ItemText='DEM']/NodeItem";
            XmlNode pXmlnode = pXmlDoc.SelectSingleNode(strSearch);
            if (pXmlnode == null)
            {
                return;
            }
            //获取节点的NodeKey
            string strNodeKey = pXmlnode.Attributes["NodeKey"].Value;
            pXmlDoc = null;
            GeoLayerTreeLib.LayerManager.UcDataLib pLayerTree = _hook.LayerTree as GeoLayerTreeLib.LayerManager.UcDataLib;
            DevComponents.AdvTree.Node pNode = pLayerTree.GetNodeByNodeKey(strNodeKey);

            //changed by chulili 20111118 加载或卸载指定NodeKey下面的数据
            if (base._Checked == false) //按钮先前未被按下
            {
                pNode.SetChecked(true, DevComponents.AdvTree.eTreeAction.Mouse);    //加载数据
                if (pNode.Checked)  //若数据被正确加载
                {
                    base._Checked = true;   //按钮置于按下状态 
                }
            }
            else //按钮先前是按下的状态
            {
                pNode.SetChecked(false, DevComponents.AdvTree.eTreeAction.Mouse);   //卸载数据
                if (!pNode.Checked) //若数据被成功卸载
                {
                    base._Checked = false;  //按钮置于非按下状态
                }
            }


            
            ////调用函数,加载该节点内容
            //UserControlSMPD pUserControl = _hook.MainUserControl as UserControlSMPD;
            //if (pUserControl != null)
            //{
            //    pUserControl.AddDataDir(strNodeKey);
            //}

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            _hook = hook as Plugin.Application.AppGidUpdate;
        }
    }
}
