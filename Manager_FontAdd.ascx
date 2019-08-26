<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_FontAdd.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Manager_FontAdd" %>

      <!-- start: PAGE HEADER -->
      <div class="row">
        <div class="col-sm-12"> 
          <!-- start: PAGE TITLE & BREADCRUMB -->
           
          <div class="page-header">
            <h1><i class="fa fa-link"></i> <%=ViewResourceText("Header_Title", "Add Google Font")%> 
                <%--<asp:HyperLink ID="hlReturnFonts" runat="server" CssClass="btn btn-xs btn-default"  Text="<i class='fa fa-font'></i> Return Fonts " resourcekey="hlReturnFonts" ></asp:HyperLink>--%>
            </h1>
          </div>
          <!-- end: PAGE TITLE & BREADCRUMB --> 
        </div>
      </div>
      <!-- end: PAGE HEADER --> 


<div class="row">
    <div class="col-sm-12">
 <div class="panel panel-default">
            <div class="panel-heading">
                <i class="fa fa-external-link-square"></i><%=ViewResourceText("Title_AddFonts", "Add & Edit Fonts")%>
                <div class="panel-tools">
                    <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a>
                </div>
            </div>
            <div class="panel-body buttons-widget form-horizontal">

                  <div class="row form-group">
                    <%=ViewControlTitle("lblFontAlias", "Font Alias", "txtFontAlias", ":", "col-sm-3 control-label")%>
                    <div class="col-sm-2">
                        <asp:TextBox ID="txtFontAlias" runat="server" CssClass="form-control validate[required]"></asp:TextBox>
                    </div>
                </div>

                 <div class="row form-group divFontUrl">
                    <%=ViewControlTitle("lblFontUrl", "Font Link", "txtFontUrl", ":", "col-sm-3 control-label")%>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtFontUrl" runat="server" CssClass="form-control validate[required]"></asp:TextBox>
                    </div>
                     <div class="col-sm-2">
                        <a class="Analysis btn btn-default">Analyze</a>
                     </div>
                </div>
              
                <div class="row form-group">

                    <%=ViewControlTitle("lblFontFamily", "Font Family", "txtFontFamily", ":", "col-sm-3 control-label")%>
                    <div class="col-sm-2">
                        <asp:TextBox ID="txtFontFamily" runat="server" CssClass="form-control   validate[required]"></asp:TextBox>
                    </div>

                     <%=ViewControlTitle("lblFontSubset", "Font Subset", "txtFontSubset", ":", "col-sm-3 control-label")%>
                    <div class="col-sm-2">
                        <asp:TextBox ID="txtFontSubset" runat="server" CssClass="form-control   validate[]"></asp:TextBox>
                    </div>
                </div>
                <div class="row form-group">
                    <%=ViewControlTitle("lblFontBold", "Font Weight", "txtFontBold", ":", "col-sm-3 control-label")%>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtFontBold" runat="server" CssClass="form-control   validate[required]"></asp:TextBox>
                    </div>
                </div>
                <div class="row form-group">
                    <%=ViewControlTitle("lblEnable", "Enable", "cbEnable", ":", "col-sm-3 control-label")%>
                    <div class="col-sm-7">
                       <div class="checkbox-inline"><asp:CheckBox ID="cbEnable" runat="server" CssClass="auto" /></div>
                    </div>
                </div>

                <div id="divMessage" style="display:none; color:Red;"><%=ViewResourceText("lblMessage", "No repeating for skin names.")%></div>
            </div>
        </div>
    </div>
</div>


       <div class="row">
        <div class="col-sm-3"> </div>
        <div class="col-sm-9">
            
         <asp:Button CssClass="btn btn-primary" lang="Submit" ID="cmdUpdate" resourcekey="cmdUpdate"
        runat="server" Text="Update" OnClick="cmdUpdate_Click"></asp:Button>&nbsp;
        <asp:Button CssClass="btn btn-default" ID="cmdCancel" resourcekey="cmdCancel" runat="server"
            Text="Cancel" CausesValidation="False" OnClick="cmdCancel_Click"  OnClientClick="CancelValidation();"></asp:Button>&nbsp;

         </div>
      </div>

<script type="text/javascript">
    function parseURL(url) {
        var a = document.createElement('a');
        a.href = url;
        return {
            source: url,
            protocol: a.protocol.replace(':', ''),
            host: a.hostname,
            port: a.port,
            query: a.search,
            params: (function () {
                var ret = {},
                    seg = a.search.replace(/^\?/, '').split('&'),
                    len = seg.length, i = 0, s;
                for (; i < len; i++) {
                    if (!seg[i]) { continue; }
                    s = seg[i].split('=');
                    ret[s[0]] = s[1];
                }
                return ret;
            })(),
            file: (a.pathname.match(/\/([^\/?#]+)$/i) || [, ''])[1],
            hash: a.hash.replace('#', ''),
            path: a.pathname.replace(/^([^\/])/, '/$1'),
            relative: (a.href.match(/tps?:\/\/[^\/]+(.+)/) || [, ''])[1],
            segments: a.pathname.replace(/^\//, '').split('/')
        };
    }
    function UrlDecode(str) {
        var ret = "";
        for (var i = 0; i < str.length; i++) {
            var chr = str.charAt(i);
            if (chr == "+") {
                ret += " ";
            } else if (chr == "%") {
                var asc = str.substring(i + 1, i + 3);
                if (parseInt("0x" + asc) > 0x7f) {
                    ret += asc2str(parseInt("0x" + asc + str.substring(i + 4, i + 6)));
                    i += 5;
                } else {
                    ret += asc2str(parseInt("0x" + asc));
                    i += 2;
                }
            } else {
                ret += chr;
            }
        }
        return ret;
    }

    $(function ($) {

        $(".Analysis").click(function () {
            var FontUrl = $("#<%=txtFontUrl.ClientID%>").val();
            if (FontUrl!== "") {


              
                FontUrl = FontUrl.replace("&amp;", "&");
                try {
                    var myURL = parseURL(FontUrl);
                    if (myURL.params.family.indexOf(":") >= 0) {
                        var familys = myURL.params.family.split(":");
                        if (familys.length == 2) {
                            $("#<%=txtFontFamily.ClientID%>").val(familys[0].replace("+"," "));
                            $("#<%=txtFontBold.ClientID%>").val(familys[1]);
                        }
                    }
                    else {
                        $("#<%=txtFontFamily.ClientID%>").val(myURL.params.family.replace("+", " "));
                    }
                    $("#<%=txtFontSubset.ClientID%>").val(myURL.params.subset);
                }
                catch (e) {

                }
            }

        });




    });
</script>

 