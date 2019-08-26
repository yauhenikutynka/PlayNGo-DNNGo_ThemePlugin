using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DNNGo.Modules.ThemePlugin
{
   

    /// <summary>
    /// 菜单集合
    /// </summary>
    public class MenuTabCollection
    {
        private Dictionary<String, MenuTabItem> _MenuTabItem = new Dictionary<String, MenuTabItem>();
        /// <summary>
        /// 根据MenuTab取出数据
        /// </summary>
        /// <param name="MenuTab">MenuTab</param>
        /// <returns></returns>
        public MenuTabItem this[String MenuTab]
        {
            get { return _MenuTabItem[MenuTab.ToLower()]; }
            set { _MenuTabItem[MenuTab.ToLower()] = value; }
        }
        /// <summary>
        /// 确认MenuTabItem是否包含指定的键
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Boolean ContainsKey(String key)
        {
            return _MenuTabItem.ContainsKey(key);
        }

        /// <summary>
        /// 数据数量
        /// </summary>
        public Int32 Count
        {
            get { return _MenuTabItem.Count; }
        }
        /// <summary>
        /// 插入MenuTab项
        /// </summary>
        /// <param name="__MenuTabItem">MenuTab项</param>
        /// <returns>插入后的数量</returns>
        public Int32 Add(MenuTabItem __MenuTabItem)
        {

            _MenuTabItem.Add(__MenuTabItem.Token.ToLower(), __MenuTabItem);
            return _MenuTabItem.Count;
        }
        /// <summary>
        /// MenuTab集合
        /// </summary>
        /// <returns></returns>
        public List<MenuTabItem> ToList()
        {
            List<MenuTabItem> lst = new List<MenuTabItem>();
            foreach (string key in _MenuTabItem.Keys)
            {
                lst.Add(_MenuTabItem[key]);
            }
            return lst;
        }

        /// <summary>
        /// 根据上级编号查找
        /// </summary>
        /// <param name="Parent"></param>
        /// <returns></returns>
        public List<MenuTabItem> FindByParent(String Parent)
        {
            List<MenuTabItem> lst = new List<MenuTabItem>();
            foreach (string key in _MenuTabItem.Keys)
            {
                if (_MenuTabItem[key].Parent == Parent)
                {
                    lst.Add(_MenuTabItem[key]);
                }
            }
            return lst;
        }

        /// <summary>
        /// 根据上级编号统计子集的数量
        /// </summary>
        /// <param name="Parent"></param>
        /// <returns></returns>
        public Int32 FindCountByParent(String Parent)
        {
            Int32 i = 0;
            foreach (string key in _MenuTabItem.Keys)
            {
                if (_MenuTabItem[key].Parent == Parent)
                {
                    i++;
                }
            }
            return i;
        }

    }
    /// <summary>
    /// 标签项
    /// </summary>
    [XmlEntityAttributes("DNNGo_ThemePlugin//Tabs//TabItem")]
    public class MenuTabItem
    {
        public MenuTabItem()
        { }


        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="__Token"></param>
        /// <param name="__Title"></param>
        /// <param name="__Src"></param>
        /// <param name="__UrlItems"></param>
        public MenuTabItem(String __Token, String __Parent, String __Title, String __Src, List<String> __UrlItems)
        {
            _Token = __Token;
            _Parent = __Parent;
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
        public MenuTabItem(String __Token, String __Parent, String __Title, String __Src)
        {
            _Token = __Token;
            _Parent = __Parent;
            _Title = __Title;
            _Src = __Src;
            _UrlItems = new String[] { "Token=" + _Token };
        }


        public MenuTabItem(String __Token, String __Parent, String __Title, String __Src, String __Icon)
        {
            _Token = __Token;
            _Parent = __Parent;
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


        private String _Link = String.Empty;
        /// <summary>
        /// 关联KEY(一般用不到)
        /// </summary>
        public String Link
        {
            get { return _Link; }
            set { _Link = value; }
        }

        private String _Parameter = String.Empty;
        /// <summary>
        /// Url参数,逗号相隔(一般用不到)
        /// </summary>
              public String Parameter
        {
            get { return _Parameter; }
            set { _Parameter = value; }
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

        private String _Parent = String.Empty;
        /// <summary>
        /// 父级KEY
        /// </summary>
        public String Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        private String[] _UrlItems = new String[] { "" };
        /// <summary>
        /// 参数组
        /// </summary>
        public String[] UrlItems
        {
            get { return _UrlItems; }
            set { _UrlItems = value; }
        }

        private Boolean _Visible = true;
        /// <summary>
        /// 是否显示菜单
        /// </summary>
        public Boolean Visible
        {
            get { return _Visible; }
            set { _Visible = value; }
        }


        private Boolean _IsAdministrator = false;
        /// <summary>
        /// 是否管理员菜单
        /// </summary>
        public Boolean IsAdministrator
        {
            get { return _IsAdministrator; }
            set { _IsAdministrator = value; }
        }


        //private MenuTabItem _ThisItem;
        ///// <summary>
        ///// 当前项(主菜单)
        ///// </summary>
        //public MenuTabItem ThisItem
        //{
        //    get { return _ThisItem; }
        //    set { _ThisItem = value; }
        //}



        //private List<MenuTabItem> _ChildList = new List<MenuTabItem>();
        ///// <summary>
        ///// 菜单子集
        ///// </summary>
        //public List<MenuTabItem> ChildList
        //{
        //    get { return _ChildList; }
        //    set { _ChildList = value; }
        //}


    }
}