using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    /// 标签集合
    /// </summary>
    public class TokenCollection
    {
        private Dictionary<String, TokenItem> _TokenItem = new Dictionary<String, TokenItem>();
        /// <summary>
        /// 根据Token取出数据
        /// </summary>
        /// <param name="Token">Token</param>
        /// <returns></returns>
        public TokenItem this[String Token]
        {
            get { return _TokenItem[Token.ToLower()]; }
            set { _TokenItem[Token.ToLower()] = value; }
        }
        /// <summary>
        /// 确认TokenItem是否包含指定的键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean ContainsKey(String key)
        {
            return _TokenItem.ContainsKey(key);
        }

        /// <summary>
        /// 数据数量
        /// </summary>
        public Int32 Count
        {
            get { return _TokenItem.Count; }
        }
        /// <summary>
        /// 插入Token项
        /// </summary>
        /// <param name="__TokenItem">Token项</param>
        /// <returns>插入后的数量</returns>
        public Int32 Add(TokenItem __TokenItem)
        {
           
            _TokenItem.Add(__TokenItem.Token.ToLower(), __TokenItem);
            return _TokenItem.Count;
        }
        /// <summary>
        /// Token集合
        /// </summary>
        /// <returns></returns>
        public List<TokenItem> ToList()
        {
            List<TokenItem> lst = new List<TokenItem>();
            foreach (string key in _TokenItem.Keys)
            {
                lst.Add(_TokenItem[key]);
            }
            return lst;
        }


    }
    /// <summary>
    /// 标签项
    /// </summary>
    public class TokenItem
    {
        public TokenItem()
        { }


        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="__Token"></param>
        /// <param name="__Title"></param>
        /// <param name="__Src"></param>
        /// <param name="__UrlItems"></param>
        public TokenItem(String __Token, String __Title, String __Src, List<String> __UrlItems)
        {
            _Token = __Token;
            _Title = __Title;
            _Src = __Src;
            List<String> Urls = new List<string>(__UrlItems.ToArray());
            Urls.Add("Token=" + _Token);
            _UrlItems = Urls.ToArray();
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="__Token"></param>
        /// <param name="__Title"></param>
        /// <param name="__Src"></param>
        public TokenItem(String __Token, String __Title, String __Src)
        {
            _Token = __Token;
            _Title = __Title;
            _Src = __Src;
            _UrlItems = new String[] { "Token=" + _Token };
        }

         
        public TokenItem(String __Token, String __Title, String __Src,String __Icon)
        {
            _Token = __Token;
            _Title = __Title;
            _Src = __Src;
            _UrlItems = new String[] { "Token=" + _Token };
            _Icon = __Icon;
        }



        private String _Token = String.Empty;
        /// <summary>
        /// 标签名
        /// </summary>
        public String Token
        {
            get { return _Token; }
            set { _Token = value; }
        }

        private String _Title = String.Empty;
        /// <summary>
        /// 标题
        /// </summary>
        public String Title
        {
            get { return _Title; }
            set { _Title = value; }
        }


        private String _Src = String.Empty;
        /// <summary>
        /// 模块路径
        /// </summary>
        public String Src
        {
            get { return _Src; }
            set { _Src = value; }
        }

        private String _Icon = String.Empty;
        /// <summary>
        /// 图标
        /// </summary>
        public String Icon
        {
            get { return _Icon; }
            set { _Icon = value; }
        } 



        private String[] _UrlItems = new  String[]{""};
        /// <summary>
        /// 参数组
        /// </summary>
        public String[] UrlItems
        {
            get { return _UrlItems; }
            set { _UrlItems = value; }
        }

        private TokenItem _ThisItem;
        /// <summary>
        /// 当前项(主菜单)
        /// </summary>
        public TokenItem ThisItem
        {
            get { return _ThisItem; }
            set { _ThisItem = value; }
        }



        private List<TokenItem> _ChildList = new List<TokenItem>();
        /// <summary>
        /// 菜单子集
        /// </summary>
        public List<TokenItem> ChildList
        {
            get { return _ChildList; }
            set { _ChildList = value; }
        }


    }
}
