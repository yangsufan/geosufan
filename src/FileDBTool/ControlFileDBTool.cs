using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Plugin;
using DevComponents.DotNetBar;
using System.IO;
using System.Xml;

namespace FileDBTool
{
    public class ControlFileDBTool: Plugin.Interface.ControlRefBase
    {
        private Plugin.Application.IAppFormRef _hook;
        private UserControlFileDBTool _ControlFileDBTool;

        //构造函数
        public ControlFileDBTool()
        {
            base._Name = "FileDBTool.ControlFileDBTool";
            base._Caption = "文件库工程管理";
            base._Visible = false;
            base._Enabled = false;
        }

        public override bool Visible
        {
            get
            {
                try
                {
                    if (_hook == null)
                    {
                        base._Enabled = false;
                        return false;
                    }
                    if (_hook.CurrentSysName != base._Name)
                    {
                        base._Visible = false;
                        _ControlFileDBTool.Visible = false;
                        ModData.v_AppFileDB.StatusBar.Visible = false;
                        return false;
                    }

                    base._Visible = true;
                    _ControlFileDBTool.Visible = true;
                    ModData.v_AppFileDB.StatusBar.Visible = true;
                    return true;
                }
                catch
                {
                    base._Visible = false;
                    return false;
                }
                
            }
        }

        public override bool Enabled
        {
            get
            {
                try
                {
                    if (_hook != null)
                    {
                        if (_hook.CurrentSysName != base._Name)
                        {
                            base._Enabled = false;
                            _ControlFileDBTool.Enabled = false;
                            ModData.v_AppFileDB.StatusBar.Enabled = false;
                            return false;
                        }

                        base._Enabled = true;
                        _ControlFileDBTool.Enabled = true;
                        ModData.v_AppFileDB.StatusBar.Enabled = true;
                        return true;
                    }
                    else
                    {
                        base._Enabled = false;
                        return false;
                    }
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
             
            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            _hook = hook as Plugin.Application.IAppFormRef;

            if (_hook == null) return;
            ModData.v_AppFileDB = new Plugin.Application.AppFileDB(_hook.MainForm, _hook.ControlContainer, _hook.SystemXml, _hook.DataTreeXml, _hook.DatabaseInfoXml, _hook.ColParsePlugin, _hook.ImageResPath);

            _ControlFileDBTool = new UserControlFileDBTool(this.Name, this.Caption);

            _ControlFileDBTool.Show();

            _hook.MainForm.Controls.Add(_ControlFileDBTool);
            _hook.MainForm.Controls.Add(ModData.v_AppFileDB.StatusBar);
            _hook.MainForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MainForm_FormClosing);
            _ControlFileDBTool.EnabledChanged += new EventHandler(_ControlFileDBTool_EnabledChanged);  // Enable事件，用来触发数据库工程树图界面的初始化

            ModData.v_AppFileDB.RefScaleCmb.SelectedIndexChanged += new EventHandler(RefScaleCmb_SelectedIndexChanged);
            ModData.v_AppFileDB.CurScaleCmb.SelectedIndexChanged += new EventHandler(CurScaleCmb_SelectedIndexChanged);

            //添加回车事件自定义比例尺
            DevComponents.DotNetBar.Controls.ComboBoxEx vComboEx = ModData.v_AppFileDB.CurScaleCmb.ComboBoxEx;
            vComboEx.KeyDown += new KeyEventHandler(vComboEx_KeyDown);
            
        }

        //响应回车时间 改变当前显示比例尺
        void vComboEx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            DevComponents.DotNetBar.Controls.ComboBoxEx vComboEx = sender as DevComponents.DotNetBar.Controls.ComboBoxEx;
            string strScale = vComboEx.Text;
            double dblSacle = 0;

            try
            {
                if (double.TryParse(strScale, out dblSacle))
                {
                    ModData.v_AppFileDB.MapControl.Map.MapScale = dblSacle;
                    ModData.v_AppFileDB.MapControl.ActiveView.Refresh();
                }
                else
                {
                    vComboEx.Text = ModData.v_AppFileDB.MapControl.Map.MapScale.ToString();
                }
            }
            catch
            {
            }
        }

