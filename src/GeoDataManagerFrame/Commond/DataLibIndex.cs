using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using GeoDataCenterFunLib;
using SysCommon.Gis;
using SysCommon;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;

//数据仓库目录
namespace GeoDataManagerFrame
{
    public class DataLibIndex : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGisUpdateRef m_Hook;
        private Plugin.Application.IAppFormRef m_frmhook;
        private enumWSType wsType;
        public DataLibIndex()
        {
            base._Name = "GeoDataManagerFrame.DataLibIndex";
            base._Caption = "加载目录";
            base._Tooltip = "加载数据目录";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "加载目录";
        }

        public override void OnClick()
        {
            if (m_Hook != null)
            {
                LogFile log = new LogFile(m_Hook.tipRichBox, m_Hook.strLogFilePath);

                if (log != null)
                {
                    log.Writelog("加载数据目录");
                }


                //根据连接的数据库信息 初始化数据单元树
                InitDataUnitTree();

                //加载数据
                if (m_Hook.gisDataSet == null)
                {
                    m_Hook.gisDataSet = new SysGisDataSet();
                }
                if (m_Hook.gisDataSet.WorkSpace == null)
                {
                    GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
                    string mypath = dIndex.GetDbValue("dbServerPath");
                    string dbType = dIndex.GetDbValue("dbType");
                    string dataBase = dIndex.GetDbValue("dbServerName");
                    string user = dIndex.GetDbValue("dbUser");
                    string password = dIndex.GetDbValue("dbPassword");
                    string service = dIndex.GetDbValue("dbService");
                    string version = dIndex.GetDbValue("dbVersion");

                    Exception eError = null;
                    if (dbType.Equals("SDE"))
                    {
                        wsType = enumWSType.SDE;
                    }
                    else if (dbType.Equals("PDB"))
                    {
                        wsType = enumWSType.PDB;
                    }
                    else if (dbType.Equals("GDB"))
                    {
                        wsType = enumWSType.GDB;
                    }
                    IWorkspace pWorkspace = GetWorkspace(mypath, service, dataBase, user, password, version, wsType, out eError);
                }


                //索引树跳转到地图文档窗口
                   // m_Hook.IndextabControl.SelectedTab = m_Hook.IndextabControl.Tabs["PageIndex"];
            }

        }

