﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GeoDataChecker
{
    //等高线检查
    public class GeoContourLineCheck : IDataCheckLogic
    {
        public event DataErrTreatHandle DataErrTreat;
        public event ProgressChangeHandle ProgressShow;
        private string Name;
        private IDataCheckHook Hook;

        public GeoContourLineCheck()
        {
            Name = "GeoDataChecker.GeoContourLineCheck";
        }

        #region IDataCheck 成员
        public void OnCreate(IDataCheckHook hook)
        {
            Hook = hook;
        }

        public void OnDataCheck()
        {
            GeoDataChecker.LoadDataCheckLogicFunction(Name, Hook, this);
        }

        #endregion

        #region IDataCheckLogic 成员

        public void DataCheckLogic_DataErrTreat(object sender, DataErrTreatEvent e)
        {
            DataErrTreat(sender, e);
        }

        #endregion

        /// <summary>
        /// 显示进度条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataCheck_ProgressShow(object sender, ProgressChangeEvent e)
        { }
    }
}
