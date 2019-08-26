<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Skin_DropDownList.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Skin_DropDownList" %>
<div id="gomenu<%=ClientID %>" class="gomenu <%=CssClass %>">
   <asp:DropDownList ID="ddlSelectMenu" CssClass="select_menu" runat="server"></asp:DropDownList>
</div>
<script type="text/javascript">
    jQuery().ready(function ($) {
        $("#<%=ddlSelectMenu.ClientID %>").change(function () {
            var _val = $(this).val();
            if (_val != '' && _val != 'javascript:;') {
                window.location.href = $(this).val();
            }
        });
    });
</script>