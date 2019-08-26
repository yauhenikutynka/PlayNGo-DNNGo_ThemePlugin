using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;


namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 数据访问层。
    /// <remarks>
    /// 主要用于选择不同的数据库，不同的数据库的CRUD有所差别。
    /// 每一个数据库链接字符串，对应唯一的一个DAL实例。
    /// 数据库链接字符串可以写在配置文件中，然后在Create时指定名字；
    /// 也可以直接把链接字符串作为Create的参数传入。
    /// 在Web中，每一个DAL实例，会为每一个请求初始化一个DataBase实例；
    /// 在WinForm中，每一个DAL实例，会为每一个线程初始化一个DataBase实例。
    /// 每一个数据库操作都必须指定表名，空表名或*将匹配所有缓存
    /// </remarks>
    /// </summary>
    public class DAL
    {
        #region 创建函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connName">配置名</param>
        private DAL(String connName)
        {
            _ConnName = connName;
            // 如果无法取到链接字符串，则把connName当作连接字符串
            if (!GetConnStr()) ConnStr = connName;
        }

        private static Dictionary<String, DAL> _dals = new Dictionary<String, DAL>();
        /// <summary>
        /// 创建一个数据访问层对象。以null作为参数可获得当前默认对象
        /// </summary>
        /// <param name="connName">配置名，或链接字符串</param>
        /// <returns>对应于指定链接的全局唯一的数据访问层对象</returns>
        public static DAL Create(String connName)
        {
            //当connName为null时，_dals里面并没有包含null的项，所以需要提前处理
            if (String.IsNullOrEmpty(connName) && _dals.Count > 0)
            {
                foreach (DAL dd in _dals.Values)
                {
                    return dd;
                }
            }

            if (_dals.ContainsKey(connName)) return _dals[connName];
            DAL d;
            lock (_dals)
            {
                if (_dals.ContainsKey(connName)) return _dals[connName];
                d = new DAL(connName);
                // 不用connName，因为可能在创建过程中自动识别了ConnName
                _dals.Add(d.ConnName, d);
            }

            return d;
        }

        /// <summary>
        /// 取得连接字符串
        /// </summary>
        private Boolean GetConnStr()
        {
            ConnectionStringSettingsCollection cssc = System.Configuration.ConfigurationManager.ConnectionStrings;
            // 链接配置段为空，退出，需要从参数中拿链接字符串
            if (cssc == null || cssc.Count < 1) return false;

            // 没有设置连接名，则查找默认连接名。
            if (String.IsNullOrEmpty(_ConnName))
            {
                if (cssc["SiteSqlServer"] != null) // 如果有default的设置，则使用default
                    _ConnName = "SiteSqlServer";
                else // 否则，使用第一个不是aspnet数据库的连接
                {
                    for (Int32 i = 0; i < cssc.Count; i++)
                    {
                        if (!cssc[i].ConnectionString.ToLower().Contains("aspnetdb.mdf"))
                        {
                            _ConnName = cssc[i].Name;
                            break;
                        }
                    }
                    if (String.IsNullOrEmpty(_ConnName)) return false;
                }
            }
            else if (cssc[_ConnName] == null) //如果根据连接名无法取得值，则该连接名可能是连接字符串，退出
                return false;
            _ConnStr = cssc[ConnName].ConnectionString;
            // 分析类型
            if (String.IsNullOrEmpty(cssc[ConnName].ProviderName)) // 没有指定驱动名，则需要从连接字符串中分析。
                GetDbType();
            else
            {
                String ass = cssc[ConnName].ProviderName;
                if (ass.Contains("SqlClient"))
                    DALType = typeof(SqlServer);
                else if (ass.ToLower().Contains("microsoft.jet.oledb"))
                    DALType = typeof(Access);
                else if (ass.ToLower().Contains("mysql"))
                    DALType = typeof(MySql);
                else if (ass.ToLower().Contains("sqlite"))
                    DALType = typeof(SQLite);
                else
                {
                    if (ass.Contains(",")) // 带有程序集名称，加载程序集
                        DALType = Assembly.Load(ass.Substring(0, ass.IndexOf(","))).GetType(ass.Substring(ass.IndexOf(",") + 1, ass.Length), true, false);
                    else // 没有程序集名称，则使用本程序集
                        DALType = this.GetType().Assembly.GetType(ass, true, false);
                }
            }
            return true;
        }

        /// <summary>
        /// 取得数据库类型
        /// </summary>
        private void GetDbType()
        {
            if (String.IsNullOrEmpty(_ConnStr)) return;
            // 分析类型
            String str = _ConnStr.ToLower();
            if (str.Contains("mssql") || str.Contains("sqloledb"))
                DALType = typeof(SqlServer);
            else if (str.Contains("microsoft.jet.oledb"))
                DALType = typeof(Access);
            else
                DALType = typeof(SqlServer);
        }
        #endregion

        #region 静态属性
        //private static Object _Default_ref = new Object();
        private static DAL _Default;
        /// <summary>
        /// 当前数据访问对象
        /// </summary>
        public static DAL Default
        {
            get
            {
                if (_Default == null) _Default = Create(null);
                //if (_Default != null) return _Default;
                //if (_dals.Count > 0) return _dals[0];
                //lock (_dals)
                //{
                //    if (_Default != null) return _Default;
                //    if (_dals.Count > 0) return _dals[0];
                //    _Default = new DAL(null);
                //    if (!_dals.ContainsKey(_Default.ConnName)) _dals.Add(_Default.ConnName, _Default);
                //}
                return _Default;
            }
            // 允许给Current赋值。在非Web程序中，是没有默认连接字符串的，所以必须手工配置默认访问对象
            set { _Default = value; }
        }
        #endregion

        #region 属性
        private String _ConnName;
        /// <summary>
        /// 配置名。只读，若要设置，请重新声明一个DAL对象。
        /// </summary>
        public String ConnName
        {
            get { return _ConnName; }
        }

        private Type _DALType;
        /// <summary>
        /// 数据访问层基层类型。
        /// <remarks>改变数据访问层数据库实体会断开当前链接，建议在任何数据库操作之前改变</remarks>
        /// </summary>
        public Type DALType
        {
            get { return _DALType; }
            set	// 如果外部需要改变数据访问层数据库实体
            {
                IDataBase idb;
                if (HttpContext.Current == null)
                    idb = _DBs != null && _DBs.ContainsKey(ConnName) ? _DBs[ConnName] : null;
                else
                    idb = HttpContext.Current.Items[ConnName + "_DB"] as IDataBase;
                if (idb != null)
                {
                    idb.Dispose();
                    idb = null;
                }
                _DALType = value;
            }
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType DbType
        {
            get { return DB.DbType; }
        }

        private String _ConnStr;
        /// <summary>
        /// 默认连接字符串，第一个ConnectionString就是
        /// </summary>
        public String ConnStr
        {
            get { return _ConnStr; }
            // 允许给ConnStr赋值。在某些特殊情况下必须手工配置，但不允许外部赋值，外部必须通过Create调用
            private set
            {
                _ConnStr = value;
                GetDbType();
            }
        }

        /// <summary>
        /// ThreadStatic 指示静态字段的值对于每个线程都是唯一的。
        /// </summary>
        [ThreadStatic]
        private static IDictionary<String, IDataBase> _DBs;
        /// <summary>
        /// DAL对象。
        /// <remarks>
        /// 这里使用线程级缓存或请求级缓存，保证所有数据库操作线程安全。
        /// 使用外部数据库驱动会使得性能稍有下降。
        /// </remarks>
        /// </summary>
        private IDataBase DB
        {
            get
            {
                if (String.IsNullOrEmpty(ConnStr)) throw new Exception("Please set the connection string in the database before using");

                //if (HttpContext.Current == null) // 非Web程序，使用线程级缓存
                return CreateForNotWeb();
                //else
                //    return CreateForWeb();
            }
        }

        private static Dictionary<String, Boolean> IsSql2005 = new Dictionary<String, Boolean>();

        private IDataBase CreateForNotWeb()
        {
            if (_DBs == null) _DBs = new Dictionary<String, IDataBase>();
            if (_DBs.ContainsKey(ConnName)) return _DBs[ConnName];
            lock (_DBs)
            {
                if (_DBs.ContainsKey(ConnName)) return _DBs[ConnName];

                // 创建对象，先取得程序集，再创建实例，是为了防止在本程序集创建外部DAL类的实例而出错

                IDataBase _DB;
                if (DALType == typeof(Access))
                    _DB = new Access(ConnStr);
                else if (DALType == typeof(SqlServer))
                    _DB = new SqlServer(ConnStr);
                else if (DALType == typeof(SqlServer2005))
                    _DB = new SqlServer2005(ConnStr);
                else if (DALType == typeof(MySql))
                    _DB = new MySql(ConnStr);
                else if (DALType == typeof(SQLite))
                    _DB = new SQLite(ConnStr);
                else
                    _DB = DALType.Assembly.CreateInstance(DALType.FullName, false, BindingFlags.Default, null, new Object[] { ConnStr }, null, null) as IDataBase;
                //检查是否SqlServer2005
                //_DB = CheckSql2005(_DB);

                if (!IsSql2005.ContainsKey(ConnName))
                {
                    lock (IsSql2005)
                    {
                        if (!IsSql2005.ContainsKey(ConnName))
                        {
                            IsSql2005.Add(ConnName, CheckSql2005(_DB));
                        }
                    }
                }

                if (IsSql2005.ContainsKey(ConnName) && IsSql2005[ConnName])
                {
                    _DALType = typeof(SqlServer2005);
                    _DB.Dispose();
                    _DB = new SqlServer2005(ConnStr);
                }

                _DB.SqlLog = EnableSqlLog;

                _DBs.Add(ConnName, _DB);

                if (DataBase.Debug) DataBase.WriteLog("创建DB（NotWeb）：{0}", _DB.ID);

                return _DB;
            }
        }

        private IDataBase CreateForWeb()
        {
            String key = ConnName + "_DB";
            IDataBase d;

            if (HttpContext.Current.Items[key] != null && HttpContext.Current.Items[key] is IDataBase)
                d = HttpContext.Current.Items[key] as IDataBase;
            else
            {
                if (DALType == typeof(Access))
                    d = new Access(ConnStr);
                else if (DALType == typeof(SqlServer))
                    d = new SqlServer(ConnStr);
                else if (DALType == typeof(MySql))
                    d = new MySql(ConnStr);
                else if (DALType == typeof(SQLite))
                    d = new SQLite(ConnStr);
                else
                    d = DALType.Assembly.CreateInstance(DALType.FullName, false, BindingFlags.Default, null, new Object[] { ConnStr }, null, null) as IDataBase;

                if (DataBase.Debug) DataBase.WriteLog("创建DB（Web）：{0}", d.ID);

                HttpContext.Current.Items.Add(key, d);
            }
            //检查是否SqlServer2005
            //_DB = CheckSql2005(_DB);

            if (!IsSql2005.ContainsKey(ConnName))
            {
                lock (IsSql2005)
                {
                    if (!IsSql2005.ContainsKey(ConnName))
                    {
                        IsSql2005.Add(ConnName, CheckSql2005(d));
                    }
                }
            }

            if (IsSql2005.ContainsKey(ConnName) && IsSql2005[ConnName])
            {
                _DALType = typeof(SqlServer2005);
                d.Dispose();
                d = new SqlServer2005(ConnStr);
            }

            d.SqlLog = EnableSqlLog;
            return d;
        }

        //private IDataBase CheckSql2005(IDataBase db)
        //{
        //    //检查是否SqlServer2005
        //    if (db.DbType != DatabaseType.SqlServer) return db;

        //    //取数据库版本
        //    DataSet ds = db.Query("Select @@Version");
        //    if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
        //    {
        //        String ver = ds.Tables[0].Rows[0][0].ToString();
        //        if (!String.IsNullOrEmpty(ver) && ver.StartsWith("Microsoft SQL Server 2005"))
        //        {
        //            _DALType = typeof(SqlServer2005);
        //            db.Dispose();
        //            db = new SqlServer2005(ConnStr);
        //        }
        //    }
        //    return db;
        //}

        private Boolean CheckSql2005(IDataBase db)
        {
            //检查是否SqlServer2005
            if (db.DbType != DatabaseType.SqlServer) return false;

            //取数据库版本
            DataSet ds = db.Query("Select @@Version");
            if (ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
            {
                String ver = ds.Tables[0].Rows[0][0].ToString();
                if (!String.IsNullOrEmpty(ver) && ver.StartsWith("Microsoft SQL Server 2005"))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否存在DB实例。
        /// 如果直接使用DB属性判断是否存在，它将会创建一个实例。
        /// </summary>
        private Boolean ExistDB
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Items == null)
                {
                    if (_DBs != null && !_DBs.ContainsKey(ConnName)) return true;
                    return false;
                }
                else
                {
                    String key = ConnName + "_DB";
                    if (HttpContext.Current.Items[key] != null && HttpContext.Current.Items[key] is IDataBase) return true;
                    return false;
                }
            }
        }
        #endregion

        #region 使用缓存后的数据操作方法
        #region 属性
        private Boolean _EnableCache = true;
        /// <summary>
        /// 是否启用缓存。
        /// <remarks>设为false可清空缓存</remarks>
        /// </summary>
        public Boolean EnableCache
        {
            get { return _EnableCache; }
            set
            {
                _EnableCache = value;
                if (!_EnableCache) XCache.RemoveAll();
            }
        }

        /// <summary>
        /// 缓存个数
        /// </summary>
        public Int32 CacheCount
        {
            get
            {
                return XCache.Count;
            }
        }

        /// <summary>
        /// 查询次数
        /// </summary>
        public Int32 QueryTimes
        {
            get { return DB != null ? DB.QueryTimes : 0; }
        }

        /// <summary>
        /// 执行次数
        /// </summary>
        public Int32 ExecuteTimes
        {
            get { return DB != null ? DB.ExecuteTimes : 0; }
        }

        private Boolean _EnableSqlLog;
        /// <summary>
        /// 是否启用Sql日志输出。
        /// </summary>
        public Boolean EnableSqlLog
        {
            get { return _EnableSqlLog; }
            set { _EnableSqlLog = value; }
        }
        #endregion

        private static Dictionary<String, String> _PageSplitCache = new Dictionary<String, String>();
        /// <summary>
        /// 根据条件把普通查询SQL格式化为分页SQL。
        /// </summary>
        /// <remarks>
        /// 因为需要继承重写的原因，在数据类中并不方便缓存分页SQL。
        /// 所以在这里做缓存。
        /// </remarks>
        /// <param name="sql">SQL语句</param>
        /// <param name="startRowIndex">开始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <param name="keyColumn">唯一键。用于not in分页</param>
        /// <returns>分页SQL</returns>
        public String PageSplit(String sql, Int32 startRowIndex, Int32 maximumRows, String keyColumn)
        {
            String cacheKey = String.Format("{0}_{1}_{2}_{3}_", sql, startRowIndex, maximumRows, ConnName);
            if (!String.IsNullOrEmpty(keyColumn)) cacheKey += keyColumn;
            if (_PageSplitCache.ContainsKey(cacheKey)) return _PageSplitCache[cacheKey];
            lock (_PageSplitCache)
            {
                if (_PageSplitCache.ContainsKey(cacheKey)) return _PageSplitCache[cacheKey];
                String s = DB.PageSplit(sql, startRowIndex, maximumRows, keyColumn);
                _PageSplitCache.Add(cacheKey, s);
                return s;
            }
        }

        /// <summary>
        /// 执行SQL查询，返回记录集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableNames">所依赖的表的表名</param>
        /// <returns></returns>
        public DataSet Select(String sql, String[] tableNames)
        {
            String cacheKey = sql + "_" + ConnName;
            if (EnableCache && XCache.Contain(cacheKey)) return XCache.Item(cacheKey);
            DataSet ds = DB.Query(sql);
            if (EnableCache) XCache.Add(cacheKey, ds, tableNames);
            return ds;
        }

        /// <summary>
        /// 执行SQL查询，返回记录集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">所依赖的表的表名</param>
        /// <returns></returns>
        public DataSet Select(String sql, String tableName)
        {
            return Select(sql, new String[] { tableName });
        }

        /// <summary>
        /// 执行SQL查询，返回分页记录集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="startRowIndex">开始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <param name="keyColumn">唯一键。用于not in分页</param>
        /// <param name="tableNames">所依赖的表的表名</param>
        /// <returns></returns>
        public DataSet Select(String sql, Int32 startRowIndex, Int32 maximumRows, String keyColumn, String[] tableNames)
        {
            String cacheKey = sql + "_" + startRowIndex + "_" + maximumRows + "_" + ConnName;
            if (EnableCache && XCache.Contain(cacheKey)) return XCache.Item(cacheKey);
            DataSet ds = DB.Query(PageSplit(sql, startRowIndex, maximumRows, keyColumn));
            if (EnableCache) XCache.Add(cacheKey, ds, tableNames);
            return ds;
        }

        /// <summary>
        /// 执行SQL查询，返回分页记录集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="startRowIndex">开始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <param name="keyColumn">唯一键。用于not in分页</param>
        /// <param name="tableName">所依赖的表的表名</param>
        /// <returns></returns>
        public DataSet Select(String sql, Int32 startRowIndex, Int32 maximumRows, String keyColumn, String tableName)
        {
            return Select(sql, startRowIndex, maximumRows, keyColumn, new String[] { tableName });
        }

        /// <summary>
        /// 执行SQL查询，返回总记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableNames">所依赖的表的表名</param>
        /// <returns></returns>
        public Int32 SelectCount(String sql, String[] tableNames)
        {
            String cacheKey = sql + "_SelectCount" + "_" + ConnName;
            if (EnableCache && XCache.IntContain(cacheKey)) return XCache.IntItem(cacheKey);
            Int32 rs = DB.QueryCount(sql);
            if (EnableCache) XCache.Add(cacheKey, rs, tableNames);
            return rs;
        }

        /// <summary>
        /// 执行SQL查询，返回总记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">所依赖的表的表名</param>
        /// <returns></returns>
        public Int32 SelectCount(String sql, String tableName)
        {
            return SelectCount(sql, new String[] { tableName });
        }

        /// <summary>
        /// 执行SQL查询，返回总记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="startRowIndex">开始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <param name="keyColumn">唯一键。用于not in分页</param>
        /// <param name="tableNames">所依赖的表的表名</param>
        /// <returns></returns>
        public Int32 SelectCount(String sql, Int32 startRowIndex, Int32 maximumRows, String keyColumn, String[] tableNames)
        {
            return SelectCount(PageSplit(sql, startRowIndex, maximumRows, keyColumn), tableNames);
        }

        /// <summary>
        /// 执行SQL查询，返回总记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="startRowIndex">开始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <param name="keyColumn">唯一键。用于not in分页</param>
        /// <param name="tableName">所依赖的表的表名</param>
        /// <returns></returns>
        public Int32 SelectCount(String sql, Int32 startRowIndex, Int32 maximumRows, String keyColumn, String tableName)
        {
            return SelectCount(sql, startRowIndex, maximumRows, keyColumn, new String[] { tableName });
        }

        /// <summary>
        /// 执行SQL语句，返回受影响的行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableNames">受影响的表的表名</param>
        /// <returns></returns>
        public Int32 Execute(String sql, String[] tableNames)
        {
            // 移除所有和受影响表有关的缓存
            if (EnableCache) XCache.Remove(tableNames);
            return DB.Execute(sql);
        }

        /// <summary>
        /// 执行SQL语句，返回受影响的行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">受影响的表的表名</param>
        /// <returns></returns>
        public Int32 Execute(String sql, String tableName)
        {
            return Execute(sql, new String[] { tableName });
        }

        /// <summary>
        /// 执行SQL语句，返回受影响的行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableNames">受影响的表的表名</param>
        /// <returns></returns>
        public Object ExecuteScalar(String sql, String[] tableNames)
        {
            // 移除所有和受影响表有关的缓存
            if (EnableCache) XCache.Remove(tableNames);
            return DB.ExecuteScalar(sql);
        }

        /// <summary>
        /// 执行SQL语句，返回受影响的行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">受影响的表的表名</param>
        /// <returns></returns>
        public Object ExecuteScalar(String sql, String tableName)
        {
            return ExecuteScalar(sql, new String[] { tableName });
        }

        /// <summary>
        /// 执行插入语句并返回新增行的自动编号
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="tableNames">受影响的表的表名</param>
        /// <returns>新增行的自动编号</returns>
        public Int32 InsertAndGetIdentity(String sql, String[] tableNames)
        {
            // 移除所有和受影响表有关的缓存
            if (EnableCache) XCache.Remove(tableNames);
            return DB.InsertAndGetIdentity(sql);
        }

        /// <summary>
        /// 执行插入语句并返回新增行的自动编号
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="tableName">受影响的表的表名</param>
        /// <returns>新增行的自动编号</returns>
        public Int32 InsertAndGetIdentity(String sql, String tableName)
        {
            return InsertAndGetIdentity(sql, new String[] { tableName });
        }

        /// <summary>
        /// 执行CMD，返回记录集
        /// </summary>
        /// <param name="cmd">CMD</param>
        /// <param name="tableNames">所依赖的表的表名</param>
        /// <returns></returns>
        public DataSet Select(DbCommand cmd, String[] tableNames)
        {
            String cacheKey = cmd.CommandText + "_" + ConnName;
            if (EnableCache && XCache.Contain(cacheKey)) return XCache.Item(cacheKey);
            DataSet ds = DB.Query(cmd);
            if (EnableCache) XCache.Add(cacheKey, ds, tableNames);
            DB.AutoClose();
            return ds;
        }

        /// <summary>
        /// 执行CMD，返回受影响的行数
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public Int32 Execute(DbCommand cmd, String[] tableNames)
        {
            // 移除所有和受影响表有关的缓存
            if (EnableCache) XCache.Remove(tableNames);
            Int32 ret = DB.Execute(cmd);
            DB.AutoClose();
            return ret;
        }

        ///// <summary>
        ///// 获取一个DbCommand。
        ///// 配置了连接，并关联了事务。
        ///// 连接已打开。
        ///// 使用完毕后，必须调用AutoClose方法，以使得在非事务及设置了自动关闭的情况下关闭连接。
        ///// 除非迫不得已，否则，请不要使用该方法，可以考虑用Select(cmd)和Execute(cmd)来代替。
        ///// 非法使用会使得资源失去控制。极度危险！
        ///// </summary>
        ///// <returns></returns>
        //private DbCommand PrepareCommand()
        //{
        //    return DB.PrepareCommand();
        //}

        /// <summary>
        /// 取得所有表和视图的构架信息
        /// </summary>
        /// <returns></returns>
        public IList<XTable> Tables
        {
            get
            {
                return new List<XTable>(DB.GetTables());
            }
        }
        #endregion

        #region 事务
        /// <summary>
        /// 开始事务。
        /// 事务一旦开始，请务必在操作完成后提交或者失败时回滚，否则可能会造成资源失去控制。极度危险！
        /// </summary>
        /// <returns></returns>
        public DbTransaction BeginTransaction()
        {
            return DB.BeginTransaction();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            DB.Commit();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            DB.Rollback();
        }
        #endregion

        #region 导入导出
        /// <summary>
        /// 导出架构信息
        /// </summary>
        /// <returns></returns>
        public String Export()
        {
            if (Tables == null || Tables.Count < 1) return null;

            XmlSerializer serializer = new XmlSerializer(typeof(XTable[]));
            using (StringWriter sw = new StringWriter())
            {
                serializer.Serialize(sw, Tables);
                return sw.ToString();
            }
        }

        /// <summary>
        /// 导入架构信息
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static XTable[] Import(String xml)
        {
            if (String.IsNullOrEmpty(xml)) return null;

            XmlSerializer serializer = new XmlSerializer(typeof(XTable[]));
            using (StringReader sr = new StringReader(xml))
            {
                return serializer.Deserialize(sr) as XTable[];
            }
        }
        #endregion
    }
}