<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Skin_HSlide.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Skin_HSlide" %>
<div id="gomenu<%=ClientID %>" class="gomenu <%=CssClass %>">
    <asp:Literal ID="LiContent" runat="server"></asp:Literal>
</div>
<script type="text/javascript">
    window["gomenu<%=ClientID %>"] = DnnDev.Create("gomenu<%=ClientID %>"); window["gomenu<%=ClientID %>"].Initialize({ "Enabled": true }, {});
	jQuery(document).ready(function () {
		jQuery("#gomenu<%=ClientID %>").has("ul").find(".dir > a").attr("aria-haspopup", "true");
	}); 
    </script>
