using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GeoDataManagerFrame
{
    /// <summary>
    /// 作者：yjl
    /// 日期：2011.05.24
    /// 说明：创建地图书签窗体
    /// </summary> 
    [Serializable]
    public partial class FrmCreateMBMark : DevComponents.DotNetBar.Office2007Form
    {
        private string m_bookmark;
        private bool m_check;
        private IList<string> bookmarks;
        private Dictionary<string,double[]> dicMapMark;
        private IMap pMap;
        //IMapDocument mapDoc = new MapDocumentClass();
        private bool _Writelog = true;  //added by chulili 2012-09-07 是否写日志
        public bool WriteLog
        {
            get
            {
                return _Writelog;
            }
            set
            {
                _Writelog = value;
            }
        }
        public FrmCreateMBMark(IList<string> inbookmarks)
        {
            InitializeComponent();
            m_check = false;
            bookmarks = inbookmarks;
        }
        public FrmCreateMBMark(IMap inMap)
        {
            InitializeComponent();
            m_check = false;
            pMap = inMap;
            dicMapMark = new Dictionary<string, double[]>();
            //string fpath = Application.StartupPath + "\\..\\OutputResults\\curMapBookMark.mxd";
            //if (!File.Exists(fpath))
            //    mapDoc.New(fpath);
            //mapDoc.Open(fpath, "");
            //if ((pMap as IMapBookmarks).Bookmarks.Next() == null)
            //{
                InitMapBookMark();
            //}
        }
        //反序列化书签对象添加到现在的地图上
        private void InitMapBookMark()
        {
            Stream s = File.Open(Application.StartupPath + "\\..\\Bin\\curMapBookMark.dat", FileMode.OpenOrCreate);
            try
            {
               
                BinaryFormatter bf = new BinaryFormatter();
                object o = bf.Deserialize(s);
                dicMapMark = o as Dictionary<string, double[]>;
        //        IEnumSpatialBookmark enumSBM =(mapDoc.get_Map(0) as IMapBookmarks).Bookmarks;
        //        ISpatialBookmark SBM = enumSBM.Next();
        //        while (SBM != null)
        //        {
        //            (pMap as IMapBookmarks).AddBookmark(SBM);
        //            SBM = enumSBM.Next();
        //        }
                s.Close();
            }
            catch
            {
                s.Close();
            }
        }
        private void FrmCreateMBMark_Load(object sender, EventArgs e)
        {
            btnXOK.Enabled = false;
            txtMBMName.Text = "新书签";
        }
        public string Bookmark
        {
            get { return m_bookmark; }
        }
        public bool Check
        {
            get { return m_check; }
        }
        private void txtMBMName_TextChanged(object sender, EventArgs e)
        {
            //Set Enabled property
            if (txtMBMName.Text == "")
                btnXOK.Enabled = false;
            else
                btnXOK.Enabled = true;
        }

        private void btnXOK_Click(object sender, EventArgs e)
        {
            if (pMap != null)
            {
                if (isExsitByName(txtMBMName.Text))
                    if (MessageBox.Show("书签名称已存在，要替换吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                    {
                        if (this.WriteLog)
                        {
                            Plugin.LogTable.Writelog("不替换已存在的书签，已取消创建");//xisheng 2011.07.09 增加日志
                        }
                        return;
                    }
                    else
                    {
                        if (this.WriteLog)
                        {
                            Plugin.LogTable.Writelog("替换书签" + txtMBMName.Text);//xisheng 2011.07.09 增加日志
                        }
                        removeByName(txtMBMName.Text);
                    }
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("创建书签(" + txtMBMName.Text + ")");//xisheng 2011.07.09 增加日志
                }
                addBookMark(txtMBMName.Text, pMap);
            }
            //else
            //{
            //    if ((bookmarks as IList<string>).Contains(txtMBMName.Text))
            //        if (MessageBox.Show("书签名称已存在，要替换吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
            //            return;
            //    m_bookmark = txtMBMName.Text;
            //    m_check = true;
            //    txtMBMName.Text = "";
            //}
            //IMapDocument mapDoc = new MapDocumentClass();
            //mapDoc.New(Application.StartupPath + "\\..\\OutputResults\\curMapBookMark.mxd");
            //mapDoc.ReplaceContents(pMap as IMxdContents);
            //mapDoc.Save(true, false);
            this.Close();
        }
        //检查是否存在名字是bmName的书签
        public bool isExsitByName(string bmName)
        {
            ////Get the bookmarks of the focus map
            //IMapBookmarks mapBookmarks = (IMapBookmarks)pMap;

            ////Get bookmarks enumerator
            //IEnumSpatialBookmark enumSpatialBookmarks = mapBookmarks.Bookmarks;
            //enumSpatialBookmarks.Reset();

            ////Loop through the bookmarks to get bookmark names
            //ISpatialBookmark spatialBookmark = enumSpatialBookmarks.Next();

            //int bookmarkCount = 0;
            //while (spatialBookmark != null)
            //{
            //    //Get the correct bookmark
            //    if (spatialBookmark.Name == bmName)
            //    {
            //        return true;
            //        break;
            //    }
            //    bookmarkCount = bookmarkCount + 1;
            //    spatialBookmark = enumSpatialBookmarks.Next();
            //}
            if (dicMapMark.ContainsKey(bmName))
                return true;

            return false;
        }
        //删除名字是bmName的书签
        public void removeByName(string bmName)
        {
            ////Get the bookmarks of the focus map
            //IMapBookmarks mapBookmarks = (IMapBookmarks)pMap;

            ////Get bookmarks enumerator
            //IEnumSpatialBookmark enumSpatialBookmarks = mapBookmarks.Bookmarks;
            //enumSpatialBookmarks.Reset();

            ////Loop through the bookmarks to get bookmark names
            //ISpatialBookmark spatialBookmark = enumSpatialBookmarks.Next();

            //int bookmarkCount = 0;
            //while (spatialBookmark != null)
            //{
            //    //Get the correct bookmark
            //    if (spatialBookmark.Name == bmName)
            //    {
            //        mapBookmarks.RemoveBookmark(spatialBookmark);
            //        break;
            //    }
            //    bookmarkCount = bookmarkCount + 1;
            //    spatialBookmark = enumSpatialBookmarks.Next();
            //}

            dicMapMark.Remove(bmName);

           
        }

        private void addBookMark(string bmName,IMap inMap)
        {
            IActiveView activeView = (IActiveView)inMap;
            //IAOIBookmark bookmark = new AOIBookmarkClass();
            ////Set the location to the current extent of the focus map
            //bookmark.Location = activeView.Extent;
            ////Set the bookmark name
            //bookmark.Name = bmName;
            ////Get the bookmark collection of the focus map
            //IMapBookmarks mapBookmarks = (IMapBookmarks)inMap;
            ////Add the bookmark to the bookmarks collection
            //mapBookmarks.AddBookmark(bookmark);
            double[] pExtent = new double[4];
            pExtent[0] = activeView.Extent.Envelope.XMin;
            pExtent[1] = activeView.Extent.Envelope.YMin;
            pExtent[2] = activeView.Extent.Envelope.XMax;
            pExtent[3] = activeView.Extent.Envelope.YMax;
            dicMapMark.Add(bmName, pExtent);


 
        }
        private void btnXCancel_Click(object sender, EventArgs e)
        {
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("取消创建地图书签");//xisheng 2011.07.09 增加日志
            }
            this.Close();
        }

        private void FrmCreateMBMark_FormClosed(object sender, FormClosedEventArgs e)
        {
            Stream s = File.Open(Application.StartupPath + "\\..\\bin\\curMapBookMark.dat", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, dicMapMark);
            s.Close();
            //mapDoc.Close();
        }
    }
}
