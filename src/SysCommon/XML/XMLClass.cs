using System;
using System.Collections.Generic;
using System.Text;

namespace SysCommon.XML
{
    public class XMLClass
    {
        #region
        public static void XmlSerializer(System.Object xmlObject, System.String xmlNodeName, out string strXML)
        {
            strXML = "";
            System.String elementURI = "http://www.esri.com/schemas/ArcGIS/9.2";
            // Create xml writer           
            ESRI.ArcGIS.esriSystem.IXMLWriter xmlWriterCls = new ESRI.ArcGIS.esriSystem.XMLWriterClass();
            // Create xml stream            
            ESRI.ArcGIS.esriSystem.IXMLStream xmlStreamCls = new ESRI.ArcGIS.esriSystem.XMLStreamClass();
            // Explicit Cast for IStream and then write to stream             
            xmlWriterCls.WriteTo((ESRI.ArcGIS.esriSystem.IStream)xmlStreamCls);
            // Serialize             
            ESRI.ArcGIS.esriSystem.IXMLSerializer xmlSerializerCls = new ESRI.ArcGIS.esriSystem.XMLSerializerClass();
            xmlSerializerCls.WriteObject(xmlWriterCls, null, null, xmlNodeName, elementURI, xmlObject);
            // Save the xmlObject to an xml file. When using xmlstream the cpu keeps data in memory until it is written to file.            
            //xmlStreamCls.SaveToFile(@xmlPathFile);
            strXML = xmlStreamCls.SaveToString();
        }

        public static object XmlDeSerializer(string xmlPathFile)
        {
            // Create xmlStream and load in the .XML file            
            ESRI.ArcGIS.esriSystem.IXMLStream xmlStreamCls = new ESRI.ArcGIS.esriSystem.XMLStream();
            xmlStreamCls.LoadFromFile(xmlPathFile);
            // Create xmlReader and read the XML stream            
            ESRI.ArcGIS.esriSystem.IXMLReader xmlReaderCls = new ESRI.ArcGIS.esriSystem.XMLReader();
            xmlReaderCls.ReadFrom((ESRI.ArcGIS.esriSystem.IStream)xmlStreamCls);
            // Explicit Cast
            // Create a serializer           
            ESRI.ArcGIS.esriSystem.IXMLSerializer xmlSerializerCls = new ESRI.ArcGIS.esriSystem.XMLSerializer();
            // Return the XML contents            
            return xmlSerializerCls.ReadObject(xmlReaderCls, null, null);
        }

        public static object XmlDeSerializer2(string xmlValue)
        {
            // Create xmlStream and load in the .XML file            
            ESRI.ArcGIS.esriSystem.IXMLStream xmlStreamCls = new ESRI.ArcGIS.esriSystem.XMLStream();
            xmlStreamCls.LoadFromString(xmlValue);
            // Create xmlReader and read the XML stream            
            ESRI.ArcGIS.esriSystem.IXMLReader xmlReaderCls = new ESRI.ArcGIS.esriSystem.XMLReader();
            xmlReaderCls.ReadFrom((ESRI.ArcGIS.esriSystem.IStream)xmlStreamCls);
            // Explicit Cast
            // Create a serializer           
            ESRI.ArcGIS.esriSystem.IXMLSerializer xmlSerializerCls = new ESRI.ArcGIS.esriSystem.XMLSerializer();
            // Return the XML contents            
            return xmlSerializerCls.ReadObject(xmlReaderCls, null, null);
        }
        #endregion
    }
}
