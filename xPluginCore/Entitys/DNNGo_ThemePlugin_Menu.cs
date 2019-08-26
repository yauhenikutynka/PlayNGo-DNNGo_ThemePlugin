using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace DNNGo.Modules.ThemePlugin
{
	/// <summary>
	/// 菜单
	/// </summary>
	[Serializable]
	[DataObject]
	[Description("菜单")]
	[BindTable("DNNGo_ThemePlugin_Menu", Description = "菜单", ConnName = "SiteSqlServer")]
	public partial class DNNGo_ThemePlugin_Menu : Entity<DNNGo_ThemePlugin_Menu>
	{
		#region 属性
		private Int32 _ID;
		/// <summary>
		/// 菜单编号
		/// </summary>
		[Description("菜单编号")]
		[DataObjectField(true, true, false, 10)]
		[BindColumn("ID", Description = "菜单编号", DefaultValue = "", Order = 1)]
		public Int32 ID
		{
			get { return _ID; }
			set { if (OnPropertyChange("ID", value)) _ID = value; }
		}

		private Int32 _TabID;
		/// <summary>
		/// DNN菜单
		/// </summary>
		[Description("DNN菜单")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("TabID", Description = "DNN菜单", DefaultValue = "", Order = 2)]
		public Int32 TabID
		{
			get { return _TabID; }
			set { if (OnPropertyChange("TabID", value)) _TabID = value; }
		}

		private String _Language;
		/// <summary>
		/// 语言
		/// </summary>
		[Description("语言")]
		[DataObjectField(false, false, false, 50)]
		[BindColumn("Language", Description = "语言", DefaultValue = "", Order = 3)]
		public String Language
		{
			get { return _Language; }
			set { if (OnPropertyChange("Language", value)) _Language = value; }
		}

		private Int32 _MenuType;
		/// <summary>
		/// 菜单类型
		/// </summary>
		[Description("菜单类型")]
		[DataObjectField(false, false, true, 3)]
		[BindColumn("MenuType", Description = "菜单类型", DefaultValue = "", Order = 4)]
		public Int32 MenuType
		{
			get { return _MenuType; }
			set { if (OnPropertyChange("MenuType", value)) _MenuType = value; }
		}

		private Int32 _MenuWidth;
		/// <summary>
		/// 菜单宽度
		/// </summary>
		[Description("菜单宽度")]
		[DataObjectField(false, false, true, 10)]
		[BindColumn("MenuWidth", Description = "菜单宽度", DefaultValue = "", Order = 5)]
		public Int32 MenuWidth
		{
			get { return _MenuWidth; }
			set { if (OnPropertyChange("MenuWidth", value)) _MenuWidth = value; }
		}

		private Int32 _Globals_Background =1;
		/// <summary>
		/// 全局背景
		/// </summary>
		[Description("全局背景")]
		[DataObjectField(false, false, false, 3)]
		[BindColumn("Globals_Background", Description = "全局背景", DefaultValue = "1", Order = 6)]
		public Int32 Globals_Background
		{
			get { return _Globals_Background; }
			set { if (OnPropertyChange("Globals_Background", value)) _Globals_Background = value; }
		}

		private Int32 _Globals_Breadcrumb =1;
		/// <summary>
		/// 全局面包屑
		/// </summary>
		[Description("全局面包屑")]
		[DataObjectField(false, false, false, 3)]
		[BindColumn("Globals_Breadcrumb", Description = "全局面包屑", DefaultValue = "1", Order = 7)]
		public Int32 Globals_Breadcrumb
		{
			get { return _Globals_Breadcrumb; }
			set { if (OnPropertyChange("Globals_Breadcrumb", value)) _Globals_Breadcrumb = value; }
		}

		private String _Options;
		/// <summary>
		/// 选项集合
		/// </summary>
		[Description("选项集合")]
		[DataObjectField(false, false, false, 1073741823)]
		[BindColumn("Options", Description = "选项集合", DefaultValue = "", Order = 8)]
		public String Options
		{
			get { return _Options; }
			set { if (OnPropertyChange("Options", value)) _Options = value; }
		}

		private Int32 _ModuleId;
		/// <summary>
		/// 模块编号
		/// </summary>
		[Description("模块编号")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("ModuleId", Description = "模块编号", DefaultValue = "", Order = 9)]
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
		[BindColumn("PortalId", Description = "站点编号", DefaultValue = "", Order = 10)]
		public Int32 PortalId
		{
			get { return _PortalId; }
			set { if (OnPropertyChange("PortalId", value)) _PortalId = value; }
		}

		private DateTime _LastTime;
		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Description("最后更新时间")]
		[DataObjectField(false, false, false, 23)]
		[BindColumn("LastTime", Description = "最后更新时间", DefaultValue = "", Order = 11)]
		public DateTime LastTime
		{
			get { return _LastTime; }
			set { if (OnPropertyChange("LastTime", value)) _LastTime = value; }
		}

		private Int32 _LastUser;
		/// <summary>
		/// 最后更新用户
		/// </summary>
		[Description("最后更新用户")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("LastUser", Description = "最后更新用户", DefaultValue = "", Order = 12)]
		public Int32 LastUser
		{
			get { return _LastUser; }
			set { if (OnPropertyChange("LastUser", value)) _LastUser = value; }
		}

		private String _LastIP;
		/// <summary>
		/// 最后更新IP
		/// </summary>
		[Description("最后更新IP")]
		[DataObjectField(false, false, false, 50)]
		[BindColumn("LastIP", Description = "最后更新IP", DefaultValue = "", Order = 13)]
		public String LastIP
		{
			get { return _LastIP; }
			set { if (OnPropertyChange("LastIP", value)) _LastIP = value; }
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
					case "TabID" : return _TabID;
					case "Language" : return _Language;
					case "MenuType" : return _MenuType;
					case "MenuWidth" : return _MenuWidth;
					case "Globals_Background" : return _Globals_Background;
					case "Globals_Breadcrumb" : return _Globals_Breadcrumb;
					case "Options" : return _Options;
					case "ModuleId" : return _ModuleId;
					case "PortalId" : return _PortalId;
					case "LastTime" : return _LastTime;
					case "LastUser" : return _LastUser;
					case "LastIP" : return _LastIP;
					default: return base[name];
				}
			}
			set
			{
				switch (name)
				{
					case "ID" : _ID = Convert.ToInt32(value); break;
					case "TabID" : _TabID = Convert.ToInt32(value); break;
					case "Language" : _Language = Convert.ToString(value); break;
					case "MenuType" : _MenuType = Convert.ToInt32(value); break;
					case "MenuWidth" : _MenuWidth = Convert.ToInt32(value); break;
					case "Globals_Background" : _Globals_Background = Convert.ToInt32(value); break;
					case "Globals_Breadcrumb" : _Globals_Breadcrumb = Convert.ToInt32(value); break;
					case "Options" : _Options = Convert.ToString(value); break;
					case "ModuleId" : _ModuleId = Convert.ToInt32(value); break;
					case "PortalId" : _PortalId = Convert.ToInt32(value); break;
					case "LastTime" : _LastTime = Convert.ToDateTime(value); break;
					case "LastUser" : _LastUser = Convert.ToInt32(value); break;
					case "LastIP" : _LastIP = Convert.ToString(value); break;
					default: base[name] = value; break;
				}
			}
		}
		#endregion

		#region 字段名
		/// <summary>
		/// 取得菜单字段名的快捷方式
		/// </summary>
		public class _
		{
			///<summary>
			/// 菜单编号
			///</summary>
			public const String ID = "ID";

			///<summary>
			/// DNN菜单
			///</summary>
			public const String TabID = "TabID";

			///<summary>
			/// 语言
			///</summary>
			public const String Language = "Language";

			///<summary>
			/// 菜单类型
			///</summary>
			public const String MenuType = "MenuType";

			///<summary>
			/// 菜单宽度
			///</summary>
			public const String MenuWidth = "MenuWidth";

			///<summary>
			/// 全局背景
			///</summary>
			public const String Globals_Background = "Globals_Background";

			///<summary>
			/// 全局面包屑
			///</summary>
			public const String Globals_Breadcrumb = "Globals_Breadcrumb";

			///<summary>
			/// 选项集合
			///</summary>
			public const String Options = "Options";

			///<summary>
			/// 模块编号
			///</summary>
			public const String ModuleId = "ModuleId";

			///<summary>
			/// 站点编号
			///</summary>
			public const String PortalId = "PortalId";

			///<summary>
			/// 最后更新时间
			///</summary>
			public const String LastTime = "LastTime";

			///<summary>
			/// 最后更新用户
			///</summary>
			public const String LastUser = "LastUser";

			///<summary>
			/// 最后更新IP
			///</summary>
			public const String LastIP = "LastIP";
		}
		#endregion
	}
}