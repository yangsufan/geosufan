namespace GeoSysSetting
{
    partial class FormFullMapConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFullMapConfig));
            this.buttonCancel = new DevComponents.DotNetBar.ButtonX();
            this.buttonOK = new DevComponents.DotNetBar.ButtonX();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.advTreeLayerList = new DevComponents.AdvTree.AdvTree();
            this.node1 = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.txtBoxXmin = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtBoxYmin = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtBoxXmax = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtBoxYmax = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.txtBoxXminNew = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtBoxYminNew = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtBoxXmaxNew = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtBoxYmaxNew = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX10 = new DevComponents.DotNetBar.LabelX();
            this.labelX11 = new DevComponents.DotNetBar.LabelX();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLayerList)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonCancel.Location = new System.Drawing.Point(410, 312);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(70, 23);
            this.buttonCancel.TabIndex = 16;
            this.buttonCancel.Text = "取消";
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonOK.Location = new System.Drawing.Point(329, 312);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(70, 23);
            this.buttonOK.TabIndex = 15;
            this.buttonOK.Text = "确定";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.panelEx1.Controls.Add(this.advTreeLayerList);
            this.panelEx1.Location = new System.Drawing.Point(3, 25);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(223, 310);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 13;
            this.panelEx1.Text = "panelEx1";
            // 
            // advTreeLayerList
            // 
            this.advTreeLayerList.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTreeLayerList.AllowDrop = true;
            this.advTreeLayerList.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTreeLayerList.BackgroundStyle.Class = "TreeBorderKey";
            this.advTreeLayerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.advTreeLayerList.Location = new System.Drawing.Point(0, 0);
            this.advTreeLayerList.Name = "advTreeLayerList";
            this.advTreeLayerList.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1});
            this.advTreeLayerList.NodesConnector = this.nodeConnector1;
            this.advTreeLayerList.NodeStyle = this.elementStyle1;
            this.advTreeLayerList.PathSeparator = ";";
            this.advTreeLayerList.Size = new System.Drawing.Size(223, 310);
            this.advTreeLayerList.Styles.Add(this.elementStyle1);
            this.advTreeLayerList.TabIndex = 0;
            this.advTreeLayerList.Text = "advTree1";
            this.advTreeLayerList.AfterNodeSelect += new DevComponents.AdvTree.AdvTreeNodeEventHandler(this.advTreeLayerList_AfterNodeSelect);
            // 
            // node1
            // 
            this.node1.Expanded = true;
            this.node1.Name = "node1";
            this.node1.Text = "node1";
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle1
            // 
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // ImageList
            // 
            this.ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList.Images.SetKeyName(0, "earth");
            this.ImageList.Images.SetKeyName(1, "Root");
            this.ImageList.Images.SetKeyName(2, "DIR");
            this.ImageList.Images.SetKeyName(3, "DataDIRHalfOpen");
            this.ImageList.Images.SetKeyName(4, "DataDIRClosed");
            this.ImageList.Images.SetKeyName(5, "DataDIROpen");
            this.ImageList.Images.SetKeyName(6, "Layer");
            this.ImageList.Images.SetKeyName(7, "_annotation");
            this.ImageList.Images.SetKeyName(8, "_Dimension");
            this.ImageList.Images.SetKeyName(9, "_line");
            this.ImageList.Images.SetKeyName(10, "_MultiPatch");
            this.ImageList.Images.SetKeyName(11, "_point");
            this.ImageList.Images.SetKeyName(12, "_polygon");
            this.ImageList.Images.SetKeyName(13, "annotation");
            this.ImageList.Images.SetKeyName(14, "Dimension");
            this.ImageList.Images.SetKeyName(15, "line");
            this.ImageList.Images.SetKeyName(16, "MultiPatch");
            this.ImageList.Images.SetKeyName(17, "point");
            this.ImageList.Images.SetKeyName(18, "polygon");
            this.ImageList.Images.SetKeyName(19, "PublicVersion");
            this.ImageList.Images.SetKeyName(20, "PersonalVersion");
            this.ImageList.Images.SetKeyName(21, "INVISIBLE");
            this.ImageList.Images.SetKeyName(22, "VISIBLE");
            // 
            // labelX1
            // 
            this.labelX1.Location = new System.Drawing.Point(3, 3);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(219, 25);
            this.labelX1.TabIndex = 17;
            this.labelX1.Text = "选择目标图层：";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(231, 25);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(63, 26);
            this.labelX2.TabIndex = 18;
            this.labelX2.Text = "X最小值：";
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(231, 57);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(63, 26);
            this.labelX3.TabIndex = 18;
            this.labelX3.Text = "Y最小值：";
            // 
            // labelX4
            // 
            this.labelX4.Location = new System.Drawing.Point(231, 89);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(63, 26);
            this.labelX4.TabIndex = 18;
            this.labelX4.Text = "X最大值：";
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(231, 121);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(63, 26);
            this.labelX5.TabIndex = 18;
            this.labelX5.Text = "Y最大值：";
            // 
            // txtBoxXmin
            // 
            // 
            // 
            // 
            this.txtBoxXmin.Border.Class = "TextBoxBorder";
            this.txtBoxXmin.Location = new System.Drawing.Point(288, 26);
            this.txtBoxXmin.Name = "txtBoxXmin";
            this.txtBoxXmin.Size = new System.Drawing.Size(192, 21);
            this.txtBoxXmin.TabIndex = 19;
            // 
            // txtBoxYmin
            // 
            // 
            // 
            // 
            this.txtBoxYmin.Border.Class = "TextBoxBorder";
            this.txtBoxYmin.Location = new System.Drawing.Point(288, 57);
            this.txtBoxYmin.Name = "txtBoxYmin";
            this.txtBoxYmin.Size = new System.Drawing.Size(192, 21);
            this.txtBoxYmin.TabIndex = 19;
            // 
            // txtBoxXmax
            // 
            // 
            // 
            // 
            this.txtBoxXmax.Border.Class = "TextBoxBorder";
            this.txtBoxXmax.Location = new System.Drawing.Point(288, 89);
            this.txtBoxXmax.Name = "txtBoxXmax";
            this.txtBoxXmax.Size = new System.Drawing.Size(192, 21);
            this.txtBoxXmax.TabIndex = 19;
            // 
            // txtBoxYmax
            // 
            // 
            // 
            // 
            this.txtBoxYmax.Border.Class = "TextBoxBorder";
            this.txtBoxYmax.Location = new System.Drawing.Point(288, 121);
            this.txtBoxYmax.Name = "txtBoxYmax";
            this.txtBoxYmax.Size = new System.Drawing.Size(192, 21);
            this.txtBoxYmax.TabIndex = 19;
            // 
            // labelX6
            // 
            this.labelX6.Location = new System.Drawing.Point(231, 185);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(63, 26);
            this.labelX6.TabIndex = 18;
            this.labelX6.Text = "X最小值：";
            // 
            // labelX7
            // 
            this.labelX7.Location = new System.Drawing.Point(231, 217);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(63, 26);
            this.labelX7.TabIndex = 18;
            this.labelX7.Text = "Y最小值：";
            // 
            // labelX8
            // 
            this.labelX8.Location = new System.Drawing.Point(231, 249);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(63, 26);
            this.labelX8.TabIndex = 18;
            this.labelX8.Text = "X最大值：";
            // 
            // labelX9
            // 
            this.labelX9.Location = new System.Drawing.Point(231, 281);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(63, 26);
            this.labelX9.TabIndex = 18;
            this.labelX9.Text = "Y最大值：";
            // 
            // txtBoxXminNew
            // 
            // 
            // 
            // 
            this.txtBoxXminNew.Border.Class = "TextBoxBorder";
            this.txtBoxXminNew.Location = new System.Drawing.Point(288, 186);
            this.txtBoxXminNew.Name = "txtBoxXminNew";
            this.txtBoxXminNew.Size = new System.Drawing.Size(192, 21);
            this.txtBoxXminNew.TabIndex = 19;
            // 
            // txtBoxYminNew
            // 
            // 
            // 
            // 
            this.txtBoxYminNew.Border.Class = "TextBoxBorder";
            this.txtBoxYminNew.Location = new System.Drawing.Point(288, 217);
            this.txtBoxYminNew.Name = "txtBoxYminNew";
            this.txtBoxYminNew.Size = new System.Drawing.Size(192, 21);
            this.txtBoxYminNew.TabIndex = 19;
            // 
            // txtBoxXmaxNew
            // 
            // 
            // 
            // 
            this.txtBoxXmaxNew.Border.Class = "TextBoxBorder";
            this.txtBoxXmaxNew.Location = new System.Drawing.Point(288, 249);
            this.txtBoxXmaxNew.Name = "txtBoxXmaxNew";
            this.txtBoxXmaxNew.Size = new System.Drawing.Size(192, 21);
            this.txtBoxXmaxNew.TabIndex = 19;
            // 
            // txtBoxYmaxNew
            // 
            // 
            // 
            // 
            this.txtBoxYmaxNew.Border.Class = "TextBoxBorder";
            this.txtBoxYmaxNew.Location = new System.Drawing.Point(288, 281);
            this.txtBoxYmaxNew.Name = "txtBoxYmaxNew";
            this.txtBoxYmaxNew.Size = new System.Drawing.Size(192, 21);
            this.txtBoxYmaxNew.TabIndex = 19;
            // 
            // labelX10
            // 
            this.labelX10.Location = new System.Drawing.Point(231, 3);
            this.labelX10.Name = "labelX10";
            this.labelX10.Size = new System.Drawing.Size(125, 25);
            this.labelX10.TabIndex = 17;
            this.labelX10.Text = "修改前全图参数：";
            // 
            // labelX11
            // 
            this.labelX11.Location = new System.Drawing.Point(229, 157);
            this.labelX11.Name = "labelX11";
            this.labelX11.Size = new System.Drawing.Size(125, 25);
            this.labelX11.TabIndex = 17;
            this.labelX11.Text = "修改后全图参数：";
            // 
            // FormFullMapConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 338);
            this.Controls.Add(this.txtBoxYmaxNew);
            this.Controls.Add(this.txtBoxYmax);
            this.Controls.Add(this.txtBoxXmaxNew);
            this.Controls.Add(this.txtBoxXmax);
            this.Controls.Add(this.txtBoxYminNew);
            this.Controls.Add(this.txtBoxYmin);
            this.Controls.Add(this.txtBoxXminNew);
            this.Controls.Add(this.txtBoxXmin);
            this.Controls.Add(this.labelX9);
            this.Controls.Add(this.labelX8);
            this.Controls.Add(this.labelX5);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX11);
            this.Controls.Add(this.labelX10);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.panelEx1);
            this.Controls.Add(this.labelX1);
            this.Name = "FormFullMapConfig";
            this.ShowIcon = false;
            this.Text = "全图参数配置";
            this.Load += new System.EventHandler(this.FormFullMapConfig_Load);
            this.panelEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeLayerList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX buttonCancel;
        private DevComponents.DotNetBar.ButtonX buttonOK;
        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.AdvTree.AdvTree advTreeLayerList;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        public System.Windows.Forms.ImageList ImageList;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBoxXmin;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBoxYmin;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBoxXmax;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBoxYmax;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBoxXminNew;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBoxYminNew;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBoxXmaxNew;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBoxYmaxNew;
        private DevComponents.DotNetBar.LabelX labelX10;
        private DevComponents.DotNetBar.LabelX labelX11;
    }
}