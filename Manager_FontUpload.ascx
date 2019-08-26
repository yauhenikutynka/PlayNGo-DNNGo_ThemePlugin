<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_FontUpload.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Manager_FontUpload" %>

      <!-- start: PAGE HEADER -->
      <div class="row">
        <div class="col-sm-12"> 
          <!-- start: PAGE TITLE & BREADCRUMB -->
           
          <div class="page-header">
            <h1><i class="fa fa-paperclip"></i> <%=ViewResourceText("Header_Title", "Upload Font")%> 
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
                    <%=ViewControlTitle("lblFontEot", ".eot", "fileFontEot", ":", "col-sm-3 control-label")%>
                    <div class="col-sm-7">

                        <div class="fileupload fileupload-<asp:Literal ID="liFontEotStyle" runat="server"></asp:Literal>" data-provides="fileupload">
                           <%--<asp:HiddenField ID="hfFontEot" runat="server" />--%>
							<div class="input-group">
								<div class="form-control uneditable-input">
									<i class="fa fa-file fileupload-exists"></i>
									<span class="fileupload-preview"><asp:Literal ID="liFontEot" runat="server"></asp:Literal></span>
								</div>
								<div class="input-group-btn">
									<div class="btn btn-light-grey btn-file">
										<span class="fileupload-new"><i class="fa fa-folder-open-o"></i> Select file</span>
										<span class="fileupload-exists"><i class="fa fa-folder-open-o"></i> Change</span>
                                        <asp:FileUpload ID="fileFontEot" runat="server" CssClass="file-input" />
									</div>
									<a href="#" class="btn btn-light-grey fileupload-exists" data-dismiss="fileupload">
										<i class="fa fa-times"></i> Remove
									</a>
								</div>
							</div>
						</div>
                    </div>
                </div>

                <div class="row form-group">
                    <%=ViewControlTitle("lblFontSvg", ".svg", "fileFontSvg", ":", "col-sm-3 control-label")%>
                    <div class="col-sm-7">

                        <div class="fileupload fileupload-<asp:Literal ID="liFontSvgStyle" runat="server"></asp:Literal>" data-provides="fileupload">
                            <asp:HiddenField ID="HiddenField1" runat="server" />
							<div class="input-group">
								<div class="form-control uneditable-input">
									<i class="fa fa-file fileupload-exists"></i>
									<span class="fileupload-preview"><asp:Literal ID="liFontSvg" runat="server"></asp:Literal></span>
								</div>
								<div class="input-group-btn">
									<div class="btn btn-light-grey btn-file">
										<span class="fileupload-new"><i class="fa fa-folder-open-o"></i> Select file</span>
										<span class="fileupload-exists"><i class="fa fa-folder-open-o"></i> Change</span>
                                        <asp:FileUpload ID="fileFontSvg" runat="server" CssClass="file-input" />
									</div>
									<a href="#" class="btn btn-light-grey fileupload-exists" data-dismiss="fileupload">
										<i class="fa fa-times"></i> Remove
									</a>
								</div>
							</div>
						</div>
                    </div>
                </div>

                <div class="row form-group">
                    <%=ViewControlTitle("lblFontTtf", ".ttf", "fileFontTtf", ":", "col-sm-3 control-label")%>
                    <div class="col-sm-7">

                        <div class="fileupload fileupload-<asp:Literal ID="liFontTtfStyle" runat="server"></asp:Literal>" data-provides="fileupload">
                            <asp:HiddenField ID="HiddenField2" runat="server" />
							<div class="input-group">
								<div class="form-control uneditable-input">
									<i class="fa fa-file fileupload-exists"></i>
									<span class="fileupload-preview"><asp:Literal ID="liFontTtf" runat="server"></asp:Literal></span>
								</div>
								<div class="input-group-btn">
									<div class="btn btn-light-grey btn-file">
										<span class="fileupload-new"><i class="fa fa-folder-open-o"></i> Select file</span>
										<span class="fileupload-exists"><i class="fa fa-folder-open-o"></i> Change</span>
                                        <asp:FileUpload ID="fileFontTtf" runat="server" CssClass="file-input" />
									</div>
									<a href="#" class="btn btn-light-grey fileupload-exists" data-dismiss="fileupload">
										<i class="fa fa-times"></i> Remove
									</a>
								</div>
							</div>
						</div>
                    </div>
                </div>

                <div class="row form-group">
                    <%=ViewControlTitle("lblFontWoff", ".woff", "fileFontWoff", ":", "col-sm-3 control-label")%>
                    <div class="col-sm-7">

                        <div class="fileupload fileupload-<asp:Literal ID="liFontWoffStyle" runat="server"></asp:Literal>" data-provides="fileupload">
                            <asp:HiddenField ID="HiddenField3" runat="server" />
							<div class="input-group">
								<div class="form-control uneditable-input">
									<i class="fa fa-file fileupload-exists"></i>
									<span class="fileupload-preview"><asp:Literal ID="liFontWoff" runat="server"></asp:Literal></span>
								</div>
								<div class="input-group-btn">
									<div class="btn btn-light-grey btn-file">
										<span class="fileupload-new"><i class="fa fa-folder-open-o"></i> Select file</span>
										<span class="fileupload-exists"><i class="fa fa-folder-open-o"></i> Change</span>
                                        <asp:FileUpload ID="fileFontWoff" runat="server" CssClass="file-input" />
									</div>
									<a href="#" class="btn btn-light-grey fileupload-exists" data-dismiss="fileupload">
										<i class="fa fa-times"></i> Remove
									</a>
								</div>
							</div>
						</div>
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
 

 