using System;
using System.Collections.Generic;
using System.Web;

namespace DNNGo.Modules.ThemePlugin
{
    [XmlEntityAttributes("DNNGo_GOMenu//Tabs//TabInfo")]
    public class TabEntity
    {

        private Int32 _TabID = 0;
        /// <summary>
        /// 页面编号
        /// </summary>
        public Int32 TabID
        {
            get { return _TabID; }
            set { _TabID = value; }
        }

        private String _TabName = String.Empty;
        /// <summary>
        /// 页面名称
        /// </summary>
        public String TabName
        {
            get { return _TabName; }
            set { _TabName = value; }
        }

        private String _Title = String.Empty;
        /// <summary>
        /// 页面Title
        /// </summary>
        public String Title
        {
            get { return _Title; }
            set { _Title = value; }
        }


        private Int32 _TabOrder = 0;
        /// <summary>
        /// 页面排序
        /// </summary>
        public Int32 TabOrder
        {
            get { return _TabOrder; }
            set { _TabOrder = value; }
        }



        private String _FullUrl = String.Empty;
        /// <summary>
        /// 页面地址
        /// </summary>
        public String FullUrl
        {
            get { return _FullUrl; }
            set { _FullUrl = value; }
        }


        private Int32 _ParentId = 0;
        /// <summary>
        /// 上级页面编号
        /// </summary>
        public Int32 ParentId
        {
            get { return _ParentId; }
            set { _ParentId = value; }
        }

        private Int32 _PortalID = 0;
        /// <summary>
        /// 站点编号
        /// </summary>
        public Int32 PortalID
        {
            get { return _PortalID; }
            set { _PortalID = value; }
        }

    }
}