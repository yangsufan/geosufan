
using System;
using System.Runtime.InteropServices;

namespace GeoDBATool
{
	/// <summary>
	/// Layer 的摘要说明。
	/// </summary>
	public class Layer
	{
		#region API声明

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
			public struct SE_TIME
		{
			public Int32 tm_sec;     /* seconds after the minute - [0,59] */
			public Int32 tm_min;     /* minutes after the hour - [0,59] */
			public Int32 tm_hour;    /* hours since midnight - [0,23] */
			public Int32 tm_mday;    /* day of the month - [1,31] */
			public Int32 tm_mon;     /* months since January - [0,11] */
			public Int32 tm_year;    /* years since 1900 */
			public Int32 tm_wday;    /* days since Sunday - [0,6] */
			public Int32 tm_yday;    /* days since January 1 - [0,365] */
			public Int32 tm_isdst;   /* daylight savings time flag */
		}

		/// <summary>
		/// 获取当前连接的图层信息列表
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_layer_get_info_list( IntPtr pConn, out IntPtr layer_list, out Int32 layer_count );

		/// <summary>
		/// 释放当前连接的图层信息列表
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern void SE_layer_free_info_list( Int32 layer_count, IntPtr layer_list );

		/// <summary>
		/// 获取指定图层的ID
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_layerinfo_get_id( IntPtr layer_info, out Int32 layer_id );

		/// <summary>
		/// 获取指定图层的访问权限
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_layerinfo_get_access( IntPtr layer_info, out Int32 privileges );

		/// <summary>
		/// 获取指定图层的创建时间
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_layerinfo_get_creation_date( IntPtr layer_info, ref SE_TIME ctime );

		/// <summary>
		/// 获取指定图层的空间几何类型
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_layerinfo_get_shape_types( IntPtr layer_info, out Int32 shape_types );

		/// <summary>
		/// 获取指定图层的名称和几何字段名
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_layerinfo_get_spatial_column(IntPtr layerinfo, [Out, MarshalAs(UnmanagedType.LPArray, 
					SizeConst = 1808)]char[] layername, [Out, MarshalAs(UnmanagedType.LPArray, SizeConst = 256)]char[] column);

		/// <summary>
		/// 获取指定图层的存储类型
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern Int32 SE_layerinfo_get_storage_type( IntPtr layer_info, out Int32 storage_type );

		/// <summary>
		/// 指定图层是否存在空间索引
		/// </summary>
		[DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
		public static extern bool SE_layerinfo_has_spatial_index( IntPtr layer_info );

        /// <summary>
        /// 获取图层的空间参考
        /// </summary>
        [DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
        public static extern Int32 SE_layerinfo_get_coordref(IntPtr layer_info, out IntPtr coordref);

        /// <summary>
        /// 获取图层的空间坐标描述
        /// </summary>
        [DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
        public static extern Int32 SE_coordref_get_description(IntPtr coordref, [Out, MarshalAs(UnmanagedType.LPArray,
                      SizeConst = 1808)]char[] coordescription);

        /// <summary>
        /// 获取图层描述
        /// </summary>
        [DllImport(".\\sde.dll", SetLastError = true, ThrowOnUnmappableChar = true)]
        public static extern Int32 SE_layerinfo_get_description(IntPtr layer_info, [Out, MarshalAs(UnmanagedType.LPArray,
                    SizeConst = 1808)]char[] description);

		#endregion
	}
}
