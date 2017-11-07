using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.Controls;
using SysCommon.Error;
using ESRI.ArcGIS.Geodatabase;

namespace GeoDataCenterFunLib
{
    /// <summary>
    /// ���ߣ�yjl
    /// ���ڣ�20110730
    /// ˵����������ѯ
    /// </summary>
    public class ControlsQueryRiverCommand : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppArcGISRef _AppHk;
        FrmSQLQuery frmSQL = null;

        public ControlsQueryRiverCommand()
        {
            base._Name = "GeoDataCenterFunLib.ControlsQueryRiverCommand";
            base._Caption = "������ѯ";
            base._Tooltip = "������ѯ";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "������ѯ";
            //base._Image = "";
            //base._Category = "";
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
        public override bool Enabled
        {
          /*  get
            {
                //������ͼ����ϱ�ͼ�㡢���ݲ����������ڽ���ʱ������
                if (_AppHk.MapControl == null) return false;
                if (_AppHk.MapControl.Map.LayerCount == 0) return false;
                return true;
            }*/
            get
            {
                try
                {
                    if (_AppHk.CurrentControl is ISceneControl) return false;
                    if (_AppHk.MapControl.LayerCount == 0)
                    {
                        base._Enabled = false;
                        return false;
                    }

                    base._Enabled = true;
                    return true;
                }
                catch
                {
                    base._Enabled = false;
                    return false;
                }
            }
        }
        public override void OnClick()
        {
            if (_AppHk == null) return;
            if (_AppHk.MapControl == null) return;
            IMap pMap = _AppHk.MapControl.Map;
            if(frmSQL!=null)
            {
                frmSQL.Close();
            }
            if (this.WriteLog)
            {
                Plugin.LogTable.Writelog("������ѯ");//xisheng ��־��¼;
            }
            string strLayerName = "";
            string strFieldName="";//�����ֶ�
            string strFieldCode = "";//�����ֶ�
            IFeatureClass  pRiverFeaClass=null;
            try
            {//���Һ���������
                ModQuery.GetQueryConfig("������ѯ",out pRiverFeaClass, out strLayerName,out strFieldName, out strFieldCode);
                if (pRiverFeaClass == null)
                {
                    MessageBox.Show("�Ҳ�����������,���������ļ�!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                //�����������ֶ�
                if (pRiverFeaClass.FindField(strFieldName) < 0)
                {
                    MessageBox.Show("�Ҳ���������������,���������ļ�!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    pRiverFeaClass = null;
                    return ;
                }
                //�����������ֶ�
                if (pRiverFeaClass.FindField(strFieldCode) < 0)
                {
                    MessageBox.Show("�Ҳ���������������,���������ļ�!", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    pRiverFeaClass = null;
                    return;
                }
                frmQueryRoad fmQD = new frmQueryRoad((_AppHk as Plugin.Application.IAppFormRef).MainForm, _AppHk.MapControl, pRiverFeaClass, strLayerName,strFieldName, strFieldCode, "�� �� ����", "�������룺", "������ѯ");
                fmQD.WriteLog = this.WriteLog;
                fmQD.Show((_AppHk as Plugin.Application.IAppFormRef).MainForm);
                
            }
            catch (Exception ex)
            {
                ErrorHandle.ShowFrmErrorHandle("��ʾ", ex.Message);

            }
        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppArcGISRef;
            if (_AppHk.MapControl == null) return;
        }
    }
}