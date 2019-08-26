using System;
using System.Data.Common;
using System.Reflection;
 

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// SQLite数据库
    /// </summary>
    internal class SQLite : DataBase
    {
        #region 构造函数
        private static DbProviderFactory _dbProviderFactory;
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static DbProviderFactory dbProviderFactory
        {
            get
            {
                if (_dbProviderFactory == null)
                {
                    //反射实现获取数据库工厂
                    Assembly asm = Assembly.LoadFile("System.Data.SQLite.dll");
                    Type type = asm.GetType("SQLiteFactory");
                    FieldInfo field = type.GetField("Instance");
                    _dbProviderFactory = field.GetValue(null) as DbProviderFactory;
                }
                return _dbProviderFactory;
            }
        }

        /// <summary>
        /// 构造函数。自己创建Conn对象
        /// </summary>
        /// <param name="connStr">连接字符串</param>
        public SQLite(String connStr)
            : base("", _dbProviderFactory)
        {
            if (!String.IsNullOrEmpty(connStr))
            {
                Conn = _dbProviderFactory.CreateConnection();
                Conn.ConnectionString = connStr;
            }
        }

        /// <summary>
        /// 构造函数。使用外部连接
        /// </summary>
        /// <param name="conn">连接</param>
        public SQLite(DbConnection conn) : base(conn, _dbProviderFactory) { }
        #endregion

        #region 属性
        /// <summary>
        /// 返回数据库类型。
        /// </summary>
        public override DatabaseType DbType
        {
            get { return DatabaseType.SQLite; }
        }
        #endregion

        #region 基本方法 查询/执行
        /// <summary>
        /// 执行插入语句并返回新增行的自动编号
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>新增行的自动编号</returns>
        public override Int32 InsertAndGetIdentity(string sql)
        {
            ExecuteTimes++;
            sql = "SET NOCOUNT ON;" + sql + ";Select last_insert_rowid() newid";
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
        #endregion

        #region 分页
        /// <summary>
        /// 已重写。获取分页
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="startRowIndex">开始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <param name="keyColumn">主键列。用于not in分页</param>
        /// <returns></returns>
        public override string PageSplit(string sql, Int32 startRowIndex, Int32 maximumRows, string keyColumn)
        {
            // 从第一行开始，不需要分页
            if (startRowIndex <= 0)
            {
                if (maximumRows < 1)
                    return sql;
                else
                    return String.Format("{0} limit {1}", sql, maximumRows);
            }
            if (maximumRows < 1)
                throw new NotSupportedException("不支持取第几条数据之后的所有数据！");
            else
                sql = String.Format("{0} limit {1}, {2}", sql, startRowIndex, maximumRows);
            return sql;
        }
        #endregion
    }
}
