<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Resource_GalleryImage.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Resource_GalleryImage" %>
<div class="container">
    <div class="row">
        <div class="col-sm-8">
         <div class="form-group">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="Search Text Field" x-webkit-speech></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary btn-sm" Text="Search" onclick="btnSearch_Click"  resourcekey="btnSearch" />
            </div>
        </div>
        

      </div>
     <div class="row">
        <div class="col-sm-8">
        <div class="form-group">
          <button class="btn btn-primary btn-sm" type="button" onclick="return  ReturnAlbums();"> Submit </button>
          </div>
        </div>
        <div class="col-sm-3 text_right">
        	<div class="control-inline"><asp:Label ID="lblRecordCount" runat="server"></asp:Label></div>
        </div>
      </div>

      <!-- start-->
      <div class="form-group">
             <asp:GridView ID="gvArticleList" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvArticleList_RowDataBound" OnRowCreated="gvArticleList_RowCreated" OnSorting="gvArticleList_Sorting" AllowSorting="true"
                        Width="98%" CellPadding="0" cellspacing="0" border="0" CssClass="table table-striped table-bordered table-hover"  GridLines="none" >
                        <Columns>
                             
                             <asp:TemplateField HeaderText="File Information" SortExpression="Name" >
                                <ItemTemplate>
                                    
                                    <table cellpadding="0" cellspacing="0" border="0">
                                       <tr>
                                        <td><asp:Image  runat="server" ID="imgFileName" style=" max-height:80px;max-width:80px;" />&nbsp;</td>
                                        <td> <asp:HyperLink Target="_blank" runat="server" ID="hlFileName" CssClass=""></asp:HyperLink><br />
                                   <asp:Label ID="lblFileExtension" runat="server"></asp:Label>      </td>
                                      </tr>
                                   </table>
                                    
                                    
                                                       
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="LastUser" HeaderText="Author" SortExpression="LastUser" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" /> 
                            <asp:BoundField DataField="LastTime" HeaderText="CreateTime" SortExpression="LastTime"  HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs"/> 
                             <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"/> 
                
                            
                        </Columns>
                        <PagerSettings Visible="False" />
                    </asp:GridView>
      </div>
      <!-- end--> 
      <!-- start-->
      <div class="row">
        <div class="col-sm-8">
           <ul id="paginator-ArticleList" class="pagination-purple"></ul>
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

             
               

            function ReturnAlbum(PictureUrl, PictureID) {
                window.parent.ShowAlbum(PictureUrl, PictureID);
               window.parent.$('#Pictures_Modal').modal('hide');
            }

            function ReturnAlbums() {
                var PictureUrls= new Array();
                var PictureIDs = new Array();
                var PictureNames = new Array();
                var checkok = false;
                $("input[type='checkbox'][title]:checked").each(function (index, domEle) { 
                    checkok = true;
                    PictureUrls.push($(domEle).attr("title")); 
                    PictureIDs.push($(domEle).val()); 
                    PictureNames.push($(domEle).attr("file")); 
                    //PictureUrls +=( PictureUrls !== undefined && PictureUrls !== "" ? ",":"") + $(domEle).attr("title") ;
                    //PictureIDs +=( PictureIDs !== undefined && PictureIDs !== "" ? ",":"") + $(domEle).val() ;
                });
                 if (!checkok) {
                     alert("<%=ViewResourceText("lblcheckconfirm", "Please select the records needs to be Selected!")%>");
                 }
                 else {
                    window.parent.ShowAlbums(PictureUrls, PictureIDs,PictureNames);
                    window.parent.$('#Pictures_Modal').modal('hide');
                 }
                return false;
            }
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
     
 
        // -->
            </script>

          
        </div>
      </div>
      <!-- end--> 
</div>