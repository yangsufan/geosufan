using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace GeoStatistics
{
    public partial class frmStatisticsPic : DevComponents.DotNetBar.Office2007Form
    {
        public frmStatisticsPic()
        {
            InitializeComponent();
            InitType();
        }

        private void InitType()
        {
            this.comboType.Items.Add("Öù×´Í¼");
            //this.comboType.Items.Add("±ý×´Í¼");
            this.comboType.Items.Add("Ïß×´Í¼");
            this.comboType.Items.Add("Ãæ×´Í¼");
            this.comboType.SelectedIndex = 0;
        }

        private void frmStatisticsPic_Load(object sender, EventArgs e)
        {

            //this.reportViewer1.RefreshReport();
        }

        private SortedDictionary<string, double> m_ChartValue;
        public SortedDictionary<string, double> ChartValue
        {
            set { m_ChartValue = value; }
        }

        public string m_strXtitle = "";
        public string m_strYtitle = "";
        public string m_strChartTitle = "";

        public void InitChart(AxMSChart20Lib.AxMSChart vChart)
        {
            vChart.AllowSelections = false;
            vChart.AllowSeriesSelection = false;

            vChart.Plot.get_Axis(MSChart20Lib.VtChAxisId.VtChAxisIdX, 0).AxisTitle.Text = m_strXtitle;
            vChart.Plot.get_Axis(MSChart20Lib.VtChAxisId.VtChAxisIdY, 0).AxisTitle.Text = m_strYtitle;
            vChart.TitleText = m_strChartTitle;

            vChart.Plot.get_Axis(MSChart20Lib.VtChAxisId.VtChAxisIdX, 0).AxisGrid.MajorPen.Style = MSChart20Lib.VtPenStyle.VtPenStyleNull;
            vChart.Plot.get_Axis(MSChart20Lib.VtChAxisId.VtChAxisIdX, 0).AxisGrid.MinorPen.Style = MSChart20Lib.VtPenStyle.VtPenStyleNull;
        }

        public void BindData()
        {
            if (m_ChartValue == null) return;

            SetChartValue(this.axMSChart1, m_ChartValue);
            //·ÅºóÃæÁË
            InitChart(this.axMSChart1);
        }

        public void SetChartValue(AxMSChart20Lib.AxMSChart vChart, SortedDictionary<string, double> value)
        {
            vChart.SeriesColumn = 1;
            vChart.RowCount = (short)value.Count;
            int i = 1;
            
            //
            string strOther = "";
            double dblOther = 0;
            foreach(KeyValuePair<string,double> vItem in value)
            {
                string strKey = vItem.Key;
                double dblValue = vItem.Value;
                if (strKey == "ÆäËû")
                {
                    strOther = "ÆäËû";
                    dblOther = dblValue;
                    continue;
                }
                vChart.Row = (short)i;
                vChart.RowLabel = strKey;
                vChart.DataGrid.SetData((short)i, 1, dblValue, 0);
                i++;
            }

            if (strOther != "")
            {
                vChart.Row = (short)i;
                vChart.RowLabel = strOther;
                vChart.DataGrid.SetData((short)i, 1, dblOther, 0);
            }
        }

        private void btnExportPic_Click(object sender, EventArgs e)
        {
            this.axMSChart1.EditCopy();
        }

        private void comboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboType.Text)
            {
                case "Öù×´Í¼":
                    this.axMSChart1.chartType = MSChart20Lib.VtChChartType.VtChChartType2dBar;
                    break;
                case "±ý×´Í¼":
                    this.axMSChart1.chartType = MSChart20Lib.VtChChartType.VtChChartType2dPie;
                    break;
                case "Ïß×´Í¼":
                    this.axMSChart1.chartType = MSChart20Lib.VtChChartType.VtChChartType2dLine;
                    break;
                case "Ãæ×´Í¼":
                    this.axMSChart1.chartType = MSChart20Lib.VtChChartType.VtChChartType2dArea;
                    break;
            }
        }




    }
}