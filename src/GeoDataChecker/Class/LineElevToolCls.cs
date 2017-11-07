using System;
using System.Collections.Generic;
using System.Text;

using ESRI.ArcGIS.ADF.BaseClasses;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDataChecker
{
    /// <summary>
    /// 拉线查等高线工具类
    /// </summary>
   public sealed class LineElevToolCls:BaseTool
    {
       //类的字段
       private IHookHelper m_HookHelper;
       private IMap m_Map;
       private IActiveView m_ActiveView;
       private IScreenDisplay m_ScreenDisplay;
       private INewLineFeedback m_NewLineFeedBack;
       ////private IMapControlDefault m_MapControl;
       Plugin.Application.IAppGISRef _AppHk;

       //类的构造函数
       public LineElevToolCls(Plugin.Application.IAppGISRef appHook)
       {
           _AppHk = appHook;

           base.m_caption = "拉线查等高线"; //localizable text 
           base.m_category = "GeoDataChecker";  //localizable text 
           base.m_message = "拉线查等高线";  //localizable text
           base.m_toolTip = "检查图面中所有等高线的高程是否标准，即是否是零高程或异常高程值（高程线的步进值是否是0.5的倍数）";  //localizable text
           base.m_name = base.m_category + "_" + base.m_caption;
       }
      

       #region Override Class Methods
       //创建Tool,初始化变量
       public override void OnCreate(object hook)
       {
           if (m_HookHelper == null)
           {
               m_HookHelper = new HookHelperClass();
           }
           m_HookHelper.Hook = hook;
           m_Map = m_HookHelper.FocusMap;
           m_ActiveView = m_HookHelper.ActiveView;
           m_ScreenDisplay = m_ActiveView.ScreenDisplay;
       }

       //鼠标在Map的事件
       public override void OnMouseDown(int Button, int Shift, int X, int Y)
       {
           IPoint pPt = m_ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
         
           if(m_NewLineFeedBack==null)
           {
               m_NewLineFeedBack = new NewLineFeedbackClass();

               ISimpleLineSymbol pSimLineSymbol = null;
               IRgbColor pRGBColor = null;

               pSimLineSymbol = m_NewLineFeedBack.Symbol as ISimpleLineSymbol;
               pRGBColor = new RgbColorClass();
               pRGBColor.Red = 255;
               pRGBColor.Blue = 0;
               pRGBColor.Green = 0;
               pSimLineSymbol.Color = pRGBColor as IColor;
               pSimLineSymbol.Style=esriSimpleLineStyle.esriSLSSolid;
               m_NewLineFeedBack.Display = m_ScreenDisplay;
               m_NewLineFeedBack.Start(pPt);
           }
           else 
           {
               m_NewLineFeedBack.AddPoint(pPt);
           }
       }
       //鼠标在Map上的双击事件
       public override void OnDblClick()
       {
           //获取当前所绘形状,将长度或者面积值添加到总和中
           //   双击停止feedback
           if(m_NewLineFeedBack!=null)
           {
               IPolyline pPolyLine = new PolylineClass();
               pPolyLine=m_NewLineFeedBack.Stop();
               
               //进行相关参数设置
               frmLineContourCheckSet pFrmLineCheckSet = new frmLineContourCheckSet(_AppHk, pPolyLine);
               pFrmLineCheckSet.ShowDialog();

               m_NewLineFeedBack = null;
           }
       }

       //鼠标在Map上的移动事件
       public override void OnMouseMove(int Button, int Shift, int X, int Y)
       {
           if (m_NewLineFeedBack != null)
           {
               IPoint pPnt = m_ScreenDisplay.DisplayTransformation.ToMapPoint(X, Y);
               m_NewLineFeedBack.MoveTo(pPnt);
           }
       }

       //刷屏事件
       public override void Refresh(int hDC)
       {
           if(m_NewLineFeedBack!=null)
           {
               m_NewLineFeedBack.Refresh(hDC);
           }
       }
       #endregion
   }
}
