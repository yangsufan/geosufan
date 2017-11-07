using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDBIntegration
{
    public class ControlsRefreshDBTree: Plugin.Interface.CommandRefBase
    {
        //********************************************************//
        /*
         * 功能描述：刷新树图
         * 开 发 者：陈亚飞
         * 开发时间：2011-07-13
         */
        //********************************************************//
          private Plugin.Application.IAppDBIntegraRef m_Hook;
          public ControlsRefreshDBTree()
        {
            base._Name = "GeoDBIntegration.ControlsRefreshDBTree";
            base._Caption = "加载数据库图层";
            base._Tooltip = "加载数据库图层";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "加载数据库图层";

        }
        public override bool Enabled
        {
            get
            {
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
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
            Exception ex = null;
            if (ModuleData.v_DataBaseProPanel != null)
            {
                //清空界面图标
                ModuleData.v_DataBaseProPanel.RemoveAllDataBasePro();
                //清空树图
                m_Hook.ProjectTree.SelectedNode.Nodes.Clear();
                //重新加载界面
                clsAddAppDBConnection addAppDB = new clsAddAppDBConnection();
                //判断系统维护库的连接信息是否正确
                addAppDB.JudgeAppDbConfiguration(out ex);
                if (ex != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "界面刷新化失败，\n原因:" + ex.Message);
                    return;
                }
                #region cyf 20110627 add:初始化工程树图
                IFeatureWorkspace pFeaWS = ModuleData.TempWks as IFeatureWorkspace;
                if (pFeaWS == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "连接系统维护库失败！");
                    return;
                }
                string pTableName = "DATABASETYPEMD";
                string pFieldNames = "DATABASETYPE";
                ICursor pCursor = ModDBOperate.GetCursor(pFeaWS, pTableName, pFieldNames, "", out ex);
                if (ex != null || pCursor == null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "查询数据库系统维护库中的数据库类型表失败！");
                    return;
                }
                IRow pRow = pCursor.NextRow();
                //遍历行，将树节点加载在树图上
                while (pRow != null)
                {
                    string pDBType = pRow.get_Value(0).ToString();  //数据库类型
                    DevComponents.AdvTree.Node pNode = new DevComponents.AdvTree.Node();
                    pNode.Expanded = true;
                    pNode.Name = "node2";
                    pNode.TagString = "Database";
                    pNode.Text = pDBType;
                    pNode.Image = m_Hook.ProjectTree.ImageList.Images[1];  //cyf 20110711 添加图标
                    m_Hook.ProjectTree.SelectedNode.Nodes.Add(pNode);
                    pRow = pCursor.NextRow();
                }
                //释放游标
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                m_Hook.ProjectTree.Refresh();
                #endregion

                //刷新界面
                while (!addAppDB.refurbish(out ex))
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "界面初始化化失败，\n原因:" + ex.Message);
                    return;
                }
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
            if (m_Hook == null) return;
        }
    }
}
