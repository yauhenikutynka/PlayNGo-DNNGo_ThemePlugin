using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
 

namespace DNNGo.Modules.ThemePlugin
{
    #region 数据库类型
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// MS的Access文件数据库
        /// </summary>
        Access = 0,

        /// <summary>
        /// MS的SqlServer数据库
        /// </summary>
        SqlServer = 1,

        /// <summary>
        /// Oracle数据库
        /// </summary>
        Oracle = 2,

        /// <summary>
        /// MySql数据库
        /// </summary>
        MySql = 3,

        /// <summary>
        /// MS的SqlServer2005数据库
        /// </summary>
        SqlServer2005 = 4,

        /// <summary>
        /// SQLite数据库
        /// </summary>
        SQLite = 5
    }
    #endregion

    /// <summary>
    /// 数据库基类。
    /// 声明为public，程序集以外对象可以集成，方便编写别的数据库子类。
    /// 基类为各数据库类定制了一个框架，默认使用Access。
    /// SqlServer和Oracle可重写以提高性能
    /// </summary>
    internal abstract class DataBase : IDataBase, IDisposable
    {
        #region 构造函数
        /// <summary>
        /// 构造一个数据访问对象
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        /// <param name="dbProviderFactory">数据库提供者工厂</param>
        public DataBase(String connStr, DbProviderFactory dbProviderFactory)
        {
            if (!String.IsNullOrEmpty(connStr))
            {
                Conn = _dbProviderFactory.CreateConnection();
                Conn.ConnectionString = connStr;
            }
            if (dbProviderFactory != null) _dbProviderFactory = dbProviderFactory;
        }
        /// <summary>
        /// 构造一个数据访问对象
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="dbProviderFactory">数据库提供者工厂</param>
        public DataBase(DbConnection conn, DbProviderFactory dbProviderFactory)
        {
            Conn = conn;
            _dbProviderFactory = dbProviderFactory;
        }

        /// <summary>
        /// 是否已经释放
        /// </summary>
        private Boolean IsDisposed = false;
        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            if (IsDisposed) return;
            try
            {
                // 注意，没有Commit的数据，在这里将会被回滚
                //if (Trans != null) Rollback();
                if (Trans != null && Opened) Trans.Rollback();
                if (Conn != null) Close();
                IsDisposed = true;
            }
            catch (Exception ex)
            {
                XTrace.WriteLine("执行" + this.GetType().FullName + "的Dispose时出错：" + ex.ToString());
            }
        }

        ~DataBase()
        {
            Dispose();
        }
        #endregion

        #region 属性
        private readonly DbProviderFactory _dbProviderFactory;
        private DbConnection _Conn;
        /// <summary>
        /// 数据连接对象。
        /// </summary>
        protected DbConnection Conn
        {
            get { return _Conn; }
            set { _Conn = value; }
        }
        /// <summary>
        /// 返回数据库类型。外部DAL数据库类请使用Other
        /// </summary>
        public abstract DatabaseType DbType { get; }

        private Int32 _QueryTimes;
        /// <summary>
        /// 查询次数
        /// </summary>
        public Int32 QueryTimes
        {
            get { return _QueryTimes; }
            set { _QueryTimes = value; }
        }

        private Int32 _ExecuteTimes;
        /// <summary>
        /// 执行次数
        /// </summary>
        public Int32 ExecuteTimes
        {
            get { return _ExecuteTimes; }
            set { _ExecuteTimes = value; }
        }

        private static Int32 gid = 0;

        private Int32? _ID;
        /// <summary>
        /// 标识
        /// </summary>
        public Int32 ID
        {
            get
            {
                if (_ID == null) _ID = ++gid;
                return _ID.Value;
            }
        }
        #endregion

        #region 打开/关闭
        private Boolean _IsAutoClose = true;
        /// <summary>
        /// 是否自动关闭。
        /// 启用事务后，该设置无效。
        /// 在提交或回滚事务时，如果IsAutoClose为true，则会自动关闭
        /// </summary>
        public Boolean IsAutoClose
        {
            get { return _IsAutoClose; }
            set { _IsAutoClose = value; }
        }

        /// <summary>
        /// 连接是否已经打开
        /// </summary>
        public Boolean Opened
        {
            get { return Conn != null && Conn.State != ConnectionState.Closed; }
        }

        /// <summary>
        /// 打开
        /// </summary>
        public virtual void Open()
        {
            if (Conn != null && Conn.State == ConnectionState.Closed)
            {
                try { Conn.Open(); }
                catch (Exception ex)
                {
                    XTrace.WriteLine("执行" + this.GetType().FullName + "的Open时出错：" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void Close()
        {
            if (Conn != null && Conn.State != ConnectionState.Closed)
            {
                try { Conn.Close(); }
                catch (Exception ex)
                {
                    XTrace.WriteLine("执行" + this.GetType().FullName + "的Close时出错：" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// 自动关闭。
        /// 启用事务后，不关闭连接。
        /// 在提交或回滚事务时，如果IsAutoClose为true，则会自动关闭
        /// </summary>
        public void AutoClose()
        {
            if (IsAutoClose && Trans == null && Opened) Close();
        }

        /// <summary>
        /// 当异常发生时触发。关闭数据库连接，或者返还连接到连接池。
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected virtual Exception OnException(Exception ex)
        {
            if (Trans == null && Opened) Close(); // 强制关闭数据库
            //return new XException("内部数据库实体" + this.GetType().FullName + "异常，执行" + Environment.StackTrace + "方法出错！", ex);
            String err = "内部数据库实体" + DbType.ToString() + "异常，执行方法出错！" + Environment.NewLine + ex.Message;
            if (ex != null)
                return new Exception(err, ex);
            else
                return new Exception(err);
        }

        /// <summary>
        /// 当异常发生时触发。关闭数据库连接，或者返还连接到连接池。
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected virtual Exception OnException(Exception ex, String sql)
        {
            if (Trans == null && Opened) Close(); // 强制关闭数据库
            //return new XException("内部数据库实体" + this.GetType().FullName + "异常，执行" + Environment.StackTrace + "方法出错！", ex);
            String err = "execute data:" + DbType.ToString() + " Exception，Error execution method!" + Environment.NewLine;
            if (!String.IsNullOrEmpty(sql)) err += "SQL：" + sql + Environment.NewLine;
            err += ex.Message;
            if (ex != null)
                return new Exception(err, ex);
            else
                return new Exception(err);
        }
        #endregion

        #region 事务
        private DbTransaction _Trans;
        /// <summary>
        /// 数据库事务
        /// </summary>
        protected DbTransaction Trans
        {
            get { return _Trans; }
            set { _Trans = value; }
        }

        /// <summary>
        /// 事务计数。
        /// 当且仅当事务计数等于1时，才提交或回滚。
        /// </summary>
        private Int32 TransactionCount = 0;

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns></returns>
        public DbTransaction BeginTransaction()
        {
            if (Debug) WriteLog("开始事务：{0}", ID);

            TransactionCount++;
            if (TransactionCount > 1) return Trans;

            try
            {
                if (!Opened) Open();
                Trans = Conn.BeginTransaction();
                TransactionCount = 1;
                return Trans;
            }
            catch (Exception ex)
            {
                throw OnException(ex);
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (Debug) WriteLog("提交事务：{0}", ID);

            TransactionCount--;
            if (TransactionCount > 0) return;

            if (Trans == null) throw new InvalidOperationException("当前并未开始事务，请用BeginTransaction方法开始新事务！ID=" + ID);
            try
            {
                Trans.Commit();
                Trans = null;
                if (IsAutoClose) Close();
            }
            catch (Exception ex)
            {
                throw OnException(ex);
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            if (Debug) WriteLog("回滚事务：{0}", ID);

            TransactionCount--;
            if (TransactionCount > 0) return;

            if (Trans == null) throw new InvalidOperationException("当前并未开始事务，请用BeginTransaction方法开始新事务！ID=" + ID);
            try
            {
                Trans.Rollback();
                Trans = null;
                if (IsAutoClose) Close();
            }
            catch (Exception ex)
            {
                throw OnException(ex);
            }
        }
        #endregion

        #region 基本方法 查询/执行
        /// <summary>
        /// 执行SQL查询，返回记录集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public virtual DataSet Query(String sql)
        {
            QueryTimes++;
            if (SqlLog) XTrace.WriteLine("Sql输出：" + sql);
            try
            {
                DbCommand cmd = PrepareCommand();
                cmd.CommandText = sql;
                using (DbDataAdapter da = _dbProviderFactory.CreateDataAdapter())
                {
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    AutoClose();
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw OnException(ex, sql);
            }
        }

        /// <summary>
        /// 执行DbCommand，返回记录集
        /// </summary>
        /// <param name="cmd">DbCommand</param>
        /// <returns></returns>
        public virtual DataSet Query(DbCommand cmd)
        {
            QueryTimes++;
            using (DbDataAdapter da = _dbProviderFactory.CreateDataAdapter())
            {
                try
                {
                    if (!Opened) Open();
                    cmd.Connection = Conn;
                    if (Trans != null) cmd.Transaction = Trans;
                    da.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    AutoClose();
                    return ds;
                }
                catch (Exception ex)
                {
                    throw OnException(ex, cmd.CommandText);
                }
            }
        }



        /// <summary>
        /// 执行SQL查询，执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public virtual Object ExecuteScalar(String sql)
        {
            ExecuteTimes++;
            if (SqlLog) XTrace.WriteLine("Sql输出：" + sql);
            try
            {
                DbCommand cmd = PrepareCommand();
                cmd.CommandText = sql;
                Object rs = cmd.ExecuteScalar();
                AutoClose();
                return rs;
            }
            catch (Exception ex)
            {
                throw OnException(ex, sql);
            }
        }

        /// <summary>
        /// 执行DbCommand，执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="cmd">DbCommand</param>
        /// <returns></returns>
        public virtual Object ExecuteScalar(DbCommand cmd)
        {
            ExecuteTimes++;
            try
            {
                if (!Opened) Open();
                cmd.Connection = Conn;
                if (Trans != null) cmd.Transaction = Trans;
                Object rs = cmd.ExecuteScalar();
                AutoClose();
                return rs;
            }
            catch (Exception ex)
            {
                throw OnException(ex, cmd.CommandText);
            }
        }


        /// <summary>
        /// 执行SQL查询，返回总记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public virtual Int32 QueryCount(String sql)
        {
            String orderBy = CheckOrderClause(ref sql);
            sql = String.Format("Select Count(*) From {0}", CheckSimpleSQL(sql));

            QueryTimes++;
            DbCommand cmd = PrepareCommand();
            cmd.CommandText = sql;
            if (SqlLog) XTrace.WriteLine("Sql输出：" + cmd.CommandText);
            try
            {
                Int32 rs = Convert.ToInt32(cmd.ExecuteScalar());
                AutoClose();
                return rs;
            }
            catch (Exception ex)
            {
                throw OnException(ex, cmd.CommandText);
            }
        }

        /// <summary>
        /// 执行SQL语句，返回受影响的行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public virtual Int32 Execute(String sql)
        {
            ExecuteTimes++;
            if (SqlLog) XTrace.WriteLine("Sql输出：" + sql);
            try
            {
                DbCommand cmd = PrepareCommand();
                cmd.CommandText = sql;
                Int32 rs = cmd.ExecuteNonQuery();
                AutoClose();
                return rs;
            }
            catch (Exception ex)
            {
                throw OnException(ex, sql);
            }
        }

        /// <summary>
        /// 执行DbCommand，返回受影响的行数
        /// </summary>
        /// <param name="cmd">DbCommand</param>
        /// <returns></returns>
        public virtual Int32 Execute(DbCommand cmd)
        {
            ExecuteTimes++;
            try
            {
                if (!Opened) Open();
                cmd.Connection = Conn;
                if (Trans != null) cmd.Transaction = Trans;
                Int32 rs = cmd.ExecuteNonQuery();
                AutoClose();
                return rs;
            }
            catch (Exception ex)
            {
                throw OnException(ex, cmd.CommandText);
            }
        }

        /// <summary>
        /// 执行插入语句并返回新增行的自动编号
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>新增行的自动编号</returns>
        public virtual Int32 InsertAndGetIdentity(String sql)
        {
            ExecuteTimes++;
            //SQLServer写法
            sql = "SET NOCOUNT ON;" + sql + ";Select SCOPE_IDENTITY()";
            if (SqlLog) XTrace.WriteLine("Sql输出：" + sql);
            try
            {
                DbCommand cmd = PrepareCommand();
                cmd.CommandText = sql;
                Int32 rs = Int32.Parse(cmd.ExecuteScalar().ToString());
                AutoClose();
                return rs;
            }
            catch (Exception ex)
            {
                throw OnException(ex, sql);
            }
        }

        /// <summary>
        /// 获取一个DbCommand。
        /// 配置了连接，并关联了事务。
        /// 连接已打开。
        /// 使用完毕后，必须调用AutoClose方法，以使得在非事务及设置了自动关闭的情况下关闭连接
        /// </summary>
        /// <returns></returns>
        public virtual DbCommand PrepareCommand()
        {
            DbCommand cmd = _dbProviderFactory.CreateCommand();
            if (!Opened) Open();
            cmd.Connection = Conn;
            if (Trans != null) cmd.Transaction = Trans;
            return cmd;
        }
        #endregion

        #region 分页
        /// <summary>
        /// 构造分页SQL
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="startRowIndex">开始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <param name="keyColumn">唯一键。用于not in分页</param>
        /// <returns>分页SQL</returns>
        public virtual String PageSplit(String sql, Int32 startRowIndex, Int32 maximumRows, String keyColumn)
        {
            if (String.IsNullOrEmpty(keyColumn)) throw new ArgumentNullException("keyColumn", "这里用的not in分页算法要求指定主键列！");

            // 从第一行开始，不需要分页
            if (startRowIndex <= 0 && maximumRows < 1) return sql;

            #region Max/Min分页
            // 如果要使用max/min分页法，首先keyColumn必须有asc或者desc
            if (keyColumn.ToLower().EndsWith(" desc") || keyColumn.ToLower().EndsWith(" asc") || keyColumn.ToLower().EndsWith(" unknown"))
            {
                String str = PageSplitMaxMin(sql, startRowIndex, maximumRows, keyColumn);
                if (!String.IsNullOrEmpty(str)) return str;
                keyColumn = keyColumn.Substring(0, keyColumn.IndexOf(" "));
            }
            #endregion

            //检查简单SQL。为了让生成分页SQL更短
            String tablename = CheckSimpleSQL(sql);
            if (tablename != sql)
                sql = tablename;
            else
                sql = String.Format("({0}) XCode_Temp_a", sql);

            // 取第一页也不用分页。把这代码放到这里，主要是数字分页中要自己处理这种情况
            if (startRowIndex <= 0 && maximumRows > 0)
                return String.Format("Select Top {0} * From {1}", maximumRows, sql);

            if (maximumRows < 1)
                sql = String.Format("Select * From {1} Where {2} Not In(Select Top {0} {2} From {1})", startRowIndex, sql, keyColumn);
            else
                sql = String.Format("Select Top {0} * From {1} Where {2} Not In(Select Top {3} {2} From {1})", maximumRows, sql, keyColumn, startRowIndex);
            return sql;
        }

        protected String PageSplitMaxMin(String sql, Int32 startRowIndex, Int32 maximumRows, String keyColumn)
        {
            // 唯一键的顺序。默认为Empty，可以为asc或desc，如果有，则表明主键列是数字唯一列，可以使用max/min分页法
            Boolean isAscOrder = keyColumn.ToLower().EndsWith(" asc");
            // 是否使用max/min分页法
            Boolean canMaxMin = false;

            // 如果sql最外层有排序，且唯一的一个排序字段就是keyColumn时，可用max/min分页法
            // 如果sql最外层没有排序，其排序不是unknown，可用max/min分页法
            MatchCollection ms = Regex.Matches(sql, @"\border\s*by\b([^)]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (ms != null && ms.Count > 0 && ms[0].Index > 0)
            {
                // 取第一页也不用分页。把这代码放到这里，主要是数字分页中要自己处理这种情况
                if (startRowIndex <= 0 && maximumRows > 0)
                    return String.Format("Select Top {0} * From {1}", maximumRows, CheckSimpleSQL(sql));

                keyColumn = keyColumn.Substring(0, keyColumn.IndexOf(" "));
                sql = sql.Substring(0, ms[0].Index);

                String strOrderBy = ms[0].Groups[1].Value.Trim();
                // 只有一个排序字段
                if (!String.IsNullOrEmpty(strOrderBy) && !strOrderBy.Contains(","))
                {
                    // 有asc或者desc。没有时，默认为asc
                    if (strOrderBy.ToLower().EndsWith(" desc"))
                    {
                        String str = strOrderBy.Substring(0, strOrderBy.Length - " desc".Length).Trim();
                        // 排序字段等于keyColumn
                        if (str.ToLower() == keyColumn.ToLower())
                        {
                            isAscOrder = false;
                            canMaxMin = true;
                        }
                    }
                    else if (strOrderBy.ToLower().EndsWith(" asc"))
                    {
                        String str = strOrderBy.Substring(0, strOrderBy.Length - " asc".Length).Trim();
                        // 排序字段等于keyColumn
                        if (str.ToLower() == keyColumn.ToLower())
                        {
                            isAscOrder = true;
                            canMaxMin = true;
                        }
                    }
                    else if (!strOrderBy.Contains(" ")) // 不含空格，是唯一排序字段
                    {
                        // 排序字段等于keyColumn
                        if (strOrderBy.ToLower() == keyColumn.ToLower())
                        {
                            isAscOrder = true;
                            canMaxMin = true;
                        }
                    }
                }
            }
            else
            {
                // 取第一页也不用分页。把这代码放到这里，主要是数字分页中要自己处理这种情况
                if (startRowIndex <= 0 && maximumRows > 0)
                {
                    //数字分页中，业务上一般使用降序，Entity类会给keyColumn指定降序的
                    //但是，在第一页的时候，没有用到keyColumn，而数据库一般默认是升序
                    //这时候就会出现第一页是升序，后面页是降序的情况了。这里改正这个BUG
                    if (keyColumn.ToLower().EndsWith(" desc") || keyColumn.ToLower().EndsWith(" asc"))
                        return String.Format("Select Top {0} * From {1} Order By {2}", maximumRows, CheckSimpleSQL(sql), keyColumn);
                    else
                        return String.Format("Select Top {0} * From {1}", maximumRows, CheckSimpleSQL(sql));
                }

                if (!keyColumn.ToLower().EndsWith(" unknown")) canMaxMin = true;

                keyColumn = keyColumn.Substring(0, keyColumn.IndexOf(" "));
            }

            if (canMaxMin)
            {
                if (maximumRows < 1)
                    sql = String.Format("Select * From {1} Where {2}{3}(Select {4}({2}) From (Select Top {0} {2} From {1} Order By {2} {5}) XCode_Temp_a) Order By {2} {5}", startRowIndex, CheckSimpleSQL(sql), keyColumn, isAscOrder ? ">" : "<", isAscOrder ? "max" : "min", isAscOrder ? "Asc" : "Desc");
                else
                    sql = String.Format("Select Top {0} * From {1} Where {2}{4}(Select {5}({2}) From (Select Top {3} {2} From {1} Order By {2} {6}) XCode_Temp_a) Order By {2} {6}", maximumRows, CheckSimpleSQL(sql), keyColumn, startRowIndex, isAscOrder ? ">" : "<", isAscOrder ? "max" : "min", isAscOrder ? "Asc" : "Desc");
                return sql;
            }
            return null;
        }

        /// <summary>
        /// 检查简单SQL语句，比如Select * From table
        /// </summary>
        /// <param name="sql">待检查SQL语句</param>
        /// <returns>如果是简单SQL语句则返回表名，否则返回子查询(sql) XCode_Temp_a</returns>
        protected static String CheckSimpleSQL(String sql)
        {
            if (String.IsNullOrEmpty(sql)) return sql;

            Regex reg = new Regex(@"^\s*select\s+\*\s+from\s+([\w\[\]]+)\s*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection ms = reg.Matches(sql);
            if (ms == null || ms.Count < 1 || ms[0].Groups.Count < 2 ||
                String.IsNullOrEmpty(ms[0].Groups[1].Value)) return String.Format("({0}) XCode_Temp_a", sql);
            return ms[0].Groups[1].Value;
        }

        /// <summary>
        /// 检查是否以Order子句结尾，如果是，分割sql为前后两部分
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected static String CheckOrderClause(ref String sql)
        {
            if (!sql.ToLower().Contains("order")) return null;

            // 使用正则进行严格判断。必须包含Order By，并且它右边没有右括号)，表明有order by，且不是子查询的，才需要特殊处理
            MatchCollection ms = Regex.Matches(sql, @"\border\s*by\b([^)]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (ms == null || ms.Count < 1 || ms[0].Index < 1) return null;
            String orderBy = sql.Substring(ms[0].Index).Trim();
            sql = sql.Substring(0, ms[0].Index).Trim();

            return orderBy;
        }
        #endregion

        #region 构架
        /// <summary>
        /// 返回数据源的架构信息
        /// </summary>
        /// <param name="collectionName">指定要返回的架构的名称。</param>
        /// <param name="restrictionValues">为请求的架构指定一组限制值。</param>
        /// <returns></returns>
        public virtual DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            if (!Opened) Open();

            DataTable dt;
            if (restrictionValues == null || restrictionValues.Length < 1)
            {
                if (String.IsNullOrEmpty(collectionName))
                    dt = Conn.GetSchema();
                else
                    dt = Conn.GetSchema(collectionName);
            }
            else
                dt = Conn.GetSchema(collectionName, restrictionValues);

            AutoClose();

            return dt;
        }

        /// <summary>
        /// 取得所有表构架
        /// </summary>
        /// <returns></returns>
        public virtual XTable[] GetTables()
        {
            List<XTable> list = null;
            try
            {
                DataTable[] dts = new DataTable[2];
                dts[0] = GetSchema("Tables", new String[] { null, null, null, "TABLE" });
                dts[1] = GetSchema("Tables", new String[] { null, null, null, "VIEW" });
                list = new List<XTable>();
                for (Int32 i = 0; i < dts.Length; i++)
                {
                    if (dts[i] != null && dts[i].Rows != null && dts[i].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dts[i].Rows)
                        {
                            XTable xt = new XTable();
                            xt.ID = list.Count + 1;
                            xt.Name = dr["TABLE_NAME"].ToString();
                            xt.Readme = dr["DESCRIPTION"].ToString();
                            xt.IsView = i > 0;
                            xt.Fields = GetFields(xt);

                            list.Add(xt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Get all the tables frame error!", ex);
            }

            return list == null ? null : list.ToArray();
        }

        /// <summary>
        /// 取得指定表的所有列构架
        /// </summary>
        /// <param name="xt"></param>
        /// <returns></returns>
        protected virtual XField[] GetFields(XTable xt)
        {
            DataTable dt = GetSchema("Columns", new String[] { null, null, xt.Name });

            List<XField> list = new List<XField>();
            DataRow[] drs = dt.Select("", "ORDINAL_POSITION");
            List<String> pks = GetPrimaryKeys(xt);
            List<Dictionary<String, String>> fks = GetForeignKeys(xt);
            foreach (DataRow dr in drs)
            {
                XField xf = new XField();
                xf.ID = Int32.Parse(dr["ORDINAL_POSITION"].ToString());
                xf.Name = dr["COLUMN_NAME"].ToString();
                xf.FieldType = FieldTypeToClassType(dr["DATA_TYPE"].ToString());
                xf.Identity = dr["DATA_TYPE"].ToString() == "3" && (dr["COLUMN_FLAGS"].ToString() == "16" || dr["COLUMN_FLAGS"].ToString() == "90");

                xf.PrimaryKey = pks != null && pks.Contains(xf.Name);
                xf.ForeignKey = false;
                xf.ForeignTableName = "";
                xf.ForeignTablePrimaryName = "";
                if (fks != null)
                {
                    foreach (Dictionary<String, String> dic in fks)
                    {
                        if (dic["FK_COLUMN_NAME"] == xf.Name)
                        {
                            xf.ForeignKey = true;
                            xf.ForeignTableName = dic["PK_TABLE_NAME"];
                            xf.ForeignTablePrimaryName = dic["PK_COLUMN_NAME"];
                            break;
                        }
                    }
                }

                if (xf.FieldType == "Int32" || xf.FieldType == "Double")
                {
                    xf.Length = dr["NUMERIC_PRECISION"] == DBNull.Value ? 0 : Int32.Parse(dr["NUMERIC_PRECISION"].ToString());
                    xf.NumOfByte = 0;
                    xf.Digit = dr["NUMERIC_SCALE"] == DBNull.Value ? 0 : Int32.Parse(dr["NUMERIC_SCALE"].ToString());
                }
                else if (xf.FieldType == "DateTime")
                {
                    xf.Length = dr["DATETIME_PRECISION"] == DBNull.Value ? 0 : Int32.Parse(dr["DATETIME_PRECISION"].ToString());
                    xf.NumOfByte = 0;
                    xf.Digit = 0;
                }
                else
                {
                    if (dr["DATA_TYPE"].ToString() == "130" && dr["COLUMN_FLAGS"].ToString() == "234") //备注类型
                    {
                        xf.Length = Int32.MaxValue;
                        xf.NumOfByte = Int32.MaxValue;
                    }
                    else
                    {
                        xf.Length = dr["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? 0 : Int32.Parse(dr["CHARACTER_MAXIMUM_LENGTH"].ToString());
                        xf.NumOfByte = dr["CHARACTER_OCTET_LENGTH"] == DBNull.Value ? 0 : Int32.Parse(dr["CHARACTER_OCTET_LENGTH"].ToString());
                    }
                    xf.Digit = 0;
                }

                try
                {
                    xf.Nullable = Boolean.Parse(dr["IS_NULLABLE"].ToString());
                }
                catch
                {
                    xf.Nullable = dr["IS_NULLABLE"].ToString() == "YES";
                }
                try
                {
                    xf.Default = dr["COLUMN_HASDEFAULT"].ToString() == "False" ? "" : dr["COLUMN_DEFAULT"].ToString();
                }
                catch
                {
                    xf.Default = dr["COLUMN_DEFAULT"].ToString();
                }
                try
                {
                    xf.Readme = dr["DESCRIPTION"] == DBNull.Value ? "" : dr["DESCRIPTION"].ToString();
                }
                catch
                {
                    xf.Readme = "";
                }
                list.Add(xf);
            }

            return list.ToArray();
        }

        #region 主键构架
        /// <summary>
        /// 取得指定表的所有主键构架
        /// </summary>
        /// <param name="xt"></param>
        /// <returns></returns>
        protected List<String> GetPrimaryKeys(XTable xt)
        {
            if (PrimaryKeys == null) return null;
            try
            {
                DataRow[] drs = PrimaryKeys.Select("TABLE_NAME='" + xt.Name + @"'");
                if (drs == null || drs.Length < 1) return null;
                List<String> list = new List<string>();
                foreach (DataRow dr in drs)
                {
                    list.Add(dr["COLUMN_NAME"] == DBNull.Value ? "" : dr["COLUMN_NAME"].ToString());
                }
                return list;
            }
            catch { return null; }
        }

        protected DataTable _PrimaryKeys;
        /// <summary>
        /// 主键构架
        /// </summary>
        protected virtual DataTable PrimaryKeys
        {
            get
            {
                if (_PrimaryKeys == null) _PrimaryKeys = GetSchema("PrimaryKeys", new String[] { null, null, null });
                return _PrimaryKeys;
            }
        }
        #endregion

        #region 外键构架
        /// <summary>
        /// 取得指定表的所有外键构架
        /// </summary>
        /// <param name="xt"></param>
        /// <returns></returns>
        protected List<Dictionary<String, String>> GetForeignKeys(XTable xt)
        {
            if (ForeignKeys == null) return null;
            try
            {
                DataRow[] drs = ForeignKeys.Select("FK_TABLE_NAME='" + xt.Name + @"'");
                if (drs == null || drs.Length < 1) return null;
                List<Dictionary<String, String>> list = new List<Dictionary<String, String>>();
                foreach (DataRow dr in drs)
                {
                    Dictionary<String, String> dic = new Dictionary<string, string>();
                    dic.Add("FK_COLUMN_NAME", dr["FK_COLUMN_NAME"] == DBNull.Value ? "" : dr["FK_COLUMN_NAME"].ToString());
                    dic.Add("PK_TABLE_NAME", dr["PK_TABLE_NAME"] == DBNull.Value ? "" : dr["PK_TABLE_NAME"].ToString());
                    dic.Add("PK_COLUMN_NAME", dr["PK_COLUMN_NAME"] == DBNull.Value ? "" : dr["PK_COLUMN_NAME"].ToString());
                    list.Add(dic);
                }
                return list;
            }
            catch { return null; }
        }

        protected DataTable _ForeignKeys;
        /// <summary>
        /// 外键构架
        /// </summary>
        protected virtual DataTable ForeignKeys
        {
            get
            {
                if (_ForeignKeys == null) _ForeignKeys = GetSchema("ForeignKeys", new String[] { null, null, null });
                return _ForeignKeys;
            }
        }
        #endregion

        #region 字段类型到数据类型对照表
        /// <summary>
        /// 字段类型到数据类型对照表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual String FieldTypeToClassType(String type)
        {
            Int32 t = Int32.Parse(type);
            switch (t)
            {
                case 16:// adTinyInt
                case 2:// adSmallInt
                case 17:// adUnsignedTinyInt
                case 18:// adUnsignedSmallInt
                    return "Int16";
                case 3:// adInteger
                case 19:// adUnsignedInt
                case 14:// adDecimal
                case 131:// adNumeric
                    return "Int32";
                case 20:// adBigInt
                case 21:// adUnsignedBigInt
                    return "Int64";
                case 4:// adSingle
                case 5:// adDouble
                case 6:// adCurrency
                    return "Double";
                case 11:// adBoolean
                    return "Boolean";
                case 7:// adDate
                case 133:// adDBDate
                case 134:// adDBTime
                case 135:// adDBTimeStamp
                    return "DateTime";
                case 8:// adBSTR
                case 129:// adChar
                case 200:// adVarChar
                case 201:// adLongVarChar
                case 130:// adWChar
                case 202:// adVarWChar
                case 203:// adLongVarWChar
                    return "String";
                case 128:// adBinary
                case 204:// adVarBinary
                case 205:// adLongVarBinary 
                case 0:// adEmpty
                case 10:// adError
                case 132:// adUserDefined
                case 12:// adVariant
                case 9:// adIDispatch
                case 13:// adIUnknown
                case 72:// adGUID
                default:
                    return "Object";
            }
        }
        #endregion
        #endregion

        #region Sql日志输出
        private Boolean _SqlLog = false;
        /// <summary>
        /// Sql日志输出
        /// </summary>
        public Boolean SqlLog
        {
            get
            {
                //return _SqlLog;
                if (_SqlLog || Debug)
                    return true;
                else
                    return false;
            }
            set
            {
                _SqlLog = value;
            }
        }

        private static Boolean? _Debug;
        /// <summary>
        /// 是否调试
        /// </summary>
        public static Boolean Debug
        {
            get
            {
                if (_Debug != null) return _Debug.Value;

                String str = AppConfig.OrmDebug;
                if (String.IsNullOrEmpty(str)) return false;
                if (str == "1" || str.Equals(Boolean.TrueString, StringComparison.OrdinalIgnoreCase)) return true;
                if (str == "0" || str.Equals(Boolean.FalseString, StringComparison.OrdinalIgnoreCase)) return false;
                _Debug = Convert.ToBoolean(str);
                return _Debug.Value;
            }
            set { _Debug = value; }
        }

        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLog(String msg)
        {
            XTrace.WriteLine(msg);
        }

        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WriteLog(String format, params Object[] args)
        {
            WriteLog(String.Format(format, args));
        }
        #endregion
    }
}