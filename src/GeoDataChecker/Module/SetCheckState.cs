using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesGDB;
using System.Data;

namespace GeoDataChecker
{
    /// <summary>
    /// 定义检查时使用到的状态及相关的全局变量 及一些可共用的函数委托
    /// </summary>
   public class SetCheckState
    {
       public static bool GeoCor = false;//用来确定检查功能里是否有用到开启和关闭编辑的操作
       public static bool CheckState = false;//检查的状态，根据对应的数据来设置
       public static ITopology pT=null;//拓扑
       public static IGeoDataset Geodatabase = null;//设置一个集合
       public static ITopologyRuleContainer pRuleCont;//一个拓扑规则检查的集合容器
       public static string CheckDataBaseName = "";//当前选择的库体名称
       /// <summary>
       /// 申明一个委托，用来弹出对话框
       /// </summary>
       /// <param name="tip"></param>
       /// <param name="content"></param>
       private delegate void ShowContent(string tip, string content);
       /// <summary>
       /// 弹出对话框的具体实现函数
       /// </summary>
       /// <param name="tip"></param>
       /// <param name="content"></param>
       private static void ShowMessage(string tip, string content)
       {
           SysCommon.Error.ErrorHandle.ShowFrmErrorHandle(tip, content);//调用之前定义过的弹出对话框函数
       }
       /// <summary>
       /// 实际调用函数 弹出对话框
       /// </summary>
       /// <param name="pAppForm"></param>
       public static void Message(Plugin.Application.IAppFormRef pAppForm,string tip,string content)
       {

           pAppForm.MainForm.Invoke(new ShowContent(ShowMessage), new object[] { tip, content });
       }
       /// <summary>
       /// 状态栏显示内容的委托
       /// </summary>
       /// <param name="pAppForm"></param>
       /// <param name="tip"></param>
       private delegate void ShowTips(Plugin.Application.IAppFormRef pAppForm, string tip);
       /// <summary>
       /// 状态栏显示内容的方法
       /// </summary>
       /// <param name="pAppForm"></param>
       /// <param name="tip"></param>
       private static void ShowTipsFun(Plugin.Application.IAppFormRef pAppForm, string tip)
       {
           pAppForm.OperatorTips = tip;//将我们传入的内容显示在状态栏上
       }
       /// <summary>
       /// 在使用线程时，窗体控件要使用窗体的安全线程委托进行调用
       /// </summary>
       /// <param name="pAppForm"></param>
       /// <param name="tip"></param>
       public static void CheckShowTips(Plugin.Application.IAppFormRef pAppForm, string tip)
       {
           pAppForm.MainForm.Invoke(new ShowTips(ShowTipsFun), new object[] { pAppForm, tip });
       }
       /// <summary>
       /// 初始化检查树图 使用拓扑规则适用 委托
       /// </summary>
       /// <param name="list"></param>
       private delegate void Ini_Tree(ArrayList list,Plugin.Application.IAppGISRef pAppForm);
       /// <summary>
       /// 初始化树的主要方法
       /// </summary>
       /// <param name="list"></param>
       /// <param name="pAppForm"></param>
       private static void Ini_trees(ArrayList list, Plugin.Application.IAppGISRef pAppForm)
       {
           ImageList imglist = pAppForm.DataTree.ImageList;
           pAppForm.DataTree.Nodes.Clear();
           pAppForm.DataTree.Columns.Clear();
           DevComponents.AdvTree.Node Firstnode = new DevComponents.AdvTree.Node();
           Firstnode.Text = Overridfunction.name;
           Firstnode.Image = imglist.Images[3];
           foreach (IFeatureClass TempClass in list)
           {
               IDataset ds = TempClass as IDataset;
               DevComponents.AdvTree.Node TempNode = new DevComponents.AdvTree.Node();
               TempNode.Text = ds.Name;
               TempNode.Image = imglist.Images[6];
               Firstnode.Nodes.Add(TempNode);
           }
           Firstnode.Expand();
           pAppForm.DataTree.Nodes.Add(Firstnode);
       }
       /// <summary>
       /// 配合检查功能初始化树图
       /// </summary>
       /// <param name="list"></param>
       /// <param name="pAppForm"></param>
       public static void TreeIni_Fun(ArrayList list, Plugin.Application.IAppGISRef pAppForm)
       {
           Plugin.Application.IAppFormRef Form = pAppForm as Plugin.Application.IAppFormRef;
           Form.MainForm.Invoke(new Ini_Tree(Ini_trees), new object[] { list, pAppForm });
       }
       /// <summary>
       /// 设置进度操作时选中树节点
       /// </summary>
       /// <param name="name"></param>
       /// <param name="index"></param>
       private delegate void TreeCheck(string name, int index, Plugin.Application.IAppGISRef pAppForm);
       /// <summary>
       /// 选中的具体方法
       /// </summary>
       /// <param name="name"></param>
       /// <param name="index"></param>
       /// <param name="pAppForm"></param>
       private static void TreeCheck_Fun(string name, int index, Plugin.Application.IAppGISRef pAppForm)
       {
           if (pAppForm.DataTree.Nodes[0].Nodes[index].Text == name)
           {
               pAppForm.DataTree.SelectedNode=pAppForm.DataTree.Nodes[0].Nodes[index];//选中树节点

           }
       }
       /// <summary>
       /// 可供外部调用的选中方法 树节点选中
       /// </summary>
       /// <param name="name"></param>
       /// <param name="index"></param>
       /// <param name="pAppForm"></param>
       public static void TreeCheckFun(string name, int index, Plugin.Application.IAppGISRef pAppForm)
       {
           Plugin.Application.IAppFormRef Form = pAppForm as Plugin.Application.IAppFormRef;
           Form.MainForm.Invoke(new TreeCheck(TreeCheck_Fun), new object[] {name,index, pAppForm });
       }
   }
}
