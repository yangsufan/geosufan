using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataChecker
{
    //数据检查错误处理委托事件定义
    public delegate void DataErrTreatHandle(object sender, DataErrTreatEvent e);
    public class DataErrTreatEvent : EventArgs
    {
        //数据检查错误描述内容
        private IDataErrInfo DataErrInfo;
        public IDataErrInfo ErrInfo
        {
            get
            {
                return DataErrInfo;
            }
        }

        //  默认构造函数
        public DataErrTreatEvent()
        {
        }

        //  实际构造函数
        public DataErrTreatEvent(IDataErrInfo dataErrInfo)
        {
            DataErrInfo = dataErrInfo;
        }
    }
    public delegate void ProgressChangeHandle(object sender,ProgressChangeEvent e);
    public class ProgressChangeEvent : EventArgs
    {
        private int pMax;
        private int pValue;
        public int Max
        {
            get
            {
                return pMax;
            }
            set
            {
                pMax = value;
            }
        }
        public int Value
        {
            get
            {
                return pValue;
            }
            set
            {
                pValue = value;
            }
        }
    }

    //数据检查错误描述内容
    public interface IDataErrInfo
    {
        //执行功能组名称
        string FunctionCategoryName { get;set;}

        //执行功能名称
        string FunctionName { get;set;}

        //数据文件名称
        string DataFileName { get;set;}

        //错误类型ID
        int ErrID { get;set;}

        //错误描述
        string ErrDescription { get;set;}

        //错误定位点X坐标
        double MapX { get;set;}

        //错误定位点Y坐标
        double MapY { get;set;}

        //错误源数据所在要素类名
        string OriginClassName { get;set;}

        //错误源数据要素OID
        int OriginFeatOID { get;set;}

        //错误目标数据所在要素类名
        string DestinationClassName { get;set;}

        //错误目标数据要素OID
        int DestinationFeatOID { get;set;}

        //是否修改
        bool Modified { get;set;}

        //检查时间
        string OperatorTime { get;set;}
    }
    public class DataErrInfo : IDataErrInfo
    {
        private List<object> ErrInfo;
        public DataErrInfo(List<object> errInfo)
        {
            if (errInfo.Count != 13) return;
            ErrInfo = errInfo;
        }

        #region IDataErrInfo 成员

        public string FunctionCategoryName
        {
            get
            {
                return ErrInfo[0].ToString();
            }
            set
            {
                ErrInfo[0] = value;
            }
        }

        public string FunctionName
        {
            get
            {
                return ErrInfo[1].ToString();
            }
            set
            {
                ErrInfo[1] = value;
            }
        }

        public string DataFileName
        {
            get
            {
                return ErrInfo[2].ToString();
            }
            set
            {
                ErrInfo[2] = value;
            }
        }

        public int ErrID
        {
            get
            {
                return Convert.ToInt32(ErrInfo[3]);
            }
            set
            {
                ErrInfo[3] = value;
            }
        }

        public string ErrDescription
        {
            get
            {
                return ErrInfo[4].ToString();
            }
            set
            {
                ErrInfo[4] = value;
            }
        }

        public double MapX
        {
            get
            {
                return Convert.ToDouble(ErrInfo[5]);
            }
            set
            {
                ErrInfo[5] = value;
            }
        }

        public double MapY
        {
            get
            {
                return Convert.ToDouble(ErrInfo[6]);
            }
            set
            {
                ErrInfo[6] = value;
            }
        }

        public string OriginClassName
        {
            get
            {
                return ErrInfo[7].ToString();
            }
            set
            {
                ErrInfo[7] = value;
            }
        }

        public int OriginFeatOID
        {
            get
            {
                return Convert.ToInt32(ErrInfo[8]);
            }
            set
            {
                ErrInfo[8] = value;
            }
        }

        public string DestinationClassName
        {
            get
            {
                return ErrInfo[9].ToString();
            }
            set
            {
                ErrInfo[9] = value;
            }
        }

        public int DestinationFeatOID
        {
            get
            {
                return Convert.ToInt32(ErrInfo[10]);
            }
            set
            {
                ErrInfo[10] = value;
            }
        }

        public bool Modified
        {
            get
            {
                return Convert.ToBoolean(ErrInfo[11]);
            }
            set
            {
                ErrInfo[11] = value;
            }
        }

        public string OperatorTime
        {
            get
            {
                return ErrInfo[12].ToString();
            }
            set
            {
                ErrInfo[12] = value;
            }
        }

        #endregion
    }
}
