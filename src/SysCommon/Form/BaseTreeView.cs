using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

/// <summary>
/// 基础树控件
/// </summary>
namespace SysCommon
{
    public partial class BaseTreeView : DevExpress.XtraTreeList.TreeList
    {
        public BaseTreeView()
        {
            InitializeComponent();
            Initial();
        }

        private void Initial()
        {
            BeginInit();
            OptionsView.ShowColumns = false;
            OptionsView.ShowCaption = false;
            OptionsView.ShowHorzLines = false;
            OptionsView.ShowVertLines = false;
            OptionsLayout.AddNewColumns = false;
            OptionsBehavior.Editable = false;
            EndInit();
        }
        public BaseTreeView(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
    }
}
