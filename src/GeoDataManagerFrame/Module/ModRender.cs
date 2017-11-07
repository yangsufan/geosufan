//*********************************************************************************
//** 文件名：ModRender.cs
//** CopyRight (c) 2000-2007 武汉吉奥信息工程技术有限公司工程部
//** 创建人：chulili
//** 日  期：2011-03-15
//** 修改人：
//** 日  期：
//** 描  述：用于图层符号化 
//**
//** 版  本：1.0
//*********************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using SysCommon.Gis;
using System.Xml;
using System.IO;

using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using GeoDataCenterFunLib;
namespace GeoDataManagerFrame
{
    public class ModRender
    {   
        //函数重载
        public static void SetRenderByXML(ILayer pLayer)
        {
            SetRenderByXML( pLayer,null);
        }
        //added by chulili
        //函数功能：根据xml配置文件为图层设置符号(xml配置文件暂时采用指定全路径)
        //输入参数：图层 日志文件  输出参数：无
        public static void SetRenderByXML(ILayer pLayer,LogFile Log)
        {
            string serverstylename;     //符号库名称

            XmlDocument xmldoc = new XmlDocument();
            string strCurFile = Application.StartupPath + "\\..\\Template\\Render.xml";
            //查找配置文件中关于该图层信息
            string strSearchExp = "//GisLayer [@ItemName='" + pLayer.Name  + "']";
            XmlNode xmlRenderNode;
            if (File.Exists(strCurFile) == false)
            {
                if (Log != null) Log.Writelog("符号配置文件不存在");
                return;
            }
            if (Log!=null) Log.Writelog("加载符号配置文件");
            //读取xml配置文件
            xmldoc.Load(strCurFile);
            xmlRenderNode = xmldoc.SelectSingleNode(strSearchExp);
            if (xmlRenderNode == null)
            {
                if (Log != null) Log.Writelog("符号配置文件中不存在图层节点");
                return;
            }
            if (Log != null) Log.Writelog("读取符号配置文件信息");
            XmlElement xmlElent = (XmlElement)xmlRenderNode;
            string strStyle, ColName,defaultname,defaultLab;
            //读取图层节点属性值
            strStyle = xmlElent.GetAttribute("sTyle");
            ColName = xmlElent.GetAttribute("ColName");         //符号化依据的字段名称
            defaultname = xmlElent.GetAttribute("DefaultSymbol");//默认符号
            defaultLab = xmlElent.GetAttribute("DefaultLabel");  //默认符号的标签
            IFeatureLayer pFLayer = pLayer as IFeatureLayer;
            IFeatureClass fcls = pFLayer.FeatureClass;
            IUniqueValueRenderer ptmpRender;
            ptmpRender = new UniqueValueRendererClass();
            //固定按照一个字段进行符号化，设置符号化字段
            ptmpRender.FieldCount = 1;
            ptmpRender.set_Field(0, ColName);
            ptmpRender.RemoveAllValues();
            //读取符号库（暂时使用固定的符号库，以后应该可配置）
            string stylefileFullname = Application.StartupPath + @"\..\Styles\testStyle.ServerStyle";
            string colvalue, symbolname, labelname;
            if (Log != null) Log.Writelog("读取符号库,为图层设置符号");
            foreach (XmlNode xmlchild in xmlRenderNode.ChildNodes)//根据xml构造每个symbol
            {
                if (xmlchild.NodeType.ToString().Equals("Element"))
                {   
                    //读取符号节点的属性
                    xmlElent = (XmlElement)xmlchild;
                    colvalue = xmlElent.GetAttribute("ColValue");   //字段值
                    symbolname = xmlElent.GetAttribute("Symbol");   //符号名
                    labelname = xmlElent.GetAttribute("Label");     //符号标签
                    //根据符号库全路径，符号类别，符号名获取符号
                    ISymbol pSymbol = GetSymbol(stylefileFullname, strStyle, symbolname);
                    //添加符号
                    ptmpRender.AddValue(colvalue, "", pSymbol);
                    //设置符号标签
                    ptmpRender.set_Label(colvalue, labelname);
                }
            }
            //设置默认符号（即地物的字段值不满足其他渲染条件的情况下，地物采用的符号）
            ISymbol pDefaultSym = GetSymbol(stylefileFullname, strStyle, defaultname);
            ptmpRender.DefaultSymbol = pDefaultSym;
            ptmpRender.DefaultLabel = defaultLab;
            if (ptmpRender.DefaultSymbol!=null)
                ptmpRender.UseDefaultSymbol=true;
            (pFLayer as IGeoFeatureLayer).Renderer = ptmpRender as IFeatureRenderer;
            if (Log != null) Log.Writelog("图层符号配置完成");

        }
        //added by chulili 
        //函数功能：获取符号库中的符号
        //输入参数：符号库全路径 符号类别  符号名
        //输出参数：符号
        //代码来源：借鉴同事代码
        private static ISymbol GetSymbol(string stylefileFullname, string styleClassName, string sSymbolName)
        {


            ISymbol pSymbol = null;
            IStyleGallery pStyleGallery = new ServerStyleGalleryClass();
            IStyleGalleryStorage pStyleGalleryStorage;
            IEnumStyleGalleryItem pEnumStyleGalleryItem = new EnumServerStyleGalleryItemClass();
            pStyleGalleryStorage = pStyleGallery as IStyleGalleryStorage;
            pStyleGalleryStorage.AddFile(stylefileFullname);

            pEnumStyleGalleryItem = pStyleGallery.get_Items(styleClassName, stylefileFullname, "");
            pEnumStyleGalleryItem.Reset();
            IStyleGalleryItem pEnumItem;
            pEnumItem = pEnumStyleGalleryItem.Next();
            while (pEnumItem != null)
            {
                if (pEnumItem.Name == sSymbolName)
                {
                    pSymbol = pEnumItem.Item as ISymbol;

                    break;
                }
                pEnumItem = pEnumStyleGalleryItem.Next();
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pEnumStyleGalleryItem);
            return pSymbol;
        }
    }
}
