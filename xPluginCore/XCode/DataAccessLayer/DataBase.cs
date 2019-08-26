using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
 

namespace DNNGo.Modules.ThemePlugin
{
    #region ���ݿ�����
    /// <summary>
    /// ���ݿ�����
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>
        /// MS��Access�ļ����ݿ�
        /// </summary>
        Access = 0,

        /// <summary>
        /// MS��SqlServer���ݿ�
        /// </summary>
        SqlServer = 1,

        /// <summary>
        /// Oracle���ݿ�
        /// </summary>
        Oracle = 2,

        /// <summary>
        /// MySql���ݿ�
        /// </summary>
        MySql = 3,

        /// <summary>
        /// MS��SqlServer2005���ݿ�
        /// </summary>
        SqlServer2005 = 4,

        /// <summary>
        /// SQLite���ݿ�
        /// </summary>
        SQLite = 5
    }
    #endregion

    /// <summary>
    /// ���ݿ���ࡣ
    /// ����Ϊpublic���������������Լ��ɣ������д������ݿ����ࡣ
    /// ����Ϊ�����ݿ��ඨ����һ����ܣ�Ĭ��ʹ��Access��
    /// SqlServer��Oracle����д���������
    /// </summary>
    internal abstract class DataBase : IDataBase, IDisposable
    {
        #region ���캯��
        /// <summary>
        /// ����һ�����ݷ��ʶ���
        /// </summary>
        /// <param name="connStr">�����ַ���</param>
        /// <param name="dbProviderFactory">���ݿ��ṩ�߹���</param>
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
        /// ����һ�����ݷ��ʶ���
        /// </summary>
        /// <param name="conn">����</param>
        /// <param name="dbProviderFactory">���ݿ��ṩ�߹���</param>
        public DataBase(DbConnection conn, DbProviderFactory dbProviderFactory)
        {
            Conn = conn;
            _dbProviderFactory = dbProviderFactory;
        }

        /// <summary>
        /// �Ƿ��Ѿ��ͷ�
        /// </summary>
        private Boolean IsDisposed = false;
        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        public virtual void Dispose()
        {
            if (IsDisposed) return;
            try
            {
                // ע�⣬û��Commit�����ݣ������ｫ�ᱻ�ع�
                //if (Trans != null) Rollback();
                if (Trans != null && Opened) Trans.Rollback();
                if (Conn != null) Close();
                IsDisposed = true;
            }
            catch (Exception ex)
            {
                XTrace.WriteLine("ִ��" + this.GetType().FullName + "��Disposeʱ����" + ex.ToString());
            }
        }

        ~DataBase()
        {
            Dispose();
        }
        #endregion

        #region ����
        private readonly DbProviderFactory _dbProviderFactory;
        private DbConnection _Conn;
        /// <summary>
        /// �������Ӷ���
        /// </summary>
        protected DbConnection Conn
        {
            get { return _Conn; }
            set { _Conn = value; }
        }
        /// <summary>
        /// �������ݿ����͡��ⲿDAL���ݿ�����ʹ��Other
        /// </summary>
        public abstract DatabaseType DbType { get; }

        private Int32 _QueryTimes;
        /// <summary>
        /// ��ѯ����
        /// </summary>
        public Int32 QueryTimes
        {
            get { return _QueryTimes; }
            set { _QueryTimes = value; }
        }

        private Int32 _ExecuteTimes;
        /// <summary>
        /// ִ�д���
        /// </summary>
        public Int32 ExecuteTimes
        {
            get { return _ExecuteTimes; }
            set { _ExecuteTimes = value; }
        }

        private static Int32 gid = 0;

        private Int32? _ID;
        /// <summary>
        /// ��ʶ
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

        #region ��/�ر�
        private Boolean _IsAutoClose = true;
        /// <summary>
        /// �Ƿ��Զ��رա�
        /// ��������󣬸�������Ч��
        /// ���ύ��ع�����ʱ�����IsAutoCloseΪtrue������Զ��ر�
        /// </summary>
        public Boolean IsAutoClose
        {
            get { return _IsAutoClose; }
            set { _IsAutoClose = value; }
        }

        /// <summary>
        /// �����Ƿ��Ѿ���
        /// </summary>
        public Boolean Opened
        {
            get { return Conn != null && Conn.State != ConnectionState.Closed; }
        }

        /// <summary>
        /// ��
        /// </summary>
        public virtual void Open()
        {
            if (Conn != null && Conn.State == ConnectionState.Closed)
            {
                try { Conn.Open(); }
                catch (Exception ex)
                {
                    XTrace.WriteLine("ִ��" + this.GetType().FullName + "��Openʱ����" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// �ر�
        /// </summary>
        public virtual void Close()
        {
            if (Conn != null && Conn.State != ConnectionState.Closed)
            {
                try { Conn.Close(); }
                catch (Exception ex)
                {
                    XTrace.WriteLine("ִ��" + this.GetType().FullName + "��Closeʱ����" + ex.ToString());
                }
            }
        }

        /// <summary>
        /// �Զ��رա�
        /// ��������󣬲��ر����ӡ�
        /// ���ύ��ع�����ʱ�����IsAutoCloseΪtrue������Զ��ر�
        /// </summary>
        public void AutoClose()
        {
            if (IsAutoClose && Trans == null && Opened) Close();
        }

        /// <summary>
        /// ���쳣����ʱ�������ر����ݿ����ӣ����߷������ӵ����ӳء�
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected virtual Exception OnException(Exception ex)
        {
            if (Trans == null && Opened) Close(); // ǿ�ƹر����ݿ�
            //return new XException("�ڲ����ݿ�ʵ��" + this.GetType().FullName + "�쳣��ִ��" + Environment.StackTrace + "��������", ex);
            String err = "�ڲ����ݿ�ʵ��" + DbType.ToString() + "�쳣��ִ�з�������" + Environment.NewLine + ex.Message;
            if (ex != null)
                return new Exception(err, ex);
            else
                return new Exception(err);
        }

        /// <summary>
        /// ���쳣����ʱ�������ر����ݿ����ӣ����߷������ӵ����ӳء�
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected virtual Exception OnException(Exception ex, String sql)
        {
            if (Trans == null && Opened) Close(); // ǿ�ƹر����ݿ�
            //return new XException("�ڲ����ݿ�ʵ��" + this.GetType().FullName + "�쳣��ִ��" + Environment.StackTrace + "��������", ex);
            String err = "execute data:" + DbType.ToString() + " Exception��Error execution method!" + Environment.NewLine;
            if (!String.IsNullOrEmpty(sql)) err += "SQL��" + sql + Environment.NewLine;
            err += ex.Message;
            if (ex != null)
                return new Exception(err, ex);
            else
                return new Exception(err);
        }
        #endregion

        #region ����
        private DbTransaction _Trans;
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        protected DbTransaction Trans
        {
            get { return _Trans; }
            set { _Trans = value; }
        }

        /// <summary>
        /// ���������
        /// ���ҽ��������������1ʱ�����ύ��ع���
        /// </summary>
        private Int32 TransactionCount = 0;

        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <returns></returns>
        public DbTransaction BeginTransaction()
        {
            if (Debug) WriteLog("��ʼ����{0}", ID);

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
        /// �ύ����
        /// </summary>
        public void Commit()
        {
            if (Debug) WriteLog("�ύ����{0}", ID);

            TransactionCount--;
            if (TransactionCount > 0) return;

            if (Trans == null) throw new InvalidOperationException("��ǰ��δ��ʼ��������BeginTransaction������ʼ������ID=" + ID);
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
        /// �ع�����
        /// </summary>
        public void Rollback()
        {
            if (Debug) WriteLog("�ع�����{0}", ID);

            TransactionCount--;
            if (TransactionCount > 0) return;

            if (Trans == null) throw new InvalidOperationException("��ǰ��δ��ʼ��������BeginTransaction������ʼ������ID=" + ID);
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

        #region �������� ��ѯ/ִ��
        /// <summary>
        /// ִ��SQL��ѯ�����ؼ�¼��
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns></returns>
        public virtual DataSet Query(String sql)
        {
            QueryTimes++;
            if (SqlLog) XTrace.WriteLine("Sql�����" + sql);
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
        /// ִ��DbCommand�����ؼ�¼��
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
        /// ִ��SQL��ѯ��ִ�в�ѯ�������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С������������к��н������ԡ�
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns></returns>
        public virtual Object ExecuteScalar(String sql)
        {
            ExecuteTimes++;
            if (SqlLog) XTrace.WriteLine("Sql�����" + sql);
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
        /// ִ��DbCommand��ִ�в�ѯ�������ز�ѯ�����صĽ�����е�һ�еĵ�һ�С������������к��н������ԡ�
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
        /// ִ��SQL��ѯ�������ܼ�¼��
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns></returns>
        public virtual Int32 QueryCount(String sql)
        {
            String orderBy = CheckOrderClause(ref sql);
            sql = String.Format("Select Count(*) From {0}", CheckSimpleSQL(sql));

            QueryTimes++;
            DbCommand cmd = PrepareCommand();
            cmd.CommandText = sql;
            if (SqlLog) XTrace.WriteLine("Sql�����" + cmd.CommandText);
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
        /// ִ��SQL��䣬������Ӱ�������
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns></returns>
        public virtual Int32 Execute(String sql)
        {
            ExecuteTimes++;
            if (SqlLog) XTrace.WriteLine("Sql�����" + sql);
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
        /// ִ��DbCommand��������Ӱ�������
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
        /// ִ�в�����䲢���������е��Զ����
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns>�����е��Զ����</returns>
        public virtual Int32 InsertAndGetIdentity(String sql)
        {
            ExecuteTimes++;
            //SQLServerд��
            sql = "SET NOCOUNT ON;" + sql + ";Select SCOPE_IDENTITY()";
            if (SqlLog) XTrace.WriteLine("Sql�����" + sql);
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
        /// ��ȡһ��DbCommand��
        /// ���������ӣ�������������
        /// �����Ѵ򿪡�
        /// ʹ����Ϻ󣬱������AutoClose��������ʹ���ڷ������������Զ��رյ�����¹ر�����
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

        #region ��ҳ
        /// <summary>
        /// �����ҳSQL
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
        /// <param name="maximumRows">��󷵻�����</param>
        /// <param name="keyColumn">Ψһ��������not in��ҳ</param>
        /// <returns>��ҳSQL</returns>
        public virtual String PageSplit(String sql, Int32 startRowIndex, Int32 maximumRows, String keyColumn)
        {
            if (String.IsNullOrEmpty(keyColumn)) throw new ArgumentNullException("keyColumn", "�����õ�not in��ҳ�㷨Ҫ��ָ�������У�");

            // �ӵ�һ�п�ʼ������Ҫ��ҳ
            if (startRowIndex <= 0 && maximumRows < 1) return sql;

            #region Max/Min��ҳ
            // ���Ҫʹ��max/min��ҳ��������keyColumn������asc����desc
            if (keyColumn.ToLower().EndsWith(" desc") || keyColumn.ToLower().EndsWith(" asc") || keyColumn.ToLower().EndsWith(" unknown"))
            {
                String str = PageSplitMaxMin(sql, startRowIndex, maximumRows, keyColumn);
                if (!String.IsNullOrEmpty(str)) return str;
                keyColumn = keyColumn.Substring(0, keyColumn.IndexOf(" "));
            }
            #endregion

            //����SQL��Ϊ�������ɷ�ҳSQL����
            String tablename = CheckSimpleSQL(sql);
            if (tablename != sql)
                sql = tablename;
            else
                sql = String.Format("({0}) XCode_Temp_a", sql);

            // ȡ��һҳҲ���÷�ҳ���������ŵ������Ҫ�����ַ�ҳ��Ҫ�Լ������������
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
            // Ψһ����˳��Ĭ��ΪEmpty������Ϊasc��desc������У������������������Ψһ�У�����ʹ��max/min��ҳ��
            Boolean isAscOrder = keyColumn.ToLower().EndsWith(" asc");
            // �Ƿ�ʹ��max/min��ҳ��
            Boolean canMaxMin = false;

            // ���sql�������������Ψһ��һ�������ֶξ���keyColumnʱ������max/min��ҳ��
            // ���sql�����û��������������unknown������max/min��ҳ��
            MatchCollection ms = Regex.Matches(sql, @"\border\s*by\b([^)]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (ms != null && ms.Count > 0 && ms[0].Index > 0)
            {
                // ȡ��һҳҲ���÷�ҳ���������ŵ������Ҫ�����ַ�ҳ��Ҫ�Լ������������
                if (startRowIndex <= 0 && maximumRows > 0)
                    return String.Format("Select Top {0} * From {1}", maximumRows, CheckSimpleSQL(sql));

                keyColumn = keyColumn.Substring(0, keyColumn.IndexOf(" "));
                sql = sql.Substring(0, ms[0].Index);

                String strOrderBy = ms[0].Groups[1].Value.Trim();
                // ֻ��һ�������ֶ�
                if (!String.IsNullOrEmpty(strOrderBy) && !strOrderBy.Contains(","))
                {
                    // ��asc����desc��û��ʱ��Ĭ��Ϊasc
                    if (strOrderBy.ToLower().EndsWith(" desc"))
                    {
                        String str = strOrderBy.Substring(0, strOrderBy.Length - " desc".Length).Trim();
                        // �����ֶε���keyColumn
                        if (str.ToLower() == keyColumn.ToLower())
                        {
                            isAscOrder = false;
                            canMaxMin = true;
                        }
                    }
                    else if (strOrderBy.ToLower().EndsWith(" asc"))
                    {
                        String str = strOrderBy.Substring(0, strOrderBy.Length - " asc".Length).Trim();
                        // �����ֶε���keyColumn
                        if (str.ToLower() == keyColumn.ToLower())
                        {
                            isAscOrder = true;
                            canMaxMin = true;
                        }
                    }
                    else if (!strOrderBy.Contains(" ")) // �����ո���Ψһ�����ֶ�
                    {
                        // �����ֶε���keyColumn
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
                // ȡ��һҳҲ���÷�ҳ���������ŵ������Ҫ�����ַ�ҳ��Ҫ�Լ������������
                if (startRowIndex <= 0 && maximumRows > 0)
                {
                    //���ַ�ҳ�У�ҵ����һ��ʹ�ý���Entity����keyColumnָ�������
                    //���ǣ��ڵ�һҳ��ʱ��û���õ�keyColumn�������ݿ�һ��Ĭ��������
                    //��ʱ��ͻ���ֵ�һҳ�����򣬺���ҳ�ǽ��������ˡ�����������BUG
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
        /// ����SQL��䣬����Select * From table
        /// </summary>
        /// <param name="sql">�����SQL���</param>
        /// <returns>����Ǽ�SQL����򷵻ر��������򷵻��Ӳ�ѯ(sql) XCode_Temp_a</returns>
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
        /// ����Ƿ���Order�Ӿ��β������ǣ��ָ�sqlΪǰ��������
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        protected static String CheckOrderClause(ref String sql)
        {
            if (!sql.ToLower().Contains("order")) return null;

            // ʹ����������ϸ��жϡ��������Order By���������ұ�û��������)��������order by���Ҳ����Ӳ�ѯ�ģ�����Ҫ���⴦��
            MatchCollection ms = Regex.Matches(sql, @"\border\s*by\b([^)]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (ms == null || ms.Count < 1 || ms[0].Index < 1) return null;
            String orderBy = sql.Substring(ms[0].Index).Trim();
            sql = sql.Substring(0, ms[0].Index).Trim();

            return orderBy;
        }
        #endregion

        #region ����
        /// <summary>
        /// ��������Դ�ļܹ���Ϣ
        /// </summary>
        /// <param name="collectionName">ָ��Ҫ���صļܹ������ơ�</param>
        /// <param name="restrictionValues">Ϊ����ļܹ�ָ��һ������ֵ��</param>
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
        /// ȡ�����б���
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
        /// ȡ��ָ����������й���
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
                    if (dr["DATA_TYPE"].ToString() == "130" && dr["COLUMN_FLAGS"].ToString() == "234") //��ע����
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

        #region ��������
        /// <summary>
        /// ȡ��ָ�����������������
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
        /// ��������
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

        #region �������
        /// <summary>
        /// ȡ��ָ����������������
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
        /// �������
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

        #region �ֶ����͵��������Ͷ��ձ�
        /// <summary>
        /// �ֶ����͵��������Ͷ��ձ�
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

        #region Sql��־���
        private Boolean _SqlLog = false;
        /// <summary>
        /// Sql��־���
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
        /// �Ƿ����
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
        /// �����־
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLog(String msg)
        {
            XTrace.WriteLine(msg);
        }

        /// <summary>
        /// �����־
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