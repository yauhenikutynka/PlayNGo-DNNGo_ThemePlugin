<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_FontList.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Manager_FontList" %>
  <!-- start: PAGE HEADER -->
      <div class="row">
        <div class="col-sm-12"> 
          <!-- start: PAGE TITLE & BREADCRUMB -->
           
          <div class="page-header">
            <h1><i class="fa fa-font"></i> <%=ViewResourceText("Header_Title", "Fonts")%> 
                <asp:HyperLink ID="hlAddFontByGoogle" runat="server" CssClass="btn btn-xs btn-bricky" Text="<i class='fa fa-link'></i> Add Google Font " resourcekey="hlAddFontByGoogle" ></asp:HyperLink>
                 <asp:HyperLink ID="hlAddFontByUpload" runat="server" CssClass="btn btn-xs btn-green"  Text="<i class='fa fa-paperclip'></i> Upload Font " resourcekey="hlAddFontByUpload" ></asp:HyperLink>
            </h1>

            <div id="AddMedia_Modal" class="modal fade" tabindex="-1" data-width="760" data-height="400" style="display: none;">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>
                    <h4 class="modal-title">
                        <i class='fa fa-folder-open'></i> <%=ViewResourceText("Title_AddNew", "Add New Media Library")%></h4>
                </div>
                <div class="modal-body">
                    <iframe id="AddMedia_Iframe" width="100%" height="100%" style="border-width: 0px;">
                    </iframe>
                </div>
            </div>


          </div>
          <!-- end: PAGE TITLE & BREADCRUMB --> 
        </div>
      </div>
      <!-- end: PAGE HEADER --> 

        <!-- start: PAGE CONTENT -->
      
      <div class="row">
        <div class="col-sm-12">
          <div class="form-group">
            <div class="row">
              <div class="col-sm-8">
                  <div class="btn-group">
                    <asp:HyperLink runat="server" ID="hlFontAll" CssClass="btn btn-default" Text="All"></asp:HyperLink>
                    <asp:HyperLink runat="server" ID="hlFontGoogle" CssClass="btn btn-default" Text="Google Font"></asp:HyperLink>
                    <asp:HyperLink runat="server" ID="hlFontUpload" CssClass="btn btn-default" Text="Upload Font"></asp:HyperLink>
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
          <div class="form-group">
            <div class="row">
              <div class="col-sm-9">
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control form_default">
                    <asp:ListItem Value="-1" Text="Bulk Actions"  resourcekey="ddlStatus_BulkActions"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Delete" resourcekey="ddlStatus_Delete"></asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnApply" runat="server" CssClass="btn btn-default" Text="Apply" resourcekey="btnApply" onclick="btnApply_Click" OnClientClick="return ApplyStatus();" />

              </div>
              <div class="col-sm-3 text_right"> <br/>
                <asp:Label ID="lblRecordCount" runat="server"></asp:Label> </div>
            </div>
          </div>
          <div class="form-group">
            <asp:GridView ID="gvCommentList" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvCommentList_RowDataBound" OnRowCreated="gvCommentList_RowCreated" 
                        Width="100%" CellPadding="0" cellspacing="0" border="0" CssClass="table table-striped table-bordered table-hover"  GridLines="none" >
                        <Columns>
                           <asp:BoundField DataField="Alias" HeaderText="Alias" /> 
                           <asp:BoundField DataField="Family" HeaderText="Font Family Name" /> 
       
                           <asp:BoundField DataField="UpdateTime" HeaderText="UpdateTime"  HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs"/> 
                           <asp:BoundField DataField="CreateTime" HeaderText="CreateTime"  HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" /> 
                           <asp:BoundField DataField="IsFontLink" HeaderText="Type" />
                           <asp:BoundField DataField="IsSystem" HeaderText="System" />
                           <asp:BoundField DataField="Enable" HeaderText="Enable" />
                            <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="center">
                                <ItemTemplate>
                                     <div class="visible-md visible-lg hidden-sm hidden-xs">
                                        <asp:HyperLink ID="hlEdit" runat="server" CssClass="btn btn-xs btn-teal tooltips" data-original-title="Edit" data-placement="top" Text="<i class='fa fa-edit'></i>"></asp:HyperLink>
                                        <asp:LinkButton ID="btnRemove" runat="server" CssClass="btn btn-xs btn-bricky tooltips" data-original-title="Remove" data-placement="top" Text="<i class='fa fa-times fa fa-white'></i>" OnClick="btnRemove_Click"></asp:LinkButton>
                                         
                                     </div>
                                    <div class="visible-xs visible-sm hidden-md hidden-lg">
                                      <div class="btn-group"> <a href="#" data-toggle="dropdown" class="btn btn-primary dropdown-toggle btn-sm"> <i class="fa fa-cog"></i> <span class="caret"></span> </a>
                                        <ul class="dropdown-menu pull-right" role="menu">
                                          <li role="presentation">  <asp:HyperLink ID="hlMobileEdit" runat="server" tabindex="-1" role="menuitem" Text="<i class='fa fa-edit'></i> Edit"></asp:HyperLink></li>
                                          <li role="presentation"> <asp:LinkButton ID="btnMobileRemove" runat="server" tabindex="-1" role="menuitem" data-placement="top" Text="<i class='fa fa-times'></i> Remove"  OnClick="btnRemove_Click"></asp:LinkButton></li>
                                        </ul>
                                      </div>
                                    </div>
                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
                    <ul id="paginator-CommentList" class="pagination-purple"></ul>
                    <script type="text/javascript">
                        $(document).ready(function () {
                            $('#paginator-CommentList').bootstrapPaginator({
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
                    </script>
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
             $("#<%=gvCommentList.ClientID %> input[type='checkbox'][type-item='true']").each(function (i, n) {
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