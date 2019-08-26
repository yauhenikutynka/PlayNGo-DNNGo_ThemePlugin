using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using DotNetNuke.Framework.Providers;


namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 实体类配置管理类
    /// </summary>
    internal class xConfig
    {
        private static Dictionary<Type, FieldItem[]> _Fields = new Dictionary<Type, FieldItem[]>();
        /// <summary>
        /// 取得指定类的帮定到数据表字段的属性。
        /// 某些数据字段可能只是用于展现数据，并不帮定到数据表字段，
        /// 区分的方法就是，DataObjectField属性是否为空。
        /// 静态缓存。
        /// </summary>
        /// <param name="t">实体类型</param>
        /// <returns>帮定到数据表字段的属性对象列表</returns>
        public static FieldItem[] Fields(Type t)
        {
            if (_Fields.ContainsKey(t)) return _Fields[t];
            lock (_Fields)
            {
                if (_Fields.ContainsKey(t)) return _Fields[t];

                List<FieldItem> cFields = new List<FieldItem>(AllFields(t));
                cFields = cFields.FindAll(delegate(FieldItem item) { return item.DataObjectField != null; });

                _Fields.Add(t, cFields.ToArray());
                return cFields.ToArray();
            }
        }

        private static Dictionary<Type, FieldItem[]> _AllFields = new Dictionary<Type, FieldItem[]>();
        /// <summary>
        /// 取得指定类的所有数据属性。
        /// 静态缓存。
        /// </summary>
        /// <param name="t">实体类型</param>
        /// <returns>所有数据属性对象列表</returns>
        public static FieldItem[] AllFields(Type t)
        {
            if (_AllFields.ContainsKey(t)) return _AllFields[t];
            lock (_AllFields)
            {
                if (_AllFields.ContainsKey(t)) return _AllFields[t];

                List<FieldItem> cFields = new List<FieldItem>();
                PropertyInfo[] pis = t.GetProperties();
                List<String> names = new List<String>();
                foreach (PropertyInfo item in pis)
                {
                    FieldItem field = new FieldItem();
                    field.Property = item;
                    field.Column = BindColumnAttribute.GetCustomAttribute(item);
                    field.DataObjectField = DataObjectAttribute.GetCustomAttribute(item, typeof(DataObjectFieldAttribute)) as DataObjectFieldAttribute;
                    cFields.Add(field);

                    if (names.Contains(item.Name)) throw new Exception(String.Format("{0}类中出现重复属性{1}", t.Name, item.Name));
                        names.Add(item.Name);
                }
                _AllFields.Add(t, cFields.ToArray());
                return cFields.ToArray();
            }
        }

        private static Dictionary<Type, TableMapAttribute[]> _AllTableMaps = new Dictionary<Type, TableMapAttribute[]>();
        /// <summary>
        /// 所有多表映射
        /// </summary>
        /// <param name="t">实体类型</param>
        /// <returns>所有多表映射列表</returns>
        public static TableMapAttribute[] AllTableMaps(Type t)
        {
            if (_AllTableMaps.ContainsKey(t)) return _AllTableMaps[t];
            lock (_AllTableMaps)
            {
                if (_AllTableMaps.ContainsKey(t)) return _AllTableMaps[t];
                List<TableMapAttribute> maps = new List<TableMapAttribute>();
                PropertyInfo[] pis = t.GetProperties();
                foreach (PropertyInfo pi in pis)
                {
                    TableMapAttribute table = TableMapAttribute.GetCustomAttribute(pi);
                    maps.Add(table);
                }
                _AllTableMaps.Add(t, maps.ToArray());
                return maps.ToArray();
            }
        }

        /// <summary>
        /// 查找指定类型的映射
        /// </summary>
        /// <param name="type"></param>
        /// <param name="jointypes"></param>
        /// <returns></returns>
        public static TableMapAttribute[] TableMaps(Type type, Type[] jointypes)
        {
            //取得所有映射关系
            List<Type> joinlist = new List<Type>(jointypes);
            //根据传入的实体类型列表来决定处理哪些多表关联
            List<TableMapAttribute> maps = new List<TableMapAttribute>();
            foreach (TableMapAttribute item in AllTableMaps(type))
            {
                Type t = joinlist.Find(delegate(Type elm) { return elm == item.MapEntity; });
                if (t != null)
                {
                    maps.Add(item);
                    joinlist.Remove(t);
                }
            }
            return maps.ToArray();
        }

        private const string providerType = "data";
        private static ProviderConfiguration providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);
     

        private static Dictionary<Type, BindTableAttribute> _Tables = new Dictionary<Type, BindTableAttribute>();
        /// <summary>
        /// 取得指定类的数据表。
        /// 静态缓存。
        /// </summary>
        /// <param name="t">实体类型</param>
        /// <returns>实体类绑定的数据表</returns>
        public static BindTableAttribute Table(Type t)
        {
            if (_Tables.ContainsKey(t)) return _Tables[t];
            lock (_Tables)
            {
                if (_Tables.ContainsKey(t)) return _Tables[t];

                BindTableAttribute table = BindTableAttribute.GetCustomAttribute(t);

                _Tables.Add(t, table);
                
                return table;
            }
        }

        /// <summary>
        /// 取得指定类的数据表名。
        /// 静态缓存。
        /// </summary>
        /// <param name="t">实体类型</param>
        /// <returns>实体类绑定的数据表</returns>
        public static String TableName(Type t)
        {
            BindTableAttribute table = Table(t);
            String str;
            if (table != null)
                str = string.Format("{0}.[{1}{2}]", TableOwner(t), TablePrefix(t), table.Name);
            else
                str = string.Format("{0}.[{1}{2}]", TableOwner(t), TablePrefix(t), t.Name);


            return str;
        }
        /// <summary>
        /// 获取指定类的表名前缀
        /// 静态缓存。
        /// </summary>
        /// <param name="t">实体类型</param>
        /// <returns>前缀名</returns>
        public static String TablePrefix(Type t)
        {
            BindTableAttribute table = Table(t);
            String str = String.Empty;
            if (table != null)
                str = table.Prefix;
            
            return str;
        }

        /// <summary>
        /// 获取指定类的所有者
        /// 静态缓存。
        /// </summary>
        /// <param name="t">实体类型</param>
        /// <returns>所有者</returns>
        public static String TableOwner(Type t)
        {
            BindTableAttribute table = Table(t);
            String str = String.Empty;
            if (table != null)
                str = table.Owner;

            return str;
        }




        private static Dictionary<Type, String> _ConnName = new Dictionary<Type, String>();
        /// <summary>
        /// 取得指定类的数据库连接名。
        /// 静态缓存。
        /// </summary>
        /// <param name="t">实体类型</param>
        /// <returns>实体类绑定的数据库连接名</returns>
        public static String ConnName(Type t)
        {
            BindTableAttribute table = Table(t);
            if (table != null)
                return table.ConnName;
            else
                return null;
        }

        private static Dictionary<Type, FieldItem[]> _Unique = new Dictionary<Type, FieldItem[]>();
        /// <summary>
        /// 唯一键
        /// 如果有标识列，则返回标识列集合；
        /// 否则，返回主键集合。
        /// </summary>
        /// <param name="t">实体类型</param>
        /// <returns>唯一键数组</returns>
        public static FieldItem[] Unique(Type t)
        {
            if (_Unique.ContainsKey(t)) return _Unique[t];
            lock (_Unique)
            {
                if (_Unique.ContainsKey(t)) return _Unique[t];

                FieldItem[] fs = Fields(t);
                List<FieldItem> list = new List<FieldItem>();
                foreach (FieldItem fi in fs)
                {
                    if (fi.DataObjectField.IsIdentity)
                    {
                        list.Add(fi);
                    }
                }
                if (list.Count < 1) // 没有标识列，使用主键
                {
                    foreach (FieldItem fi in fs)
                    {
                        if (fi.DataObjectField.PrimaryKey)
                        {
                            list.Add(fi);
                        }
                    }
                }
                _Unique.Add(t, list.ToArray());
                return list.ToArray();
            }
        }

        private static Dictionary<Type, String> _Selects = new Dictionary<Type, String>();
        /// <summary>
        /// 取得指定类对应的Select字句字符串。
        /// 静态缓存。
        /// </summary>
        /// <param name="t">实体类型</param>
        /// <returns>Select字句字符串</returns>
        public static String Selects(Type t)
        {
            if (_Selects.ContainsKey(t)) return _Selects[t];
            lock (_Selects)
            {
                if (_Selects.ContainsKey(t)) return _Selects[t];

                FieldItem[] fs = Fields(t);
                StringBuilder sbSelects = new StringBuilder();
                foreach (FieldItem fi in fs)
                {
                    if (sbSelects.Length > 0) sbSelects.Append(", ");
                    sbSelects.AppendFormat("{0}", fi.ColumnName);
                }
                String str = sbSelects.ToString();
                _Selects.Add(t, str);
                return str;
            }
        }

        private static Dictionary<Type, String> _SelectsEx = new Dictionary<Type, String>();
        /// <summary>
        /// 取得指定类对应的Select字句字符串。每个字段均带前缀。
        /// 静态缓存。
        /// </summary>
        /// <param name="t">实体类型</param>
        /// <returns>Select字句字符串</returns>
        public static String SelectsEx(Type t)
        {
            if (_SelectsEx.ContainsKey(t)) return _SelectsEx[t];
            lock (_SelectsEx)
            {
                if (_SelectsEx.ContainsKey(t)) return _SelectsEx[t];
                FieldItem[] fs = Fields(t);
                String prefix = ColumnPrefix(t);
                String tablename = TableName(t);
                StringBuilder sbSelects = new StringBuilder();
                foreach (FieldItem fi in fs)
                {
                    if (sbSelects.Length > 0) sbSelects.Append(", ");
                    sbSelects.AppendFormat("{0}.{1} as {2}{1}", tablename, fi.ColumnName, prefix);
                }
                String str = sbSelects.ToString();
                _SelectsEx.Add(t, str);
                return str;
            }
        }

        /// <summary>
        /// 取得字段前缀
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static String ColumnPrefix(Type t)
        {
            return String.Format("XCode_Map_{0}_", xConfig.TableName(t));
        }

        private static Dictionary<String, String> _SQL = new Dictionary<String, String>();
        /// <summary>
        /// 取得指定类的数据操作SQL模版。
        /// 静态缓存。
        /// </summary>
        /// <param name="t">实体类型</param>
        /// <param name="methodType">方法的数据操作类型</param>
        /// <returns></returns>
        public static String SQL(Type t, DataObjectMethodType methodType)
        {
            String mt = "Select";
            if (methodType == DataObjectMethodType.Fill) mt = "Fill";
            else if (methodType == DataObjectMethodType.Insert) mt = "Insert";
            else if (methodType == DataObjectMethodType.Update) mt = "Update";
            else if (methodType == DataObjectMethodType.Delete) mt = "Delete";
            // 因为methodType.ToString()非常耗资源，所以不再使用，估计内部是反射实现的
            String key = String.Format("{0}_{1}", t.FullName, mt);
            if (_SQL.ContainsKey(key)) return _SQL[key];
            lock (_SQL)
            {
                if (_SQL.ContainsKey(key)) return _SQL[key];

                String sql = "";
                FieldItem[] ps = Fields(t);
                StringBuilder sbNames = new StringBuilder();
                StringBuilder sbValues = new StringBuilder();
                Int32 i = 0;
                switch (methodType)
                {
                    case DataObjectMethodType.Fill:
                        //sql = String.Format("Select {0} From {1}", Selects(t), TableName(t));
                        sql = String.Format("Select * From {0}", TableName(t));
                        break;
                    case DataObjectMethodType.Select:
                        // 标识列作为查询关键字
                        foreach (FieldItem fi in ps)
                        {
                            if (fi.DataObjectField.IsIdentity)
                            {
                                //sql = String.Format("Select {0} From {1} Where {2}={{0}}", Selects(t), TableName(t), fi.ColumnName);
                                sql = String.Format("Select * From {0} Where {1}={{0}}", TableName(t), fi.ColumnName);
                                break;
                            }
                        }
                        break;
                    case DataObjectMethodType.Insert:
                        // 只读列没有插入操作
                        foreach (FieldItem fi in ps)
                        {
                            // 只有整型的标识列不需要插入，GUID和别的类型都需要
                            if (!fi.DataObjectField.IsIdentity || fi.Property.PropertyType != typeof(Int32))
                            {
                                if (i > 0) sbNames.Append(", "); // 加逗号
                                //sbNames.Append(fi.ColumnName);
                                sbNames.AppendFormat("{0}", fi.ColumnName);
                                if (i > 0) sbValues.Append(", "); // 加逗号
                                sbValues.Append("{" + i++ + "}"); // 位置必须递增
                            }
                        }
                        sql = String.Format("Insert Into {0}({1}) Values({2})", TableName(t), sbNames.ToString(), sbValues.ToString());
                        break;
                    case DataObjectMethodType.Update:
                        // 只读列没有更新操作
                        String keyColumn = null;
                        foreach (FieldItem fi in ps)
                        {
                            if (!fi.DataObjectField.IsIdentity)
                            {
                                if (i > 0) sbNames.Append(", "); // 加逗号
                                sbNames.AppendFormat("{0}=", fi.ColumnName);
                                sbNames.Append("{" + ++i + "}"); // 不能使用0开始，主键要用
                            }
                            else
                                keyColumn = String.Format("{0}={{0}}", fi.ColumnName);
                        }
                        if (String.IsNullOrEmpty(keyColumn)) break;
                        sql = String.Format("Update {0} Set {1} Where {2}", TableName(t), sbNames.ToString(), keyColumn);
                        break;
                    case DataObjectMethodType.Delete:
                        // 标识列作为删除关键字
                        foreach (FieldItem fi in ps)
                            if (fi.DataObjectField.IsIdentity) sql = String.Format("Delete From {0} Where {1}={{0}}", TableName(t), fi.ColumnName);
                        break;
                }
                _SQL.Add(key, sql);
                return sql;
            }
        }

        private static Dictionary<Type, DAL> _StaticDBO = new Dictionary<Type, DAL>();
        /// <summary>
        /// 取得指定类的静态数据操作对象。
        /// 静态缓存。
        /// </summary>
        /// <param name="t">实体类型</param>
        /// <returns>实体类绑定的数据操作对象</returns>
        public static DAL StaticDBO(Type t)
        {
            if (_StaticDBO.ContainsKey(t)) return _StaticDBO[t];
            lock (_StaticDBO)
            {
                if (_StaticDBO.ContainsKey(t)) return _StaticDBO[t];
                // 实体类的SessionDBO没有设置时会自动返回EntryDBO
                PropertyInfo pi = t.GetProperty("SessionDBO");
                DAL dal = null;
                if (pi != null)
                {
                    dal = pi.GetValue(null, null) as DAL;
                }
                _StaticDBO.Add(t, dal);
                return dal;
            }
        }
        ///// <summary>
        ///// 设置指定类的数据操作对象
        ///// </summary>
        ///// <param name="t">实体类型</param>
        ///// <param name="dbo">数据操作对象</param>
        //public static void SetEntryDBO(Type t, DAL dbo)
        //{
        //    lock (_StaticDBO)
        //    {
        //        if (_StaticDBO.ContainsKey(t))
        //            _StaticDBO[t] = dbo;
        //        else
        //            _StaticDBO.Add(t, dbo);
        //    }
        //}
    }
}