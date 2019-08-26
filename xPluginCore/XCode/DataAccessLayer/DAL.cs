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
    /// ���ݷ��ʲ㡣
    /// <remarks>
    /// ��Ҫ����ѡ��ͬ�����ݿ⣬��ͬ�����ݿ��CRUD�������
    /// ÿһ�����ݿ������ַ�������ӦΨһ��һ��DALʵ����
    /// ���ݿ������ַ�������д�������ļ��У�Ȼ����Createʱָ�����֣�
    /// Ҳ����ֱ�Ӱ������ַ�����ΪCreate�Ĳ������롣
    /// ��Web�У�ÿһ��DALʵ������Ϊÿһ�������ʼ��һ��DataBaseʵ����
    /// ��WinForm�У�ÿһ��DALʵ������Ϊÿһ���̳߳�ʼ��һ��DataBaseʵ����
    /// ÿһ�����ݿ����������ָ���������ձ�����*��ƥ�����л���
    /// </remarks>
    /// </summary>
    public class DAL
    {
        #region ��������
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="connName">������</param>
        private DAL(String connName)
        {
            _ConnName = connName;
            // ����޷�ȡ�������ַ��������connName���������ַ���
            if (!GetConnStr()) ConnStr = connName;
        }

        private static Dictionary<String, DAL> _dals = new Dictionary<String, DAL>();
        /// <summary>
        /// ����һ�����ݷ��ʲ������null��Ϊ�����ɻ�õ�ǰĬ�϶���
        /// </summary>
        /// <param name="connName">���������������ַ���</param>
        /// <returns>��Ӧ��ָ�����ӵ�ȫ��Ψһ�����ݷ��ʲ����</returns>
        public static DAL Create(String connName)
        {
            //��connNameΪnullʱ��_dals���沢û�а���null���������Ҫ��ǰ����
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
                // ����connName����Ϊ�����ڴ����������Զ�ʶ����ConnName
                _dals.Add(d.ConnName, d);
            }

            return d;
        }

        /// <summary>
        /// ȡ�������ַ���
        /// </summary>
        private Boolean GetConnStr()
        {
            ConnectionStringSettingsCollection cssc = System.Configuration.ConfigurationManager.ConnectionStrings;
            // �������ö�Ϊ�գ��˳�����Ҫ�Ӳ������������ַ���
            if (cssc == null || cssc.Count < 1) return false;

            // û�������������������Ĭ����������
            if (String.IsNullOrEmpty(_ConnName))
            {
                if (cssc["SiteSqlServer"] != null) // �����default�����ã���ʹ��default
                    _ConnName = "SiteSqlServer";
                else // ����ʹ�õ�һ������aspnet���ݿ������
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
            else if (cssc[_ConnName] == null) //��������������޷�ȡ��ֵ����������������������ַ������˳�
                return false;
            _ConnStr = cssc[ConnName].ConnectionString;
            // ��������
            if (String.IsNullOrEmpty(cssc[ConnName].ProviderName)) // û��ָ��������������Ҫ�������ַ����з�����
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
                    if (ass.Contains(",")) // ���г������ƣ����س���
                        DALType = Assembly.Load(ass.Substring(0, ass.IndexOf(","))).GetType(ass.Substring(ass.IndexOf(",") + 1, ass.Length), true, false);
                    else // û�г������ƣ���ʹ�ñ�����
                        DALType = this.GetType().Assembly.GetType(ass, true, false);
                }
            }
            return true;
        }

        /// <summary>
        /// ȡ�����ݿ�����
        /// </summary>
        private void GetDbType()
        {
            if (String.IsNullOrEmpty(_ConnStr)) return;
            // ��������
            String str = _ConnStr.ToLower();
            if (str.Contains("mssql") || str.Contains("sqloledb"))
                DALType = typeof(SqlServer);
            else if (str.Contains("microsoft.jet.oledb"))
                DALType = typeof(Access);
            else
                DALType = typeof(SqlServer);
        }
        #endregion

        #region ��̬����
        //private static Object _Default_ref = new Object();
        private static DAL _Default;
        /// <summary>
        /// ��ǰ���ݷ��ʶ���
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
            // �����Current��ֵ���ڷ�Web�����У���û��Ĭ�������ַ����ģ����Ա����ֹ�����Ĭ�Ϸ��ʶ���
            set { _Default = value; }
        }
        #endregion

        #region ����
        private String _ConnName;
        /// <summary>
        /// ��������ֻ������Ҫ���ã�����������һ��DAL����
        /// </summary>
        public String ConnName
        {
            get { return _ConnName; }
        }

        private Type _DALType;
        /// <summary>
        /// ���ݷ��ʲ�������͡�
        /// <remarks>�ı����ݷ��ʲ����ݿ�ʵ���Ͽ���ǰ���ӣ��������κ����ݿ����֮ǰ�ı�</remarks>
        /// </summary>
        public Type DALType
        {
            get { return _DALType; }
            set	// ����ⲿ��Ҫ�ı����ݷ��ʲ����ݿ�ʵ��
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
        /// ���ݿ�����
        /// </summary>
        public DatabaseType DbType
        {
            get { return DB.DbType; }
        }

        private String _ConnStr;
        /// <summary>
        /// Ĭ�������ַ�������һ��ConnectionString����
        /// </summary>
        public String ConnStr
        {
            get { return _ConnStr; }
            // �����ConnStr��ֵ����ĳЩ��������±����ֹ����ã����������ⲿ��ֵ���ⲿ����ͨ��Create����
            private set
            {
                _ConnStr = value;
                GetDbType();
            }
        }

        /// <summary>
        /// ThreadStatic ָʾ��̬�ֶε�ֵ����ÿ���̶߳���Ψһ�ġ�
        /// </summary>
        [ThreadStatic]
        private static IDictionary<String, IDataBase> _DBs;
        /// <summary>
        /// DAL����
        /// <remarks>
        /// ����ʹ���̼߳���������󼶻��棬��֤�������ݿ�����̰߳�ȫ��
        /// ʹ���ⲿ���ݿ�������ʹ�����������½���
        /// </remarks>
        /// </summary>
        private IDataBase DB
        {
            get
            {
                if (String.IsNullOrEmpty(ConnStr)) throw new Exception("Please set the connection string in the database before using");

                //if (HttpContext.Current == null) // ��Web����ʹ���̼߳�����
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

                // ����������ȡ�ó��򼯣��ٴ���ʵ������Ϊ�˷�ֹ�ڱ����򼯴����ⲿDAL���ʵ��������

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
                //����Ƿ�SqlServer2005
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

                if (DataBase.Debug) DataBase.WriteLog("����DB��NotWeb����{0}", _DB.ID);

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

                if (DataBase.Debug) DataBase.WriteLog("����DB��Web����{0}", d.ID);

                HttpContext.Current.Items.Add(key, d);
            }
            //����Ƿ�SqlServer2005
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
        //    //����Ƿ�SqlServer2005
        //    if (db.DbType != DatabaseType.SqlServer) return db;

        //    //ȡ���ݿ�汾
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
            //����Ƿ�SqlServer2005
            if (db.DbType != DatabaseType.SqlServer) return false;

            //ȡ���ݿ�汾
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
        /// �Ƿ����DBʵ����
        /// ���ֱ��ʹ��DB�����ж��Ƿ���ڣ������ᴴ��һ��ʵ����
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

        #region ʹ�û��������ݲ�������
        #region ����
        private Boolean _EnableCache = true;
        /// <summary>
        /// �Ƿ����û��档
        /// <remarks>��Ϊfalse����ջ���</remarks>
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
        /// �������
        /// </summary>
        public Int32 CacheCount
        {
            get
            {
                return XCache.Count;
            }
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        public Int32 QueryTimes
        {
            get { return DB != null ? DB.QueryTimes : 0; }
        }

        /// <summary>
        /// ִ�д���
        /// </summary>
        public Int32 ExecuteTimes
        {
            get { return DB != null ? DB.ExecuteTimes : 0; }
        }

        private Boolean _EnableSqlLog;
        /// <summary>
        /// �Ƿ�����Sql��־�����
        /// </summary>
        public Boolean EnableSqlLog
        {
            get { return _EnableSqlLog; }
            set { _EnableSqlLog = value; }
        }
        #endregion

        private static Dictionary<String, String> _PageSplitCache = new Dictionary<String, String>();
        /// <summary>
        /// ������������ͨ��ѯSQL��ʽ��Ϊ��ҳSQL��
        /// </summary>
        /// <remarks>
        /// ��Ϊ��Ҫ�̳���д��ԭ�����������в������㻺���ҳSQL��
        /// ���������������档
        /// </remarks>
        /// <param name="sql">SQL���</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
        /// <param name="maximumRows">��󷵻�����</param>
        /// <param name="keyColumn">Ψһ��������not in��ҳ</param>
        /// <returns>��ҳSQL</returns>
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
        /// ִ��SQL��ѯ�����ؼ�¼��
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="tableNames">�������ı�ı���</param>
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
        /// ִ��SQL��ѯ�����ؼ�¼��
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="tableName">�������ı�ı���</param>
        /// <returns></returns>
        public DataSet Select(String sql, String tableName)
        {
            return Select(sql, new String[] { tableName });
        }

        /// <summary>
        /// ִ��SQL��ѯ�����ط�ҳ��¼��
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
        /// <param name="maximumRows">��󷵻�����</param>
        /// <param name="keyColumn">Ψһ��������not in��ҳ</param>
        /// <param name="tableNames">�������ı�ı���</param>
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
        /// ִ��SQL��ѯ�����ط�ҳ��¼��
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
        /// <param name="maximumRows">��󷵻�����</param>
        /// <param name="keyColumn">Ψһ��������not in��ҳ</param>
        /// <param name="tableName">�������ı�ı���</param>
        /// <returns></returns>
        public DataSet Select(String sql, Int32 startRowIndex, Int32 maximumRows, String keyColumn, String tableName)
        {
            return Select(sql, startRowIndex, maximumRows, keyColumn, new String[] { tableName });
        }

        /// <summary>
        /// ִ��SQL��ѯ�������ܼ�¼��
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="tableNames">�������ı�ı���</param>
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
        /// ִ��SQL��ѯ�������ܼ�¼��
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="tableName">�������ı�ı���</param>
        /// <returns></returns>
        public Int32 SelectCount(String sql, String tableName)
        {
            return SelectCount(sql, new String[] { tableName });
        }

        /// <summary>
        /// ִ��SQL��ѯ�������ܼ�¼��
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
        /// <param name="maximumRows">��󷵻�����</param>
        /// <param name="keyColumn">Ψһ��������not in��ҳ</param>
        /// <param name="tableNames">�������ı�ı���</param>
        /// <returns></returns>
        public Int32 SelectCount(String sql, Int32 startRowIndex, Int32 maximumRows, String keyColumn, String[] tableNames)
        {
            return SelectCount(PageSplit(sql, startRowIndex, maximumRows, keyColumn), tableNames);
        }

        /// <summary>
        /// ִ��SQL��ѯ�������ܼ�¼��
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
        /// <param name="maximumRows">��󷵻�����</param>
        /// <param name="keyColumn">Ψһ��������not in��ҳ</param>
        /// <param name="tableName">�������ı�ı���</param>
        /// <returns></returns>
        public Int32 SelectCount(String sql, Int32 startRowIndex, Int32 maximumRows, String keyColumn, String tableName)
        {
            return SelectCount(sql, startRowIndex, maximumRows, keyColumn, new String[] { tableName });
        }

        /// <summary>
        /// ִ��SQL��䣬������Ӱ�������
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="tableNames">��Ӱ��ı�ı���</param>
        /// <returns></returns>
        public Int32 Execute(String sql, String[] tableNames)
        {
            // �Ƴ����к���Ӱ����йصĻ���
            if (EnableCache) XCache.Remove(tableNames);
            return DB.Execute(sql);
        }

        /// <summary>
        /// ִ��SQL��䣬������Ӱ�������
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="tableName">��Ӱ��ı�ı���</param>
        /// <returns></returns>
        public Int32 Execute(String sql, String tableName)
        {
            return Execute(sql, new String[] { tableName });
        }

        /// <summary>
        /// ִ��SQL��䣬������Ӱ�������
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="tableNames">��Ӱ��ı�ı���</param>
        /// <returns></returns>
        public Object ExecuteScalar(String sql, String[] tableNames)
        {
            // �Ƴ����к���Ӱ����йصĻ���
            if (EnableCache) XCache.Remove(tableNames);
            return DB.ExecuteScalar(sql);
        }

        /// <summary>
        /// ִ��SQL��䣬������Ӱ�������
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="tableName">��Ӱ��ı�ı���</param>
        /// <returns></returns>
        public Object ExecuteScalar(String sql, String tableName)
        {
            return ExecuteScalar(sql, new String[] { tableName });
        }

        /// <summary>
        /// ִ�в�����䲢���������е��Զ����
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="tableNames">��Ӱ��ı�ı���</param>
        /// <returns>�����е��Զ����</returns>
        public Int32 InsertAndGetIdentity(String sql, String[] tableNames)
        {
            // �Ƴ����к���Ӱ����йصĻ���
            if (EnableCache) XCache.Remove(tableNames);
            return DB.InsertAndGetIdentity(sql);
        }

        /// <summary>
        /// ִ�в�����䲢���������е��Զ����
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="tableName">��Ӱ��ı�ı���</param>
        /// <returns>�����е��Զ����</returns>
        public Int32 InsertAndGetIdentity(String sql, String tableName)
        {
            return InsertAndGetIdentity(sql, new String[] { tableName });
        }

        /// <summary>
        /// ִ��CMD�����ؼ�¼��
        /// </summary>
        /// <param name="cmd">CMD</param>
        /// <param name="tableNames">�������ı�ı���</param>
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
        /// ִ��CMD��������Ӱ�������
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public Int32 Execute(DbCommand cmd, String[] tableNames)
        {
            // �Ƴ����к���Ӱ����йصĻ���
            if (EnableCache) XCache.Remove(tableNames);
            Int32 ret = DB.Execute(cmd);
            DB.AutoClose();
            return ret;
        }

        ///// <summary>
        ///// ��ȡһ��DbCommand��
        ///// ���������ӣ�������������
        ///// �����Ѵ򿪡�
        ///// ʹ����Ϻ󣬱������AutoClose��������ʹ���ڷ������������Զ��رյ�����¹ر����ӡ�
        ///// �����Ȳ����ѣ������벻Ҫʹ�ø÷��������Կ�����Select(cmd)��Execute(cmd)�����档
        ///// �Ƿ�ʹ�û�ʹ����Դʧȥ���ơ�����Σ�գ�
        ///// </summary>
        ///// <returns></returns>
        //private DbCommand PrepareCommand()
        //{
        //    return DB.PrepareCommand();
        //}

        /// <summary>
        /// ȡ�����б����ͼ�Ĺ�����Ϣ
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

        #region ����
        /// <summary>
        /// ��ʼ����
        /// ����һ����ʼ��������ڲ�����ɺ��ύ����ʧ��ʱ�ع���������ܻ������Դʧȥ���ơ�����Σ�գ�
        /// </summary>
        /// <returns></returns>
        public DbTransaction BeginTransaction()
        {
            return DB.BeginTransaction();
        }

        /// <summary>
        /// �ύ����
        /// </summary>
        public void Commit()
        {
            DB.Commit();
        }

        /// <summary>
        /// �ع�����
        /// </summary>
        public void Rollback()
        {
            DB.Rollback();
        }
        #endregion

        #region ���뵼��
        /// <summary>
        /// �����ܹ���Ϣ
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
        /// ����ܹ���Ϣ
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