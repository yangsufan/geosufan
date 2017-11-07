using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Carto;
using System.IO;
using ESRI.ArcGIS.Geometry;
using System.Runtime.Serialization.Formatters.Binary;


namespace GeoDataManagerFrame
{
    /// <summary>
    /// 作者：yjl
    /// 日期：2011.05.24
    /// 说明：地图书签管理窗体
    /// </summary>
    public partial class FrmMapBookMark : DevComponents.DotNetBar.Office2007Form
    {
        private IMap pMap;
        private IList<string> bkmkName;
        //IMapDocument mapDoc = new MapDocumentClass();
        private Dictionary<string, double[]> dicMapMark;
        private static  bool _Writelog = true;  //added by chulili 2012-09-07 是否写日志
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
        public FrmMapBookMark(IMap inIMap,bool write)
        {
            InitializeComponent();
            pMap = inIMap;
            dicMapMark = new Dictionary<string, double[]>();
            //string fpath = Application.StartupPath + "\\..\\OutputResults\\curMapBookMark.mxd";
            //if (!File.Exists(fpath))
            //    mapDoc.New(fpath);
            //mapDoc.Open(fpath, "");
            //if ((pMap as IMapBookmarks).Bookmarks.Next() == null)
            //{ 
                InitMapBookMark(); 
            //}
            //bkmkName = new List<string>();
            FillList();
            WriteLog = write;
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("打开地图书签管理");
            }
            if (listViewEx1.Items.Count == 0)//xisheng 20110628 一项没有，全删的按钮灰掉
            {
                btnXDeleteAll.Enabled = false;
            }
            else
            {
                btnXDeleteAll.Enabled = true;
            }
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
        private void btnXCreate_Click(object sender, EventArgs e)
        {
            //Get a name for bookmark from the user
            FrmCreateMBMark frm = new FrmCreateMBMark(pMap);
            frm.WriteLog = WriteLog; //ygc 2012-9-12 新增是否写日志
            frm.ShowDialog(this);
            InitMapBookMark();
            FillList();
            if (listViewEx1.Items.Count == 0)//xisheng 20110628 一项没有，全删的按钮灰掉
            {
                btnXDeleteAll.Enabled = false;
            }
            else
            {
                btnXDeleteAll.Enabled = true;
            }
            
           
        }
        //删除名字是bmName的书签
        public void removeByName(string  bmName)
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

            //return "";
            dicMapMark.Remove(bmName);
            Stream s = File.Open(Application.StartupPath + "\\..\\bin\\curMapBookMark.dat", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, dicMapMark);
            s.Close();//yjl0616
        }
        //根据书签名字返回书签 和 索引
        public void findByName(string  bmName,ref ISpatialBookmark  outBM,ref int outIndex)
        {
            //Get the bookmarks of the focus map
            IMapBookmarks mapBookmarks = (IMapBookmarks)pMap;

            //Get bookmarks enumerator
            IEnumSpatialBookmark enumSpatialBookmarks = mapBookmarks.Bookmarks;
            enumSpatialBookmarks.Reset();

            //Loop through the bookmarks to get bookmark names
            ISpatialBookmark spatialBookmark = enumSpatialBookmarks.Next();

            int bookmarkCount = 0;
            while (spatialBookmark != null)
            {
                //Get the correct bookmark
                if (spatialBookmark.Name == bmName)
                {
                    outBM = spatialBookmark;
                    outIndex = bookmarkCount;
                    break;
                }
                bookmarkCount = bookmarkCount + 1;
                spatialBookmark = enumSpatialBookmarks.Next();
            }

           
        }


        //读取书签填充listview
        public void FillList()
        {
            ////Get the bookmarks of the focus map
            //IMapBookmarks mapBookmarks = (IMapBookmarks)pMap;

            ////Get bookmarks enumerator
            //IEnumSpatialBookmark enumSpatialBookmarks = mapBookmarks.Bookmarks;
            //enumSpatialBookmarks.Reset();

            ////Loop through the bookmarks to get bookmark names
            //ISpatialBookmark spatialBookmark = enumSpatialBookmarks.Next();
            ////bkmkName.Clear();
            //listViewEx1.Items.Clear();
            //while (spatialBookmark != null)
            //{
            //    listViewEx1.Items.Add(spatialBookmark.Name);
            //    //bkmkName.Add(spatialBookmark.Name);
            //    spatialBookmark = enumSpatialBookmarks.Next();
            //}
            listViewEx1.Items.Clear();
            foreach(KeyValuePair<string,double[]> kvp in dicMapMark)
            {
                listViewEx1.Items.Add(kvp.Key);
 
            }

                if (listViewEx1.Items.Count > 0)
                {
                    listViewEx1.Items[0].Selected = true;
                    listViewEx1.Focus();
                }
           
        }

