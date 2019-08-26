<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Skin_Background.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Skin_Background" %>
<asp:Literal ID="liHTML" runat="server"></asp:Literal>
<script type="text/javascript">
    jQuery(document).ready(function ($) {
        $("#<%=SkinClientID %>").phototabs({
            switchtime: <%=Switchtime %>,
            animationtime: <%=Animationtime %>,
            startpic: <%=Startpic %>,
            autoPaly: <%=autoPaly %>,
            showArrow: <%=showArrow %>
        });
    });
</script>
<%--
CssClass="pic_tab"
Token = "Background":"Breadcrumb" 
Switchtime="5000"
Animationtime="1000"
Startpic="0"
autoPaly="true"
showArrow="true"
--%>