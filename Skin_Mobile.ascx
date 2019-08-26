<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Skin_Mobile.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Skin_Mobile" %>
<a href="#gomenu<%=ClientID %>" class="mobilemenu_close">X</a>
<div id="gomenu<%=ClientID %>" class="mobile_menu <%=CssClass %>">
    <asp:Literal ID="LiContent" runat="server"></asp:Literal>
</div>
 
 <script type="text/javascript">
     jQuery(document).ready(function ($) {


		 $('#gomenu<%=ClientID %>').find(".dir.mm-selected").removeClass("mm-selected").parent().parent().addClass("mm-selected")
		 
         $('#gomenu<%=ClientID %>').mobile_menu({
             slidingSubmenus: <%=slidingSubmenus %>,
             counters: <%=counters %>,
             navbartitle: "<%=navbartitle %>",
             headerbox: "<%=headerbox %>",
             footerbox: "<%=footerbox %>"			 
         });
		 
		  $('#gomenu<%=ClientID %>').find("a[href='javascript:;']").on("click",function () {
			  $(this).siblings(".mm-next").click();
		  })
     });
</script>