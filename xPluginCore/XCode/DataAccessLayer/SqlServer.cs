using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// SqlServer���ݿ�
    /// </summary>
    internal class SqlServer : DataBase
    {
        #region ����
        /// <summary>
        /// ���캯�����Լ�����Conn����
        /// </summary>
        /// <param name="connStr"></param>
        public SqlServer(String connStr)
            : base("", SqlClientFactory.Instance)
        {
            connStr = Regex.Replace(connStr, "Provider[ ]*=.*?;", "", RegexOptions.IgnoreCase);
            connStr = Regex.Replace(connStr, "Provider[ ]*=.*?$", "", RegexOptions.IgnoreCase);
            Conn = new SqlConnection(connStr);
            // WinForm���򣬹ر��Զ��ر����ӹ��ܡ���Ϊһ���������ǵ����������ά�����ݿ��������Ż�����
            //if (System.Web.HttpContext.Current == null) IsAutoClose = false;
        }

        /// <summary>
        /// ���캯����ʹ���ⲿ����
        /// </summary>
        /// <param name="conn">����</param>
        public SqlServer(SqlConnection conn) : base(conn, SqlClientFactory.Instance) { }

        /// <summary>
        /// �������ݿ����͡��ⲿDAL���ݿ�����ʹ��Other
        /// </summary>
        public override DatabaseType DbType
        {
            get { return DatabaseType.SqlServer; }
        }
        #endregion

        #region ��ҳ
        /// <summary>
        /// ִ��SQL��ѯ�����ط�ҳ��¼��
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʼ</param>
        /// <param name="maximumRows">��󷵻�����</param>
        /// <param name="keyColumn">�����С�����not in��ҳ</param>
        /// <returns></returns>
        public override String PageSplit(string sql, Int32 startRowIndex, Int32 maximumRows, string keyColumn)
        {
            // �ӵ�һ�п�ʼ������Ҫ��ҳ
            if (startRowIndex <= 0 && maximumRows < 1) return sql;

            // ָ������ʼ�У�������SQL2005�����ϰ汾��ʹ��RowNumber�㷨
            //if (startRowIndex > 0 && IsSQL2005) return PageSplitRowNumber(sql, startRowIndex, maximumRows, keyColumn);

            // ���û��Order By��ֱ�ӵ��û��෽��
            // �����ַ����жϣ������ʸߣ�����������ߴ���Ч��
            if (!sql.Contains(" Order "))
            {
                if (!sql.ToLower().Contains(" order ")) return base.PageSplit(sql, startRowIndex, maximumRows, keyColumn);
            }
            //// ʹ����������ϸ��жϡ��������Order By���������ұ�û��������)��������order by���Ҳ����Ӳ�ѯ�ģ�����Ҫ���⴦��
            //MatchCollection ms = Regex.Matches(sql, @"\border\s*by\b([^)]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //if (ms == null || ms.Count < 1 || ms[0].Index < 1)
            String sql2 = sql;
            String orderBy = CheckOrderClause(ref sql2);
            if (String.IsNullOrEmpty(orderBy))
            {
                return base.PageSplit(sql, startRowIndex, maximumRows, keyColumn);
            }
            // ��ȷ����sql����㺬��order by���ټ��������Ƿ���top����Ϊû��top��order by�ǲ�������Ϊ�Ӳ�ѯ��
            if (Regex.IsMatch(sql, @"^[^(]+\btop\b", RegexOptions.Compiled | RegexOptions.IgnoreCase))
            {
                return base.PageSplit(sql, startRowIndex, maximumRows, keyColumn);
            }
            //String orderBy = sql.Substring(ms[0].Index);

            // �ӵ�һ�п�ʼ������Ҫ��ҳ
            if (startRowIndex <= 0)
            {
                if (maximumRows < 1)
                    return sql;
                else
                    return String.Format("Select Top {0} * From {1} {2}", maximumRows, CheckSimpleSQL(sql2), orderBy);
                //return String.Format("Select Top {0} * From {1} {2}", maximumRows, CheckSimpleSQL(sql.Substring(0, ms[0].Index)), orderBy);
            }

            #region Max/Min��ҳ
            // ���Ҫʹ��max/min��ҳ��������keyColumn������asc����desc
            String kc = keyColumn.ToLower();
            if (kc.EndsWith(" desc") || kc.EndsWith(" asc") || kc.EndsWith(" unknown"))
            {
                String str = PageSplitMaxMin(sql, startRowIndex, maximumRows, keyColumn);
                if (!String.IsNullOrEmpty(str)) return str;
                keyColumn = keyColumn.Substring(0, keyColumn.IndexOf(" "));
            }
            #endregion

            sql = CheckSimpleSQL(sql2);

            if (String.IsNullOrEmpty(keyColumn)) throw new ArgumentNullException("keyColumn", "��ҳҪ��ָ�������л��������ֶΣ�");

            if (maximumRows < 1)
                sql = String.Format("Select * From {1} Where {2} Not In(Select Top {0} {2} From {1} {3}) {3}", startRowIndex, sql, keyColumn, orderBy);
            else
                sql = String.Format("Select Top {0} * From {1} Where {2} Not In(Select Top {3} {2} From {1} {4}) {4}", maximumRows, sql, keyColumn, startRowIndex, orderBy);
            return sql;
        }
        /// <summary>
        /// ����д����ȡ��ҳ
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <param name="startRowIndex">��ʼ�У�0��ʾ��һ��</param>
        /// <param name="maximumRows">��󷵻�������0��ʾ������</param>
        /// <param name="keyColumn">�����С�����not in��ҳ</param>
        /// <returns></returns>
        public String PageSplitRowNumber(String sql, Int32 startRowIndex, Int32 maximumRows, String keyColumn)
        {
            // �ӵ�һ�п�ʼ������Ҫ��ҳ
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
                // ʹ����������ϸ��жϡ��������Order By���������ұ�û��������)��������order by���Ҳ����Ӳ�ѯ�ģ�����Ҫ���⴦��
                //MatchCollection ms = Regex.Matches(sql, @"\border\s*by\b([^)]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                //if (ms != null && ms.Count > 0 && ms[0].Index > 0)
                String sql2 = sql;
                String orderBy2 = CheckOrderClause(ref sql2);
                if (String.IsNullOrEmpty(orderBy))
                {
                    // ��ȷ����sql����㺬��order by���ټ��������Ƿ���top����Ϊû��top��order by�ǲ�������Ϊ�Ӳ�ѯ��
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
                if (String.IsNullOrEmpty(keyColumn)) throw new ArgumentNullException("keyColumn", "��ҳҪ��ָ�������л��������ֶΣ�");

                //if (keyColumn.EndsWith(" Unknown", StringComparison.OrdinalIgnoreCase)) keyColumn = keyColumn.Substring(0, keyColumn.LastIndexOf(" "));
                orderBy = "Order By " + keyColumn;
            }
            sql = CheckSimpleSQL(sql);

            //row_number()��1��ʼ
            if (maximumRows < 1)
                sql = String.Format("Select * From (Select *, row_number() over({2}) as rowNumber From {1}) XCode_Temp_b Where rowNumber>={0}", startRowIndex + 1, sql, orderBy);
            else
                sql = String.Format("Select * From (Select *, row_number() over({3}) as rowNumber From {1}) XCode_Temp_b Where rowNumber Between {0} And {2}", startRowIndex + 1, sql, startRowIndex + maximumRows, orderBy);

            return sql;
        }
        #endregion

        #region ����
        /// <summary>
        /// ȡ�����б���
        /// </summary>
        /// <returns></returns>
        public override XTable[] GetTables()
        {
            List<XTable> list = null;
            try
            {
                DataTable dt = GetSchema("Tables", null);

                //һ���԰����еı�˵�������
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
        /// ȡ��ָ����������й���
        /// </summary>
        /// <param name="xt"></param>
        /// <returns></returns>
        protected override XField[] GetFields(XTable xt)
        {
            //if (allfields == null) allfields = Query(SchemaSql).Tables[0];
            DataTable allfields = Query(String.Format(SchemaSql, xt.Name)).Tables[0];
            if (allfields == null) return base.GetFields(xt);

            List<XField> list = new List<XField>();
            //DataRow[] drs = allfields.Select("����='" + xt.Name + "'", "�ֶ����");
            //if (drs == null || drs.Length < 1) return base.GetFields(xt);
            if (allfields.Rows == null || allfields.Rows.Count < 1) return base.GetFields(xt);

            List<String> pks = GetPrimaryKeys(xt);
            List<Dictionary<String, String>> fks = GetForeignKeys(xt);
            foreach (DataRow dr in allfields.Rows)
            {
                XField xf = new XField();
                xf.ID = Int32.Parse(dr["�ֶ����"].ToString());
                xf.Name = dr["�ֶ���"].ToString();
                xf.FieldType = FieldTypeToClassType(dr["����"].ToString());
                xf.Identity = Boolean.Parse(dr["��ʶ"].ToString());

                //xf.PrimaryKey = pks != null && pks.Contains(xf.Name);
                xf.PrimaryKey = Boolean.Parse(dr["����"].ToString());
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

                xf.Length = Int32.Parse(dr["����"].ToString());
                xf.NumOfByte = Int32.Parse(dr["ռ���ֽ���"].ToString());
                xf.Digit = Int32.Parse(dr["С��λ��"].ToString());

                xf.Nullable = Boolean.Parse(dr["�����"].ToString());
                xf.Default = dr["Ĭ��ֵ"].ToString();
                xf.Readme = dr["�ֶ�˵��"].ToString();
                list.Add(xf);
            }

            return list.ToArray();
        }

        /// <summary>
        /// �����ء���������
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

        #region ȡ���ֶ���Ϣ��SQLģ��
        private String _SchemaSql = "";
        /// <summary>
        /// ����SQL
        /// </summary>
        public virtual String SchemaSql
        {
            get
            {
                if (String.IsNullOrEmpty(_SchemaSql))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT ");
                    sb.Append("����=d.name,");
                    sb.Append("�ֶ����=a.colorder,");
                    sb.Append("�ֶ���=a.name,");
                    sb.Append("��ʶ=case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then Convert(Bit,1) else Convert(Bit,0) end,");
                    sb.Append("����=case when exists(SELECT 1 FROM sysobjects where xtype='PK' and name in (");
                    sb.Append("SELECT name FROM sysindexes WHERE indid in(");
                    sb.Append("SELECT indid FROM sysindexkeys WHERE id = a.id AND colid=a.colid");
                    sb.Append("))) then Convert(Bit,1) else Convert(Bit,0) end,");
                    sb.Append("����=b.name,");
                    sb.Append("ռ���ֽ���=a.length,");
                    sb.Append("����=COLUMNPROPERTY(a.id,a.name,'PRECISION'),");
                    sb.Append("С��λ��=isnull(COLUMNPROPERTY(a.id,a.name,'Scale'),0),");
                    sb.Append("�����=case when a.isnullable=1 then Convert(Bit,1)else Convert(Bit,0) end,");
                    sb.Append("Ĭ��ֵ=isnull(e.text,''),");
                    sb.Append("�ֶ�˵��=isnull(g.[value],'')");
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
        /// ȡ��˵��SQL
        /// </summary>
        public virtual String DescriptionSql { get { return _DescriptionSql; } }
        #endregion
        #endregion
    }
}