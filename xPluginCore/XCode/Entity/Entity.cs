using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Globalization;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 数据实体类基类。所有数据实体类都必须继承该类。
    /// </summary>
    [Serializable]
    public class Entity<TEntity> : ICloneable where TEntity : Entity<TEntity>, new()
    {
        #region 元数据
        /// <summary>
        /// 元数据
        /// </summary>
        public static class Meta
        {
            /// <summary>
            /// 实体类型
            /// </summary>
            public static Type ThisType { get { return typeof(TEntity); } }

            /// <summary>
            /// 实体链接名
            /// </summary>
            public static String ConnName { get { return xConfig.ConnName(ThisType); } }

            /// <summary>
            /// 表名
            /// </summary>
            public static String TableName { get { return xConfig.TableName(ThisType); } }

            /// <summary>
            /// 所有数据属性
            /// </summary>
            public static FieldItem[] AllFields { get { return xConfig.AllFields(ThisType); } }

            /// <summary>
            /// 所有绑定到数据表的属性
            /// </summary>
            public static FieldItem[] Fields { get { return xConfig.Fields(ThisType); } }

            private static List<String> _FieldNames;
            /// <summary>
            /// 字段名列表
            /// </summary>
            public static List<String> FieldNames
            {
                get
                {
                    if (_FieldNames != null) return _FieldNames;
                    lock (typeof(Meta))
                    {
                        if (_FieldNames != null) return _FieldNames;

                        _FieldNames = new List<String>();
                        //Fields.ForEach(delegate(FieldItem item) { if (!_FieldNames.Contains(item.Name))_FieldNames.Add(item.Name); });
                        foreach (FieldItem item in Fields)
                        {
                            if (!_FieldNames.Contains(item.Name)) _FieldNames.Add(item.Name);
                        }
                        return _FieldNames;
                    }
                }
            }

            /// <summary>
            /// 唯一键
            /// </summary>
            public static FieldItem[] Uniques { get { return xConfig.Unique(ThisType); } }

            /// <summary>
            /// 唯一键
            /// </summary>
            public static FieldItem Unique
            {
                get
                {
                    FieldItem[] fis = Uniques;
                    if (fis == null || fis.Length < 1) return null;
                    foreach (FieldItem item in fis)
                    {
                        if (item.DataObjectField != null && item.DataObjectField.IsIdentity) return item;
                    }
                    if (fis.Length == 1) return fis[0];
                    return null;
                }
            }

            /// <summary>
            /// 取得字段前缀
            /// </summary>
            public static String ColumnPrefix { get { return xConfig.ColumnPrefix(ThisType); } }

            /// <summary>
            /// 取得指定类对应的Select字句字符串。
            /// </summary>
            public static String Selects { get { return xConfig.Selects(ThisType); } }

            /// <summary>
            /// 数据操作对象。设为私有，是为了保证能够拦截所有更新数据的操作，以便清除实体缓存
            /// </summary>
            private static DAL DBO { get { return DAL.Create(Meta.ConnName); } }

            #region DAL操作
            /// <summary>
            /// 数据库类型
            /// </summary>
            public static DatabaseType DbType { get { return DBO.DbType; } }

            /// <summary>
            /// 查询
            /// </summary>
            /// <param name="sql">SQL语句</param>
            /// <returns>结果记录集</returns>
            public static DataSet Query(String sql)
            {
                return DBO.Select(sql, Meta.TableName);
            }

            /// <summary>
            /// 查询
            /// </summary>
            /// <param name="sql">SQL语句</param>
            /// <param name="tableNames">所依赖的表的表名</param>
            /// <returns>结果记录集</returns>
            public static DataSet Query(String sql, String[] tableNames)
            {
                return DBO.Select(sql, tableNames);
            }

            /// <summary>
            /// 查询记录数
            /// </summary>
            /// <param name="sql">SQL语句</param>
            /// <returns>记录数</returns>
            public static Int32 QueryCount(String sql)
            {
                return DBO.SelectCount(sql, Meta.TableName);
            }

            /// <summary>
            /// 执行
            /// </summary>
            /// <param name="sql">SQL语句</param>
            /// <returns>影响的结果</returns>
            public static Int32 Execute(String sql)
            {
                Int32 rs = DBO.Execute(sql, Meta.TableName);
                Cache.Clear();
                return rs;
            }


            /// <summary>
            /// 执行
            /// </summary>
            /// <param name="sql">SQL语句</param>
            /// <returns>影响的结果</returns>
            public static Object ExecuteScalar(String sql)
            {
                Object rs = DBO.ExecuteScalar(sql, Meta.TableName);
                Cache.Clear();
                return rs;
            }

            /// <summary>
            /// 执行插入语句并返回新增行的自动编号
            /// </summary>
            /// <param name="sql">SQL语句</param>
            /// <returns>新增行的自动编号</returns>
            public static Int32 InsertAndGetIdentity(String sql)
            {
                Int32 rs = DBO.InsertAndGetIdentity(sql, Meta.TableName);
                Cache.Clear();
                return rs;
            }

            /// <summary>
            /// 根据条件把普通查询SQL格式化为分页SQL。
            /// </summary>
            /// <param name="sql">SQL语句</param>
            /// <param name="startRowIndex">开始行，0开始</param>
            /// <param name="maximumRows">最大返回行数</param>
            /// <param name="keyColumn">唯一键。用于not in分页</param>
            /// <returns>分页SQL</returns>
            public static String PageSplit(String sql, Int32 startRowIndex, Int32 maximumRows, String keyColumn)
            {
                return DBO.PageSplit(sql, startRowIndex, maximumRows, keyColumn);
            }
            #endregion

            /// <summary>
            /// 实体缓存
            /// </summary>
            public static EntityCache<TEntity> Cache = new EntityCache<TEntity>();
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 静态构造
        /// </summary>
        static Entity()
        {
            CheckModify();
        }
        #endregion

        #region 填充数据
        /// <summary>
        /// 加载记录集
        /// </summary>
        /// <param name="ds">记录集</param>
        /// <returns>实体数组</returns>
        public static List<TEntity> LoadData(DataSet ds)
        {
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1) return new List<TEntity>();
            return LoadData(ds.Tables[0]);
        }

        /// <summary>
        /// 加载数据表
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <returns>实体数组</returns>
        protected static List<TEntity> LoadData(DataTable dt)
        {
            if (dt == null || dt.Rows.Count < 1) return new List<TEntity>();
            return LoadData(dt, null);
        }

        /// <summary>
        /// 加载数据表
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="jointypes"></param>
        /// <returns>实体数组</returns>
        protected static List<TEntity> LoadData(DataTable dt, Type[] jointypes)
        {
            if (dt == null || dt.Rows.Count < 1) return null;
            List<TEntity> list = new List<TEntity>(dt.Rows.Count);
            String prefix = null;
            TableMapAttribute[] maps = null;
            Boolean hasprefix = false;
            if (jointypes != null && jointypes.Length > 0)
            {
                maps = xConfig.TableMaps(Meta.ThisType, jointypes);
                prefix = Meta.ColumnPrefix;
                hasprefix = true;
            }
            List<FieldItem> ps = CheckColumn(dt, prefix);
            foreach (DataRow dr in dt.Rows)
            {
                TEntity obj = new TEntity();
                obj.LoadData(dr, hasprefix, ps.ToArray(), maps);
                list.Add(obj);
            }
            return list;
        }

        /// <summary>
        /// 从一个数据行对象加载数据。不加载关联对象。
        /// </summary>
        /// <param name="dr">数据行</param>
        public virtual void LoadData(DataRow dr)
        {
            if (dr == null) return;
            LoadData(dr, null);
        }

        /// <summary>
        /// 从一个数据行对象加载数据。指定要加载哪些关联的实体类对象。
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <param name="jointypes">多表关联</param>
        protected virtual void LoadData(DataRow dr, Type[] jointypes)
        {
            if (dr == null) return;
            String prefix = null;
            TableMapAttribute[] maps = null;
            Boolean hasprefix = false;
            if (jointypes != null && jointypes.Length > 0)
            {
                maps = xConfig.TableMaps(Meta.ThisType, jointypes);
                prefix = Meta.ColumnPrefix;
                hasprefix = true;
            }
            List<FieldItem> ps = CheckColumn(dr.Table, prefix);
            LoadData(dr, hasprefix, ps.ToArray(), maps);
        }

        /// <summary>
        /// 从一个数据行对象加载数据。带前缀。
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <param name="ps">要加载数据的字段</param>
        /// <returns></returns>
        protected virtual void LoadDataWithPrefix(DataRow dr, FieldItem[] ps)
        {
            if (dr == null) return;
            if (ps == null || ps.Length < 1) ps = Meta.Fields;
            String prefix = Meta.ColumnPrefix;
            foreach (FieldItem fi in ps)
            {
                // 两次dr[fi.ColumnName]简化为一次
                Object v = dr[prefix + fi.ColumnNameEx];
                this[fi.Name] = v == DBNull.Value ? null : v;
            }
        }

        /// <summary>
        /// 从一个数据行对象加载数据。指定要加载数据的字段，以及要加载哪些关联的实体类对象。
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <param name="hasprefix">是否带有前缀</param>
        /// <param name="ps">要加载数据的字段</param>
        /// <param name="maps">要关联的实体类</param>
        /// <returns></returns>
        private void LoadData(DataRow dr, Boolean hasprefix, FieldItem[] ps, TableMapAttribute[] maps)
        {
            if (dr == null) return;
            if (ps == null || ps.Length < 1) ps = Meta.Fields;
            String prefix = null;
            if (hasprefix) prefix = Meta.ColumnPrefix;
            foreach (FieldItem fi in ps)
            {
                // 两次dr[fi.ColumnName]简化为一次
                Object v = dr[prefix + fi.ColumnNameEx];
                this[fi.Name] = v == DBNull.Value ? null : v;
            }
            //给关联属性赋值
            if (maps != null && maps.Length > 0)
            {
                foreach (TableMapAttribute item in maps)
                {
                    LoadDataEx(dr, item);
                }
            }
        }

        /// <summary>
        /// 从一个数据行对象加载数据。现在用反射实现，为了更好性能，实体类应该重载该方法。
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="map"></param>
        protected virtual void LoadDataEx(DataRow dr, TableMapAttribute map)
        {
            //创建一个对象
            Object obj = Activator.CreateInstance(map.MapEntity);
            //找到装载数据的方法
            MethodInfo method = map.MapEntity.GetMethod("LoadDataWithPrefix");
            //给这个对象装载数据
            method.Invoke(this, new Object[] { dr, null });
            //给关联属性赋值
            map.LocalField.SetValue(this, obj, null);
        }

        /// <summary>
        /// 检查实体类中的哪些字段在数据表中
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="prefix">字段前缀</param>
        /// <returns></returns>
        private static List<FieldItem> CheckColumn(DataTable dt, String prefix)
        {
            // 检查dr中是否有该属性的列。考虑到Select可能是不完整的，此时，只需要局部填充
            FieldItem[] allps = Meta.AllFields;
            if (allps == null || allps.Length < 1) return null;

            //这里可千万不能删除allps中的项，那样会影响到全局的Fields缓存的
            List<FieldItem> ps = new List<FieldItem>();
            for (Int32 i = allps.Length - 1; i >= 0; i--)
            {
                if (dt.Columns.Contains(prefix + allps[i].ColumnNameEx)) ps.Add(allps[i]);
            }
            return ps;
        }

        ///// <summary>
        ///// 把数据复制到数据行对象中。
        ///// </summary>
        ///// <param name="dr">数据行</param>
        //public virtual DataRow ToData(ref DataRow dr)
        //{
        //    if (dr == null) return null;
        //    List<FieldItem> ps = Meta.Fields;
        //    foreach (FieldItem fi in ps)
        //    {
        //        // 检查dr中是否有该属性的列。考虑到Select可能是不完整的，此时，只需要局部填充
        //        if (dr.Table.Columns.Contains(fi.ColumnName))
        //            dr[fi.ColumnName] = this[fi.Name];
        //    }
        //    return dr;
        //}
        #endregion

        #region 操作
        /// <summary>
        /// 把该对象持久化到数据库
        /// </summary>
        /// <returns></returns>
        public virtual Int32 Insert()
        {
            String sql = SQL(this, DataObjectMethodType.Insert);

            //AC和SqlServer支持获取自增字段的最新编号
            if (Meta.DbType == DatabaseType.Access ||
                Meta.DbType == DatabaseType.SqlServer)
            {
                //检查是否有标识列，标识列需要特殊处理
                FieldItem[] ps = Meta.Uniques;
                if (ps != null && ps.Length > 0 && ps[0].DataObjectField != null && ps[0].DataObjectField.IsIdentity)
                {
                    Int32 res = Meta.InsertAndGetIdentity(sql);
                    if (res > 0) ps[0].Property.SetValue(this, res, null);
                    return res;
                }
            }
            return Meta.Execute(sql);
        }

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <returns></returns>
        public virtual Int32 Update()
        {
            String sql = SQL(this, DataObjectMethodType.Update);
            if (String.IsNullOrEmpty(sql)) return 0;
            return Meta.Execute(sql);
        }

        /// <summary>
        /// 从数据库中删除该对象
        /// </summary>
        /// <returns></returns>
        public virtual Int32 Delete()
        {
            return Meta.Execute(SQL(this, DataObjectMethodType.Delete));
        }
        #endregion

        #region 查找单个实体
        /// <summary>
        /// 根据属性以及对应的值，查找单个实体
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static TEntity Find(String name, Object value)
        {
            return Find(new String[] { name }, new Object[] { value });
        }

        /// <summary>
        /// 根据属性列表以及对应的值列表，查找单个实体
        /// </summary>
        /// <param name="names"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static TEntity Find(String[] names, Object[] values)
        {
            return Find(MakeCondition(names, values, "And"));
        }

        /// <summary>
        /// 根据主键查询信息实体对象用于表单编辑
        /// </summary>
        ///<param name="__ID">主键编号</param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static TEntity FindByKeyForEdit(object __ID)
        {

            TEntity entity = Find(new String[] { Meta.Unique.Name }, new Object[] { __ID });
            if (entity == null)
            {
                entity = new TEntity();
            }
            return entity;
        }

        /// <summary>
        /// 根据条件查找单个实体
        /// </summary>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static TEntity Find(String whereClause)
        {
            IList<TEntity> list = FindAll(whereClause, null, null, 0, 1);
            if (list == null || list.Count < 1)
                return null;
            else
                return list[0];
        }
        #endregion

        #region 静态查询
        /// <summary>
        /// 获取所有实体对象。获取大量数据时会非常慢，慎用
        /// </summary>
        /// <returns>实体数组</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<TEntity> FindAll()
        {
            return LoadData(Meta.Query(xConfig.SQL(Meta.ThisType, DataObjectMethodType.Fill)));
        }

        /// <summary>
        /// 查询并返回实体对象数组。
        /// 表名以及所有字段名，请使用类名以及字段对应的属性名，方法内转换为表名和列名
        /// </summary>
        /// <param name="whereClause">条件，不带Where</param>
        /// <param name="orderClause">排序，不带Order By</param>
        /// <param name="selects">查询列</param>
        /// <param name="startRowIndex">开始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <returns>实体数组</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<TEntity> FindAll(String whereClause, String orderClause, String selects, Int32 startRowIndex, Int32 maximumRows)
        {
            String sql = PageSplitSQL(whereClause, orderClause, selects, startRowIndex, maximumRows);
            return LoadData(Meta.Query(sql));
        }

        /// <summary>
        /// 根据属性列表以及对应的值列表，获取所有实体对象
        /// </summary>
        /// <param name="names">属性列表</param>
        /// <param name="values">值列表</param>
        /// <returns>实体数组</returns>
        public static List<TEntity> FindAll(String[] names, Object[] values)
        {
            return FindAll(MakeCondition(names, values, "And"), null, null, 0, 0);
        }

        /// <summary>
        /// 根据属性以及对应的值，获取所有实体对象
        /// </summary>
        /// <param name="name">属性</param>
        /// <param name="value">值</param>
        /// <returns>实体数组</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<TEntity> FindAll(String name, Object value)
        {
            return FindAll(new String[] { name }, new Object[] { value });
        }

        /// <summary>
        /// 根据属性以及对应的值，获取所有实体对象
        /// </summary>
        /// <param name="name">属性</param>
        /// <param name="value">值</param>
        /// <param name="startRowIndex">起始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <returns>实体数组</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<TEntity> FindAll(String name, Object value, Int32 startRowIndex, Int32 maximumRows)
        {
            if (String.IsNullOrEmpty(name))
                return FindAll(null, null, null, startRowIndex, maximumRows);
            else
                return FindAll(MakeCondition(new String[] { name }, new Object[] { value }, "And"), null, null, startRowIndex, maximumRows);
        }

        /// <summary>
        /// 根据属性以及对应的值，获取所有实体对象
        /// </summary>
        /// <param name="name">属性</param>
        /// <param name="value">值</param>
        /// <param name="orderClause">排序，不带Order By</param>
        /// <param name="startRowIndex">起始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <returns>实体数组</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static List<TEntity> FindAllByName(String name, Object value, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        {
            if (String.IsNullOrEmpty(name))
                return FindAll(null, orderClause, null, startRowIndex, maximumRows);
            else
                return FindAll(MakeCondition(new String[] { name }, new Object[] { value }, "And"), orderClause, null, startRowIndex, maximumRows);
        }

        /// <summary>
        /// 查询SQL并返回实体对象数组。
        /// Select方法将直接使用参数指定的查询语句进行查询，不进行任何转换。
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>实体数组</returns>
        public static List<TEntity> FindAll(String sql)
        {
            return LoadData(Meta.Query(sql));
        }

        /// <summary>
        /// 查询并返回实体对象数组。
        /// 如果指定了jointypes参数，则同时返回参数中指定的关联对象
        /// </summary>
        /// <param name="whereClause">条件，不带Where</param>
        /// <param name="orderClause">排序，不带Order By</param>
        /// <param name="selects">查询列</param>
        /// <param name="startRowIndex">开始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <param name="jointypes">要关联的实体类型列表</param>
        /// <returns>实体数组</returns>
        public static List<TEntity> FindAllMultiple(String whereClause, String orderClause, String selects, Int32 startRowIndex, Int32 maximumRows, Type[] jointypes)
        {
            if (jointypes == null || jointypes.Length < 1) return FindAll(whereClause, orderClause, selects, startRowIndex, maximumRows);

            //根据传入的实体类型列表来决定处理哪些多表关联
            TableMapAttribute[] maps = xConfig.TableMaps(Meta.ThisType, jointypes);
            //没有找到带有映射特性的字段
            if (maps == null || maps.Length < 1) return FindAll(whereClause, orderClause, selects, startRowIndex, maximumRows);

            String LocalTableName = Meta.TableName;
            //准备拼接SQL查询语句
            StringBuilder sb = new StringBuilder();
            sb.Append("Select ");
            //sb.Append(selects);
            if (String.IsNullOrEmpty(selects) || selects == "*" || selects.Trim() == "*")
            {
                sb.Append(xConfig.SelectsEx(Meta.ThisType));
            }
            else
            {
                String[] ss = selects.Split(',');
                Boolean isfirst = false;
                foreach (String item in ss)
                {
                    if (!isfirst)
                    {
                        sb.Append(", ");
                        isfirst = true;
                    }
                    sb.AppendFormat("{0}.{1} as {2}{1}", LocalTableName, OqlToSql(item), Meta.ColumnPrefix);
                }
            }

            //对于每一个关联的实体类型表进行处理
            foreach (TableMapAttribute item in maps)
            {
                sb.Append(", ");
                sb.Append(xConfig.SelectsEx(item.MapEntity));
            }
            sb.Append(" From ");
            sb.Append(LocalTableName);

            List<String> tables = new List<string>();
            tables.Add(LocalTableName);
            //对于每一个关联的实体类型表进行处理
            foreach (TableMapAttribute item in maps)
            {
                String tablename = xConfig.TableName(item.MapEntity);
                tables.Add(tablename);
                sb.Append(" ");
                //关联类型
                sb.Append(item.MapType.ToString().Replace("_", " "));
                sb.Append(" ");
                //关联表
                sb.Append(tablename);
                sb.Append(" On ");
                sb.AppendFormat("{0}.{1}={2}.{3}", LocalTableName, item.LocalColumn, tablename, item.MapColumn);
            }

            if (!String.IsNullOrEmpty(whereClause))
            {
                //加上前缀
                whereClause = Regex.Replace(whereClause, "(w+)", "");
                sb.AppendFormat(" Where {0} ", OqlToSql(whereClause));
            }
            if (!String.IsNullOrEmpty(orderClause))
            {
                //加上前缀
                sb.AppendFormat(" Order By {0} ", OqlToSql(orderClause));
            }

            FieldItem fi = Meta.Unique;
            String keyColumn = null;
            if (fi != null)
            {
                keyColumn = Meta.ColumnPrefix + fi.ColumnName;
                // 加上Desc标记，将使用MaxMin分页算法。标识列，单一主键且为数字类型
                if (fi.DataObjectField.IsIdentity || fi.Property.PropertyType == typeof(Int32)) keyColumn += " Desc";
            }
            String sql = Meta.PageSplit(sb.ToString(), startRowIndex, maximumRows, keyColumn);
            DataSet ds = Meta.Query(sql, tables.ToArray());
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1) return null;

            return LoadData(ds.Tables[0], jointypes);
        }

        /// <summary>
        /// 查询满足条件的记录集，分页、排序
        /// </summary>
        /// <param name="qp">查询构造器</param>
        /// <param name="RecordCount">最大结果数量</param>
        /// <returns>实体集</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static TEntity FindAllByItem(QueryParam qp, out int RecordCount)
        {
            List<TEntity> list = FindAll(qp, out RecordCount);
            return list != null && list.Count > 0 ? list[0] : new TEntity();
        }


        /// <summary>
        /// 查询满足条件的记录集，分页、排序
        /// </summary>
        /// <param name="qp">查询构造器</param>
        /// <returns>实体集</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static TEntity FindAllByItem(QueryParam qp)
        {
            Int32 RecordCount = 0;
            return FindAllByItem(qp, out RecordCount);
        }


        /// <summary>
        /// 查询满足条件的记录集，分页、排序
        /// </summary>
        /// <param name="qp">查询构造器</param>
        /// <param name="RecordCount">最大结果数量</param>
        /// <returns>实体集</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static List<TEntity> FindAll(QueryParam qp, out int RecordCount)
        {
            //构造排序字段
            string orderClause = null;
            if (qp.Orderfld == string.Empty || qp.Orderfld == "")
            {
                //有主键的情况
                if (Meta.Unique != null)
                    qp.Orderfld = Meta.Unique.Name;
            }

            //避免无主键而无法排序的情况下的判断
            if (qp.Orderfld != string.Empty && qp.Orderfld != "")
                orderClause = string.Format("{0} {1}", qp.Orderfld, qp.OrderType == 1 ? "desc" : "asc");

            //构造查询条件
            String WhereCase = qp.ToSql();

            //返回当前最大结果数量
            qp.RecordCount = RecordCount = FindCount(WhereCase, null, null, 0, 0);
            //返回所有结果集
            return FindAll(WhereCase, orderClause, qp.ReturnFields, qp.startRowIndex, qp.PageSize);
        }

        /// <summary>
        /// 查询满足条件的记录集，分页、排序
        /// </summary>
        /// <param name="qp">查询构造器</param>
        /// <returns>最大结果数量</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static Int32 FindCount(QueryParam qp)
        {
            //构造排序字段
            string orderClause = string.Empty;
            if (qp.Orderfld == string.Empty)
                qp.Orderfld = Meta.Unique.Name;


            orderClause = string.Format("{0} {1}", qp.Orderfld, qp.OrderType == 1 ? "desc" : "asc");

            //构造查询条件
            String WhereCase = qp.ToSql();

            //返回当前最大结果数量
            return FindCount(WhereCase, null, null, 0, 0);
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="qp">查询构造器</param>
        /// <returns>返回的结果集中第一行的第一列</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static Object FindScalar(QueryParam qp)
        {
            //构造排序字段
            string orderClause = string.Empty;
            if (qp.Orderfld == string.Empty)
                qp.Orderfld = Meta.Unique.Name;


            orderClause = string.Format("{0} {1}", qp.Orderfld, qp.OrderType == 1 ? "desc" : "asc");

            //构造查询条件
            String WhereCase = qp.ToSql();

            //返回当前最大结果数量
            return FindScalar(WhereCase, orderClause, qp.ReturnFields);
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static List<TEntity> FindRandomAll(QueryParam qp, out int RecordCount)
        {
            string str = null;
            if (((qp.Orderfld == string.Empty) || (qp.Orderfld == "")) && (Meta.Unique != null))
            {
                qp.Orderfld = Meta.Unique.Name;
            }
            if ((qp.Orderfld != string.Empty) && (qp.Orderfld != ""))
            {
                str = string.Format("{0} {1}", qp.Orderfld, (qp.OrderType == 1) ? "desc" : "asc");
            }
            string whereClause = qp.ToSql();
            qp.RecordCount = RecordCount = Entity<TEntity>.FindCount(whereClause, null, null, 0, 0);
            StringBuilder builder = new StringBuilder();
            builder.Append("Select ");
            builder.Append(string.IsNullOrEmpty(qp.ReturnFields) ? "*" : Entity<TEntity>.OqlToSql(qp.ReturnFields));
            builder.Append(" From ");
            builder.Append(Meta.TableName);
            if (!string.IsNullOrEmpty(whereClause))
            {
                builder.AppendFormat(" Where {0} ", Entity<TEntity>.OqlToSql(whereClause));
            }
            if (!string.IsNullOrEmpty(str))
            {
                builder.AppendFormat(" Order By {0} ", Entity<TEntity>.OqlToSql(str));
            }
            return Entity<TEntity>.LoadData(Meta.Query(builder.ToString()));
        }

 

        #endregion

        #region 取总记录数
        /// <summary>
        /// 返回总记录数
        /// </summary>
        /// <returns></returns>
        public static Int32 FindCount()
        {
            return Meta.QueryCount(xConfig.SQL(Meta.ThisType, DataObjectMethodType.Fill));
        }

        /// <summary>
        /// 返回总记录数
        /// </summary>
        /// <param name="whereClause">条件，不带Where</param>
        /// <param name="orderClause">排序，不带Order By</param>
        /// <param name="selects">查询列</param>
        /// <param name="startRowIndex">开始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <returns>总行数</returns>
        public static Int32 FindCount(String whereClause, String orderClause, String selects, Int32 startRowIndex, Int32 maximumRows)
        {
            String sql = PageSplitSQL(whereClause, orderClause, selects, startRowIndex, maximumRows);
            return Meta.QueryCount(sql);
        }


        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="whereClause">条件，不带Where</param>
        /// <param name="orderClause">排序，不带Order By</param>
        /// <param name="selects">查询列</param>
        /// <returns>总行数</returns>
        public static Object FindScalar(String whereClause, String orderClause, String selects)
        {
            String sql = PageSplitSQL(whereClause, orderClause, selects, 0, 1);
            return Meta.ExecuteScalar(sql);
        }

        /// <summary>
        /// 根据属性列表以及对应的值列表，返回总记录数
        /// </summary>
        /// <param name="names">属性列表</param>
        /// <param name="values">值列表</param>
        /// <returns>总行数</returns>
        public static Int32 FindCount(String[] names, Object[] values)
        {
            return FindCount(MakeCondition(names, values, "And"), null, null, 0, 0);
        }

        /// <summary>
        /// 根据属性以及对应的值，返回总记录数
        /// </summary>
        /// <param name="name">属性</param>
        /// <param name="value">值</param>
        /// <returns>总行数</returns>
        public static Int32 FindCount(String name, Object value)
        {
            return FindCount(name, value,0,0);
        }

        /// <summary>
        /// 根据属性以及对应的值，返回总记录数
        /// </summary>
        /// <param name="name">属性</param>
        /// <param name="value">值</param>
        /// <param name="startRowIndex">开始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <returns>总行数</returns>
        public static Int32 FindCount(String name, Object value, Int32 startRowIndex, Int32 maximumRows)
        {
            if (String.IsNullOrEmpty(name))
                return FindCount(null, null, null, startRowIndex, maximumRows);
            else
                return FindCount(MakeCondition(new String[] { name }, new Object[] { value }, "And"), null, null, startRowIndex, maximumRows);
        }

        /// <summary>
        /// 根据属性以及对应的值，返回总记录数
        /// </summary>
        /// <param name="name">属性</param>
        /// <param name="value">值</param>
        /// <param name="orderClause">排序，不带Order By</param>
        /// <param name="startRowIndex">开始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <returns>总行数</returns>
        public static Int32 FindCountByName(String name, Object value, String orderClause, int startRowIndex, int maximumRows)
        {
            if (String.IsNullOrEmpty(name))
                return FindCount(null, orderClause, null, startRowIndex, maximumRows);
            else
                return FindCount(MakeCondition(new String[] { name }, new Object[] { value }, "And"), orderClause, null, startRowIndex, maximumRows);
        }
        #endregion

        #region 静态操作
        /// <summary>
        /// 把一个实体对象持久化到数据库
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <returns>返回受影响的行数</returns>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public static Int32 Insert(TEntity obj)
        {
            return obj.Insert();
        }

        /// <summary>
        /// 把一个实体对象持久化到数据库
        /// </summary>
        /// <param name="names">更新属性列表</param>
        /// <param name="values">更新值列表</param>
        /// <returns>返回受影响的行数</returns>
        public static Int32 Insert(String[] names, Object[] values)
        {
            if (names == null) throw new ArgumentNullException("names", "属性列表和值列表不能为空");
            if (values == null) throw new ArgumentNullException("values", "属性列表和值列表不能为空");

            if (names.Length != values.Length) throw new ArgumentException("属性列表必须和值列表一一对应");
            FieldItem[] fis = Meta.Fields;
            Dictionary<String, FieldItem> fs = new Dictionary<String, FieldItem>();
            foreach (FieldItem fi in fis)
                fs.Add(fi.Name, fi);
            StringBuilder sbn = new StringBuilder();
            StringBuilder sbv = new StringBuilder();
            for (Int32 i = 0; i < names.Length; i++)
            {
                if (!fs.ContainsKey(names[i])) throw new ArgumentException("类[" + Meta.ThisType.FullName + "]中不存在[" + names[i] + "]属性");
                // 同时构造SQL语句。names是属性列表，必须转换成对应的字段列表
                if (i > 0)
                {
                    sbn.Append(", ");
                    sbv.Append(", ");
                }
                sbn.Append(fs[names[i]].Name);
                sbv.Append(SqlDataFormat(values[i], fs[names[i]]));
            }
            String tablename = Meta.TableName;
            return Meta.Execute(String.Format("Inset Into {2}({0}) values({1})", sbn.ToString(), sbv.ToString(), tablename));
        }

        /// <summary>
        /// 把一个实体对象更新到数据库
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <returns>返回受影响的行数</returns>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static Int32 Update(TEntity obj)
        {
            return obj.Update();
        }

        /// <summary>
        /// 更新一批实体数据
        /// </summary>
        /// <param name="setClause">要更新的项和数据</param>
        /// <param name="whereClause">指定要更新的实体</param>
        /// <returns></returns>
        public static Int32 Update(String setClause, String whereClause)
        {
            if (String.IsNullOrEmpty(setClause) || !setClause.Contains("=")) throw new ArgumentException("Incorrect parameter");
            String sql = String.Format("Update {0} Set {1}", Meta.TableName, setClause);
            if (!String.IsNullOrEmpty(whereClause)) sql += " Where " + whereClause;
            return Meta.Execute(sql);
        }

        /// <summary>
        /// 更新一批实体数据
        /// </summary>
        /// <param name="setNames">更新属性列表</param>
        /// <param name="setValues">更新值列表</param>
        /// <param name="whereNames">条件属性列表</param>
        /// <param name="whereValues">条件值列表</param>
        /// <returns>返回受影响的行数</returns>
        public static Int32 Update(String[] setNames, Object[] setValues, String[] whereNames, Object[] whereValues)
        {
            String sc = MakeCondition(setNames, setValues, ", ");
            String wc = MakeCondition(whereNames, whereValues, " And ");
            return Update(sc, wc);
        }

        /// <summary>
        /// 从数据库中删除指定实体对象。
        /// 实体类应该实现该方法的另一个副本，以唯一键或主键作为参数
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <returns>返回受影响的行数，可用于判断被删除了多少行，从而知道操作是否成功</returns>
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static Int32 Delete(TEntity obj)
        {
            return obj.Delete();
        }

        /// <summary>
        /// 从数据库中删除指定条件的实体对象。
        /// </summary>
        /// <param name="whereClause">限制条件</param>
        /// <returns></returns>
        public static Int32 Delete(String whereClause)
        {
            String sql = String.Format("Delete From {0}", Meta.TableName);
            if (!String.IsNullOrEmpty(whereClause)) sql += " Where " + whereClause;
            return Meta.Execute(sql);
        }

        /// <summary>
        /// 从数据库中删除指定属性列表和值列表所限定的实体对象。
        /// </summary>
        /// <param name="names">属性列表</param>
        /// <param name="values">值列表</param>
        /// <returns></returns>
        public static Int32 Delete(String[] names, Object[] values)
        {
            return Delete(MakeCondition(names, values, "And"));
        }
        #endregion

        #region 辅助方法
        private static DateTime year1900 = new DateTime(1900, 1, 1);
        private static DateTime year1753 = new DateTime(1753, 1, 1);
        private static DateTime year9999 = new DateTime(9999, 1, 1);
        /// <summary>
        /// 取得一个值的Sql值。
        /// 当这个值是字符串类型时，会在该值前后加单引号；
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="field">字段特性</param>
        /// <returns>Sql值的字符串形式</returns>
        protected static String SqlDataFormat(Object obj, FieldItem field)
        {
            Boolean isNullable = field.DataObjectField.IsNullable;
            String typeName = field.Property.PropertyType.FullName;
            if (typeName.Contains("String"))
            {
                if (obj == null) return isNullable ? "null" : "''";
                if (String.IsNullOrEmpty(obj.ToString()) && isNullable) return "null";
                return "N'" + obj.ToString().Replace("'", "''") + "'";
            }
            else if (typeName.Contains("DateTime"))
            {
                if (obj == null) return isNullable ? "null" : "''";
                DateTime dt = Convert.ToDateTime(obj);
                if ((dt == DateTime.MinValue || dt == year1900) && isNullable) return "null";
                if (Meta.DbType == DatabaseType.Oracle)
                    return String.Format("To_Date('{0}', 'YYYYMMDDHH24MISS')", dt.ToString("yyyyMMddhhmmss"));
                // SqlServer拒绝所有其不能识别为 1753 年到 9999 年间的日期的值
                if (Meta.DbType == DatabaseType.SqlServer)
                {
                    try
                    {
                        if (dt < year1753 || dt > year9999) return isNullable ? "null" : "''";
                    }
                    catch { }
                }
                return "{ts '" + dt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'}";
                //return "{ts" + String.Format("'{0:yyyy-MM-dd HH:mm:ss}'", dt) + "}";
                //return string.Format("CONVERT(datetime,'{0}',21)", dt.ToString("yyyy-MM-dd HH:mm:ss").Replace(".", ":"));
                //return "'" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            else if (typeName.Contains("Boolean"))
            {
                if (obj == null) return isNullable ? "null" : "";
                if (Meta.DbType == DatabaseType.SqlServer)
                    return Convert.ToBoolean(obj) ? "1" : "0";
                else
                    return obj.ToString();
            }
            else
            {
                if (obj == null) return isNullable ? "null" : "";
                return obj.ToString();
            }
        }

        /// <summary>
        /// 把SQL模版格式化为SQL语句
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <param name="methodType"></param>
        /// <returns>SQL字符串</returns>
        protected static String SQL(Entity<TEntity> obj, DataObjectMethodType methodType)
        {
            String sql;
            StringBuilder sbNames;
            StringBuilder sbValues;
            Boolean isFirst = true;
            switch (methodType)
            {
                case DataObjectMethodType.Fill:
                    //return String.Format("Select {0} From {1}", Meta.Selects, Meta.TableName);
                    return String.Format("Select * From {0}", Meta.TableName);
                case DataObjectMethodType.Select:
                    sql = DefaultCondition(obj);
                    // 没有标识列和主键，返回取所有数据的语句
                    if (String.IsNullOrEmpty(sql)) return null;
                    //return String.Format("Select {0} From {1} Where {2}", Meta.Selects, Meta.TableName, sql);
                    return String.Format("Select * From {0} Where {1}", Meta.TableName, sql);
                case DataObjectMethodType.Insert:
                    sbNames = new StringBuilder();
                    sbValues = new StringBuilder();
                    // 只读列没有插入操作
                    foreach (FieldItem fi in Meta.Fields)
                    {
                        // 标识列不需要插入，别的类型都需要
                        if (!fi.DataObjectField.IsIdentity)
                        {
                            if (!isFirst) sbNames.Append(", "); // 加逗号
                            sbNames.Append(fi.ColumnName);
                            if (!isFirst)
                                sbValues.Append(", "); // 加逗号
                            else
                                isFirst = false;
                            sbValues.Append(SqlDataFormat(obj[fi.Name], fi)); // 数据
                        }
                    }
                    return String.Format("Insert Into {0}({1}) Values({2})", Meta.TableName, sbNames.ToString(), sbValues.ToString());
                case DataObjectMethodType.Update:
                    sbNames = new StringBuilder();
                    // 只读列没有更新操作
                    foreach (FieldItem fi in Meta.Fields)
                    {
                        if (fi.DataObjectField.IsIdentity) continue;

                        //脏数据判断
                        if (!obj.Dirtys[fi.Name]) continue;

                        if (!isFirst)
                            sbNames.Append(", "); // 加逗号
                        else
                            isFirst = false;
                        sbNames.Append(fi.ColumnName);
                        sbNames.Append("=");
                        sbNames.Append(SqlDataFormat(obj[fi.Name], fi)); // 数据
                    }

                    if (sbNames.Length <= 0) return null;

                    sql = DefaultCondition(obj);
                    if (String.IsNullOrEmpty(sql)) return null;
                    return String.Format("Update {0} Set {1} Where {2}", Meta.TableName, sbNames.ToString(), sql);
                case DataObjectMethodType.Delete:
                    // 标识列作为删除关键字
                    sql = DefaultCondition(obj);
                    if (String.IsNullOrEmpty(sql))
                        return null;
                    return String.Format("Delete From {0} Where {1}", Meta.TableName, sql);
            }
            return null;
        }

        /// <summary>
        /// 根据属性列表和值列表，构造查询条件。
        /// 例如构造多主键限制查询条件。
        /// </summary>
        /// <param name="names">属性列表</param>
        /// <param name="values">值列表</param>
        /// <param name="action">联合方式</param>
        /// <returns>条件子串</returns>
        protected static String MakeCondition(String[] names, Object[] values, String action)
        {
            if (names == null) throw new ArgumentNullException("names", "属性列表和值列表不能为空");
            if (values == null) throw new ArgumentNullException("values", "属性列表和值列表不能为空");

            if (names.Length != values.Length) throw new ArgumentException("属性列表必须和值列表一一对应");
            Dictionary<String, FieldItem> fs = new Dictionary<String, FieldItem>();
            foreach (FieldItem fi in Meta.Fields)
                fs.Add(fi.Name.ToLower(), fi);
            StringBuilder sb = new StringBuilder();
            for (Int32 i = 0; i < names.Length; i++)
            {
                if (!fs.ContainsKey(names[i].ToLower())) throw new ArgumentException("类[" + Meta.ThisType.FullName + "]中不存在[" + names[i] + "]属性");
                // 同时构造SQL语句。names是属性列表，必须转换成对应的字段列表
                if (i > 0) sb.AppendFormat(" {0} ", action);
                sb.AppendFormat("{0}={1}", fs[names[i].ToLower()].ColumnName, SqlDataFormat(values[i], fs[names[i].ToLower()]));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 默认条件。
        /// 若有标识列，则使用一个标识列作为条件；
        /// 如有主键，则使用全部主键作为条件。
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <returns>条件</returns>
        protected static String DefaultCondition(Entity<TEntity> obj)
        {
            Type t = obj.GetType();
            // 唯一键作为查询关键字
            FieldItem[] ps = Meta.Uniques;
            // 没有标识列和主键，返回取所有数据的语句
            if (ps == null || ps.Length < 1) return null;
            // 标识列作为查询关键字
            if (ps[0].DataObjectField.IsIdentity)
            {
                return String.Format("{0}={1}", ps[0].ColumnName, SqlDataFormat((obj as TEntity)[ps[0].Name], ps[0]));
            }
            // 主键作为查询关键字
            StringBuilder sb = new StringBuilder();
            foreach (FieldItem fi in ps)
            {
                if (sb.Length > 0) sb.Append(" And ");
                sb.Append(fi.ColumnName);
                sb.Append("=");
                sb.Append(SqlDataFormat(obj[fi.Name], fi));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 把对象Oql转换称为标准TSql
        /// </summary>
        /// <param name="oql">实体对象oql</param>
        /// <returns>Sql字符串</returns>
        protected static String OqlToSql(String oql)
        {
            if (String.IsNullOrEmpty(oql)) return oql;
            String sql = oql;
            if (Meta.ThisType.Name != Meta.TableName)
                sql = Regex.Replace(sql, @"\b" + Meta.ThisType.Name + @"\b", Meta.TableName, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            foreach (FieldItem fi in Meta.Fields)
                if (fi.Name != fi.ColumnName)
                    sql = Regex.Replace(sql, @"\b" + fi.Name + @"\b", fi.ColumnName, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return sql;
        }


        /// <summary>
        /// 取得指定实体类型的分页SQL
        /// </summary>
        /// <param name="whereClause">条件，不带Where</param>
        /// <param name="orderClause">排序，不带Order By</param>
        /// <param name="selects">查询列</param>
        /// <param name="startRowIndex">开始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <returns>分页SQL</returns>
        protected static String PageSplitSQL(String whereClause, String orderClause, String selects, Int32 startRowIndex, Int32 maximumRows)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select ");
            sb.Append(String.IsNullOrEmpty(selects) ? "*" : OqlToSql(selects));
            sb.Append(" From ");
            sb.Append(Meta.TableName);
            if (!String.IsNullOrEmpty(whereClause)) sb.AppendFormat(" Where {0} ", OqlToSql(whereClause));
            if (!String.IsNullOrEmpty(orderClause)) sb.AppendFormat(" Order By {0} ", OqlToSql(orderClause));
            return PageSplitSQL(sb.ToString(), startRowIndex, maximumRows);
        }

        /// <summary>
        /// 取得指定实体类型的分页SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="startRowIndex">开始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <returns>分页SQL</returns>
        protected static String PageSplitSQL(String sql, Int32 startRowIndex, Int32 maximumRows)
        {
            FieldItem fi = Meta.Unique;
            String keyColumn = null;
            if (fi != null)
            {
                keyColumn = fi.ColumnName;
                // 加上Desc标记，将使用MaxMin分页算法。标识列，单一主键且为数字类型
                if (fi.DataObjectField.IsIdentity || fi.Property.PropertyType == typeof(Int32)) keyColumn += " Desc";
            }
            return Meta.PageSplit(sql, startRowIndex, maximumRows, keyColumn);
        }
        #endregion

        #region 获取/设置 字段值
        /// <summary>
        /// 获取/设置 字段值。
        /// 一个索引，反射实现。
        /// 派生实体类可重写该索引，以避免发射带来的性能损耗
        /// </summary>
        /// <param name="name">字段名</param>
        /// <returns></returns>
        public virtual Object this[String name]
        {
            get
            {
                foreach (FieldItem fi in Meta.Fields)
                    if (fi.Name == name) return fi.Property.GetValue(this, null);
                throw new ArgumentException("类[" + this.GetType().FullName + "]中不存在[" + name + "]属性");
            }
            set
            {
                foreach (FieldItem fi in Meta.Fields)
                    if (fi.Name == name) { fi.Property.SetValue(this, value, null); return; }
                throw new ArgumentException("类[" + this.GetType().FullName + "]中不存在[" + name + "]属性");
            }
        }
        #endregion

        #region 导入导出XML
        /// <summary>
        /// 导出XML
        /// </summary>
        /// <returns></returns>
        public virtual String ToXML()
        {
            XmlSerializer serial = new XmlSerializer(Meta.ThisType);
            //using (StringWriter writer = new StringWriter())
            //{
            //    serial.Serialize(writer, this);
            //    return writer.ToString();
            //}
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
                serial.Serialize(writer, this);
                writer.Close();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static TEntity FromXML(String xml)
        {
            XmlSerializer serial = new XmlSerializer(Meta.ThisType);
            using (StringReader reader = new StringReader(xml))
            {
                return serial.Deserialize(reader) as TEntity;
            }
        }

        ///// <summary>
        ///// 高级序列化
        ///// </summary>
        ///// <param name="writer">文本读写器</param>
        ///// <param name="propertyAsAttribute">属性作为Xml属性进行序列化</param>
        ///// <param name="hasNamespace"></param>
        //public virtual void Serialize(TextWriter writer, Boolean propertyAsAttribute, Boolean hasNamespace)
        //{
        //    XmlAttributeOverrides overrides = null;
        //    overrides = new XmlAttributeOverrides();
        //    Type type = this.GetType();
        //    //IList<FieldItem> fs = FieldItem.GetDataObjectFields(type);
        //    PropertyInfo[] pis = type.GetProperties();
        //    //foreach (FieldItem item in fs)
        //    foreach (PropertyInfo item in pis)
        //    {
        //        if (!item.CanRead) continue;

        //        if (propertyAsAttribute)
        //        {
        //            XmlAttributeAttribute att = new XmlAttributeAttribute();
        //            XmlAttributes xas = new XmlAttributes();
        //            xas.XmlAttribute = att;
        //            overrides.Add(type, item.Name, xas);
        //        }
        //        else
        //        {
        //            XmlAttributes xas = new XmlAttributes();
        //            xas.XmlElements.Add(new XmlElementAttribute());
        //            overrides.Add(type, item.Name, xas);
        //        }
        //    }

        //    XmlSerializer serial = new XmlSerializer(this.GetType(), overrides);
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        serial.Serialize(writer, this);
        //        writer.Close();
        //    }
        //}

        ///// <summary>
        ///// 高级序列化
        ///// </summary>
        ///// <param name="propertyAsAttribute">属性作为Xml属性进行序列化</param>
        ///// <param name="hasNamespace"></param>
        ///// <returns></returns>
        //public virtual String Serialize(Boolean propertyAsAttribute, Boolean hasNamespace)
        //{
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
        //        Serialize(writer, propertyAsAttribute, hasNamespace);
        //        writer.Close();
        //        return Encoding.UTF8.GetString(stream.ToArray());
        //    }
        //}
        #endregion

        #region 克隆
        /// <summary>
        /// 创建当前对象的克隆对象，仅拷贝基本字段
        /// </summary>
        /// <returns></returns>
        public virtual Object Clone()
        {
            return CloneEntity();
        }

        /// <summary>
        /// 克隆实体。创建当前对象的克隆对象，仅拷贝基本字段
        /// </summary>
        /// <returns></returns>
        public virtual TEntity CloneEntity()
        {
            TEntity obj = new TEntity();
            foreach (FieldItem fi in Meta.Fields)
            {
                obj[fi.Name] = this[fi.Name];
            }
            return obj;
        }
        #endregion

        #region 其它
        /// <summary>
        /// 已重载。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Meta.FieldNames.Contains("Name"))
                return this["Name"] == null ? null : this["Name"].ToString();
            else if (Meta.FieldNames.Contains("ID"))
                return this["ID"] == null ? null : this["ID"].ToString();
            else
                return "实体" + Meta.ThisType.Name;
        }
        #endregion

        #region 脏数据
        [NonSerialized]
        private DirtyCollection _Dirtys;
        /// <summary>脏属性。存储哪些属性的数据被修改过了。</summary>
        [XmlIgnore]
        private DirtyCollection Dirtys
        {
            get
            {
                if (_Dirtys == null) _Dirtys = new DirtyCollection();
                return _Dirtys;
            }
            set { _Dirtys = value; }
        }

        /// <summary>
        /// 设置所有数据的脏属性
        /// </summary>
        /// <param name="isDirty">改变脏属性的属性个数</param>
        /// <returns></returns>
        private Int32 SetDirty(Boolean isDirty)
        {
            Int32 count = 0;
            foreach (String item in Meta.FieldNames)
            {
                if (isDirty)
                {
                    if (!Dirtys.ContainsKey(item) || !Dirtys[item])
                    {
                        Dirtys[item] = true;
                        count++;
                    }
                }
                else
                {
                    if (_Dirtys == null || Dirtys.Count < 1) break;
                    if (Dirtys.ContainsKey(item) && Dirtys[item])
                    {
                        Dirtys[item] = false;
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 属性改变。重载时记得调用基类的该方法，以设置脏数据属性，否则数据将无法Update到数据库。
        /// </summary>
        /// <param name="fieldName">字段名</param>
        /// <param name="newValue">新属性值</param>
        /// <returns>是否允许改变</returns>
        protected virtual Boolean OnPropertyChange(String fieldName, Object newValue)
        {
            Dirtys[fieldName] = true;
            return true;
        }
        #endregion

        #region 自动修改数据表结构
        private static Object schemasLock = new Object();
        private static Boolean hasChecked = false;
        /// <summary>
        /// 检查数据表架构是否已被修改
        /// </summary>
        private static void CheckModify()
        {
            if (hasChecked) return;
            lock (schemasLock)
            {
                if (hasChecked) return;

                DatabaseSchema schema = new DatabaseSchema(Meta.ConnName, Meta.ThisType);
                schema.BeginCheck();

                hasChecked = true;
            }
        }
        #endregion
    }
}