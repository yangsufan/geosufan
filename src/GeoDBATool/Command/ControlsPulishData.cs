using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ESRI.ArcGIS.Geometry;

namespace GeoDBATool
{
    /// <summary>
    /// 提交更新后，发布更新数据   陈亚飞 添加20101011
    /// </summary>
    public class ControlsPulishData: Plugin.Interface.CommandRefBase
    {

        private Plugin.Application.IAppGISRef m_Hook;
        XmlElement workDBElem = null;                     //范围树节点

        public ControlsPulishData()
        {
            base._Name = "GeoDBATool.ControlsPulishData";
            base._Caption = "发布更新";
            base._Tooltip = "发布更新";
            base._Visible = true;
            base._Enabled = true;
            base._Message = "发布更新";

        }

        public override bool Enabled
        {
            get
            {
                if (m_Hook == null) return false;
                if (m_Hook.CurrentThread != null) return false;
                if (m_Hook.ProjectTree.SelectedNode == null) return false;
                if (m_Hook.ProjectTree.SelectedNode.DataKeyString != "project") return false;
                if (m_Hook.ProjectTree.SelectedNode.Tag == null) return false;
                XmlNode ProNode = m_Hook.ProjectTree.SelectedNode.Tag as XmlNode;
                if (ProNode == null) return false;
                workDBElem = ProNode.SelectSingleNode(".//内容//图幅工作库//范围信息") as XmlElement;
                if (workDBElem == null) return false;
                if (!workDBElem.HasAttribute("范围")) return false;
                string pRangeStr = workDBElem.GetAttribute("范围").Trim();
                if (pRangeStr == "") return false;
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
            //IGeometry pGeo = null;
            ////通过解析得到几何范围
            //if (workDBElem == null)
            //{
            //    return;
            //}
            //string pRangeStr = workDBElem.GetAttribute("范围").Trim();
            //if (pRangeStr == "") 
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "几何范围为空！");
            //    return;
            //}
            ////进行解析
            //byte[] xmlByte = Convert.FromBase64String(pRangeStr);
            //object pGeoObj = new PolygonClass();
            //if (XmlDeSerializer(xmlByte, pGeoObj) == false)
            //{
            //    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "几何范围解析出错！");
            //    return;
            //}

            //pGeo = pGeoObj as IGeometry;
            //if (pGeo == null) return;

            ////设置参数
            //string strProjectName = m_Hook.ProjectTree.SelectedNode.Name;// 工程名 "新建项目";
            
            //IEnvelope pEnvelo = pGeo.Envelope;
            ////调用更新发布接口

            ////程序启动时的代码，初始化
            //CSendUpdateMsg msg = new CSendUpdateMsg();

            ////将外接矩形geometry和原geometry进行比较
            //if ((pEnvelo as IGeometry) == pGeo)
            //{
            //    //若一样，则按第一种方法来发送消息

            //    //获得范围的外接矩形的最小最大坐标
            //    double pMinX = 0;
            //    double pMaxX = 0;
            //    double pMinY = 0;
            //    double pMaxY = 0;
            //    pEnvelo.QueryCoords(out pMinX, out pMinY, out pMaxX, out pMaxY);

            //    //第一种情况  矩形

            //    if (msg.SendEnvelopUpdateMsg(strProjectName, pMinX, pMinY, pMaxX, pMaxY))
            //    {
            //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "消息发送成功！");
            //    }
            //    else
            //    {
            //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "消息发送失败！");
            //    }
           
            //}
            //else
            //{
            //    //若不一样，则按照第二种方法来发送消息
            //    double[] vCood=GetColByPolygon(pGeo);
            //    if(vCood==null)
            //    {
            //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "获取几何图形坐标串出错！");
            //        return;
            //    }

            //    //第二种情况
            //    if (msg.SendPolygonUpdateMsg(strProjectName, vCood))
            //    {
            //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "消息发送成功！");
            //    }
            //    else
            //    {
            //        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "消息发送失败！");
            //    }

            //}

            ////释放
            //msg.DisConnect(msg.STRDISCONNECT);

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            m_Hook = hook as Plugin.Application.IAppGISRef;
        }

        // 从范围Polygon得到对应的坐标字符串  陈亚飞添加
        private double[] GetColByPolygon(IGeometry polygon)
        {
            
            if (polygon == null) return null;
            IPointCollection pPointCol = (IPointCollection)polygon;
            double[] vCood = new double[pPointCol.PointCount * 2];
            try
            {
                int j=0;
                for (int index = 0; index < pPointCol.PointCount; index++)
                {
                    IPoint pPoint = pPointCol.get_Point(index);
                    vCood[j] = pPoint.X;
                    vCood[j + 1] = pPoint.Y;

                    j=j+2;
                }
                return vCood;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// 将xmlByte解析为obj
        /// </summary>
        /// <param name="xmlByte"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool XmlDeSerializer(byte[] xmlByte, object obj)
        {
            try
            {
                //判断字符串是否为空
                if (xmlByte != null)
                {
                    ESRI.ArcGIS.esriSystem.IPersistStream pStream = obj as ESRI.ArcGIS.esriSystem.IPersistStream;

                    ESRI.ArcGIS.esriSystem.IXMLStream xmlStream = new ESRI.ArcGIS.esriSystem.XMLStreamClass();

                    xmlStream.LoadFromBytes(ref xmlByte);
                    pStream.Load(xmlStream as ESRI.ArcGIS.esriSystem.IStream);

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}