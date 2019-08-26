<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_TabPictures.ascx.cs"
    Inherits="DNNGo.Modules.ThemePlugin.Manager_TabPictures" %>
    <script src="<%=ModulePath %>Resource/plugins/nestable/jquery.nestable.js"></script>

            <div class="tabbable">
          <ul id="myTab4" runat="server" class="nav nav-tabs tab-bricky">
            <li id="liNavigation_PictureList" runat="server"><asp:HyperLink ID="hlNavigation_PictureList" runat="server" Text="<i class='fa clip-list-4'></i> Picture list" ></asp:HyperLink> <%--<a href="#panel_tab3_example1"> Add pictures </a> --%></li>
            <li id="liNavigation_PictureSort" runat="server"> <asp:HyperLink ID="hlNavigation_PictureSort" runat="server" Text="<i class='fa fa-sort'></i> Picture sort" ></asp:HyperLink><%--<a href="#panel_tab3_example2" > Picture list </a>--%> </li>
          </ul>
          <div class="tab-content">
            <asp:Panel ID="PanelPictureList" runat="server" CssClass="tab-pane">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <i class="fa fa-external-link-square"></i>
                        <%=ViewResourceText("Title_Picture", "Pictures")%>
                        <div class="panel-tools">
                            <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                        </div>
                    </div>
                    <div class="panel-body buttons-widget">
                        <div class="row form-group">
                            <div class="col-sm-1"></div>
                            <div class="col-sm-10">
                                 <asp:HyperLink runat="server" ID="hlPictures" resourcekey="hlPictures" 
                                    data-toggle="modal" NavigateUrl="#Pictures_Modal" Text="<i class='fa clip-images'></i> Set Pictures"
                                    ToolTip="Set Pictures" CssClass="btn btn-bricky"></asp:HyperLink>
                                <asp:HiddenField runat="server" ID="hfPictures" />
                                 <div class="form-group">
                                    <table id="ul_albums" class="ul_albums table table-striped table-bordered table-hover">
                                          <tbody>
                                            <asp:Literal ID="liPictures" runat="server"></asp:Literal>
                                         </tbody>
                                    </table>
                               </div>
                                <div id="Pictures_Modal" class="modal fade" tabindex="-1" data-width="820"
                                    data-height="400" style="display: none;">
                                    <div class="modal-header">
                                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                                            &times;
                                        </button>
                                        <h4 class="modal-title">
                                            <i class='fa fa-folder-open'></i> Set Picture Pictures</h4>
                                    </div>
                                    <div class="modal-body">
                                        <iframe id="Pictures_Iframe" width="100%" height="100%" style="border-width: 0px;">
                                        </iframe>
                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>
                </div>

                 <div class="panel panel-default" id="div_GlobalSettings" runat="server">
                    <div class="panel-heading">
                        <i class="fa fa-external-link-square"></i><%=ViewResourceText("Title_GlobalSettings", "Global Settings")%>
                        <div class="panel-tools">
                            <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                        </div>
                    </div>
                    <div class="panel-body buttons-widget">
                         <div class="row form-group">
                            <%=ViewControlTitle("lblUseGlobalSettings", "Use Global Settings", "rblUseGlobalSettings", ":", "col-sm-3 control-label")%>
                            <div class="col-sm-7">
                                  <asp:RadioButtonList ID="rblUseGlobalSettings" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
                            </div>
                        </div>
                    </div>
                </div>




            </asp:Panel>
            <asp:Panel ID="PanelPictureSort" runat="server" CssClass="tab-pane">
                  
                 <div class="panel panel-default">
                    <div class="panel-heading"> <i class="fa fa-external-link-square"></i> <%=ViewResourceText("Title_PictureSort", "Picture Sort")%>
                      <div class="panel-tools"> <a href="#" class="btn btn-xs btn-link panel-collapse collapses"> </a> </div>
                    </div>
                    <div class="panel-body">
                         <div class="form-horizontal">
                            <div class="dd" id="nestable">
                                <ol class="dd-list">
                                    <asp:Repeater ID="RepeaterFields" runat="server" OnItemDataBound="RepeaterFields_ItemDataBound">
                                        <ItemTemplate>
                                            <li class="dd-item dd3-item" data-ID="<%#Eval("ID")%>">
                                                <div class="dd-handle dd3-handle">
                                                </div>
                                                <div class="dd3-content">
                                                   <div class="row">
                                                        <div class="col-sm-3 hidden-xs"><asp:Image ID="imgPicture" runat="server"  Height="20" /> </div>
                                                        <div class="col-sm-8">Name:<asp:Literal ID="liName" runat="server"></asp:Literal></div>
                                                        
                                                   </div>
                                     
                                                </div>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ol>
                            </div>
                            <asp:HiddenField ID="nestable_output" runat="server"  />
                        </div>
                    </div>                    
                  </div>
            </asp:Panel>
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

    function ShowAlbums(PictureUrls, PictureIDs, PictureNames) {
        var IDs = jQuery('#<%=hfPictures.ClientID %>').val();
        for (var i = 0; i < PictureIDs.length; i++) {
            var id = PictureIDs[i];
            var Urls = PictureUrls[i];
            var Names = PictureNames[i];

            if (IDs !== "" && IDs.indexOf(id + ",") >= 0) {
                continue;
            }
            $("#ul_albums").append("<tr data-value='" + id + "'><td>" + Names + "<br/><img src='" + Urls + "' style='max-width:120px; margin-right:15px;'/></td><td class='center'><a data-value='" + id + "' class='btn btn-xs btn-bricky tooltips' href='javascript:;' data-placement='top' data-original-title='Delete'><i class='fa fa-times fa fa-white'></i></a></td></tr>");
            //$("#ul_albums").append("<li data-value='" + id + "'><img  src='" + Urls + "'/> <a data-value='" + id + "' href='javascript:;' class='btn btn-xs btn-bricky'><i class='fa fa-trash-o'></i> Remove</a> </li>");
            IDs += (id + ",");
        }
        jQuery('#<%=hfPictures.ClientID %>').val(IDs);

        $("#ul_albums tr a").click(function () {
            var id = $(this).attr("data-value");
            $("#ul_albums tr[data-value='" + id + "']").hide("fast", function () {
                $(this).remove();
            });
            jQuery('#<%=hfPictures.ClientID %>').val(jQuery('#<%=hfPictures.ClientID %>').val().replace(id + ",", ""));
        });
    }


    jQuery(function ($) {
        $("#<%=hlPictures.ClientID %>").click(function () { $("#Pictures_Iframe").attr("src", $(this).attr("data-href")); });
        $("#ul_albums tr a").click(function () {
            var id = $(this).attr("data-value");
            $("#ul_albums tr[data-value='" + id + "']").hide("fast", function () {
                $(this).remove();
            });
            jQuery('#<%=hfPictures.ClientID %>').val(jQuery('#<%=hfPictures.ClientID %>').val().replace(id + ",", ""));
        });

    });



</script>

<script type="text/javascript">
    var UINestable = function () {
        //function to initiate jquery.nestable
        var updateOutput = function (e) {
            var list = e.length ? e : $(e.target),
            output = list.data('output');
            if (window.JSON) {
                output.val(window.JSON.stringify(list.nestable('serialize')));
                //, null, 2));
            } else {
                output.val('JSON browser support required for this demo.');
            }
        };
        var runNestable = function () {
            // activate Nestable for list 1
            $('#nestable').nestable({
                maxDepth: 1
            }).on('change', updateOutput);
            // output initial serialised data
            updateOutput($('#nestable').data('output', $('#<%=nestable_output.ClientID %>')));

        };
        return {
            //main function to initiate template pages
            init: function () {
                runNestable();
            }
        };
    } ();


    jQuery(document).ready(function () {
        UINestable.init();
    });
</script>
