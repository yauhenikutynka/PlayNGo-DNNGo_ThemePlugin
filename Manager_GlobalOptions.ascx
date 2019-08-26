<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_GlobalOptions.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Manager_GlobalOptions" %>
<!-- start: PAGE HEADER -->
<div class="row">
    <div class="col-sm-12">
        <!-- start: PAGE TITLE & BREADCRUMB -->
        <div class="page-header">
            <h1>
                <i class="fa fa-plus"></i> <%=ViewResourceText("Header_Title", "Global Options")%>
                <%--<small>overview &amp; stats </small>--%></h1>
        </div>
        <!-- end: PAGE TITLE & BREADCRUMB -->
    </div>
</div>
<!-- end: PAGE HEADER -->



        <div class="row"> 
          <!-- start: PAGE CONTENT -->
          <div class="col-sm-8">
            <ul id="myTab_ul_tabs" class="myTab_ul_tabs ul_tabs nav nav-tabs tab-bricky">
              <asp:Literal runat="server" ID="liNavTabsHTML_Left"></asp:Literal>
            </ul>
            <div class="tab-content tab_left">
            
                <asp:Repeater ID="RepeaterCategories_Left" runat="server" OnItemDataBound="RepeaterCategories_ItemDataBound">
                    <ItemTemplate>

                     <div class="tab-pane <%#(Container.ItemIndex==1)?"in active":""%>" id="tabs-Left-<%#FormatName( Eval("Key"))%>">
                        <div id="accordion<%#FormatName( Eval("Key"))%>">
                          <asp:Repeater ID="RepeaterGroup" runat="server" OnItemDataBound="RepeaterGroup_ItemDataBound">
                            <ItemTemplate>
                                 <div class="panel panel-default small-bottom">
                                    <div class="panel-heading"> <i class="fa fa-external-link-square"></i> <%# Eval("Key")%>
                                      <div class="panel-tools"> <a class="btn btn-xs btn-link panel-collapse collapses" data-toggle="collapse" data-parent="#accordion<%#FormatName( Eval("Parent"))%>" href="#options_<%#FormatName( Eval("Key"))%>"></a> </div>
                                    </div>
                                    <div id="options_<%#FormatName( Eval("Key"))%>" class="panel-collapse collapse <%#(Container.ItemIndex==1)?"in":""%>">
                                      <div class="panel-body">
                                        <div class="form-horizontal  form-patch">
                                            <asp:Repeater ID="RepeaterOptions" runat="server" OnItemDataBound="RepeaterOptions_ItemDataBound">
                                                <ItemTemplate>
                                                       <div class="form-group">
                                                        <asp:Literal ID="liTitle" runat="server"></asp:Literal>
                                                        <div class="controls col-sm-9">
                                                            <asp:PlaceHolder ID="ThemePH" runat="server"></asp:PlaceHolder>
                                                            <asp:Literal ID="liHelp" runat="server"></asp:Literal>
                                                        </div>
                                                      </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            </div>
                                         </div>
                                    </div>
                                  </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                  </div>
                 </ItemTemplate>
                </asp:Repeater>
      
            <div class="row"> 
                <div class="col-sm-12"> 

                  <div class="form-group">
                         <asp:Button CssClass="btn btn-primary" lang="Submit" ID="cmdUpdate" resourcekey="cmdUpdate"
                            runat="server" Text="Update" OnClick="cmdUpdate_Click"></asp:Button>&nbsp;
                        <asp:Button CssClass="input_button btn" lang="Submit" ID="cmdReset" resourcekey="cmdReset" runat="server"
                            Text="Reset" OnClick="cmdReset_Click"></asp:Button>&nbsp;
                        
                        
                        </div>
                  </div>
            </div>

            </div>





          </div>






          <div class="col-sm-4">
          
           <ul id="myTab_ul_tabs_Right" class="myTab_ul_tabs ul_tabs nav nav-tabs tab-bricky">
              <asp:Literal runat="server" ID="liNavTabsHTML_Right"></asp:Literal>
            </ul>
            <div class="tab-content tab_right">
              
                <asp:Repeater ID="RepeaterCategories_Right" runat="server" OnItemDataBound="RepeaterCategories_ItemDataBound">
                    <ItemTemplate>

                     <div class="tab-pane <%#(Container.ItemIndex==1)?"in active":""%>" id="tabs-Right-<%#FormatName( Eval("Key"))%>">
                        <div id="accordion<%#FormatName( Eval("Key"))%>">
                          <asp:Repeater ID="RepeaterGroup" runat="server" OnItemDataBound="RepeaterGroup_ItemDataBound">
                            <ItemTemplate>
                                 <div class="panel panel-default small-bottom">
                                    <div class="panel-heading"> <i class="fa fa-external-link-square"></i> <%# Eval("Key")%>
                                      <div class="panel-tools"> <a class="btn btn-xs btn-link panel-collapse collapses" data-toggle="collapse" data-parent="#accordion<%#FormatName( Eval("Parent"))%>" href="#options_<%#FormatName( Eval("Key"))%>"></a> </div>
                                    </div>
                                    <div id="options_<%#FormatName( Eval("Key"))%>" class="panel-collapse collapse <%#(Container.ItemIndex==1)?"in":""%>">
                                      <div class="panel-body">
                                        <div class="form-horizontal  form-patch">
                                            <asp:Repeater ID="RepeaterOptions" runat="server" OnItemDataBound="RepeaterOptions_ItemDataBound">
                                                <ItemTemplate>
                                                       <div class="form-group">
                                                        <asp:Literal ID="liTitle" runat="server"></asp:Literal>
                                                        <div class="controls col-sm-9">
                                                            <asp:PlaceHolder ID="ThemePH" runat="server"></asp:PlaceHolder>
                                                            <asp:Literal ID="liHelp" runat="server"></asp:Literal>
                                                        </div>
                                                      </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            </div>
                                         </div>
                                    </div>
                                  </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                  </div>
                 </ItemTemplate>
                </asp:Repeater>
             
 

            </div>





          </div>
          
        </div>

        <!-- end: PAGE CONTENT--> 


<script type="text/javascript">
    jQuery(function ($) {
        $("#myTab_ul_tabs_Left li a").click(function () { $.cookie("RepeaterCategories_Left", $(this).attr("href")); $.cookie("RepeaterGroup_Left", ""); });
        $(".tab_left").find("div.panel-tools a.panel-collapse").click(function () { $.cookie("RepeaterGroup_Left", $(this).attr("href")); });

        var Categories_Left = $.cookie("RepeaterCategories_Left");
        var Group_Left = $.cookie("RepeaterGroup_Left");

        if (Categories_Left != undefined && Categories_Left !== "") {
            $(".tab_left").find("div.tab-pane").removeClass("in").removeClass("active");
            $(".tab_left").find(Categories_Left).addClass("in").addClass("active");
            $("#myTab_ul_tabs_Left li").removeClass("active");
            $("#myTab_ul_tabs_Left li a[href='" + Categories_Left + "']").parent().addClass("active");

        }

        if (Group_Left != undefined && Group_Left !== "") {
            $(".tab_left").find(Group_Left).parents("div.tab-pane").find("div.panel-collapse").removeClass("in");
            $(".tab_left").find(Group_Left).addClass("in");
        }

    });
</script>