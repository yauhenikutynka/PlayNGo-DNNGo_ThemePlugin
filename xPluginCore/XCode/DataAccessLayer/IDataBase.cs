using System;
using System.Data;
using System.Data.Common;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 数据库接口。处于数据访问层DAL的每个数据库子类，都必须实现该接口
    /// </summary>
    interface IDataBase : IDisposable
    {
        /// <summary>
        /// 返回数据库类型。外部DAL数据库类请使用Other
        /// </summary>
        DatabaseType DbType { get; }

        /// <summary>
        /// 查询次数
        /// </summary>
        Int32 QueryTimes { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        Int32 ExecuteTimes { get; set; }

        #region 打开/关闭
        /// <summary>
        /// 是否自动关闭。
        /// 启用事务后，该设置无效。
        /// 在提交或回滚事务时，如果IsAutoClose为true，则会自动关闭
        /// </summary>
        bool IsAutoClose { get; set; }
        /// <summary>
        /// 连接是否已经打开
        /// </summary>
        bool Opened { get; }
        /// <summary>
        /// 打开
        /// </summary>
        void Open();
        /// <summary>
        /// 关闭
        /// </summary>
        void Close();

        /// <summary>
        /// 自动关闭。
        /// 启用事务后，不关闭连接。
        /// 在提交或回滚事务时，如果IsAutoClose为true，则会自动关闭
        /// </summary>
        void AutoClose();
        #endregion

        #region 事务
        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns></returns>
        DbTransaction BeginTransaction();
        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();
        #endregion

        #region 基本方法 查询/执行
        /// <summary>
        /// 执行SQL查询，返回记录集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        DataSet Query(String sql);
        /// <summary>
        /// 执行DbCommand，返回记录集
        /// </summary>
        /// <param name="cmd">DbCommand</param>
        /// <returns></returns>
        DataSet Query(DbCommand cmd);
        /// <summary>
        /// 执行SQL查询，返回总记录数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        Int32 QueryCount(String sql);
        /// <summary>
        /// 执行SQL语句，返回受影响的行数
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        Int32 Execute(String sql);
        /// <summary>
        /// 执行DbCommand，执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="cmd">DbCommand</param>
        /// <returns></returns>
        Object ExecuteScalar(DbCommand cmd);
        /// <summary>
        /// 执行SQL语句，执行查询，并返回查询所返回的结果集中第一行的第一列。所有其他的列和行将被忽略。
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        Object ExecuteScalar(String sql);
        /// <summary>
        /// 执行DbCommand，返回受影响的行数
        /// </summary>
        /// <param name="cmd">DbCommand</param>
        /// <returns></returns>
        Int32 Execute(DbCommand cmd);
        /// <summary>
        /// 执行插入语句并返回新增行的自动编号
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        Int32 InsertAndGetIdentity(String sql);
        /// <summary>
        /// 获取一个DbCommand。
        /// 配置了连接，并关联了事务。
        /// 连接已打开。
        /// 使用完毕后，必须调用AutoClose方法，以使得在非事务及设置了自动关闭的情况下关闭连接
        /// </summary>
        /// <returns></returns>
        DbCommand PrepareCommand();
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
        String PageSplit(String sql, Int32 startRowIndex, Int32 maximumRows, String keyColumn);
        #endregion

        #region 构架
        /// <summary>
        /// 取得所有表构架
        /// </summary>
        /// <returns></returns>
        XTable[] GetTables();
        #endregion

        #region Sql日志输出
        /// <summary>
        /// Sql日志输出
        /// </summary>
        Boolean SqlLog { get; set; }
        #endregion

        Int32 ID { get; }
    }
}