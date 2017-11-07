using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace GeoSysUpdate
{
    public partial class ucProjectInfo : UserControl
    {
        public ucProjectInfo()
        {
            InitializeComponent();
            this.treeProjectInfo.ImageList = this.IconContainer;
        }

        public DevComponents.AdvTree.AdvTree CurrentTree
        {
            get { return this.treeProjectInfo; }
        }
    }
}
