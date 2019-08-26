using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Threading;
 

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 数据库构架
    /// </summary>
    public class DatabaseSchema
    {
        #region 属性
        #region 基础属性
        private String _ConnName;
        /// <summary>链接名</summary>
        public String ConnName
        {
            get { return _ConnName; }
            set { _ConnName = value; }
        }

        private DAL _Database;
        /// <summary>数据库</summary>
        public DAL Database
        {
            get
            {
                //延迟加载，这里时候可能已经在别的线程里面了
                if (_Database == null) _Database = DAL.Create(ConnName);
                return _Database;
            }
            set { _Database = value; }
        }

        private Type _EntityType;
        /// <summary>实体类型</summary>
        public Type EntityType
        {
            get { return _EntityType; }
            set { _EntityType = value; }
        }
        #endregion

        #region 扩展属性
        /// <summary>
        /// 表
        /// </summary>
        public BindTableAttribute Table { get { return xConfig.Table(EntityType); } }

        private List<FieldItem> _Fields;
        /// <summary>
        /// 字段集合
        /// </summary>
        internal List<FieldItem> Fields
        {
            get
            {
                if (_Fields == null)
                {
                    List<FieldItem> list = new List<FieldItem>(xConfig.Fields(EntityType));
                    list.Sort(delegate(FieldItem item1, FieldItem item2) { return (item1.Column != null && item2.Column != null ? item1.Column.Order.CompareTo(item2.Column.Order) : 0) * 10 + item1.Property.Name.CompareTo(item2.Property.Name); });
                    _Fields = list;
                }
                return _Fields;
            }
        }

        /// <summary>
        /// 表名
        /// </summary>
        public String TableName { get { return xConfig.TableName(EntityType); } }
        #endregion
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造一个数据库构架对象
        /// </summary>
        /// <param name="connName"></param>
        /// <param name="type"></param>
        public DatabaseSchema(String connName, Type type)
        {
            ConnName = connName;
            EntityType = type;
        }
        #endregion

        #region 生成SQL
        /// <summary>
        /// 创建数据表的SQL
        /// </summary>
        /// <returns></returns>
        public String CreateTable()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("CREATE TABLE [dbo].[{0}](", TableName);
            //foreach (FieldItem item in Fields)
            //{
            //    sb.AppendLine("\t");
            //    sb.Append(FieldClause(item));
            //    sb.Append(",");
            //}
            List<String> keys = new List<string>();
            for (Int32 i = 0; i < Fields.Count; i++)
            {
                sb.AppendLine();
                sb.Append("\t");
                sb.Append(FieldClause(Fields[i], true));
                if (i < Fields.Count - 1) sb.Append(",");

                if (Fields[i].DataObjectField != null && Fields[i].DataObjectField.PrimaryKey)
                {
                    if (Fields[i].Column != null && !String.IsNullOrEmpty(Fields[i].Column.Name))
                        keys.Add(Fields[i].Column.Name);
                    else
                        keys.Add(Fields[i].Property.Name);
                }
            }
            //主键
            if (keys.Count > 0)
            {
                sb.Append(",");
                sb.AppendLine();
                sb.Append("\t");
                sb.AppendFormat("CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED", TableName);
                sb.AppendLine();
                sb.Append("\t");
                sb.Append("(");
                for (Int32 i = 0; i < keys.Count; i++)
                {
                    sb.AppendLine();
                    sb.Append("\t\t");
                    sb.AppendFormat("[{0}] ASC", keys[i]);
                    if (i < keys.Count - 1) sb.Append(",");
                }
                sb.AppendLine();
                sb.Append("\t");
                sb.Append(")WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]");
            }

            sb.AppendLine();
            sb.Append(") ON [PRIMARY]");

            //表注释
            if (Table != null && !String.IsNullOrEmpty(Table.Description))
            {
                sb.AppendLine(";");
                sb.AppendLine();
                //sb.AppendFormat("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{1}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}'", TableName, Table.Description);
                sb.Append(CreateDescriptionSQLForTable(TableName, Table.Description));
            }

            //字段注释
            foreach (FieldItem item in Fields)
            {
                if (item.Column == null || String.IsNullOrEmpty(item.Column.Description)) continue;
                sb.AppendLine(";");
                //sb.AppendFormat("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{1}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'{2}'", TableName, item.Column.Description, item.Column.Name);
                sb.Append(CreateDescriptionSQLForColumn(TableName, item.Column.Name, item.Column.Description));
            }

            ////字段默认值
            //foreach (FieldItem item in Fields)
            //{
            //    if (item.Column == null || String.IsNullOrEmpty(item.Column.DefaultValue)) continue;
            //    sb.AppendLine();
            //    sb.AppendLine(";");
            //    String v = item.Column.DefaultValue;
            //    if (item.Property.PropertyType == typeof(String))
            //        v = String.Format("N'{0}'", v);
            //    else if (item.Property.PropertyType == typeof(DateTime))
            //    {
            //        if (v.StartsWith("(")) v = v.Substring(1, v.Length - 2);
            //    }
            //    sb.AppendFormat("ALTER TABLE dbo.{0} ADD CONSTRAINT DF_{0}_{1} DEFAULT {2} FOR {1}", TableName, item.Column.Name, v);
            //}

            return sb.ToString();
        }

        /// <summary>
        /// 字段片段
        /// </summary>
        /// <param name="field"></param>
        /// <param name="includeDefault">是否包含默认值</param>
        /// <returns></returns>
        private String FieldClause(FieldItem field, Boolean includeDefault)
        {
            StringBuilder sb = new StringBuilder();

            //字段名
            String name = field.Property.Name;
            if (field.Column != null) name = field.Column.Name;
            sb.AppendFormat("[{0}] ", name);

            //类型
            Type type = field.Property.PropertyType;
            if (type == typeof(Int32))
            {
                sb.Append("[int] ");
                if (field.DataObjectField != null && field.DataObjectField.IsIdentity) sb.Append("IDENTITY(1,1) ");
            }
            else if (type == typeof(Int16))
            {
                sb.Append("[smallint] ");
                if (field.DataObjectField != null && field.DataObjectField.IsIdentity) sb.Append("IDENTITY(1,1) ");
            }
            else if (type == typeof(Int64))
            {
                sb.Append("[bigint] ");
                if (field.DataObjectField != null && field.DataObjectField.IsIdentity) sb.Append("IDENTITY(1,1) ");
            }
            else if (type == typeof(Double))
            {
                sb.Append("[float] ");
            }
            else if (type == typeof(DateTime))
            {
                sb.Append("[datetime] ");
            }
            else if (type == typeof(Boolean))
            {
                sb.Append("[bit] ");
            }
            else if (type == typeof(Decimal))
            {
                sb.Append("[money] ");
            }
            else if (type == typeof(String))
            {
                Int32 len = 50;
                if (field.DataObjectField != null) len = field.DataObjectField.Length;
                if (len < 1) len = 50;
                if (len > 4000)
                    sb.Append("[ntext] ");
                else
                    sb.AppendFormat("[nvarchar]({0}) ", len);
            }

            //是否为空
            if (field.DataObjectField != null && field.DataObjectField.IsNullable)
                sb.Append("NULL");
            else
                sb.Append("NOT NULL");

            //默认值
            if (includeDefault && field.Column != null && !String.IsNullOrEmpty(field.Column.DefaultValue))
            {
                if (field.Property.PropertyType == typeof(String))
                    sb.AppendFormat(" DEFAULT ('{0}')", field.Column.DefaultValue);
                else
                    sb.AppendFormat(" DEFAULT {0}", field.Column.DefaultValue);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 修改表
        /// </summary>
        /// <returns></returns>
        public String AlterTable()
        {
            DataTable dt = Prepare();
            StringBuilder sb = new StringBuilder();

            #region 新增列
            foreach (FieldItem item in Fields)
            {
                String name = item.Property.Name;
                if (item.Column != null) name = item.Column.Name;

                if (!dt.Columns.Contains(name))
                {
                    if (sb.Length > 0) sb.AppendLine(";");
                    sb.AppendFormat("ALTER TABLE [{0}] ADD {1}", TableName, FieldClause(item, false));
                    if (!String.IsNullOrEmpty(item.Column.DefaultValue))
                    {
                        String v = item.Column.DefaultValue;
                        if (item.Property.PropertyType == typeof(String))
                            v = String.Format("N'{0}'", v);
                        else if (item.Property.PropertyType == typeof(DateTime))
                        {
                            if (v.StartsWith("(")) v = v.Substring(1, v.Length - 2);
                        }
                        if (!String.IsNullOrEmpty(v))
                        {
                            //if (sb.Length > 0) sb.AppendLine(";");
                            //sb.AppendFormat("ALTER TABLE dbo.{0} ADD CONSTRAINT DF_{0}_{1} DEFAULT {2} FOR {1}", TableName, item.Column.Name, v);
                            sb.AppendFormat(" Default {0}", v);
                        }
                    }
                    if (item.Column != null && !String.IsNullOrEmpty(item.Column.Description))
                    {
                        if (sb.Length > 0) sb.AppendLine(";");
                        //sb.AppendFormat("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{1}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'{2}'", TableName, item.Column.Description, item.Column.Name);
                        sb.Append(CreateDescriptionSQLForColumn(TableName, item.Column.Name, item.Column.Description));
                    }
                }
            }
            #endregion

            #region 删除列
            StringBuilder sb2 = new StringBuilder();
            Dictionary<String, FieldItem> names = new Dictionary<String, FieldItem>();
            Fields.ForEach(delegate(FieldItem item) { names.Add(item.Column != null && !String.IsNullOrEmpty(item.Column.Name) ? item.Column.Name : item.Property.Name, item); });
            foreach (DataColumn item in dt.Columns)
            {
                if (!names.ContainsKey(item.ColumnName))
                {
                    //如果原来就有默认值，需要先删除
                    if (!String.IsNullOrEmpty(item.Caption))
                    {
                        //if (sb2.Length > 0) sb2.AppendLine(";");
                        //sb2.AppendFormat("if exists( select * from sysobjects where name='DF_{0}_{1}') ALTER TABLE dbo.{0} DROP CONSTRAINT DF_{0}_{1}", TableName, item.ColumnName);
                        String sql = DeleteConstraintsSQL(TableName, item.ColumnName, null);
                        if (!String.IsNullOrEmpty(sql))
                        {
                            if (sb2.Length > 0) sb2.AppendLine(";");
                            sb2.Append(sql);
                        }
                    }
                    if (sb2.Length > 0) sb2.AppendLine(";");
                    sb2.AppendFormat("ALTER TABLE [{0}] DROP COLUMN {1}", TableName, item.ColumnName);
                }
            }
            if (sb2.Length > 0)
            {
                if (NoDelete)
                {
                    //不许删除列，显示日志
                    XTrace.WriteLine("数据表中发现有多余字段，DatabaseSchema_NoDelete被设置为True，请手工执行以下语句删除：" + Environment.NewLine + sb2.ToString());
                }
                else
                {
                    if (sb.Length > 0) sb.AppendLine(";");
                    sb.Append(sb2.ToString());
                }
            }
            #endregion

            #region 修改列
            foreach (FieldItem item in Fields)
            {
                String name = item.Property.Name;
                if (item.Column != null) name = item.Column.Name;

                if (dt.Columns.Contains(name))
                {
                    DataColumn dc = dt.Columns[name];
                    Boolean needUpdate = false;

                    #region 字段比对
                    //类型
                    if (item.Property.PropertyType != dc.DataType && !(item.Property.PropertyType == typeof(String) && dc.DataType == typeof(Guid)))
                        needUpdate = true;
                    else if (item.DataObjectField != null)
                    {
                        //字符串长度
                        if (item.Property.PropertyType == typeof(String) && dc.DataType != typeof(Guid) && dc.MaxLength != item.DataObjectField.Length)
                            needUpdate = true;
                        else if (item.DataObjectField.IsNullable != dc.AllowDBNull)//允许空
                            needUpdate = true;
                        else if (item.DataObjectField.IsIdentity != dc.AutoIncrement)//自增
                            needUpdate = true;
                    }
                    #endregion

                    if (needUpdate)
                    {
                        //默认值。
                        if (item.Column != null && !String.IsNullOrEmpty(item.Column.DefaultValue) &&
                            item.Column.DefaultValue != dc.Caption &&
                            dc.Caption != String.Format("('{0}')", item.Column.DefaultValue) &&
                            dc.Caption != String.Format("(N'{0}')", item.Column.DefaultValue))
                        {
                            String v = item.Column.DefaultValue;
                            if (item.Property.PropertyType == typeof(String))
                                v = String.Format("N'{0}'", v);
                            else if (item.Property.PropertyType == typeof(DateTime))
                            {
                                if (v.StartsWith("(")) v = v.Substring(1, v.Length - 2);
                            }
                            if (!String.IsNullOrEmpty(v))
                            {
                                //如果原来就有默认值，需要先删除
                                //if (!String.IsNullOrEmpty(dc.Caption))
                                //{
                                //    if (sb.Length > 0) sb.AppendLine(";");
                                //    sb.AppendFormat("if exists( select * from sysobjects where name='DF_{0}_{1}') ALTER TABLE dbo.{0} DROP CONSTRAINT DF_{0}_{1}", TableName, item.Column.Name);
                                //}
                                //if (sb.Length > 0) sb.AppendLine(";");
                                //sb.AppendFormat("ALTER TABLE dbo.{0} ADD CONSTRAINT DF_{0}_{1} DEFAULT {2} FOR {1}", TableName, item.Column.Name, v);
                                String sql = DeleteConstraintsSQL(TableName, item.Column.Name, "D");
                                if (!String.IsNullOrEmpty(sql))
                                {
                                    sb.Append(sql);
                                    sb.AppendLine(";");
                                }
                                sb.Append(CreateDefaultSQL(TableName, item.Column.Name, v));
                            }
                        }

                        if (sb.Length > 0) sb.AppendLine(";");
                        sb.AppendFormat("ALTER TABLE [{0}] ALTER COLUMN {1}", TableName, FieldClause(item, false));

                        sb.AppendLine(";");
                        if (item.Column != null && !String.IsNullOrEmpty(item.Column.Description))
                        {
                            //sb.AppendFormat("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{1}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'{2}'", TableName, item.Column.Description, item.Column.Name);
                            String sql = DeleteDescriptionSQL(TableName, item.Column.Name);
                            if (!String.IsNullOrEmpty(sql))
                            {
                                sb.Append(sql);
                                sb.AppendLine(";");
                            }
                            sb.Append(CreateDescriptionSQLForColumn(TableName, item.Column.Name, item.Column.Description));
                        }
                    }
                }
            }
            #endregion

            #region 表说明
            String remark = String.Empty;
            if (Table != null) remark = Table.Description;
            if (!String.IsNullOrEmpty(remark))
            {
                String sql = String.Format("select b.name n, a.value v from sys.extended_properties a inner join sysobjects b on a.major_id=b.id and a.minor_id=0 and a.name = 'MS_Description' and b.name='{0}'", TableName);
                DataSet ds = Database.Select(sql, "");
                if (ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                {
                    String v = ds.Tables[0].Rows[0][1].ToString();
                    if (!String.IsNullOrEmpty(v) && v != remark)
                    {
                        if (sb.Length > 0) sb.AppendLine(";");
                        //sb.AppendFormat("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{1}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}'", TableName, Table.Description);
                        sb.Append(CreateDescriptionSQLForColumn(TableName, null, Table.Description));
                        //sb.AppendLine();
                    }
                }
            }
            #endregion

            return sb.ToString();
        }

        /// <summary>
        /// 创建表说明的SQL
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        private String CreateDescriptionSQLForTable(String tablename, String description)
        {
            if (String.IsNullOrEmpty(tablename) || String.IsNullOrEmpty(description)) return null;

            //表说明
            return String.Format("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{1}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}'", tablename, description);
        }

        /// <summary>
        /// 创建字段说明的SQL
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="columnname"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        private String CreateDescriptionSQLForColumn(String tablename, String columnname, String description)
        {
            if (String.IsNullOrEmpty(tablename) || String.IsNullOrEmpty(columnname) || String.IsNullOrEmpty(description)) return null;

            //字段说明
            return String.Format("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{1}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'{2}'", tablename, description, columnname);
        }

        /// <summary>
        /// 删除说明的SQL
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="columnname"></param>
        /// <returns></returns>
        private String DeleteDescriptionSQL(String tablename, String columnname)
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(columnname))
            {
                //字段说明
                sb.AppendFormat("if exists( select * from sys.tables a inner join sys.extended_properties b on a.object_id=b.major_id inner join sys.columns c on a.object_id=c.object_id and b.minor_id=c.column_id where b.name='MS_Description' and a.name='{0}' and c.name='{1}')", tablename, columnname);
                sb.AppendFormat("EXEC sys.sp_dropextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}', @level2type=N'COLUMN',@level2name=N'{1}'", tablename, columnname);
            }
            else
            {
                //表说明
                sb.AppendFormat("if exists( select * from sys.tables a inner join sys.extended_properties b on a.object_id=b.major_id where b.name='MS_Description' and b.minor_id=0 and a.name='{0}')", tablename);
                sb.AppendFormat("EXEC sys.sp_dropextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{0}'", tablename);
            }
            return sb.ToString();
        }

        private String CreateDefaultSQL(String tablename, String columnname, String value)
        {
            return String.Format("ALTER TABLE dbo.{0} ADD CONSTRAINT DF_{0}_{1} DEFAULT {2} FOR {1}", tablename, columnname, value);
        }

        /// <summary>
        /// 删除约束脚本。
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="columnname"></param>
        /// <param name="type">约束类型，默认值是D，如果未指定，则删除所有约束</param>
        /// <returns></returns>
        private String DeleteConstraintsSQL(String tablename, String columnname, String type)
        {
            //StringBuilder sb = new StringBuilder();
            //sb.AppendFormat("if exists( select * from sys.tables a inner join sys.default_constraints b on a.object_id=b.parent_object_id inner join sys.columns c on a.object_id=c.object_id and b.parent_column_id=c.column_id where a.name='{0}' and c.name='{1}'", tablename, columnname);
            //if (!String.IsNullOrEmpty(type)) sb.AppendFormat(" and type='{0}'", type);
            //sb.Append(")");
            //return sb.ToString();
            String sql = String.Format("select b.name from sys.tables a inner join sys.default_constraints b on a.object_id=b.parent_object_id inner join sys.columns c on a.object_id=c.object_id and b.parent_column_id=c.column_id where a.name='{0}' and c.name='{1}'", tablename, columnname);
            if (!String.IsNullOrEmpty(type)) sql += String.Format(" and b.type='{0}'", type);
            DataSet ds = Database.Select(sql, "");
            if (ds == null || ds.Tables == null || ds.Tables[0].Rows.Count < 1) return null;

            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                String name = dr[0].ToString();
                if (sb.Length > 0) sb.AppendLine(";");
                sb.AppendFormat("ALTER TABLE {0} DROP CONSTRAINT {1}", tablename, name);
            }
            return sb.ToString();
        }
        #endregion

        #region 准备构架
        private DataTable Prepare()
        {
            DataSet ds = Database.Select(String.Format("Select top 1 * From [{0}]", TableName), TableName);
            DataTable dt = ds.Tables[0];

            DataTable dts = Database.Select(SchemaSql, "").Tables[0];
            if (dts.Rows != null && dts.Rows.Count > 0)
            {
                foreach (DataRow dr in dts.Rows)
                {
                    String name = dr["字段名"].ToString();
                    DataColumn dc = dt.Columns[name];
                    dc.AutoIncrement = Boolean.Parse(dr["标识"].ToString());
                    dc.AllowDBNull = Boolean.Parse(dr["允许空"].ToString());
                    if (dc.DataType == typeof(String)) dc.MaxLength = Int32.Parse(dr["长度"].ToString());
                    dc.Caption = null;
                    if (!dc.AutoIncrement)
                    {
                        String v = dr["默认值"].ToString();
                        if (!String.IsNullOrEmpty(v)) dc.Caption = v;
                    }
                }
            }

            return dt;
        }

        private String _SchemaSql = "";
        /// <summary>
        /// 构架SQL
        /// </summary>
        private String SchemaSql
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
                    sb.Append("left join sys.extended_properties g on a.id=g.major_id and a.colid=g.minor_id and g.name = 'MS_Description'  ");
                    sb.AppendFormat(" Where d.name='{0}' ", TableName);
                    sb.Append("order by a.id,a.colorder");
                    _SchemaSql = sb.ToString();
                }
                return _SchemaSql;
            }
        }
        #endregion

        #region 检查表是否存在
        /// <summary>
        /// 检查表是否存在
        /// </summary>
        /// <returns></returns>
        public Boolean TableExists()
        {
            String sql = String.Format("select * from sysobjects where xtype='U' and name='{0}'", TableName);
            DataSet ds = Database.Select(sql, "");

            return ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0;
        }
        #endregion

        #region 业务
        /// <summary>
        /// 开始检查
        /// </summary>
        public void BeginCheck()
        {
            if (Exclude.Count > 0)
            {
                //检查是否被排除的链接
                if (Exclude.Exists(delegate(String item) { return String.Equals(item, ConnName); })) return;
            }

            ThreadPool.QueueUserWorkItem(new WaitCallback(Check));
            //Check(null);
        }

        private static List<Type> dones = new List<Type>();
        private void Check(Object state)
        {
            if (dones.Contains(EntityType)) return;
            lock (EntityType)
            {
                if (dones.Contains(EntityType)) return;
                dones.Add(EntityType);

                try
                {
                    //检查是否SQL2005，因为本功能仅支持SQL2005
                    DataSet ds = Database.Select("Select @@Version", "");
                    if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows == null || ds.Tables[0].Rows.Count < 1) return;

                    String v = ds.Tables[0].Rows[0][0].ToString();
                    if (String.IsNullOrEmpty(v)) return;

                    if (!v.Contains("2005")) return;

                    String sql = String.Empty;
                    if (!TableExists())
                    {
                        sql = CreateTable();
                        if (Enable != null && Enable.Value)
                        {
                            XTrace.WriteLine("创建表：" + Environment.NewLine + sql);
                            Database.Execute(sql, "");
                        }
                        else
                            XTrace.WriteLine("DatabaseSchema_Enable没有设置为True，请手工使用以下语句创建表：" + Environment.NewLine + sql);
                    }
                    else
                    {
                        sql = AlterTable();
                        if (!String.IsNullOrEmpty(sql))
                        {
                            if (Enable != null && Enable.Value)
                            {
                                XTrace.WriteLine("修改表：" + Environment.NewLine + sql);
                                //Database.Execute(sql, "");
                                //拆分成多条执行
                                String[] sqls = sql.Split(';');
                                foreach (String item in sqls)
                                {
                                    try
                                    {
                                        Database.Execute(item, "");
                                    }
                                    catch { }
                                }
                            }
                            else
                                XTrace.WriteLine("DatabaseSchema_Enable没有设置为True，请手工使用以下语句修改表：" + Environment.NewLine + sql);
                        }
                    }
                }
                catch (Exception ex)
                {
                    XTrace.WriteLine("检查构架信息错误！" + ex.ToString());
                }
            }
        }

        private static Boolean? _Enable;
        /// <summary>
        /// 是否启用数据架构
        /// </summary>
        public static Boolean? Enable
        {
            get
            {
                if (_Enable != null) return _Enable.Value;

                String str = AppConfig.DatabaseSchema_Enable;
                if (String.IsNullOrEmpty(str)) return null;
                if (str == "1" || str.Equals(Boolean.TrueString, StringComparison.OrdinalIgnoreCase))
                    _Enable = true;
                else if (str == "0" || str.Equals(Boolean.FalseString, StringComparison.OrdinalIgnoreCase))
                    _Enable = false;
                else
                    _Enable = Convert.ToBoolean(str);
                return _Enable.Value;
            }
            set { _Enable = value; }
        }

        private static Boolean? _NoDelete;
        /// <summary>
        /// 是否启用不删除字段
        /// </summary>
        public static Boolean NoDelete
        {
            get
            {
                if (_NoDelete != null) return _Enable.Value;

                String str = AppConfig.DatabaseSchema_NoDelete;
                if (String.IsNullOrEmpty(str)) return false;
                if (str == "1" || str.Equals(Boolean.TrueString, StringComparison.OrdinalIgnoreCase)) return true;
                if (str == "0" || str.Equals(Boolean.FalseString, StringComparison.OrdinalIgnoreCase)) return false;
                _NoDelete = Convert.ToBoolean(str);
                return _NoDelete.Value;
            }
            set { _NoDelete = value; }
        }

        private static List<String> _Exclude;
        /// <summary>
        /// 要排除的链接名
        /// </summary>
        public static List<String> Exclude
        {
            get
            {
                if (_Exclude != null) return _Exclude;

                String str =AppConfig.DatabaseSchema_Exclude;
                if (String.IsNullOrEmpty(str))
                    _Exclude = new List<String>();
                else
                    _Exclude = new List<String>(str.Split(new Char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries));

                return _Exclude;
            }
        }
        #endregion
    }
}