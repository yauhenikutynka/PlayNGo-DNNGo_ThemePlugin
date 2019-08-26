using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace DNNGo.Modules.ThemePlugin
{
	/// <summary>
	/// 菜单行
	/// </summary>
	[Serializable]
	[DataObject]
	[Description("菜单行")]
	[BindTable("DNNGo_ThemePlugin_MenuRowItem", Description = "菜单行", ConnName = "SiteSqlServer")]
	public partial class DNNGo_ThemePlugin_MenuRowItem : Entity<DNNGo_ThemePlugin_MenuRowItem>
	{
		#region 属性
		private Int32 _ID;
		/// <summary>
		/// 行编号
		/// </summary>
		[Description("行编号")]
		[DataObjectField(true, true, false, 10)]
		[BindColumn("ID", Description = "行编号", DefaultValue = "", Order = 1)]
		public Int32 ID
		{
			get { return _ID; }
			set { if (OnPropertyChange("ID", value)) _ID = value; }
		}

		private Int32 _PaneID;
		/// <summary>
		/// 列编号
		/// </summary>
		[Description("列编号")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("PaneID", Description = "列编号", DefaultValue = "", Order = 2)]
		public Int32 PaneID
		{
			get { return _PaneID; }
			set { if (OnPropertyChange("PaneID", value)) _PaneID = value; }
		}


        private Int32 _TabID;
        /// <summary>
        /// 归属页面
        /// </summary>
        [Description("归属页面")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("TabID", Description = "归属页面", DefaultValue = "", Order = 3)]
        public Int32 TabID
        {
            get { return _TabID; }
            set { if (OnPropertyChange("TabID", value)) _TabID = value; }
        }

        private Int32 _Sort;
		/// <summary>
		/// 排序
		/// </summary>
		[Description("排序")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("Sort", Description = "排序", DefaultValue = "", Order = 4)]
		public Int32 Sort
		{
			get { return _Sort; }
			set { if (OnPropertyChange("Sort", value)) _Sort = value; }
		}

		private String _TagList;
		/// <summary>
		/// 标签名称
		/// </summary>
		[Description("标签名称")]
		[DataObjectField(false, false, false, 50)]
		[BindColumn("TagList", Description = "标签名称", DefaultValue = "", Order = 5)]
		public String TagList
		{
			get { return _TagList; }
			set { if (OnPropertyChange("TagList", value)) _TagList = value; }
		}


        private String _Title;
        /// <summary>
        /// 标题
        /// </summary>
        [Description("标题")]
        [DataObjectField(false, false, true, 300)]
        [BindColumn("Title", Description = "标题", DefaultValue = "", Order = 6)]
        public String Title
        {
            get { return _Title; }
            set { if (OnPropertyChange("Title", value)) _Title = value; }
        }

        private Int32 _RowType;
        /// <summary>
        /// 行类型(0.菜单/1.HTML/2.模块)
        /// </summary>
        [Description("行类型(0.菜单/1.HTML/2.模块)")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn("RowType", Description = "行类型(0.菜单/1.HTML/2.模块)", DefaultValue = "", Order = 7)]
        public Int32 RowType
        {
            get { return _RowType; }
            set { if (OnPropertyChange("RowType", value)) _RowType = value; }
        }

        private Int32 _BindTabID;
		/// <summary>
		/// 绑定页面
		/// </summary>
		[Description("绑定页面")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("BindTabID", Description = "绑定页面", DefaultValue = "", Order = 8)]
		public Int32 BindTabID
		{
			get { return _BindTabID; }
			set { if (OnPropertyChange("BindTabID", value)) _BindTabID = value; }
		}

		private Int32 _BindModuleID;
		/// <summary>
		/// 绑定模块
		/// </summary>
		[Description("绑定模块")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("BindModuleID", Description = "绑定模块", DefaultValue = "", Order = 9)]
		public Int32 BindModuleID
		{
			get { return _BindModuleID; }
			set { if (OnPropertyChange("BindModuleID", value)) _BindModuleID = value; }
		}

		private String _HTML_Title;
		/// <summary>
		/// HTML标题
		/// </summary>
		[Description("HTML标题")]
		[DataObjectField(false, false, false, 300)]
		[BindColumn("HTML_Title", Description = "HTML标题", DefaultValue = "", Order = 10)]
		public String HTML_Title
		{
			get { return _HTML_Title; }
			set { if (OnPropertyChange("HTML_Title", value)) _HTML_Title = value; }
		}

		private String _HTML_Content;
		/// <summary>
		/// HTML内容
		/// </summary>
		[Description("HTML内容")]
		[DataObjectField(false, false, false, 1073741823)]
		[BindColumn("HTML_Content", Description = "HTML内容", DefaultValue = "", Order = 11)]
		public String HTML_Content
		{
			get { return _HTML_Content; }
			set { if (OnPropertyChange("HTML_Content", value)) _HTML_Content = value; }
		}

		private Int32 _MenuID;
		/// <summary>
		/// 菜单编号
		/// </summary>
		[Description("菜单编号")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("MenuID", Description = "菜单编号", DefaultValue = "", Order = 12)]
		public Int32 MenuID
		{
			get { return _MenuID; }
			set { if (OnPropertyChange("MenuID", value)) _MenuID = value; }
		}

		private Int32 _MenuLevel;
		/// <summary>
		/// 菜单级别
		/// </summary>
		[Description("菜单级别")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("MenuLevel", Description = "菜单级别", DefaultValue = "", Order = 13)]
		public Int32 MenuLevel
		{
			get { return _MenuLevel; }
			set { if (OnPropertyChange("MenuLevel", value)) _MenuLevel = value; }
		}

		private Int32 _MenuDisplayTitle;
		/// <summary>
		/// 菜单显示标题
		/// </summary>
		[Description("菜单显示标题")]
		[DataObjectField(false, false, false, 3)]
		[BindColumn("MenuDisplayTitle", Description = "菜单显示标题", DefaultValue = "", Order = 14)]
		public Int32 MenuDisplayTitle
		{
			get { return _MenuDisplayTitle; }
			set { if (OnPropertyChange("MenuDisplayTitle", value)) _MenuDisplayTitle = value; }
		}

		private String _MenuSytle;
		/// <summary>
		/// 菜单样式
		/// </summary>
		[Description("菜单样式")]
		[DataObjectField(false, false, false, 100)]
		[BindColumn("MenuSytle", Description = "菜单样式", DefaultValue = "", Order = 15)]
		public String MenuSytle
		{
			get { return _MenuSytle; }
			set { if (OnPropertyChange("MenuSytle", value)) _MenuSytle = value; }
		}

		private String _Options;
		/// <summary>
		/// 选项集合
		/// </summary>
		[Description("选项集合")]
		[DataObjectField(false, false, false, 1073741823)]
		[BindColumn("Options", Description = "选项集合", DefaultValue = "", Order = 16)]
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
		[BindColumn("ModuleId", Description = "模块编号", DefaultValue = "", Order = 17)]
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
		[BindColumn("PortalId", Description = "站点编号", DefaultValue = "", Order = 18)]
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
		[BindColumn("LastTime", Description = "最后更新时间", DefaultValue = "", Order = 19)]
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
		[BindColumn("LastUser", Description = "最后更新用户", DefaultValue = "", Order = 20)]
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
		[BindColumn("LastIP", Description = "最后更新IP", DefaultValue = "", Order = 21)]
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
					case "PaneID" : return _PaneID;
                    case "TabID": return _TabID;
                    case "Sort" : return _Sort;
					case "TagList" : return _TagList;
                    case "Title": return _Title;
                    case "RowType" : return _RowType;
					case "BindTabID" : return _BindTabID;
					case "BindModuleID" : return _BindModuleID;
					case "HTML_Title" : return _HTML_Title;
					case "HTML_Content" : return _HTML_Content;
					case "MenuID" : return _MenuID;
					case "MenuLevel" : return _MenuLevel;
					case "MenuDisplayTitle" : return _MenuDisplayTitle;
					case "MenuSytle" : return _MenuSytle;
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
					case "PaneID" : _PaneID = Convert.ToInt32(value); break;
                    case "TabID": _TabID = Convert.ToInt32(value); break;
                    case "Sort" : _Sort = Convert.ToInt32(value); break;
					case "TagList" : _TagList = Convert.ToString(value); break;
                    case "Title": _Title = Convert.ToString(value); break;
                    case "RowType" : _RowType = Convert.ToInt32(value); break;
					case "BindTabID" : _BindTabID = Convert.ToInt32(value); break;
					case "BindModuleID" : _BindModuleID = Convert.ToInt32(value); break;
					case "HTML_Title" : _HTML_Title = Convert.ToString(value); break;
					case "HTML_Content" : _HTML_Content = Convert.ToString(value); break;
					case "MenuID" : _MenuID = Convert.ToInt32(value); break;
					case "MenuLevel" : _MenuLevel = Convert.ToInt32(value); break;
					case "MenuDisplayTitle" : _MenuDisplayTitle = Convert.ToInt32(value); break;
					case "MenuSytle" : _MenuSytle = Convert.ToString(value); break;
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
		/// 取得菜单行字段名的快捷方式
		/// </summary>
		public class _
		{
			///<summary>
			/// 行编号
			///</summary>
			public const String ID = "ID";

			///<summary>
			/// 列编号
			///</summary>
			public const String PaneID = "PaneID";

            ///<summary>
            /// 归属页面
            ///</summary>
            public const String TabID = "TabID";

            ///<summary>
            /// 排序
            ///</summary>
            public const String Sort = "Sort";

			///<summary>
			/// 标签名称
			///</summary>
			public const String TagList = "TagList";

            ///<summary>
            /// 标题
            ///</summary>
            public const String Title = "Title";

            ///<summary>
            /// 行类型(模块/HTML/菜单)
            ///</summary>
            public const String RowType = "RowType";

			///<summary>
			/// 绑定页面
			///</summary>
			public const String BindTabID = "BindTabID";

			///<summary>
			/// 绑定模块
			///</summary>
			public const String BindModuleID = "BindModuleID";

			///<summary>
			/// HTML标题
			///</summary>
			public const String HTML_Title = "HTML_Title";

			///<summary>
			/// HTML内容
			///</summary>
			public const String HTML_Content = "HTML_Content";

			///<summary>
			/// 菜单编号
			///</summary>
			public const String MenuID = "MenuID";

			///<summary>
			/// 菜单级别
			///</summary>
			public const String MenuLevel = "MenuLevel";

			///<summary>
			/// 菜单显示标题
			///</summary>
			public const String MenuDisplayTitle = "MenuDisplayTitle";

			///<summary>
			/// 菜单样式
			///</summary>
			public const String MenuSytle = "MenuSytle";

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