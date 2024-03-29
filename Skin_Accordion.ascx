﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Skin_Accordion.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Skin_Accordion" %>
<asp:Literal ID="LiContent" runat="server"></asp:Literal>
<script type="text/javascript">
    jQuery().ready(function ($) {
        var activeindex = false;
        $.each($('#gomenu<%=ClientID %>>li'), function (i) {
            if ($(this).attr("class").indexOf('current') != -1) {
                activeindex = i;
            }
        });
        jQuery('#gomenu<%=ClientID %>').accordion({
            header: '.head',
            collapsible: true,
            event: 'mouseover',
            active: activeindex,
            autoHeight: <%=autoHeight %>,
            animated: '<%=animated %>',
            toggle: '<%=toggle %>'
        });
    });
</script>