//*********************************************************************************
//** 文件名：FormFlexcell.cs
//** CopyRight (c) 2000-2007 武汉吉奥信息工程技术有限公司工程部
//** 创建人：chulili
//** 日  期：2011-03
//** 修改人：
//** 日  期：
//** 描  述：用于图层符号化 
//**
//** 版  本：1.0
//*********************************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using System.Data.OleDb ;

namespace GeoDataCenterFunLib
{
    public partial class FormFlexcell : DevComponents.DotNetBar.Office2007Form
    {
        
        private string mstrFile;
        private string m_DefaultFile;
        private string[,] marrCellText=new string [100000,150];

        public FormFlexcell()
        {   
            InitializeComponent();
        }
        public AxFlexCell.AxGrid GetGrid()
        {
            return axGrid1;
        }
        //获取某单元格的内容
        private void axGrid1_GetCellText(object sender, AxFlexCell.__Grid_GetCellTextEvent e)
        {
            if ((e.row >= ModFlexcell.m_startRow && e.col >= ModFlexcell.m_startCol) || e.row == ModFlexcell.m_SpecialRow || e.row == ModFlexcell.m_SpecialRow_ex || e.row == ModFlexcell.m_SpecialRow_ex2)
            {
                e.text = marrCellText[e.row, e.col];
                e.changed = true; //使用虚表功能
            }
        }
        //设置某单元格的内容
        private void axGrid1_SetCellText(object sender, AxFlexCell.__Grid_SetCellTextEvent e)
        {
            if(e.row>0 && e.col>0)
            {
                marrCellText[e.row,e.col]=e.text;
                e.cancel=true;
            }

        }
        //清空单元格内容
        public void ClearAll(long rowstart,long colstart,long rowend,long colend)
        {
            long i,j;
            for(i=rowstart;i<=rowend;i++)
            {
                for (j=colstart;j<=colend;j++)
                {
                    marrCellText[i,j]="";
                }

            }

        }

        private void FormFlexcell_Load(object sender, EventArgs e)
        {
            mstrFile = "";
            m_DefaultFile = "";
            axGrid1.Height =this.Height-buttonXtoExcel.Height-60;
            axGrid1.Top=buttonXtoExcel.Top+buttonXtoExcel.Height+15;
            axGrid1.Left=buttonXSave.Left ;
            axGrid1.Width=this.Width-30;

        }
        //保存按钮
        private void buttonXSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "FlexCell Document (*.cel)|*.cel";
            dlg.FileName = m_DefaultFile;
            string oldpath;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                oldpath = dlg.FileName;
                if (System.IO.File.Exists(oldpath) == true)
                {
                    if (MessageBox.Show("文件'"+oldpath+"'已经存在，要覆盖它吗？","询问",MessageBoxButtons.YesNo)==DialogResult.Yes )
                    {
                        axGrid1.SaveFile(oldpath);
                    }
                }
                axGrid1.SaveFile(oldpath);
                MessageBox.Show("保存成功！");
            }
            else
            {
            }


        }
        public void SaveFile(string path)
        {
            try 
            { 
                axGrid1.SaveFile(path);
                m_DefaultFile = path;
            }
            catch
            {
            }
        }
        //另存为按钮
        private void buttonXSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "FlexCell Document (*.cel)|*.cel";
            string oldpath;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                oldpath = dlg.FileName;
                if (System.IO.File.Exists(oldpath) == true)
                {
                    if (MessageBox.Show("文件'" + oldpath + "'已经存在，要覆盖它吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        axGrid1.SaveFile(oldpath);
                    }
                }
                axGrid1.SaveFile(oldpath);
                MessageBox.Show("保存成功！");
            }
            else
            {
            }
        }
        //函数功能：转化成Excel格式
        private void buttonXtoExcel_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Excel WorkBook (*.xls)|*.xls";
            string oldpath;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                oldpath = dlg.FileName;
                if (System.IO.File.Exists(oldpath) == true)
                {
                    if (MessageBox.Show("文件'" + oldpath + "'已经存在，要覆盖它吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //直接调用flexcell表格的ExportToExcel功能转化成excel格式文件，输入参数是excel文件全路径
                        axGrid1.ExportToExcel(oldpath);
                    }
                }
                axGrid1.ExportToExcel(oldpath);
                MessageBox.Show("导出成功！");
            }
            else
            {
            }

        }
        //函数功能：根据Reader内容设置表格内容
        //输入参数：起始行，起始列，Reader，模板全路径，行数，字体，字号
        public  bool SetTextFromRS(int startrow,int startcol,OleDbDataReader dr,string TemplateFile,int recordcount,string sFontName,int iFontSize)
        {
            if (dr.FieldCount <= 0)
                return false;
            if (TemplateFile.Equals(""))
                return false;
            //先打开模板文件
            axGrid1.OpenFile(TemplateFile);
            int rowcount, colcount;
            rowcount = recordcount;
            colcount = dr.FieldCount;
            //设置表格行数和列数
            axGrid1.Rows = startrow + rowcount;
            axGrid1.Cols = startcol + colcount+1;
            //设置字体和字号
            axGrid1.Range(startrow, startcol, axGrid1.Rows - 1, axGrid1.Cols - 1).FontName =sFontName;
            axGrid1.Range(startrow, startcol, axGrid1.Rows - 1, axGrid1.Cols - 1).FontSize = iFontSize;

            m_DefaultFile = TemplateFile;
            
            axGrid1.Refresh();
            //关闭自动刷新
            axGrid1.AutoRedraw = false;
            
            int i, j;
            //根据Reader内容设置表格内容
            for (i = startrow ; i <= startrow + rowcount - 1; i++)
            {
                if (dr.Read())
                for (j = startcol; j <= startcol + colcount - 1; j++)
                { 
                    //此处采用的是虚表
                    marrCellText[i, j] = dr.GetValue(j - startcol).ToString ();
                }
            }
            //开启自动刷新
            axGrid1.AutoRedraw=true;
            axGrid1.Refresh();
            return true;
        }

        private void FormFlexcell_Resize(object sender, EventArgs e)
        {
            axGrid1.Height = this.Height - buttonXtoExcel.Height - 60;
            axGrid1.Top = buttonXtoExcel.Top + buttonXtoExcel.Height + 15;
            axGrid1.Left = buttonXSave.Left;
            axGrid1.Width = this.Width - 30;
        }
        //函数功能：直接打开一个flexcell文件
        //输入参数：flexcell文件全路径  输出参数：无
        public void OpenFlexcellFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                ModFlexcell.m_startCol = 100000;
                ModFlexcell.m_startRow = 50;
                ModFlexcell.m_SpecialRow = -1;
                ModFlexcell.m_SpecialRow_ex = -1;
                ModFlexcell.m_SpecialRow_ex2 = -1;
                axGrid1.OpenFile(path);
                axGrid1.Refresh();
            }
        }
    }
}