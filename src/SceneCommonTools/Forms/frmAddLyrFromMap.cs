using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Analyst3D;

namespace SceneCommonTools.Forms
{
    public partial class frmAddLyrFromMap : Form
    {
        public frmAddLyrFromMap()
        {
            InitializeComponent();
        }

        private void frmAddLyrFromMap_Load(object sender, EventArgs e)
        {
            InitLst();
        }

        private IDictionary<int, ILayer> m_vDicLyrs = null;

        private IMap m_pMap = null;
        private IScene m_pScene = null;
        public IMap CurMap
        {
            set { m_pMap = value; }
        }
        public IScene CurScene
        {
            set { m_pScene = value; }
        }

        private void InitLst()
        {
            m_vDicLyrs = new Dictionary<int, ILayer>();

            if (m_pMap == null) return;

            for (int i = 0; i < m_pMap.LayerCount; i++)
            {
                ILayer pLyr = m_pMap.get_Layer(i);
                m_vDicLyrs.Add(i,pLyr);
                this.lstLyrs.Items.Add(pLyr.Name,pLyr.Visible);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (m_pScene == null) return;

            for (int i = 0; i < this.lstLyrs.Items.Count; i++)
            {
                if (this.lstLyrs.GetItemChecked(i) && !ContainLyr(m_vDicLyrs[i],m_pScene))
                {
                    m_pScene.AddLayer(m_vDicLyrs[i],true);
                }
            }
        }

        private bool ContainLyr(ILayer pLyr, IScene pScene)
        {
            for (int i = 0; i < pScene.LayerCount; i++)
            {
                if (pScene.get_Layer(i).Equals(pLyr)) return true;
            }

            return false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.lstLyrs.Items.Count; i++)
            {
                this.lstLyrs.SetItemChecked(i, true);
            }
        }

        private void btnNoSelAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.lstLyrs.Items.Count; i++)
            {
                this.lstLyrs.SetItemChecked(i, !this.lstLyrs.GetItemChecked(i));
            }
        }
    }
}
