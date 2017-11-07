using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataChecker
{
    /// <summary>
    /// 检查错误事件参数类
    /// </summary>
    public class ErrorEventArgs : EventArgs
    {
        private string _ErrorName;
        /// <summary>
        /// 错误类型名称
        /// </summary>
        public string ErrorName
        {
            get { return _ErrorName; }
            set { _ErrorName = value; }
        }

        private string _ErrDescription;
        /// <summary>
        /// 错误描述
        /// </summary>
        public string ErrDescription
        {
          get { return _ErrDescription; }
          set { _ErrDescription = value; }
        }
        private double _MapX;
        /// <summary>
        /// 错误定位点X坐标
        /// </summary>
        public double MapX
        {
            get { return _MapX; }
            set { _MapX = value; }
        }

        private double _MapY;
        /// <summary>
        /// 错误定位点Y坐标
        /// </summary>
        public double MapY
        {
            get { return _MapY; }
            set { _MapY = value; }
        }

        private string _FeatureClassName;
        /// <summary>
        /// 发现错误的要素类名称
        /// </summary>
        public string FeatureClassName
        {
            get { return _FeatureClassName; }
            set { _FeatureClassName = value; }
        }

        private long[] _OIDs;
        /// <summary>
        /// 发现错误的要素OID组
        /// </summary>
        public long[] OIDs
        {
            get { return _OIDs; }
            set { _OIDs = value; }
        }

        /// <summary>
        /// 检查时间
        /// </summary>
        private string pCheckTime;
        public string CheckTime
        {
            get
            {
                return pCheckTime;
            }
            set
            {
                pCheckTime = value;
            }
        }
    }
}