        //在退出系统前如正在处理数据应提示
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Plugin.Application.IAppFileRef pApp = ModData.v_AppFileDB as Plugin.Application.IAppFileRef;
            if (pApp == null) return;
            if (pApp.CurrentThread != null)
            {
                pApp.CurrentThread.Suspend();
                if (SysCommon.Error.ErrorHandle.ShowFrmInformation("确定", "取消", "当前任务正在进行,是否终止退出?") == true)
                {
                    pApp.CurrentThread.Abort();
                }
                else
                {
                    pApp.CurrentThread.Resume();
                    e.Cancel = true;
                }
            }
        }


        //参考比例尺事件　陈亚飞添加
        private void RefScaleCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModData.v_AppFileDB.MapControl.Map.ReferenceScale = Convert.ToDouble(ModData.v_AppFileDB.RefScaleCmb.SelectedItem.ToString().Trim());
            }
            catch
            {
                ModData.v_AppFileDB.RefScaleCmb.Text = ModData.v_AppFileDB.MapControl.Map.ReferenceScale.ToString("0");
            }
            ModData.v_AppFileDB.MapControl.ActiveView.Refresh();
        }
        //当前比例尺事件 陈亚飞添加
        private void CurScaleCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ModData.v_AppFileDB.MapControl.Map.MapScale = Convert.ToDouble(ModData.v_AppFileDB.CurScaleCmb.SelectedItem.ToString().Trim());
            }
            catch
            {
                ModData.v_AppFileDB.CurScaleCmb.Text = ModData.v_AppFileDB.MapControl.Map.MapScale.ToString("0");
            }
            ModData.v_AppFileDB.MapControl.ActiveView.Refresh();
        }

        //初始化数据供工程树图  chenyafei  add 20110215 
        private void _ControlFileDBTool_EnabledChanged(object sender, EventArgs e)
        {
            if (_ControlFileDBTool.Enabled)
            {
                //若Enable为true，则根据xml初始化数据库工程树图节点
                //清空工程树图和图层
                ModData.v_AppFileDB.ProjectTree.Nodes.Clear();
                ModData.v_AppFileDB.MapControl.Map.ClearLayers();
                ModData.v_AppFileDB.MapControl.ActiveView.Refresh();
                ModData.v_AppFileDB.TOCControl.Update();

                InitialDataConnTree();
            }
            else
            {
                //否则清空树图节点
                ModData.v_AppFileDB.ProjectTree.Nodes.Clear();
            }
        }


        //初始化数据连接界面

        private void InitialDataConnTree()
        {
            if (File.Exists(ModData.v_CoonectionInfoXML))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(ModData.v_CoonectionInfoXML);
                ModData.v_AppFileDB.DBXmlDocument = xml;
                AddTreeNodeByXML(xml,ModData.v_AppFileDB.ProjectTree);
            }
        }

        /// <summary>
        /// 读取xml将树图信息添加到工程树图上  陈亚飞编写
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="MainTree"></param>
        private void AddTreeNodeByXML(XmlDocument xmlDoc, DevComponents.AdvTree.AdvTree MainTree)
        {
            if (MainTree.Nodes != null)
            {
                MainTree.Nodes.Clear();
            }
            foreach (XmlNode oneNode in xmlDoc.FirstChild.ChildNodes)
            {
                XmlElement xmlElem = oneNode as XmlElement;
                DevComponents.AdvTree.Node ConnNode = new DevComponents.AdvTree.Node();
                ConnNode.Name = xmlElem.GetAttribute("NodeName");
                ConnNode.Text = xmlElem.GetAttribute("NodeText");
                ConnNode.DataKey = xmlElem.GetAttribute("NodeType").ToString();
                if (ConnNode.DataKey.ToString() == EnumTreeNodeType.DATABASE.ToString())
                {
                    //数据库节点

                    ConnNode.ImageIndex = 1;

                    //若为数据库且子节点信息不为空，则将子节点（连接信息）挂在树上
                    XmlNode subXmlNode = oneNode.FirstChild;
                    if (subXmlNode != null)
                    {
                        ConnNode.Tag = subXmlNode;//连接信息
                    }
                }
                else if (ConnNode.DataKey.ToString() == EnumTreeNodeType.DATACONNECT.ToString())
                {
                    //数据连接节点
                    ConnNode.ImageIndex = 0;
                }
                MainTree.Nodes.Add(ConnNode);
            }
        }
       
    }
}
