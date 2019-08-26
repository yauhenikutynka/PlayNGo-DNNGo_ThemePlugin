using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace DNNGo.Modules.ThemePlugin
{
	/// <summary>
	/// 菜单容器
	/// </summary>
	[Serializable]
	[DataObject]
	[Description("菜单容器")]
	[BindTable("DNNGo_ThemePlugin_MenuPane", Description = "菜单容器", ConnName = "SiteSqlServer")]
	public partial class DNNGo_ThemePlugin_MenuPane : Entity<DNNGo_ThemePlugin_MenuPane>
	{
		#region 属性
		private Int32 _ID;
		/// <summary>
		/// 编号
		/// </summary>
		[Description("编号")]
		[DataObjectField(true, true, false, 10)]
		[BindColumn("ID", Description = "编号", DefaultValue = "", Order = 1)]
		public Int32 ID
		{
			get { return _ID; }
			set { if (OnPropertyChange("ID", value)) _ID = value; }
		}

		private Int32 _Sort;
		/// <summary>
		/// 排序
		/// </summary>
		[Description("排序")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("Sort", Description = "排序", DefaultValue = "", Order = 2)]
		public Int32 Sort
		{
			get { return _Sort; }
			set { if (OnPropertyChange("Sort", value)) _Sort = value; }
		}

		private String _TagPane;
		/// <summary>
		/// 列标记
		/// </summary>
		[Description("列标记")]
		[DataObjectField(false, false, false, 50)]
		[BindColumn("TagPane", Description = "列标记", DefaultValue = "", Order = 3)]
		public String TagPane
		{
			get { return _TagPane; }
			set { if (OnPropertyChange("TagPane", value)) _TagPane = value; }
		}

		private String _PaneName;
		/// <summary>
		/// 列名称
		/// </summary>
		[Description("列名称")]
		[DataObjectField(false, false, false, 50)]
		[BindColumn("PaneName", Description = "列名称", DefaultValue = "", Order = 4)]
		public String PaneName
		{
			get { return _PaneName; }
			set { if (OnPropertyChange("PaneName", value)) _PaneName = value; }
		}

		private String _PaneWidth;
		/// <summary>
		/// 列宽度
		/// </summary>
		[Description("列宽度")]
		[DataObjectField(false, false, false, 100)]
		[BindColumn("PaneWidth", Description = "列宽度", DefaultValue = "", Order = 5)]
		public String PaneWidth
		{
			get { return _PaneWidth; }
			set { if (OnPropertyChange("PaneWidth", value)) _PaneWidth = value; }
		}

		private Int32 _TabID;
		/// <summary>
		/// DNN菜单
		/// </summary>
		[Description("DNN菜单")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("TabID", Description = "DNN菜单", DefaultValue = "", Order = 6)]
		public Int32 TabID
		{
			get { return _TabID; }
			set { if (OnPropertyChange("TabID", value)) _TabID = value; }
		}

		private String _Options;
		/// <summary>
		/// 选项集合
		/// </summary>
		[Description("选项集合")]
		[DataObjectField(false, false, false, 1073741823)]
		[BindColumn("Options", Description = "选项集合", DefaultValue = "", Order = 7)]
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
		[BindColumn("ModuleId", Description = "模块编号", DefaultValue = "", Order = 8)]
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
		[BindColumn("PortalId", Description = "站点编号", DefaultValue = "", Order = 9)]
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
		[BindColumn("LastTime", Description = "最后更新时间", DefaultValue = "", Order = 10)]
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
		[BindColumn("LastUser", Description = "最后更新用户", DefaultValue = "", Order = 11)]
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
		[BindColumn("LastIP", Description = "最后更新IP", DefaultValue = "", Order = 12)]
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
					case "Sort" : return _Sort;
					case "TagPane" : return _TagPane;
					case "PaneName" : return _PaneName;
					case "PaneWidth" : return _PaneWidth;
					case "TabID" : return _TabID;
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
					case "Sort" : _Sort = Convert.ToInt32(value); break;
					case "TagPane" : _TagPane = Convert.ToString(value); break;
					case "PaneName" : _PaneName = Convert.ToString(value); break;
					case "PaneWidth" : _PaneWidth = Convert.ToString(value); break;
					case "TabID" : _TabID = Convert.ToInt32(value); break;
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
		/// 取得菜单容器字段名的快捷方式
		/// </summary>
		public class _
		{
			///<summary>
			/// 编号
			///</summary>
			public const String ID = "ID";

			///<summary>
			/// 排序
			///</summary>
			public const String Sort = "Sort";

			///<summary>
			/// 列标记
			///</summary>
			public const String TagPane = "TagPane";

			///<summary>
			/// 列名称
			///</summary>
			public const String PaneName = "PaneName";

			///<summary>
			/// 列宽度
			///</summary>
			public const String PaneWidth = "PaneWidth";

			///<summary>
			/// DNN菜单
			///</summary>
			public const String TabID = "TabID";

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