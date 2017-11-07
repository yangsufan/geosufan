using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using ESRI.ArcGIS.Display;
using stdole;

namespace ElementCommandTool
{
    public partial class frmEdit : DevComponents.DotNetBar.Office2007Form
    {
        bool canFill;
        public ITextSymbol m_pTextSymbol;
        public IFillSymbol m_pFillSymbol;
        public ILineSymbol m_pLineSymbol;

        public string AnnoText
        {
            get
            {
                return this.txtAnnoText.Text;
            }
        }

        public frmEdit(bool postil)
        {
            InitializeComponent();
            m_pTextSymbol = new TextSymbolClass();
            m_pFillSymbol = new SimpleFillSymbolClass();
            m_pLineSymbol = new SimpleLineSymbolClass();
            canFill = postil;
        }

        private void txtText_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void txtText_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Hide();
            }
        }

        private void txtText_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtAnnoText.Text.Trim().Equals("输入文本"))
            {
                txtAnnoText.Text = "";
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtAnnoText.Text.Trim().Equals("输入文本"))
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                return;
            }
            if (m_pTextSymbol == null)
                m_pTextSymbol = new TextSymbolClass();
            //得到注记字体格式
            IFontDisp pFontDisp = m_pTextSymbol.Font;
            pFontDisp.Name = cmbTextFont.Text;

            pFontDisp.Bold = toolBarStyle.Buttons["toolBlod"].Pushed;
            pFontDisp.Italic = toolBarStyle.Buttons["toolItalic"].Pushed;
            pFontDisp.Underline = toolBarStyle.Buttons["toolUnderline"].Pushed;
            pFontDisp.Size = numTextSize.Value;
            IRgbColor pRGBColor = new RgbColorClass();
            pRGBColor.Red = colorAnno.Color.R;//yjl修改，之前颜色始终为灰
            pRGBColor.Green = colorAnno.Color.G;
            pRGBColor.Blue = colorAnno.Color.B;
            m_pTextSymbol.Color = pRGBColor as IColor;
            m_pTextSymbol.Font = pFontDisp;
            //得到注记对齐格式
            if (toolBarAlign.Buttons["toolLeft"].Pushed)
                m_pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft;
            else if (toolBarAlign.Buttons["toolCenter"].Pushed)
                m_pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter;
            else if (toolBarAlign.Buttons["toolRight"].Pushed)
                m_pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHARight;
            else
                m_pTextSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHAFull;

            m_pTextSymbol.Angle = (double)this.anGle.Value;

            if (canFill)
            {
                if (m_pLineSymbol == null)
                    m_pLineSymbol = new SimpleLineSymbolClass();
                if (m_pFillSymbol == null)
                    m_pFillSymbol = new SimpleFillSymbolClass();

                pRGBColor = new RgbColorClass();
                pRGBColor.Red = l_ColorForLine.ForeColor.R;
                pRGBColor.Green = l_ColorForLine.ForeColor.G;
                pRGBColor.Blue = l_ColorForLine.ForeColor.B;
                m_pLineSymbol.Color = pRGBColor as IColor;

                pRGBColor.Red = l_ColorForPolygon.BackColor.R;
                pRGBColor.Green = l_ColorForPolygon.BackColor.G;
                pRGBColor.Blue = l_ColorForPolygon.BackColor.B;
                m_pFillSymbol.Color = pRGBColor as IColor;

                try
                {
                    if (!string.IsNullOrEmpty(txt_LineWidth.Text))
                    {
                        m_pLineSymbol.Width = (Convert.ToDouble(txt_LineWidth.Text) * 72) / 25.4;
                    }
                }
                catch
                {
                }
                m_pFillSymbol.Outline = m_pLineSymbol;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void cmbTextFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtAnnoText.Font = GetFont();
        }

        private System.Drawing.Font GetFont()
        {
            FontStyle fontStyle = FontStyle.Regular;
            if (toolBarStyle.Buttons["toolBlod"].Pushed)
                fontStyle = fontStyle | FontStyle.Bold;
            if (toolBarStyle.Buttons["toolItalic"].Pushed)
                fontStyle = fontStyle | FontStyle.Italic;
            if (toolBarStyle.Buttons["toolUnderline"].Pushed)
                fontStyle = fontStyle | FontStyle.Underline;
            float size=(float)numTextSize.Value;
            System.Drawing.Font font = new System.Drawing.Font(cmbTextFont.Text, (size<=1?1:size), fontStyle);
            return font;
        }

        private void numTextSize_ValueChanged(object sender, EventArgs e)
        {
            txtAnnoText.Font = GetFont();
        }

        private void toolBarStyle_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            e.Button.Pushed = !e.Button.Pushed;
            txtAnnoText.Font = GetFont();
        }

        private void toolBarAlign_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            ToolBarButton btn;
            for (int i = 0; i < toolBarAlign.Buttons.Count; i++)
            {
                btn = toolBarAlign.Buttons[i];
                btn.Pushed = false;
            }
            btn = e.Button;
            btn.Pushed = true;
        }

        private void cmb_ColorForLine_Click(object sender, EventArgs e)
        {
            l_ColorForLine.ForeColor = GetColor(l_ColorForLine.ForeColor);
        }

        private void cmb_ColorForPolygon_Click(object sender, EventArgs e)
        {
            l_ColorForPolygon.BackColor = GetColor(l_ColorForPolygon.BackColor);
        }

        private Color GetColor(Color pColor)
        {
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.AnyColor = true;
            colorDlg.Color = pColor;
            if (colorDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return colorDlg.Color;
            }
            else
            {
                return pColor;
            }
        }

        private void colorAnno_Changed(object sender, EventArgs e)
        {
            txtAnnoText.ForeColor = colorAnno.Color;            
        }

        private void frmEdit_Load(object sender, EventArgs e)
        {
            if (!canFill)
            {
                this.Height = this.Height - fillGroup.Height;
            }
            List<string> TextStyle = new List<string>();
            System.Drawing.Text.InstalledFontCollection CollectionFont = new System.Drawing.Text.InstalledFontCollection();
            FontFamily[] Familys = CollectionFont.Families;
            for (int i = 0; i < Familys.Length; i++)
            {
                TextStyle.Add(Familys[i].Name);
            }
            cmbTextFont.Items.AddRange(TextStyle.ToArray());
            cmbTextFont.Text = txtAnnoText.Font.FontFamily.Name;

            fillGroup.Visible=canFill;
            numTextSize.Value = decimal.Parse(txtAnnoText.Font.Size.ToString());
            cmbTextFont.SelectedIndexChanged+=new EventHandler(cmbTextFont_SelectedIndexChanged);
            numTextSize.ValueChanged+=new EventHandler(numTextSize_ValueChanged);

            //颜色值默认是黑色的
            colorAnno.Color = System.Drawing.Color.Black;
        }
    }
}