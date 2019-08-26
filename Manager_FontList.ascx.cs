using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DNNGo.Modules.ThemePlugin
{
    public partial class Manager_FontList : BaseModule
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
        /// 是否外部链接
        /// </summary>
        public Int32 IsFontLink = WebHelper.GetIntParam(HttpContext.Current.Request, "IsFontLink", -1);



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

                if (IsFontLink >= 0)
                {
                    urls.Add(String.Format("IsFontLink={0}", IsFontLink));
                }


                if (!String.IsNullOrEmpty(Search_Title))
                {
                    urls.Add(String.Format("SearchText={0}", Search_Title));
                }

                return xUrl("", "", "Fonts", urls.ToArray());
            }
        }





        #endregion



        #region "方法"

        /// <summary>
        /// 绑定列表
        /// </summary>
        private void BindDataList()
        {
            QueryParam qp = new QueryParam();
            

            #region "分页的一系列代码"


            int RecordCount = 0;
            int pagesize = qp.PageSize = 10;
            qp.PageIndex = PageIndex;


            #endregion

        


            FontDB SearchFont = new FontDB();

            //查询的方法
            if (!String.IsNullOrEmpty(Search_Title))
            {
                SearchFont.Alias = Search_Title;
                txtSearch.Text = Search_Title;
            }

            if (IsFontLink >= 0)
            {
                SearchFont.IsFontLink = IsFontLink == 1;
            }



            List<FontDB> FontList = new FontDBHelper(this).FindAll(SearchFont, qp.PageIndex, qp.PageSize, out RecordCount);
            FontList.Sort(delegate (FontDB x, FontDB y)
            {
                if (!x.IsFontLink.HasValue || !y.IsFontLink.HasValue) return 0;
                else return y.IsFontLink.Value.CompareTo(x.IsFontLink.Value);
            });


            qp.RecordCount = RecordCount;
            RecordPages = qp.Pages;
            lblRecordCount.Text = String.Format("{0} {2} / {1} {3}", RecordCount, RecordPages, ViewResourceText("Title_Items", "Items"), ViewResourceText("Title_Pages", "Pages"));

            

            gvCommentList.DataSource = FontList;
            gvCommentList.DataBind();
            BindGridViewEmpty<FontDB>(gvCommentList, new FontDB());
        }



        /// <summary>
        /// 绑定页面项
        /// </summary>
        private void BindPageItem()
        {

            hlFontAll.NavigateUrl = xUrl("Fonts");
            hlFontGoogle.NavigateUrl = xUrl("IsFontLink", ((Int32)EnumFontLink.Google).ToString(), "Fonts");
            hlFontUpload.NavigateUrl = xUrl("IsFontLink", ((Int32)EnumFontLink.Upload).ToString(), "Fonts");

            switch (IsFontLink)
            {
                case -1: hlFontAll.CssClass = "btn btn-default active"; break;
                case (Int32)EnumFontLink.Google: hlFontGoogle.CssClass = "btn btn-default active"; break;
                case (Int32)EnumFontLink.Upload: hlFontUpload.CssClass = "btn btn-default active"; break;
                default: hlFontAll.CssClass = "btn btn-default active"; break;
            }

            //hlAddNewLink.NavigateUrl = xUrl("AddMedia");

            //插入谷歌字体菜单
            hlAddFontByGoogle.NavigateUrl = xUrl("AddFontByGoogle");

            //字体上传菜单
            hlAddFontByUpload.NavigateUrl = xUrl("AddFontByUpload");
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
        protected void gvCommentList_RowCreated(object sender, GridViewRowEventArgs e)
        {

            String DataIDX = String.Empty;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //增加check列头全选
                TableCell cell = new TableCell();
                cell.Width = Unit.Pixel(5);
                cell.Text = "<label> <input id='CheckboxAll' value='0' type='checkbox' class='input_text' onclick='SelectAll()'/></label>";
                e.Row.Cells.AddAt(0, cell);


            }
            else
            {
                //增加行选项
                DataIDX = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "PrimaryGuid"));
                TableCell cell = new TableCell();
                cell.Width = Unit.Pixel(5);
                cell.Text = string.Format("<label> <input name='Checkbox' id='Checkbox' value='{0}' type='checkbox' type-item=\"true\" class=\"input_text\" /></label>", DataIDX);
                e.Row.Cells.AddAt(0, cell);

            }


        }

        /// <summary>
        /// 列表行绑定
        /// </summary>
        protected void gvCommentList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //还原出数据
                FontDB FontItem = e.Row.DataItem as FontDB;

                if (FontItem != null && !String.IsNullOrEmpty( FontItem.PrimaryGuid))
                {
                    #region "编辑&删除按钮"
                    HyperLink hlEdit = e.Row.FindControl("hlEdit") as HyperLink;
                    HyperLink hlMobileEdit = e.Row.FindControl("hlMobileEdit") as HyperLink;
                    LinkButton btnRemove = e.Row.FindControl("btnRemove") as LinkButton;
                    LinkButton btnMobileRemove = e.Row.FindControl("btnMobileRemove") as LinkButton;
                    //设置按钮的CommandArgument
                    btnRemove.CommandArgument = btnMobileRemove.CommandArgument = FontItem.PrimaryGuid;
                    //设置删除按钮的提示
                    //if (Media.Status == (Int32)EnumFileStatus.Recycle)
                    //{


                    if (FontItem.IsFontLink.HasValue && FontItem.IsFontLink.Value)
                    {
                        hlEdit.NavigateUrl = hlMobileEdit.NavigateUrl = xUrl("PrimaryGuid", FontItem.PrimaryGuid, "AddFontByGoogle");
                    }
                    else
                    {
                        hlEdit.NavigateUrl = hlMobileEdit.NavigateUrl = xUrl("PrimaryGuid", FontItem.PrimaryGuid, "AddFontByUpload");
                    }

                    if (FontItem.IsSystem.HasValue && !FontItem.IsSystem.Value)
                    {
                        btnRemove.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");
                        btnMobileRemove.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');");

                     
                    }
                    else
                    {
                        btnRemove.Visible = btnRemove.Enabled = false;
                        btnMobileRemove.Visible = btnMobileRemove.Enabled = false;
                    }

                       
                    //}
                    //else
                    //{
                    //    btnRemove.Attributes.Add("onClick", "javascript:return confirm('" + ViewResourceText("DeleteRecycleItem", "Are you sure to move it to recycle bin?") + "');");
                    //    btnMobileRemove.Attributes.Add("onClick", "javascript:return confirm('" + ViewResourceText("DeleteRecycleItem", "Are you sure to move it to recycle bin?") + "');");
                    //}

               
                    #endregion

                 


                    //发布时间
                    e.Row.Cells[4].Text = FontItem.CreateTime.ToShortDateString();
                    e.Row.Cells[3].Text = FontItem.UpdateTime.ToShortDateString();

                    //系统字体
                    e.Row.Cells[6].Text = EnumHelper.GetEnumTextVal(FontItem.IsSystem.HasValue && FontItem.IsSystem.Value ? 1 : 0,  typeof(EnumFontSystem));
                    //外部链接字体
                    e.Row.Cells[5].Text = EnumHelper.GetEnumTextVal(FontItem.IsFontLink.HasValue && FontItem.IsFontLink.Value ? 1 : 0, typeof(EnumFontLink));

                  


                }
            }
        }




        /// <summary>
        /// 列表上的项删除事件
        /// </summary>
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btnRemove = (LinkButton)sender;

                if (btnRemove != null && !String.IsNullOrEmpty(btnRemove.CommandArgument))
                {

                    mTips.IsPostBack = true;

                    FontDB FontItem = new FontDBHelper(this).Find(btnRemove.CommandArgument);

                    if (FontItem != null && !String.IsNullOrEmpty( FontItem.PrimaryGuid))
                    {
                        if (FontItem.IsSystem.HasValue && !FontItem.IsSystem.Value)
                        {
                            new FontDBHelper(this).Delete(FontItem.PrimaryGuid);

                            mTips.LoadMessage("DeleteMediaLibrarySuccess", EnumTips.Success, this, new String[] { FontItem.Alias });
                        }
                        else
                        {
                            //这是系统内置的字体不允许删除
                            mTips.LoadMessage("DeleteMediaLibrarySuccess", EnumTips.Success, this, new String[] { FontItem.Alias });

                        }
                         
                        BindDataList();
                    }
                }
            }
            catch (Exception ex)
            {
                ProcessModuleLoadException(ex);
            }
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
                    String IDX = String.Empty;
                    mTips.IsPostBack = true;
                    for (int i = 0; i < Checkbox_Value_Array.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(Checkbox_Value_Array[i]))
                        {
 
                            FontDB FontItem = new FontDBHelper(this).Find(Checkbox_Value_Array[i]);

                            if (FontItem != null && !String.IsNullOrEmpty(FontItem.PrimaryGuid))
                            {
                                if (FontItem.IsSystem.HasValue && !FontItem.IsSystem.Value)
                                {
                                    new FontDBHelper(this).Delete(FontItem.PrimaryGuid);

                                    mTips.LoadMessage("DeleteMediaLibrarySuccess", EnumTips.Success, this, new String[] { FontItem.Alias });
                                }
                                else
                                {
                                    //这是系统内置的字体不允许删除
                                    mTips.LoadMessage("DeleteMediaLibrarySuccess", EnumTips.Success, this, new String[] { FontItem.Alias });

                                }

                           
                            }

 
                        }
                    }
                    BindDataList();
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