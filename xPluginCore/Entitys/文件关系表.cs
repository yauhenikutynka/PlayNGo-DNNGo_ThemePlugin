using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 文件关系表
    /// </summary>
    [Serializable]
    [DataObject]
    [Description("文件关系表")]
    [BindTable("DNNGo_ThemePlugin_Relationships", Description = "文件关系表", ConnName = "SiteSqlServer")]
    public partial class DNNGo_ThemePlugin_Relationships : Entity<DNNGo_ThemePlugin_Relationships>
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

        private Int32 _TabID;
        /// <summary>
        /// 页面编号
        /// </summary>
        [Description("页面编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("TabID", Description = "页面编号", DefaultValue = "", Order = 2)]
        public Int32 TabID
        {
            get { return _TabID; }
            set { if (OnPropertyChange("TabID", value)) _TabID = value; }
        }

        private Int32 _FileID;
        /// <summary>
        /// 文件编号
        /// </summary>
        [Description("文件编号")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("FileID", Description = "文件编号", DefaultValue = "", Order = 3)]
        public Int32 FileID
        {
            get { return _FileID; }
            set { if (OnPropertyChange("FileID", value)) _FileID = value; }
        }

        private Int32 _Type;
        /// <summary>
        /// 关系类型
        /// </summary>
        [Description("关系类型")]
        [DataObjectField(false, false, false, 3)]
        [BindColumn("Type", Description = "关系类型", DefaultValue = "", Order = 4)]
        public Int32 Type
        {
            get { return _Type; }
            set { if (OnPropertyChange("Type", value)) _Type = value; }
        }

        private Int32 _Sort;
        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("Sort", Description = "排序", DefaultValue = "0", Order = 5)]
        public Int32 Sort
        {
            get { return _Sort; }
            set { if (OnPropertyChange("Sort", value)) _Sort = value; }
        }

        private DateTime _LastTime;
        /// <summary>
        /// 最后操作时间
        /// </summary>
        [Description("最后操作时间")]
        [DataObjectField(false, false, false, 23)]
        [BindColumn("LastTime", Description = "最后操作时间", DefaultValue = "getdate()", Order = 6)]
        public DateTime LastTime
        {
            get { return _LastTime; }
            set { if (OnPropertyChange("LastTime", value)) _LastTime = value; }
        }

        private Int32 _LastUser;
        /// <summary>
        /// 最后操作用户
        /// </summary>
        [Description("最后操作用户")]
        [DataObjectField(false, false, false, 10)]
        [BindColumn("LastUser", Description = "最后操作用户", DefaultValue = "0", Order = 7)]
        public Int32 LastUser
        {
            get { return _LastUser; }
            set { if (OnPropertyChange("LastUser", value)) _LastUser = value; }
        }

        private String _LastIP;
        /// <summary>
        /// 最后操作IP
        /// </summary>
        [Description("最后操作IP")]
        [DataObjectField(false, false, false, 32)]
        [BindColumn("LastIP", Description = "最后操作IP", DefaultValue = "127.0.0.1", Order = 8)]
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
        [BindColumn("ModuleId", Description = "模块编号", DefaultValue = "0", Order = 9)]
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
        [BindColumn("PortalId", Description = "站点编号", DefaultValue = "0", Order = 10)]
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
                    case "ID": return _ID;
                    case "TabID": return _TabID;
                    case "FileID": return _FileID;
                    case "Type": return _Type;
                    case "Sort": return _Sort;
                    case "LastTime": return _LastTime;
                    case "LastUser": return _LastUser;
                    case "LastIP": return _LastIP;
                    case "ModuleId": return _ModuleId;
                    case "PortalId": return _PortalId;
                    default: return base[name];
                }
            }
            set
            {
                switch (name)
                {
                    case "ID": _ID = Convert.ToInt32(value); break;
                    case "TabID": _TabID = Convert.ToInt32(value); break;
                    case "FileID": _FileID = Convert.ToInt32(value); break;
                    case "Type": _Type = Convert.ToInt32(value); break;
                    case "Sort": _Sort = Convert.ToInt32(value); break;
                    case "LastTime": _LastTime = Convert.ToDateTime(value); break;
                    case "LastUser": _LastUser = Convert.ToInt32(value); break;
                    case "LastIP": _LastIP = Convert.ToString(value); break;
                    case "ModuleId": _ModuleId = Convert.ToInt32(value); break;
                    case "PortalId": _PortalId = Convert.ToInt32(value); break;
                    default: base[name] = value; break;
                }
            }
        }
        #endregion

        #region 字段名
        /// <summary>
        /// 取得文件关系表字段名的快捷方式
        /// </summary>
        public class _
        {
            ///<summary>
            /// 编号
            ///</summary>
            public const String ID = "ID";

            ///<summary>
            /// 页面编号
            ///</summary>
            public const String TabID = "TabID";

            ///<summary>
            /// 文件编号
            ///</summary>
            public const String FileID = "FileID";

            ///<summary>
            /// 关系类型
            ///</summary>
            public const String Type = "Type";

            ///<summary>
            /// 排序
            ///</summary>
            public const String Sort = "Sort";

            ///<summary>
            /// 最后操作时间
            ///</summary>
            public const String LastTime = "LastTime";

            ///<summary>
            /// 最后操作用户
            ///</summary>
            public const String LastUser = "LastUser";

            ///<summary>
            /// 最后操作IP
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