using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesGDB;
using System.Data;
namespace GeoDataChecker
{
    /// <summary>
    /// 处理检查结果栏的委托，使它能够以安全线程在主窗体上无错显示
    /// 王冰编写
    /// </summary>
    public class ClearDataCheckGrid
    {
        /// <summary>
        /// 定义一个委托，用来清除检查结果
        /// </summary>
        /// <param name="AppHk"></param>
        public delegate void ClearGrid(Plugin.Application.IAppGISRef AppHk);//定义一个委托
        /// <summary>
        /// 清除检查结果显示的内容方法
        /// </summary>
        /// <param name="_AppHk"></param>
        public void ClearResult(Plugin.Application.IAppGISRef _AppHk)
        {
            if (_AppHk.DataCheckGrid.DataSource != null)//如果显示框里有之前的记录，就先要把之前功能检查的清除掉，以便显示当前的
                _AppHk.DataCheckGrid.DataSource = null;
        }

        /// <summary>
        /// 使用窗体句柄调节器用委托及相关的方法，实际执行
        /// </summary>
        /// <param name="pAppForm"></param>
        /// <param name="_AppHk"></param>
        public void Operate(Plugin.Application.IAppFormRef pAppForm, Plugin.Application.IAppGISRef _AppHk)
        {
            pAppForm.MainForm.Invoke(new ClearGrid(ClearResult), new object[] { _AppHk });
        }

        /// <summary>
        /// 定义一个高亮显示检查结果栏的委托
        /// </summary>
        /// <param name="hook"></param>
        private delegate void Delegate_CheckDataGrid(Plugin.Application.IAppGISRef hook);
        /// <summary>
        /// /选中检查出错列表
        /// </summary>
        private void CheckDataGrid(Plugin.Application.IAppGISRef hook)
        {
            DevComponents.DotNetBar.PanelDockContainer PanelTip = hook.DataCheckGrid.Parent as DevComponents.DotNetBar.PanelDockContainer;
            if (PanelTip != null)
            {
                PanelTip.DockContainerItem.Selected = true;
            }
        }
        /// <summary>
        /// 使用主窗体安全委托,高亮显示检查结果栏
        /// </summary>
        /// <param name="pAppForm"></param>
        /// <param name="hook"></param>
        public void CheckDataGridShow(Plugin.Application.IAppFormRef pAppForm, Plugin.Application.IAppGISRef hook)
        {
            pAppForm.MainForm.Invoke(new Delegate_CheckDataGrid(CheckDataGrid), new object[] { hook });
        }
    }
}
