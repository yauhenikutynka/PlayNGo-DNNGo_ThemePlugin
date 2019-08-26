<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_TabList.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Manager_TabList" %>
 <!-- start: PAGE HEADER -->
      <div class="row">
        <div class="col-sm-12"> 
          <!-- start: PAGE TITLE & BREADCRUMB -->
          
          <div class="page-header">
            <h1><i class="fa fa-pagelines"></i> <%=ViewResourceText("Header_Title", "All Pages")%>
            </h1>
          </div>
          <!-- end: PAGE TITLE & BREADCRUMB --> 
        </div>
      </div>
      <!-- end: PAGE HEADER --> 
      <!-- start: PAGE CONTENT -->
      
      <div class="row">
        <div class="col-sm-12">
        <!--
          <div class="form-group">
            <div class="row">
              <div class="col-sm-8 ">
               
                  <div class="btn-group">
                   <asp:HyperLink runat="server" ID="hlAllArticle" CssClass="btn btn-default" Text="All"></asp:HyperLink> 
                    <asp:HyperLink runat="server" ID="hlPublishedArticle" CssClass="btn btn-default" Text="Published"></asp:HyperLink>  
                    <asp:HyperLink runat="server" ID="hlPendingArticle" CssClass="btn btn-default" Text="Pending"></asp:HyperLink>   
                    <asp:HyperLink runat="server" ID="hlDraftsArticle" CssClass="btn btn-default" Text="Drafts"></asp:HyperLink> 
                    <asp:HyperLink runat="server" ID="hlRecycleBinArticle" CssClass="btn btn-default" Text="Recycle Bin"></asp:HyperLink> 
                  </div>
              </div>
              <div class="col-sm-4 input-group text_right">
             
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Width="100%" placeholder="Search Text Field" x-webkit-speech></asp:TextBox>
                <div class="input-group-btn">
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary" 
                      Text="<i class='fa fa-search'></i>" onclick="btnSearch_Click" />
				</div>
              
              </div>
            </div>
          </div>
          -->
          <div class="form-group">
            <div class="row">
              <div class="col-sm-9">
              <!--
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control form_default">
                    <asp:ListItem Value="-1" Text="Bulk Actions"  resourcekey="ddlStatus_BulkActions"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Published" resourcekey="ddlStatus_Published"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Pending" resourcekey="ddlStatus_Pending"></asp:ListItem>
                    <asp:ListItem Value="4" Text="Delete" resourcekey="ddlStatus_Delete"></asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnApply" runat="server" CssClass="btn btn-default" Text="Apply" resourcekey="btnApply" onclick="btnApply_Click" OnClientClick="return ApplyStatus();" />
                -->
              
              </div>
              <div class="col-sm-3 text_right"> <br/>
                <asp:Label ID="lblRecordCount" runat="server"></asp:Label> </div>
            </div>
          </div>
          <div class="form-group">
            <asp:GridView ID="gvArticleList" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvArticleList_RowDataBound" OnRowCreated="gvArticleList_RowCreated" OnSorting="gvArticleList_Sorting" AllowSorting="true"
                        Width="100%" CellPadding="0" cellspacing="0" border="0" CssClass="table table-striped table-bordered table-hover"  GridLines="none" >
                        <Columns>
                             

                            <asp:BoundField DataField="IndentedTabName" HeaderText="TabName"  /> 
                            <asp:BoundField DataField="Title" HeaderText="Title"  HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" /> 
                            
                            <asp:BoundField DataField="IsVisible" HeaderText="Visible" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" /> 
                            <asp:BoundField DataField="DisableLink" HeaderText="DisableLink" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" /> 
                            <asp:BoundField DataField="TabType" HeaderText="TabType" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" /> 
                            <asp:BoundField DataField="CreatedOnDate" HeaderText="CreatedDate" DataFormatString="{0:d}"  HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" /> 
                           

                            <asp:TemplateField HeaderText="Icon" HeaderStyle-CssClass="visible-md visible-lg hidden-sm hidden-xs" ItemStyle-CssClass="visible-md visible-lg hidden-sm hidden-xs" HeaderStyle-Width="150" >
                                <ItemTemplate>
                                     <div class="visible-md visible-lg hidden-sm hidden-xs">
                                        <asp:HyperLink ID="hlIconEdit" runat="server" CssClass="tooltips" data-original-title="Edit Icon" data-placement="top" Text="[Edit]"></asp:HyperLink>
                                     </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Background" HeaderStyle-CssClass="visible-md visible-lg hidden-sm hidden-xs"  ItemStyle-CssClass="visible-md visible-lg hidden-sm hidden-xs" HeaderStyle-Width="100" >
                                <ItemTemplate>
                                     <div class="visible-md visible-lg hidden-sm hidden-xs">
                                        <asp:HyperLink ID="hlBackgroundEdit" runat="server" CssClass="btn btn-xs btn-teal tooltips" data-original-title="Edit Background" data-placement="top" Text="<i class='fa clip-images'></i>"></asp:HyperLink>
                                     </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Breadcrumb" HeaderStyle-CssClass="visible-md visible-lg hidden-sm hidden-xs"  ItemStyle-CssClass="visible-md visible-lg hidden-sm hidden-xs" HeaderStyle-Width="100">
                                <ItemTemplate>
                                     <div class="visible-md visible-lg hidden-sm hidden-xs">
                                        <asp:HyperLink ID="hlBreadcrumbEdit" runat="server" CssClass="btn btn-xs btn-teal tooltips" data-original-title="Edit Breadcrumb" data-placement="top" Text="<i class='fa clip-images'></i>"></asp:HyperLink>
                                     </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Megamenu" HeaderStyle-CssClass="visible-md visible-lg hidden-sm hidden-xs"  ItemStyle-CssClass="visible-md visible-lg hidden-sm hidden-xs" HeaderStyle-Width="100">
                                <ItemTemplate>
                                     <div class="visible-md visible-lg hidden-sm hidden-xs">
                                        <asp:HyperLink ID="hlMegamenuEdit" runat="server" CssClass="btn btn-xs btn-teal tooltips" data-original-title="Edit Megamenu" data-placement="top" Text="<i class='fa clip-images'></i>"></asp:HyperLink>
                                     </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="visible-xs visible-sm hidden-md hidden-lg" ItemStyle-CssClass="center visible-xs visible-sm hidden-md hidden-lg" HeaderStyle-Width="100">
                                <ItemTemplate>
                                    <div class="visible-xs visible-sm hidden-md hidden-lg">
                                      <div class="btn-group"> <a href="#" data-toggle="dropdown" class="btn btn-primary dropdown-toggle btn-sm"> <i class="fa fa-cog"></i> <span class="caret"></span> </a>
                                        <ul class="dropdown-menu pull-right" role="menu">
                                         <li role="presentation">  <asp:HyperLink ID="hlMobileBackgroundEdit" runat="server" tabindex="-1" role="menuitem" Text="<i class='fa clip-images'></i> Edit Background"></asp:HyperLink></li>
                                         <li role="presentation">  <asp:HyperLink ID="hlMobileBreadcrumbEdit" runat="server" tabindex="-1" role="menuitem" Text="<i class='fa clip-images'></i> Edit Breadcrumb"></asp:HyperLink></li>
                                         <li role="presentation">  <asp:HyperLink ID="hlMobileMegamenuEdit" runat="server" tabindex="-1" role="menuitem" Text="<i class='fa clip-images'></i> Edit Megamenu"></asp:HyperLink></li>
                                        </ul>
                                      </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>



                            
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                    <!--<ul id="paginator-ArticleList" class="pagination-purple"></ul>
                    <script type="text/javascript">
                        $(document).ready(function () {
                            $('#paginator-ArticleList').bootstrapPaginator({
                                bootstrapMajorVersion: 3,
                                currentPage: <%=PageIndex %>,
                                totalPages: <%=RecordPages %>,
                                numberOfPages:7,
                                useBootstrapTooltip:true,
                                onPageClicked: function (e, originalEvent, type, page) {
                                    window.location.href='<%=CurrentUrl %>&PageIndex='+ page;
                                }
                            });
                        });
                    </script>-->
          </div>
        </div>
        
        <!-- end: PAGE CONTENT--> 
      </div>


 <script type="text/javascript">
