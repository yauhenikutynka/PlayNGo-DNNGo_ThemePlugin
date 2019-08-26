<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Resource_MegaEdit.aspx.cs" Inherits="DNNGo.Modules.ThemePlugin.Resource_MegaEdit" %>
<!DOCTYPE HTML>
<html lang="en" class="no-js">
<head>
<meta charset="utf-8">
<title>jQuery File Upload Example</title>
<!--[if lt IE 9]>
<script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
<![endif]-->
     <!-- start: MAIN CSS -->
    <link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/bootstrap/css/bootstrap.min.css"  media="screen" />
    <link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" href="<%=ModulePath %>Resource/fonts/style.css" />
    <link rel="stylesheet" href="<%=ModulePath %>Resource/css/main.css" />
    <link rel="stylesheet" href="<%=ModulePath %>Resource/css/main-responsive.css" />
  
    <!--<link rel="stylesheet/less" type="text/css" href="<%=ModulePath %>Resource/css/styles.less" />-->
    <link rel="stylesheet" href="<%=ModulePath %>Resource/css/theme_light.css" type="text/css" id="skin_color" />
    <!--[if IE 7]>
		    <link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/font-awesome/css/font-awesome-ie7.min.css" />
		    <![endif]-->
    <!-- end: MAIN CSS -->
    <!-- start: CSS REQUIRED FOR THIS PAGE ONLY -->
    <link rel="stylesheet" href="<%=ModulePath %>Resource/plugins/bootstrap-social-buttons/social-buttons-3.css" />

       <!-- end: CSS REQUIRED FOR THIS PAGE ONLY -->
    <script src="<%=ModulePath %>Resource/js/jquery.min.js"></script>
    <script src="<%=ModulePath %>Resource/js/jquery-ui.min.js"></script>


    
    <script src="<%=ModulePath %>Resource/js/jquery.validationEngine-en.js"></script>
    <script src="<%=ModulePath %>Resource/js/jquery.validationEngine.js"></script>
    

    <!-- start: MAIN JAVASCRIPTS -->
    <!--[if lt IE 9]>
		    <script src="<%=ModulePath %>Resource/plugins/respond.min.js"></script>
		    <script src="<%=ModulePath %>Resource/plugins/excanvas.min.js"></script>
    <![endif]-->

    <script src="<%=ModulePath %>Resource/plugins/bootstrap/js/bootstrap.min.js"></script>
    <script src="<%=ModulePath %>Resource/plugins/blockUI/jquery.blockUI.js"></script>
    <script src="<%=ModulePath %>Resource/plugins/bootstrap-paginator/src/bootstrap-paginator.js"></script>

    <script src="<%=ModulePath %>Resource/plugins/tinymce/tinymce.min.js"></script>
</head>
<body>
<form id="Form" runat="server">
 <div id="PlaceHolder_container" class="container validationEngineContainer"> 
        <div class="tabbable">
          <ul id="myTab4" runat="server" class="nav nav-tabs tab-bricky">
            <li id="liNavigation_html" runat="server"><asp:HyperLink ID="hlNavigation_html" runat="server" Text="<i class='fa fa-plus'></i> Add Html module" ></asp:HyperLink></li>
            <li id="liNavigation_module" runat="server"> <asp:HyperLink ID="hlNavigation_module" runat="server" Text="<i class='fa clip-list-4'></i> Add Other module" ></asp:HyperLink></li>
          </ul>
           <div class="tab-content">
            <asp:Panel ID="panel_tab3_example1" runat="server" CssClass="tab-pane">
                <asp:PlaceHolder ID="phHTML" runat="server"></asp:PlaceHolder>
           </asp:Panel>
            <asp:Panel ID="panel_tab3_example2" runat="server" CssClass="tab-pane">
                  
                  <asp:PlaceHolder ID="phModule" runat="server"></asp:PlaceHolder>
               
            </asp:Panel>
          </div>
        </div>
        </div>
           </form>
<script type="text/javascript">

  jQuery(function ($) {
      tinymce.init({
          selector: "textarea.tinymce",
          entity_encoding: "raw",
          convert_urls: false,
          plugins: [
		        "advlist autolink link image lists charmap print preview hr anchor pagebreak",
		        "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
		        "save table contextmenu directionality template paste textcolor"
	        ]
      });

      $("#PlaceHolder_container").validationEngine({
          promptPosition: "topRight"
      });

      $("#PlaceHolder_container input[lang='Submit']").click(function () {
          if (!$('#PlaceHolder_container').validationEngine('validate')) {
              return false;
          }
      });
  });

  function CancelValidation() {
      $('#Form').validationEngine('detach');
  }
</script>
</body> 
</html>