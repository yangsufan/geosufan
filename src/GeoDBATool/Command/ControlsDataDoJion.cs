using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESRI.ArcGIS.Geodatabase;
using System.Xml;

namespace GeoDBATool
{
    class ControlsDataDoJion : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef m_Hook;
        private DataTable JoinResultTable;
        private DataTable GetPolylineTable;///记录要参与接边的线
        private DataTable GetPolygonTable;////记录要参与接边的多边形
        public ControlsDataDoJion()
        {
            base._Name = "GeoDBATool.ControlsDataJoinSetting";
            base._Caption = "执行接边";
            base._Tooltip = "";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "执行接边";
            
        }

        public override bool Enabled
        {
            get
            {
                if (ModData.v_AppGIS.PolygonSearchGrid.DataSource == null)
                {
                    if (ModData.v_AppGIS.PolylineSearchGrid.DataSource == null)
                        return false;
                }
                if (ModData.v_AppGIS.PolylineSearchGrid.DataSource == null)
                {
                    if (ModData.v_AppGIS.PolygonSearchGrid.DataSource == null)
                        return false;
                }
                //if (((DataTable)ModData.v_AppGIS.PolylineSearchGrid.DataSource).TableName != "PolylineSearchTable")
                //     return false;
                // if (((DataTable)ModData.v_AppGIS.PolygonSearchGrid.DataSource).TableName != "PolygonSearchTable")
                //     return false;

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
            if (ModData.v_AppGIS.PolygonSearchGrid.DataSource == null && ModData.v_AppGIS.PolylineSearchGrid.DataSource == null)
                return;
            this.JoinResultTable.Rows.Clear();
            m_Hook.JoinMergeResultGrid.DataSource = null;
            List<IFeatureClass> FeaClsList = m_Hook.PolylineSearchGrid.Tag as List<IFeatureClass>;
            if (null == FeaClsList)
                return;
            IJoinOperation JoinOper = new ClsJoinOperationer();
            JoinOper.JoinFeaClss = FeaClsList;
            FrmProcessBar ProcessBar = new FrmProcessBar();
            ProcessBar.Text = "开始执行接边";//xisheng 20110901
            ProcessBar.Show();
            /////////
            XmlDocument Doc = new XmlDocument();
            Doc.Load(ModData.v_JoinSettingXML);
            bool IsCreatLog = false;
            Exception ex = null;
            if (Doc != null)
            {
                XmlElement ele = Doc.SelectSingleNode(".//日志设置") as XmlElement;
                string LogPath = ele.GetAttribute("日志路径").Trim() ;
                if (string.IsNullOrEmpty(LogPath))
                    IsCreatLog = false;
                else
                    IsCreatLog = true;
            }
            if (IsCreatLog)
            {
                IJoinLOG JoinLog = new ClsJoinLog();
                JoinLog.onDataJoin_Start(1, out ex);
                JoinOper.CreatLog = true;
            }
            else
            {
                JoinOper.CreatLog = false;
            }
            /////////
            if (null != m_Hook.PolylineSearchGrid.DataSource)/////线的接边
            {
                GetPolylineTable.Rows.Clear();
                DataTable TemTable = m_Hook.PolylineSearchGrid.DataSource as DataTable;
                for (int i = 0; i < m_Hook.PolylineSearchGrid.Rows.Count; i++)////遍历线型接边搜索表将选中的行记录下来
                {
                    if (m_Hook.PolylineSearchGrid.Rows[i].Cells[0].Value == null)
                        continue;
                    if (((bool)m_Hook.PolylineSearchGrid.Rows[i].Cells[0].Value) == true)
                    {
                        GetPolylineTable.Rows.Add(TemTable.Rows[i].ItemArray);
                    }
                }
                    if (null != GetPolylineTable)
                    {
                        DataTable mergeTable = JoinOper.MovePolylinePnt(GetPolylineTable);
                        if (null != mergeTable)
                        {
                            ProcessBar.SetFrmProcessBarMax(mergeTable.Rows.Count);
                            for (int i = 0; i < mergeTable.Rows.Count; i++)
                            {
                                ProcessBar.SetFrmProcessBarText("正在进行线的接边：");
                                ProcessBar.SetFrmProcessBarValue(i);
                                System.Windows.Forms.Application.DoEvents();
                                this.JoinResultTable.Rows.Add(mergeTable.Rows[i].ItemArray);
                            }
                        }
                    }
            }
            if (null != m_Hook.PolygonSearchGrid.DataSource)/////多边形的接边
            {
                GetPolygonTable.Rows.Clear();
                DataTable TemTable = m_Hook.PolygonSearchGrid.DataSource as DataTable;
                for (int i = 0; i < m_Hook.PolygonSearchGrid.Rows.Count; i++)////遍历线型接边搜索表将选中的行记录下来
                {
                    if (m_Hook.PolygonSearchGrid.Rows[i].Cells[0].Value == null)
                        continue;
                    if (((bool)m_Hook.PolygonSearchGrid.Rows[i].Cells[0].Value) == true)
                    {
                        GetPolygonTable.Rows.Add(TemTable.Rows[i].ItemArray);
                    }
                }
                if (null != GetPolygonTable)
                {
                    DataTable mergeTable = JoinOper.MovePolygonPnt(GetPolygonTable);
                    if (null != mergeTable)
                    {
                        ProcessBar.SetFrmProcessBarMax(mergeTable.Rows.Count);
                        for (int i = 0; i < mergeTable.Rows.Count; i++)
                        {
                            ProcessBar.SetFrmProcessBarText("正在进行多边形的接边：");
                            ProcessBar.SetFrmProcessBarValue(i);
                            System.Windows.Forms.Application.DoEvents();
                            this.JoinResultTable.Rows.Add(mergeTable.Rows[i].ItemArray);
                        }
                    }
                }
            }
            //m_Hook.PolylineSearchGrid.DataSource = null;
            //m_Hook.PolygonSearchGrid.DataSource = null;
            if (IsCreatLog)
            {
                IJoinLOG JoinLog = new ClsJoinLog();
                JoinLog.onDataJoin_Terminate(1, out ex);
            }
            m_Hook.JoinMergeResultGrid.DataSource= this.JoinResultTable;
            ControlsDataJoinSearch.SelectALL(m_Hook.JoinMergeResultGrid);
            ProcessBar.Close();
            
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
            if (m_Hook == null) return;
            ////////////初始化接边结果记录表
            JoinResultTable = new DataTable();
            JoinResultTable.TableName = "JoinResultTable";
            DataColumn dc1 = new DataColumn("数据集", Type.GetType("System.String"));
            DataColumn dc2 = new DataColumn("要素类型", Type.GetType("System.String"));
            DataColumn dc3 = new DataColumn("源要素ID", Type.GetType("System.Int64"));
             dc3.DefaultValue = -1;
            DataColumn dc4 = new DataColumn("目标要素ID", Type.GetType("System.Int64"));
             dc4.DefaultValue = -1;
            DataColumn dc5 = new DataColumn("处理结果", Type.GetType("System.String"));

            this.JoinResultTable.Columns.Add(dc1);
            this.JoinResultTable.Columns.Add(dc2);
            this.JoinResultTable.Columns.Add(dc3);
            this.JoinResultTable.Columns.Add(dc4);
            this.JoinResultTable.Columns.Add(dc5);
            /////////////////初始化线型接边记录表
            GetPolylineTable = new DataTable();
            GetPolylineTable.TableName = "PolylineSearchTable";
            dc1 = new DataColumn("数据集", Type.GetType("System.String"));
            dc2 = new DataColumn("要素类型", Type.GetType("System.String"));
            dc3 = new DataColumn("源要素ID", Type.GetType("System.Int64"));
            dc3.DefaultValue = -1;
            dc4 = new DataColumn("OriPtn", Type.GetType("System.String"));
            dc5 = new DataColumn("目标要素ID", Type.GetType("System.Int64"));
            dc5.DefaultValue = -1;
            DataColumn dc6 = new DataColumn("DesPtn", Type.GetType("System.String"));
            DataColumn dc7 = new DataColumn("接边状态", Type.GetType("System.String"));
            GetPolylineTable.Columns.Add(dc1);
            GetPolylineTable.Columns.Add(dc2);
            GetPolylineTable.Columns.Add(dc3);
            GetPolylineTable.Columns.Add(dc4);
            GetPolylineTable.Columns.Add(dc5);
            GetPolylineTable.Columns.Add(dc6);
            GetPolylineTable.Columns.Add(dc7);
            //////////////////////////////////////
            ////初始化多边形接边记录表
            GetPolygonTable = new DataTable();
            GetPolygonTable.TableName = "PolygonSearchTable";
            dc1 = new DataColumn("数据集", Type.GetType("System.String"));
            dc2 = new DataColumn("要素类型", Type.GetType("System.String"));
            dc3 = new DataColumn("源要素ID", Type.GetType("System.Int64"));
            dc3.DefaultValue = -1;
            dc4 = new DataColumn("OriLineIndex", Type.GetType("System.Int64"));
            dc4.DefaultValue = -1;
            dc5 = new DataColumn("目标要素ID", Type.GetType("System.Int64"));
            dc5.DefaultValue = -1;
            dc6 = new DataColumn("DesLineIndex", Type.GetType("System.Int64"));
            dc6.DefaultValue = -1;
            dc7 = new DataColumn("接边状态", Type.GetType("System.String"));
            GetPolygonTable.Columns.Add(dc1);
            GetPolygonTable.Columns.Add(dc2);
            GetPolygonTable.Columns.Add(dc3);
            GetPolygonTable.Columns.Add(dc4);
            GetPolygonTable.Columns.Add(dc5);
            GetPolygonTable.Columns.Add(dc6);
            GetPolygonTable.Columns.Add(dc7);
            ////////////////////////////////////////////////////
        }
    }
}
