<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Skin_VSlide.ascx.cs"  Inherits="DNNGo.Modules.ThemePlugin.Skin_VSlide" %>
<div id="gomenu<%=ClientID %>" class="gomenu <%=CssClass %>">
    <asp:Literal ID="LiContent" runat="server"></asp:Literal>
</div>
<script type="text/javascript">
    DnnDevNamespace.ItemFlow = { H: 1, Horizontal: 1 };
    window["gomenu<%=ClientID %>"] = DnnDev.Create("gomenu<%=ClientID %>"); window["gomenu<%=ClientID %>"].Initialize({ "Enabled": true }, {});
</script>
