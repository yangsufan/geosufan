using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Reflection;
using Fan.Plugin.Application;

namespace Fan.Plugin.Interface
{
    #region 插件接口定义

    //框架插件接口的基接口
    public interface IPlugin
    {
        string Name { get; }
        string Caption { get; }
        bool Visible { get; }
        bool Enabled { get; }
        void OnCreate(IApplicationRef hook);

    }
    //命令按钮接口插件
    public interface ICommandRef : IPlugin
    {
        //鼠标移动到按钮上时出现的文字
        string Tooltip { get;}
        //图标
        Image Image { get;}
        //所属类别
        string Category { get;}
        //是否被选择
        bool Checked { get;}
        //鼠标移动到按钮上时状态栏出现的文字
        string Message { get;}
        bool WriteLog { get; set; } 
        //鼠标从按钮上移走时清空状态栏文字
        void ClearMessage();
        //单击方法
        void OnClick();
    }
    //命令操作按钮接口插件
    public interface IToolRef : IPlugin
    {
        //鼠标移动到按钮上时出现的文字
        string Tooltip { get;}
        //图标
        Image Image { get;}
        //所属类别
        string Category { get;}
        //是否被选择
        bool Checked { get;}
        //鼠标移动到按钮上时状态栏出现的文字
        string Message { get;}
        //鼠标从按钮上移走时清空状态栏文字
        void ClearMessage();
        //鼠标的显示样式
        Cursor Cursor { get;}
        //激活状态设置
        bool Deactivate { get;}
        //单击响应事件
        void OnClick();
        //双击响应事件
        void OnDblClick();
        //右键菜单弹出响应事件
        bool OnContextMenu(int x, int y);
        //鼠标移动响应事件
        void OnMouseMove(int button, int shift, int x, int y);
        //鼠标按下响应事件
        void OnMouseDown(int button, int shift, int x, int y);
        //鼠标弹起响应事件
        void OnMouseUp(int button, int shift, int x, int y);
        //刷新响应事件
        void Refresh(int hDc);
        //键盘按钮按下响应事件
        void OnKeyDown(int keycode, int shift);
        //键盘按钮弹起响应事件
        void OnKeyUp(int keycode, int shift);
        bool WriteLog { get; set; }    
       
    }
    //菜单栏接口插件
    public interface IMenuRef : IPlugin
    {
        //菜单栏上的项数量
        long ItemCount { get;}
        //访问项方法
        void GetItemInfo(int pos, GetITemRef itemref);
    }
    //工具栏接口插件
    public interface IToolBarRef : IPlugin
    {
        //停靠的子控件
        Control ChildHWND { get;}
        //工具栏上的项数量
        long ItemCount { get;}
        //访问项方法
        void GetItemInfo(int pos, GetITemRef itemref);
    }
    #region 菜单栏或工具栏上的项
    public interface IITemRef
    {
        //是否是一新组
        bool Group { get;set;}
        //ID
        string ID { get;set;}
        //子项
        long SubType { get;set;}

    }
    public class GetITemRef : IITemRef
    {
        bool v_Group;
        string v_Id;
        long v_SubType;
        public bool Group
        {
            get
            {
                return this.v_Group;
            }
            set
            {
                this.v_Group = value;
            }
        }
        public string ID
        {
            get
            {
                return this.v_Id;
            }
            set
            {
                this.v_Id = value;
            }
        }
        public long SubType
        {
            get
            {
                return this.v_SubType;
            }
            set
            {
                this.v_SubType = value;
            }
        }
    }
    #endregion
    //浮动窗体接口插件
    public interface IDockableWindowRef : IPlugin
    {
        //停靠的子控件
        Control ChildHWND { get;}
        //关闭响应事件
        void OnDestroy();
    }
    //用户自定义控件接口插件
    public interface IControlRef : IPlugin
    {
        //加载数据方法
        void LoadData();
    }
    #endregion
    #region 插件实现抽象类定义
    public abstract class CommandRefBase : ICommandRef
    {
        protected string _Name;
        protected string _Caption;
        protected string _Tooltip;
        protected Image _Image;
        protected string _Category;
        protected bool _Checked;
        protected bool _Visible;
        protected bool _Enabled;
        protected string _Message;
        protected bool _Writelog;
        #region ICommandRef 成员
        public virtual string Name
        {
            get{ return _Name;}
        }
        public virtual string Caption
        {
            get{return _Caption;}
        }
        public virtual string Tooltip
        {
            get{return _Tooltip;}
        }
        public virtual Image Image
        {
            get{return _Image;}
        }
        public virtual string Category
        {
            get{return _Category;}
        }
        public virtual bool Checked
        {
            get{return _Checked;}
        }
        public virtual bool Visible
        {
            get{return _Visible;}
        }
        public virtual bool Enabled
        {
            get{return _Enabled;}
        }
        public virtual string Message
        {
            get{return _Message;}
        }
        public virtual bool WriteLog
        {
            get {return _Writelog;}
            set{_Writelog = value;}
        }
        public virtual void ClearMessage()
        {
        }
        public virtual void OnClick()
        {
        }
        public virtual void OnCreate(IApplicationRef hook)
        {
        }
        #endregion
    }
    public abstract class ToolRefBase : IToolRef
    {
        protected string _Name;
        protected string _Caption;
        protected string _Tooltip;
        protected Image _Image;
        protected string _Category;
        protected bool _Checked;
        protected bool _Visible;
        protected bool _Enabled;
        protected string _Message;
        protected Cursor _Cursor;
        protected bool _Deactivate;
        protected bool _Writelog;

