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
    /// ����ʵ������ࡣ��������ʵ���඼����̳и��ࡣ
    /// </summary>
    [Serializable]
    public class Entity<TEntity> : ICloneable where TEntity : Entity<TEntity>, new()
    {
        #region Ԫ����
        /// <summary>
        /// Ԫ����
        /// </summary>
        public static class Meta
        {
            /// <summary>
            /// ʵ������
            /// </summary>
            public static Type ThisType { get { return typeof(TEntity); } }

            /// <summary>
            /// ʵ��������
            /// </summary>
            public static String ConnName { get { return xConfig.ConnName(ThisType); } }

            /// <summary>
            /// ����
            /// </summary>
            public static String TableName { get { return xConfig.TableName(ThisType); } }

            /// <summary>
            /// ������������
            /// </summary>
            public static FieldItem[] AllFields { get { return xConfig.AllFields(ThisType); } }

            /// <summary>
            /// ���а󶨵����ݱ������
            /// </summary>
            public static FieldItem[] Fields { get { return xConfig.Fields(ThisType); } }

            private static List<String> _FieldNames;
            /// <summary>
            /// �ֶ����б�
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
            /// Ψһ��
            /// </summary>
            public static FieldItem[] Uniques { get { return xConfig.Unique(ThisType); } }

            /// <summary>
            /// Ψһ��
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
            /// ȡ���ֶ�ǰ׺
            /// </summary>
            public static String ColumnPrefix { get { return xConfig.ColumnPrefix(ThisType); } }

            /// <summary>
            /// ȡ��ָ�����Ӧ��Select�־��ַ�����
            /// </summary>
            public static String Selects { get { return xConfig.Selects(ThisType); } }

            /// <summary>
            /// ���ݲ���������Ϊ˽�У���Ϊ�˱�֤�ܹ��������и������ݵĲ������Ա����ʵ�建��
            /// </summary>
            private static DAL DBO { get { return DAL.Create(Meta.ConnName); } }

            #region DAL����
            /// <summary>
            /// ���ݿ�����
            /// </summary>
            public static DatabaseType DbType { get { return DBO.DbType; } }

            /// <summary>
            /// ��ѯ
            /// </summary>
            /// <param name="sql">SQL���</param>
            /// <returns>�����¼��</returns>
            public static DataSet Query(String sql)
            {
                return DBO.Select(sql, Meta.TableName);
            }

            /// <summary>
            /// ��ѯ
            /// </summary>
            /// <param name="sql">SQL���</param>
            /// <param name="tableNames">�������ı�ı���</param>
            /// <returns>�����¼��</returns>
            public static DataSet Query(String sql, String[] tableNames)
            {
                return DBO.Select(sql, tableNames);
            }

            /// <summary>
            /// ��ѯ��¼��
            /// </summary>
            /// <param name="sql">SQL���</param>
            /// <returns>��¼��</returns>
            public static Int32 QueryCount(String sql)
            {
                return DBO.SelectCount(sql, Meta.TableName);
            }

            /// <summary>
            /// ִ��
            /// </summary>
            /// <param name="sql">SQL���</param>
            /// <returns>Ӱ��Ľ��</returns>
            public static Int32 Execute(String sql)
            {
                Int32 rs = DBO.Execute(sql, Meta.TableName);
                Cache.Clear();
                return rs;
            }


            /// <summary>
            /// ִ��
            /// </summary>
            /// <param name="sql">SQL���</param>
            /// <returns>Ӱ��Ľ��</returns>
            public static Object ExecuteScalar(String sql)
            {
                Object rs = DBO.ExecuteScalar(sql, Meta.TableName);
                Cache.Clear();
                return rs;
            }

            /// <summary>
            /// ִ�в�����䲢���������е��Զ����
            /// </summary>
            /// <param name="sql">SQL���</param>
            /// <returns>�����е��Զ����</returns>
            public static Int32 InsertAndGetIdentity(String sql)
            {
                Int32 rs = DBO.InsertAndGetIdentity(sql, Meta.TableName);
                Cache.Clear();
                return rs;
            }

            /// <summary>
            /// ������������ͨ��ѯSQL��ʽ��Ϊ��ҳSQL��
            /// </summary>
            /// <param name="sql">SQL���</param>
            /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
            /// <param name="maximumRows">��󷵻�����</param>
            /// <param name="keyColumn">Ψһ��������not in��ҳ</param>
            /// <returns>��ҳSQL</returns>
            public static String PageSplit(String sql, Int32 startRowIndex, Int32 maximumRows, String keyColumn)
            {
                return DBO.PageSplit(sql, startRowIndex, maximumRows, keyColumn);
            }
            #endregion

            /// <summary>
            /// ʵ�建��
            /// </summary>
            public static EntityCache<TEntity> Cache = new EntityCache<TEntity>();
        }
        #endregion

        #region ���캯��
        /// <summary>
        /// ��̬����
        /// </summary>
        static Entity()
        {
            CheckModify();
        }
        #endregion

        #region �������
        /// <summary>
        /// ���ؼ�¼��
        /// </summary>
        /// <param name="ds">��¼��</param>
        /// <returns>ʵ������</returns>
        public static List<TEntity> LoadData(DataSet ds)
        {
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1) return new List<TEntity>();
            return LoadData(ds.Tables[0]);
        }

        /// <summary>
        /// �������ݱ�
        /// </summary>
        /// <param name="dt">���ݱ�</param>
        /// <returns>ʵ������</returns>
        protected static List<TEntity> LoadData(DataTable dt)
        {
            if (dt == null || dt.Rows.Count < 1) return new List<TEntity>();
            return LoadData(dt, null);
        }

        /// <summary>
        /// �������ݱ�
        /// </summary>
        /// <param name="dt">���ݱ�</param>
        /// <param name="jointypes"></param>
        /// <returns>ʵ������</returns>
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
        /// ��һ�������ж���������ݡ������ع�������
        /// </summary>
        /// <param name="dr">������</param>
        public virtual void LoadData(DataRow dr)
        {
            if (dr == null) return;
            LoadData(dr, null);
        }

        /// <summary>
        /// ��һ�������ж���������ݡ�ָ��Ҫ������Щ������ʵ�������
        /// </summary>
        /// <param name="dr">������</param>
        /// <param name="jointypes">������</param>
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
        /// ��һ�������ж���������ݡ���ǰ׺��
        /// </summary>
        /// <param name="dr">������</param>
        /// <param name="ps">Ҫ�������ݵ��ֶ�</param>
        /// <returns></returns>
        protected virtual void LoadDataWithPrefix(DataRow dr, FieldItem[] ps)
        {
            if (dr == null) return;
            if (ps == null || ps.Length < 1) ps = Meta.Fields;
            String prefix = Meta.ColumnPrefix;
            foreach (FieldItem fi in ps)
            {
                // ����dr[fi.ColumnName]��Ϊһ��
                Object v = dr[prefix + fi.ColumnNameEx];
                this[fi.Name] = v == DBNull.Value ? null : v;
            }
        }

        /// <summary>
        /// ��һ�������ж���������ݡ�ָ��Ҫ�������ݵ��ֶΣ��Լ�Ҫ������Щ������ʵ�������
        /// </summary>
        /// <param name="dr">������</param>
        /// <param name="hasprefix">�Ƿ����ǰ׺</param>
        /// <param name="ps">Ҫ�������ݵ��ֶ�</param>
        /// <param name="maps">Ҫ������ʵ����</param>
        /// <returns></returns>
        private void LoadData(DataRow dr, Boolean hasprefix, FieldItem[] ps, TableMapAttribute[] maps)
        {
            if (dr == null) return;
            if (ps == null || ps.Length < 1) ps = Meta.Fields;
            String prefix = null;
            if (hasprefix) prefix = Meta.ColumnPrefix;
            foreach (FieldItem fi in ps)
            {
                // ����dr[fi.ColumnName]��Ϊһ��
                Object v = dr[prefix + fi.ColumnNameEx];
                this[fi.Name] = v == DBNull.Value ? null : v;
            }
            //���������Ը�ֵ
            if (maps != null && maps.Length > 0)
            {
                foreach (TableMapAttribute item in maps)
                {
                    LoadDataEx(dr, item);
                }
            }
        }

        /// <summary>
        /// ��һ�������ж���������ݡ������÷���ʵ�֣�Ϊ�˸������ܣ�ʵ����Ӧ�����ظ÷�����
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="map"></param>
        protected virtual void LoadDataEx(DataRow dr, TableMapAttribute map)
        {
            //����һ������
            Object obj = Activator.CreateInstance(map.MapEntity);
            //�ҵ�װ�����ݵķ���
            MethodInfo method = map.MapEntity.GetMethod("LoadDataWithPrefix");
            //���������װ������
            method.Invoke(this, new Object[] { dr, null });
            //���������Ը�ֵ
            map.LocalField.SetValue(this, obj, null);
        }

        /// <summary>
        /// ���ʵ�����е���Щ�ֶ������ݱ���
        /// </summary>
        /// <param name="dt">���ݱ�</param>
        /// <param name="prefix">�ֶ�ǰ׺</param>
        /// <returns></returns>
        private static List<FieldItem> CheckColumn(DataTable dt, String prefix)
        {
            // ���dr���Ƿ��и����Ե��С����ǵ�Select�����ǲ������ģ���ʱ��ֻ��Ҫ�ֲ����
            FieldItem[] allps = Meta.AllFields;
            if (allps == null || allps.Length < 1) return null;

            //�����ǧ����ɾ��allps�е��������Ӱ�쵽ȫ�ֵ�Fields�����
            List<FieldItem> ps = new List<FieldItem>();
            for (Int32 i = allps.Length - 1; i >= 0; i--)
            {
                if (dt.Columns.Contains(prefix + allps[i].ColumnNameEx)) ps.Add(allps[i]);
            }
            return ps;
        }

        ///// <summary>
        ///// �����ݸ��Ƶ������ж����С�
        ///// </summary>
        ///// <param name="dr">������</param>
        //public virtual DataRow ToData(ref DataRow dr)
        //{
        //    if (dr == null) return null;
        //    List<FieldItem> ps = Meta.Fields;
        //    foreach (FieldItem fi in ps)
        //    {
        //        // ���dr���Ƿ��и����Ե��С����ǵ�Select�����ǲ������ģ���ʱ��ֻ��Ҫ�ֲ����
        //        if (dr.Table.Columns.Contains(fi.ColumnName))
        //            dr[fi.ColumnName] = this[fi.Name];
        //    }
        //    return dr;
        //}
        #endregion

        #region ����
        /// <summary>
        /// �Ѹö���־û������ݿ�
        /// </summary>
        /// <returns></returns>
        public virtual Int32 Insert()
        {
            String sql = SQL(this, DataObjectMethodType.Insert);

            //AC��SqlServer֧�ֻ�ȡ�����ֶε����±��
            if (Meta.DbType == DatabaseType.Access ||
                Meta.DbType == DatabaseType.SqlServer)
            {
                //����Ƿ��б�ʶ�У���ʶ����Ҫ���⴦��
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
        /// �������ݿ�
        /// </summary>
        /// <returns></returns>
        public virtual Int32 Update()
        {
            String sql = SQL(this, DataObjectMethodType.Update);
            if (String.IsNullOrEmpty(sql)) return 0;
            return Meta.Execute(sql);
        }

        /// <summary>
        /// �����ݿ���ɾ���ö���
        /// </summary>
        /// <returns></returns>
        public virtual Int32 Delete()
        {
            return Meta.Execute(SQL(this, DataObjectMethodType.Delete));
        }
        #endregion

        #region ���ҵ���ʵ��
        /// <summary>
        /// ���������Լ���Ӧ��ֵ�����ҵ���ʵ��
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
        /// ���������б��Լ���Ӧ��ֵ�б����ҵ���ʵ��
        /// </summary>
        /// <param name="names"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static TEntity Find(String[] names, Object[] values)
        {
            return Find(MakeCondition(names, values, "And"));
        }

        /// <summary>
        /// ����������ѯ��Ϣʵ��������ڱ��༭
        /// </summary>
        ///<param name="__ID">�������</param>
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
        /// �����������ҵ���ʵ��
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

        #region ��̬��ѯ
        /// <summary>
        /// ��ȡ����ʵ����󡣻�ȡ��������ʱ��ǳ���������
        /// </summary>
        /// <returns>ʵ������</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<TEntity> FindAll()
        {
            return LoadData(Meta.Query(xConfig.SQL(Meta.ThisType, DataObjectMethodType.Fill)));
        }

        /// <summary>
        /// ��ѯ������ʵ��������顣
        /// �����Լ������ֶ�������ʹ�������Լ��ֶζ�Ӧ����������������ת��Ϊ����������
        /// </summary>
        /// <param name="whereClause">����������Where</param>
        /// <param name="orderClause">���򣬲���Order By</param>
        /// <param name="selects">��ѯ��</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
        /// <param name="maximumRows">��󷵻�����</param>
        /// <returns>ʵ������</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<TEntity> FindAll(String whereClause, String orderClause, String selects, Int32 startRowIndex, Int32 maximumRows)
        {
            String sql = PageSplitSQL(whereClause, orderClause, selects, startRowIndex, maximumRows);
            return LoadData(Meta.Query(sql));
        }

        /// <summary>
        /// ���������б��Լ���Ӧ��ֵ�б���ȡ����ʵ�����
        /// </summary>
        /// <param name="names">�����б�</param>
        /// <param name="values">ֵ�б�</param>
        /// <returns>ʵ������</returns>
        public static List<TEntity> FindAll(String[] names, Object[] values)
        {
            return FindAll(MakeCondition(names, values, "And"), null, null, 0, 0);
        }

        /// <summary>
        /// ���������Լ���Ӧ��ֵ����ȡ����ʵ�����
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="value">ֵ</param>
        /// <returns>ʵ������</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<TEntity> FindAll(String name, Object value)
        {
            return FindAll(new String[] { name }, new Object[] { value });
        }

        /// <summary>
        /// ���������Լ���Ӧ��ֵ����ȡ����ʵ�����
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="value">ֵ</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
        /// <param name="maximumRows">��󷵻�����</param>
        /// <returns>ʵ������</returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public static List<TEntity> FindAll(String name, Object value, Int32 startRowIndex, Int32 maximumRows)
        {
            if (String.IsNullOrEmpty(name))
                return FindAll(null, null, null, startRowIndex, maximumRows);
            else
                return FindAll(MakeCondition(new String[] { name }, new Object[] { value }, "And"), null, null, startRowIndex, maximumRows);
        }

        /// <summary>
        /// ���������Լ���Ӧ��ֵ����ȡ����ʵ�����
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="value">ֵ</param>
        /// <param name="orderClause">���򣬲���Order By</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
        /// <param name="maximumRows">��󷵻�����</param>
        /// <returns>ʵ������</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static List<TEntity> FindAllByName(String name, Object value, String orderClause, Int32 startRowIndex, Int32 maximumRows)
        {
            if (String.IsNullOrEmpty(name))
                return FindAll(null, orderClause, null, startRowIndex, maximumRows);
            else
                return FindAll(MakeCondition(new String[] { name }, new Object[] { value }, "And"), orderClause, null, startRowIndex, maximumRows);
        }

        /// <summary>
        /// ��ѯSQL������ʵ��������顣
        /// Select������ֱ��ʹ�ò���ָ���Ĳ�ѯ�����в�ѯ���������κ�ת����
        /// </summary>
        /// <param name="sql">��ѯ���</param>
        /// <returns>ʵ������</returns>
        public static List<TEntity> FindAll(String sql)
        {
            return LoadData(Meta.Query(sql));
        }

        /// <summary>
        /// ��ѯ������ʵ��������顣
        /// ���ָ����jointypes��������ͬʱ���ز�����ָ���Ĺ�������
        /// </summary>
        /// <param name="whereClause">����������Where</param>
        /// <param name="orderClause">���򣬲���Order By</param>
        /// <param name="selects">��ѯ��</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
        /// <param name="maximumRows">��󷵻�����</param>
        /// <param name="jointypes">Ҫ������ʵ�������б�</param>
        /// <returns>ʵ������</returns>
        public static List<TEntity> FindAllMultiple(String whereClause, String orderClause, String selects, Int32 startRowIndex, Int32 maximumRows, Type[] jointypes)
        {
            if (jointypes == null || jointypes.Length < 1) return FindAll(whereClause, orderClause, selects, startRowIndex, maximumRows);

            //���ݴ����ʵ�������б�������������Щ������
            TableMapAttribute[] maps = xConfig.TableMaps(Meta.ThisType, jointypes);
            //û���ҵ�����ӳ�����Ե��ֶ�
            if (maps == null || maps.Length < 1) return FindAll(whereClause, orderClause, selects, startRowIndex, maximumRows);

            String LocalTableName = Meta.TableName;
            //׼��ƴ��SQL��ѯ���
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

            //����ÿһ��������ʵ�����ͱ���д���
            foreach (TableMapAttribute item in maps)
            {
                sb.Append(", ");
                sb.Append(xConfig.SelectsEx(item.MapEntity));
            }
            sb.Append(" From ");
            sb.Append(LocalTableName);

            List<String> tables = new List<string>();
            tables.Add(LocalTableName);
            //����ÿһ��������ʵ�����ͱ���д���
            foreach (TableMapAttribute item in maps)
            {
                String tablename = xConfig.TableName(item.MapEntity);
                tables.Add(tablename);
                sb.Append(" ");
                //��������
                sb.Append(item.MapType.ToString().Replace("_", " "));
                sb.Append(" ");
                //������
                sb.Append(tablename);
                sb.Append(" On ");
                sb.AppendFormat("{0}.{1}={2}.{3}", LocalTableName, item.LocalColumn, tablename, item.MapColumn);
            }

            if (!String.IsNullOrEmpty(whereClause))
            {
                //����ǰ׺
                whereClause = Regex.Replace(whereClause, "(w+)", "");
                sb.AppendFormat(" Where {0} ", OqlToSql(whereClause));
            }
            if (!String.IsNullOrEmpty(orderClause))
            {
                //����ǰ׺
                sb.AppendFormat(" Order By {0} ", OqlToSql(orderClause));
            }

            FieldItem fi = Meta.Unique;
            String keyColumn = null;
            if (fi != null)
            {
                keyColumn = Meta.ColumnPrefix + fi.ColumnName;
                // ����Desc��ǣ���ʹ��MaxMin��ҳ�㷨����ʶ�У���һ������Ϊ��������
                if (fi.DataObjectField.IsIdentity || fi.Property.PropertyType == typeof(Int32)) keyColumn += " Desc";
            }
            String sql = Meta.PageSplit(sb.ToString(), startRowIndex, maximumRows, keyColumn);
            DataSet ds = Meta.Query(sql, tables.ToArray());
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1) return null;

            return LoadData(ds.Tables[0], jointypes);
        }

        /// <summary>
        /// ��ѯ���������ļ�¼������ҳ������
        /// </summary>
        /// <param name="qp">��ѯ������</param>
        /// <param name="RecordCount">���������</param>
        /// <returns>ʵ�弯</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static TEntity FindAllByItem(QueryParam qp, out int RecordCount)
        {
            List<TEntity> list = FindAll(qp, out RecordCount);
            return list != null && list.Count > 0 ? list[0] : new TEntity();
        }


        /// <summary>
        /// ��ѯ���������ļ�¼������ҳ������
        /// </summary>
        /// <param name="qp">��ѯ������</param>
        /// <returns>ʵ�弯</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static TEntity FindAllByItem(QueryParam qp)
        {
            Int32 RecordCount = 0;
            return FindAllByItem(qp, out RecordCount);
        }


        /// <summary>
        /// ��ѯ���������ļ�¼������ҳ������
        /// </summary>
        /// <param name="qp">��ѯ������</param>
        /// <param name="RecordCount">���������</param>
        /// <returns>ʵ�弯</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static List<TEntity> FindAll(QueryParam qp, out int RecordCount)
        {
            //���������ֶ�
            string orderClause = null;
            if (qp.Orderfld == string.Empty || qp.Orderfld == "")
            {
                //�����������
                if (Meta.Unique != null)
                    qp.Orderfld = Meta.Unique.Name;
            }

            //�������������޷����������µ��ж�
            if (qp.Orderfld != string.Empty && qp.Orderfld != "")
                orderClause = string.Format("{0} {1}", qp.Orderfld, qp.OrderType == 1 ? "desc" : "asc");

            //�����ѯ����
            String WhereCase = qp.ToSql();

            //���ص�ǰ���������
            qp.RecordCount = RecordCount = FindCount(WhereCase, null, null, 0, 0);
            //�������н����
            return FindAll(WhereCase, orderClause, qp.ReturnFields, qp.startRowIndex, qp.PageSize);
        }

        /// <summary>
        /// ��ѯ���������ļ�¼������ҳ������
        /// </summary>
        /// <param name="qp">��ѯ������</param>
        /// <returns>���������</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static Int32 FindCount(QueryParam qp)
        {
            //���������ֶ�
            string orderClause = string.Empty;
            if (qp.Orderfld == string.Empty)
                qp.Orderfld = Meta.Unique.Name;


            orderClause = string.Format("{0} {1}", qp.Orderfld, qp.OrderType == 1 ? "desc" : "asc");

            //�����ѯ����
            String WhereCase = qp.ToSql();

            //���ص�ǰ���������
            return FindCount(WhereCase, null, null, 0, 0);
        }

        /// <summary>
        /// ִ�в�ѯ�������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С������������к��н������ԡ�
        /// </summary>
        /// <param name="qp">��ѯ������</param>
        /// <returns>���صĽ�����е�һ�еĵ�һ��</returns>
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public static Object FindScalar(QueryParam qp)
        {
            //���������ֶ�
            string orderClause = string.Empty;
            if (qp.Orderfld == string.Empty)
                qp.Orderfld = Meta.Unique.Name;


            orderClause = string.Format("{0} {1}", qp.Orderfld, qp.OrderType == 1 ? "desc" : "asc");

            //�����ѯ����
            String WhereCase = qp.ToSql();

            //���ص�ǰ���������
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

        #region ȡ�ܼ�¼��
        /// <summary>
        /// �����ܼ�¼��
        /// </summary>
        /// <returns></returns>
        public static Int32 FindCount()
        {
            return Meta.QueryCount(xConfig.SQL(Meta.ThisType, DataObjectMethodType.Fill));
        }

        /// <summary>
        /// �����ܼ�¼��
        /// </summary>
        /// <param name="whereClause">����������Where</param>
        /// <param name="orderClause">���򣬲���Order By</param>
        /// <param name="selects">��ѯ��</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
        /// <param name="maximumRows">��󷵻�����</param>
        /// <returns>������</returns>
        public static Int32 FindCount(String whereClause, String orderClause, String selects, Int32 startRowIndex, Int32 maximumRows)
        {
            String sql = PageSplitSQL(whereClause, orderClause, selects, startRowIndex, maximumRows);
            return Meta.QueryCount(sql);
        }


        /// <summary>
        /// ִ�в�ѯ�������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С������������к��н������ԡ�
        /// </summary>
        /// <param name="whereClause">����������Where</param>
        /// <param name="orderClause">���򣬲���Order By</param>
        /// <param name="selects">��ѯ��</param>
        /// <returns>������</returns>
        public static Object FindScalar(String whereClause, String orderClause, String selects)
        {
            String sql = PageSplitSQL(whereClause, orderClause, selects, 0, 1);
            return Meta.ExecuteScalar(sql);
        }

        /// <summary>
        /// ���������б��Լ���Ӧ��ֵ�б������ܼ�¼��
        /// </summary>
        /// <param name="names">�����б�</param>
        /// <param name="values">ֵ�б�</param>
        /// <returns>������</returns>
        public static Int32 FindCount(String[] names, Object[] values)
        {
            return FindCount(MakeCondition(names, values, "And"), null, null, 0, 0);
        }

        /// <summary>
        /// ���������Լ���Ӧ��ֵ�������ܼ�¼��
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="value">ֵ</param>
        /// <returns>������</returns>
        public static Int32 FindCount(String name, Object value)
        {
            return FindCount(name, value,0,0);
        }

        /// <summary>
        /// ���������Լ���Ӧ��ֵ�������ܼ�¼��
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="value">ֵ</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
        /// <param name="maximumRows">��󷵻�����</param>
        /// <returns>������</returns>
        public static Int32 FindCount(String name, Object value, Int32 startRowIndex, Int32 maximumRows)
        {
            if (String.IsNullOrEmpty(name))
                return FindCount(null, null, null, startRowIndex, maximumRows);
            else
                return FindCount(MakeCondition(new String[] { name }, new Object[] { value }, "And"), null, null, startRowIndex, maximumRows);
        }

        /// <summary>
        /// ���������Լ���Ӧ��ֵ�������ܼ�¼��
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="value">ֵ</param>
        /// <param name="orderClause">���򣬲���Order By</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
        /// <param name="maximumRows">��󷵻�����</param>
        /// <returns>������</returns>
        public static Int32 FindCountByName(String name, Object value, String orderClause, int startRowIndex, int maximumRows)
        {
            if (String.IsNullOrEmpty(name))
                return FindCount(null, orderClause, null, startRowIndex, maximumRows);
            else
                return FindCount(MakeCondition(new String[] { name }, new Object[] { value }, "And"), orderClause, null, startRowIndex, maximumRows);
        }
        #endregion

        #region ��̬����
        /// <summary>
        /// ��һ��ʵ�����־û������ݿ�
        /// </summary>
        /// <param name="obj">ʵ�����</param>
        /// <returns>������Ӱ�������</returns>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public static Int32 Insert(TEntity obj)
        {
            return obj.Insert();
        }

        /// <summary>
        /// ��һ��ʵ�����־û������ݿ�
        /// </summary>
        /// <param name="names">���������б�</param>
        /// <param name="values">����ֵ�б�</param>
        /// <returns>������Ӱ�������</returns>
        public static Int32 Insert(String[] names, Object[] values)
        {
            if (names == null) throw new ArgumentNullException("names", "�����б��ֵ�б���Ϊ��");
            if (values == null) throw new ArgumentNullException("values", "�����б��ֵ�б���Ϊ��");

            if (names.Length != values.Length) throw new ArgumentException("�����б�����ֵ�б�һһ��Ӧ");
            FieldItem[] fis = Meta.Fields;
            Dictionary<String, FieldItem> fs = new Dictionary<String, FieldItem>();
            foreach (FieldItem fi in fis)
                fs.Add(fi.Name, fi);
            StringBuilder sbn = new StringBuilder();
            StringBuilder sbv = new StringBuilder();
            for (Int32 i = 0; i < names.Length; i++)
            {
                if (!fs.ContainsKey(names[i])) throw new ArgumentException("��[" + Meta.ThisType.FullName + "]�в�����[" + names[i] + "]����");
                // ͬʱ����SQL��䡣names�������б�����ת���ɶ�Ӧ���ֶ��б�
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
        /// ��һ��ʵ�������µ����ݿ�
        /// </summary>
        /// <param name="obj">ʵ�����</param>
        /// <returns>������Ӱ�������</returns>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public static Int32 Update(TEntity obj)
        {
            return obj.Update();
        }

        /// <summary>
        /// ����һ��ʵ������
        /// </summary>
        /// <param name="setClause">Ҫ���µ��������</param>
        /// <param name="whereClause">ָ��Ҫ���µ�ʵ��</param>
        /// <returns></returns>
        public static Int32 Update(String setClause, String whereClause)
        {
            if (String.IsNullOrEmpty(setClause) || !setClause.Contains("=")) throw new ArgumentException("Incorrect parameter");
            String sql = String.Format("Update {0} Set {1}", Meta.TableName, setClause);
            if (!String.IsNullOrEmpty(whereClause)) sql += " Where " + whereClause;
            return Meta.Execute(sql);
        }

        /// <summary>
        /// ����һ��ʵ������
        /// </summary>
        /// <param name="setNames">���������б�</param>
        /// <param name="setValues">����ֵ�б�</param>
        /// <param name="whereNames">���������б�</param>
        /// <param name="whereValues">����ֵ�б�</param>
        /// <returns>������Ӱ�������</returns>
        public static Int32 Update(String[] setNames, Object[] setValues, String[] whereNames, Object[] whereValues)
        {
            String sc = MakeCondition(setNames, setValues, ", ");
            String wc = MakeCondition(whereNames, whereValues, " And ");
            return Update(sc, wc);
        }

        /// <summary>
        /// �����ݿ���ɾ��ָ��ʵ�����
        /// ʵ����Ӧ��ʵ�ָ÷�������һ����������Ψһ����������Ϊ����
        /// </summary>
        /// <param name="obj">ʵ�����</param>
        /// <returns>������Ӱ����������������жϱ�ɾ���˶����У��Ӷ�֪�������Ƿ�ɹ�</returns>
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public static Int32 Delete(TEntity obj)
        {
            return obj.Delete();
        }

        /// <summary>
        /// �����ݿ���ɾ��ָ��������ʵ�����
        /// </summary>
        /// <param name="whereClause">��������</param>
        /// <returns></returns>
        public static Int32 Delete(String whereClause)
        {
            String sql = String.Format("Delete From {0}", Meta.TableName);
            if (!String.IsNullOrEmpty(whereClause)) sql += " Where " + whereClause;
            return Meta.Execute(sql);
        }

        /// <summary>
        /// �����ݿ���ɾ��ָ�������б��ֵ�б����޶���ʵ�����
        /// </summary>
        /// <param name="names">�����б�</param>
        /// <param name="values">ֵ�б�</param>
        /// <returns></returns>
        public static Int32 Delete(String[] names, Object[] values)
        {
            return Delete(MakeCondition(names, values, "And"));
        }
        #endregion

        #region ��������
        private static DateTime year1900 = new DateTime(1900, 1, 1);
        private static DateTime year1753 = new DateTime(1753, 1, 1);
        private static DateTime year9999 = new DateTime(9999, 1, 1);
        /// <summary>
        /// ȡ��һ��ֵ��Sqlֵ��
        /// �����ֵ���ַ�������ʱ�����ڸ�ֵǰ��ӵ����ţ�
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="field">�ֶ�����</param>
        /// <returns>Sqlֵ���ַ�����ʽ</returns>
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
                // SqlServer�ܾ������䲻��ʶ��Ϊ 1753 �굽 9999 �������ڵ�ֵ
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
        /// ��SQLģ���ʽ��ΪSQL���
        /// </summary>
        /// <param name="obj">ʵ�����</param>
        /// <param name="methodType"></param>
        /// <returns>SQL�ַ���</returns>
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
                    // û�б�ʶ�к�����������ȡ�������ݵ����
                    if (String.IsNullOrEmpty(sql)) return null;
                    //return String.Format("Select {0} From {1} Where {2}", Meta.Selects, Meta.TableName, sql);
                    return String.Format("Select * From {0} Where {1}", Meta.TableName, sql);
                case DataObjectMethodType.Insert:
                    sbNames = new StringBuilder();
                    sbValues = new StringBuilder();
                    // ֻ����û�в������
                    foreach (FieldItem fi in Meta.Fields)
                    {
                        // ��ʶ�в���Ҫ���룬������Ͷ���Ҫ
                        if (!fi.DataObjectField.IsIdentity)
                        {
                            if (!isFirst) sbNames.Append(", "); // �Ӷ���
                            sbNames.Append(fi.ColumnName);
                            if (!isFirst)
                                sbValues.Append(", "); // �Ӷ���
                            else
                                isFirst = false;
                            sbValues.Append(SqlDataFormat(obj[fi.Name], fi)); // ����
                        }
                    }
                    return String.Format("Insert Into {0}({1}) Values({2})", Meta.TableName, sbNames.ToString(), sbValues.ToString());
                case DataObjectMethodType.Update:
                    sbNames = new StringBuilder();
                    // ֻ����û�и��²���
                    foreach (FieldItem fi in Meta.Fields)
                    {
                        if (fi.DataObjectField.IsIdentity) continue;

                        //�������ж�
                        if (!obj.Dirtys[fi.Name]) continue;

                        if (!isFirst)
                            sbNames.Append(", "); // �Ӷ���
                        else
                            isFirst = false;
                        sbNames.Append(fi.ColumnName);
                        sbNames.Append("=");
                        sbNames.Append(SqlDataFormat(obj[fi.Name], fi)); // ����
                    }

                    if (sbNames.Length <= 0) return null;

                    sql = DefaultCondition(obj);
                    if (String.IsNullOrEmpty(sql)) return null;
                    return String.Format("Update {0} Set {1} Where {2}", Meta.TableName, sbNames.ToString(), sql);
                case DataObjectMethodType.Delete:
                    // ��ʶ����Ϊɾ���ؼ���
                    sql = DefaultCondition(obj);
                    if (String.IsNullOrEmpty(sql))
                        return null;
                    return String.Format("Delete From {0} Where {1}", Meta.TableName, sql);
            }
            return null;
        }

        /// <summary>
        /// ���������б��ֵ�б������ѯ������
        /// ���繹����������Ʋ�ѯ������
        /// </summary>
        /// <param name="names">�����б�</param>
        /// <param name="values">ֵ�б�</param>
        /// <param name="action">���Ϸ�ʽ</param>
        /// <returns>�����Ӵ�</returns>
        protected static String MakeCondition(String[] names, Object[] values, String action)
        {
            if (names == null) throw new ArgumentNullException("names", "�����б��ֵ�б���Ϊ��");
            if (values == null) throw new ArgumentNullException("values", "�����б��ֵ�б���Ϊ��");

            if (names.Length != values.Length) throw new ArgumentException("�����б�����ֵ�б�һһ��Ӧ");
            Dictionary<String, FieldItem> fs = new Dictionary<String, FieldItem>();
            foreach (FieldItem fi in Meta.Fields)
                fs.Add(fi.Name.ToLower(), fi);
            StringBuilder sb = new StringBuilder();
            for (Int32 i = 0; i < names.Length; i++)
            {
                if (!fs.ContainsKey(names[i].ToLower())) throw new ArgumentException("��[" + Meta.ThisType.FullName + "]�в�����[" + names[i] + "]����");
                // ͬʱ����SQL��䡣names�������б�����ת���ɶ�Ӧ���ֶ��б�
                if (i > 0) sb.AppendFormat(" {0} ", action);
                sb.AppendFormat("{0}={1}", fs[names[i].ToLower()].ColumnName, SqlDataFormat(values[i], fs[names[i].ToLower()]));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Ĭ��������
        /// ���б�ʶ�У���ʹ��һ����ʶ����Ϊ������
        /// ������������ʹ��ȫ��������Ϊ������
        /// </summary>
        /// <param name="obj">ʵ�����</param>
        /// <returns>����</returns>
        protected static String DefaultCondition(Entity<TEntity> obj)
        {
            Type t = obj.GetType();
            // Ψһ����Ϊ��ѯ�ؼ���
            FieldItem[] ps = Meta.Uniques;
            // û�б�ʶ�к�����������ȡ�������ݵ����
            if (ps == null || ps.Length < 1) return null;
            // ��ʶ����Ϊ��ѯ�ؼ���
            if (ps[0].DataObjectField.IsIdentity)
            {
                return String.Format("{0}={1}", ps[0].ColumnName, SqlDataFormat((obj as TEntity)[ps[0].Name], ps[0]));
            }
            // ������Ϊ��ѯ�ؼ���
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
        /// �Ѷ���Oqlת����Ϊ��׼TSql
        /// </summary>
        /// <param name="oql">ʵ�����oql</param>
        /// <returns>Sql�ַ���</returns>
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
        /// ȡ��ָ��ʵ�����͵ķ�ҳSQL
        /// </summary>
        /// <param name="whereClause">����������Where</param>
        /// <param name="orderClause">���򣬲���Order By</param>
        /// <param name="selects">��ѯ��</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
        /// <param name="maximumRows">��󷵻�����</param>
        /// <returns>��ҳSQL</returns>
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
        /// ȡ��ָ��ʵ�����͵ķ�ҳSQL
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
        /// <param name="maximumRows">��󷵻�����</param>
        /// <returns>��ҳSQL</returns>
        protected static String PageSplitSQL(String sql, Int32 startRowIndex, Int32 maximumRows)
        {
            FieldItem fi = Meta.Unique;
            String keyColumn = null;
            if (fi != null)
            {
                keyColumn = fi.ColumnName;
                // ����Desc��ǣ���ʹ��MaxMin��ҳ�㷨����ʶ�У���һ������Ϊ��������
                if (fi.DataObjectField.IsIdentity || fi.Property.PropertyType == typeof(Int32)) keyColumn += " Desc";
            }
            return Meta.PageSplit(sql, startRowIndex, maximumRows, keyColumn);
        }
        #endregion

        #region ��ȡ/���� �ֶ�ֵ
        /// <summary>
        /// ��ȡ/���� �ֶ�ֵ��
        /// һ������������ʵ�֡�
        /// ����ʵ�������д���������Ա��ⷢ��������������
        /// </summary>
        /// <param name="name">�ֶ���</param>
        /// <returns></returns>
        public virtual Object this[String name]
        {
            get
            {
                foreach (FieldItem fi in Meta.Fields)
                    if (fi.Name == name) return fi.Property.GetValue(this, null);
                throw new ArgumentException("��[" + this.GetType().FullName + "]�в�����[" + name + "]����");
            }
            set
            {
                foreach (FieldItem fi in Meta.Fields)
                    if (fi.Name == name) { fi.Property.SetValue(this, value, null); return; }
                throw new ArgumentException("��[" + this.GetType().FullName + "]�в�����[" + name + "]����");
            }
        }
        #endregion

        #region ���뵼��XML
        /// <summary>
        /// ����XML
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
        /// ����
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
        ///// �߼����л�
        ///// </summary>
        ///// <param name="writer">�ı���д��</param>
        ///// <param name="propertyAsAttribute">������ΪXml���Խ������л�</param>
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
        ///// �߼����л�
        ///// </summary>
        ///// <param name="propertyAsAttribute">������ΪXml���Խ������л�</param>
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

        #region ��¡
        /// <summary>
        /// ������ǰ����Ŀ�¡���󣬽����������ֶ�
        /// </summary>
        /// <returns></returns>
        public virtual Object Clone()
        {
            return CloneEntity();
        }

        /// <summary>
        /// ��¡ʵ�塣������ǰ����Ŀ�¡���󣬽����������ֶ�
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

        #region ����
        /// <summary>
        /// �����ء�
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Meta.FieldNames.Contains("Name"))
                return this["Name"] == null ? null : this["Name"].ToString();
            else if (Meta.FieldNames.Contains("ID"))
                return this["ID"] == null ? null : this["ID"].ToString();
            else
                return "ʵ��" + Meta.ThisType.Name;
        }
        #endregion

        #region ������
        [NonSerialized]
        private DirtyCollection _Dirtys;
        /// <summary>�����ԡ��洢��Щ���Ե����ݱ��޸Ĺ��ˡ�</summary>
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
        /// �����������ݵ�������
        /// </summary>
        /// <param name="isDirty">�ı������Ե����Ը���</param>
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
        /// ���Ըı䡣����ʱ�ǵõ��û���ĸ÷��������������������ԣ��������ݽ��޷�Update�����ݿ⡣
        /// </summary>
        /// <param name="fieldName">�ֶ���</param>
        /// <param name="newValue">������ֵ</param>
        /// <returns>�Ƿ�����ı�</returns>
        protected virtual Boolean OnPropertyChange(String fieldName, Object newValue)
        {
            Dirtys[fieldName] = true;
            return true;
        }
        #endregion

        #region �Զ��޸����ݱ�ṹ
        private static Object schemasLock = new Object();
        private static Boolean hasChecked = false;
        /// <summary>
        /// ������ݱ�ܹ��Ƿ��ѱ��޸�
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