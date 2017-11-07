
using System;
using System.Runtime.InteropServices;

namespace GeoDBATool
{
	/// <summary>
	/// Version 的摘要说明。
	/// </summary>
	public class Version
	{
		/// <summary>
		/// 获取当前连接的版本信息列表
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_version_get_info_list( IntPtr pConn, string where_clause, out IntPtr version_list, out Int32 version_count );

		/// <summary>
		/// 释放当前连接的版本信息列表
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern void SE_version_free_info_list( Int32 version_count, IntPtr version_list );

		/// <summary>
		/// 获取指定版本的ID
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_versioninfo_get_id( IntPtr version_info, out Int32 version_id );

		/// <summary>
		/// 获取指定版本的名称
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_versioninfo_get_name( IntPtr version_info, [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 64)]char[] version_name );

		/// <summary>
		/// 获取指定版本的访问权限
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_versioninfo_get_access( IntPtr version_info, out Int32 version_access );

		/// <summary>
		/// 获取指定版本的父版本名称
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_versioninfo_get_parent_name( IntPtr version_info, [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 64)]char[] parent_name );

		/// <summary>
		/// 获取指定版本的创建时间
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_versioninfo_get_creation_time( IntPtr version_info, ref Layer.SE_TIME ctime );

		/// <summary>
		/// 获取指定版本的父版本名称
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_versioninfo_get_description( IntPtr version_info, [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 64)]char[] description );


	}
}
