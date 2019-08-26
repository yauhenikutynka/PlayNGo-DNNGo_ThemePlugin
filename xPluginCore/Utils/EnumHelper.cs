using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;

namespace DNNGo.Modules.ThemePlugin
{

    /// <summary>
    /// 枚举操作类
    /// </summary>
    public class EnumHelper
    {
        #region 取枚举数据


        /// <summary>
        /// 获取枚举的列表
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>枚举的键值集合</returns>
        public static List<EnumEntity> GetEnumList(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException();
            }

            List<EnumEntity> entitys = new List<EnumEntity>();

            Type typeDescription = typeof(TextAttribute);

            FieldInfo[] fields = enumType.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum == true)
                {
                    EnumEntity entity = new EnumEntity();

                    entity.Value = (int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);

                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        TextAttribute aa = (TextAttribute)arr[0];
                        entity.Text = aa.Text;
                    }
                    else
                    {
                        entity.Text = field.Name;
                    }
                    entitys.Add(entity); ;
                }
            }

            return entitys;
        }



        /// <summary>
        /// 过去枚举属性TEXT
        /// </summary>
        /// <param name="enumConst">枚举值</param>
        /// <param name="enumType">枚举类型 typeOf()</param>
        /// <returns></returns>
        public static string GetEnumTextVal(int enumConst, Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException();
            }

            string textVal = "";

            Type typeDescription = typeof(TextAttribute);
            FieldInfo fieldInfo = enumType.GetField(System.Enum.GetName(enumType, enumConst).ToString());

            if (fieldInfo != null)
            {
                object[] arr = fieldInfo.GetCustomAttributes(typeDescription, true);
                if (arr.Length > 0)
                {
                    TextAttribute textAttribute = (TextAttribute)arr[0];
                    textVal = textAttribute.Text;
                }
            }

            return textVal;
        }

        /// <summary>
        /// 枚举返回成数据表
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static DataTable GetEnumTable(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException();
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(System.String));
            dt.Columns.Add("Value", typeof(System.String));

            Type typeDescription = typeof(TextAttribute);

            FieldInfo[] fields = enumType.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum == true)
                {
                    DataRow dr = dt.NewRow();

                    dr["Value"] = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();

                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        TextAttribute aa = (TextAttribute)arr[0];
                        dr["Text"] = aa.Text;
                    }
                    else
                    {
                        dr["Text"] = field.Name;
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        /// <summary>
        /// 转换枚举为表
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static DataTable GetEnumTableForText(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException();
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(System.String));
            dt.Columns.Add("Value", typeof(System.String));

            FieldInfo[] fields = enumType.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum == true)
                {
                    DataRow dr = dt.NewRow();

                    dr["Value"] = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    dr["Text"] = enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null).ToString();
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }
        #endregion


    }
}
