<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View_Index.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.View_Index" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<asp:Panel ID="plLicense" runat="server">
<asp:PlaceHolder ID="phScript" runat="server"></asp:PlaceHolder>


<div class="validationEngineContainer form_div_<%=ModuleId %>" >
    <asp:PlaceHolder  ID="phContainer" runat="server"></asp:PlaceHolder>
    
 </div>

 

</asp:Panel>
<asp:Panel ID="pnlTrial" runat="server">
    <center>
        <asp:Literal ID="litTrial" runat="server"></asp:Literal>
        <asp:Label ID="lblMessages" runat="server" CssClass="SubHead" resourcekey="lblMessages"
            Visible="false" ForeColor="Red"></asp:Label>
    </center>
</asp:Panel>


<asp:Panel ID="panNavigation" runat="server"  CssClass="Navigation">
    <asp:HyperLink ID="hlManager" runat="server" CssClass="ThemePlugin_Manager" Text="<i class='icon-file'></i> Manager" resourcekey="Actions_Manager" Target="_blank"></asp:HyperLink>
</asp:Panel>

