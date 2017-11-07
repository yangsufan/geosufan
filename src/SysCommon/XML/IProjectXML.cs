using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SysCommon.XML
{
    public interface IProjectXML
    {
        //根据工程文档类型获得模板文档
        XmlDocument GetProjectDocumentSchema(ProjectType pType);

        //添加新建工程节点
        XmlNode AddNewProjectNode(string ProjectFilePath);




    }

    public enum ProjectType
    {
        LoginShecma, InnerProjectSchema, BoundProjectSchema
    }

}
