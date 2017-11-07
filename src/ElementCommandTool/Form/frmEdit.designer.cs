namespace ElementCommandTool

{
    partial class frmEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEdit));
            this.txtAnnoText = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.styleGroup = new System.Windows.Forms.GroupBox();
            this.anGle = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.colorAnno = new ElementCommandTool.ColorButton();
            this.cmbTextFont = new System.Windows.Forms.ComboBox();
            this.toolBarAlign = new System.Windows.Forms.ToolBar();
            this.toolLeft = new System.Windows.Forms.ToolBarButton();
            this.toolCenter = new System.Windows.Forms.ToolBarButton();
            this.toolRight = new System.Windows.Forms.ToolBarButton();
            this.toolFull = new System.Windows.Forms.ToolBarButton();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.toolBarStyle = new System.Windows.Forms.ToolBar();
            this.toolBlod = new System.Windows.Forms.ToolBarButton();
            this.toolItalic = new System.Windows.Forms.ToolBarButton();
            this.toolUnderline = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnTextColor = new System.Windows.Forms.Button();
            this.numTextSize = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.fillGroup = new System.Windows.Forms.GroupBox();
            this.l_ColorForPolygon = new System.Windows.Forms.Label();
            this.cmb_ColorForPolygon = new System.Windows.Forms.Button();
            this.l_ColorForLine = new System.Windows.Forms.Label();
            this.cmb_ColorForLine = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_LineWidth = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.styleGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.anGle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTextSize)).BeginInit();
            this.fillGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtAnnoText
            // 
            resources.ApplyResources(this.txtAnnoText, "txtAnnoText");
            this.txtAnnoText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAnnoText.Name = "txtAnnoText";
            this.txtAnnoText.TextChanged += new System.EventHandler(this.txtText_TextChanged);
            this.txtAnnoText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtText_KeyDown);
            this.txtAnnoText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtText_MouseDown);
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // styleGroup
            // 
            resources.ApplyResources(this.styleGroup, "styleGroup");
            this.styleGroup.Controls.Add(this.anGle);
            this.styleGroup.Controls.Add(this.label2);
            this.styleGroup.Controls.Add(this.colorAnno);
            this.styleGroup.Controls.Add(this.cmbTextFont);
            this.styleGroup.Controls.Add(this.toolBarAlign);
            this.styleGroup.Controls.Add(this.toolBarStyle);
            this.styleGroup.Controls.Add(this.btnTextColor);
            this.styleGroup.Controls.Add(this.numTextSize);
            this.styleGroup.Controls.Add(this.label15);
            this.styleGroup.Controls.Add(this.label14);
            this.styleGroup.Controls.Add(this.label12);
            this.styleGroup.ForeColor = System.Drawing.Color.DimGray;
            this.styleGroup.Name = "styleGroup";
            this.styleGroup.TabStop = false;
            // 
            // anGle
            // 
            resources.ApplyResources(this.anGle, "anGle");
            this.anGle.Name = "anGle";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // colorAnno
            // 
            this.colorAnno.Automatic = "Automatic";
            this.colorAnno.Color = System.Drawing.Color.Black;
            resources.ApplyResources(this.colorAnno, "colorAnno");
            this.colorAnno.MoreColors = "More Colors...";
            this.colorAnno.Name = "colorAnno";
            this.colorAnno.UseVisualStyleBackColor = true;
            this.colorAnno.Changed += new System.EventHandler(this.colorAnno_Changed);
            // 
            // cmbTextFont
            // 
            this.cmbTextFont.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.cmbTextFont.FormattingEnabled = true;
            resources.ApplyResources(this.cmbTextFont, "cmbTextFont");
            this.cmbTextFont.Name = "cmbTextFont";
            // 
            // toolBarAlign
            // 
            this.toolBarAlign.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolLeft,
            this.toolCenter,
            this.toolRight,
            this.toolFull});
            resources.ApplyResources(this.toolBarAlign, "toolBarAlign");
            this.toolBarAlign.Divider = false;
            this.toolBarAlign.ImageList = this.imageList2;
            this.toolBarAlign.Name = "toolBarAlign";
            this.toolBarAlign.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarAlign_ButtonClick);
            // 
            // toolLeft
            // 
            resources.ApplyResources(this.toolLeft, "toolLeft");
            this.toolLeft.Name = "toolLeft";
            // 
            // toolCenter
            // 
            resources.ApplyResources(this.toolCenter, "toolCenter");
            this.toolCenter.Name = "toolCenter";
            // 
            // toolRight
            // 
            resources.ApplyResources(this.toolRight, "toolRight");
            this.toolRight.Name = "toolRight";
            // 
            // toolFull
            // 
            resources.ApplyResources(this.toolFull, "toolFull");
            this.toolFull.Name = "toolFull";
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "LeftAlign.bmp");
            this.imageList2.Images.SetKeyName(1, "CenterAlign.bmp");
            this.imageList2.Images.SetKeyName(2, "RightAlign.bmp");
            this.imageList2.Images.SetKeyName(3, "JustifiedAlign.bmp");
            // 
            // toolBarStyle
            // 
            this.toolBarStyle.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBlod,
            this.toolItalic,
            this.toolUnderline});
            resources.ApplyResources(this.toolBarStyle, "toolBarStyle");
            this.toolBarStyle.Divider = false;
            this.toolBarStyle.ImageList = this.imageList1;
            this.toolBarStyle.Name = "toolBarStyle";
            this.toolBarStyle.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBarStyle_ButtonClick);
            // 
            // toolBlod
            // 
            resources.ApplyResources(this.toolBlod, "toolBlod");
            this.toolBlod.Name = "toolBlod";
            // 
            // toolItalic
            // 
            resources.ApplyResources(this.toolItalic, "toolItalic");
            this.toolItalic.Name = "toolItalic";
            // 
            // toolUnderline
            // 
            resources.ApplyResources(this.toolUnderline, "toolUnderline");
            this.toolUnderline.Name = "toolUnderline";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Bold.bmp");
            this.imageList1.Images.SetKeyName(1, "Italic.bmp");
            this.imageList1.Images.SetKeyName(2, "Underline.bmp");
            // 
            // btnTextColor
            // 
            resources.ApplyResources(this.btnTextColor, "btnTextColor");
            this.btnTextColor.Name = "btnTextColor";
            this.btnTextColor.UseVisualStyleBackColor = true;
            // 
            // numTextSize
            // 
            resources.ApplyResources(this.numTextSize, "numTextSize");
            this.numTextSize.Name = "numTextSize";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // fillGroup
            // 
            resources.ApplyResources(this.fillGroup, "fillGroup");
            this.fillGroup.Controls.Add(this.l_ColorForPolygon);
            this.fillGroup.Controls.Add(this.cmb_ColorForPolygon);
            this.fillGroup.Controls.Add(this.l_ColorForLine);
            this.fillGroup.Controls.Add(this.cmb_ColorForLine);
            this.fillGroup.Controls.Add(this.label3);
            this.fillGroup.Controls.Add(this.label1);
            this.fillGroup.Controls.Add(this.txt_LineWidth);
            this.fillGroup.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.fillGroup.Name = "fillGroup";
            this.fillGroup.TabStop = false;
            // 
            // l_ColorForPolygon
            // 
            this.l_ColorForPolygon.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.l_ColorForPolygon, "l_ColorForPolygon");
            this.l_ColorForPolygon.Name = "l_ColorForPolygon";
            // 
            // cmb_ColorForPolygon
            // 
            resources.ApplyResources(this.cmb_ColorForPolygon, "cmb_ColorForPolygon");
            this.cmb_ColorForPolygon.Name = "cmb_ColorForPolygon";
            this.cmb_ColorForPolygon.UseVisualStyleBackColor = true;
            this.cmb_ColorForPolygon.Click += new System.EventHandler(this.cmb_ColorForPolygon_Click);
            // 
            // l_ColorForLine
            // 
            resources.ApplyResources(this.l_ColorForLine, "l_ColorForLine");
            this.l_ColorForLine.Name = "l_ColorForLine";
            // 
            // cmb_ColorForLine
            // 
            resources.ApplyResources(this.cmb_ColorForLine, "cmb_ColorForLine");
            this.cmb_ColorForLine.Name = "cmb_ColorForLine";
            this.cmb_ColorForLine.UseVisualStyleBackColor = true;
            this.cmb_ColorForLine.Click += new System.EventHandler(this.cmb_ColorForLine_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txt_LineWidth
            // 
            resources.ApplyResources(this.txt_LineWidth, "txt_LineWidth");
            this.txt_LineWidth.Name = "txt_LineWidth";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // frmEdit
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.fillGroup);
            this.Controls.Add(this.styleGroup);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtAnnoText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.frmEdit_Load);
            this.styleGroup.ResumeLayout(false);
            this.styleGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.anGle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTextSize)).EndInit();
            this.fillGroup.ResumeLayout(false);
            this.fillGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAnnoText;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox styleGroup;
        private ColorButton colorAnno;
        private System.Windows.Forms.ComboBox cmbTextFont;
        private System.Windows.Forms.ToolBar toolBarAlign;
        private System.Windows.Forms.ToolBarButton toolLeft;
        private System.Windows.Forms.ToolBarButton toolCenter;
        private System.Windows.Forms.ToolBarButton toolRight;
        private System.Windows.Forms.ToolBarButton toolFull;
        private System.Windows.Forms.ToolBar toolBarStyle;
        private System.Windows.Forms.ToolBarButton toolBlod;
        private System.Windows.Forms.ToolBarButton toolItalic;
        private System.Windows.Forms.ToolBarButton toolUnderline;
        private System.Windows.Forms.Button btnTextColor;
        private System.Windows.Forms.NumericUpDown numTextSize;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.GroupBox fillGroup;
        private System.Windows.Forms.Label l_ColorForLine;
        private System.Windows.Forms.Button cmb_ColorForLine;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_LineWidth;
        private System.Windows.Forms.Label l_ColorForPolygon;
        private System.Windows.Forms.Button cmb_ColorForPolygon;
        private System.Windows.Forms.NumericUpDown anGle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;

    }
}