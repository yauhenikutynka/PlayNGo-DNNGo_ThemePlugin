<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_SkinOptions.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Manager_SkinOptions" %>
<!-- start: PAGE HEADER -->
<div class="row">
    <div class="col-sm-12">
        <!-- start: PAGE TITLE & BREADCRUMB -->
        <div class="page-header">
            <h1>
                <i class="fa fa-plus"></i> <%=ViewResourceText("Header_Title", "Skin Options")%>
                <%--<small>overview &amp; stats </small>--%></h1>
        </div>
        <!-- end: PAGE TITLE & BREADCRUMB -->
    </div>
</div>
<!-- end: PAGE HEADER -->
<style type="text/css">
.panel-default[class^="Header"]{
	display:none;
}
.panel-default.HeaderStyle{
	display:block; 
}
.radio-img-list[id^="Ctl_HeaderType"]{
	display:none; 
}
.panel-default[class^="Breadcrumb"]{
	display:none;
}
.panel-default.BreadcrumbStyle {
	display:block; 
}
.radio-img-list[id^="Ctl_Breadcrumbstyle"],
.iboxfixed .radio-img-list input{
	display:none; 
}

.panel-default[class^="PageTitle"]{
	display:none;
}
.panel-default.PageTitleStyle{
	display:block; 
}
.radio-img-list[id^="Ctl_PageTitleStyle"],
.iboxfixed .radio-img-list input{
	display:none; 
}

.panel-default[class^="Footer"]{
	display:none;
}
.panel-default.FooterStyle{
	display:block; 
}
.radio-img-list[id^="Ctl_FooterType"]{
	display:none; 
}

.iboxfixed{
	background-color:rgba(255,255,255,0.7);
	position:fixed;
	z-index:1003;
	left:0;
	top:0;
	bottom:0;
	right:0;
	text-align:center;
	display:none;
}
.iboxfixed .radio-img-list{
	display:block;
	max-width:50%;
	max-height:80%;
	overflow:auto;
	display:inline-block;
	vertical-align:middle;
	text-align:left;
	background-color:#FFF;
	background-color: #ffffff;
	border: 1px solid #999999;
	border: 1px solid rgba(0, 0, 0, 0.2);
	border-radius: 6px;
	-webkit-box-shadow: 0 3px 9px rgba(0, 0, 0, 0.5);
	box-shadow: 0 3px 9px rgba(0, 0, 0, 0.5);
}
.iboxfixed .radio-img-list img{
	max-width:100%;
}
.iboxfixed:before{
	content:"";
	display:inline-block;
	height:100%;
	vertical-align:middle;
}
.iboxfixed .radio-img-list h3{
	margin:0 0 20px;
	font-size:18px;
    min-height: 16.428571429px;
    padding: 15px;
    border-bottom: 1px solid #e5e5e5;
}
.iboxfixed .radio-img-list .iradio_minimal-grey{
	display:none;
}
.iboxfixed .radio-img-list .radio-inline {
	margin:10px !important;
	padding:0;
	position:relative;
}
.radio-img-list .m-checked,
.thumb.m-checked{
	position:relative;
	cursor:pointer;
}
.radio-img-list .m-checked:before{
	content:"";
	position:absolute;
	top:-8px;
	left:-8px;
	right:-8px;
	bottom:-8px;
	border:4px solid #1d88c3;
	opacity:0.8;
}
.radio-img-list .m-checked:after{
	content:"\f00c";
	position:absolute;
	width:20px;
	color:#FFFFFF;
    padding: 4px 0px;
	text-align:center;
	background-color:#1d88c3;
	right:0;
	top:0;
    font: normal normal normal 14px/1 FontAwesome;
}
.iboxfixed .radio-img-list h4{
	position:absolute;
	top:0px;
	left:0px;
	color:#FFF;
	background-color:rgba(0,0,0,0.5);
	padding:5px;
	margin:0;
	font-size:14px;
}
.thumb.m-checked .Edit{
	display:inline-block;
	margin-top:10px;
}

