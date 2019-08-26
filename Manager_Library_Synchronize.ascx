<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_Library_Synchronize.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Manager_Library_Synchronize" %>

      <!-- start: PAGE HEADER -->
      <div class="row">
        <div class="col-sm-12"> 
          <!-- start: PAGE TITLE & BREADCRUMB -->
           
          <div class="page-header">
            <h1><i class="fa fa-cloud-upload"></i> <%=ViewResourceText("Header_Title", "Synchronize Files")%> 
                <asp:HyperLink ID="hlReturnLibrary" runat="server" CssClass="btn btn-xs btn-default"  Text="<i class='fa fa-mail-reply-all'></i> Return Library " resourcekey="hlReturnLibrary" ></asp:HyperLink>
            </h1>
          </div>
          <!-- end: PAGE TITLE & BREADCRUMB --> 
        </div>
      </div>
      <!-- end: PAGE HEADER --> 


       <!-- start: PAGE CONTENT -->
      
      <div class="row">
        <div class="col-sm-12">

          <div class="panel panel-default">
            <div class="panel-heading"> <i class="fa clip-earth-2"></i> <%=ViewResourceText("Title_SynchronizeFiles", "")%>
              <div class="panel-tools"> <a href="#" class="btn btn-xs btn-link panel-collapse collapses"> </a> </div>
            </div>
            <div class="panel-body">
              <div class="row">
                <div class="form-horizontal">


                     <div class="form-group">
                        <div class="col-sm-1"></div>
                        <div class="col-sm-11">
                            <%=ViewResourceText("Title_SynchronizeFilesDescription", "Please put the files which need synchronizing to the following path, no nested folders. It will clear all files under that directory after synchronizing.")%>
                            
                        </div>
                      </div>
                      <div class="form-group">
                        <div class="col-sm-1"></div>
                        <div class="col-sm-11">
                            <asp:Label ID="labShowWebFolderPath" runat="server" CssClass=""></asp:Label>
                        </div>
                      </div>
                      <div class="form-group">
                        <div class="col-sm-1"></div>
                        <div class="col-sm-11">
                            <asp:Label ID="labShowFolderPath" runat="server" CssClass=""></asp:Label>
                        </div>
                      </div>


                     <div class="form-group">
                        <div class="col-sm-1"></div>
                        <div class="col-sm-11">
                            <asp:Label ID="labShowFileCount" runat="server" CssClass=""></asp:Label>
                        </div>
                      </div>

                      
                     <div class="form-group">
                        <div class="col-sm-1"></div>
                        <div class="col-sm-11">
                            <asp:Literal ID="liShowFileList" runat="server"></asp:Literal>
                        </div>
                      </div>

                     <div class="form-group">
                        <div class="col-sm-1"></div>
                        <div class="col-sm-11">
                           <asp:Button runat="server" Text="Synchronize All Files" ID="cmdSynchronizeFiles"  resourcekey="cmdSynchronizeFiles" onclick="cmdSynchronizeFiles_Click"  OnClientClick="CancelValidation();" CssClass="btn btn-bricky btn-lg" /> 
                        </div>
                      </div>
                </div>
              </div>
            </div>
          </div>


 
 
        </div>
      </div>
      
      <!-- end: PAGE CONTENT--> 