using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Carto;

namespace Fan.Common
{
    //ZQ 2011 1126 add
    public delegate void Action<T1, T2>(
            T1 arg1,
            T2 arg2);
    public class ScreenDraw
    {
        public static List<Action<IDisplay, esriViewDrawPhase>> list = new List<Action<IDisplay, esriViewDrawPhase>>();
    }

}
