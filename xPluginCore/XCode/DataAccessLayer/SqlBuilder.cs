using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// SQL��ѯ���������
    /// </summary>
    public class SqlBuilder
    {
        #region ����
        private const String SelectRegex = @"^\s*Select\s+(?<ѡ����>(?:(?! From )[\w\W])+?)\s+From\s+(?<���ݱ�>(?:(?! Where | Group\s+By | Having | Order\s+By )[\w\W])+?)(?:\s+Where\s+(?<����>(?:(?! Group\s+By | Having | Order\s+By )[\w\W])+?))?(?:\s+Group\s+By\s+(?<����>(?:(?! Having | Order\s+By )[\w\W])+?))?(?:\s+Having\s+(?<��������>(?:(?! Having | Order\s+By )[\w\W])+?))?(?:\s+Order\s+By\s+(?<����>[\w\W]+?))?\s*$";

        private DatabaseType _DbType = DatabaseType.Access;
        /// <summary>
        /// ���ݿ�����
        /// </summary>
        public DatabaseType DbType
        {
            get { return _DbType; }
            set { _DbType = value; }
        }

        /// <summary>
        /// �����SQL��仺��
        /// </summary>
        private String _outCache = String.Empty;
        #endregion

        #region SQL��ѯ����Ա
        ///// <summary>
        ///// �ڽ�������Ƿ������ʾ�ظ��С�ALL|DISTINCT
        ///// </summary>
        //public Boolean IsSelectAll = true;
        ///// <summary>
        ///// ����ǰN��
        ///// </summary>
        //public Int32 Top = 0;
        ///// <summary>
        ///// �Ƿ񷵻�ǰ�ٷ�֮N��
        ///// </summary>
        //public Boolean IsPercent = false;

        ///// <summary>
        ///// ѡ����
        ///// </summary>
        //public String[] Column;
        ///// <summary>
        ///// ���ݱ�
        ///// </summary>
        //public String[] FromTable;
        #endregion

        #region SQL��ѯ��������������
        private String _ColumnClause;
        /// <summary>
        /// ѡ����
        /// </summary>
        public String ColumnClause
        {
            get { return _ColumnClause; }
            set { _ColumnClause = value; _outCache = String.Empty; }
        }
        private String _TableClause;
        /// <summary>
        /// ���ݱ�
        /// </summary>
        public String TableClause
        {
            get { return _TableClause; }
            set { _TableClause = value; _outCache = String.Empty; }
        }
        private String _WhereClause;
        /// <summary>
        /// ����
        /// </summary>
        public String WhereClause
        {
            get { return _WhereClause; }
            set { _WhereClause = value; _outCache = String.Empty; }
        }
        private String _GroupByClause;
        /// <summary>
        /// ����
        /// </summary>
        public String GroupByClause
        {
            get { return _GroupByClause; }
            set { _GroupByClause = value; _outCache = String.Empty; }
        }
        private String _HavingClause;
        /// <summary>
        /// ��������
        /// </summary>
        public String HavingClause
        {
            get { return _HavingClause; }
            set { _HavingClause = value; _outCache = String.Empty; }
        }
        private String _OrderByClause;
        /// <summary>
        /// ����
        /// </summary>
        public String OrderByClause
        {
            get { return _OrderByClause; }
            set { _OrderByClause = value; _outCache = String.Empty; }
        }
        #endregion

        #region ���캯��
        /// <summary>
        /// ���캯��
        /// </summary>
        public SqlBuilder(DatabaseType dbType)
        {
            DbType = dbType;
        }

        /// <summary>
        /// ͨ������һ��SQL�������ʼ��һ��ʵ��
        /// </summary>
        /// <param name="dbType">���ݿ�����</param>
        /// <param name="sql">Ҫ������SQL���</param>
        private SqlBuilder(DatabaseType dbType, String sql)
        {
            DbType = dbType;
            Parse(sql);
        }

        private static Dictionary<String, SqlBuilder> _SqlBuilderCache = new Dictionary<string, SqlBuilder>();
        /// <summary>
        /// ͨ������һ��SQL�������ʼ��һ��ʵ��
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlBuilder Create(DatabaseType dbType, String sql)
        {
            String Key = String.Format("{0}_{1}", dbType, sql);
            if (_SqlBuilderCache.ContainsKey(Key)) return _SqlBuilderCache[Key];
            lock (_SqlBuilderCache)
            {
                if (_SqlBuilderCache.ContainsKey(Key)) return _SqlBuilderCache[Key];
                SqlBuilder sb = new SqlBuilder(dbType, sql);
                _SqlBuilderCache.Add(Key, sb);
                return sb;
            }
        }
        #endregion

        #region ����SQL
        /// <summary>
        /// ����һ��SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private Boolean Parse(String sql)
        {
            sql = TranSql(sql);

            Regex reg = new Regex(SelectRegex, RegexOptions.IgnoreCase);
            MatchCollection ms = reg.Matches(sql);
            if (ms[0].Success)
            {
                Match m = ms[0];
                ColumnClause = RevTranSql(m.Groups["ѡ����"].Value);
                TableClause = RevTranSql(m.Groups["���ݱ�"].Value);
                WhereClause = RevTranSql(m.Groups["����"].Value);
                GroupByClause = RevTranSql(m.Groups["����"].Value);
                HavingClause = RevTranSql(m.Groups["��������"].Value);
                OrderByClause = RevTranSql(m.Groups["����"].Value);
            }

            return Regex.IsMatch(sql, SelectRegex, RegexOptions.IgnoreCase);
        }
        #endregion

        #region ����SQL
        /// <summary>
        /// ����д����ȡ��Builder��������SQL���
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (!String.IsNullOrEmpty(_outCache)) return _outCache;

            StringBuilder sb = new StringBuilder();
            sb.Append("Select ");
            sb.Append(String.IsNullOrEmpty(ColumnClause) ? "*" : ColumnClause);
            //sb.Append(ColumnClause);
            sb.Append(" From ");
            sb.Append(TableClause);
            if (!String.IsNullOrEmpty(WhereClause)) sb.Append(" Where " + WhereClause);
            if (!String.IsNullOrEmpty(GroupByClause)) sb.Append(" Group By " + GroupByClause);
            if (!String.IsNullOrEmpty(HavingClause)) sb.Append(" Having " + HavingClause);
            if (!String.IsNullOrEmpty(OrderByClause)) sb.Append(" Order By " + OrderByClause);
            _outCache = RevTranSql(sb.ToString());
            return _outCache;
        }
        #endregion

        #region ��������
        /// <summary>
        /// SQLת���б�
        /// </summary>
        private Dictionary<Int32, String> SqlTranList = new Dictionary<Int32, String>();

        /// <summary>
        /// ��ת���б�
        /// </summary>
        private Dictionary<String, String> RevCache = new Dictionary<String, String>();

        /// <summary>
        /// SQLת�塣ȥ�������Ӳ�ѯ�������� �Լ� ˫����
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private String TranSql(String sql)
        {
            SqlTranList.Clear();
            sql = TranSql(sql, @"\(", @"\)");
            sql = TranSql(sql, "'", "'");
            sql = TranSql(sql, "\"", "\"");
            return sql;
        }

        /// <summary>
        /// ת��
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private String TranSql(String sql, String start, String end)
        {
            Regex reg = new Regex(String.Format("{0}.*?{1}", start, end), RegexOptions.IgnoreCase | RegexOptions.Compiled);
            MatchCollection ms = reg.Matches(sql);
            if (ms.Count < 1) return sql;
            foreach (Match m in ms)
            {
                if (!String.IsNullOrEmpty(m.Groups[0].Value))
                {
                    sql = sql.Replace(m.Groups[0].Value, String.Format("#{0}#", SqlTranList.Count));
                    SqlTranList.Add(SqlTranList.Count, m.Groups[0].Value);
                }
            }
            // �Ѿ��������һ���ת�壬����ݹ���������ת��
            return TranSql(sql, start, end);
        }

        /// <summary>
        /// ��ת��SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private String RevTranSql(String sql)
        {
            Regex reg = new Regex(@"#\d+#");
            if (!reg.IsMatch(sql)) return sql;
            foreach (Int32 k in SqlTranList.Keys)
            {
                sql = sql.Replace(String.Format("#{0}#", k), SqlTranList[k]);
            }
            return RevTranSql(sql);
        }

        /// <summary>
        /// ��ת��һ�㡣ʵ�����⵽��չ������
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public String RevTranTop(String str)
        {
            if (RevCache.ContainsKey(str)) return RevCache[str];
            lock (this)
            {
                if (RevCache.ContainsKey(str)) return RevCache[str];
                String tem = str;
                foreach (Int32 k in SqlTranList.Keys)
                {
                    tem = tem.Replace(String.Format("#{0}#", k), SqlTranList[k]);
                }
                RevCache.Add(str, tem);
                return tem;
            }
        }
        #endregion
    }
}