using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataChecker
{

    public static class CommonSet
    {
        private static string _ContourLinesAnnMatchRule = "";
        private static string _ContourLinesElevPointMatchRule = "";
        private static string _ControlPointAnnMatchRule = "";
        private static string _ElevationPointsAnnMatchRule = "";

        /// <summary>
        /// 等高线层与注记层匹配规则
        /// </summary>
        public static string ContourLinesAnnMatchRule
        {
            get
            {
                return _ContourLinesAnnMatchRule;
            }

            set
            {
                _ContourLinesAnnMatchRule = value;
            }

        }

        /// <summary>
        /// 等高线层与高程点层匹配规则
        /// </summary>
        public static string ContourLinesElevPointMatchRule
        {
            get
            {
                return _ContourLinesElevPointMatchRule;
            }

            set
            {
                _ContourLinesElevPointMatchRule = value;
            }
        }

        /// <summary>
        /// 控制点层与注记层匹配规则
        /// </summary>
        public static string ControlPointAnnMatchRule
        {
            get
            {
                return _ControlPointAnnMatchRule;
            }

            set
            {
                _ControlPointAnnMatchRule = value;
            }

        }

        /// <summary>
        /// 高程点与注记层匹配规则
        /// </summary>
        public static string ElevationPointsAnnMatchRule
        {
            get
            {
                return _ElevationPointsAnnMatchRule;
            }

            set
            {
                _ElevationPointsAnnMatchRule = value;
            }
        }



    }
}
