using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
 

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// Access数据库
    /// </summary>
    internal class Access : DataBase
    {
        #region 构造
        /// <summary>
        /// 构造函数。Access数据库需要处理连接字符串中的相对路径，得自己创建Conn对象
        /// </summary>
        /// <param name="connStr"></param>
        public Access(String connStr)
            : base("", OleDbFactory.Instance)
        {
            if (String.IsNullOrEmpty(connStr) || connStr.Length < 10) throw new Exception("Are you sure you set the connection string correct it?");

            try
            {
                OleDbConnectionStringBuilder csb = new OleDbConnectionStringBuilder(connStr);
                // 不是绝对路径
                if (!String.IsNullOrEmpty(csb.DataSource) && csb.DataSource.Length > 1 && csb.DataSource.Substring(1, 1) != ":")
                {
                    String mdbPath = csb.DataSource;
                    if (mdbPath.StartsWith("~/") || mdbPath.StartsWith("~\\"))
                    {
                        mdbPath = mdbPath.Replace("/", "\\").Replace("~\\", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\");
                    }
                    else if (mdbPath.StartsWith("./") || mdbPath.StartsWith(".\\"))
                    {
                        mdbPath = mdbPath.Replace("/", "\\").Replace(".\\", AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\");
                    }
                    else
                    {
                        mdbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, mdbPath.Replace("/", "\\"));
                    }
                    csb.DataSource = mdbPath;
                    connStr = csb.ConnectionString;
                }
                //// 使用连接池后，不需要创建Conn
                //Conn = null;
                //ConnectionString = connStr;
                Conn = new OleDbConnection(connStr);
            }
            catch (Exception ex)
            {
                throw new Exception("Error analysis OLEDB connection string", ex);
            }
        }

        ///// <summary>
        ///// 连接字符串
        ///// </summary>
        //private String ConnectionString;

        /// <summary>
        /// 构造函数。使用外部连接
        /// </summary>
        /// <param name="conn">连接</param>
        public Access(OleDbConnection conn)
            : base(conn, OleDbFactory.Instance)
        {
            //ConnectionString = conn.ConnectionString;
        }
        #endregion

        #region 重载使用连接池
        /// <summary>
        /// 返回数据库类型。外部DAL数据库类请使用Other
        /// </summary>
        public override DatabaseType DbType
        {
            get { return DatabaseType.Access; }
        }

        ///// <summary>
        ///// 打开。已重写，为了使用连接池
        ///// </summary>
        //public override void Open()
        //{
        //    Conn = AccessPool.Open(ConnectionString);
        //}

        ///// <summary>
        ///// 关闭。已重写，为了使用连接池
        ///// </summary>
        //public override void Close()
        //{
        //    AccessPool.Close(ConnectionString, Conn as OleDbConnection);
        //    Conn = null;
        //}

        ///// <summary>
        ///// 析构时，返还连接到连接池
        ///// </summary>
        //~Access()
        //{
        //    if (Conn != null) Close();
        //}
        #endregion

        #region 基本方法 查询/执行
        /// <summary>
        /// 执行插入语句并返回新增行的自动编号
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>新增行的自动编号</returns>
        public override Int32 InsertAndGetIdentity(String sql)
        {
            ExecuteTimes++;
            if (SqlLog) XTrace.WriteLine("Sql输出：" + sql);
            try
            {
                DbCommand cmd = PrepareCommand();
                cmd.CommandText = sql;
                Int32 rs = cmd.ExecuteNonQuery();
                if (rs > 0)
                {
                    cmd.CommandText = "Select @@Identity";
                    rs = Int32.Parse(cmd.ExecuteScalar().ToString());
                }
                AutoClose();
                return rs;
            }
            catch (Exception ex)
            {
                throw OnException(ex, sql);
            }
        }
        #endregion

        #region 构架
        /// <summary>
        /// 已重载。主键构架
        /// </summary>
        protected override DataTable PrimaryKeys
        {
            get
            {
                if (_PrimaryKeys != null) return _PrimaryKeys;

                if (!Opened) Open();

                _PrimaryKeys = (Conn as OleDbConnection).GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, new String[] { null, null, null });

                AutoClose();

                return _PrimaryKeys;
            }
        }

        /// <summary>
        /// 已重载。外键构架
        /// </summary>
        protected override DataTable ForeignKeys
        {
            get
            {
                if (_ForeignKeys != null) return _ForeignKeys;

                if (!Opened) Open();

                _ForeignKeys = (Conn as OleDbConnection).GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, new String[] { null, null, null });

                AutoClose();

                return _ForeignKeys;
            }
        }
        #endregion
    }

    #region OleDb连接池
    ///// <summary>
    ///// Access数据库连接池。
    ///// 每个连接字符串一个连接池。
    ///// 一定时间后，关闭未使用的多余连接；
    ///// </summary>
    //internal class AccessPool : IDisposable
    //{
    //    #region 连接池的创建与销毁
    //    /// <summary>
    //    /// 连接字符串
    //    /// </summary>
    //    private String ConnectionString;
    //    /// <summary>
    //    /// 私有构造函数，禁止外部创建实例。
    //    /// </summary>
    //    /// <param name="connStr">连接字符串</param>
    //    private AccessPool(String connStr)
    //    {
    //        ConnectionString = connStr;
    //    }

    //    private Boolean Disposed = false;
    //    /// <summary>
    //    /// 释放所有连接
    //    /// </summary>
    //    public void Dispose()
    //    {
    //        if (Disposed) return;
    //        lock (this)
    //        {
    //            if (Disposed) return;
    //            foreach (OleDbConnection conn in FreeList)
    //            {
    //                try
    //                {
    //                    if (conn != null && conn.State != ConnectionState.Closed) conn.Close();
    //                }
    //                catch (Exception ex)
    //                {
    //                    Trace.WriteLine("在AccessPool连接池中释放所有连接时出错！" + ex.ToString());
    //                }
    //            }
    //            FreeList.Clear();
    //            foreach (OleDbConnection conn in UsedList)
    //            {
    //                try
    //                {
    //                    if (conn != null && conn.State != ConnectionState.Closed) conn.Close();
    //                }
    //                catch (Exception ex)
    //                {
    //                    Trace.WriteLine("在AccessPool连接池中释放所有连接时出错！" + ex.ToString());
    //                }
    //            }
    //            UsedList.Clear();
    //            //双锁
    //            if (Pools.ContainsKey(ConnectionString))
    //            {
    //                lock (Pools)
    //                {
    //                    if (Pools.ContainsKey(ConnectionString)) Pools.Remove(ConnectionString);
    //                }
    //            }
    //            Disposed = true;
    //        }
    //    }

    //    ~AccessPool()
    //    {
    //        // 析构调用每个连接池对象的Dispose，Dispose后又可能引发析构
    //        Dispose();
    //    }
    //    #endregion

    //    #region 借/还 连接
    //    /// <summary>
    //    /// 空闲列表
    //    /// </summary>
    //    private List<OleDbConnection> FreeList = new List<OleDbConnection>();
    //    /// <summary>
    //    /// 使用列表
    //    /// </summary>
    //    private List<OleDbConnection> UsedList = new List<OleDbConnection>();
    //    /// <summary>
    //    /// 最大池大小
    //    /// </summary>
    //    public Int32 MaxPoolSize = 100;
    //    /// <summary>
    //    /// 最小池大小
    //    /// </summary>
    //    public Int32 MinPoolSize = 0;

    //    /// <summary>
    //    /// 取连接
    //    /// </summary>
    //    /// <returns></returns>
    //    private OleDbConnection Open()
    //    {
    //        // 多线程冲突锁定，以下代码在同一时刻只能有一个线程进入
    //        lock (this)
    //        {
    //            if (UsedList.Count >= MaxPoolSize) throw new XException("连接池的连接数超过最大限制，无法提供服务");
    //            OleDbConnection conn;
    //            // 看看是否还有连接，如果没有，需要马上创建
    //            if (FreeList.Count < 1)
    //            {
    //                Trace.WriteLine("新建连接");
    //                conn = new OleDbConnection(ConnectionString);
    //                conn.Open();
    //                // 直接进入使用列表
    //                UsedList.Add(conn);
    //                return conn;
    //            }
    //            // 从空闲列表中取第一个连接
    //            conn = FreeList[0];
    //            // 第一个连接离开空闲列表
    //            FreeList.RemoveAt(0);
    //            // 该连接进入使用列表
    //            UsedList.Add(conn);
    //            // 检查连接是否已经打开，如果没打开，则打开
    //            if (conn.State == ConnectionState.Closed) conn.Open();
    //            return conn;
    //        }
    //    }

    //    /// <summary>
    //    /// 返还连接
    //    /// </summary>
    //    /// <param name="conn">连接对象</param>
    //    private void Close(OleDbConnection conn)
    //    {
    //        if (conn == null || UsedList == null || UsedList.Count < 1) return;
    //        lock (this)
    //        {
    //            if (UsedList == null || UsedList.Count < 1) return;
    //            // 下面的检查，原来放在lock外面，在高并发的环境下报了那个不可能的异常，谨记以后一定要Double Lock
    //            // Double Lock也就是：检查->锁定->再检查->执行
    //            // 检查该连接对象是否来自本连接池。该信息应该在设计时期就显示，以帮助开发者快速修正错误
    //            if (!UsedList.Contains(conn)) throw new XException("返还给AccessPool连接池的连接，不是来自本连接池！");
    //            // 离开使用列表
    //            UsedList.Remove(conn);
    //            // 回到空闲列表
    //            FreeList.Add(conn);
    //        }
    //    }
    //    #endregion

    //    #region 检查连接
    //    /// <summary>
    //    /// 检查连接池。关闭未使用连接，防止打开过多连接而又不关闭
    //    /// </summary>
    //    /// <returns>是否关闭了连接，调用者将以此为依据来决定是否停用定时器</returns>
    //    private Boolean Check()
    //    {
    //        if (FreeList.Count < 1 || FreeList.Count + UsedList.Count <= MinPoolSize) return false;
    //        lock (this)
    //        {
    //            if (FreeList.Count < 1 || FreeList.Count + UsedList.Count <= MinPoolSize) return false;
    //            Trace.WriteLine("删除连接");
    //            try
    //            {
    //                // 关闭所有空闲连接，仅保留最小池大小
    //                while (FreeList.Count > 0 && FreeList.Count + UsedList.Count > MinPoolSize)
    //                {
    //                    OleDbConnection conn = FreeList[0];
    //                    FreeList.RemoveAt(0);
    //                    conn.Close();
    //                    conn.Dispose();
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                Trace.WriteLine("检查AccessPool连接池时出错！" + ex.ToString());
    //            }
    //            return true;
    //        }
    //    }
    //    #endregion

    //    #region 从连接池中 借/还 连接
    //    /// <summary>
    //    /// 连接池集合。连接字符串作为索引，每个连接字符串对应一个连接池。
    //    /// </summary>
    //    private static Dictionary<String, AccessPool> Pools = new Dictionary<string, AccessPool>();

    //    /// <summary>
    //    /// 获得连接
    //    /// </summary>
    //    /// <param name="connStr">连接字符串</param>
    //    /// <returns></returns>
    //    public static OleDbConnection Open(String connStr)
    //    {
    //        if (String.IsNullOrEmpty(connStr)) return null;
    //        // 检查是否存在连接字符串为connStr的连接池
    //        if (!Pools.ContainsKey(connStr))
    //        {
    //            lock (Pools)
    //            {
    //                if (!Pools.ContainsKey(connStr))
    //                {
    //                    Pools.Add(connStr, new AccessPool(connStr));
    //                    // 从现在开始10秒后，每隔10秒检查一次连接池，删除一个不使用的连接
    //                    CreateAndStartTimer();
    //                }
    //            }
    //        }
    //        return Pools[connStr].Open();
    //    }

    //    /// <summary>
    //    /// 把连接返回连接池
    //    /// </summary>
    //    /// <param name="connStr">连接字符串</param>
    //    /// <param name="conn">连接</param>
    //    public static void Close(String connStr, OleDbConnection conn)
    //    {
    //        if (String.IsNullOrEmpty(connStr)) return;
    //        if (conn == null) return;
    //        if (!Pools.ContainsKey(connStr)) return;
    //        Pools[connStr].Close(conn);
    //    }
    //    #endregion

    //    #region 检查连接池
    //    /// <summary>
    //    /// 检查连接池定时器。用于定时清理多余的连接
    //    /// </summary>
    //    private static Timer CheckPoolTimer;

    //    /// <summary>
    //    /// 建立并启动计时器。
    //    /// 使用无线等待时间的方式，使得线程池检查工作在可控的方式下进行
    //    /// 无限等待时间时，检查工作只会执行一次。
    //    /// 可以在一次检查完成的时候再启动新一次的等待。
    //    /// </summary>
    //    private static void CreateAndStartTimer()
    //    {
    //        if (CheckPoolTimer == null)
    //            CheckPoolTimer = new Timer(new TimerCallback(CheckPool), null, 10000, Timeout.Infinite);
    //        else
    //            CheckPoolTimer.Change(10000, Timeout.Infinite);
    //    }

    //    /// <summary>
    //    /// 定时检查连接池，每次检查都删除每个连接池的一个空闲连接
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private static void CheckPool(Object obj)
    //    {
    //        // 是否有连接被关闭
    //        Boolean IsClose = false;
    //        if (Pools != null && Pools.Values != null && Pools.Values.Count > 0)
    //        {
    //            foreach (AccessPool pool in Pools.Values)
    //            {
    //                Trace.WriteLine("CheckPool " + Pools.Count.ToString());
    //                if (pool.Check()) IsClose = true;
    //                Trace.WriteLine("CheckPool " + Pools.Count.ToString());
    //            }
    //        }
    //        if (IsClose) CreateAndStartTimer();
    //        //// 所有连接池都没有连接被关闭，那么，停止计时器，节省线程资源
    //        //if (!IsClose && CheckPoolTimer != null)
    //        //{
    //        //    lock (CheckPoolTimer)
    //        //    {
    //        //        if (!IsClose && CheckPoolTimer != null)
    //        //        {
    //        //            CheckPoolTimer.Dispose();
    //        //            CheckPoolTimer = null;
    //        //        }
    //        //    }
    //        //}
    //    }
    //    #endregion
    //}
    #endregion
}