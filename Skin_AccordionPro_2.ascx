﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Skin_AccordionPro_2.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Skin_AccordionPro_2" %>
 
<asp:Literal ID="LiContent" runat="server"></asp:Literal>
 
<script type="text/javascript">
    jQuery().ready(function ($) {

        function HoverOver<%=ClientID %>() {

            $(this).addClass('rmhover').find(".subMenu:first").stop().animate({ height: 'show' }, <%=AccordionPro_AnimateSpeed %>);
        }

        function HoverOut<%=ClientID %>() {
            $(this).removeClass('rmhover').find(".subMenu:first").animate({ height: 'hide' }, <%=AccordionPro_AnimateSpeed %>, function () {
                $(this).hide();
            });
        }

        function HoverOversub<%=ClientID %>() {
            $(this).find(".subMenu:first").stop().animate({ height: 'show' }, <%=AccordionPro_AnimateSpeed %>);
        }

        function HoverOutsub<%=ClientID %>() {
            $(this).find(".subMenu:first").stop().animate({ height: 'hide' }, <%=AccordionPro_AnimateSpeed %>, function () {
                $(this).hide();
            });
        }

        var config<%=ClientID %> = {
            sensitivity: <%=AccordionPro_Sensitivity  %>,
            interval: <%=AccordionPro_Interval %>,
            over: HoverOver<%=ClientID %>,
            timeout: <%=AccordionPro_Timeout %>,
            out: HoverOut<%=ClientID %>
        };
        var configsub<%=ClientID %> = {
            sensitivity: <%=AccordionPro_Sensitivity  %>,
            interval: <%=AccordionPro_Interval %>,
            over: HoverOversub<%=ClientID %>,
            timeout: <%=AccordionPro_Timeout %>,
            out: HoverOutsub<%=ClientID %>
        };
        //$("#gomenu<%=ClientID %> li.dir").hoverIntent(config<%=ClientID %>);
        $("#gomenu<%=ClientID %> .subMenu li.dir").hoverIntent(configsub<%=ClientID %>);
        $("#gomenu<%=ClientID %>").find("li.dir").each(function (i) {
             var d= $(this);
             $(d).find(".btn_click").click( function () { 
                $(d).find(".subMenu").slideToggle("fast",function(){
                    if($(this).css("display") == "none")
                    {
                        $(d).find(".btn_click").addClass("btn_down").removeClass("btn_up");
                    }else
                    {
                        $(d).find(".btn_click").addClass("btn_up").removeClass("btn_down");
                    }
                 });
 
             });
         
      
        });
    });
</script>
 
 