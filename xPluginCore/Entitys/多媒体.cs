using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace DNNGo.Modules.ThemePlugin
{
	/// <summary>
	/// 多媒体
	/// </summary>
	[Serializable]
	[DataObject]
	[Description("多媒体")]
	[BindTable("DNNGo_ThemePlugin_Multimedia", Description = "多媒体", ConnName = "SiteSqlServer")]
	public partial class DNNGo_ThemePlugin_Multimedia : Entity<DNNGo_ThemePlugin_Multimedia>
	{
		#region 属性
		private Int32 _ID;
		/// <summary>
		/// 媒体编号
		/// </summary>
		[Description("媒体编号")]
		[DataObjectField(true, true, false, 10)]
		[BindColumn("ID", Description = "媒体编号", DefaultValue = "", Order = 1)]
		public Int32 ID
		{
			get { return _ID; }
			set { if (OnPropertyChange("ID", value)) _ID = value; }
		}

		private String _Name;
		/// <summary>
		/// 名称
		/// </summary>
		[Description("名称")]
		[DataObjectField(false, false, true, 300)]
		[BindColumn("Name", Description = "名称", DefaultValue = "", Order = 2)]
		public String Name
		{
			get { return _Name; }
			set { if (OnPropertyChange("Name", value)) _Name = value; }
		}

		private Int32 _FileSize;
		/// <summary>
		/// 文件大小(KB)
		/// </summary>
		[Description("文件大小(KB)")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("FileSize", Description = "文件大小(KB)", DefaultValue = "0", Order = 3)]
		public Int32 FileSize
		{
			get { return _FileSize; }
			set { if (OnPropertyChange("FileSize", value)) _FileSize = value; }
		}

		private Int32 _ImageWidth;
		/// <summary>
		/// 图片的宽度
		/// </summary>
		[Description("图片的宽度")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("ImageWidth", Description = "图片的宽度", DefaultValue = "0", Order = 4)]
		public Int32 ImageWidth
		{
			get { return _ImageWidth; }
			set { if (OnPropertyChange("ImageWidth", value)) _ImageWidth = value; }
		}

		private Int32 _ImageHeight;
		/// <summary>
		/// 图片的高度
		/// </summary>
		[Description("图片的高度")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("ImageHeight", Description = "图片的高度", DefaultValue = "0", Order = 5)]
		public Int32 ImageHeight
		{
			get { return _ImageHeight; }
			set { if (OnPropertyChange("ImageHeight", value)) _ImageHeight = value; }
		}

		private String _Exif;
		/// <summary>
		/// 相片的扩展信息
		/// </summary>
		[Description("相片的扩展信息")]
		[DataObjectField(false, false, true, 1073741823)]
		[BindColumn("Exif", Description = "相片的扩展信息", DefaultValue = "", Order = 6)]
		public String Exif
		{
			get { return _Exif; }
			set { if (OnPropertyChange("Exif", value)) _Exif = value; }
		}

		private String _FileName;
		/// <summary>
		/// 文件名
		/// </summary>
		[Description("文件名")]
		[DataObjectField(false, false, false, 300)]
		[BindColumn("FileName", Description = "文件名", DefaultValue = "", Order = 7)]
		public String FileName
		{
			get { return _FileName; }
			set { if (OnPropertyChange("FileName", value)) _FileName = value; }
		}

		private String _FileMate;
		/// <summary>
		/// Mate
		/// </summary>
		[Description("Mate")]
		[DataObjectField(false, false, false, 32)]
		[BindColumn("FileMate", Description = "Mate", DefaultValue = "", Order = 8)]
		public String FileMate
		{
			get { return _FileMate; }
			set { if (OnPropertyChange("FileMate", value)) _FileMate = value; }
		}

		private String _FilePath;
		/// <summary>
		/// 文件路径
		/// </summary>
		[Description("文件路径")]
		[DataObjectField(false, false, false, 500)]
		[BindColumn("FilePath", Description = "文件路径", DefaultValue = "", Order = 9)]
		public String FilePath
		{
			get { return _FilePath; }
			set { if (OnPropertyChange("FilePath", value)) _FilePath = value; }
		}

		private String _FileExtension;
		/// <summary>
		/// 后缀名
		/// </summary>
		[Description("后缀名")]
		[DataObjectField(false, false, false, 32)]
		[BindColumn("FileExtension", Description = "后缀名", DefaultValue = "", Order = 10)]
		public String FileExtension
		{
			get { return _FileExtension; }
			set { if (OnPropertyChange("FileExtension", value)) _FileExtension = value; }
		}

		private DateTime _LastTime;
		/// <summary>
		/// 更新时间
		/// </summary>
		[Description("更新时间")]
		[DataObjectField(false, false, false, 23)]
		[BindColumn("LastTime", Description = "更新时间", DefaultValue = "", Order = 11)]
		public DateTime LastTime
		{
			get { return _LastTime; }
			set { if (OnPropertyChange("LastTime", value)) _LastTime = value; }
		}

		private Int32 _LastUser;
		/// <summary>
		/// 更新用户
		/// </summary>
		[Description("更新用户")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("LastUser", Description = "更新用户", DefaultValue = "", Order = 12)]
		public Int32 LastUser
		{
			get { return _LastUser; }
			set { if (OnPropertyChange("LastUser", value)) _LastUser = value; }
		}

		private String _LastIP;
		/// <summary>
		/// 更新IP
		/// </summary>
		[Description("更新IP")]
		[DataObjectField(false, false, false, 32)]
		[BindColumn("LastIP", Description = "更新IP", DefaultValue = "", Order = 13)]
		public String LastIP
		{
			get { return _LastIP; }
			set { if (OnPropertyChange("LastIP", value)) _LastIP = value; }
		}

		private Int32 _ModuleId;
		/// <summary>
		/// 模块编号
		/// </summary>
		[Description("模块编号")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("ModuleId", Description = "模块编号", DefaultValue = "", Order = 14)]
		public Int32 ModuleId
		{
			get { return _ModuleId; }
			set { if (OnPropertyChange("ModuleId", value)) _ModuleId = value; }
		}

		private Int32 _PortalId;
		/// <summary>
		/// 站点编号
		/// </summary>
		[Description("站点编号")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("PortalId", Description = "站点编号", DefaultValue = "", Order = 15)]
		public Int32 PortalId
		{
			get { return _PortalId; }
			set { if (OnPropertyChange("PortalId", value)) _PortalId = value; }
		}

		private Int16 _Status;
		/// <summary>
		/// 状态
		/// </summary>
		[Description("状态")]
		[DataObjectField(false, false, false, 3)]
		[BindColumn("Status", Description = "状态", DefaultValue = "1", Order = 16)]
		public Int16 Status
		{
			get { return _Status; }
			set { if (OnPropertyChange("Status", value)) _Status = value; }
		}

		private Int32 _Sort;
		/// <summary>
		/// 排序
		/// </summary>
		[Description("排序")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("Sort", Description = "排序", DefaultValue = "0", Order = 17)]
		public Int32 Sort
		{
			get { return _Sort; }
			set { if (OnPropertyChange("Sort", value)) _Sort = value; }
		}

		private Int16 _Extension1;
		/// <summary>
		/// 扩展字段1 (tinyint)
		/// </summary>
		[Description("扩展字段1 (tinyint)")]
		[DataObjectField(false, false, false, 3)]
		[BindColumn("Extension1", Description = "扩展字段1 (tinyint)", DefaultValue = "0", Order = 18)]
		public Int16 Extension1
		{
			get { return _Extension1; }
			set { if (OnPropertyChange("Extension1", value)) _Extension1 = value; }
		}

		private Int32 _Extension2;
		/// <summary>
		/// 扩展字段2 (int)
		/// </summary>
		[Description("扩展字段2 (int)")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("Extension2", Description = "扩展字段2 (int)", DefaultValue = "0", Order = 19)]
		public Int32 Extension2
		{
			get { return _Extension2; }
			set { if (OnPropertyChange("Extension2", value)) _Extension2 = value; }
		}

		private String _Extension3;
		/// <summary>
		/// 扩展字段3 (nvarchar512)
		/// </summary>
		[Description("扩展字段3 (nvarchar512)")]
		[DataObjectField(false, false, true, 512)]
		[BindColumn("Extension3", Description = "扩展字段3 (nvarchar512)", DefaultValue = "", Order = 20)]
		public String Extension3
		{
			get { return _Extension3; }
			set { if (OnPropertyChange("Extension3", value)) _Extension3 = value; }
		}

		private String _Extension4;
		/// <summary>
		/// 扩展字段4 (ntext)
		/// </summary>
		[Description("扩展字段4 (ntext)")]
		[DataObjectField(false, false, true, 1073741823)]
		[BindColumn("Extension4", Description = "扩展字段4 (ntext)", DefaultValue = "", Order = 21)]
		public String Extension4
		{
			get { return _Extension4; }
			set { if (OnPropertyChange("Extension4", value)) _Extension4 = value; }
		}
		#endregion

		#region 获取/设置 字段值
		/// <summary>
		/// 获取/设置 字段值。
		/// 一个索引，基类使用反射实现。
		/// 派生实体类可重写该索引，以避免反射带来的性能损耗
		/// </summary>
		/// <param name="name">字段名</param>
		/// <returns></returns>
		public override Object this[String name]
		{
			get
			{
				switch (name)
				{
					case "ID" : return _ID;
					case "Name" : return _Name;
					case "FileSize" : return _FileSize;
					case "ImageWidth" : return _ImageWidth;
					case "ImageHeight" : return _ImageHeight;
					case "Exif" : return _Exif;
					case "FileName" : return _FileName;
					case "FileMate" : return _FileMate;
					case "FilePath" : return _FilePath;
					case "FileExtension" : return _FileExtension;
					case "LastTime" : return _LastTime;
					case "LastUser" : return _LastUser;
					case "LastIP" : return _LastIP;
					case "ModuleId" : return _ModuleId;
					case "PortalId" : return _PortalId;
					case "Status" : return _Status;
					case "Sort" : return _Sort;
					case "Extension1" : return _Extension1;
					case "Extension2" : return _Extension2;
					case "Extension3" : return _Extension3;
					case "Extension4" : return _Extension4;
					default: return base[name];
				}
			}
			set
			{
				switch (name)
				{
					case "ID" : _ID = Convert.ToInt32(value); break;
					case "Name" : _Name = Convert.ToString(value); break;
					case "FileSize" : _FileSize = Convert.ToInt32(value); break;
					case "ImageWidth" : _ImageWidth = Convert.ToInt32(value); break;
					case "ImageHeight" : _ImageHeight = Convert.ToInt32(value); break;
					case "Exif" : _Exif = Convert.ToString(value); break;
					case "FileName" : _FileName = Convert.ToString(value); break;
					case "FileMate" : _FileMate = Convert.ToString(value); break;
					case "FilePath" : _FilePath = Convert.ToString(value); break;
					case "FileExtension" : _FileExtension = Convert.ToString(value); break;
					case "LastTime" : _LastTime = Convert.ToDateTime(value); break;
					case "LastUser" : _LastUser = Convert.ToInt32(value); break;
					case "LastIP" : _LastIP = Convert.ToString(value); break;
					case "ModuleId" : _ModuleId = Convert.ToInt32(value); break;
					case "PortalId" : _PortalId = Convert.ToInt32(value); break;
					case "Status" : _Status = Convert.ToInt16(value); break;
					case "Sort" : _Sort = Convert.ToInt32(value); break;
					case "Extension1" : _Extension1 = Convert.ToInt16(value); break;
					case "Extension2" : _Extension2 = Convert.ToInt32(value); break;
					case "Extension3" : _Extension3 = Convert.ToString(value); break;
					case "Extension4" : _Extension4 = Convert.ToString(value); break;
					default: base[name] = value; break;
				}
			}
		}
		#endregion

		#region 字段名
		/// <summary>
		/// 取得多媒体字段名的快捷方式
		/// </summary>
		public class _
		{
			///<summary>
			/// 媒体编号
			///</summary>
			public const String ID = "ID";

			///<summary>
			/// 名称
			///</summary>
			public const String Name = "Name";

			///<summary>
			/// 文件大小(KB)
			///</summary>
			public const String FileSize = "FileSize";

			///<summary>
			/// 图片的宽度
			///</summary>
			public const String ImageWidth = "ImageWidth";

			///<summary>
			/// 图片的高度
			///</summary>
			public const String ImageHeight = "ImageHeight";

			///<summary>
			/// 相片的扩展信息
			///</summary>
			public const String Exif = "Exif";

			///<summary>
			/// 文件名
			///</summary>
			public const String FileName = "FileName";

			///<summary>
			/// Mate
			///</summary>
			public const String FileMate = "FileMate";

			///<summary>
			/// 文件路径
			///</summary>
			public const String FilePath = "FilePath";

			///<summary>
			/// 后缀名
			///</summary>
			public const String FileExtension = "FileExtension";

			///<summary>
			/// 更新时间
			///</summary>
			public const String LastTime = "LastTime";

			///<summary>
			/// 更新用户
			///</summary>
			public const String LastUser = "LastUser";

			///<summary>
			/// 更新IP
			///</summary>
			public const String LastIP = "LastIP";

			///<summary>
			/// 模块编号
			///</summary>
			public const String ModuleId = "ModuleId";

			///<summary>
			/// 站点编号
			///</summary>
			public const String PortalId = "PortalId";

			///<summary>
			/// 状态
			///</summary>
			public const String Status = "Status";

			///<summary>
			/// 排序
			///</summary>
			public const String Sort = "Sort";

			///<summary>
			/// 扩展字段1 (tinyint)
			///</summary>
			public const String Extension1 = "Extension1";

			///<summary>
			/// 扩展字段2 (int)
			///</summary>
			public const String Extension2 = "Extension2";

			///<summary>
			/// 扩展字段3 (nvarchar512)
			///</summary>
			public const String Extension3 = "Extension3";

			///<summary>
			/// 扩展字段4 (ntext)
			///</summary>
			public const String Extension4 = "Extension4";
		}
		#endregion
	}
}