using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 分页存储过程查询参数类
    /// </summary>
    [Serializable]
    public class QueryParam
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public QueryParam()
            : this(1, int.MaxValue)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_mPageIndex">当前页码</param>
        /// <param name="_mPageSize">每页记录数</param>
        public QueryParam(int _mPageIndex, int _mPageSize)
        {
            _PageIndex = _mPageIndex;
            _PageSize = _mPageSize;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_mOrderType">排序类型 1:降序 其它为升序</param>
        /// <param name="_mPageIndex">当前页码</param>
        /// <param name="_mPageSize">每页记录数</param>
        public QueryParam(int _mOrderType, int _mPageIndex, int _mPageSize)
        {
            _OrderType = _mOrderType;
            _PageIndex = _mPageIndex;
            _PageSize = _mPageSize;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_mWhere">查询条件 需带Where</param>
        /// <param name="_mOrderType">排序类型 1:降序 其它为升序</param>
        /// <param name="_mPageIndex">当前页码</param>
        /// <param name="_mPageSize">每页记录数</param>
        public QueryParam(List<SearchParam> _mWhere, int _mOrderType,
            int _mPageIndex, int _mPageSize)
        {
            _Where = _mWhere;
            _OrderType = _mOrderType;
            _PageIndex = _mPageIndex;
            _PageSize = _mPageSize;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_mWhere">查询条件 需带Where</param>
        /// <param name="_mOrderfld">排序字段</param>
        /// <param name="_mOrderType">排序类型 1:降序 其它为升序</param>
        /// <param name="_mPageIndex">当前页码</param>
        /// <param name="_mPageSize">每页记录数</param>
        public QueryParam(List<SearchParam> _mWhere, string _mOrderfld, int _mOrderType,
            int _mPageIndex, int _mPageSize)
        {
            _Where = _mWhere;
            _Orderfld = _mOrderfld;
            _OrderType = _mOrderType;
            _PageIndex = _mPageIndex;
            _PageSize = _mPageSize;
        }







        #region "Private Variables"
        private string _ReturnFields = "*";
        private List<SearchParam> _Where = new List<SearchParam>();
        private string _Orderfld = string.Empty;
        private int _OrderType = 1;
        private int _PageIndex = 1;
        private int _PageSize = int.MaxValue;
        private int _startRowIndex = 0;
        private int _RecordCount = int.MaxValue;
        private String _TableName = String.Empty;
        #endregion

        #region "Public Variables"
        /// <summary>
        /// 返回字段
        /// </summary>
        public string ReturnFields
        {
            get
            {
                return _ReturnFields;
            }
            set
            {
                _ReturnFields = value;
            }
        }
        /// <summary>
        /// 查询条件 需带Where
        /// </summary>
        public List<SearchParam> Where
        {
            get
            {
                return _Where;
            }
            set
            {
                _Where = value;
            }
        }





        /// <summary>
        /// 排序字段
        /// </summary>
        public string Orderfld
        {
            get
            {
                return _Orderfld;
            }
            set
            {
                _Orderfld = value;
            }
        }


        /// <summary>
        /// 排序类型 1:降序 其它为升序
        /// </summary>
        public int OrderType
        {
            get
            {
                return _OrderType;
            }
            set
            {
                _OrderType = value;
            }
        }


        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex
        {
            get
            {
                return _PageIndex;
            }
            set
            {
                _PageIndex = value;
            }

        }


        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize
        {
            get
            {
                return _PageSize;
            }
            set
            {
                _PageSize = value;
            }
        }

        /// <summary>
        /// 开始行数
        /// </summary>
        public int startRowIndex
        {
            get
            {
                //第一页时,行集索引从零开始 || 实际的行数不够分一页时
                if (_PageIndex == 1 || _RecordCount <= _PageSize) return _startRowIndex;

                //计算出总页数
                int MaxPage = _RecordCount / _PageSize + (_RecordCount % _PageSize > 0 ? 1 : 0);

                //页数不能超过实际最大页面的判断
                if (_PageIndex > MaxPage) _PageIndex = MaxPage;

                //起始值
                return (_PageIndex - 1) * _PageSize;
            }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int Pages
        {
            get
            {
                //总数量不足以1页时
                if (RecordCount < PageSize) return 1;
                return RecordCount / PageSize + (RecordCount % PageSize > 0 ? 1 : 0);
            }
        }

        /// <summary>
        /// 结果数量
        /// </summary>
        public int RecordCount
        {
            get
            {
                return _RecordCount;
            }
            set
            {
                _RecordCount = value;
            }
        }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get
            {
                return _TableName;
            }
            set
            {
                _TableName = value;
            }
        }



        private StringBuilder _WhereSql = new StringBuilder();
        /// <summary>
        /// 查询条件,封装OR的时候用
        /// </summary>
        public StringBuilder WhereSql
        {
            get { return _WhereSql; }
            set { _WhereSql = value; }
        }



        /// <summary>
        /// 转换成Sql语句
        /// </summary>
        /// <returns>Sql语句</returns>
        public string ToSql()
        {

            //构造查询条件
            StringBuilder sb = new StringBuilder();

            if (!String.IsNullOrEmpty(WhereSql.ToString()))
            {
                sb.Append(WhereSql);
            }


            foreach (SearchParam sp in Where)
            {
                if (sb.Length > 0) sb.Append(" AND ");

                sb.Append(sp.ToSql());
            }
            return sb.ToString();

        }
        #endregion
    }

    /// <summary>
    /// 查询参数
    /// </summary>
    [Serializable]
    public class SearchParam
    {

        #region "Private Variables"
        private string _FieldName;
        private object _FieldValue;
        private SearchType _searchType = SearchType.Equal;



        #endregion

        #region "Public Variables"


        /// <summary>
        /// 查询字段
        /// </summary>
        public string FieldName
        {
            get
            {
                return _FieldName;
            }
            set
            {
                _FieldName = value;
            }
        }

        /// <summary>
        /// 查询值
        /// </summary>
        public object FieldValue
        {
            get
            {
                return _FieldValue;
            }
            set
            {
                _FieldValue = value;
            }
        }
        /// <summary>
        /// 查询类型
        /// </summary>
        public SearchType searchType
        {
            get
            {
                return _searchType;
            }
            set
            {
                _searchType = value;
            }
        }


        #endregion


        public SearchParam()
        {

        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="__FieldName">查询字段</param>
        /// <param name="__FieldValue">查询值</param>
        /// <param name="__searchType">查询类型</param>
        public SearchParam(string __FieldName, object __FieldValue, SearchType __searchType)
        {
            _FieldName = __FieldName;
            _FieldValue = __FieldValue;
            _searchType = __searchType;
        }



        /// <summary>
        /// 转换成Sql语句
        /// </summary>
        /// <returns>Sql语句</returns>
        public string ToSql()
        {
            if (searchType == SearchType.Like)
                return string.Format(" {0} like {1} ", _FieldName, FormatValue(_FieldValue, SearchType.Like));
            else if (searchType == SearchType.Gt)
                return string.Format(" {0} > {1} ", _FieldName, FormatValue(_FieldValue));
            else if (searchType == SearchType.Lt)
                return string.Format(" {0} < {1} ", _FieldName, FormatValue(_FieldValue));
            else if (searchType == SearchType.Ne)
                return string.Format(" {0} <> {1} ", _FieldName, FormatValue(_FieldValue));
            else if (searchType == SearchType.GtEqual)
                return string.Format(" {0} >= {1} ", _FieldName, FormatValue(_FieldValue));
            else if (searchType == SearchType.LtEqual)
                return string.Format(" {0} <= {1} ", _FieldName, FormatValue(_FieldValue));
            else if (searchType == SearchType.NotIn)
                return string.Format(" {0} not in ({1}) ", _FieldName, FormatValue(_FieldValue));
            else if (searchType == SearchType.In)
                return string.Format(" {0} in ({1}) ", _FieldName, _FieldValue);
            else
                return string.Format(" {0} = {1} ", _FieldName, FormatValue(_FieldValue));
        }

        private String FormatValue(Object value)
        {
            return FormatValue(value, SearchType.Equal);
        }

        private String FormatValue(Object value, SearchType Action)
        {
            Boolean isNullable = true;
            Type type = value.GetType();
            TypeCode code = Type.GetTypeCode(type);

            if (code == TypeCode.String)
            {
                if (value == null) return isNullable ? "null" : "''";
                if (String.IsNullOrEmpty(value.ToString()) && isNullable) return "null";

                if (Action == SearchType.Like)
                {
                    return "N'%" + value.ToString().Replace("'", "''") + "%'";
                }
                else
                {
                    return "N'" + value.ToString().Replace("'", "''") + "'";
                }
            }
            else if (code == TypeCode.DateTime)
            {
                if (value == null) return isNullable ? "null" : "''";
                DateTime dt = Convert.ToDateTime(value);

                if (dt < DateTime.MinValue || dt > DateTime.MaxValue) return isNullable ? "null" : "''";

                if ((dt == DateTime.MinValue || dt == DateTime.MinValue) && isNullable) return "null";

                return FormatDateTime(dt);
            }
            else if (code == TypeCode.Boolean)
            {
                if (value == null) return isNullable ? "null" : "";
                return Convert.ToBoolean(value) ? "1" : "0";
            }
            else if (type == typeof(Byte[]))
            {
                Byte[] bts = (Byte[])value;
                if (bts == null || bts.Length < 1) return "0x0";

                return "0x" + BitConverter.ToString(bts).Replace("-", null);
            }
            else
            {
                if (value == null) return isNullable ? "null" : "";
 
                return value.ToString();
            }

        }

        private String FormatDateTime(DateTime dateTime)
        {
            return "{ts '" + xCalendar.Process(dateTime).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + "'}";
            //return "{ts" + String.Format("'{0:yyyy-MM-dd HH:mm:ss}'", xCalendar.Process(dateTime)).Replace(".", ":") + "}"; 
        }
    }


    /// <summary>
    /// 查询类型
    /// </summary>
    public enum SearchType
    {
        /// <summary>
        /// 模糊
        /// </summary>
        [Text("模糊")]
        Like = 1,
        /// <summary>
        /// 等于
        /// </summary>
        [Text("等于")]
        Equal = 2,
        /// <summary>
        /// 不等于
        /// </summary>
        [Text("不等于")]
        Ne = 3,
        /// <summary>
        /// 大于
        /// </summary>
        [Text("大于")]
        Gt = 4,
        /// <summary>
        /// 小于
        /// </summary>
        [Text("小于")]
        Lt = 5,
        /// <summary>
        /// 大于等于
        /// </summary>
        [Text("大于等于")]
        GtEqual = 6,
        /// <summary>
        /// 小于等于
        /// </summary>
        [Text("小于等于")]
        LtEqual = 7,
        /// <summary>
        /// 不包含
        /// </summary>
        [Text("不包含")]
        NotIn = 8,
        /// <summary>
        /// 包含
        /// </summary>
        [Text("包含")]
        In = 9

    }
}
