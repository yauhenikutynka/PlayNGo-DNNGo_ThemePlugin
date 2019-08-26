using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Manager_TabList : BaseModule
    {


        #region "属性"

        /// <summary>
        /// 提示操作类
        /// </summary>
        MessageTips mTips = new MessageTips();

        /// <summary>
        /// 当前页码
        /// </summary>
        public Int32 PageIndex = WebHelper.GetIntParam(HttpContext.Current.Request, "PageIndex", 1);

        /// <summary>
        /// 文章状态
        /// </summary>
        public Int32 ArticleStatus = WebHelper.GetIntParam(HttpContext.Current.Request, "Status", (Int32)EnumArticleStatus.Published);



        /// <summary>
        /// 文章搜索_标题
        /// </summary>
        public String Search_Title = WebHelper.GetStringParam(HttpContext.Current.Request, "SearchText", "");

        /// <summary>
        /// 总页码数
        /// </summary>
        public Int32 RecordPages
        {
            get;
            set;
        }

        /// <summary>
        /// 当前页面URL(不包含分页)
        /// </summary>
        public String CurrentUrl
        {
            get
            {

                List<String> urls = new List<String>();

                //if (ArticleStatus >= 0)
                //{
                urls.Add(String.Format("Status={0}", ArticleStatus));
                //}

                if (!String.IsNullOrEmpty(Orderfld))
                {
                    urls.Add(String.Format("sort_f={0}", Orderfld));
                }

                if (OrderType == 0)
                {
                    urls.Add(String.Format("sort_t={0}", OrderType));
                }

                if (!String.IsNullOrEmpty(Search_Title))
                {
                    urls.Add(String.Format("SearchText={0}", Search_Title));
                }

                return xUrl("", "", "Pages", urls.ToArray());
            }
        }


        /// <summary>
        /// 排序字段
        /// </summary>
        public string Orderfld = WebHelper.GetStringParam(HttpContext.Current.Request, "sort_f", "");


        /// <summary>
        /// 排序类型 1:降序 0:升序
        /// </summary>
        public int OrderType = WebHelper.GetIntParam(HttpContext.Current.Request, "sort_t", 1);



        #endregion



        #region "方法"

        /// <summary>
        /// 绑定列表
        /// </summary>
        private void BindDataList()
        {
            QueryParam qp = new QueryParam();
            qp.OrderType = OrderType;
            //if (!String.IsNullOrEmpty(Orderfld))
            //{
            //    qp.Orderfld = Orderfld;
            //}
            //else
            //{
            //    qp.Orderfld = String.Format("{0} desc,{1}", DNNGo_xBlog_Articles._.TopStatus, DNNGo_xBlog_Articles._.ID);
            //}

            #region "分页的一系列代码"


            int RecordCount = 0;
            int pagesize = qp.PageSize = 9999;
            qp.PageIndex = PageIndex;


            #endregion

            //查询的方法
            qp.Where = BindSearch();

            List<TabInfo> Articles = TabController.GetPortalTabs(PortalId, Null.NullInteger, true, true, false, true);
            Articles.RemoveAt(0);
            qp.RecordCount = Articles.Count;
            RecordPages = qp.Pages;
            lblRecordCount.Text = String.Format("{0} {2} / {1} {3}", qp.RecordCount, RecordPages, ViewResourceText("Title_Items", "Items"), ViewResourceText("Title_Pages", "Pages"));


            //增加一个名为Globals的菜单



            Articles.Insert(0, InitGlobalTab());
            gvArticleList.DataSource = Articles;
            gvArticleList.DataBind();
            BindGridViewEmpty<TabInfo>(gvArticleList, new TabInfo());
        }



        /// <summary>
        /// 绑定页面项
        /// </summary>
        private void BindPageItem()
        {

            hlAllArticle.NavigateUrl = xUrl("Status", "-1", "Pages");
            hlPublishedArticle.NavigateUrl = xUrl("Status", ((Int32)EnumArticleStatus.Published).ToString(), "Pages");
            hlPendingArticle.NavigateUrl = xUrl("Status", ((Int32)EnumArticleStatus.Pending).ToString(), "Pages");
            hlDraftsArticle.NavigateUrl = xUrl("Status", ((Int32)EnumArticleStatus.Draft).ToString(), "Pages");
            hlRecycleBinArticle.NavigateUrl = xUrl("Status", ((Int32)EnumArticleStatus.Recycle).ToString(), "Pages");

            switch (ArticleStatus)
            {
                case -1: hlAllArticle.CssClass = "btn btn-default active"; break;
                case (Int32)EnumArticleStatus.Published: hlPublishedArticle.CssClass = "btn btn-default active"; break;
                case (Int32)EnumArticleStatus.Pending: hlPendingArticle.CssClass = "btn btn-default active"; break;
                case (Int32)EnumArticleStatus.Draft: hlDraftsArticle.CssClass = "btn btn-default active"; break;
                case (Int32)EnumArticleStatus.Recycle: hlRecycleBinArticle.CssClass = "btn btn-default active"; break;
                default: hlPublishedArticle.CssClass = "btn btn-default active"; break;
            }


            //非管理员状态下需要屏蔽批量审核状态
            if (!IsAdministrator)
            {
                ddlStatus.Items.RemoveAt(1);
            }

        }


        /// <summary>
        /// 绑定查询的方法
        /// </summary>
        private List<SearchParam> BindSearch()
        {
            List<SearchParam> Where = new List<SearchParam>();
            Where.Add(new SearchParam("ModuleId", ModuleId, SearchType.Equal));







            return Where;
        }


        /// <summary>
        /// 格式化图标
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public String FormatIconFile(TabInfo tab)
        {
            String IconFile = String.Empty;
            if (!String.IsNullOrEmpty(tab.IconFile))
            {
                IconFile = tab.IconFile;

                if (IconFile.IndexOf("~/", StringComparison.CurrentCultureIgnoreCase) < 0 && Common.ConvertVersion(PortalProperty("Version")) <= Common.ConvertVersion("5.4.4"))
                {
                    //if (tab.IsAdminTab)
                    //{
                    //    IconFile = String.Format("~/images/{0}", IconFile);
                    //}
                    //else
                    //{
                        IconFile = PortalSettings.HomeDirectory + IconFile;
                    //}
                }
                IconFile = String.Format("<img src=\"{0}\" class=\"menuicon\"  /> ", ResolveUrl(IconFile));
            }
            return IconFile;
        }

        #endregion


        #region "事件"

        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindDataList();
                    BindPageItem();
                }
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
            }

        }

        /// <summary>
        /// 列表行创建
        /// </summary>
        protected void gvArticleList_RowCreated(object sender, GridViewRowEventArgs e)
        {

            Int32 DataIDX = 0;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //增加check列头全选
                TableCell cell = new TableCell();
                cell.Width = Unit.Pixel(5);
                cell.Text = "<label> <input id='CheckboxAll' value='0' type='checkbox' class='input_text' onclick='SelectAll()'/></label>";
                e.Row.Cells.AddAt(0, cell);


                foreach (TableCell var in e.Row.Cells)
                {
                    if (var.Controls.Count > 0 && var.Controls[0] is LinkButton)
                    {
                        string Colume = ((LinkButton)var.Controls[0]).CommandArgument;
                        if (Colume == Orderfld)
                        {
                            LinkButton l = (LinkButton)var.Controls[0];
                            l.Text += string.Format("<i class=\"fa {0}{1}\"></i>", Orderfld == "Title" ? "fa-sort-alpha-" : "fa-sort-amount-", (OrderType == 0) ? "asc" : "desc");
                        }
                    }
                }

            }
            else
            {
                //增加行选项
                DataIDX = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TabID"));
                TableCell cell = new TableCell();
                cell.Width = Unit.Pixel(5);
                cell.Text = string.Format("<label> <input name='Checkbox' id='Checkbox' value='{0}' type='checkbox' type-item=\"true\" class=\"input_text\" /></label>", DataIDX);
                e.Row.Cells.AddAt(0, cell);

            }


        }

        /// <summary>
        /// 列表行绑定
        /// </summary>
        protected void gvArticleList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //还原出数据
                TabInfo tabItem = e.Row.DataItem as TabInfo;

                if (tabItem != null && tabItem.TabID > 0 )
                {
                    HyperLink hlIconEdit = e.Row.FindControl("hlIconEdit") as HyperLink;
                    hlIconEdit.NavigateUrl = xUrl("EditTabID", tabItem.TabID.ToString(), "EditIcon");
                    String IconString = GetPortalSetting(String.Format("ThemePlugin_TabIcon_{0}", tabItem.TabID), "");
                    if (!String.IsNullOrEmpty(IconString))
                    {
                        hlIconEdit.Text = String.Format("[Edit] <i class=\"{0}\"></i>", IconString);  
                    }
                    //图标
                    //e.Row.Cells[3].Text = FormatIconFile(tabItem);
                    

                    //换背景的按钮
                    HyperLink hlBackgroundEdit = e.Row.FindControl("hlBackgroundEdit") as HyperLink;
                    HyperLink hlMobileBackgroundEdit = e.Row.FindControl("hlMobileBackgroundEdit") as HyperLink;
                    hlBackgroundEdit.NavigateUrl = hlMobileBackgroundEdit.NavigateUrl = xUrl("EditTabID", tabItem.TabID.ToString(), "Background", "FileType=" + ((Int32)(Int32)EnumRelationshipsFileType.Background).ToString());
                    //换面包屑的按钮
                    HyperLink hlBreadcrumbEdit = e.Row.FindControl("hlBreadcrumbEdit") as HyperLink;
                    HyperLink hlMobileBreadcrumbEdit = e.Row.FindControl("hlMobileBreadcrumbEdit") as HyperLink;
                    hlBreadcrumbEdit.NavigateUrl = hlMobileBreadcrumbEdit.NavigateUrl = xUrl("EditTabID", tabItem.TabID.ToString(), "Breadcrumb", "FileType=" + ((Int32)(Int32)EnumRelationshipsFileType.Breadcrumb).ToString());
                    //菜单的按钮
                    HyperLink hlMegamenuEdit = e.Row.FindControl("hlMegamenuEdit") as HyperLink;
                    HyperLink hlMobileMegamenuEdit = e.Row.FindControl("hlMobileMegamenuEdit") as HyperLink;
                    hlMegamenuEdit.NavigateUrl = hlMobileMegamenuEdit.NavigateUrl = xUrl("EditTabID", tabItem.TabID.ToString(), "MegaMenu");
                    hlMegamenuEdit.Visible = hlMobileMegamenuEdit.Visible = tabItem.ParentId == Null.NullInteger && tabItem.TabID != int.MaxValue;
                 

                }


            }
        }

        /// <summary>
        /// 列表排序
        /// </summary>
        protected void gvArticleList_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (Orderfld == e.SortExpression)
            {
                if (OrderType == 0)
                {
                    OrderType = 1;
                }
                else
                {
                    OrderType = 0;
                }
            }
            Orderfld = e.SortExpression;
            //BindDataList();
            Response.Redirect(CurrentUrl);
        }


 


        /// <summary>
        /// 搜索按钮事件
        /// </summary>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                Search_Title = HttpUtility.UrlEncode(txtSearch.Text.Trim());
                Response.Redirect(CurrentUrl, false);
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
            }
        }

        /// <summary>
        /// 状态应用按钮事件
        /// </summary>
        protected void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 Status = WebHelper.GetIntParam(Request, ddlStatus.UniqueID, -1);

                if (Status >= 0)
                {
                    string Checkbox_Value = WebHelper.GetStringParam(Request, "Checkbox", "");
                    string[] Checkbox_Value_Array = Checkbox_Value.Split(',');
                    Int32 IDX = 0;
                    for (int i = 0; i < Checkbox_Value_Array.Length; i++)
                    {
                        if (Int32.TryParse(Checkbox_Value_Array[i], out IDX))
                        {
                            //DNNGo_xBlog_Articles Article = DNNGo_xBlog_Articles.FindByKeyForEdit(IDX);
                            //if (Article != null && Article.ID > 0)
                            //{

                            //    if (Article.Status == (Int32)EnumArticleStatus.Recycle && Status == (Int32)EnumArticleStatus.Recycle)
                            //    {
                            //        if (Article.Delete() > 0)
                            //        {
                            //            //删除分类项
                            //            DNNGo_xBlog_Category_Relationships.DeleteByArticleID(Article.ID);
                            //            DNNGo_xBlog_Comments.DeleteByArticleID(Article.ID);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        Article.Status = Status;
                            //        if (Article.Update() > 0)
                            //        {
                            //        }
                            //    }
                            //}
                        }
                    }
                    BindDataList();

                    mTips.IsPostBack = true;
                    mTips.LoadMessage("ApplyStatusSuccess", EnumTips.Success, this, new String[] { EnumHelper.GetEnumTextVal(Status, typeof(EnumArticleStatus)) });
                }
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
            }
        }



        #endregion



















    }
}