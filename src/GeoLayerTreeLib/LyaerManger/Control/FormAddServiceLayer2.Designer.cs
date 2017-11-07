namespace GeoLayerTreeLib.LayerManager
{
    partial class FormAddServiceLayer2
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
            this.txtServiceLocation = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelServiceLocation = new DevComponents.DotNetBar.LabelX();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnAddServiceLayer = new DevComponents.DotNetBar.ButtonX();
            this.txtLayerName = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.advTree1 = new DevComponents.AdvTree.AdvTree();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtServiceLocation
            // 
            // 
            // 
            // 
            this.txtServiceLocation.Border.Class = "TextBoxBorder";
            this.txtServiceLocation.Location = new System.Drawing.Point(72, 38);
            this.txtServiceLocation.Multiline = true;
            this.txtServiceLocation.Name = "txtServiceLocation";
            this.txtServiceLocation.Size = new System.Drawing.Size(230, 21);
            this.txtServiceLocation.TabIndex = 28;
            this.txtServiceLocation.WordWrap = false;
            this.txtServiceLocation.TextChanged += new System.EventHandler(this.txtServiceLocation_TextChanged);
            // 
            // labelServiceLocation
            // 
            this.labelServiceLocation.Location = new System.Drawing.Point(7, 39);
            this.labelServiceLocation.Name = "labelServiceLocation";
            this.labelServiceLocation.Size = new System.Drawing.Size(68, 19);
            this.labelServiceLocation.TabIndex = 32;
            this.labelServiceLocation.Text = "服务地址：";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnCancel.Location = new System.Drawing.Point(227, 385);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAddServiceLayer
            // 
            this.btnAddServiceLayer.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddServiceLayer.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnAddServiceLayer.Location = new System.Drawing.Point(145, 385);
            this.btnAddServiceLayer.Name = "btnAddServiceLayer";
            this.btnAddServiceLayer.Size = new System.Drawing.Size(75, 23);
            this.btnAddServiceLayer.TabIndex = 30;
            this.btnAddServiceLayer.Text = "添加";
            this.btnAddServiceLayer.Click += new System.EventHandler(this.btnAddServiceLayer_Click);
            // 
            // txtLayerName
            // 
            // 
            // 
            // 
            this.txtLayerName.Border.Class = "TextBoxBorder";
            this.txtLayerName.Location = new System.Drawing.Point(72, 13);
            this.txtLayerName.Name = "txtLayerName";
            this.txtLayerName.Size = new System.Drawing.Size(230, 21);
            this.txtLayerName.TabIndex = 27;
            this.txtLayerName.WordWrap = false;
            // 
            // labelX5
            // 
            this.labelX5.Location = new System.Drawing.Point(9, 14);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(65, 19);
            this.labelX5.TabIndex = 29;
            this.labelX5.Text = "图层名：";
            // 
            // advTree1
            // 
            this.advTree1.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTree1.AllowDrop = true;
            this.advTree1.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTree1.BackgroundStyle.Class = "TreeBorderKey";
            this.advTree1.DragDropEnabled = false;
            this.advTree1.DragDropNodeCopyEnabled = false;
            this.advTree1.Location = new System.Drawing.Point(7, 65);
            this.advTree1.Name = "advTree1";
            this.advTree1.NodesConnector = this.nodeConnector1;
            this.advTree1.NodeStyle = this.elementStyle1;
            this.advTree1.PathSeparator = ";";
            this.advTree1.Size = new System.Drawing.Size(295, 314);
            this.advTree1.Styles.Add(this.elementStyle1);
            this.advTree1.TabIndex = 33;
            this.advTree1.Text = "advTree1";
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
            // FormAddServiceLayer2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 413);
            this.Controls.Add(this.advTree1);
            this.Controls.Add(this.txtServiceLocation);
            this.Controls.Add(this.labelServiceLocation);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAddServiceLayer);
            this.Controls.Add(this.txtLayerName);
            this.Controls.Add(this.labelX5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddServiceLayer2";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加服务层";
            this.Load += new System.EventHandler(this.FormAddServiceLayer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX txtServiceLocation;
        private DevComponents.DotNetBar.LabelX labelServiceLocation;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnAddServiceLayer;
        private DevComponents.DotNetBar.Controls.TextBoxX txtLayerName;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.AdvTree.AdvTree advTree1;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
    }
}