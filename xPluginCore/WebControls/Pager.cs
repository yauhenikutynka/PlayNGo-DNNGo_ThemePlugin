using System;
using System.Collections.Generic;
using System.Web;

using DotNetNuke.Common;
using System.Collections.Specialized;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;

namespace DNNGo.Modules.ThemePlugin
{
    /// <summary>
    ///前面分页类
    /// </summary>
    public class Pager 
    {
        private Int32 PageIndex = 1;
        private Int32 PageSize = 10;
        private Int32 ModuleId = 0;
        private Int32 RecordCount = 0;
        private EnumPageType PageType =  EnumPageType.DnnURL;
        private Boolean IsParameter = false;
                /// <summary>
        /// 模块基类
        /// </summary>
        private PortalModuleBase pmb = new DotNetNuke.Entities.Modules.PortalModuleBase();


        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="__PageIndex">页码</param>
        /// <param name="__ModuleId"></param>
        /// <param name="__PageSize"></param>
        /// <param name="__RecordCount"></param>
        public Pager(Int32 __PageIndex, Int32 __PageSize, Int32 __ModuleId, Int32 __RecordCount)
            : this(__PageIndex, __PageSize, __ModuleId, __RecordCount, EnumPageType.DnnURL)
        {}

        public Pager(Int32 __PageIndex, Int32 __PageSize, Int32 __ModuleId, Int32 __RecordCount, EnumPageType __PageType)
            : this(__PageIndex, __PageSize, __ModuleId, __RecordCount, __PageType, false)
        { }


        public Pager(Int32 __PageIndex, Int32 __PageSize, Int32 __ModuleId, Int32 __RecordCount, EnumPageType __PageType, Boolean __IsParameter)
        {
            PageIndex = __PageIndex;
            PageSize = __PageSize;
            ModuleId = __ModuleId;
            RecordCount = __RecordCount;
            PageType = __PageType;
            IsParameter = __IsParameter;
 
        }
 

        /// <summary>
        /// 创建HTML
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="RecordCount"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public  string CreateHtml()
        {
            System.Text.StringBuilder html = new System.Text.StringBuilder();
           

            if (RecordCount == 0)
            {
                return html.ToString();
            }

            int pageCount = RecordCount / PageSize;


            if (RecordCount % PageSize > 0)
            {
                pageCount += 1;
            }
            string total = string.Empty;
            string left = string.Empty;
            string right = string.Empty;
            string center = string.Empty;
          

            //显示首页 上一页
            if (PageIndex != 1)
            {
                left = String.Format("{0}<a class=\"button\" href=\"{1}\">{2}</a>", left, CreateUrl(1), Localization.GetString("First", DotNetNuke.Services.Localization.Localization.SharedResourceFile));
                left = String.Format("{0}<a class=\"button\" href=\"{1}\">{2}</a>", left, CreateUrl(PageIndex - 1), Localization.GetString("Previous", Localization.SharedResourceFile));


            }
            else
            {
                left = String.Format("{0}<span class=\"disabled\">{1}</span>", left, Localization.GetString("First", Localization.SharedResourceFile));
                left = String.Format("{0}<span class=\"disabled\">{1}</span>", left, Localization.GetString("Previous", Localization.SharedResourceFile));
            }

            //显示尾页 下一页
            if (PageIndex < pageCount && pageCount > 1)
            {
                right = String.Format("{0}<a class=\"button\" href=\"{1}\">{2}</a>", right, CreateUrl(PageIndex + 1), Localization.GetString("Next", Localization.SharedResourceFile));
                right = String.Format("{0}<a class=\"button\" href=\"{1}\">{2}</a>", right, CreateUrl(pageCount), Localization.GetString("Last", Localization.SharedResourceFile));
            }
            else
            {
                right = String.Format("{0}<span class=\"disabled\">{1}</span>", right, Localization.GetString("Next", Localization.SharedResourceFile));
                right = String.Format("{0}<span class=\"disabled\">{1}</span>", right, Localization.GetString("Last", Localization.SharedResourceFile));
            }

            int min = 1;	//要显示的页面数最小值
            int max = pageCount;   	//要显示的页面数最大值

            if (pageCount > 5)
            {
                if (PageIndex > 2 && PageIndex < (pageCount - 1))
                {
                    min = PageIndex - 2;
                    max = PageIndex + 2;
                }
                else if (PageIndex <= 2)
                {
                    min = 1;
                    max = 5;
                }

                else if (PageIndex >= (pageCount - 1))
                {
                    min = pageCount - 4;
                    max = pageCount;
                }
            }


            //循环显示数字
            for (int i = min; i <= max; i++)
            {
                if (PageIndex == i)	//如果是当前页，用粗体和红色显示
                {
                    center += "<span class=\"disabled\">[" + i + "]</span>";
                }
                else
                {
                    center += "<a class=\"button\" href=\"" + CreateUrl(i) +"\">" + i + "</a>";
                }
            }

            total = string.Format(Localization.GetString("Pages"), PageIndex, pageCount);

            html.Append("<div class=\"pager\">");
            html.Append(total + left + center + right);
            html.Append( "</div>");



            return html.ToString();

        }



        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="_PageIndex"></param>
        /// <returns></returns>
        private String CreateUrl(Int32 _PageIndex)
        {
            if (PageType == EnumPageType.Other)
                return Globals.NavigateURL("", String.Format("PageIndex{0}={1}", ModuleId, _PageIndex));
            else if (PageType == EnumPageType.NormalURL)
            {
                String indexString = String.Format("pageindex{0}", ModuleId);
                String QueryString = String.Empty;

                if (IsParameter)
                {
                    List<String> Querys = new List<String>();

                    NameValueCollection QueryStrings = HttpContext.Current.Request.QueryString;

                    foreach (String Qkey in QueryStrings.AllKeys)
                    {
                        if (!String.IsNullOrEmpty(Qkey) &&   indexString != Qkey.ToLower())
                        {
                            QueryString += String.Format("{0}{1}={2}", QueryString == String.Empty ? "?" : "&", Qkey, QueryStrings[Qkey]);
                        }
                    }
                }

                QueryString += String.Format("{0}{1}={2}", QueryString == String.Empty ? "?" : "&", indexString, _PageIndex);
                return String.Format("{0}{1}", HttpContext.Current.Request.Path, QueryString);
            }
            else 
            {
                String indexString = String.Format("pageindex{0}", ModuleId);
                String QueryString = String.Empty;
                NameValueCollection QueryStrings = HttpContext.Current.Request.QueryString;
                List<String> Querys = new List<String>();

                foreach (String Qkey in QueryStrings.AllKeys)
                {
                    if (!String.IsNullOrEmpty(Qkey))
                    {
                        if (IsParameter || (("calendar,archive,categoryid,search,searchtag,token,articleid").IndexOf(Qkey.ToLower()) >= 0))
                        {
                            if (indexString != Qkey.ToLower())
                            {
                                // QueryString += String.Format("{0}{1}={2}", QueryString == String.Empty ? "?" : "&", Qkey, QueryStrings[Qkey]);
                                Querys.Add(String.Format("{0}={1}", Qkey, QueryStrings[Qkey]));
                            }
                        }
                    }
                }
                Querys.Add(String.Format("{0}={1}", indexString, _PageIndex));
               // QueryString += String.Format("{0}{1}={2}", QueryString == String.Empty ? "?" : "&", indexString, _PageIndex);
                return Globals.NavigateURL("", Querys.ToArray());

            }
        }

     



    }


    public enum EnumPageType
    {
        NormalURL = 2,
        DnnURL = 1,
        Other =0


    }

}