using System;
using System.Collections.Generic;
using System.ComponentModel;
 
namespace DNNGo.Modules.ThemePlugin
{
	/// <summary>
	/// 菜单参数表
	/// </summary>
	[Serializable]
	[DataObject]
	[Description("菜单参数表")]
	[BindTable("DNNGo_ThemePlugin_MenuOptions", Description = "菜单参数表", ConnName = "SiteSqlServer")]
	public partial class DNNGo_ThemePlugin_MenuOptions : Entity<DNNGo_ThemePlugin_MenuOptions>
	{
		#region 属性
		private Int32 _ID = 0;
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

		private Int32 _TabID = 0;
		/// <summary>
		/// 菜单编号
		/// </summary>
		[Description("菜单编号")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("TabID", Description = "菜单编号", DefaultValue = "", Order = 2)]
		public Int32 TabID
		{
			get { return _TabID; }
			set { if (OnPropertyChange("TabID", value)) _TabID = value; }
		}

		private Int32 _TabType = (Int32)EnumTabType.SliderNenu;
		/// <summary>
		/// 菜单类型
		/// </summary>
		[Description("菜单类型")]
		[DataObjectField(false, false, false, 3)]
		[BindColumn("TabType", Description = "菜单类型", DefaultValue = "", Order = 3)]
		public Int32 TabType
		{
			get { return _TabType; }
			set { if (OnPropertyChange("TabType", value)) _TabType = value; }
		}

		private String _Options = String.Empty;
		/// <summary>
		/// 参数集合
		/// </summary>
		[Description("参数集合")]
		[DataObjectField(false, false, false, 1073741823)]
		[BindColumn("Options", Description = "参数集合", DefaultValue = "", Order = 4)]
		public String Options
		{
			get { return _Options; }
			set { if (OnPropertyChange("Options", value)) _Options = value; }
		}

		private DateTime _LastTime = DateTime.Now;
		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Description("最后更新时间")]
		[DataObjectField(false, false, false, 23)]
		[BindColumn("LastTime", Description = "最后更新时间", DefaultValue = "", Order = 5)]
		public DateTime LastTime
		{
			get { return _LastTime; }
			set { if (OnPropertyChange("LastTime", value)) _LastTime = value; }
		}

		private Int32 _LastUser = 0;
		/// <summary>
		/// 最后更新用户
		/// </summary>
		[Description("最后更新用户")]
		[DataObjectField(false, false, false, 10)]
		[BindColumn("LastUser", Description = "最后更新用户", DefaultValue = "", Order = 6)]
		public Int32 LastUser
		{
			get { return _LastUser; }
			set { if (OnPropertyChange("LastUser", value)) _LastUser = value; }
		}

		private String _LastIP = String.Empty;
		/// <summary>
		/// 最后更新IP
		/// </summary>
		[Description("最后更新IP")]
		[DataObjectField(false, false, false, 32)]
		[BindColumn("LastIP", Description = "最后更新IP", DefaultValue = "", Order = 7)]
		public String LastIP
		{
			get { return _LastIP; }
			set { if (OnPropertyChange("LastIP", value)) _LastIP = value; }
		}

		private Int32 _ModuleId = 0;
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

		private Int32 _PortalId = 0;
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
					case "TabType" : return _TabType;
					case "Options" : return _Options;
					case "LastTime" : return _LastTime;
					case "LastUser" : return _LastUser;
					case "LastIP" : return _LastIP;
					case "ModuleId" : return _ModuleId;
					case "PortalId" : return _PortalId;
					default: return base[name];
				}
			}
			set
			{
				switch (name)
				{
					case "ID" : _ID = Convert.ToInt32(value); break;
					case "TabID" : _TabID = Convert.ToInt32(value); break;
					case "TabType" : _TabType = Convert.ToInt32(value); break;
					case "Options" : _Options = Convert.ToString(value); break;
					case "LastTime" : _LastTime = Convert.ToDateTime(value); break;
					case "LastUser" : _LastUser = Convert.ToInt32(value); break;
					case "LastIP" : _LastIP = Convert.ToString(value); break;
					case "ModuleId" : _ModuleId = Convert.ToInt32(value); break;
					case "PortalId" : _PortalId = Convert.ToInt32(value); break;
					default: base[name] = value; break;
				}
			}
		}
		#endregion

		#region 字段名
		/// <summary>
		/// 取得菜单参数表字段名的快捷方式
		/// </summary>
		public class _
		{
			///<summary>
			/// 编号
			///</summary>
			public const String ID = "ID";

			///<summary>
			/// 菜单编号
			///</summary>
			public const String TabID = "TabID";

			///<summary>
			/// 菜单类型
			///</summary>
			public const String TabType = "TabType";

			///<summary>
			/// 参数集合
			///</summary>
			public const String Options = "Options";

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

			///<summary>
			/// 模块编号
			///</summary>
			public const String ModuleId = "ModuleId";

			///<summary>
			/// 站点编号
			///</summary>
			public const String PortalId = "PortalId";
		}
		#endregion
	}
}