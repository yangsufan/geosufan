using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESRI.ArcGIS.Geodatabase;
using System.Xml;

namespace GeoDBATool
{
    class ControlsDataDoMerge : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        public ControlsDataDoMerge()
        {
            base._Name = "GeoDBATool.ControlsDataJoinSetting";
            base._Caption = "执行融合";
            base._Tooltip = "";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "执行融合";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.JoinMergeResultGrid.DataSource == null)
                    return false;
                if (((DataTable)m_Hook.JoinMergeResultGrid.DataSource).TableName != "JoinResultTable")
                    return false;
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
            if (m_Hook.JoinMergeResultGrid.DataSource == null)
                return ;
            if (((DataTable)m_Hook.JoinMergeResultGrid.DataSource).TableName != "JoinResultTable")
                return ;
            List<IFeatureClass>MergeFeaClsList=null;
            ////////获取融合参数///////
            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(ModData.v_JoinSettingXML);
            if (null == XmlDoc)
            {
                SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取融合参数配置文件失败！");
                return;
            }
            XmlElement ele = XmlDoc.SelectSingleNode(".//融合设置") as XmlElement;
            string sIsDesAtrToOri = ele.GetAttribute("属性覆盖").Trim();
            ////////
            
            MergeFeaClsList=m_Hook.PolylineSearchGrid.Tag as List<IFeatureClass>;
            if(null==MergeFeaClsList)
                return;
            IMergeOperation Meroper=new ClsMergeOperationer();
            Meroper.JoinFeaClss=MergeFeaClsList;
            //////////融合要素属性处理选择////////
            if (sIsDesAtrToOri=="Y")
                Meroper.SetDesValueToOri=true;
            else
                Meroper.SetDesValueToOri = false;
            /////////日志
            bool IsCreatLog = false;
            Exception ex = null;
            if (XmlDoc != null)
            {
                XmlElement ele2 = XmlDoc.SelectSingleNode(".//日志设置") as XmlElement;
                string LogPath = ele2.GetAttribute("日志路径").Trim();
                if (string.IsNullOrEmpty(LogPath))
                {
                    IsCreatLog = false;
                    Meroper.CreatLog = false;
                }
                else
                {
                    IsCreatLog = true;
                    Meroper.CreatLog = true;
                }
                //////////////////日志中属性处理记录信息
                string sAttriPro = string.Empty;
                if (sIsDesAtrToOri == "Y")
                    sAttriPro = "覆盖";
                else 
                    sAttriPro = "累加";                    
                /////////////若日志存在则记录到日志当中
                if (!string.IsNullOrEmpty(LogPath))
                {
                    try
                    {
                        XmlDocument Doc = new XmlDocument();
                        Doc.Load(LogPath);
                        if (null != Doc)
                        {
                            XmlElement Setele = Doc.SelectSingleNode(".//融合操作/参数") as XmlElement;
                            Setele.SetAttribute("属性处理", sAttriPro);
                            Doc.Save(LogPath);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            if (IsCreatLog)
            {
                IJoinLOG JoinLog = new ClsJoinLog();
                JoinLog.onDataJoin_Start(2, out ex);
               
            }
            /////////
            DataTable UnionResTable = new DataTable();
            DataColumn dc1 = new DataColumn("数据集", Type.GetType("System.String"));
            DataColumn dc2 = new DataColumn("要素类型", Type.GetType("System.String"));
            DataColumn dc3 = new DataColumn("源要素ID", Type.GetType("System.Int64"));
            dc3.DefaultValue = -1;
            DataColumn dc4 = new DataColumn("目标要素ID", Type.GetType("System.Int64"));
            dc4.DefaultValue = -1;
            DataColumn dc5 = new DataColumn("处理结果", Type.GetType("System.String"));

            UnionResTable.Columns.Add(dc1);
            UnionResTable.Columns.Add(dc2);
            UnionResTable.Columns.Add(dc3);
            UnionResTable.Columns.Add(dc4);
            UnionResTable.Columns.Add(dc5);
            DataTable JoinResultTable=m_Hook.JoinMergeResultGrid.DataSource as DataTable;
            /////////////////
           // DataTable UnionResTable = JoinResultTable;
            UnionResTable.TableName = "MergeResultTable";
          //  UnionResTable.Rows.Clear();
             if (JoinResultTable != null)//////融合操作（考虑多个要素相互融合的情况）
            {
                FrmProcessBar ProcessBar = new FrmProcessBar(JoinResultTable.Rows.Count);
                int max = JoinResultTable.Rows.Count;
                ProcessBar.Show();
               
                while (JoinResultTable.Rows.Count > 0)
                {
                   
                    List<int> lDeleRowNo = new List<int>();
                    long OriFeaOID = -1;
                    List<long> lDesFeaOID = new List<long>();
                    /////获取第一行
                    string DataSetName = string.Empty;
                    string type = string.Empty;
                    long OriOID = -1;
                    long DesOID = -1;
                    string result = string.Empty;
                    try
                    {
                        DataSetName = JoinResultTable.Rows[0]["数据集"].ToString().Trim();
                        type = JoinResultTable.Rows[0]["要素类型"].ToString().Trim();
                        OriOID = Convert.ToInt64(JoinResultTable.Rows[0]["源要素ID"].ToString());
                        DesOID = Convert.ToInt64(JoinResultTable.Rows[0]["目标要素ID"].ToString());
                        result = JoinResultTable.Rows[0]["处理结果"].ToString().Trim();
                    }
                    catch
                    {
                        return;
                    }
                    OriFeaOID = OriOID;
                    ProcessBar.SetFrmProcessBarText("处理要素：" + OriFeaOID);
                    ProcessBar.SetFrmProcessBarValue(max - JoinResultTable.Rows.Count);
                    System.Windows.Forms.Application.DoEvents();
                    if (result == "已接边")
                    {
                        lDesFeaOID.Add(DesOID);
                    }
                    lDeleRowNo.Add(0);
                    /////遍历剩下的行，若存在互接边关系记录到融合列表中，同时记录下这些行进行删除      
                    #region 遍历剩下的行，若存在互接边关系记录到融合列表中，同时记录下这些行进行删除
                    //////做两次检索避免遗漏（当一条线多次穿越接边边界时，一次循环检索可能会遗漏部分融合要素，两次循环使遗漏减至最少）
                    GetAllunionFea(OriOID,ref lDesFeaOID,ref lDeleRowNo, JoinResultTable, type, DataSetName);
                    GetAllunionFea(OriOID, ref lDesFeaOID, ref lDeleRowNo, JoinResultTable, type, DataSetName);
                    #endregion
                    //////融合记录列表中的要素
                    string sunionres = string.Empty;
                    #region  融合记录列表中的要素
                    if (lDesFeaOID != null && OriFeaOID!=-1)
                    {
                        for (int i = 0; i < lDesFeaOID.Count; i++)
                        {
                            long UnioOid = lDesFeaOID[i];
                            ProcessBar.SetFrmProcessBarText("融合要素：" + OriFeaOID + "，" + UnioOid);
                            System.Windows.Forms.Application.DoEvents();
                            if (type == "Polyline")
                            {
                                if (Meroper.MergePolyline(DataSetName, OriFeaOID, UnioOid))
                                    sunionres = "已融合";
                                else
                                    sunionres = "未融合";
                            }
                            else if (type == "Polygon")
                            {
                                if(Meroper.MergePolygon(DataSetName, OriFeaOID, UnioOid))
                                    sunionres = "已融合";
                                else
                                    sunionres = "未融合";
                            }
                        }
                    }
                    #endregion
                    DataRow addrow = UnionResTable.NewRow();
                    addrow["数据集"] = DataSetName;
                    addrow["要素类型"] = type;
                    addrow["源要素ID"] = OriFeaOID;
                    addrow["目标要素ID"] = 0;
                    addrow["处理结果"] = sunionres;
                    UnionResTable.Rows.Add(addrow);
                    
                    #region 删除行
                    if (null != lDeleRowNo)
                    {
                        for (int i = 0; i < lDeleRowNo.Count; i++)
                        {
                            JoinResultTable.Rows.Remove(JoinResultTable.Rows[lDeleRowNo[i]-i]);
                        }
                    }
                    #endregion

                }
                ProcessBar.Close();
             }
             if (IsCreatLog)
             {
                 IJoinLOG JoinLog = new ClsJoinLog();
                 JoinLog.onDataJoin_Terminate(2, out ex);

             }
            //// JoinResultTable.TableName = "MergeResultTable";
            //// ((DataTable)m_Hook.JoinMergeResultGrid.DataSource).TableName = "MergeResultTable";
             m_Hook.JoinMergeResultGrid.DataSource = UnionResTable;
             ControlsDataJoinSearch.SelectALL(m_Hook.JoinMergeResultGrid);//选择所有；xisheng 20110901
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
        }
        private void GetAllunionFea( long OID,ref List<long>DesOID,ref List<int>DeleRowNo , DataTable joinTable ,string type,string DatasetName)
        {

            for (int i = 1; i < joinTable.Rows.Count; i++)
            {
                if (DeleRowNo.Contains(i))
                    continue;
                if (m_Hook.JoinMergeResultGrid.Rows[i].Cells[0].Value == null)
                {
                    if (!DeleRowNo.Contains(i))
                        DeleRowNo.Add(i);
                    continue;
                }
                if (((bool)m_Hook.JoinMergeResultGrid.Rows[i].Cells[0].Value) == false)
                {
                    if (!DeleRowNo.Contains(i))
                        DeleRowNo.Add(i);
                    continue;
                }                   
                if (DatasetName == joinTable.Rows[i]["数据集"].ToString().Trim() && type == joinTable.Rows[i]["要素类型"].ToString().Trim())
                {
                    long getOriOid = -1;
                    long getDesOid = -1;
                    string getresult = string.Empty;
                    try
                    {
                        getOriOid = Convert.ToInt64(joinTable.Rows[i]["源要素ID"].ToString());
                        getDesOid = Convert.ToInt64(joinTable.Rows[i]["目标要素ID"].ToString());
                        getresult = joinTable.Rows[i]["处理结果"].ToString().Trim();
                    }
                    catch
                    {
                        continue;
                    }

                    if (getOriOid == OID || DesOID.Contains(getOriOid))
                    {
                        DeleRowNo.Add(i);
                        if (getresult == "已接边")
                        {
                            if (!DesOID.Contains(getDesOid))
                            {
                                DesOID.Add(getDesOid);                               
                            }                            
                        }
                        if (!DeleRowNo.Contains(i))
                             DeleRowNo.Add(i);
                    }
                    else if (getDesOid == OID || DesOID.Contains(getDesOid))
                    {
                        DeleRowNo.Add(i);
                        if (getresult == "已接边")
                        {
                            if (!DesOID.Contains(getOriOid))
                            {
                                DesOID.Add(getOriOid);
                                
                            }                           
                        }
                        if (!DeleRowNo.Contains(i))
                             DeleRowNo.Add(i);
                    }

                }
            }
        }
    }
}
