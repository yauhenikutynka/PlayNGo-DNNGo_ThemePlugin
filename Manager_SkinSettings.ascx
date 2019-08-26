<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_SkinSettings.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Manager_SkinSettings" %>
<!-- start: PAGE HEADER -->
<div class="row">
    <div class="col-sm-12">
        <!-- start: PAGE TITLE & BREADCRUMB -->
        <div class="page-header">
            <h1>
                <i class="fa fa-plus"></i> <%=ViewResourceText("Header_Title", "Skin Settings")%>
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
                <i class="fa fa-external-link-square"></i><%=ViewResourceText("Title_Initialization", "Initialization Skin Plugin")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body buttons-widget  form-horizontal">
                 <div class="row form-group">
                     <div class="col-sm-3"><asp:Button CssClass="btn btn-default" ID="cmdReInit" resourcekey="cmdReInit" runat="server" Text="Initialization" CausesValidation="False" OnClick="cmdReInit_Click"  OnClientClick="CancelValidation();"></asp:Button> </div>
                    <div class="col-sm-9"></div>

                
                </div>
            </div> 
        </div>

         <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i><%=ViewResourceText("Title_ShowPluginIcon", "Show Plugin Icon")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body buttons-widget  form-horizontal">
                 <div class="row form-group">
                    <%=ViewControlTitle("lblShowIconHost", "Host", "cbShowIconHost", ":", "col-sm-3 control-label")%>
                    <div class="col-sm-9">
                            <div class="control-inline">   <asp:CheckBox ID="cbShowIconHost" runat="server" /></div>
                    </div>
                </div>
                      <div class="row form-group">
                    <%=ViewControlTitle("lblShowIconAdmin", "Admin", "cbShowIconAdmin", ":", "col-sm-3 control-label")%>
                    <div class="col-sm-9">
                            <div class="control-inline">   <asp:CheckBox ID="cbShowIconAdmin" runat="server" /></div>
                    </div>
                </div>
            </div> 
        </div>

        <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i><%=ViewResourceText("Title_Skin", "")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body buttons-widget  form-horizontal">
                 <div class="row form-group">
                    <%=ViewControlTitle("lblScope", "Scope", "rblScope", ":", "col-sm-3 control-label")%>
                    <div class="col-sm-2">
                             <asp:DropDownList  ID="rblScope" runat="server" CssClass="form-control  input_text"></asp:DropDownList>
                    </div>
                </div>
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


