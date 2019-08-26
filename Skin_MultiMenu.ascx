<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Skin_MultiMenu.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Skin_MultiMenu" %>
<div class="menu_main">
  <div id="<%=MultiMenuAction %>_menu<%=ClientID %>" class="<%=MultiMenuAction %>_menu">
<asp:Literal ID="LiContent" runat="server"></asp:Literal>
   </div>
</div>



<script type="text/javascript">
    jQuery(function ($) {
        $("#<%=MultiMenuAction %>_menu<%=ClientID %>").accordionpro<%=MultiMenuAction %>({
            accordion: true,
            speed: 300,
            closedSign: '+',
            openedSign: '-'
        });
    }); 
</script>
