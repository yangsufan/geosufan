namespace GeoHistory
{
    partial class FrmAddLayer
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
            this.txtBoxNewLayer = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.txtBoxOldLayer = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnOK = new DevComponents.DotNetBar.ButtonX();
            this.advTreeNewLayer = new DevComponents.AdvTree.AdvTree();
            this.node2 = new DevComponents.AdvTree.Node();
            this.nodeConnector2 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle2 = new DevComponents.DotNetBar.ElementStyle();
            this.advTreeOldLayer = new DevComponents.AdvTree.AdvTree();
            this.node1 = new DevComponents.AdvTree.Node();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeNewLayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeOldLayer)).BeginInit();
            this.SuspendLayout();
            // 
            // txtBoxNewLayer
            // 
            // 
            // 
            // 
            this.txtBoxNewLayer.Border.Class = "TextBoxBorder";
            this.txtBoxNewLayer.Location = new System.Drawing.Point(79, 47);
            this.txtBoxNewLayer.Name = "txtBoxNewLayer";
            this.txtBoxNewLayer.Size = new System.Drawing.Size(193, 21);
            this.txtBoxNewLayer.TabIndex = 7;
            this.txtBoxNewLayer.TextChanged += new System.EventHandler(this.txtBoxNewLayer_TextChanged);
            this.txtBoxNewLayer.Click += new System.EventHandler(this.txtBoxNewLayer_Click);
            // 
            // txtBoxOldLayer
            // 
            // 
            // 
            // 
            this.txtBoxOldLayer.Border.Class = "TextBoxBorder";
            this.txtBoxOldLayer.Location = new System.Drawing.Point(79, 16);
            this.txtBoxOldLayer.Name = "txtBoxOldLayer";
            this.txtBoxOldLayer.Size = new System.Drawing.Size(193, 21);
            this.txtBoxOldLayer.TabIndex = 6;
            this.txtBoxOldLayer.TextChanged += new System.EventHandler(this.txtBoxOldLayer_TextChanged);
            this.txtBoxOldLayer.Click += new System.EventHandler(this.txtBoxOldLayer_Click);
            // 
            // labelX3
            // 
            this.labelX3.Location = new System.Drawing.Point(10, 47);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(85, 20);
            this.labelX3.TabIndex = 4;
            this.labelX3.Text = "林斑层(新):";
            // 
            // labelX2
            // 
            this.labelX2.Location = new System.Drawing.Point(10, 16);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(85, 20);
            this.labelX2.TabIndex = 5;
            this.labelX2.Text = "林斑层(旧):";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(205, 108);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(62, 22);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "取消";
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnOK.Location = new System.Drawing.Point(117, 108);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(62, 22);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "加载";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // advTreeNewLayer
            // 
            this.advTreeNewLayer.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTreeNewLayer.AllowDrop = true;
            this.advTreeNewLayer.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTreeNewLayer.BackgroundStyle.Class = "TreeBorderKey";
            this.advTreeNewLayer.Location = new System.Drawing.Point(79, 47);
            this.advTreeNewLayer.Name = "advTreeNewLayer";
            this.advTreeNewLayer.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node2});
            this.advTreeNewLayer.NodesConnector = this.nodeConnector2;
            this.advTreeNewLayer.NodeStyle = this.elementStyle2;
            this.advTreeNewLayer.PathSeparator = ";";
            this.advTreeNewLayer.Size = new System.Drawing.Size(56, 83);
            this.advTreeNewLayer.Styles.Add(this.elementStyle2);
            this.advTreeNewLayer.TabIndex = 12;
            this.advTreeNewLayer.Text = "advTree1";
            this.advTreeNewLayer.Visible = false;
            this.advTreeNewLayer.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTreeNewLayer_NodeClick);
            // 
            // node2
            // 
            this.node2.Expanded = true;
            this.node2.Name = "node2";
            this.node2.Text = "node2";
            // 
            // nodeConnector2
            // 
            this.nodeConnector2.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle2
            // 
            this.elementStyle2.Name = "elementStyle2";
            this.elementStyle2.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // advTreeOldLayer
            // 
            this.advTreeOldLayer.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTreeOldLayer.AllowDrop = true;
            this.advTreeOldLayer.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTreeOldLayer.BackgroundStyle.Class = "TreeBorderKey";
            this.advTreeOldLayer.Location = new System.Drawing.Point(79, 16);
            this.advTreeOldLayer.Name = "advTreeOldLayer";
            this.advTreeOldLayer.Nodes.AddRange(new DevComponents.AdvTree.Node[] {
            this.node1});
            this.advTreeOldLayer.NodesConnector = this.nodeConnector1;
            this.advTreeOldLayer.NodeStyle = this.elementStyle1;
            this.advTreeOldLayer.PathSeparator = ";";
            this.advTreeOldLayer.Size = new System.Drawing.Size(24, 110);
            this.advTreeOldLayer.Styles.Add(this.elementStyle1);
            this.advTreeOldLayer.TabIndex = 12;
            this.advTreeOldLayer.Text = "advTree1";
            this.advTreeOldLayer.Visible = false;
            this.advTreeOldLayer.NodeClick += new DevComponents.AdvTree.TreeNodeMouseEventHandler(this.advTreeOldLayer_NodeClick);
            // 
            // node1
            // 
            this.node1.Name = "node1";
            this.node1.Text = "node2";
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
            // FrmAddLayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 132);
            this.Controls.Add(this.advTreeNewLayer);
            this.Controls.Add(this.advTreeOldLayer);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtBoxNewLayer);
            this.Controls.Add(this.txtBoxOldLayer);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAddLayer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmAddLayer";
            this.Load += new System.EventHandler(this.FrmAddLayer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.advTreeNewLayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTreeOldLayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtBoxNewLayer;
        private DevComponents.DotNetBar.Controls.TextBoxX txtBoxOldLayer;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnOK;
        private DevComponents.AdvTree.AdvTree advTreeNewLayer;
        private DevComponents.AdvTree.Node node2;
        private DevComponents.AdvTree.NodeConnector nodeConnector2;
        private DevComponents.DotNetBar.ElementStyle elementStyle2;
        private DevComponents.AdvTree.AdvTree advTreeOldLayer;
        private DevComponents.AdvTree.Node node1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
    }
}