        private void btnXZoomTo_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewEx1.SelectedItems == null)
                    return;
                ////Get the bookmarks of the focus map
                //IMapBookmarks mapBookmarks = (IMapBookmarks)pMap;

                ////Get bookmarks enumerator
                //IEnumSpatialBookmark enumSpatialBookmarks = mapBookmarks.Bookmarks;
                //enumSpatialBookmarks.Reset();

                ////Loop through the bookmarks to get bookmark names
                //ISpatialBookmark spatialBookmark = enumSpatialBookmarks.Next();


                //while (spatialBookmark != null)
                //{
                //    if (spatialBookmark.Name == listViewEx1.SelectedItems[0].Text)
                //    {
                //        spatialBookmark.ZoomTo(pMap);
                //        (pMap as IActiveView).Refresh();
                //        break;
                //    }
                //    spatialBookmark = enumSpatialBookmarks.Next();
                //}
                double[] pExtent = new double[4];
                dicMapMark.TryGetValue(listViewEx1.SelectedItems[0].Text, out pExtent);
                IEnvelope pEnv = new EnvelopeClass();
                pEnv.PutCoords(pExtent[0], pExtent[1], pExtent[2], pExtent[3]);
                (pMap as IActiveView).Extent = pEnv;
                (pMap as IActiveView).Refresh();
                listViewEx1.Focus();
            }
            catch
            {
 
            }
        }

        private void btnXUp_Click(object sender, EventArgs e)
        {
            listViewEx1.BeginUpdate();
            foreach (ListViewItem lvi in listViewEx1.SelectedItems)
            {
                int lviindex = lvi.Index;
                listViewEx1.Items.Remove(lvi);
                listViewEx1.Items.Insert(lviindex - 1, lvi);
                //ISpatialBookmark spaBM=null;
                //int bmIndex = 0;
                //findByName(lvi.Text,ref spaBM, ref bmIndex);
                //(pMap as IMapBookmarks2).MoveBookmarkTo(spaBM,bmIndex-1);
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();

        }

        private void btnXDown_Click(object sender, EventArgs e)
        {
            listViewEx1.BeginUpdate();
            foreach (ListViewItem lvi in listViewEx1.SelectedItems)
            {
                int lviindex = lvi.Index;
                listViewEx1.Items.Remove(lvi);
                listViewEx1.Items.Insert(lviindex + 1, lvi);
                //ISpatialBookmark spaBM=null;
                //int bmIndex = 0;
                //    findByName(lvi.Text,ref spaBM, ref bmIndex);
                //(pMap as IMapBookmarks2).MoveBookmarkTo(spaBM, bmIndex + 1);
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();
        }

        private void btnXTop_Click(object sender, EventArgs e)
        {
            listViewEx1.BeginUpdate();
            int i = 0;
            foreach (ListViewItem lvi in listViewEx1.SelectedItems)
            {
                int lviindex = lvi.Index;
                listViewEx1.Items.Remove(lvi);
                listViewEx1.Items.Insert(i, lvi);
                //ISpatialBookmark spaBM=null;
                //int bmIndex = 0;
                //    findByName(lvi.Text,ref spaBM, ref bmIndex);
                //(pMap as IMapBookmarks2).MoveBookmarkTo(spaBM, i);
                i++;
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();
        }

        private void btnXBottom_Click(object sender, EventArgs e)
        {
            listViewEx1.BeginUpdate();
            int i = listViewEx1.Items.Count - listViewEx1.SelectedItems.Count ;
            foreach (ListViewItem lvi in listViewEx1.SelectedItems)
            {
                int lviindex = lvi.Index;
                listViewEx1.Items.Remove(lvi);
                listViewEx1.Items.Insert(i, lvi);
                //ISpatialBookmark spaBM=null;
                //int bmIndex = 0;
                //    findByName(lvi.Text,ref spaBM, ref bmIndex);
                //(pMap as IMapBookmarks2).MoveBookmarkTo(spaBM, i);
                i++;
            }
            listViewEx1.EndUpdate();
            listViewEx1.Focus();
        }

        private void btnXDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除选择的书签吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                return;
            listViewEx1.BeginUpdate();
            foreach (ListViewItem lvi in listViewEx1.SelectedItems)
            {
               
                listViewEx1.Items.Remove(lvi);
                if (this.WriteLog)
                {
                    Plugin.LogTable.Writelog("删除地图书签(" + lvi.Text + ")");
                }
                removeByName(lvi.Text);
            }
            listViewEx1.EndUpdate();
           
            if (listViewEx1.Items.Count == 0)//xisheng 20110628 一项没有，全删的按钮灰掉
            {
                btnXDeleteAll.Enabled = false;
            }

        }

        private void FrmMapBookMark_Load(object sender, EventArgs e)
        {
           
        }

        private void btnXClose_Click(object sender, EventArgs e)
        {
            this.Close();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("关闭地图书签管理");
            }
        }

        private void btnXDeleteAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要全部删除吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                listViewEx1.Items.Clear();
                dicMapMark.Clear();
                Stream s = File.Open(Application.StartupPath + "\\..\\bin\\curMapBookMark.dat", FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(s, dicMapMark);
                s.Close();//yjl0616
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("删除全部地图书签");
            }
            if (listViewEx1.Items.Count == 0)//xisheng 20110628 一项没有，全删的按钮灰掉
            {
                btnXDeleteAll.Enabled = false;
                btnXDelete.Enabled = false;
                btnXZoomTo.Enabled = false;
            }
        }

        private void listViewEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnXZoomTo.Enabled = true;
            btnXUp.Enabled = true;
            btnXDown.Enabled = true;
            btnXTop.Enabled = true;
            btnXBottom.Enabled = true;
            btnXDelete.Enabled=true;
            if (listViewEx1.SelectedItems.Count == 0)
            {
                btnXZoomTo.Enabled = false;
                btnXDelete.Enabled = false;//added by xisheng 2011.06.28 未选中删除为灰色
                return;
            }
            
            foreach (ListViewItem lvi in listViewEx1.SelectedItems)
            {
               

                if (lvi.Index == 0)
                {
                    btnXUp.Enabled = false;
                    btnXTop.Enabled = false;
                }
               
                if (lvi.Index == listViewEx1.Items.Count - 1)
                {
                    btnXDown.Enabled = false;
                    btnXBottom.Enabled = false;
                }
            }
            
        }

        private void FrmMapBookMark_FormClosed(object sender, FormClosedEventArgs e)
        {
            //IMapDocument mapDoc = new MapDocumentClass();
            //mapDoc.New(Application.StartupPath + "\\..\\OutputResults\\curMapBookMark.mxd");
            //mapDoc.ReplaceContents(pMap as IMxdContents);
            //mapDoc.Save(true, false);
            //mapDoc.Close();
            Dictionary<string, double[]> tmpDic=new Dictionary<string,double[]>();
            for (int i = 0; i < listViewEx1.Items.Count; i++)
            {
                foreach (KeyValuePair<string, double[]> kvp in dicMapMark)
                {
                    if (listViewEx1.Items[i].Text == kvp.Key)
                        tmpDic.Add(kvp.Key, kvp.Value);

                }
            }
            dicMapMark = tmpDic;
            Stream s = File.Open(Application.StartupPath + "\\..\\bin\\curMapBookMark.dat", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(s, dicMapMark);
            s.Close();

        }

        private void btnXZoomTo_DoubleClick(object sender, EventArgs e)
        {
        }

        private void listViewEx1_Click(object sender, EventArgs e)
        {

        }

        private void listViewEx1_DoubleClick(object sender, EventArgs e)
        {

            if (listViewEx1.SelectedItems == null)
                return;
            ////Get the bookmarks of the focus map
            //IMapBookmarks mapBookmarks = (IMapBookmarks)pMap;

            ////Get bookmarks enumerator
            //IEnumSpatialBookmark enumSpatialBookmarks = mapBookmarks.Bookmarks;
            //enumSpatialBookmarks.Reset();

            ////Loop through the bookmarks to get bookmark names
            //ISpatialBookmark spatialBookmark = enumSpatialBookmarks.Next();


            //while (spatialBookmark != null)
            //{
            //    if (spatialBookmark.Name == listViewEx1.SelectedItems[0].Text)
            //    {
            //        spatialBookmark.ZoomTo(pMap);
            //        (pMap as IActiveView).Refresh();
            //        break;
            //    }
            //    spatialBookmark = enumSpatialBookmarks.Next();
            //}
            double[] pExtent = new double[4];
            dicMapMark.TryGetValue(listViewEx1.SelectedItems[0].Text, out pExtent);
            IEnvelope pEnv = new EnvelopeClass();
            pEnv.PutCoords(pExtent[0], pExtent[1], pExtent[2], pExtent[3]);
            (pMap as IActiveView).Extent = pEnv;
            (pMap as IActiveView).Refresh();
            listViewEx1.Focus();
            listViewEx1.Focus();
        }
	
	
    }
}
