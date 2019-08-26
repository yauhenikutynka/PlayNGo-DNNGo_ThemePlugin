using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// SQL查询语句生成器
    /// </summary>
    public class SqlBuilder
    {
        #region 属性
        private const String SelectRegex = @"^\s*Select\s+(?<选择列>(?:(?! From )[\w\W])+?)\s+From\s+(?<数据表>(?:(?! Where | Group\s+By | Having | Order\s+By )[\w\W])+?)(?:\s+Where\s+(?<条件>(?:(?! Group\s+By | Having | Order\s+By )[\w\W])+?))?(?:\s+Group\s+By\s+(?<分组>(?:(?! Having | Order\s+By )[\w\W])+?))?(?:\s+Having\s+(?<分组条件>(?:(?! Having | Order\s+By )[\w\W])+?))?(?:\s+Order\s+By\s+(?<排序>[\w\W]+?))?\s*$";

        private DatabaseType _DbType = DatabaseType.Access;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DatabaseType DbType
        {
            get { return _DbType; }
            set { _DbType = value; }
        }

        /// <summary>
        /// 输出的SQL语句缓存
        /// </summary>
        private String _outCache = String.Empty;
        #endregion

        #region SQL查询语句成员
        ///// <summary>
        ///// 在结果集中是否可以显示重复行。ALL|DISTINCT
        ///// </summary>
        //public Boolean IsSelectAll = true;
        ///// <summary>
        ///// 返回前N行
        ///// </summary>
        //public Int32 Top = 0;
        ///// <summary>
        ///// 是否返回前百分之N行
        ///// </summary>
        //public Boolean IsPercent = false;

        ///// <summary>
        ///// 选择列
        ///// </summary>
        //public String[] Column;
        ///// <summary>
        ///// 数据表
        ///// </summary>
        //public String[] FromTable;
        #endregion

        #region SQL查询语句六大基本部分
        private String _ColumnClause;
        /// <summary>
        /// 选择列
        /// </summary>
        public String ColumnClause
        {
            get { return _ColumnClause; }
            set { _ColumnClause = value; _outCache = String.Empty; }
        }
        private String _TableClause;
        /// <summary>
        /// 数据表
        /// </summary>
        public String TableClause
        {
            get { return _TableClause; }
            set { _TableClause = value; _outCache = String.Empty; }
        }
        private String _WhereClause;
        /// <summary>
        /// 条件
        /// </summary>
        public String WhereClause
        {
            get { return _WhereClause; }
            set { _WhereClause = value; _outCache = String.Empty; }
        }
        private String _GroupByClause;
        /// <summary>
        /// 分组
        /// </summary>
        public String GroupByClause
        {
            get { return _GroupByClause; }
            set { _GroupByClause = value; _outCache = String.Empty; }
        }
        private String _HavingClause;
        /// <summary>
        /// 分组条件
        /// </summary>
        public String HavingClause
        {
            get { return _HavingClause; }
            set { _HavingClause = value; _outCache = String.Empty; }
        }
        private String _OrderByClause;
        /// <summary>
        /// 排序
        /// </summary>
        public String OrderByClause
        {
            get { return _OrderByClause; }
            set { _OrderByClause = value; _outCache = String.Empty; }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlBuilder(DatabaseType dbType)
        {
            DbType = dbType;
        }

        /// <summary>
        /// 通过分析一条SQL语句来初始化一个实例
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="sql">要分析的SQL语句</param>
        private SqlBuilder(DatabaseType dbType, String sql)
        {
            DbType = dbType;
            Parse(sql);
        }

        private static Dictionary<String, SqlBuilder> _SqlBuilderCache = new Dictionary<string, SqlBuilder>();
        /// <summary>
        /// 通过分析一条SQL语句来初始化一个实例
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

        #region 导入SQL
        /// <summary>
        /// 分析一条SQL
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
                ColumnClause = RevTranSql(m.Groups["选择列"].Value);
                TableClause = RevTranSql(m.Groups["数据表"].Value);
                WhereClause = RevTranSql(m.Groups["条件"].Value);
                GroupByClause = RevTranSql(m.Groups["分组"].Value);
                HavingClause = RevTranSql(m.Groups["分组条件"].Value);
                OrderByClause = RevTranSql(m.Groups["排序"].Value);
            }

            return Regex.IsMatch(sql, SelectRegex, RegexOptions.IgnoreCase);
        }
        #endregion

        #region 导出SQL
        /// <summary>
        /// 已重写。获取本Builder所分析的SQL语句
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

        #region 辅助函数
        /// <summary>
        /// SQL转义列表
        /// </summary>
        private Dictionary<Int32, String> SqlTranList = new Dictionary<Int32, String>();

        /// <summary>
        /// 反转义列表
        /// </summary>
        private Dictionary<String, String> RevCache = new Dictionary<String, String>();

        /// <summary>
        /// SQL转义。去除所有子查询，单引号 以及 双引号
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
        /// 转义
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
            // 已经完成最内一层的转义，下面递归对外面进行转义
            return TranSql(sql, start, end);
        }

        /// <summary>
        /// 反转义SQL
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
        /// 反转义一层。实现由外到内展开括号
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