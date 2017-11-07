using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace GeoDataChecker
{
    class ControlsDBStructCheck : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;
        private System.Data.DataTable _Datatable;
        private GOGISDBStructErrorTreator _ErrorTreator;
        private GOGISStructErrorChecker _StructErrorChecker;

        public ControlsDBStructCheck()
        {
            base._Name = "GeoDataChecker.ControlsDBStructCheck";
            base._Caption = "数据属性结构检查";
            base._Tooltip = "检查数据中的图层名称与对应属性字段是否与标准中一致";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "数据属性结构检查";
            //base._Image = "";
            //base._Category = "";
        }

        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                try
                {
                    if (_AppHk.MapControl.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }
                    else
                    {
                        //if (SetCheckState.CheckState)
                        //{
                        base._Enabled = true;
                        return true;
                        //}
                        //else
                        //{
                        //    base._Enabled = false;
                        //    return false;
                        //}
                    }
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
            }
        }
        public override string Message
        {
            get
            {
                Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
                if (pAppFormRef != null)
                {
                    pAppFormRef.OperatorTips = base._Message;
                }
                return base._Message;
            }
        }

        public override void ClearMessage()
        {
            Plugin.Application.IAppFormRef pAppFormRef = _AppHk as Plugin.Application.IAppFormRef;
            if (pAppFormRef != null)
            {
                pAppFormRef.OperatorTips = string.Empty;
            }
        } 
              
        public override void OnClick()
        {

            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;

            #region 格网初始化与数据绑定
            _Datatable = new System.Data.DataTable();
            ///创建格网列结构
            ///
            _Datatable.Columns.Add("要素类名称", typeof(string));
            _Datatable.Columns.Add("错误类型", typeof(string));
            _Datatable.Columns.Add("错误描述", typeof(string));
            _Datatable.Columns.Add("检查时间", typeof(string));

            Plugin.Application.IAppGISRef hook = _AppHk as Plugin.Application.IAppGISRef;
            Plugin.Application.IAppFormRef pAppForm = _AppHk as Plugin.Application.IAppFormRef;

            hook.DataCheckGrid.DataSource = _Datatable; 
            
            #endregion

            this._StructErrorChecker = new GOGISStructErrorChecker();

            this._ErrorTreator = new GOGISDBStructErrorTreator();

            this._StructErrorChecker.FindErr += new GOGISErrorChecker.EventHandle(StructErrorChecker_FindErr);
            this._StructErrorChecker.ProgressStep += new GOGISErrorChecker.ProgressHandle(_StructErrorChecker_ProgressStep);

            ///一系列设置
            this._StructErrorChecker.DBSchemaDocPath = Application.StartupPath + "\\..\\Template\\DBSchema.mdb";

            
            pAppForm.ProgressBar.Visible = true;
            this._StructErrorChecker.ExcuteCheck(this._AppHk);
            pAppForm.ProgressBar.Visible = false;

            this._ErrorTreator.Dispose();
        }

        private void _StructErrorChecker_ProgressStep(object sender, int CurStep, int MaxValue)
        {
            Plugin.Application.IAppFormRef pAppForm = sender as Plugin.Application.IAppFormRef;
            pAppForm.ProgressBar.Maximum = MaxValue;
            pAppForm.ProgressBar.Minimum = 0;
            pAppForm.ProgressBar.Value = CurStep;
        }

        /// <summary>
        /// 发现错误后的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ErrorArg"></param>
        private void StructErrorChecker_FindErr(object sender, ErrorEventArgs ErrorArg)
        {

            ///用户界面上表现出错误信息
            /// 

            string[] ErrInfo = new string[4];
            ErrInfo[0] = ErrorArg.FeatureClassName;
            ErrInfo[1] = ErrorArg.ErrorName;
            ErrInfo[2] = ErrorArg.ErrDescription;
            ErrInfo[3] = System.DateTime.Now.ToString();
            _Datatable.Rows.Add(ErrInfo);

            Plugin.Application.IAppGISRef hook = sender as Plugin.Application.IAppGISRef;
            hook.DataCheckGrid.Update();
            
            ///调用错误处理类的函数,将检查到的错误信息，写入日志
            ///
            this._ErrorTreator.LogErr(sender, ErrorArg);
        }


        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;

        }
    }
}