        #region IToolRef 成员
        public virtual string Name
        {
            get
            {
                return _Name;
            }
        }
        public virtual string Caption
        {
            get
            {
                return _Caption;
            }
        }
        public virtual string Tooltip
        {
            get
            {
                return _Tooltip;
            }
        }
        public virtual Image Image
        {
            get
            {
                return _Image;
            }
        }
        public virtual string Category
        {
            get
            {
                return _Category;
            }
        }
        public virtual bool Checked
        {
            get
            {
                return _Checked;
            }
        }
        public virtual bool Visible
        {
            get
            {
                return _Visible;
            }
        }
        public virtual bool Enabled
        {
            get
            {
                return _Enabled;
            }
        }
        public virtual string Message
        {
            get
            {
                return _Message;
            }
        }
        public virtual void ClearMessage()
        {
        }
        public virtual Cursor Cursor
        {
            get
            {
                return _Cursor;
            }
        }
        public virtual bool Deactivate
        {
            get
            {
                return _Deactivate;
            }
        }
        public virtual void OnClick()
        {

        }
        public virtual void OnCreate(IApplicationRef hook)
        {

        }
        public virtual void OnDblClick()
        {

        }
        public virtual bool OnContextMenu(int x, int y)
        {
            return false;
        }
        public virtual void OnMouseMove(int button, int shift, int x, int y)
        {

        }
        public virtual void OnMouseDown(int button, int shift, int x, int y)
        {

        }
        public virtual void OnMouseUp(int button, int shift, int x, int y)
        {

        }
        public virtual void Refresh(int hDc)
        {

        }
        public virtual void OnKeyDown(int keycode, int shift)
        {

        }
        public virtual void OnKeyUp(int keycode, int shift)
        {

        }
        public virtual bool WriteLog
        {
            get
            {
                return _Writelog;
            }
            set 
            {
                _Writelog = value;
            }
        }
        #endregion
    }
    public abstract class MenuRefBase : IMenuRef
    {
        protected string _Name;
        protected string _Caption;
        protected bool _Visible;
        protected bool _Enabled;
        protected long _ItemCount;
        #region IMenuRef 成员
        public virtual string Name
        {
            get
            {
                return _Name;
            }
        }
        public virtual string Caption
        {
            get
            {
                return _Caption;
            }
        }
        public virtual bool Visible
        {
            get
            {
                return _Visible;
            }
        }
        public virtual bool Enabled
        {
            get
            {
                return _Enabled;
            }
        }
        public virtual long ItemCount
        {
            get
            {
                return _ItemCount;
            }
        }
        public virtual void GetItemInfo(int pos, GetITemRef itemref)
        {

        }
        public virtual void OnCreate(IApplicationRef hook)
        {

        }
        #endregion
    }
    public abstract class ToolBarRefBase : IToolBarRef
    {
        protected string _Name;
        protected string _Caption;
        protected Control _ChildHWND;
        protected bool _Visible;
        protected bool _Enabled;
        protected long _ItemCount;
        #region IToolBarRef 成员
        public virtual string Name
        {
            get
            {
                return _Name;
            }
        }
        public virtual string Caption
        {
            get
            {
                return _Caption;
            }
        }
        public virtual bool Visible
        {
            get
            {
                return _Visible;
            }
        }
        public virtual bool Enabled
        {
            get
            {
                return _Enabled;
            }
        }
        public virtual long ItemCount
        {
            get
            {
                return _ItemCount;
            }
        }
        public virtual Control ChildHWND
        {
            get
            {
                return _ChildHWND;
            }
        }
        public virtual void GetItemInfo(int pos, GetITemRef itemref)
        {

        }
        public virtual void OnCreate(IApplicationRef hook)
        {

        }
        #endregion
    }
    public abstract class DockableWindowRefBase : IDockableWindowRef
    {
        protected string _Name;
        protected string _Caption;
        protected Control _ChildHWND;
        protected object _UserData;
        protected bool _Visible;
        protected bool _Enabled;
        #region IDockableWindowRef 成员
        public virtual bool Visible
        {
            get { return _Visible; }
        }
        public virtual bool Enabled
        {
            get { return _Enabled; }
        }
        public virtual string Name
        {
            get
            {
                return _Name;
            }
        }
        public virtual string Caption
        {
            get
            {
                return _Caption;
            }
        }
        public virtual Control ChildHWND
        {
            get
            {
                return _ChildHWND;
            }
        }
        public virtual object UserData
        {
            get
            {
                return _UserData;
            }
        }
        public virtual void OnCreate(IApplicationRef hook)
        {

        }
        public virtual void OnDestroy()
        {

        }
        #endregion
    }
    public abstract class ControlRefBase : IControlRef
    {
        protected string _Name;
        protected string _Caption;
        protected bool _Visible;
        protected bool _Enabled;
        #region IControlRef 成员
        public virtual string Name
        {
            get
            {
                return _Name;
            }
        }
        public virtual string Caption
        {
            get
            {
                return _Caption;
            }
        }
        public virtual bool Visible
        {
            get
            {
                return _Visible;
            }
        }
        public virtual bool Enabled
        {
            get
            {
                return _Enabled;
            }
        }
        public virtual void OnCreate(IApplicationRef hook)
        {

        }
        public virtual void LoadData()
        {

        }
        #endregion
    }
    #endregion
}
