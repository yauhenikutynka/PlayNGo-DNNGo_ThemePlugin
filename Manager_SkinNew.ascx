<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_SkinNew.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Manager_SkinNew" %>
<!-- start: PAGE HEADER -->
<div class="row">
    <div class="col-sm-12">
        <!-- start: PAGE TITLE & BREADCRUMB -->
        <div class="page-header">
            <h1>
                <i class="fa fa-plus"></i> <%=ViewResourceText("Header_Title", "Create New Skin")%>
                <%--<small>overview &amp; stats </small>--%></h1>
        </div>
        <!-- end: PAGE TITLE & BREADCRUMB -->
    </div>
</div>
<!-- end: PAGE HEADER -->



<div class="row">
    <div class="col-sm-12">
 <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i><%=ViewResourceText("Title_CopySkin", "Copy skin")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body buttons-widget form-horizontal">
                 <div class="row form-group">
                    <%=ViewControlTitle("lblSkinFiles", "Source Skin", "ddlSkinFiles", ":", "col-sm-3 control-label")%>
                    <div class="col-sm-7">
                             <asp:DropDownList ID="ddlSkinFiles" runat="server" CssClass="form-control form_default input_text validate[required]"></asp:DropDownList>.ascx
                    </div>
                </div>
                <div class="row form-group">
                    <%=ViewControlTitle("lblNewSkin", "New Skin", "txtNewSkin", ":", "col-sm-3 control-label")%>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtNewSkin" runat="server" CssClass="form-control form_default input_text validate[required,custom[onlyLetterNumber]]"></asp:TextBox>.ascx
                    </div>
                </div>
                <div id="divMessage" style="display:none; color:Red;"><%=ViewResourceText("lblMessage", "No repeating for skin names.")%></div>
            </div>
        </div>
    </div>
</div>


       <div class="row">
        <div class="col-sm-3"> </div>
        <div class="col-sm-9">
            
         <asp:Button CssClass="btn btn-primary" lang="Submit" ID="cmdUpdate" resourcekey="cmdUpdate"
        runat="server" Text="Update" OnClick="cmdUpdate_Click"></asp:Button>&nbsp;
        <asp:Button CssClass="btn btn-default" ID="cmdCancel" resourcekey="cmdCancel" runat="server"
            Text="Cancel" CausesValidation="False" OnClick="cmdCancel_Click"  OnClientClick="CancelValidation();"></asp:Button>&nbsp;

         </div>
      </div>




      <script type="text/javascript">

          jQuery(function ($) {
              $("#<%=txtNewSkin.ClientID %>").keyup(function () {
                  var a, b, c;
                  a = false;
                  b = $(this).val();
                  $("#<%=ddlSkinFiles.ClientID %> option").each(function (i) { if (this.value.toLowerCase() === b.toLowerCase()) a = true; });
                  if (a) {
                      $("#<%=cmdUpdate.ClientID %>").attr("disabled", "disabled");
                      $("#divMessage").show();
                  } else {
                      $("#<%=cmdUpdate.ClientID %>").removeAttr("disabled");
                      $("#divMessage").hide();
                  }

              });

          });
</script>