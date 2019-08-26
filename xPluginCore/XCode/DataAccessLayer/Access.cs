using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
 

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// Access���ݿ�
    /// </summary>
    internal class Access : DataBase
    {
        #region ����
        /// <summary>
        /// ���캯����Access���ݿ���Ҫ���������ַ����е����·�������Լ�����Conn����
        /// </summary>
        /// <param name="connStr"></param>
        public Access(String connStr)
            : base("", OleDbFactory.Instance)
        {
            if (String.IsNullOrEmpty(connStr) || connStr.Length < 10) throw new Exception("Are you sure you set the connection string correct it?");

            try
            {
                OleDbConnectionStringBuilder csb = new OleDbConnectionStringBuilder(connStr);
                // ���Ǿ���·��
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
                //// ʹ�����ӳغ󣬲���Ҫ����Conn
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
        ///// �����ַ���
        ///// </summary>
        //private String ConnectionString;

        /// <summary>
        /// ���캯����ʹ���ⲿ����
        /// </summary>
        /// <param name="conn">����</param>
        public Access(OleDbConnection conn)
            : base(conn, OleDbFactory.Instance)
        {
            //ConnectionString = conn.ConnectionString;
        }
        #endregion

        #region ����ʹ�����ӳ�
        /// <summary>
        /// �������ݿ����͡��ⲿDAL���ݿ�����ʹ��Other
        /// </summary>
        public override DatabaseType DbType
        {
            get { return DatabaseType.Access; }
        }

        ///// <summary>
        ///// �򿪡�����д��Ϊ��ʹ�����ӳ�
        ///// </summary>
        //public override void Open()
        //{
        //    Conn = AccessPool.Open(ConnectionString);
        //}

        ///// <summary>
        ///// �رա�����д��Ϊ��ʹ�����ӳ�
        ///// </summary>
        //public override void Close()
        //{
        //    AccessPool.Close(ConnectionString, Conn as OleDbConnection);
        //    Conn = null;
        //}

        ///// <summary>
        ///// ����ʱ���������ӵ����ӳ�
        ///// </summary>
        //~Access()
        //{
        //    if (Conn != null) Close();
        //}
        #endregion

        #region �������� ��ѯ/ִ��
        /// <summary>
        /// ִ�в�����䲢���������е��Զ����
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns>�����е��Զ����</returns>
        public override Int32 InsertAndGetIdentity(String sql)
        {
            ExecuteTimes++;
            if (SqlLog) XTrace.WriteLine("Sql�����" + sql);
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

        #region ����
        /// <summary>
        /// �����ء���������
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
        /// �����ء��������
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

    #region OleDb���ӳ�
    ///// <summary>
    ///// Access���ݿ����ӳء�
    ///// ÿ�������ַ���һ�����ӳء�
    ///// һ��ʱ��󣬹ر�δʹ�õĶ������ӣ�
    ///// </summary>
    //internal class AccessPool : IDisposable
    //{
    //    #region ���ӳصĴ���������
    //    /// <summary>
    //    /// �����ַ���
    //    /// </summary>
    //    private String ConnectionString;
    //    /// <summary>
    //    /// ˽�й��캯������ֹ�ⲿ����ʵ����
    //    /// </summary>
    //    /// <param name="connStr">�����ַ���</param>
    //    private AccessPool(String connStr)
    //    {
    //        ConnectionString = connStr;
    //    }

    //    private Boolean Disposed = false;
    //    /// <summary>
    //    /// �ͷ���������
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
    //                    Trace.WriteLine("��AccessPool���ӳ����ͷ���������ʱ����" + ex.ToString());
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
    //                    Trace.WriteLine("��AccessPool���ӳ����ͷ���������ʱ����" + ex.ToString());
    //                }
    //            }
    //            UsedList.Clear();
    //            //˫��
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
    //        // ��������ÿ�����ӳض����Dispose��Dispose���ֿ�����������
    //        Dispose();
    //    }
    //    #endregion

    //    #region ��/�� ����
    //    /// <summary>
    //    /// �����б�
    //    /// </summary>
    //    private List<OleDbConnection> FreeList = new List<OleDbConnection>();
    //    /// <summary>
    //    /// ʹ���б�
    //    /// </summary>
    //    private List<OleDbConnection> UsedList = new List<OleDbConnection>();
    //    /// <summary>
    //    /// ���ش�С
    //    /// </summary>
    //    public Int32 MaxPoolSize = 100;
    //    /// <summary>
    //    /// ��С�ش�С
    //    /// </summary>
    //    public Int32 MinPoolSize = 0;

    //    /// <summary>
    //    /// ȡ����
    //    /// </summary>
    //    /// <returns></returns>
    //    private OleDbConnection Open()
    //    {
    //        // ���̳߳�ͻ���������´�����ͬһʱ��ֻ����һ���߳̽���
    //        lock (this)
    //        {
    //            if (UsedList.Count >= MaxPoolSize) throw new XException("���ӳص�����������������ƣ��޷��ṩ����");
    //            OleDbConnection conn;
    //            // �����Ƿ������ӣ����û�У���Ҫ���ϴ���
    //            if (FreeList.Count < 1)
    //            {
    //                Trace.WriteLine("�½�����");
    //                conn = new OleDbConnection(ConnectionString);
    //                conn.Open();
    //                // ֱ�ӽ���ʹ���б�
    //                UsedList.Add(conn);
    //                return conn;
    //            }
    //            // �ӿ����б���ȡ��һ������
    //            conn = FreeList[0];
    //            // ��һ�������뿪�����б�
    //            FreeList.RemoveAt(0);
    //            // �����ӽ���ʹ���б�
    //            UsedList.Add(conn);
    //            // ��������Ƿ��Ѿ��򿪣����û�򿪣����
    //            if (conn.State == ConnectionState.Closed) conn.Open();
    //            return conn;
    //        }
    //    }

    //    /// <summary>
    //    /// ��������
    //    /// </summary>
    //    /// <param name="conn">���Ӷ���</param>
    //    private void Close(OleDbConnection conn)
    //    {
    //        if (conn == null || UsedList == null || UsedList.Count < 1) return;
    //        lock (this)
    //        {
    //            if (UsedList == null || UsedList.Count < 1) return;
    //            // ����ļ�飬ԭ������lock���棬�ڸ߲����Ļ����±����Ǹ������ܵ��쳣�������Ժ�һ��ҪDouble Lock
    //            // Double LockҲ���ǣ����->����->�ټ��->ִ��
    //            // �������Ӷ����Ƿ����Ա����ӳء�����ϢӦ�������ʱ�ھ���ʾ���԰��������߿�����������
    //            if (!UsedList.Contains(conn)) throw new XException("������AccessPool���ӳص����ӣ��������Ա����ӳأ�");
    //            // �뿪ʹ���б�
    //            UsedList.Remove(conn);
    //            // �ص������б�
    //            FreeList.Add(conn);
    //        }
    //    }
    //    #endregion

    //    #region �������
    //    /// <summary>
    //    /// ������ӳء��ر�δʹ�����ӣ���ֹ�򿪹������Ӷ��ֲ��ر�
    //    /// </summary>
    //    /// <returns>�Ƿ�ر������ӣ������߽��Դ�Ϊ�����������Ƿ�ͣ�ö�ʱ��</returns>
    //    private Boolean Check()
    //    {
    //        if (FreeList.Count < 1 || FreeList.Count + UsedList.Count <= MinPoolSize) return false;
    //        lock (this)
    //        {
    //            if (FreeList.Count < 1 || FreeList.Count + UsedList.Count <= MinPoolSize) return false;
    //            Trace.WriteLine("ɾ������");
    //            try
    //            {
    //                // �ر����п������ӣ���������С�ش�С
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
    //                Trace.WriteLine("���AccessPool���ӳ�ʱ����" + ex.ToString());
    //            }
    //            return true;
    //        }
    //    }
    //    #endregion

    //    #region �����ӳ��� ��/�� ����
    //    /// <summary>
    //    /// ���ӳؼ��ϡ������ַ�����Ϊ������ÿ�������ַ�����Ӧһ�����ӳء�
    //    /// </summary>
    //    private static Dictionary<String, AccessPool> Pools = new Dictionary<string, AccessPool>();

    //    /// <summary>
    //    /// �������
    //    /// </summary>
    //    /// <param name="connStr">�����ַ���</param>
    //    /// <returns></returns>
    //    public static OleDbConnection Open(String connStr)
    //    {
    //        if (String.IsNullOrEmpty(connStr)) return null;
    //        // ����Ƿ���������ַ���ΪconnStr�����ӳ�
    //        if (!Pools.ContainsKey(connStr))
    //        {
    //            lock (Pools)
    //            {
    //                if (!Pools.ContainsKey(connStr))
    //                {
    //                    Pools.Add(connStr, new AccessPool(connStr));
    //                    // �����ڿ�ʼ10���ÿ��10����һ�����ӳأ�ɾ��һ����ʹ�õ�����
    //                    CreateAndStartTimer();
    //                }
    //            }
    //        }
    //        return Pools[connStr].Open();
    //    }

    //    /// <summary>
    //    /// �����ӷ������ӳ�
    //    /// </summary>
    //    /// <param name="connStr">�����ַ���</param>
    //    /// <param name="conn">����</param>
    //    public static void Close(String connStr, OleDbConnection conn)
    //    {
    //        if (String.IsNullOrEmpty(connStr)) return;
    //        if (conn == null) return;
    //        if (!Pools.ContainsKey(connStr)) return;
    //        Pools[connStr].Close(conn);
    //    }
    //    #endregion

    //    #region ������ӳ�
    //    /// <summary>
    //    /// ������ӳض�ʱ�������ڶ�ʱ������������
    //    /// </summary>
    //    private static Timer CheckPoolTimer;

    //    /// <summary>
    //    /// ������������ʱ����
    //    /// ʹ�����ߵȴ�ʱ��ķ�ʽ��ʹ���̳߳ؼ�鹤���ڿɿصķ�ʽ�½���
    //    /// ���޵ȴ�ʱ��ʱ����鹤��ֻ��ִ��һ�Ρ�
    //    /// ������һ�μ����ɵ�ʱ����������һ�εĵȴ���
    //    /// </summary>
    //    private static void CreateAndStartTimer()
    //    {
    //        if (CheckPoolTimer == null)
    //            CheckPoolTimer = new Timer(new TimerCallback(CheckPool), null, 10000, Timeout.Infinite);
    //        else
    //            CheckPoolTimer.Change(10000, Timeout.Infinite);
    //    }

    //    /// <summary>
    //    /// ��ʱ������ӳأ�ÿ�μ�鶼ɾ��ÿ�����ӳص�һ����������
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    private static void CheckPool(Object obj)
    //    {
    //        // �Ƿ������ӱ��ر�
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
    //        //// �������ӳض�û�����ӱ��رգ���ô��ֹͣ��ʱ������ʡ�߳���Դ
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