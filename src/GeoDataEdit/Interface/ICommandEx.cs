using System;
using System.Collections.Generic;
using System.Text;



namespace GeoDataEdit
{
    public interface ICommandEx
    {
        bool GetInputMsg ( string sType , string strInfo );

        //用于得到右键菜单的Key值
        void GetMenuKey ( string sKey );

        //回置Tool里的变量
        void ResetMoveEvent ();
    }
}
