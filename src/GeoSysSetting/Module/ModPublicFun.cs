using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Windows.Forms;
using SysCommon.Gis;
using SysCommon.Error;
using SysCommon.Authorize;
using ESRI.ArcGIS.esriSystem;

using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.IO;
namespace GeoSysSetting
{
    public static class ModPublicFun
    {
        public static String _layerTreePath = Application.StartupPath + "\\..\\res\\xml\\配置图层树.xml";
        public static bool _SaveLayerTree = true;
        private static string _LogFilePath = Application.StartupPath + "\\..\\Log\\GeoSysSetting.txt";
        public static void WriteLog(string strLog)
        {
            //判断文件是否存在  不存在就创建添加写日志的函数，为了测试加载历史数据的效率
            if (!File.Exists(_LogFilePath))
            {
                System.IO.FileStream pFileStream = File.Create(_LogFilePath);
                pFileStream.Close();
            }
            //FileStream fs = File.Open(_LogFilePath,FileMode.Append);

            //StreamReader reader = new StreamReader(fs, Encoding.GetEncoding("gb2312"));

            string strTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string strread = strLog + "     " + strTime + "\r\n";
            StreamWriter sw = new StreamWriter(_LogFilePath, true, Encoding.GetEncoding("gb2312"));
            sw.Write(strread);
            sw.Close();
            //fs.Close();
            sw = null;
            //fs = null;
        }
        /// <summary>
        /// 将权限文档显示在listView上
        /// </summary>
        /// <param name="document"></param>
        /// <param name="view"></param>
        ///        
        //added by chulili若图层目录修改，则提示保存
        public static void DealChangeSave()
        {
            //_SaveLayerTree = false;//用户是否选择保存目录
            if (SysCommon.ModSysSetting.IsLayerTreeChanged == false)
            {
                //_SaveLayerTree = true;
                return;
            }
            if (MessageBox.Show("是否保存您对图层目录的修改?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                GeoLayerTreeLib.LayerManager.ModuleMap.SaveLayerTree(Plugin.ModuleCommon.TmpWorkSpace, _layerTreePath);
                _SaveLayerTree = true;
            }
            else
            {
                _SaveLayerTree = false;
            }
            if (SysCommon.ModSysSetting.IsLayerTreeChanged)
            {
                SysCommon.ModSysSetting.IsConfigLayerTreeChanged = true;
            }
            SysCommon.ModSysSetting.IsLayerTreeChanged = false;

        }
        public static void DisplaylayerInLstView(XmlDocument document, DevComponents.AdvTree.AdvTree tree,ImageList pImgList)
        {
            if (document.DocumentElement != null)
            {
                tree.Nodes.Clear();
                tree.Tag = document;
                string xPath = "//Root";
                XmlNode rootNode = document.DocumentElement;
                XmlNodeList nodeList = rootNode.SelectNodes(xPath);
                if (nodeList == null) return;
                foreach (XmlNode node in nodeList)
                {
                    XmlElement pElement = node as XmlElement;
                    string caption = pElement.GetAttribute("NodeText") == null ? "" : pElement.GetAttribute("NodeText");
                    string strKey = pElement.GetAttribute("NodeKey") == null ? "" : pElement.GetAttribute("NodeKey");

                    DevComponents.AdvTree.Node aNode = new DevComponents.AdvTree.Node();
                    aNode.Text = caption;
                    aNode.Name = strKey;
                    aNode.Tag = node.Name;
                    aNode.Expanded = true;
                    switch (node.Name)
                    {
                        case "Root":
                            aNode.Image=pImgList.Images["Root"];
                            break;
                        case "DIR":
                            aNode.Image=pImgList.Images["DIR"];
                            break;
                        case "DataDIR":
                            aNode.Image = pImgList.Images["DataDIROpen"];
                            break;
                        case "Layer":
                            aNode.Image = pImgList.Images["Layer"];
                            break;

                    }
                    tree.Nodes.Add(aNode);
                    if (node.HasChildNodes)
                    {
                        DisPlaySublayerNodeView(node, aNode,pImgList );
                    }
                }
            }
        }
        /// <summary>
        /// 将子结点显示到listView上
        /// </summary>
        /// <param name="pNode"></param>
        /// <param name="View"></param>
        private static void DisPlaySublayerNodeView(XmlNode xNode, DevComponents.AdvTree.Node treeNode,ImageList pImgList)
        {
            foreach (XmlNode node in xNode.ChildNodes)
            {
                if (node.Name != "DIR" && node.Name != "DataDIR" && node.Name != "Layer")
                    continue;
                XmlElement pElement = node as XmlElement;
                if (pElement == null) return;
                string caption = pElement.GetAttribute("NodeText") == null ? "" : pElement.GetAttribute("NodeText");
                string strID = pElement.GetAttribute("NodeKey") == null ? "" : pElement.GetAttribute("NodeKey");

                DevComponents.AdvTree.Node aNode = new DevComponents.AdvTree.Node();
                aNode.Text = caption;
                aNode.Name = strID;
                aNode.Tag = node.Name;
                switch (node.Name)
                {
                    case "Root":
                        aNode.Image = pImgList.Images["Root"];
                        break;
                    case "DIR":
                        aNode.Image = pImgList.Images["DIR"];
                        break;
                    case "DataDIR":
                        aNode.Image = pImgList.Images["DataDIROpen"];
                        break;
                    case "Layer":
                        aNode.Image = pImgList.Images["Layer"];
                        break;

                }
                treeNode.Nodes.Add(aNode);
                if (node.HasChildNodes)
                {
                    DisPlaySublayerNodeView(node, aNode,pImgList );
                }
            }
        }





    }
}