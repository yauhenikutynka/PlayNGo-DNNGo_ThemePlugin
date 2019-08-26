using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Users;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Resource_Attachments : BaseModule
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
        public Int32 ArticleStatus = WebHelper.GetIntParam(HttpContext.Current.Request, "Status", -1);



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

                if (ArticleStatus >= 0)
                {
                    urls.Add(String.Format("Status={0}", ArticleStatus));
                }

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

                System.Text.StringBuilder sb = new System.Text.StringBuilder();


                sb.AppendFormat("{0}?PortalId={1}&TabId={2}&ModuleId={3}&language={4}", "Resource_Attachments.aspx", PortalId, TabId, ModuleId, language);

                foreach (String parameter in urls)
                {
                    sb.AppendFormat("&{0}", parameter);
                }

                return sb.ToString(); ;
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
            if (!String.IsNullOrEmpty(Orderfld))
            {
                qp.Orderfld = Orderfld;
            }
            else
            {
                qp.Orderfld = DNNGo_ThemePlugin_Multimedia._.ID;
            }

            #region "分页的一系列代码"


            int RecordCount = 0;
            int pagesize = qp.PageSize = 10;
            qp.PageIndex = PageIndex;


            #endregion

            //查询的方法
            qp.Where = BindSearch();

            List<DNNGo_ThemePlugin_Multimedia> Articles = DNNGo_ThemePlugin_Multimedia.FindAll(qp, out RecordCount);
            qp.RecordCount = RecordCount;
            RecordPages = qp.Pages;
            lblRecordCount.Text = String.Format("{0} {2} / {1} {3}", RecordCount, RecordPages, ViewResourceText("Title_Items", "Items"), ViewResourceText("Title_Pages", "Pages"));


            //Boolean is_admin = !IsAdministrator && !IsAdmin;




            //ctlPagingControl.TotalRecords = RecordCount;

            //if (RecordCount <= pagesize)
            //{
            //    ctlPagingControl.Visible = false;

            //}

            gvArticleList.DataSource = Articles;
            gvArticleList.DataBind();
            BindGridViewEmpty<DNNGo_ThemePlugin_Multimedia>(gvArticleList, new DNNGo_ThemePlugin_Multimedia());
        }



        /// <summary>
        /// 绑定页面项
        /// </summary>
        private void BindPageItem()
        {



        }


        /// <summary>
        /// 绑定查询的方法
        /// </summary>
        private List<SearchParam> BindSearch()
        {
            List<SearchParam> Where = new List<SearchParam>();
            Where.Add(new SearchParam(DNNGo_ThemePlugin_Multimedia._.PortalId, PortalId, SearchType.Equal));


            //筛选文章的状态
            if (ArticleStatus >= 0)
            {
                Where.Add(new SearchParam(DNNGo_ThemePlugin_Multimedia._.Status, ArticleStatus, SearchType.Equal));
            }


            if (!String.IsNullOrEmpty(Search_Title))
            {
                txtSearch.Text = HttpUtility.UrlDecode(Search_Title);
                Where.Add(new SearchParam(DNNGo_ThemePlugin_Multimedia._.Name, HttpUtility.UrlDecode(Search_Title), SearchType.Like));
            }






            return Where;
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
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
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
                cell.Text = " <input id='CheckboxAll' value='0' type='checkbox' onclick='SelectAll()'/>";
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
                DataIDX = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ID"));
                String FilePath = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "FilePath"));
                String FileExtension = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "FileExtension"));
                String Name = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Name"));
                String FileName = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "FileName"));
                TableCell cell = new TableCell();
                cell.Width = Unit.Pixel(5);
                cell.Text = string.Format(" <input name='Checkbox' id='Checkbox' value='{0}' title='{1}' file='{2}' type='checkbox'  />", DataIDX, GetPhotoExtension(FileExtension, FilePath), FileName);
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
                DNNGo_ThemePlugin_Multimedia Media = e.Row.DataItem as DNNGo_ThemePlugin_Multimedia;

                if (Media != null && Media.ID > 0)
                {




                    Label lblFileExtension = e.Row.FindControl("lblFileExtension") as Label;
                    lblFileExtension.Text = Media.FileExtension;

                    Image imgFileName = e.Row.FindControl("imgFileName") as Image;

                    HyperLink hlFileName = e.Row.FindControl("hlFileName") as HyperLink;
                    hlFileName.Text = Media.FileName.Replace("." + Media.FileExtension, "");
                    imgFileName.ImageUrl = GetPhotoExtension(Media.FileExtension, Media.FilePath);
                    hlFileName.NavigateUrl = GetPhotoPath(Media.FilePath);



                    //发布者信息
                    e.Row.Cells[2].Text = "--";
                    if (Media.LastUser > 0)
                    {
                        UserInfo uInfo = UserController.GetUserById(PortalId, Media.LastUser);
                        if (uInfo != null && uInfo.UserID > 0) e.Row.Cells[2].Text = String.Format("{0}<br />{1}", uInfo.Username, uInfo.DisplayName);
                    }


                    //发布时间
                    e.Row.Cells[3].Text = Media.LastTime.ToShortDateString();

                    //状态
                    e.Row.Cells[4].Text = EnumHelper.GetEnumTextVal(Media.Status, typeof(EnumFileStatus));

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
                Response.Redirect(CurrentUrl);
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }
        }



        #endregion




    }
}