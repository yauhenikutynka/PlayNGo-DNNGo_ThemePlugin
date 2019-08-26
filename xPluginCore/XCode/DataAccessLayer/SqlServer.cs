using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// SqlServer数据库
    /// </summary>
    internal class SqlServer : DataBase
    {
        #region 构造
        /// <summary>
        /// 构造函数。自己创建Conn对象
        /// </summary>
        /// <param name="connStr"></param>
        public SqlServer(String connStr)
            : base("", SqlClientFactory.Instance)
        {
            connStr = Regex.Replace(connStr, "Provider[ ]*=.*?;", "", RegexOptions.IgnoreCase);
            connStr = Regex.Replace(connStr, "Provider[ ]*=.*?$", "", RegexOptions.IgnoreCase);
            Conn = new SqlConnection(connStr);
            // WinForm程序，关闭自动关闭连接功能。因为一般该类程序是单机程序，最好维持数据库连接以优化性能
            //if (System.Web.HttpContext.Current == null) IsAutoClose = false;
        }

        /// <summary>
        /// 构造函数。使用外部连接
        /// </summary>
        /// <param name="conn">连接</param>
        public SqlServer(SqlConnection conn) : base(conn, SqlClientFactory.Instance) { }

        /// <summary>
        /// 返回数据库类型。外部DAL数据库类请使用Other
        /// </summary>
        public override DatabaseType DbType
        {
            get { return DatabaseType.SqlServer; }
        }
        #endregion

        #region 分页
        /// <summary>
        /// 执行SQL查询，返回分页记录集
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="startRowIndex">开始行，0开始</param>
        /// <param name="maximumRows">最大返回行数</param>
        /// <param name="keyColumn">主键列。用于not in分页</param>
        /// <returns></returns>
        public override String PageSplit(string sql, Int32 startRowIndex, Int32 maximumRows, string keyColumn)
        {
            // 从第一行开始，不需要分页
            if (startRowIndex <= 0 && maximumRows < 1) return sql;

            // 指定了起始行，并且是SQL2005及以上版本，使用RowNumber算法
            //if (startRowIndex > 0 && IsSQL2005) return PageSplitRowNumber(sql, startRowIndex, maximumRows, keyColumn);

            // 如果没有Order By，直接调用基类方法
            // 先用字符串判断，命中率高，这样可以提高处理效率
            if (!sql.Contains(" Order "))
            {
                if (!sql.ToLower().Contains(" order ")) return base.PageSplit(sql, startRowIndex, maximumRows, keyColumn);
            }
            //// 使用正则进行严格判断。必须包含Order By，并且它右边没有右括号)，表明有order by，且不是子查询的，才需要特殊处理
            //MatchCollection ms = Regex.Matches(sql, @"\border\s*by\b([^)]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //if (ms == null || ms.Count < 1 || ms[0].Index < 1)
            String sql2 = sql;
            String orderBy = CheckOrderClause(ref sql2);
            if (String.IsNullOrEmpty(orderBy))
            {
                return base.PageSplit(sql, startRowIndex, maximumRows, keyColumn);
            }
            // 已确定该sql最外层含有order by，再检查最外层是否有top。因为没有top的order by是不允许作为子查询的
            if (Regex.IsMatch(sql, @"^[^(]+\btop\b", RegexOptions.Compiled | RegexOptions.IgnoreCase))
            {
                return base.PageSplit(sql, startRowIndex, maximumRows, keyColumn);
            }
            //String orderBy = sql.Substring(ms[0].Index);

            // 从第一行开始，不需要分页
            if (startRowIndex <= 0)
            {
                if (maximumRows < 1)
                    return sql;
                else
                    return String.Format("Select Top {0} * From {1} {2}", maximumRows, CheckSimpleSQL(sql2), orderBy);
                //return String.Format("Select Top {0} * From {1} {2}", maximumRows, CheckSimpleSQL(sql.Substring(0, ms[0].Index)), orderBy);
            }

            #region Max/Min分页
            // 如果要使用max/min分页法，首先keyColumn必须有asc或者desc
            String kc = keyColumn.ToLower();
            if (kc.EndsWith(" desc") || kc.EndsWith(" asc") || kc.EndsWith(" unknown"))
            {
                String str = PageSplitMaxMin(sql, startRowIndex, maximumRows, keyColumn);
                if (!String.IsNullOrEmpty(str)) return str;
                keyColumn = keyColumn.Substring(0, keyColumn.IndexOf(" "));
            }
            #endregion

            sql = CheckSimpleSQL(sql2);

            if (String.IsNullOrEmpty(keyColumn)) throw new ArgumentNullException("keyColumn", "分页要求指定主键列或者排序字段！");

            if (maximumRows < 1)
                sql = String.Format("Select * From {1} Where {2} Not In(Select Top {0} {2} From {1} {3}) {3}", startRowIndex, sql, keyColumn, orderBy);
            else
                sql = String.Format("Select Top {0} * From {1} Where {2} Not In(Select Top {3} {2} From {1} {4}) {4}", maximumRows, sql, keyColumn, startRowIndex, orderBy);
            return sql;
        }
        /// <summary>
        /// 已重写。获取分页
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="startRowIndex">开始行，0表示第一行</param>
        /// <param name="maximumRows">最大返回行数，0表示所有行</param>
        /// <param name="keyColumn">主键列。用于not in分页</param>
        /// <returns></returns>
        public String PageSplitRowNumber(String sql, Int32 startRowIndex, Int32 maximumRows, String keyColumn)
        {
            // 从第一行开始，不需要分页
            if (startRowIndex <= 0)
            {
                if (maximumRows < 1)
                    return sql;
                else
                    return base.PageSplit(sql, startRowIndex, maximumRows, keyColumn);
            }

            String orderBy = String.Empty;
            if (sql.ToLower().Contains(" order "))
            {
                // 使用正则进行严格判断。必须包含Order By，并且它右边没有右括号)，表明有order by，且不是子查询的，才需要特殊处理
                //MatchCollection ms = Regex.Matches(sql, @"\border\s*by\b([^)]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                //if (ms != null && ms.Count > 0 && ms[0].Index > 0)
                String sql2 = sql;
                String orderBy2 = CheckOrderClause(ref sql2);
                if (String.IsNullOrEmpty(orderBy))
                {
                    // 已确定该sql最外层含有order by，再检查最外层是否有top。因为没有top的order by是不允许作为子查询的
                    if (!Regex.IsMatch(sql, @"^[^(]+\btop\b", RegexOptions.Compiled | RegexOptions.IgnoreCase))
                    {
                        //orderBy = sql.Substring(ms[0].Index).Trim();
                        //sql = sql.Substring(0, ms[0].Index).Trim();
                        orderBy = orderBy2.Trim();
                        sql = sql2.Trim();
                    }
                }
            }

            if (String.IsNullOrEmpty(orderBy))
            {
                if (String.IsNullOrEmpty(keyColumn)) throw new ArgumentNullException("keyColumn", "分页要求指定主键列或者排序字段！");

                //if (keyColumn.EndsWith(" Unknown", StringComparison.OrdinalIgnoreCase)) keyColumn = keyColumn.Substring(0, keyColumn.LastIndexOf(" "));
                orderBy = "Order By " + keyColumn;
            }
            sql = CheckSimpleSQL(sql);

            //row_number()从1开始
            if (maximumRows < 1)
                sql = String.Format("Select * From (Select *, row_number() over({2}) as rowNumber From {1}) XCode_Temp_b Where rowNumber>={0}", startRowIndex + 1, sql, orderBy);
            else
                sql = String.Format("Select * From (Select *, row_number() over({3}) as rowNumber From {1}) XCode_Temp_b Where rowNumber Between {0} And {2}", startRowIndex + 1, sql, startRowIndex + maximumRows, orderBy);

            return sql;
        }
        #endregion

        #region 构架
        /// <summary>
        /// 取得所有表构架
        /// </summary>
        /// <returns></returns>
        public override XTable[] GetTables()
        {
            List<XTable> list = null;
            try
            {
                DataTable dt = GetSchema("Tables", null);

                //一次性把所有的表说明查出来
                DataSet ds = Query(DescriptionSql);
                DataTable DescriptionTable = ds == null || ds.Tables == null || ds.Tables.Count < 1 ? null : ds.Tables[0];

                list = new List<XTable>();
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["TABLE_NAME"].ToString() != "dtproperties" &&
                           dr["TABLE_NAME"].ToString() != "sysconstraints" &&
                               dr["TABLE_NAME"].ToString() != "syssegments" &&
                                (dr["TABLE_TYPE"].ToString() == "BASE TABLE" || dr["TABLE_TYPE"].ToString() == "VIEW"))
                        {
                            XTable xt = new XTable();
                            xt.ID = list.Count + 1;
                            xt.Name = dr["TABLE_NAME"].ToString();


                            DataRow[] drs = DescriptionTable == null ? null : DescriptionTable.Select("n='" + xt.Name + "'");
                            xt.Readme = drs == null || drs.Length < 1 ? "" : drs[0][1].ToString();

                            xt.IsView = dr["TABLE_TYPE"].ToString() == "VIEW";
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

        //private DataTable allfields;
        /// <summary>
        /// 取得指定表的所有列构架
        /// </summary>
        /// <param name="xt"></param>
        /// <returns></returns>
        protected override XField[] GetFields(XTable xt)
        {
            //if (allfields == null) allfields = Query(SchemaSql).Tables[0];
            DataTable allfields = Query(String.Format(SchemaSql, xt.Name)).Tables[0];
            if (allfields == null) return base.GetFields(xt);

            List<XField> list = new List<XField>();
            //DataRow[] drs = allfields.Select("表名='" + xt.Name + "'", "字段序号");
            //if (drs == null || drs.Length < 1) return base.GetFields(xt);
            if (allfields.Rows == null || allfields.Rows.Count < 1) return base.GetFields(xt);

            List<String> pks = GetPrimaryKeys(xt);
            List<Dictionary<String, String>> fks = GetForeignKeys(xt);
            foreach (DataRow dr in allfields.Rows)
            {
                XField xf = new XField();
                xf.ID = Int32.Parse(dr["字段序号"].ToString());
                xf.Name = dr["字段名"].ToString();
                xf.FieldType = FieldTypeToClassType(dr["类型"].ToString());
                xf.Identity = Boolean.Parse(dr["标识"].ToString());

                //xf.PrimaryKey = pks != null && pks.Contains(xf.Name);
                xf.PrimaryKey = Boolean.Parse(dr["主键"].ToString());
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

                xf.Length = Int32.Parse(dr["长度"].ToString());
                xf.NumOfByte = Int32.Parse(dr["占用字节数"].ToString());
                xf.Digit = Int32.Parse(dr["小数位数"].ToString());

                xf.Nullable = Boolean.Parse(dr["允许空"].ToString());
                xf.Default = dr["默认值"].ToString();
                xf.Readme = dr["字段说明"].ToString();
                list.Add(xf);
            }

            return list.ToArray();
        }

        /// <summary>
        /// 已重载。主键构架
        /// </summary>
        protected override DataTable PrimaryKeys
        {
            get
            {
                if (_PrimaryKeys == null) _PrimaryKeys = GetSchema("IndexColumns", new String[] { null, null, null });
                return _PrimaryKeys;
            }
        }

        public override string FieldTypeToClassType(String type)
        {
            switch (type)
            {
                case "text":
                case "uniqueidentifier":
                case "ntext":
                case "varchar":
                case "char":
                case "timestamp":
                case "nvarchar":
                case "nchar":
                    return "String";
                case "bit":
                    return "Boolean";
                case "tinyint":
                case "smallint":
                    return "Int16";
                case "int":
                case "numeric":
                    return "Int32";
                case "bigint":
                    return "Int64";
                case "decimal":
                case "money":
                case "smallmoney":
                    return "Decimal";
                case "smallldatetime":
                case "datetime":
                    return "DateTime";
                case "real":
                case "float":
                    return "Double";
                case "image":
                case "sql_variant":
                case "varbinary":
                case "binary":
                case "systemname":
                default:
                    return "String";
            }
            //if (type.Equals("Int32", StringComparison.OrdinalIgnoreCase)) return "Int32";
            //if (type.Equals("varchar", StringComparison.OrdinalIgnoreCase)) return "String";
            //if (type.Equals("text", StringComparison.OrdinalIgnoreCase)) return "String";
            //if (type.Equals("double", StringComparison.OrdinalIgnoreCase)) return "Double";
            //if (type.Equals("datetime", StringComparison.OrdinalIgnoreCase)) return "DateTime";
            //if (type.Equals("Int32", StringComparison.OrdinalIgnoreCase)) return "Int32";
            //if (type.Equals("Int32", StringComparison.OrdinalIgnoreCase)) return "Int32";
            //throw new Exception("Error");
        }

        #region 取得字段信息的SQL模版
        private String _SchemaSql = "";
        /// <summary>
        /// 构架SQL
        /// </summary>
        public virtual String SchemaSql
        {
            get
            {
                if (String.IsNullOrEmpty(_SchemaSql))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT ");
                    sb.Append("表名=d.name,");
                    sb.Append("字段序号=a.colorder,");
                    sb.Append("字段名=a.name,");
                    sb.Append("标识=case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then Convert(Bit,1) else Convert(Bit,0) end,");
                    sb.Append("主键=case when exists(SELECT 1 FROM sysobjects where xtype='PK' and name in (");
                    sb.Append("SELECT name FROM sysindexes WHERE indid in(");
                    sb.Append("SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid");
                    sb.Append("))) then Convert(Bit,1) else Convert(Bit,0) end,");
                    sb.Append("类型=b.name,");
                    sb.Append("占用字节数=a.length,");
                    sb.Append("长度=COLUMNPROPERTY(a.id,a.name,'PRECISION'),");
                    sb.Append("小数位数=isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),");
                    sb.Append("允许空=case when a.isnullable=1 then Convert(Bit,1)else Convert(Bit,0) end,");
                    sb.Append("默认值=isnull(e.text,''),");
                    sb.Append("字段说明=isnull(g.[value],'')");
                    sb.Append("FROM syscolumns a ");
                    sb.Append("left join systypes b on a.xtype=b.xusertype ");
                    sb.Append("inner join sysobjects d on a.id=d.id  and d.xtype='U' ");
                    sb.Append("left join syscomments e on a.cdefault=e.id ");
                    sb.Append("left join sysproperties g on a.id=g.id and a.colid=g.smallid  ");
                    sb.Append("where d.name='{0}' ");
                    sb.Append("order by a.id,a.colorder");
                    _SchemaSql = sb.ToString();
                }
                return _SchemaSql;
            }
        }

        private String _DescriptionSql = "select b.name n, a.value v from sysproperties a inner join sysobjects b on a.id=b.id where a.smallid=0";
        /// <summary>
        /// 取表说明SQL
        /// </summary>
        public virtual String DescriptionSql { get { return _DescriptionSql; } }
        #endregion
        #endregion
    }
}