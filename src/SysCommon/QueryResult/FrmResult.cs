using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace SysCommon
{
    public partial class FrmResult :BaseForm
    {
        private string m_Title = "";
        private string m_XLabels = "";
        private string m_YLabels = "";
        private ArrayList m_XLable = new ArrayList();
        private ArrayList m_ColorGuider = new ArrayList();
        private ArrayList[] m_CharData = { new ArrayList() };
        public FrmResult()
        {
            InitializeComponent();
            //初始化下拉框
            object[] strType = new object[] { "柱状图", "饼状图" };
            cbStyle.Properties.Items.AddRange(strType);
            cbStyle.SelectedIndex = 0;
        }
        private List<Dictionary<ArrayList, ArrayList[]>> m_Lst = new List<Dictionary<ArrayList, ArrayList[]>>();
        private int m_iPage = 1;
        /// <summary>
        /// 设置统计图表的标题
        /// </summary>
        public string strTitle
        {
            set
            {
                m_Title = value;
            }
        }
        /// <summary>
        /// 设置统计图表X轴的名称
        /// </summary>
        public string strXLabels
        {
            set
            {
                m_XLabels = value;
            }
        }
        /// <summary>
        /// 设置统计图表Y轴名称
        /// </summary>
        public string strYLabels
        {
            set
            {
                m_YLabels = value;
            }
        }
        /// <summary>
        /// 设置统计图表X轴坐标标识数组
        /// </summary>
        public ArrayList ArrXLable
        {
            set
            {
                m_XLable = value;
            }
        }
        /// <summary>
        /// 设置统计图表图例说明
        /// </summary>
        public ArrayList ArrColorGuider
        {
            set
            {
                m_ColorGuider = value;
            }
        }
        /// <summary>
        /// 设置统计图表的数据源
        /// </summary>
        public ArrayList[] ArrCharData
        {
            set
            {
                m_CharData = value;
            }
        }

        private void cbStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cbStyle.Text == "") { return; }
            //switch (cbStyle.Text)
            //{
            //    case "柱状图":
            //        chartlet1.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Bar_2D_Aurora_FlatCrystal_Glow_NoBorder;
            //        break;
            //    case "线状图":
            //        chartlet1.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Line_3D_Aurora_FlatCrystalSquare_NoGlow_NoBorder;
            //        break;
            //    case "饼状图":
            //        chartlet1.AppearanceStyle = FanG.Chartlet.AppearanceStyles.Pie_3D_Aurora_FlatCrystal_NoGlow_NoBorder;
            //        break;
            //    default :
            //        break;

            //}
            ////每次希望修改属性后立即重绘图形就需要调用 Refresh()方法
            //chartlet1.Refresh();
        }

        private void btnExportChart_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    SaveFileDialog pSaveFileDialog = new SaveFileDialog();
            //    pSaveFileDialog.Filter = "JPEG|*.jpg|BMP|*.bmp";
            //    if (pSaveFileDialog.ShowDialog() != DialogResult.OK)
            //    {
            //        return;
            //    }
            //    string strPath = pSaveFileDialog.FileName;
            //    if (System.IO.File.Exists(strPath))
            //    {
            //        try
            //        {
            //            System.IO.File.Delete(strPath);
            //        }
            //        catch { MessageBox.Show("图片导出失败！", "提示！"); return; }

            //    }
            //    Image myImage = new Bitmap(chartlet1.Width , chartlet1.Height);
            //    //从一个继承自Image类的对象中创建Graphics对象
            //   // Graphics g = Graphics.FromImage(myImage);
            //    //抓屏并拷贝到myimage里
            //   // g.CopyFromScreen(this.Location, chartlet1.Location, new Size(chartlet1.Width + 20, chartlet1.Height + 45));
            //    Bitmap pBitmap = new Bitmap(myImage);
            //    //Bitmap vBitmap;
            //    ///将抓屏获得图片在进行截取 由于通过抓屏无法获得最小的统计图
            //     Rectangle cloneRect = new Rectangle(chartlet1.Location ,chartlet1.Size );
            //    //vBitmap = pBitmap.Clone(cloneRect, pBitmap.PixelFormat);
            //    //保存为文件
            //     chartlet1.DrawToBitmap(pBitmap, cloneRect);//更新图片导出功能 ygc 2012-9-3
            //     pBitmap.Save(strPath);
            //    //myImage.Save(strPath);
            //     pBitmap.Dispose();
            //    //g.Dispose();
            //    myImage.Dispose();
            //    //chartlet.BackgroundImage.Save(pSaveFileDialog.FileName);
            //    MessageBox.Show("图片导出成功！", "提示！");
            //}
            //catch
            //{
            //    MessageBox.Show("图片导出失败！", "提示！");
            //}
           
        }

        private void btnExportExecl_Click(object sender, EventArgs e)
        {
            //Microsoft.Office.Interop.Excel.Application excel = null;
            //Microsoft.Office.Interop.Excel.Workbook wb = null;
            //try
            //{
            //    ///统计长度或面积总和
            //    double DSum = 0;
            //    //建立Excel对象
            //    excel = new Microsoft.Office.Interop.Excel.Application();
            //    wb = excel.Application.Workbooks.Add(true);
            //    excel.Visible = true;
            //    wb.Application.ActiveWindow.Caption = m_Title + " 统计表";
            //    excel.Cells[1, 1] = m_XLabels;
            //    excel.Cells[1, 2] = m_Title.Substring(0, m_Title.Length - 3);
            //    ///遍历获得X轴和对应的值
            //    for (int i = 0; i < m_XLable.Count; i++)
            //    {
            //        excel.Cells[i + 2, 1] = m_XLable[i];
            //        excel.Cells[i + 2, 2] = m_CharData[0][i];
            //        DSum = DSum + Convert.ToDouble(m_CharData[0][i].ToString());
            //    }
            //    excel.Cells[m_XLable.Count + 2, 1] = "合计为：";
            //    excel.Cells[m_XLable.Count + 2, 2] = DSum.ToString();
            //    ///弹出对话保存生成统计表的路径
            //    Microsoft.Office.Core.FileDialog fd = wb.Application.get_FileDialog(Microsoft.Office.Core.MsoFileDialogType.msoFileDialogSaveAs);
            //    fd.InitialFileName = m_Title + " 统计表";
            //    int result = fd.Show();
            //    if (result == 0) return;
            //    string fileName = fd.InitialFileName;
            //    if (!string.IsNullOrEmpty(fileName))
            //    {
            //        if (fileName.IndexOf(".xls") == -1)
            //        {
            //            fileName += ".xls";
            //        }
            //        ///保存生成的统计表
            //        wb.SaveAs(fileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //    }

            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
            //    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
            //    GC.Collect();
            //}
            //catch
            //{

            //}
        }

        private void FrmResult_Load(object sender, EventArgs e)
        {
            ///当X轴标识数组过大时对其以40为基准进行分页处理
            ///用于存储X轴的标识数组
            ArrayList ArrXLable = new ArrayList();
            ///存储统计数据的项
            ArrayList[] ArrCharData = { new ArrayList() };
            ///存储X轴和统计数据项的数组
            Dictionary<ArrayList, ArrayList[]> pDic = new Dictionary<ArrayList, ArrayList[]>();
            int iCount = (int)m_XLable.Count / 40;
            int iCutCount = 0;
            int iIndex = 0;
            for (int i = 1; i <= iCount; i++)
            {
                iCutCount = 40;
                ///截取开始的索引号
                iIndex = (i - 1) * 40;
                ArrayList[] pArrCharData = { new ArrayList() };
                ArrXLable = m_XLable.GetRange(iIndex, iCutCount);
                pArrCharData[0] = m_CharData[0].GetRange(iIndex, iCutCount);
                pDic.Add(ArrXLable, pArrCharData);
                ///将获取第i页的数据存入m_Lst
                m_Lst.Add(pDic);
                ArrXLable = new ArrayList();
                pDic = new Dictionary<ArrayList, ArrayList[]>();

            }
            ///处理最后不到40的项
            if ((iCutCount = m_XLable.Count - 40 * iCount) > 0)
            {
                iIndex = iCount * 40;
                ArrXLable = m_XLable.GetRange(iIndex, iCutCount);
                ArrCharData[0] = m_CharData[0].GetRange(iIndex, iCutCount);
                pDic.Add(ArrXLable, ArrCharData);
                m_Lst.Add(pDic);
            }
            //////
            //chartlet1.ChartTitle.Text = m_Title;
            //chartlet1.XLabels.UnitText = m_XLabels;
            //chartlet1.YLabels.UnitText = m_YLabels;
            //chartlet.Crystal.Enable = true;
            //chartlet.Crystal.CoverFull = true;
            //chartlet.Crystal.Contraction = 4;
            //默认显示第一页
            pDic = m_Lst[0];
            foreach (ArrayList vArrXLable in pDic.Keys)
            {
                //chartlet1.InitializeData(pDic[vArrXLable], vArrXLable, m_ColorGuider);
            }
            ///判断下页按钮是否可用
            if (m_Lst.Count > 1 && m_iPage != m_Lst.Count)
            {
                btnNextPage.Enabled = true;
            }
            txtPageCount.Text = m_iPage + "/" + m_Lst.Count;
            //chartlet.MaxValueY = chartlet.MaxValueY + chartlet.MaxValueY / 10;
            //chartlet1.Refresh();
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            ///点击下页当前页面就加一
            if (m_Lst.Count == 1)
            {
                return;
            }
            m_iPage = m_iPage + 1;
            txtPageCount.Text = m_iPage + "/" + m_Lst.Count;
            Dictionary<ArrayList, ArrayList[]> pDic = new Dictionary<ArrayList, ArrayList[]>();
            ///由于当前页数是从一开始而索引从零开始的
            pDic = m_Lst[m_iPage - 1];
            foreach (ArrayList vArrXLable in pDic.Keys)
            {
                ///重新加载统计页面
                //chartlet1.InitializeData(pDic[vArrXLable], vArrXLable, m_ColorGuider);
                
            }
            //chartlet1.Refresh();
            btnLastPage.Enabled = true;
            if (m_iPage == m_Lst.Count)
            {
                btnNextPage.Enabled = false;
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            ///点击下页当前页面就减一
            if (m_Lst.Count == 1)
            {
                return;
            }
            m_iPage = m_iPage - 1;
            txtPageCount.Text = m_iPage + "/" + m_Lst.Count;
            Dictionary<ArrayList, ArrayList[]> pDic = new Dictionary<ArrayList, ArrayList[]>();
            ///由于当前页数是从一开始而索引从零开始的
            pDic = m_Lst[m_iPage - 1];
            foreach (ArrayList vArrXLable in pDic.Keys)
            {
                //chartlet1.InitializeData(pDic[vArrXLable], vArrXLable, m_ColorGuider);
            }
            //chartlet1.Refresh();
            if (m_iPage == 1)
            {
               btnLastPage.Enabled = false;
            }
            if (m_Lst.Count > 1 && m_iPage != m_Lst.Count)
            {
                btnNextPage.Enabled = true;
            }
        }
    }
}
