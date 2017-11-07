using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDataManagerFrame
{
    public partial class FrmResultDataManage : DevComponents.DotNetBar.Office2007Form
    {
        IWorkspace m_Workspace = null;
        public FrmResultDataManage(IWorkspace pWorkspace)
        {
            m_Workspace = pWorkspace;
            InitializeComponent();
            UcResultDataManager.pWorkspace = m_Workspace;
        }
    }
}
