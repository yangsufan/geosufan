using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;

namespace FileDBTool
{
    /// <summary>
    /// 缩放到数据范围
    /// </summary>
    public class ControlsZoomToRange:Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppFileRef m_Hook;
        int m_DataTypeID=0;

        public ControlsZoomToRange()
        {
            base._Name = "FileDBTool.ControlsZoomToRange";
            base._Caption = "缩放到数据范围";
            base._Tooltip = "缩放到数据范围";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "缩放到数据范围";

        }

        public override bool Enabled
        {
            get
            {
                bool b = false;
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey == null) return false;
                if (m_Hook.MapControl == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKey.ToString() != EnumTreeNodeType.DATAITEM.ToString()) return false;
                if (m_Hook.ProjectTree.SelectedNode.Parent == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.Parent.Tag == null) return false;
                try
                {
                    m_DataTypeID = Convert.ToInt32(m_Hook.ProjectTree.SelectedNode.Parent.Tag.ToString());
                    if (m_DataTypeID == 0 || m_DataTypeID == 1)
                    {
                        //标准图幅或非标准图幅才可以进行范围定位操作
                        b = true;
                    }
                    else
                    {
                        b = false;
                    }
                }
                catch
                {
                    return false;
                }
                return b;
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
            Exception eError = null;
            DevComponents.AdvTree.Node SelNode = m_Hook.ProjectTree.SelectedNode;

            long dataID = long.Parse(SelNode.Tag.ToString());    //数据ID
            //获得项目节点
            DevComponents.AdvTree.Node proNode = SelNode;    
            while (proNode.DataKey.ToString() != EnumTreeNodeType.PROJECT.ToString())
            {
                proNode = proNode.Parent;
            }
            long projectID = long.Parse(proNode.Tag.ToString());  //项目ID

            //获得根节点,数据库节点
            DevComponents.AdvTree.Node mDBNode = SelNode;
            while (mDBNode.Parent != null)
            {
                mDBNode = mDBNode.Parent;
            }
            //若不是数据库节点，就返回
            if (mDBNode == null) return;
            if (mDBNode.DataKey == null) return;
            if (mDBNode.DataKey.ToString() == "") return;
            if (mDBNode.DataKey.ToString() != EnumTreeNodeType.DATABASE.ToString()) return;
            if (mDBNode.Name == "文件连接")
            {
                //进行定位操作

                //连接数据库
                XmlElement dbElem = mDBNode.Tag as XmlElement;
                if (dbElem == null) return;
                string ipStr = dbElem.GetAttribute("MetaDBConn");

               // string ConnStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ipStr + ";Persist Security Info=True";//元数据连接字符串
                string ConnStr = ipStr;
                //设置元数据库连接
                SysCommon.DataBase.SysTable pSysDB = new SysCommon.DataBase.SysTable();
                pSysDB.SetDbConnection(ConnStr, SysCommon.enumDBConType.ORACLE, SysCommon.enumDBType.ORACLE, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接元数据库失败！连接地址为：" + ipStr);
                    return;
                }
                string str = "select * from ProjectMDTable where ID=" + projectID;
                DataTable dt = pSysDB.GetSQLTable(str, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "查找项目元信息表出错！");
                    pSysDB.CloseDbConnection();
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "找不到ID为：" + projectID + "的项目元信息！");
                    pSysDB.CloseDbConnection();
                    return;
                }
                string RangeDBPath = dt.Rows[0]["图幅结合表"].ToString().Trim();   //图幅结合表
                //查找数据对应的图幅的比例尺和图幅号
                string str1 = "";
                string mapNO = "";      //图幅号
                long mapSale = 0;       //图幅比例尺
                string feaClsName = ""; //范围图层名
                if (m_DataTypeID == 0)
                {
                    //标准图幅
                    str1 = "select 图幅号,图幅比例尺 from StandardMapMDTable where ID=" + dataID;
                    feaClsName = "MapFrame_";
                }
                else if (m_DataTypeID == 1)
                {
                    //非标准图幅
                    str1 = "select 块图号,块图比例尺 from NonstandardMapMDTable where ID=" + dataID;
                    feaClsName = "Range_";
                }
                DataTable tempDT = pSysDB.GetSQLTable(str1, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "查询数据表格出错！");
                    pSysDB.CloseDbConnection();
                    return;
                }
                if (tempDT.Rows.Count == 0)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "找不到数据ID为：" + dataID + "的数据元信息！");
                    pSysDB.CloseDbConnection();
                    return;
                }
                mapNO = tempDT.Rows[0][0].ToString().Trim();
                if (tempDT.Rows[0][1].ToString().Trim() != "")
                {
                    mapSale = long.Parse(tempDT.Rows[0][1].ToString().Trim());
                }
                if (mapSale == 0) return;   //若没有填写比例尺信息则返回

                feaClsName += mapSale.ToString();
                pSysDB.CloseDbConnection();

                SysCommon.Gis.SysGisDataSet pSysDT = new SysCommon.Gis.SysGisDataSet();
                pSysDT.SetWorkspace(RangeDBPath, SysCommon.enumWSType.PDB, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "连接范围库出错，连接地址为：\n" + RangeDBPath);
                    return;
                }
                IFeatureClass rangeFeaCls = pSysDT.GetFeatureClass(feaClsName, out eError);
                if (eError != null)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "查找范围图层出错，图层名为：\n" + feaClsName);
                    return;
                }
                try
                {
                    string whereStr = "MAP_NEWNO='" + mapNO + "'";
                    IQueryFilter pFilter = new QueryFilterClass();
                    pFilter.WhereClause = whereStr;
                    IFeatureCursor pCursor = rangeFeaCls.Search(pFilter,false);
                    if (pCursor == null) return;
                    IFeature pFea = pCursor.NextFeature();
                    if (pFea != null)
                    {
                        //进行定位
                        ZoomToFeature(m_Hook.MapControl, pFea);
                    }
                    else 
                    {
                        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误","范围要素不存在！");
                        return;
                    }

                    //释放CURSOR
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pCursor);
                }
                catch (Exception ex)
                {
                    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", ex.Message);
                    return;
                }
            }
            
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppFileRef;
            if (m_Hook == null) return;
        }


        /// <summary>
        /// 缩放到Feature
        /// </summary>
        /// <param name="pMapControl"></param>
        /// <param name="pFeature"></param>
        private void ZoomToFeature(IMapControlDefault pMapControl, IFeature pFeature)
        {
            if (pFeature == null) return;
            if (pFeature.Shape == null) return;
            IEnvelope pEnvelope = null;
            if (pFeature.Shape.GeometryType == esriGeometryType.esriGeometryPoint)
            {
                ITopologicalOperator pTop = pFeature.Shape as ITopologicalOperator;
                IGeometry pGeometry = pTop.Buffer(50);
                pEnvelope = pGeometry.Envelope;
            }
            else
            {
                pEnvelope = pFeature.Extent;
            }

            if (pEnvelope == null) return;
            pEnvelope.Expand(1.5, 1.5, true);
            IActiveView pActiveView = pMapControl.Map as IActiveView;
            pActiveView.Extent = pEnvelope;
            pActiveView.Refresh();
        }
       
    }
}
