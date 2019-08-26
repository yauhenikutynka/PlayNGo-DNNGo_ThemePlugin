<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_TabMegamenu.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Manager_TabMegamenu" %>
 
 <div class="row">
    <div class="col-sm-12">
 <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i><%=ViewResourceText("Title_MenuType", "Menu Type")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body buttons-widget">
                 <div class="row form-group">
                    <%=ViewControlTitle("lblTabType", "Tab Type", "ddlTabType", ":", "col-sm-3 control-label")%>
                    <div class="col-sm-7">
                             <asp:DropDownList  ID="ddlTabType" runat="server" CssClass="form-control form_default"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
 
 
 <div class="row">
            <div class="col-sm-12">
              <div class="panel panel-default">
                <div class="panel-heading"> <i class="fa fa-external-link-square"></i>Mega Menu Top
                  <div class="panel-tools "> 
                    <asp:HyperLink runat="server" ID="hlMegamenuModule_Top" resourcekey="hlMegamenuModule" data-toggle="modal" NavigateUrl="#MegamenuModule_Modal" Text="<i class='fa fa-edit'></i> Add" ToolTip="Set Content" CssClass="btn btn-xs btn-primary"></asp:HyperLink>    
                   </div>
                </div>
                <div class="panel-body form-horizontal">

                     <asp:Repeater ID="RepeaterOptions_Top" runat="server" OnItemDataBound="RepeaterOptions_ItemDataBound">
                        <ItemTemplate>
                         <div class="form-group">
                            <asp:Literal ID="liTitle" runat="server"></asp:Literal>
                            <div class="col-sm-7">
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
          <div class="row">
            <div class="col-sm-4">
              <div class="panel panel-default">
                <div class="panel-heading"> <i class="fa fa-external-link-square"></i>Mega Menu Left
                  <div class="panel-tools"> 
                        <asp:HyperLink runat="server" ID="hlMegamenuModule_Left" resourcekey="hlMegamenuModule" data-toggle="modal" NavigateUrl="#MegamenuModule_Modal" Text="<i class='fa fa-edit'></i> Add" ToolTip="Set Content" CssClass="btn btn-xs btn-primary"></asp:HyperLink>    
                  
                   </div>
                </div>
                <div class="panel-body form-horizontal">
                     <asp:Repeater ID="RepeaterOptions_Left" runat="server" OnItemDataBound="RepeaterOptions_ItemDataBound">
                        <ItemTemplate>
                         <div class="form-group">
                            <asp:Literal ID="liTitle" runat="server"></asp:Literal>
                            <div class="col-sm-7">
                                <asp:PlaceHolder ID="ThemePH" runat="server"></asp:PlaceHolder>
                                <asp:Literal ID="liHelp" runat="server"></asp:Literal> 
                            </div>
                          </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
              </div>
            </div>
            <div class="col-sm-4">
              <div class="panel panel-default">
                <div class="panel-heading"> <i class="fa fa-external-link-square"></i>Mega Menu Center
                  <div class="panel-tools"> </div>
                </div>
                <div class="panel-body form-horizontal">
                     <asp:Repeater ID="RepeaterOptions_Center" runat="server" OnItemDataBound="RepeaterOptions_ItemDataBound">
                        <ItemTemplate>
                         <div class="form-group">
                            <asp:Literal ID="liTitle" runat="server"></asp:Literal>
                            <div class="col-sm-7">
                                <asp:PlaceHolder ID="ThemePH" runat="server"></asp:PlaceHolder>
                                <asp:Literal ID="liHelp" runat="server"></asp:Literal> 
                            </div>
                          </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
              </div>
            </div>
            <div class="col-sm-4">
              <div class="panel panel-default">
                <div class="panel-heading"> <i class="fa fa-external-link-square"></i>Mega Menu Right
                  <div class="panel-tools"> 
                    <asp:HyperLink runat="server" ID="hlMegamenuModule_Right" resourcekey="hlMegamenuModule" data-toggle="modal" NavigateUrl="#MegamenuModule_Modal" Text="<i class='fa fa-edit'></i> Add" ToolTip="Set Content" CssClass="btn btn-xs btn-primary"></asp:HyperLink>    
                  </div>
                </div>
                <div class="panel-body form-horizontal">
                     <asp:Repeater ID="RepeaterOptions_Right" runat="server" OnItemDataBound="RepeaterOptions_ItemDataBound">
                        <ItemTemplate>
                         <div class="form-group">
                            <asp:Literal ID="liTitle" runat="server"></asp:Literal>
                            <div class="col-sm-7">
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
          <div class="row">
            <div class="col-sm-12">
              <div class="panel panel-default">
                <div class="panel-heading"> <i class="fa fa-external-link-square"></i>Mega Menu Bottom
                  <div class="panel-tools"> 
                        <asp:HyperLink runat="server" ID="hlMegamenuModule_Bottom" resourcekey="hlMegamenuModule" data-toggle="modal" NavigateUrl="#MegamenuModule_Modal" Text="<i class='fa fa-edit'></i> Add" ToolTip="Set Content" CssClass="btn btn-xs btn-primary"></asp:HyperLink>    
                  </div>
                </div>
                <div class="panel-body form-horizontal">
                     <asp:Repeater ID="RepeaterOptions_Bottom" runat="server" OnItemDataBound="RepeaterOptions_ItemDataBound">
                        <ItemTemplate>
                         <div class="form-group">
                            <asp:Literal ID="liTitle" runat="server"></asp:Literal>
                            <div class="col-sm-7">
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




 <div class="row">
        <div class="col-sm-3"> </div>
        <div class="col-sm-9">
            
         <asp:Button CssClass="btn btn-primary" lang="Submit" ID="cmdUpdate" resourcekey="cmdUpdate"
        runat="server" Text="Update" OnClick="cmdUpdate_Click"></asp:Button>&nbsp;
        <asp:Button CssClass="btn btn-default" ID="cmdCancel" resourcekey="cmdCancel" runat="server"
            Text="Cancel" CausesValidation="False" OnClick="cmdCancel_Click"  OnClientClick="CancelValidation();"></asp:Button>&nbsp;

         </div>
      </div>








 <div id="MegamenuModule_Modal" class="modal fade" tabindex="-1" data-width="820" data-height="400" style="display: none;">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                &times;
            </button>
            <h4 class="modal-title">
                <i class='fa fa-folder-open'></i> Set Content</h4>
        </div>
        <div class="modal-body">
            <iframe id="MegamenuModule_Iframe" width="100%" height="100%" style="border-width: 0px;">
            </iframe>
        </div>
 </div>

 

<script type="text/javascript">

    jQuery(function ($) {
        $("a[href='#MegamenuModule_Modal']").click(function () { $("#MegamenuModule_Iframe").attr("src", $(this).attr("data-href")); });
    });
</script>

