using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using SysCommon.Authorize;

namespace GeoDBIntegration
{
    /// <summary>
    /// 卸载数据库操作  陈亚飞 20100927
    /// </summary>
    public class ControlsDelDBConn: Plugin.Interface.CommandRefBase
    {
      
        private Plugin.Application.IAppDBIntegraRef m_Hook;
        //added by chulili 20110624 为了判断可用状态添加变量
        private Plugin.Application.IAppFormRef _hook;
        public ControlsDelDBConn()
        {
            base._Name = "GeoDBIntegration.ControlsDelDBConn";
            base._Caption = "删除数据库连接信息";
            base._Tooltip = "删除数据库连接信息";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "删除数据库连接信息";

        }
        public override bool Enabled
        {
            get
            {
                //若没有选中数据库工程节点，则按钮不可用
                if (m_Hook == null) return false;
                if (m_Hook.ProjectTree == null || m_Hook.MapControl == null || m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.Tag == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey == null) return false;


                //cyf 20110602 modify
                //若没有登录系统，则按钮不可用
                if ((m_Hook as Plugin.Application.IAppFormRef).LstRoleInfo == null) return false;
                //若用户不是管理员，则按钮不可用
                bool beAdmin = false;
                //added by chulili 20110624 若不处于数据源管理界面  菜单不可用
                if (!(_hook.MainForm.Controls[0] is UserControlDBIntegra))
                {
                    return false;
                }

                if (_hook.MainForm.Controls[0].Visible == false)
                {
                    return false;
                }

                //end add
                foreach (Role pRole in (m_Hook as Plugin.Application.IAppFormRef).LstRoleInfo)
                {
                    if (pRole.TYPEID == EnumRoleType.管理员.GetHashCode().ToString())
                    {
                        beAdmin = true;
                        break;
                    }
                }
                return true;
                //end
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

        //cyf 20110602 modify:系统维护库连接方式
        public override void OnClick()
        {
           //执行卸载数据库操作
            //XmlDocument XmlDoc = new XmlDocument();
            //string sConnect = string.Empty;
            string sDBtype=string.Empty;
            string sDBName=string.Empty;
            Exception ex=null;
            long lDBid = -1;
            #region 原有代码
            //if (File.Exists(ModuleData.v_AppDBConectXml))
            //{
            //    XmlDoc.Load(ModuleData.v_AppDBConectXml);
            //    XmlElement ele = XmlDoc.SelectSingleNode(".//系统维护库连接信息") as XmlElement;
                //if (ele != null)
                //{
                    try
                    {
                        //sConnect = ele.GetAttribute("连接字符串");
                        XmlElement eleinfo = m_Hook.ProjectTree.SelectedNode.Tag as XmlElement;
                        lDBid = Convert.ToInt64(eleinfo.GetAttribute("数据库ID"));
                        sDBtype = eleinfo.GetAttribute("数据库类型");
                        //cyf 20110626 modify:
                        sDBName = m_Hook.ProjectTree.SelectedNode.Text;// eleinfo.GetAttribute("数据库工程名");
                        //end
                    } catch
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取数据库信息失败");
                        return;
                    }
                //}
                //else { SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取系统维护库连接信息失败"); return; }
            //}
            //else
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "缺失系统维护库连接信息文件：" + ModuleData.v_AppDBConectXml);
            //    return;
            //}
                    /////
                    
            #endregion
                    bool bInLayerTree = ModDBOperate.IsDbSourceInLayerTree(lDBid);  //added by chulili 20111118 判断当前图层目录中是否存在该数据源
                    if (bInLayerTree)
                    {
                        if (!SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "当前图层目录中已引用该数据源，确定卸载数据源：" + sDBName + "?"))
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (!SysCommon.Error.ErrorHandle.ShowFrmInformation("是", "否", "确定卸载数据源：" + sDBName + "?"))
                        {
                            return;
                        }
                    }
            ///////
            //clsDBAdd DBdeloper = new clsDBAdd(SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, sConnect);
            //cyf 20110602 modify
            clsDBAdd DBdeloper = new clsDBAdd();
            //end
            ////////执行删除////////////
           if(!DBdeloper.DelDB(lDBid,out ex))
           {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示","数据库卸载失败，\n原因："+ex.Message);
               return;
           }

