using System;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Xml;
using DevComponents.DotNetBar;
namespace Plugin
{
    public static class ModScale
    {
        private static string _ScaleXmlPath = System.Windows.Forms.Application.StartupPath  + "\\..\\res\\xml\\ScaleConfig.xml";
        //获取当前系统的比例尺配置信息
        public static void GetScaleConfig(out object[] objScale, out int intWidth)
        {
            intWidth = 75;
            if (!File.Exists(_ScaleXmlPath))
            {
                objScale = new object[1];
                return;
            }
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.Load(_ScaleXmlPath);
            string strSearch = "//ScaleConfig";
            XmlNode pXmlnode = pXmldoc.SelectSingleNode(strSearch);
            if (pXmlnode == null)
            {
                objScale = new object[1];
                pXmldoc = null;
                return;
            }
            XmlElement pXmlele = pXmlnode as XmlElement;
            //获取小数位数
            string strWidth = "";
            if (pXmlele.HasAttribute("ComboWidth"))
            {
                strWidth = pXmlele.GetAttribute("ComboWidth");
            }
            try
            {
                if (strWidth != "")
                {
                    intWidth = Convert.ToInt32(strWidth);
                    if (intWidth < 0) intWidth = 75;
                }
            }
            catch 
            { }
            //获取比例尺字符串
            string strScale = "";
            if (pXmlnode.HasChildNodes)
            {
                objScale = new object[pXmlnode.ChildNodes.Count];
                for (int i = 0; i < pXmlnode.ChildNodes.Count; i++)
                {
                    strScale = pXmlnode.ChildNodes[i].Attributes[0].Value;
                    try
                    {
                        objScale[i] = Convert.ToInt32(strScale) as object;
                    }
                    catch (Exception e)
                    {
                        string strerr = e.Message;
                    }

                }
                pXmldoc = null;
                return;
            }
            objScale = new object[1];
            pXmldoc = null;
 
        }
        //获取当前系统的比例尺配置信息，重载，获取比例尺
        public static void GetScaleConfig(out object[] objScale)
        {

            if (!File.Exists(_ScaleXmlPath))
            {
                objScale=new object[1];
                return;
            }
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.Load(_ScaleXmlPath);
            string strSearch = "//ScaleConfig";
            XmlNode pXmlnode = pXmldoc.SelectSingleNode(strSearch);
            if (pXmlnode == null)
            {
                objScale = new object[1];
                pXmldoc = null;
                return;
            }
            //获取比例尺字符串
            string strScale = "";
            if (pXmlnode.HasChildNodes)
            {
                objScale = new object[pXmlnode.ChildNodes.Count];
                for (int i = 0; i < pXmlnode.ChildNodes.Count; i++)
                {
                    strScale =pXmlnode.ChildNodes[i].Attributes[0].Value ;
                    try 
                    {
                        objScale[i] = Convert.ToInt32(strScale) as object ;
                    }
                    catch(Exception e)
                    {
                        string strerr = e.Message;
                    }

                }
                pXmldoc = null;
                return;
            }
            objScale = new object[1];
            pXmldoc = null;
        }
        //获取当前系统的比例尺配置信息，重载，获取小数位数
        public static void GetScaleConfig(out int intDecimal)
        {
            intDecimal = 2;
            if (!File.Exists(_ScaleXmlPath))
            {
                return;
            }
            XmlDocument pXmldoc = new XmlDocument();
            pXmldoc.Load(_ScaleXmlPath);
            string strSearch = "//ScaleConfig";
            XmlNode pXmlnode = pXmldoc.SelectSingleNode(strSearch);
            if (pXmlnode == null)
            {
                pXmldoc = null;
                return;
            }
            //获取小数点位数
            XmlElement pXmlele = pXmlnode as XmlElement;
            string strDecimal = "";
            if (pXmlele.HasAttribute("Decimal"))
            {
                strDecimal = pXmlele.GetAttribute("Decimal");
            }
            try
            {
                if (strDecimal != "")
                {
                    intDecimal = Convert.ToInt32(strDecimal);
                    if (intDecimal < 0) intDecimal = 0;
                }
            }
            catch
            { }           
        }
    }
}