<!--

     function SelectAll() {
         var e = document.getElementsByTagName("input");
         var IsTrue;
         if (document.getElementById("CheckboxAll").value == "0") {
             IsTrue = true;
             document.getElementById("CheckboxAll").value = "1"
         }
         else {
             IsTrue = false;
             document.getElementById("CheckboxAll").value = "0"
         }
         for (var i = 0; i < e.length; i++) {
             if (e[i].type == "checkbox") {
                 e[i].checked = IsTrue;
             }
         }
     }
     function ApplyStatus() {
         var StatusSelected = $("#<%=ddlStatus.ClientID %>").find("option:selected").val();
         if (StatusSelected >= 0) {
             var checkok = false;
             $("#<%=gvArticleList.ClientID %> input[type='checkbox'][type-item='true']").each(function (i, n) {
                 if ($(this).prop('checked')) {
                     checkok = true;
                 }
             });

             if (checkok) {
                 return confirm('<%=ViewResourceText("Confirm_ApplyStatus", "Are you sure to operate the records you choose?") %>');
             }
             alert('<%=ViewResourceText("Alert_NoItems", "Please operate with one line of record selected at least.") %>');

         } else {
             alert('<%=ViewResourceText("Alert_NoActions", "Please choose the operation you need.") %>');
         }
         return false;
     }
   
 
// -->
    </script>