        public void InitDataUnitTree()
        {
            //从 数据单元表 中获取信息
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strDispLevel = dIndex.GetXmlElementValue("UnitTree", "tIsDisp");//是否从市级开始创建数据单元树
            string constr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  //生成连接数据库字符串
            OleDbConnection mycon = new OleDbConnection(constr);   //定义OleDbConnection对象实例并连接数据库
            string strExp = "";
            strExp = "select * from " + "数据单元表";
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            try
            {
                mycon.Open();

                //创建datareader   对象来连接到表单     
                OleDbDataReader aReader = aCommand.ExecuteReader();
                m_Hook.DataUnitTree.Nodes.Clear();
                TreeNode tparent;
                tparent = new TreeNode();
                tparent.Text = "数据单元";
                tparent.Tag = 0;

                TreeNode tRoot;
                tRoot = new TreeNode();
                tRoot = tparent;
                TreeNode tNewNode;
                TreeNode tNewNodeClild;
                TreeNode tNewLeafNode;
                m_Hook.DataUnitTree.Nodes.Add(tparent);
                while (aReader.Read())
                {
                    //如果是行政区级别是1就是根节点
                    //此处默认都已经排序合理，针对排序的维护在数据字典维护界面中实现
                    if (aReader["数据单元级别"].ToString().Equals("1")) //省级节点
                    {
                        if (strDispLevel.Equals("0"))
                        {
                            tNewNode = new TreeNode();
                            tNewNode.Text = aReader["行政名称"].ToString();
                            tNewNode.Name = aReader["行政代码"].ToString();
                            tparent.Nodes.Add(tNewNode);
                            tparent.Expand();
                            tparent = tNewNode;
                            tNewNode.Tag = 1;
                            tNewNode.ImageIndex = 17;
                        }
                 
                    }
                    else if (aReader["数据单元级别"].ToString().Equals("2")) //市级节点
                    {
                        tNewNodeClild = new TreeNode();
                        tNewNodeClild.Text = aReader["行政名称"].ToString();
                        tNewNodeClild.Name = aReader["行政代码"].ToString();
                        tparent.Nodes.Add(tNewNodeClild);
                        tparent.Expand();
                        tRoot = tNewNodeClild;
                        tNewNodeClild.Tag = 2;
                        tNewNodeClild.ImageIndex =17;

                    }
                    else if (aReader["数据单元级别"].ToString().Equals("3"))//县级节点
                    {
                        tNewLeafNode = new TreeNode();
                        tNewLeafNode.Text = aReader["行政名称"].ToString();
                        tNewLeafNode.Name = aReader["行政代码"].ToString();
                        tRoot.Nodes.Add(tNewLeafNode);
                        tRoot.Expand();
                        tNewLeafNode.Tag = 3;
                        tNewLeafNode.ImageIndex = 17;
                        
                        string strSql = "";
                        string strScaleName = "";
                        string strSubTypeName = "";
                        string strLeafName = "";
                        string strYear = "";
                        string strSubType = "";
                        string strXzqName = "";
                        string strScale = "";

                        //插入专题节点
                        CreateSubTreeItem(tNewLeafNode, constr);

                        //插入叶子节点
                        TreeNode tLeafItem;
                        TreeNode tFindParentItem;
                        GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();
                        strExp = "select * from 地图入库信息表 where 行政代码 = '" + tNewLeafNode.Name + "'";
                        OleDbCommand aCommand2 = new OleDbCommand(strExp, mycon);
                        OleDbDataReader aReader2 = aCommand2.ExecuteReader();
                        while (aReader2.Read())
                        {
                            strYear = aReader2["年度"].ToString();
                            strSubType = aReader2["专题类型"].ToString();
                            strXzqName = aReader2["行政名称"].ToString();
                            strScale = aReader2["比例尺"].ToString();

                            //获取比例尺
                            strSql = "select 描述 from 比例尺代码表 where 代码 ='" + strScale + "'";
                            strScaleName = dDbFun.GetInfoFromMdbByExp(constr, strSql);

                            //获取专题类型
                            strSql = "select 描述 from 标准专题信息表 where 专题类型 ='" + strSubType + "'";
                            strSubTypeName = dDbFun.GetInfoFromMdbByExp(constr, strSql);


                            //组织树节点 
                            strLeafName = strYear + "年【" + strScaleName + "】";

                            //获取上级节点
                            tFindParentItem = FindNode(tNewLeafNode, strSubTypeName);

                            tFindParentItem.ImageIndex = 9;
                            tFindParentItem.SelectedImageIndex = 10;
                            //此次默认 地图入库信息表中所有记录已经按照 数据单元、专题类型排序
                            tLeafItem = new TreeNode();
                            tLeafItem.Text = strLeafName;
                            tLeafItem.Tag = strSubType;  //临时把专题类型记录在tag中
                            tFindParentItem.Nodes.Add(tLeafItem);
                            //tFindParentItem.ExpandAll();
                            tLeafItem.ImageIndex = 7;
                            tLeafItem.SelectedImageIndex = 8;
                        }
                        aReader2.Close();
                    }
                    else
                    {
                        tNewNodeClild = new TreeNode();
                        tNewNodeClild.Text = aReader["行政名称"].ToString();
                        tNewNodeClild.Name = aReader["行政代码"].ToString();
                        tparent.Nodes.Add(tNewNodeClild);
                        tparent.ExpandAll();
                        tNewNodeClild.Tag = 1;
                    }
                }

                //关闭reader对象     
                aReader.Close();
                

                //关闭连接,这很重要     
                mycon.Close();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //根据节点名称获取节点
        private TreeNode FindNode(TreeNode tnParent, string strValue)
        {
            if (tnParent == null)
                return null;
            if (tnParent.Text == strValue)
                return tnParent;
            TreeNode tnRet = null;
            foreach (TreeNode tn in tnParent.Nodes)
            {
                tnRet = FindNode(tn, strValue);
                if (tnRet != null)
                    break;
            }
            return tnRet;
        } 

        /// <summary>
        /// 创建专题节点
        /// </summary>
        /// <param name="tRootItem">县级行政区节点</param>
        /// <param name="strCon">连接语句</param>
        public void CreateSubTreeItem(TreeNode tRootItem, string strCon)
        {
            string strXzqCode = tRootItem.Name;
            GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();
            string strExp = "select distinct 专题类型 from 地图入库信息表 where 行政代码 = '" + strXzqCode + "'";
            OleDbConnection mycon = new OleDbConnection(strCon);   //定义OleDbConnection对象实例并连接数据库
            OleDbCommand aCommand = new OleDbCommand(strExp, mycon);
            try
            {
                mycon.Open();
                OleDbDataReader aReader = aCommand.ExecuteReader();
                string strSql = "";
                string strSubType = "";
                string strSubTypeName = "";
                TreeNode tSubItem;

                while (aReader.Read())
                {
                    strSubType = aReader["专题类型"].ToString();

                    //获取专题类型
                    strSql = "select 描述 from 标准专题信息表 where 专题类型 ='" + strSubType + "'";
                    strSubTypeName = dDbFun.GetInfoFromMdbByExp(strCon, strSql);


                    tSubItem = new TreeNode();
                    tSubItem.Text = strSubTypeName;
                    tRootItem.Nodes.Add(tSubItem);
                    //tRootItem.ExpandAll();
                }

                //关闭reader对象     
                aReader.Close();

                //关闭连接     
                mycon.Close();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 得到工作空间
        /// </summary>
        /// <param name="eError"></param>
        /// <returns></returns>
        public IWorkspace GetWorkspace(string server, string service, string dataBase, string user, string password, string version, enumWSType wsType, out Exception eError)
        {
            eError = null;
            bool result = false;

            if (m_Hook.gisDataSet == null)
            {
                m_Hook.gisDataSet = new SysGisDataSet();
            }
            try
            {
                switch (wsType)
                {
                    case enumWSType.SDE:
                        result = m_Hook.gisDataSet.SetWorkspace(server, service, dataBase, user, password, version, out eError);
                        break;
                    case enumWSType.PDB:
                    case enumWSType.GDB:
                        result = m_Hook.gisDataSet.SetWorkspace(server, wsType, out eError);
                        break;
                    default:
                        break;
                }
                if (result)
                {
                    return m_Hook.gisDataSet.WorkSpace;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                eError = ex;
                return null;
            }
        }


        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null)
                return;
            m_Hook = hook as Plugin.Application.IAppGisUpdateRef;
            m_frmhook = hook as Plugin.Application.IAppFormRef;
        }
    }
}
