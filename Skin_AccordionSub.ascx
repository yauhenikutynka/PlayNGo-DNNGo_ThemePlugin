<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Skin_AccordionSub.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Skin_AccordionSub" %>
<style>
ul.sub{ display:none;}
</style>


<asp:Literal ID="LiContent" runat="server"></asp:Literal>


<script type="text/javascript">
    jQuery().ready(function ($) {
        $("#gomenu<%=ClientID %> li.dir a").click(function () {
            $(this).next("ul").slideToggle("normal");
        });
    });
</script>
 
 