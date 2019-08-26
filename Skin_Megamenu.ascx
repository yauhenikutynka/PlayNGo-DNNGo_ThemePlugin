<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Skin_Megamenu.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Skin_Megamenu" %>
    <div class="dnngo_gomenu" id="dnngo_megamenu<%=MenuID %>">
      
        <asp:PlaceHolder ID="phContentHTML" runat="server"></asp:PlaceHolder>
      
    </div> 
    
    
<script type="text/javascript">
    jQuery(document).ready(function ($) {
        $("#dnngo_megamenu<%=MenuID %>").dnngomegamenu({
            slide_speed: <%=slide_speed %>,
            delay_disappear: <%=delay_disappear %>,
            popUp: "<%=popUp %>",//level
			delay_show:150,
			direction:"<%=direction %>",//rtl ltr
			megamenuwidth:"<%=megamenuwidth %>",//full box
			WidthBoxClassName:"<%=WidthBoxClassName %>"
        });
    });


	jQuery(document).ready(function () {
		jQuery("#dnngo_megamenu<%=MenuID %>").has("ul").find(".dir > a").attr("aria-haspopup", "true");
	}); 

</script>