</style>



        <div class="row"> 
          <!-- start: PAGE CONTENT -->
          <div class="col-sm-9">
            <ul id="myTab_ul_tabs" class="ul_tabs nav nav-tabs tab-bricky">
              <asp:Literal runat="server" ID="liNavTabsHTML"></asp:Literal>
            </ul>
            <div class="tab-content">

                <asp:Repeater ID="RepeaterCategories" runat="server" OnItemDataBound="RepeaterCategories_ItemDataBound">
                    <ItemTemplate>

                     <div class="tab-pane <%#FormatName( Eval("Key"))%> <%#(Container.ItemIndex==1)?"in active":""%>" id="tabs-left-<%#FormatName( Eval("Key"))%>">
                        <div id="accordion<%#FormatName( Eval("Key"))%>">
                          <asp:Repeater ID="RepeaterGroup" runat="server" OnItemDataBound="RepeaterGroup_ItemDataBound">
                            <ItemTemplate>
                                 <div class="<%#FormatName( Eval("Key"))%> panel panel-default small-bottom">
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
                                                        <div class="controls col-sm-9 ">
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






          <div class="col-sm-3">
          	<h4 style="margin:0 0 10px 0;">Skin Lists
          </h4>
            <div id="accordion">

            <asp:Repeater ID="RepeaterSkins" runat="server" OnItemDataBound="RepeaterSkins_ItemDataBound">
                    <ItemTemplate>

                      <div class="panel panel-default small-bottom ">
                        <div class="panel-heading"> <i class="fa fa-external-link-square"></i> <%#Eval("Name")%>
                          <div class="panel-tools"> <a class="btn btn-xs btn-link panel-collapse <asp:Literal ID="LiExpand" runat="server"></asp:Literal>" data-toggle="collapse" data-parent="#accordion" href="#collapse_Skin<%#Container.ItemIndex %>"></a> </div>
                        </div>
                        <div id="collapse_Skin<%#Container.ItemIndex %>" class="panel-collapse collapse <asp:Literal ID="liActive" runat="server"></asp:Literal>">
                          <div class="panel-body">
                          
                            <p class="fa-border"> <asp:Literal ID="liThumbnails" runat="server"></asp:Literal></p>
                            <p>
                             <asp:HyperLink ID="hlOptions" Text="Options" resourcekey="hlOptions"   runat="server" CssClass="btn btn-primary btn-sm"></asp:HyperLink>
                            &nbsp;&nbsp;
                             <asp:HyperLink ID="hlCopy" Text="Copy" resourcekey="hlCopy"   runat="server" CssClass="btn btn-primary btn-sm"></asp:HyperLink>
                          </div>
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
		
		function linkage(e,box,hbox,sbox){
			
				if(e.length==0){return false;}
			
			var thumb =$("<div class=\"thumb m-checked\">").html(e.find("input[checked='checked']").siblings("img").clone());
				thumb.append("<a class=\"Edit\">Edit</a>")
				$(box.replace("{0}",e.find("input[checked='checked']").val())).show()
				e.before(thumb);
				e.prepend($("<h3>"+e.parent().siblings(".control-label").html()+"<span class=\"close listclose\"> × </span></h3>"));
				e.wrap("<div class=\"iboxfixed\"></div>");
			var iboxfixed=e.parent(".iboxfixed");
				e.find("input[checked='checked']").parent().addClass("m-checked");
				e.find(".radio-inline").each(function(index, element) {
					$(this).append("<h4>"+hbox+" "+$(this).find("input").val()+"</h4>")
                    
                });
				e.append("<div class=\"modal-footer\"><button type=\"button\" data-dismiss=\"modal\" class=\"btn btn-light-grey listclose\"> Close </button></div>");
				
				e.find(".radio-inline").on("click",function(){ 
					$(this).addClass("m-checked").siblings().removeClass("m-checked")
					thumb.html($(this).find("img").clone());
					thumb.append("<a class=\"Edit\">Edit</a>")
					nbox=$(box.replace("{0}",$(this).find("input").val()))
					nbox.show().siblings("[class^=\""+hbox+"\"]").hide();
					$(sbox).show();
					iboxfixed.fadeOut(function(){
						nbox.find(".panel-heading .panel-tools .btn").click()
					});
				})
				e.find(".listclose").on("click",function(){
					iboxfixed.fadeOut();
				})
				thumb.on("click",function(){
					iboxfixed.fadeIn();
				})
				
				$(".iboxfixed").on("click",function(e){
					if(!( $(e.target).closest(".radio-img-list").length != 0 || jQuery.contains($(".radio-img-list")[0], e.target) )) {
						iboxfixed.fadeOut();	
					}
				})
				
		}
		
		linkage($(".panel-default.HeaderStyle .radio-img-list"),".Header{0}Style","Header",".HeaderStyle")
		linkage($(".panel-default.BreadcrumbStyle .radio-img-list"),".Breadcrumb{0}","Breadcrumb",".BreadcrumbStyle")
		linkage($(".panel-default.PageTitleStyle .radio-img-list"),".PageTitle{0}","PageTitle",".PageTitleStyle ")
		linkage($(".panel-default.FooterStyle .radio-img-list"),".Footer{0}Style","Footer",".FooterStyle")

   });


    jQuery(function ($) {
        $("#myTab_ul_tabs li a").click(function () { $.cookie("RepeaterCategories", $(this).attr("href")); $.cookie("RepeaterGroup", ""); });
        $("div.panel-tools a.panel-collapse").click(function () { $.cookie("RepeaterGroup", $(this).attr("href")); });

        var Categories = $.cookie("RepeaterCategories");
        var Group = $.cookie("RepeaterGroup");

        if (Categories != undefined && Categories !== "") {
            $("div.tab-pane").removeClass("in").removeClass("active");
            $(Categories).addClass("in").addClass("active");
            $("#myTab_ul_tabs li").removeClass("active");
            $("#myTab_ul_tabs li a[href='" + Categories + "']").parent().addClass("active");

        }

        if (Group != undefined && Group !== "") {
            $(Group).parents("div.tab-pane").find("div.panel-collapse").removeClass("in");
            $(Group).addClass("in");
        }

    });
	
	
</script>