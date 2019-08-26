<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_TabEdit.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Manager_TabEdit"  %>
<!-- start: PAGE HEADER -->
<div class="row">
    <div class="col-sm-12">
        <!-- start: PAGE TITLE & BREADCRUMB -->
        <div class="page-header">
            <h1>
                <i class="fa fa-plus"></i> <asp:Literal ID="liHeader_Title" runat="server"></asp:Literal>  <%--<%=ViewResourceText("Header_Title", "Page Settings")%>--%>
                <%--<small>overview &amp; stats </small>--%></h1>
        </div>
        <!-- end: PAGE TITLE & BREADCRUMB -->
    </div>
</div>
<!-- end: PAGE HEADER -->

<!-- start: PAGE CONTENT -->
<div class="row">
    <div class="col-sm-8">
        <asp:PlaceHolder ID="phPlaceHolder" runat="server"></asp:PlaceHolder>
    </div>
    <div class="col-sm-4">
    <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i><%=ViewResourceText("Title_PageInfo", "Page Info")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body buttons-widget">
                 <div class="row form-group">
                    <%=ViewControlTitle("lblTabName", "Tab Name", "", ":", "col-sm-5 control-label")%>
                    <div class="col-sm-7">
                        <%=TabItem.TabName%>
                    </div>
                </div>
                <div class="row form-group">
                    <%=ViewControlTitle("lblTitle", "Title", "", ":", "col-sm-5 control-label")%>
                    <div class="col-sm-7">
                        <%=TabItem.Title %>
                    </div>
                </div>
                <div class="row form-group">
                    <%=ViewControlTitle("lblIconFile", "IconFile", "", ":", "col-sm-5 control-label")%>
                    <div class="col-sm-7">
                        <%=TabItem.IconFile%>
                    </div>
                </div>
                <div class="row form-group">
                    <%=ViewControlTitle("lblIsVisible", "IsVisible", "", ":", "col-sm-5 control-label")%>
                    <div class="col-sm-7">
                        <%=TabItem.IsVisible%>
                    </div>
                </div>
                <div class="row form-group">
                    <%=ViewControlTitle("lblCreatedOnDate", "CreatedOnDate", "", ":", "col-sm-5 control-label")%>
                    <div class="col-sm-7">
                        <%=TabItem.CreatedOnDate%>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<!-- end: PAGE CONTENT-->