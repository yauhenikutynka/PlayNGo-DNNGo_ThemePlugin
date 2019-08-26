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
    /// ��ҳ�洢���̲�ѯ������
    /// </summary>
    [Serializable]
    public class QueryParam
    {

        /// <summary>
        /// ���캯��
        /// </summary>
        public QueryParam()
            : this(1, int.MaxValue)
        { }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="_mPageIndex">��ǰҳ��</param>
        /// <param name="_mPageSize">ÿҳ��¼��</param>
        public QueryParam(int _mPageIndex, int _mPageSize)
        {
            _PageIndex = _mPageIndex;
            _PageSize = _mPageSize;
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="_mOrderType">�������� 1:���� ����Ϊ����</param>
        /// <param name="_mPageIndex">��ǰҳ��</param>
        /// <param name="_mPageSize">ÿҳ��¼��</param>
        public QueryParam(int _mOrderType, int _mPageIndex, int _mPageSize)
        {
            _OrderType = _mOrderType;
            _PageIndex = _mPageIndex;
            _PageSize = _mPageSize;
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="_mWhere">��ѯ���� ���Where</param>
        /// <param name="_mOrderType">�������� 1:���� ����Ϊ����</param>
        /// <param name="_mPageIndex">��ǰҳ��</param>
        /// <param name="_mPageSize">ÿҳ��¼��</param>
        public QueryParam(List<SearchParam> _mWhere, int _mOrderType,
            int _mPageIndex, int _mPageSize)
        {
            _Where = _mWhere;
            _OrderType = _mOrderType;
            _PageIndex = _mPageIndex;
            _PageSize = _mPageSize;
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="_mWhere">��ѯ���� ���Where</param>
        /// <param name="_mOrderfld">�����ֶ�</param>
        /// <param name="_mOrderType">�������� 1:���� ����Ϊ����</param>
        /// <param name="_mPageIndex">��ǰҳ��</param>
        /// <param name="_mPageSize">ÿҳ��¼��</param>
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
        /// �����ֶ�
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
        /// ��ѯ���� ���Where
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
        /// �����ֶ�
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
        /// �������� 1:���� ����Ϊ����
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
        /// ��ǰҳ��
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
        /// ÿҳ��¼��
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
        /// ��ʼ����
        /// </summary>
        public int startRowIndex
        {
            get
            {
                //��һҳʱ,�м��������㿪ʼ || ʵ�ʵ�����������һҳʱ
                if (_PageIndex == 1 || _RecordCount <= _PageSize) return _startRowIndex;

                //�������ҳ��
                int MaxPage = _RecordCount / _PageSize + (_RecordCount % _PageSize > 0 ? 1 : 0);

                //ҳ�����ܳ���ʵ�����ҳ����ж�
                if (_PageIndex > MaxPage) _PageIndex = MaxPage;

                //��ʼֵ
                return (_PageIndex - 1) * _PageSize;
            }
        }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        public int Pages
        {
            get
            {
                //������������1ҳʱ
                if (RecordCount < PageSize) return 1;
                return RecordCount / PageSize + (RecordCount % PageSize > 0 ? 1 : 0);
            }
        }

        /// <summary>
        /// �������
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
        /// ����
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
        /// ��ѯ����,��װOR��ʱ����
        /// </summary>
        public StringBuilder WhereSql
        {
            get { return _WhereSql; }
            set { _WhereSql = value; }
        }



        /// <summary>
        /// ת����Sql���
        /// </summary>
        /// <returns>Sql���</returns>
        public string ToSql()
        {

            //�����ѯ����
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
    /// ��ѯ����
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
        /// ��ѯ�ֶ�
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
        /// ��ѯֵ
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
        /// ��ѯ����
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
        /// ����
        /// </summary>
        /// <param name="__FieldName">��ѯ�ֶ�</param>
        /// <param name="__FieldValue">��ѯֵ</param>
        /// <param name="__searchType">��ѯ����</param>
        public SearchParam(string __FieldName, object __FieldValue, SearchType __searchType)
        {
            _FieldName = __FieldName;
            _FieldValue = __FieldValue;
            _searchType = __searchType;
        }



        /// <summary>
        /// ת����Sql���
        /// </summary>
        /// <returns>Sql���</returns>
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
    /// ��ѯ����
    /// </summary>
    public enum SearchType
    {
        /// <summary>
        /// ģ��
        /// </summary>
        [Text("ģ��")]
        Like = 1,
        /// <summary>
        /// ����
        /// </summary>
        [Text("����")]
        Equal = 2,
        /// <summary>
        /// ������
        /// </summary>
        [Text("������")]
        Ne = 3,
        /// <summary>
        /// ����
        /// </summary>
        [Text("����")]
        Gt = 4,
        /// <summary>
        /// С��
        /// </summary>
        [Text("С��")]
        Lt = 5,
        /// <summary>
        /// ���ڵ���
        /// </summary>
        [Text("���ڵ���")]
        GtEqual = 6,
        /// <summary>
        /// С�ڵ���
        /// </summary>
        [Text("С�ڵ���")]
        LtEqual = 7,
        /// <summary>
        /// ������
        /// </summary>
        [Text("������")]
        NotIn = 8,
        /// <summary>
        /// ����
        /// </summary>
        [Text("����")]
        In = 9

    }
}