           m_Hook.ProjectTree.SelectedNode.Remove();//////删除树节点
            //cyf 20110627 modify
           //if (sDBtype == enumInterDBType.成果文件数据库.ToString())
           //{
           //    ///////guozheng added ,若是卸载文件库，将文件库的XMl文件v_FTPCoonectionInfoXML 删除，这一文件会在重新选择文件库工程时会自动生成
           //    try
           //    {
           //        System.IO.File.Delete(ModuleData.v_FTPCoonectionInfoXML);
           //    }
           //    catch
           //    {
           //    }
           //}
            //end
            /////////刷新界面//////////////
            /////////////子数据库Combox刷新
           //clsRefurbishDBinfo RefOper = new clsRefurbishDBinfo(SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, sConnect);
           //cyf 20110602 modify
            //clsRefurbishDBinfo RefOper = new clsRefurbishDBinfo();
            //end
            //cyf 20110602 delete:界面修改后，没有combobox列表框
            //RefOper.UpDataComBox(sDBtype, out ex);
           //if (ex != null)
           //{
           //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库卸载成功，界面刷新失败，\n原因：" + ex.Message);
           //    return;
           //}
           //else
           //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "数据库卸载：" + sDBName + " 成功！");
            //end

            //刷新xml  chenyafei  20110222  modify 
           //if (sDBtype != enumInterDBType.成果文件数据库.ToString())
           //{
               //DeleteXmlNode(sDBtype, lDBid);  cyf 20110626 delete：不对工程树图XML做更新
           //}
            ////////////////////按钮处理////////////
           if (this.WriteLog)
           {
               Plugin.LogTable.Writelog(Caption);//xisheng 2011.07.09 增加日志
           }
            try
           {
               ModuleData.v_DataBaseProPanel.RemoveDataBasePro(sDBtype, lDBid);
           }
           catch
           {
           }
            
        }
        
        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppDBIntegraRef;
            //added by chulili 20110624
            _hook = hook as Plugin.Application.IAppFormRef;
            //end add
            if (m_Hook == null) return;
        }

        /// <summary>
        /// 删除xml文件中的节点，刷新界面
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="pProID">工程ID</param>
        /// <param name="pError"></param>
        private void DeleteXmlNode(string dbType, long pProID)
        {
            XmlDocument pXmlDoc = new XmlDocument();
            string xmlPath = "";//工程xml文件路径

            //若为框架要素库，将连接信息写入到xml中
            if (dbType == enumInterDBType.框架要素数据库.ToString())
            {
                //刷新xml
                xmlPath = ModuleData.v_feaProjectXML;
            }
            else if (dbType == "高程数据库")
            {
                //刷新xml
                //cyf 20110609 modify
                xmlPath = ModuleData.v_feaProjectXML;// v_DEMProjectXml;
            }
            else if (dbType == "影像数据库")
            {
                //刷新xml
                //cyf 20110609 modify
                xmlPath = ModuleData.v_feaProjectXML;// v_ImageProjectXml;
            }
            else if (dbType == "地名数据库")
            {
                //刷新xml
                //xmlPath = ModuleData.V_A
            }
            else if (dbType == "地理编码数据库")
            {
                //刷新xml
                //xmlPath = ModuleData.v_ImageProjectXml;
            }
            if (xmlPath != "")
            {
                try { pXmlDoc.Load(xmlPath); } catch
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "更新xml文件失败\n"+xmlPath);
                    return;
                }
                XmlNode pNode = pXmlDoc.SelectSingleNode(".//工程管理//工程[@编号='" + pProID + "']");
                //added by chulili 20110630 卸载数据源出错,添加排错代码
                if (pNode == null)
                {
                    return;
                }
                pXmlDoc.DocumentElement.RemoveChild(pNode);
                pXmlDoc.Save(xmlPath);
            }
        }
    }
}
