using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GeoDataCenterFunLib;

namespace GeoDataManagerFrame
{
    public partial class frmMapProperty :  DevComponents.DotNetBar.Office2007Form
    {
        public frmMapProperty()
        {
            InitializeComponent();
        }
        TreeNode thisNode;
        private void frmMapProperty_Load(object sender, EventArgs e)
        {

            //显示图件名称
            labNewname.Text = thisNode.Parent.Parent.Text + thisNode.Parent.Text + thisNode.Text;

            //获取专题类型
            string strSubType = thisNode.Tag.ToString();//专题类型代码
            labNewType.Text = thisNode.Parent.Text;

            //生成连接数据库字符串
            GetDataTreeInitIndex dIndex = new GetDataTreeInitIndex();
            string mypath = dIndex.GetDbInfo();
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mypath + ";Mode=ReadWrite|Share Deny None;Persist Security Info=False";  
            //获取年度
            string strYear = thisNode.Text.Substring(0, 4);
            labNewYear.Text = strYear;

            //获取比例尺及其代码
            GeoDataCenterDbFun dDbFun = new GeoDataCenterDbFun();
            int iStartPos = thisNode.Text.IndexOf("【");
            int iEndPos = thisNode.Text.IndexOf("】");
            int iLength = iEndPos - iStartPos - 1;
            string strScaleName = thisNode.Text.Substring(iStartPos + 1, iLength);
            labNewScale.Text = strScaleName;
            
            string strExp = "select 代码 from 比例尺代码表 where 描述 ='" + strScaleName + "'";
            string strScaleCode = dDbFun.GetInfoFromMdbByExp(strCon, strExp);

            //获取行政区划
            labNewDivision.Text = thisNode.Parent.Parent.Text;

            //从地图入库信息表中获取已入库数据信息（图层组成）
            strExp = "select 图层组成 from 地图入库信息表 where 行政名称 ='" + thisNode.Parent.Parent.Text + "'" + "And " + " 年度='" +
              strYear + "'" + "And " + " 比例尺='" + strScaleCode + "'" + "And " + " 专题类型='" + strSubType + "'";
            string strLayerGroup = dDbFun.GetInfoFromMdbByExp(strCon, strExp);
            string[] array = strLayerGroup.Split("/".ToCharArray());
            for (int i = 0; i < array.Length; i++)
            {
                strExp = "select 描述 from 标准图层信息表 where 代码='" + array[i] + "'";
                string strBussinessName = dDbFun.GetInfoFromMdbByExp(strCon, strExp);
                listLayer.Items.Add(strBussinessName);
            }




        }

        public TreeNode ThisNode
        {
            get { return thisNode; }
            set { thisNode = value; }
        }
    }
}