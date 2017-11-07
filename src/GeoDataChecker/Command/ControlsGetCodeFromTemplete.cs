using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SCHEMEMANAGERCLASSESLib;
using System.IO;
using System.Data.OleDb;

namespace GeoDataChecker
{
    /// <summary>
    /// 检查CODE在我们标准的库体里面是否存在，如果存在则为正确，不存在则错
    /// 编写人：王冰
    /// </summary>
    class ControlsGetCodeFromTemplete : Plugin.Interface.CommandRefBase
    {
        private Plugin.Application.IAppGISRef _AppHk;
        private ISchemeProject m_pProject;


        public ControlsGetCodeFromTemplete()
        {
            base._Name = "GeoDataChecker.ControlsGetCodeFromTemplete";
            base._Caption = "获取分类代码信息";
            base._Tooltip = "从配置方案模板获取分类代码信息";
            base._Checked = false;
            base._Visible = true;
            base._Enabled = true;
            base._Message = "获取分类代码信息";
        }

        /// <summary>
        /// 图层中存在数据时并且状态为可用时才可用
        /// </summary>
        public override bool Enabled
        {
            get
            {
                if (File.Exists(Application.StartupPath + "\\..\\Template\\DBSchema.mdb") && File.Exists(Application.StartupPath + "\\..\\Res\\checker\\GeoCheckPara.mdb"))
                {
                    return true;
                }
                else
                {
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
            ///读取配置方案到对象
            ///
            m_pProject = new SchemeProjectClass();     //创建实例
            m_pProject.Load(Application.StartupPath + "\\..\\Template\\DBSchema.mdb", e_FileType.GO_SCHEMEFILETYPE_MDB);    //加载schema文件

            ///创建数据库连接
            OleDbConnection _con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +TopologyCheckClass.GeoDataCheckParaPath);
            //OleDbConnection _con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\..\\Res\\checker\\GeoCheckPara.mdb");
            _con.Open();

            if (m_pProject != null)
            {
                MyMap pmap = null;
                m_pProject.FindAttrValueFromTable("CODE", ref pmap);

                SchemeItem psi = null;
                int ItemCount = pmap.get_MapCount();
                string CodeValue;
                string FCName;
                string CodeName;
                string geoType;

                string pDelSql = "delete from GeoCheckCode";//GeoCheckerDataClassify
                OleDbCommand DelSqlCommand = new OleDbCommand(pDelSql, _con);
                DelSqlCommand.ExecuteNonQuery();


                for (int i = 0; i < ItemCount; i++)
                {
                    psi = pmap.get_ItemByIndex(i) as SchemeItem;
                    CodeName = psi.Name;
                    FCName = psi.ExtAttributeList.get_AttributeByName("ATTRTABLENAME").Value as string;
                    CodeValue = psi.ExtAttributeList.get_AttributeByName("CODE").Value as string;
                    geoType = psi.ExtAttributeList.get_AttributeByName("GEOTYPE").Value as string;

                    string pSql = "insert into GeoCheckCode(分类代码,类型,要素说明,图层) values('" + CodeValue + "','" + geoType + "','" + CodeName + "','" + FCName + "'" + ")";
                    OleDbCommand SqlCommand = new OleDbCommand(pSql, _con);
                    SqlCommand.ExecuteNonQuery();
                }
            }

            if (_con.State == System.Data.ConnectionState.Open)
            {
                _con.Close();
            }

        }

        public override void OnCreate(Plugin.Application.IApplicationRef hook)
        {
            if (hook == null) return;
            _AppHk = hook as Plugin.Application.IAppGISRef;
            if (_AppHk.MapControl == null) return;
        }



    }
}
