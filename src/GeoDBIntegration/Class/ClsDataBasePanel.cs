using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SysCommon;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace GeoDBIntegration
{
    public class clsDatabasePanel
    {
        //********************************************************************************************************************//
        // guozheng 2011-5-16 added 数据库面板类
        // 作者:guozheng 
        // 修改：
        // 功能：该类实现系统集成管理界面的数据库图标的管理功能，包括数据库图标的刷新、显示
        //       重排列等，关于数据库图标的操作可以在该类中实现  
        //       (1)手动刷新容器面板，调用Refurbish()；
        //       (2)向容器中增加一个数据库项目，调用AddDataBasePro（）；
        //       (3)删除容器中指定的数据库项目，调用RemoveDataBasePro（） +1 重载；
        //       (4)使容器中指定的数据库项目设置输入焦点，调用SelectButton；
        // 版本：V1.0.0
        //********************************************************************************************************************//
        private int iNewButtonWith = 64;/////////////////////////////////////////////////////////////////////新按钮宽
        private int iNewButtonHeigh = 64;////////////////////////////////////////////////////////////////////新按钮高
        private int iButtonDiatance = 32;////////////////////////////////////////////////////////////////////按钮间距
        private int iDataBaseTypeLableCount = -1;////////////////////////////////////////////////////////////数据类型标注个数
        private int iDataBaseTypeLableHeight = 20;            //数据类型标注的高度
        private long m_lBID = -1;
        //cyf 20110627 modify
        //private Dictionary<enumInterDBType, List<ClsDataBaseProject>> m_DBEnumProjects = null;///////////////////数据库记录列表
        //private Dictionary<enumInterDBType, List<DevComponents.DotNetBar.ButtonX>> m_DBEnumButtons = null;///////控件列表(button)
        private Dictionary<string, List<ClsDataBaseProject>> m_DBProjects = null;///////////////////数据库记录列表
        private Dictionary<string, List<DevComponents.DotNetBar.ButtonX>> m_DBButtons = null;///////控件列表(button)
        //end
        //cyf 20110627 add:定义button下面的label的控件列表
        private Dictionary<string,DevComponents.DotNetBar.LabelX> m_DBLables = null; //控件（label）列表
        //end
        private DevComponents.DotNetBar.Controls.GroupPanel m_DataBaseGroupPanel = null;/////////////////////控件容器
        public DevComponents.DotNetBar.Controls.GroupPanel DataBaseGroupPanel
        {
            get { return this.m_DataBaseGroupPanel; }
            set { this.m_DataBaseGroupPanel = value; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="in_Panel">GroupPanel对象</param>
        public clsDatabasePanel(DevComponents.DotNetBar.Controls.GroupPanel in_Panel)
        {           
            this.m_DataBaseGroupPanel = in_Panel;
            //cyf 20110627 modify
            this.m_DBProjects = new Dictionary<string, List<ClsDataBaseProject>>();// new Dictionary<enumInterDBType, List<ClsDataBaseProject>>();
            m_DBButtons = new Dictionary<string, List<DevComponents.DotNetBar.ButtonX>>();// Dictionary<enumInterDBType, List<DevComponents.DotNetBar.ButtonX>>();
            m_DBLables = new Dictionary<string, DevComponents.DotNetBar.LabelX>();
            //end
            iDataBaseTypeLableCount = 0;
        }

        /// <summary>
        /// 刷新数据库容器界面，进行图标重排列
        /// </summary>
        public void Refurbish()
        {
            if (this.m_DataBaseGroupPanel == null) return;
            if (this.m_DBProjects == null) return;
            /////////////////////清空控件////////////////////////
            m_DBButtons.Clear();
            this.m_DataBaseGroupPanel.Controls.Clear();
            iDataBaseTypeLableCount = 0;
            int dPanelWith = this.m_DataBaseGroupPanel.Width;
            int dPanelHeight = this.m_DataBaseGroupPanel.Height;
            int iCurrentRow = 0;//////////////////////////////////当前行
            int iCurrentCol = 0;//////////////////////////////////当前列
            //bool bFirst = true;
            //cyf 20110627 modifys
            foreach (KeyValuePair<string, List<ClsDataBaseProject>> pTypeItem in m_DBProjects)
            {
                AddDataBaseTypeLable("*" + pTypeItem.Key+ ":---------------------------------------", iCurrentRow);
                AddButtons(pTypeItem.Value, ref  iCurrentRow, ref  iCurrentCol);
                ChangeRow(ref iCurrentRow, ref iCurrentCol);
            }
            //end
            #region 原有代码
            /////////////////////////为不同的数据库类型创建控件，考虑顺序////////////////
            //if (m_DBProjects.ContainsKey(enumInterDBType.成果文件数据库))
            //{
            //    if (!bFirst) { ChangeRow(ref iCurrentRow, ref iCurrentCol); }
            //    else bFirst = false;
            //    AddDataBaseTypeLable("*" + enumInterDBType.成果文件数据库.ToString() + ":---------------------------------------", iCurrentRow);
            //    List<ClsDataBaseProject> pDataBaseProjects = null;
            //    m_DBProjects.TryGetValue(enumInterDBType.成果文件数据库, out pDataBaseProjects);
            //    AddButtons(pDataBaseProjects, ref  iCurrentRow, ref  iCurrentCol);
            //}
            //if (m_DBProjects.ContainsKey(enumInterDBType.框架要素数据库))
            //{
            //    if (!bFirst) { ChangeRow(ref iCurrentRow, ref iCurrentCol); }
            //    else bFirst = false;
            //    AddDataBaseTypeLable("*" + enumInterDBType.框架要素数据库.ToString() + ":---------------------------------------", iCurrentRow);
            //    List<ClsDataBaseProject> pDataBaseProjects = null;
            //    m_DBProjects.TryGetValue(enumInterDBType.框架要素数据库, out pDataBaseProjects);
            //    AddButtons(pDataBaseProjects, ref  iCurrentRow, ref  iCurrentCol);
            //}
            //if (m_DBProjects.ContainsKey(enumInterDBType.影像数据库))
            //{
            //    if (!bFirst) { ChangeRow(ref iCurrentRow, ref iCurrentCol); }
            //    else bFirst = false;
            //    AddDataBaseTypeLable("*" + enumInterDBType.影像数据库.ToString() + ":-------------------------------------------", iCurrentRow);
            //    List<ClsDataBaseProject> pDataBaseProjects = null;
            //    m_DBProjects.TryGetValue(enumInterDBType.影像数据库, out pDataBaseProjects);
            //    AddButtons(pDataBaseProjects, ref  iCurrentRow, ref  iCurrentCol);
            //}
            //if (m_DBProjects.ContainsKey(enumInterDBType.高程数据库))
            //{
            //    if (!bFirst) { ChangeRow(ref iCurrentRow, ref iCurrentCol); }
            //    else bFirst = false;
            //    AddDataBaseTypeLable("*" + enumInterDBType.高程数据库.ToString() + ":-------------------------------------------", iCurrentRow);
            //    List<ClsDataBaseProject> pDataBaseProjects = null;
            //    m_DBProjects.TryGetValue(enumInterDBType.高程数据库, out pDataBaseProjects);
            //    AddButtons(pDataBaseProjects, ref  iCurrentRow, ref  iCurrentCol);
            //}
            #endregion
            //********************************//
            // 其他库体待实现
            //********************************//
        }

        /// <summary>
        /// 刷新数据库容器界面，进行图标重排列
        /// </summary>
        public void Refurbish(DevComponents.AdvTree.Node treenode)
        {
            if (this.m_DataBaseGroupPanel == null) return;
            if (this.m_DBProjects == null) return;
            /////////////////////清空控件////////////////////////
            m_DBButtons.Clear();
            this.m_DataBaseGroupPanel.Controls.Clear();
            iDataBaseTypeLableCount = 0;
            int dPanelWith = this.m_DataBaseGroupPanel.Width;
            int dPanelHeight = this.m_DataBaseGroupPanel.Height;
            int iCurrentRow = 0;//////////////////////////////////当前行
            int iCurrentCol = 0;//////////////////////////////////当前列
            string sDataBaseType="";
            m_lBID = -1;
            if (treenode.Level == 2)//判断是叶子节点
            {
                XmlElement DbInfoEle = treenode.Tag as XmlElement;
                m_lBID =(long) treenode.DataKey;
                sDataBaseType = DbInfoEle.GetAttribute("数据库类型"); if (string.IsNullOrEmpty(sDataBaseType)) return;
            }
            else if (treenode.Level == 1)
            {
                sDataBaseType = treenode.Text;
            }
            //bool bFirst = true;
            //cyf 20110627 modifys
            foreach (KeyValuePair<string, List<ClsDataBaseProject>> pTypeItem in m_DBProjects)
            {
                if (sDataBaseType == "" || sDataBaseType == pTypeItem.Key)
                {
                    AddDataBaseTypeLable("*" + pTypeItem.Key + ":---------------------------------------", iCurrentRow);
                    AddButtons(pTypeItem.Value, ref  iCurrentRow, ref  iCurrentCol);
                    ChangeRow(ref iCurrentRow, ref iCurrentCol);
                }
            }
            //end
         
            //********************************//
            // 其他库体待实现
            //********************************//
        }
        /// <summary>
        /// 为每一个数据库项目添加控件实例
        /// </summary>
        /// <param name="pDataBaseProjects"></param>
        /// <param name="iCurrentRow"></param>
        /// <param name="iCurrentCol"></param>
        private void AddButtons(List<ClsDataBaseProject> pDataBaseProjects,ref int iCurrentRow, ref int iCurrentCol)
        {
            if (pDataBaseProjects == null) return;
            if (pDataBaseProjects.Count <= 0) return;
                ////////////////////为不同的数据库工程创建控件//////////////////////
            for (int i = 0; i < pDataBaseProjects.Count; i++)
            {
          
                ClsDataBaseProject pDataBasePro = pDataBaseProjects[i];
                if (null == pDataBasePro) continue;
 				 ///ZQ  20111103  处理一行添加个数的长度超出控件大小产生滚动条的问题
                if (m_lBID>0 && pDataBasePro.lDBID != m_lBID) continue;
                int Length = (iCurrentCol+1) * this.iNewButtonWith + iButtonDiatance * iCurrentCol + this.iButtonDiatance;
                if (Length > this.m_DataBaseGroupPanel.Width)
                {
                    ChangeRow(ref iCurrentRow, ref iCurrentCol);
                    Length = (iCurrentCol + 1) * this.iNewButtonWith + iButtonDiatance * iCurrentCol + this.iButtonDiatance;
                    ///ZQ  20111103   解决换行存在位置重置问题
                    iCurrentCol += 1;
                    ///
                }
                else iCurrentCol += 1;
                //////////////////创建控件实例//////////////////                   
                DevComponents.DotNetBar.ButtonX NewbtnDB = new DevComponents.DotNetBar.ButtonX();
                //////////////////计算出现位置///////////////////
                int x = Length - this.iNewButtonWith;
                ///end  ZQ 
                int y = 0;
                if (iCurrentRow==0)
                    y = iCurrentRow * this.iNewButtonHeigh + iCurrentRow * iButtonDiatance + iDataBaseTypeLableHeight + iDataBaseTypeLableCount * iDataBaseTypeLableHeight;
                else
                    y = iCurrentRow * this.iNewButtonHeigh + iCurrentRow * iButtonDiatance + (iDataBaseTypeLableCount) * iDataBaseTypeLableHeight + iDataBaseTypeLableHeight;
                NewbtnDB.Location = new System.Drawing.Point(x, y);
                NewbtnDB.Size = new System.Drawing.Size(this.iNewButtonWith, this.iNewButtonHeigh);
               // NewbtnDB.Text = pDataBasePro.sDbName;
                NewbtnDB.Tooltip = pDataBasePro.sDbName;
                NewbtnDB.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
                NewbtnDB.Tag = pDataBasePro;
                NewbtnDB.Image = GeoDBIntegration.Properties.Resources.database100x100;/////////////////在此为不同数据库类型设置不同图标
                NewbtnDB.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(17, 2, 2, 2);
                NewbtnDB.BackColor = System.Drawing.Color.Transparent;
                NewbtnDB.Click += new System.EventHandler(btn_click);
                //事件绑定
                //NewbtnDB.DoubleClick += new System.EventHandler(btn_Doubleclick);
                this.m_DataBaseGroupPanel.Controls.Add(NewbtnDB);
                 /////////////////////把button记录下来///////////////
                //cyf 20110627 modify:
                if (m_DBButtons == null) m_DBButtons = new Dictionary<string, List<DevComponents.DotNetBar.ButtonX>>();// new Dictionary<enumInterDBType, List<DevComponents.DotNetBar.ButtonX>>();
                if (m_DBButtons.ContainsKey(pDataBasePro.DBTypeStr))
                {
                    List<DevComponents.DotNetBar.ButtonX> Getbuttons = null;
                    m_DBButtons.TryGetValue(pDataBasePro.DBTypeStr, out Getbuttons);
                    if (Getbuttons == null) Getbuttons = new List<DevComponents.DotNetBar.ButtonX>();
                    if (!Getbuttons.Contains(NewbtnDB)) Getbuttons.Add(NewbtnDB);
                }
                else
                {
                    List<DevComponents.DotNetBar.ButtonX> Getbuttons = new List<DevComponents.DotNetBar.ButtonX>();
                    Getbuttons.Add(NewbtnDB);
                    m_DBButtons.Add(pDataBasePro.DBTypeStr, Getbuttons);
                }
                //end
                AddLabledownButton(pDataBasePro.lDBID.ToString(), pDataBasePro.sDbName, NewbtnDB);
            }
        }

        /// <summary>
        /// 增加一个数据库工程，并刷新容器面板
        /// </summary>
        /// <param name="in_DataBasePro"></param>
        public void AddDataBasePro(ClsDataBaseProject in_DataBasePro)
        {
            if (in_DataBasePro == null) return;
            //cyf 20110627
            if (this.m_DBProjects == null) this.m_DBProjects = new Dictionary<string, List<ClsDataBaseProject>>();// new Dictionary<enumInterDBType, List<ClsDataBaseProject>>();
            if (this.m_DBProjects.ContainsKey(in_DataBasePro.DBTypeStr))
            {
                if (JudgeADataBaseProIsExist(in_DataBasePro)) return;
                List<ClsDataBaseProject> GetDataBasePros = null;
                m_DBProjects.TryGetValue(in_DataBasePro.DBTypeStr, out GetDataBasePros);
                if (GetDataBasePros == null) GetDataBasePros = new List<ClsDataBaseProject>();
                //if (GetDataBasePros.Contains(in_DataBasePro)) return;//没有起作用              
                GetDataBasePros.Add(in_DataBasePro);
                m_DBProjects.Remove(in_DataBasePro.DBTypeStr);
                m_DBProjects.Add(in_DataBasePro.DBTypeStr, GetDataBasePros);
            }
            else
            {
                List<ClsDataBaseProject> GetDataBasePros = null;
                GetDataBasePros = new List<ClsDataBaseProject>();
                GetDataBasePros.Add(in_DataBasePro);
                m_DBProjects.Add(in_DataBasePro.DBTypeStr, GetDataBasePros);
            }
            Refurbish();
            //end
        }

        /// <summary>
        /// 判断一个数据库工程是否已存在(通过ClsDataBaseProject对象)
        /// </summary>
        /// <param name="in_DataBasePro"></param>
        /// <returns>存在返回True，不存在返回False，内部异常返回False</returns>
        private bool JudgeADataBaseProIsExist(ClsDataBaseProject in_DataBasePro)
        {
            if (in_DataBasePro == null) return false;
            if (this.m_DBProjects == null) return false;
            //cyf 20110627 modify
            if (!m_DBProjects.ContainsKey(in_DataBasePro.DBTypeStr)) return false;
            List<ClsDataBaseProject> GetDataBasePros = null;
            m_DBProjects.TryGetValue(in_DataBasePro.DBTypeStr, out GetDataBasePros);
            if (null == GetDataBasePros) return false;
            foreach (ClsDataBaseProject GetExistDataBase in GetDataBasePros)
            {
                if (GetExistDataBase.lDBID == in_DataBasePro.lDBID) return true;
            }
            return false;
        }

        /// <summary>
        /// 卸载一个图标（通过数据库类型枚举值和ID）  cyf 20110627 modify
        /// </summary>
        /// <param name="enumDBType">数据库类型枚举值</param>
        /// <param name="lDBID">数据库ID</param>
        //public void RemoveDataBasePro(string DBTypeStr, long lDBID)
        //{
        //    if (this.m_DBProjects.ContainsKey(DBTypeStr))
        //    {
        //        List<ClsDataBaseProject> pDataBasePros = null;
        //        this.m_DBProjects.TryGetValue(DBTypeStr, out pDataBasePros);
        //        if (pDataBasePros == null) return;
        //        for (int i=0;i<pDataBasePros.Count;i++)
        //        {
        //            ClsDataBaseProject pGetDataBasePro=pDataBasePros[i];
        //            if (pGetDataBasePro.lDBID == lDBID)
        //            {
        //                pDataBasePros.Remove(pGetDataBasePro);
        //                break;
        //            }
        //        }
        //        Refurbish();
        //    }
        //}

        /// <summary>
        /// 卸载一个图标（通过数据库类型文本和ID）
        /// </summary>
        /// <param name="enumDBTypeStr">数据库类型的文本</param>
        /// <param name="lDBID">数据库ID</param>
        public void RemoveDataBasePro(string enumDBTypeStr, long lDBID)
        {
            //cyf 20110627 modify
            Exception ex=null;
            //enumInterDBType pDBType = GetDBType(enumDBTypeStr, out ex);
            if (ex != null) return;
            if (this.m_DBProjects.ContainsKey(enumDBTypeStr))
            {
                List<ClsDataBaseProject> pDataBasePros = null;
                this.m_DBProjects.TryGetValue(enumDBTypeStr, out pDataBasePros);
                if (pDataBasePros == null) return;
                for (int i = 0; i < pDataBasePros.Count; i++)
                {
                    ClsDataBaseProject pGetDataBasePro = pDataBasePros[i];
                    if (pGetDataBasePro.lDBID == lDBID)
                    {
                        pDataBasePros.Remove(pGetDataBasePro);
                        //added by chulili 20110725删除的彻底一些
                        m_DBLables.Remove(lDBID.ToString());
                        //end added by chulili
                        break;
                    }
                }
                Refurbish();
            }
        }
        /// <summary>
        /// 功能描述：清除所有的数据库类型
        /// 开 发 者：陈亚飞
        /// 开发时间：2011-07-13
        /// </summary>
        public void RemoveAllDataBasePro()
        {
            if (this.m_DBProjects != null)
            {
                //清除所有的数据库类型
                m_DBProjects.Clear();
                Refurbish();
            }
        }

        /// <summary>
        /// 将一个数据库相应的Button变为焦点状态
        /// </summary>
        /// <param name="enumDBTypeStr">数据库类型的文本</param>
        /// <param name="lDBID">数据库ID</param>
        public void SelectButton(string enumDBTypeStr, long lDBID)
        {
            Exception ex=null;
            try
            {
                //cyf 20110627 modify
                //enumInterDBType DataBaseTpe = GetDBType(enumDBTypeStr, out ex); if (ex != null) return;
                if (m_DBButtons.ContainsKey(enumDBTypeStr))
                {
                    List<DevComponents.DotNetBar.ButtonX> Getbuttons = new List<DevComponents.DotNetBar.ButtonX>();
                    m_DBButtons.TryGetValue(enumDBTypeStr, out Getbuttons);
                    if (Getbuttons == null) return;
                    foreach (DevComponents.DotNetBar.ButtonX Getbutton in Getbuttons)
                    {
                        if (Getbutton.Tag == null) continue;
                        ClsDataBaseProject ClsDataBase = Getbutton.Tag as ClsDataBaseProject; if (ClsDataBase == null) continue;
                        if (ClsDataBase.lDBID == lDBID)
                        {
                            Getbutton.Focus();
                            break;
                        }
                    } 
                }
                //end
            }
            catch
            {
            }
        }


        /// <summary>
        /// 将一个数据库相应的Button变为焦点状态
        /// </summary>
        /// <param name="enumDBTypeStr">数据库类型的文本</param>
        /// <param name="lDBID">数据库ID</param>
        public void SelectButton(DevComponents.AdvTree.Node treenode)
        {
            Exception ex = null;
            try
            {
                //cyf 20110627 modify
                //enumInterDBType DataBaseTpe = GetDBType(enumDBTypeStr, out ex); if (ex != null) return;
                if (treenode.Level == 2)
                {
                    XmlElement DbInfoEle = treenode.Tag as XmlElement;
                    string enumDBTypeStr = DbInfoEle.GetAttribute("数据库类型"); if (string.IsNullOrEmpty(enumDBTypeStr)) return;
                    if (m_DBButtons.ContainsKey(enumDBTypeStr))
                    {
                        List<DevComponents.DotNetBar.ButtonX> Getbuttons = new List<DevComponents.DotNetBar.ButtonX>();
                        m_DBButtons.TryGetValue(enumDBTypeStr, out Getbuttons);
                        if (Getbuttons == null) return;
                        foreach (DevComponents.DotNetBar.ButtonX Getbutton in Getbuttons)
                        {
                            if (Getbutton.Tag == null) continue;
                            ClsDataBaseProject ClsDataBase = Getbutton.Tag as ClsDataBaseProject; if (ClsDataBase == null) continue;
                            if (ClsDataBase.lDBID == (long)treenode.DataKey)
                            {
                                Getbutton.Focus();
                                break;
                            }
                        }
                    }
                }
                Refurbish(treenode);
                //end
            }
            catch
            {
            }
        }
        // *---------------------------------------------------------------------------------------
        // *开 发 者：陈亚飞
        // *功能函数：修改数据源时同步修改控件label
        // *开发日期：2011-06-27
        // *参    数：数据源ID,数据源名称
        public void EditLabel(string pDBTypeStr,string mProID,string mProName)
        {
            if (m_DBLables.ContainsKey(mProID))
            {
                DevComponents.DotNetBar.LabelX pLabel = null;  //选中数据源工程项对应的标签
                pLabel = m_DBLables[mProID];
                //added by chulili 20110714更新界面中的label
                for (int i = 0; i < this.m_DataBaseGroupPanel.Controls.Count; i++)
                {
                    DevComponents.DotNetBar.LabelX pTmpLabel = this.m_DataBaseGroupPanel.Controls[i] as DevComponents.DotNetBar.LabelX;
                    if (pTmpLabel == null)
                        continue;
                    if (pTmpLabel.Location.X==pLabel.Location.X && pTmpLabel.Location.Y==pLabel.Location.Y && pTmpLabel.Text==pLabel.Text )
                    {
                        pTmpLabel.Text = mProName;
                        break;
                    }
                }
                //end add
                pLabel.Text = mProName;
                System.Windows.Forms.Application.DoEvents();
                //cyf 20110630  add
                if (m_DBButtons.ContainsKey(pDBTypeStr))
                {
                    List<DevComponents.DotNetBar.ButtonX> Getbuttons = new List<DevComponents.DotNetBar.ButtonX>();
                    m_DBButtons.TryGetValue(pDBTypeStr, out Getbuttons);
                    if (Getbuttons == null) return;
                    foreach (DevComponents.DotNetBar.ButtonX Getbutton in Getbuttons)
                    {
                        if (Getbutton.Tag == null) continue;
                        ClsDataBaseProject ClsDataBase = Getbutton.Tag as ClsDataBaseProject; if (ClsDataBase == null) continue;
                        if (ClsDataBase.lDBID.ToString() == mProID)
                        {
                            
                            Getbutton.Tooltip = mProName;
                            break;
                        }
                    }
                }
                
                //end
            }
        }

        /// <summary>
        /// 为数据库项目添加标题，标题的字体、颜色、位置等可以在该函数中实现
        /// </summary>
        /// <param name="sLableText">标注文本</param>
        /// <param name="in_pRutton">Button对象</param>
        public void AddLabledownButton(string mProID,string sLableText, DevComponents.DotNetBar.ButtonX in_pRutton)
        {
            DevComponents.DotNetBar.LabelX ButtonLable = new DevComponents.DotNetBar.LabelX();
            int x = in_pRutton.Location.X;
            int y = in_pRutton.Location.Y + this.iNewButtonHeigh;
            ButtonLable.Location = new System.Drawing.Point(x, y);
            ButtonLable.Width = in_pRutton.Width;
            ButtonLable.Text = sLableText;
            in_pRutton.Tooltip = sLableText;//cyf 20110630 add
            ButtonLable.Font = new System.Drawing.Font("微软雅黑", 8);//cyf 20110630 :7
            ButtonLable.BackColor = System.Drawing.Color.Transparent;
            //ButtonLable.AutoSize = true;  //cyf 20110630 add
            ButtonLable.Size = new System.Drawing.Size(iNewButtonHeigh, 25);  //cyf 20110630 :21
            //cyf 20110627 将标签控件保存起来
            if (m_DBLables == null)
            {
                m_DBLables = new Dictionary<string, DevComponents.DotNetBar.LabelX>();
            }
            if (!m_DBLables.ContainsKey(mProID))
            {
                m_DBLables.Add(mProID, ButtonLable);
            }
            //end
            this.m_DataBaseGroupPanel.Controls.Add(ButtonLable);
        }

        /// <summary>
        /// 为一个数据库类型增加Lable标注，标题的字体、颜色、位置等可以在该函数中实现
        /// </summary>
        /// <param name="sLableText">标注文本</param>
        /// <param name="iCurrentRow">当前行</param>
        public void AddDataBaseTypeLable(string sLableText,  int iCurrentRow)
        {
            DevComponents.DotNetBar.LabelX CaptionLable = new DevComponents.DotNetBar.LabelX();
            int x=0;
            int y = 0;
            if (iCurrentRow == 0) y = 0;
            else y = iCurrentRow * iNewButtonHeigh + iCurrentRow * iButtonDiatance + (iDataBaseTypeLableCount) * iDataBaseTypeLableHeight+10;//cyf 20110627 modify:加10
            CaptionLable.Location = new System.Drawing.Point(x, y);
            CaptionLable.Text = sLableText;
            CaptionLable.BackColor = System.Drawing.Color.Transparent;
            CaptionLable.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            CaptionLable.ForeColor = System.Drawing.Color.Black;
            CaptionLable.Name = "DataTypeLable";
            CaptionLable.Size = new System.Drawing.Size(425, 25);
            this.m_DataBaseGroupPanel.Controls.Add(CaptionLable);
            iDataBaseTypeLableCount += 1;
        }

        /// <summary>
        /// 单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_click(object sender, System.EventArgs e)
        {

            //xisheng 20111121 屏蔽掉点击图标切换到入库系统的界面
           // DevComponents.DotNetBar.ButtonX CurrentButton = (DevComponents.DotNetBar.ButtonX)sender;//将触发此事件的对象转换为该Button对象
           //// System.Windows.Forms.MessageBox.Show("button :"+b1.Text+" click");
           // try
           // {
           //     ClsDataBaseProject pDataBaseProject = CurrentButton.Tag as ClsDataBaseProject;
           //     if (pDataBaseProject.DBType == enumInterDBType.框架要素数据库)
           //         EnterFeaDataBase(pDataBaseProject.lDBID,pDataBaseProject.DBFormatID.ToString());
           //     else if (pDataBaseProject.DBType == enumInterDBType.影像数据库)
           //         EnterImageDataBase(pDataBaseProject.lDBID);
           //     else if (pDataBaseProject.DBType == enumInterDBType.高程数据库)
           //         EnterDemDataBase(pDataBaseProject.lDBID);
           //     else if (pDataBaseProject.DBType == enumInterDBType.成果文件数据库)
           //         EnterFTPDataBase(pDataBaseProject.lDBID, pDataBaseProject.sDbName);
           // }
           // catch
           // {
           // }
           //*****************************************************end
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Doubleclick(object sender, System.EventArgs e)
        {
            //DevComponents.DotNetBar.ButtonX b1 = (DevComponents.DotNetBar.ButtonX)sender;//将触发此事件的对象转换为该Button对象
            //System.Windows.Forms.MessageBox.Show("button :" + b1.Text + " Doubleclick");
        }
        /// <summary>
        /// 换行
        /// </summary>
        /// <param name="i_CurrentRow">行号</param>
        /// <param name="i_CurrentCol">列号</param>
        private void ChangeRow(ref int i_CurrentRow, ref int i_CurrentCol)
        {
            i_CurrentRow += 1;
            i_CurrentCol = 0;
        }
        private enumInterDBType GetDBType(string in_sDBTypeStr, out Exception ex)
        {
            ex = null;
            if (in_sDBTypeStr == enumInterDBType.成果文件数据库.ToString())
                return enumInterDBType.成果文件数据库;
            else if (in_sDBTypeStr == enumInterDBType.地理编码数据库.ToString())
                return enumInterDBType.地理编码数据库;
            else if (in_sDBTypeStr == enumInterDBType.地名数据库.ToString())
                return enumInterDBType.地名数据库;
            else if (in_sDBTypeStr == enumInterDBType.高程数据库.ToString())
                return enumInterDBType.高程数据库;
            else if (in_sDBTypeStr == enumInterDBType.框架要素数据库.ToString())
                return enumInterDBType.框架要素数据库;
            else if (in_sDBTypeStr == enumInterDBType.影像数据库.ToString())
                return enumInterDBType.影像数据库;
            else if (in_sDBTypeStr == enumInterDBType.专题要素数据库.ToString())
                return enumInterDBType.专题要素数据库;
            else if (in_sDBTypeStr == enumInterDBType.电子地图数据库.ToString())
                return enumInterDBType.电子地图数据库;
            else
            {
                ex = new Exception("不支持的数据库类型");
                return enumInterDBType.框架要素数据库;
            }
        }
        private enumInterDBFormat GetDBFormate(string in_sDBFormateStr, out Exception ex)
        {
            ex = null;
            if (in_sDBFormateStr == enumInterDBFormat.ARCGISGDB.ToString())
                return enumInterDBFormat.ARCGISGDB;
            else if (in_sDBFormateStr == enumInterDBFormat.ARCGISPDB.ToString())
                return enumInterDBFormat.ARCGISPDB;
            else if (in_sDBFormateStr == enumInterDBFormat.ARCGISSDE.ToString())
                return enumInterDBFormat.ARCGISSDE;
            else if (in_sDBFormateStr == enumInterDBFormat.FTP.ToString())
                return enumInterDBFormat.FTP;
            else if (in_sDBFormateStr == enumInterDBFormat.GEOSTARACCESS.ToString())
                return enumInterDBFormat.GEOSTARACCESS;
            else if (in_sDBFormateStr == enumInterDBFormat.GEOSTARORACLE.ToString())
                return enumInterDBFormat.GEOSTARORACLE;
            else if (in_sDBFormateStr == enumInterDBFormat.GEOSTARORSQLSERVER.ToString())
                return enumInterDBFormat.GEOSTARORSQLSERVER;
            else if (in_sDBFormateStr == enumInterDBFormat.ORACLESPATIAL.ToString())
                return enumInterDBFormat.ORACLESPATIAL;
            else
            {
                ex = new Exception("不支持的数据库平台");
                return enumInterDBFormat.ARCGISSDE;
            }

        }

        //进入子系统界面的函数//
        #region 进入子系统界面的函数
        /// <summary>
        /// 进入框架要素库子系统
        /// </summary>
        /// <param name="in_lFeaDataBaseID"></param>
        /// <param name="DBInfoEle"></param>
        public void EnterFeaDataBase(long in_lFeaDataBaseID, string pDBFormatID)
        {


            //string pDBID = in_lFeaDataBaseID.ToString();  //当前要启动的工程ID
            ////将当前数据库ID写入xml中
            //SaveIDToXml(pDBID, ModuleData.v_feaProjectXML, ModuleData.v_feaProjectXMLTemp);

            //==============================================================================================================================================
            //chenyafei  modify 20100215 解决系统插件加载的问题
            string pSysName = "";   //子系统名称
            string pSysCaption = ""; //子系统标题

           // XmlElement pElem = advTreeProject.SelectedNode.Nodes[i].Tag as XmlElement;  //数据库平台节点
            //XmlElement pElem =DBInfoEle;
            //string ptStr = pElem.GetAttribute("数据库平台");   //数据库平台信息
            if (pDBFormatID == enumInterDBFormat.ARCGISGDB.GetHashCode().ToString() || pDBFormatID == enumInterDBFormat.ARCGISPDB.GetHashCode().ToString() || pDBFormatID == enumInterDBFormat.ARCGISSDE.GetHashCode().ToString())
            {
                //启动ArcGIs平台
                pSysName = "GeoDBATool.ControlDBATool";    //Name
            }
            else if (pDBFormatID == enumInterDBFormat.GEOSTARACCESS.GetHashCode().ToString() || pDBFormatID == enumInterDBFormat.GEOSTARORACLE.GetHashCode().ToString() || pDBFormatID == enumInterDBFormat.GEOSTARORSQLSERVER.GetHashCode().ToString())
            {
                //启动Geostar平台
                pSysName = "GeoStarTest.ControlTest";       //Name
            }
            else if (pDBFormatID == enumInterDBFormat.ORACLESPATIAL.GetHashCode().ToString())
            {
                //启动oraclespatial平台
                pSysName = "OracleSpatialDBTool.ControlOracleSpatialDBTool";    //Name
            }

            //根据Name获得子系统的caption
            XmlDocument sysXml = new XmlDocument();
            sysXml.Load(ModuleData.m_SysXmlPath);
            XmlNode sysNode = sysXml.SelectSingleNode("//Main//System[@Name='" + pSysName + "']");
            if (sysNode == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在Name为" + pSysName + "的系统");
                return;
            }
            pSysCaption = (sysNode as XmlElement).GetAttribute("Caption").Trim();  //caption

            //进入子系统界面
            ModDBOperate.InitialForm(pSysName, pSysCaption);

            //===================================================================================================================================================
            //*********************************************************************
            //guozheng added enter feature Db Log
            if (ModuleData.v_SysLog != null)
            {
                List<string> Pra = new List<string>();
                //Pra.Add(pElem.GetAttribute("数据库工程名"));
                //Pra.Add(pElem.GetAttribute("数据库平台"));
                //Pra.Add(pElem.GetAttribute("数据库类型"));
                //Pra.Add(pElem.GetAttribute("数据库连接信息"));
                ModuleData.v_SysLog.Write("进入框架要素库", Pra, DateTime.Now);
            }
            //*********************************************************************
         
        }

        /// <summary>
        /// 进入影像数据库  cyf 20110627 modify
        /// </summary>
        /// <param name="in_lFeaDataBaseID"></param>
        /// <param name="DBInfoEle"></param>
        public void EnterImageDataBase(long in_lFeaDataBaseID)
        {

            //string pDBID = in_lFeaDataBaseID.ToString();
            ////cyf 201106058 modify 
            // //SaveIDToXml(pDBID, ModuleData.v_ImageProjectXml, ModuleData.v_ImageProjectXmlTemp);
            // SaveIDToXml(pDBID, ModuleData.v_feaProjectXML, ModuleData.v_feaProjectXMLTemp);
             //end

            //影像数据库界面
            //==================================================================================
            //  chenayfei  modify 20110215  进入子系统界面修改
            //
            //
            string pSysName = "";   //子系统名称
            string pSysCaption = ""; //子系统标题
            //根据Name获得子系统的caption

            pSysName = "GeoDBATool.ControlDBATool";// "GeoDBImage.ControlDBImageTool";    //Name

            XmlDocument sysXml = new XmlDocument();
            sysXml.Load(ModuleData.m_SysXmlPath);
            XmlNode sysNode = sysXml.SelectSingleNode("//Main//System[@Name='" + pSysName + "']");
            if (sysNode == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在Name为" + pSysName + "的系统");
                return;
            }
            pSysCaption = (sysNode as XmlElement).GetAttribute("Caption").Trim();  //caption

            //进入子系统界面
            ModDBOperate.InitialForm(pSysName, pSysCaption);
            //==========================================================================================
        }

        /// <summary>
        /// 进入DEM高程数据库子系统
        /// </summary>
        /// <param name="in_lDemDataBaseID"></param>
        /// <param name="DBInfoEle"></param>
        public void EnterDemDataBase(long in_lDemDataBaseID)
        {
            //将当前数据库ID写入xml中
            //string pDBID = in_lDemDataBaseID.ToString();
            ////cyf 201106058 modify 
            ////SaveIDToXml(pDBID, ModuleData.v_DEMProjectXml, ModuleData.v_DEMProjectXmlTemp);
            //SaveIDToXml(pDBID, ModuleData.v_feaProjectXML, ModuleData.v_feaProjectXMLTemp);
            //end
            //高程数据库界面
            //==================================================================================
            //  chenayfei  modify 20110215  进入子系统界面修改
            string pSysName = "";   //子系统名称
            string pSysCaption = ""; //子系统标题
            //根据Name获得子系统的caption
            pSysName = "GeoDBATool.ControlDBATool";// "GeoDBContour.ControlDBContourTool";    //Name
            XmlDocument sysXml = new XmlDocument();
            sysXml.Load(ModuleData.m_SysXmlPath);
            XmlNode sysNode = sysXml.SelectSingleNode("//Main//System[@Name='" + pSysName + "']");
            if (sysNode == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在Name为" + pSysName + "的系统");
                return;
            }
            pSysCaption = (sysNode as XmlElement).GetAttribute("Caption").Trim();  //caption

            //进入子系统界面
            ModDBOperate.InitialForm(pSysName, pSysCaption);
            //==========================================================================================
        }

        /// <summary>
        /// 进入文件库子系统  cyf 20110627 modify
        /// </summary>
        /// <param name="in_lDemDataBaseID"></param>
        /// <param name="DBInfoEle"></param>
        public void EnterFTPDataBase(long in_lDemDataBaseID,string in_sDBName)
        {
            //将当前数据库ID写入xml中
            Exception ex = null;
            string pDBID = in_lDemDataBaseID.ToString();
            string sDBName = in_sDBName;
            if (pDBID == "System.Data.DataRowView" || sDBName == "System.Data.DataRowView") return;
            clsFTPOper FTPOper = new clsFTPOper();
            FTPOper.SaveProjectXML(pDBID, sDBName, out ex);
            if (ex != null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", ex.Message);
                return;
            }
            //进入文件库界面
            //==================================================================================
            //  chenayfei  modify 20110215  进入子系统界面修改
            string pSysName = "";   //子系统名称
            string pSysCaption = ""; //子系统标题
            //根据Name获得子系统的caption
            pSysName = "FileDBTool.ControlFileDBTool";    //Name
            XmlDocument sysXml = new XmlDocument();
            sysXml.Load(ModuleData.m_SysXmlPath);
            XmlNode sysNode = sysXml.SelectSingleNode("//Main//System[@Name='" + pSysName + "']");
            if (sysNode == null)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "不存在Name为" + pSysName + "的系统");
                return;
            }
            pSysCaption = (sysNode as XmlElement).GetAttribute("Caption").Trim();  //caption
            //进入子系统界面
            ModDBOperate.InitialForm(pSysName, pSysCaption);

            //========================================================================

        }

        /// <summary>
        /// 将数据库ID写入xml中  陈亚飞添加20100930
        /// </summary>
        /// <param name="pDBID">数据库ID</param>
        private void SaveIDToXml(string pDBID, string xmlCur, string xmlTemp)
        {
            try
            {
                Convert.ToInt32(pDBID);
                if (!File.Exists(xmlCur))
                {
                    File.Copy(xmlTemp, xmlCur);
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlCur);
                XmlElement pElem = xmlDoc.SelectSingleNode(".//工程管理") as XmlElement;
                pElem.SetAttribute("当前库体编号", pDBID);
                xmlDoc.Save(xmlCur);
            }
            catch (Exception eError)
            {
                //****************************************************
                if (ModuleData.v_SysLog != null)
                    ModuleData.v_SysLog.Write(eError, null, DateTime.Now);
                //****************************************************
                //SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "请先创建数据库连接工程");
                return;
            }

        }
        #endregion
    }

    
}