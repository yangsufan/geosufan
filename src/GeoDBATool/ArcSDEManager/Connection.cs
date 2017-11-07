
using System;
using System.Runtime.InteropServices;

namespace GeoDBATool
{
	/// <summary>
	/// Connection 的摘要说明。
	/// </summary>
	public class Connection
	{

		#region 结构声明

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct SE_ERROR
		{
			public Int32 sde_error;
			public Int32 ext_error;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
			public char[] err_msg1;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
			public char[] err_msg2;
		}

		#endregion

		//////////
		// API声明
		///////////

		#region Connection

		/// <summary>
		/// 提交连接事务（SE_CONNECTION ）
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_connection_commit_transaction( IntPtr pSdeConn );

		/// <summary>
		/// 创建一个连接ArcSDE服务（SE_CONNECTION ）
		/// </summary>
        //[DllImport(".\\sde91.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
        [DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_connection_create(string server, string instance, string database, string username, string pwd, ref SE_ERROR error, out IntPtr pSdeConn);

		/// <summary>
		/// 关闭当前的ArcSDE服务连接（SE_CONNECTION ）
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern void  SE_connection_free( IntPtr pSdeConn );

		/// <summary>
		/// 解除当前SDE服务连接锁定（SE_CONNECTION ）
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern void  SE_connection_free_all_locks( IntPtr pSdeConn );

		/// <summary>
		/// 获得当前SDE服务连接的管理数据库名称（SE_CONNECTION ）
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_connection_get_admin_database( IntPtr pSdeConn, out string admin_databases );

		/// <summary>
		/// 获得当前SDE服务连接的数据库名称（SE_CONNECTION ）
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_connection_get_database( IntPtr pSdeConn, out string databases );

		/// <summary>
		/// 获得当前数据库的DBMS信息（SE_CONNECTION ）
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_connection_get_dbms_info( IntPtr pSdeConn, out Int32 dbms_id, out Int32 dbms_properties );

		/// <summary>
		/// 获得当前SDE服务的系统时间（SE_CONNECTION ）
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_connection_get_server_time( IntPtr pSdeConn );

		
		
		#endregion

		#region Instance

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct SE_INSTANCE_USER
		{
			public Int32 svrpid;
			public Int32 cstime;
			public bool xdr_needed;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
			public char[] sysname;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
			public char[] nodename;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
			public char[] username;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct SE_VERSION
		{
			public Int32 major;
			public Int32 minor;
			public Int32 bug;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
			public char[] desc;
			public Int32 release;
			public Int32 rel_low_supported;
		}


		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct SE_INSTANCE_STATUS
		{
			public SE_VERSION version;
			public Int32 connections;
			public Int32 system_mode;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct SE_INSTANCE_STATS
		{
			public Int32 pid;
			public Int32 rcount;
			public Int32 wcount;
			public Int32 opcount; 
			public Int32 numlocks;
			public Int32 fb_partial;
			public Int32 fb_count;
			public Int32 fb_fcount;
			public Int32 fb_kbytes;
		}

		/// <summary>
		/// 获得当前所有用户连接信息列表
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_instance_get_users( string server, string instance, out IntPtr user_list, out Int32 user_count );

		/// <summary>
		/// 释放用户连接信息列表
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern void SE_instance_free_users( ref SE_INSTANCE_USER[] user_list, Int32 user_count );

		/// <summary>
		/// 控制服务器上SDE服务的状态
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_instance_control( string strServer, string strInstance, string strPwd, Int32 iOption, Int32 iPid );

		/// <summary>
		/// 获得当前服务器的实例状态信息
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_instance_status( string strServer, string strInstance, ref SE_INSTANCE_STATUS pStatus );

		/// <summary>
		/// 启动当前服务器的实例
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_instance_start( string strSdeHome, string strPwd );

		/// <summary>
		/// 获得当前服务实例的统计信息列表
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_instance_get_statistics( string strServer, string strInstance, out IntPtr stats_list, out Int32 stats_count );

		/// <summary>
		/// 释放当前服务实例的统计信息列表
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern void SE_instance_free_statistics( ref SE_INSTANCE_STATS[] stats_list, Int32 stats_count );


		#endregion

	}
}
