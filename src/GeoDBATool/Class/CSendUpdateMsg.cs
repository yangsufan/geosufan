using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace GeoDBATool
{
    /// <summary>
    /// 发布更新类，根据开发1部提供的组件来初始化、连接发布消息 
    /// chenyafei add 20101011
    /// chenyafei modify  20101216 content：对创建本地SOCKET出错进行保护
    /// </summary>
    public class CSendUpdateMsg
    {

        //0..9分配给GeoGlobe监听系统
        //0 分配给与geoone的通讯session
        //#define LGEOLISTEN_SESSIONNAME  L"0"

        //从a..z为监听程序与外部系统交互的Session名称
        //a 分配给Geoone系统,如果有其他的系统就递增分配
        //#define LGEOONE_SESSIONNAME  L"a"

        //定义组件中要用到的常量
        string GEOLISTEN_SESSIONNAME ="0";//远程"Server"常量; 
        string GEOONE_SESSIONNAME = "a";  //本地session常量
        int CUSTOMITEMCODE = 10001;      //常量

        //用于远程发布的端口
        int GEOLISTEN_SOCKETPORT=10101;//远程端口
        int GEOONE_SOCKETPORT = 10102;//本地端口


        //监听与Geoone之间的指令------------------------------------------start
        //更新请求消息
        public string STRGEOONE_UPDATE = "1000"; //常量
        string STRDISCONNECT = "$DISDONNECT$";  //断开连接消息

        //定义发送消息组件类：center和session
        private GeoMsgCenterCOMLib.MessgageCenter m_Center = new GeoMsgCenterCOMLib.MessgageCenter();
        private GeoMsgCenterCOMLib.MessageSession m_Session = null;
       
        /// <summary>
        /// 初始化 远程发布
        /// </summary>
        /// <param name="ipStr">本机IP</param>
        /// <param name="pError">异常</param>
        public CSendUpdateMsg(string ipStr,Exception pError)
        {
            pError = null;
            try
            {
                string strGuid = System.Guid.NewGuid().ToString();      //生成唯一的ID
                m_Center.InitializeRemote(strGuid, GEOONE_SOCKETPORT);  //创建本地socket，用于远程发布
            }
            catch
            {

                pError=new Exception("创建本地socket失败！");
                return;
            }
          
            //m_Center.InitializeRemote(ipStr, GEOONE_SOCKETPORT);  //创建本地socket，用于远程发布
            ////try
            ////{

            //    //m_Center.Initialize(System.Guid.NewGuid().ToString());
            //m_Center.Initialize(GEOONE_SESSIONNAME);                //创建本地连接通讯
            ////    m_Session = m_Center.Connect(GEOLISTEN_SESSIONNAME, 0);
            ////    if(m_Session==null)
            ////    {
            ////        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "更新发布服务连接失败，请检查！");
            ////        return;
            ////    }
            ////    m_Center.OnNewSession += new GeoMsgCenterCOMLib._IMessgageCenterEvents_OnNewSessionEventHandler(m_Center_OnNewSession);
            ////    m_Session.OnMessage += new GeoMsgCenterCOMLib._IMessageSessionEvents_OnMessageEventHandler(m_Session_OnMessage);
            ////}
            ////catch(Exception ex)
            ////{
            ////    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "更新发布服务连接失败，请检查！");
            ////    return;
            ////}
        }

        /// <summary>
        /// 初始化 本地发布
        /// </summary>
        public CSendUpdateMsg()
        {
            

            ////try
            ////{

            //    //m_Center.Initialize(System.Guid.NewGuid().ToString());
            m_Center.Initialize(GEOONE_SESSIONNAME);                //创建本地连接通讯
            ////    m_Session = m_Center.Connect(GEOLISTEN_SESSIONNAME, 0);
            ////    if(m_Session==null)
            ////    {
            ////        SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "更新发布服务连接失败，请检查！");
            ////        return;
            ////    }
            ////    m_Center.OnNewSession += new GeoMsgCenterCOMLib._IMessgageCenterEvents_OnNewSessionEventHandler(m_Center_OnNewSession);
            ////    m_Session.OnMessage += new GeoMsgCenterCOMLib._IMessageSessionEvents_OnMessageEventHandler(m_Session_OnMessage);
            ////}
            ////catch(Exception ex)
            ////{
            ////    SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("错误", "更新发布服务连接失败，请检查！");
            ////    return;
            ////}
        }

        /// <summary>
        /// 创建远程连接
        /// </summary>
        /// <param name="remoteIPStr">远程ip地址</param>
        /// <param name="eError">异常</param>
        /// <returns>返回远程连接</returns>
        public GeoMsgCenterCOMLib.MessageSession CSendUpdateMsgConn(string remoteIPStr,out Exception eError)
        {
            eError = null;
            try
            {
                if (m_Center == null) return null;
                //创建远程连接
                m_Session = m_Center.Connect(remoteIPStr, GEOLISTEN_SOCKETPORT);
                if (m_Session == null)
                {
                    eError=new Exception("更新发布服务连接失败，请检查！");
                    return null;
                }
                //注册消息发送事件
                m_Center.OnNewSession += new GeoMsgCenterCOMLib._IMessgageCenterEvents_OnNewSessionEventHandler(m_Center_OnNewSession);
                m_Session.OnMessage += new GeoMsgCenterCOMLib._IMessageSessionEvents_OnMessageEventHandler(m_Session_OnMessage);
                return m_Session;
            }
            catch (Exception ex)
            {
                eError = ex;
                return null;
            }
        }

        /// <summary>
        /// 创建本地连接
        /// </summary>
        /// <param name="eError">异常</param>
        /// <returns>本地连接</returns>
        public GeoMsgCenterCOMLib.MessageSession CSendUpdateMsgConn(out Exception eError)
        {
            eError = null;
            try
            {
                if (m_Center == null) return null;
                //创建本地连接
          　　　 m_Session = m_Center.Connect(GEOLISTEN_SESSIONNAME, 0);//连接服务，用于本地发布
                if (m_Session == null)
                {
                    eError = new Exception("更新发布服务连接失败，请检查！");
                    return null;
                }
                //注册消息发送事件
                m_Center.OnNewSession += new GeoMsgCenterCOMLib._IMessgageCenterEvents_OnNewSessionEventHandler(m_Center_OnNewSession);
                m_Session.OnMessage += new GeoMsgCenterCOMLib._IMessageSessionEvents_OnMessageEventHandler(m_Session_OnMessage);
                return m_Session;
            }
            catch (Exception ex)
            {
                eError = ex;
                return null;
            }
        }


        void m_Center_OnNewSession(GeoMsgCenterCOMLib.MessageSession pSession)
        {
        }

        void m_Session_OnMessage(object varMsg, GeoMsgCenterCOMLib.enumMsgFlag nMsgFlag)
        {

        }

        /// <summary>
        /// 将消息字符串编码成字节
        /// </summary>
        /// <param name="str">消息字符串</param>
        /// <returns>返回编码后的字节数组</returns>
        public byte[] ToGB2312Bytes(string str)
        {
            return Encoding.GetEncoding("GB2312").GetBytes(str);
        }

        /// <summary>
        /// 将消息字符串编码成字节
        /// </summary>
        /// <param name="str">消息字符串</param>
        /// <returns>返回字节数组</returns>
        public byte[] ToBytes(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        /// <summary>
        /// 发送消息，矩形范围
        /// </summary>
        /// <param name="strProjectName">工程名称</param>
        /// <param name="xmin">范围最小X坐标</param>
        /// <param name="ymin">范围最小y坐标</param>
        /// <param name="xmax">范围最大X坐标</param>
        /// <param name="ymax">范围最大Y坐标</param>
        /// <returns>是否发送成功，true：发送成功；false：发送失败</returns>
        public bool SendEnvelopUpdateMsg(string strProjectName, double xmin, double ymin, double xmax, double ymax)
        {
            //声明xml变量
            System.Xml.XmlDocument vDoc = new System.Xml.XmlDocument();
            //添加xml节点
            System.Xml.XmlNode vRoot = vDoc.AppendChild(vDoc.CreateElement("CMD"));
            //创建节点
            AppendXmlAtt(vDoc, vRoot, "val", STRGEOONE_UPDATE);
            //创建工程名称呢个节点并赋值
            vRoot.AppendChild(vDoc.CreateElement("ProjectName")).InnerText = strProjectName;

            //创建范围节点及其自己坐标
            System.Xml.XmlNode vNode = vRoot.AppendChild(vDoc.CreateElement("Envelope"));
            AppendXmlAtt(vDoc, vNode, "XMin", xmin.ToString());
            AppendXmlAtt(vDoc, vNode, "XMax", xmax.ToString());
            AppendXmlAtt(vDoc, vNode, "YMin", ymin.ToString());
            AppendXmlAtt(vDoc, vNode, "YMax", ymax.ToString());
            //发送消息
            return SendUpdateMsg(vDoc.OuterXml);
        }

        /// <summary>
        /// 发送消息 多边形范围
        /// </summary>
        /// <param name="strProjectName">工程名称</param>
        /// <param name="pointarray">点集合</param>
        /// <param name="interpret">标志多边形类型</param>
        /// <returns>是否发送成功，true：消息发送成功；false：消息发送失败</returns>
        public bool SendPolygonUpdateMsg(string strProjectName,string pointarray,string interpret)
        {
            //声明xml变量
            System.Xml.XmlDocument vDoc = new System.Xml.XmlDocument();
            //添加xml节点
            System.Xml.XmlNode vRoot = vDoc.AppendChild(vDoc.CreateElement("CMD"));
            //创建节点
            AppendXmlAtt(vDoc, vRoot, "val", STRGEOONE_UPDATE);
            //创建工程名称呢个节点并赋值
            vRoot.AppendChild(vDoc.CreateElement("ProjectName")).InnerText = strProjectName;

            //创建范围节点及其自己坐标
            System.Xml.XmlNode vNode = vRoot.AppendChild(vDoc.CreateElement("Polygon"));
            AppendXmlAtt(vDoc, vNode, "Coord", pointarray);
            //发送消息
            return SendUpdateMsg(vDoc.OuterXml);
        }

        /// <summary>
        /// 发送消息函数
        /// </summary>
        /// <param name="strUpdateXml">组织好的消息XML文档</param>
        /// <returns>是否发送成功，true：消息发送成功；false：消息发送失败</returns>
        public bool SendUpdateMsg(string strUpdateXml)
        {
            //消息打包类
            GeoMsgCenterCOMLib.MsgPackager vPackage = new GeoMsgCenterCOMLib.MsgPackager();
            //消息开始打包
            vPackage.BeginPack();
            //将消息xml打包转化为组件能够识别的字节类型
            vPackage.AddMsgItem(CUSTOMITEMCODE, ToGB2312Bytes(strUpdateXml));
            //vPackage.AddMsgItem(CUSTOMITEMCODE, ToBytes(strUpdateXml));
            //vPackage.AddMsgItem(CUSTOMITEMCODE, strUpdateXml);
            //结束消息打包
            vPackage.FinishPack();
            if (m_Session == null) return false;
            //发送消息
            return m_Session.SendMessagePackager(vPackage);

        }
        /// <summary>
        /// 创建名称为strName的xml节点，并为其设置值
        /// </summary>
        /// <param name="vDoc">xml文档</param>
        /// <param name="vNode">创建或获取的节点</param>
        /// <param name="strName">节点名称</param>
        /// <param name="strValue">节点值</param>
        /// <returns>是否创建成功，true：创建成功</returns>
        public bool AppendXmlAtt(System.Xml.XmlDocument vDoc, System.Xml.XmlNode vNode, string strName, string strValue)
        {
            //检索节点
            System.Xml.XmlNode vAttNode = vNode.Attributes.GetNamedItem(strName);
            //若节点不存在，则创建节点
            if (vAttNode == null)
                vAttNode = vDoc.CreateAttribute(strName);
            //设置节点值
            vAttNode.Value = strValue;
            //添加节点
            vNode.Attributes.SetNamedItem(vAttNode);
            return true;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void DisConnect()
        {
            //发送断开连接消息
            SendUpdateMsg(STRDISCONNECT);
        }
    }
}

