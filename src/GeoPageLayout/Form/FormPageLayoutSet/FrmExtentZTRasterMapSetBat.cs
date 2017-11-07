using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;

namespace GeoPageLayout
{
    /// <summary>
    /// 作者：yjl
    /// 日期：20110908
    /// 说明：地图范围专题图出图设置界面--栅格
    /// </summary>
    public partial class FrmExtentZTRasterMapSetBat : DevComponents.DotNetBar.Office2007Form
    {
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
        public Dictionary<string, string> MapTextElements = null;
      
        //string fstH = "", fstL = "", scaleno = "", lstH = "", lstL = "";
        //加引线
        public bool HasBootLine
        {
            get { return cBoxHasBootLine.Checked; }
        }
        //全部字段
        public IList<string> LstFields
        {
            get;
            set;
        }
        //用户选择的字段
        public IList<string> LstResFields
        {
            get;
            set;
        }
        //用户选择的副标题字段
        public string ResSubHeadFields
        {
            get;
            set;
        }
        //展示系统地图
        public IMap  SourceMap
        {
            get;
            set;
        }
        //制图地图
        public IMap DesMap
        {
            get;
            set;
        }
       
        public FrmExtentZTRasterMapSetBat()
        {
            InitializeComponent();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("设置出图信息");//xisheng 2011.07.09 增加日志
            }
            List<string> tdlyZTs = ModGetData.GetZTFromXPath("//DIR[@DIRType='DOM']/Layer");
            foreach (string tdlyzt in tdlyZTs)
            {
                cBoxZT.Items.Add(tdlyzt);
            }
            if (cBoxZT.Items.Count > 0)
                cBoxZT.SelectedIndex = 0;

        }

      
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("取消范围项目专题图");//xisheng 2011.07.09 增加日志
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cBoxZT.Text == "")
                return;
             bool isSpecial = ModGetData.IsMapSpecial();

             if (isSpecial)//如果找特定专题
             {
                 DesMap = new MapClass();
                 ModGetData.AddMapOfNoneXZQ(DesMap, "DOM", cBoxZT.Text, SourceMap);
                 if (DesMap.LayerCount == 0)
                 {
                     SysCommon.Error.ErrorHandle.ShowFrmErrorHandle("提示", "未找到图层。");
                     this.DialogResult = DialogResult.Cancel;
                 }
                 ModuleMap.LayersComposeEx(DesMap);//图层排序
             }
             else
             {
                 IObjectCopy pOC = new ObjectCopyClass();
                 DesMap = pOC.Copy(SourceMap) as IMap;//复制地图
             }
            this.DialogResult = DialogResult.OK;
          
        }

        private void cBoxScale_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSetLabel_Click(object sender, EventArgs e)
        {
            //用户设置标注字段
            frmSetLabel fmSetLabel = new frmSetLabel(LstFields);
            if(fmSetLabel.ShowDialog(this)==DialogResult.OK)
               LstResFields = fmSetLabel.ResLst;
        }

  

 

    }
}
