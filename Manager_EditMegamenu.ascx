<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_EditMegamenu.ascx.cs" Inherits="DNNGo.Modules.ThemePlugin.Manager_EditMegamenu"  %>

<!-- start: PAGE HEADER -->


<div class="row">
  <div class="col-sm-12"> 
    <!-- start: PAGE TITLE & BREADCRUMB -->
    
    <div class="page-header">
      <h1><i class="fa fa-list-alt"></i> <%=ViewResourceText("Header_Title", "Megamenu")%> </h1>
    </div>
    <!-- end: PAGE TITLE & BREADCRUMB --> 
  </div>
</div>
<!-- end: PAGE HEADER --> 

<!-- start: PAGE CONTENT -->

<div class="row">
  <div class="col-sm-12">
    <div class="panel panel-default">
      <div class="panel-heading"> <i class="fa fa-external-link-square"></i>Menu Type
        <div class="panel-tools"> <a href="#" class="btn btn-xs btn-link panel-collapse collapses"></a> </div>
      </div>
      <div class="panel-body buttons-widget form-horizontal">
        <div class="row form-group">
          <label  class="col-sm-2 control-label text-right">Tab Type:</label>
          <div class="col-sm-2">
              <asp:DropDownList ID="ddlMenuType" runat="server" CssClass="form-control form_default showMegaMenu"></asp:DropDownList>
          </div>
          
          <div class="col-sm-8">
          <label  class="col-sm-2 control-label text-right MegaMenuWidth" style="display:none;">Mega Menu Width:</label>
          <div class="col-sm-4 MegaMenuWidth" style="display:none;">
            <select class="form-control form_default" id="showMegaMenuWidth">
                <option>Auto</option>
                <option>Custom</option>
            </select>
           <span id="MegaMenuWidthvalue"> <asp:TextBox ID="txtMegaMenuWidth" runat="server" CssClass="form-control form_default "></asp:TextBox> px  </span>
          </div>
          <label  class="col-sm-2 control-label text-right MegaMenuWidth" style="display:none;">Mega Menu Position:</label>
          <div class="col-sm-2 MegaMenuWidth" style="display:none;">
               <asp:DropDownList ID="ddlMegaPosition" runat="server" CssClass="form-control form_default"></asp:DropDownList>
          </div>
          
          </div>
          
        </div>
      </div>
    </div>
  </div>
</div>

<div class="row"> 
    <div class="col-sm-3"></div>
    <div class="col-sm-9"> 
        <div class="form-group">
            <asp:Button CssClass="btn btn-primary" lang="Submit" ID="cmdUpdate" resourcekey="cmdUpdate" runat="server" Text="Update" OnClick="cmdUpdate_Click"></asp:Button>&nbsp;
            <asp:Button CssClass="input_button btn" lang="Submit" ID="cmdCancel" resourcekey="cmdCancel" runat="server" Text="Cancel" OnClick="cmdCancel_Click"></asp:Button>&nbsp;
        </div>
    </div>
</div>

<div id="MegaMenuBox" style="display:none;">
  <div class="row AddPaneTitle">
    <div class="col-sm-6">
      <h3>Mega Menu</h3>
    </div>
    <div class="col-sm-6 text-right"><a  class="btn btn-xs btn-primary" id="AddPane"><i class='fa fa-plus-square'></i> Add Pane</a> </div>
  </div>
  <ul class="" id="AddPaneBox">
  </ul>
  <div class="loading"></div>
</div>

<script id="AddPaneTmpl" type="text/x-jquery-tmpl">
  <li class="${PaneName} dragPane" data-id="${ID}" data-name="${PaneName}" style="width:${PaneWidth}%" >
	<div class="dragLilstBox">
	  <div class="panel-heading"> <i class="fa fa-list-ul"></i>${PaneName}
             <div class="panel-tools ">
				<a class="btn btn-xs btn-bricky PaneDelete" title="Delete Pane"><i class='fa fa-trash-o'></i> Delete</a> 
				<a class="btn btn-xs btn-primary" href="#columnedit" data-toggle="modal" title="Edit Pane"> <i class='fa fa-edit'></i> Edit</a>
				<a href="#addtype" data-toggle="modal" class="btn btn-xs btn-primary" title="Add Control"><i class='fa fa-plus-square'></i> Add</a> 
				<a class="btn btn-xs btn-info dragPaneIcon"><i class='fa fa-arrows' title="Sort Pane"></i></a>
			</div>
	  </div>
	  <ul class="menu-list">
		<li class="not-itme">Please Add Controls</li>
	  </ul>
	</div>
  </li>
</script>
<div id="addtype" class="modal fade" tabindex="-1" data-width="760" style="display: none;"></div>
<script id="addtypeTmpl" type="text/x-jquery-tmpl">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"> &times; </button>
    <h4 class="modal-title">Add Control</h4>
  </div>
  <div class="modal-body">
    <div class="panel panel-default">
      <div class="panel-heading"> <i class="fa fa-external-link-square"></i>Menu
        <div class="panel-tools"><a class="btn btn-xs btn-primary addtypebtn"  data-dismiss="modal" data-addtype="0" title="Add Menu"> <i class='fa clip-checkmark-2'></i> Add</a></div>
      </div>
    </div>
    <div class="panel panel-default">
      <div class="panel-heading"> <i class="fa fa-external-link-square"></i>HTML
        <div class="panel-tools"><a class="btn btn-xs btn-primary addtypebtn"  data-dismiss="modal" data-addtype="1" title="Add HTML"> <i class='fa clip-checkmark-2'></i> Add</a></div>
      </div>
    </div>
    <div class="panel panel-default">
      <div class="panel-heading"> <i class="fa fa-external-link-square"></i>Module
        <div class="panel-tools"><a class="btn btn-xs btn-primary addtypebtn"  data-dismiss="modal" data-addtype="2" title="Add Module"> <i class='fa clip-checkmark-2'></i> Add</a></div>
      </div>
    </div>
  </div>
</script> 
<script id="TypeTmpl" type="text/x-jquery-tmpl">
<li class="dragList ${TagList}" data-tag="${TagList}" data-id="${ID}">
	<div class="panel-heading">
		{{if RowType == 0}}
		<b>Menu:</b><span class="type-title">${Title}</span>
		{{/if}}	
		{{if RowType == 1}}
		<b>HTML:</b><span class="type-title">${Title}</span>
		{{/if}}	
		{{if RowType == 2}}
		<b>Module:</b><span class="type-title">${Title}</span>
		{{/if}}	
		<div class="panel-tools">
			<a class="btn btn-xs btn-bricky listdelete" title="Delete Itme"><i class='fa fa-trash-o'></i> Delete</a> 
			<a class="btn btn-xs btn-primary"  href="#typedataedit" data-toggle="modal" title="Edit Itme"><i class='fa fa-edit'></i> Edit</a> 
			<a class="btn btn-xs btn-info dragListIcon" title="Sort Itme"><i class='fa fa-arrows'></i></a> 
		</div>
	</div>
</li>
</script>
<div id="typedataedit" class="modal fade" tabindex="-1" data-width="760" style="display: none;"> </div>
<script id="typedataeditTmpl" type="text/x-jquery-tmpl">
{{if RowType == 0}}
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"> &times; </button>
    <h4 class="modal-title">Menu Settings</h4>
  </div>
  <div class="modal-body form-horizontal">
  
    <div class="row form-group">
      <div class="col-sm-2 text-right control-label"> Menu:</div>
      <div class="col-sm-10">
	   	<div class="MenuList" data-name="MenuID" data-value="${MenuID}"></div>
      </div>
    </div>
    <div class="row form-group">
      <div class="col-sm-2 text-right control-label"> Level: </div>
      <div class="col-sm-10">
        <select class="form-control form_default" data-name="MenuLevel" data-value="${MenuLevel}">
          <option value="0">All</option>
          <option value="1">1</option>
          <option value="2">2</option>
          <option value="3">3</option>
          <option value="4">4</option>
          <option value="5">5</option>
          <option value="6">6</option>
          <option value="7">7</option>
          <option value="8">8</option>
          <option value="9">9</option>
        </select>
      </div>
    </div>
    <div class="row form-group">
      <div class="col-sm-2 text-right control-label"> Display Title: </div>
      <div class="col-sm-10">
		 <label class="checkbox-inline" ><input type="checkbox"  class="square-green grey " data-name="MenuDisplayTitle" {{if MenuDisplayTitle==1 || MenuDisplayTitle=="true"}} checked="checked"{{/if}}/></label>
      </div>
    </div>
    <div class="row form-group">
      <div class="col-sm-2 text-right control-label"  data-name="MenuSytle" data-value="${MenuSytle}"> Menu Sytle: </div>
      <div class="col-sm-10">
	  <div class="radio-img-list" data-name="MenuSytle"  data-value="${MenuSytle}">
		  <label class="radio-inline">
			  <input type="radio" name="MenuSytle" value="submenulist_1" class="grey" checked="checked" />
			  <img src="<%=SkinPath%>Thumbnails/submenulist_1.jpg" class="img-responsive" alt="submenulist_1" title="submenulist_1"/>
		  </label>
		  <label class="radio-inline">
			  <input type="radio" name="MenuSytle" value="submenulist_2" class="grey" />
			  <img src="<%=SkinPath%>Thumbnails/submenulist_2.jpg" class="img-responsive" alt="submenulist_2" title="submenulist_2"/>
		  </label>		
		  <label class="radio-inline">
			  <input type="radio" name="MenuSytle" value="submenulist_3" class="grey" />
			  <img src="<%=SkinPath%>Thumbnails/submenulist_3.jpg" class="img-responsive" alt="submenulist_3" title="submenulist_3"/>
		  </label>		
		  <label class="radio-inline">
			  <input type="radio" name="MenuSytle" value="submenulist_4" class="grey" />
			  <img src="<%=SkinPath%>Thumbnails/submenulist_4.jpg" class="img-responsive" alt="submenulist_4" title="submenulist_4"/>
		  </label>		
		  <label class="radio-inline">
			  <input type="radio" name="MenuSytle" value="submenulist_5" class="grey" />
			  <img src="<%=SkinPath%>Thumbnails/submenulist_5.jpg" class="img-responsive" alt="submenulist_5" title="submenulist_5"/>
		  </label>		
	  </div>
      </div>
    </div>
	
  </div>
{{/if}}	

{{if RowType == 1}}
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"> &times; </button>
    <h4 class="modal-title">HTML Settings</h4>
  </div>
  <div class="modal-body form-horizontal ">
  
  	<div class="row form-group">
	 <div class="col-sm-12">
		<p>Title:</p>
		<input type="text" class="form-control" value="${HTML_Title}" data-name="HTML_Title" />
	</div>
	</div>
    <div class="row form-group">
      <div class="col-sm-12"> Display Title: <label class="checkbox-inline" ><input type="checkbox"  class="square-green grey " data-name="MenuDisplayTitle" {{if MenuDisplayTitle==1 || MenuDisplayTitle=="true"}} checked="checked"{{/if}}/></label>
      </div>
    </div>
  	<div class="row form-group">
	 <div class="col-sm-12">
		<p>Content Text:</p>
		<textarea class="ckeditor form-control" cols="10" rows="10"   data-name="HTML_Content">${HTML_Content}</textarea>
	</div>
	</div>
  </div>
 {{/if}}	
 
{{if RowType == 2}}
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"> &times; </button>
    <h4 class="modal-title">Module Settings</h4>
  </div>
  <div class="modal-body form-horizontal">
    <div class="row form-group">
      <div class="col-sm-2 text-right control-label" >
      Pages:
      </div>
      <div class="col-sm-10">
	   	<div class="MenuList BindTab" data-name="BindTabID" data-value="${BindTabID}"></div>
      </div>
    </div>
    <div class="row form-group">
      <div class="col-sm-2 text-right control-label" >
        Modules:
      </div>
      <div class="col-sm-10">
        <select class="form-control form_default BindModule"  data-name="BindModuleID" data-value="${BindModuleID}">
        </select>
      </div>
    </div>
    <div class="row form-group">
      <div class="col-sm-2 control-label"> Display Title: </div>
      <div class="col-sm-10">
		 <label class="checkbox-inline" ><input type="checkbox"  class="square-green grey " data-name="MenuDisplayTitle" {{if MenuDisplayTitle==1 || MenuDisplayTitle=="true"}} checked="checked"{{/if}}/></label>
      </div>
    </div>
  </div>
{{/if}}	
<div class="modal-footer">
	<button type="button" data-dismiss="modal" class="btn btn-light-grey"> Close </button>
	<button type="button" class="btn btn-blue" id="typedataeditsavechanges"> Save changes </button>
</div>
</script>
<div id="columnedit" class="modal fade" tabindex="-1" data-width="760" style="display: none;"></div>
<script id="columneditTmpl" type="text/x-jquery-tmpl">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"> &times; </button>
    <h4 class="modal-title">Pane Settings</h4>
  </div>
  <div class="modal-body form-horizontal">
    <div class="row  form-group">
      <div class="col-sm-3 text-right control-label"> Pane Width: </div>
      <div class="col-sm-7">
        <input type="text" placeholder=" " class="form-control form_default" value="${PaneWidth}" data-name="PaneWidth" > %
      </div>
    </div>
    <div class="row  form-group">
      <div class="col-sm-3 text-right control-label"> Pane Top Spacing: </div>
      <div class="col-sm-7">
        <input type="text" placeholder=" " class="form-control form_default" value="${PaneTopSpacing}" data-name="PaneTopSpacing" > px
      </div>
    </div>
    <div class="row  form-group">
      <div class="col-sm-3 text-right control-label"> Pane Right Spacing: </div>
      <div class="col-sm-7">
        <input type="text" placeholder=" " class="form-control form_default" value="${PaneRightSpacing}" data-name="PaneRightSpacing" > px
      </div>
    </div>
    <div class="row  form-group">
      <div class="col-sm-3 text-right control-label"> Pane Bottom Spacing: </div>
      <div class="col-sm-7">
        <input type="text" placeholder=" " class="form-control form_default" value="${PaneBottomSpacing}" data-name="PaneBottomSpacing" > px
      </div>
    </div>
    <div class="row  form-group">
      <div class="col-sm-3 text-right control-label"> Pane Left Spacing: </div>
      <div class="col-sm-7">
        <input type="text" placeholder=" " class="form-control form_default" value="${PaneLeftSpacing}" data-name="PaneLeftSpacing" > px
      </div>
    </div>
    <div class="row  form-group">
      <div class="col-sm-3 text-right control-label"> Dispaly Top Line: </div>
      <div class="col-sm-7">
	    <label class="checkbox-inline" ><input type="checkbox"  class="square-green grey " data-name="PaneTopLine" {{if PaneTopLine==1 || PaneTopLine=="true"}} checked="checked"{{/if}}/></label>
      </div>
    </div>
    <div class="row  form-group">
      <div class="col-sm-3 text-right control-label"> Dispaly Right Line: </div>
      <div class="col-sm-7">
	    <label class="checkbox-inline" ><input type="checkbox"  class="square-green grey " data-name="PaneRightLine" {{if PaneRightLine==1 || PaneRightLine=="true"}} checked="checked"{{/if}}/></label>
      </div>
    </div>
    <div class="row  form-group">
      <div class="col-sm-3 text-right control-label"> Dispaly Bottom Line: </div>
      <div class="col-sm-7">
	    <label class="checkbox-inline" ><input type="checkbox"  class="square-green grey " data-name="PaneBottomLine" {{if PaneBottomLine==1 || PaneBottomLine=="true"}} checked="checked"{{/if}}/></label>
      </div>
    </div>
    <div class="row  form-group">
      <div class="col-sm-3 text-right control-label"> Dispaly Left Line: </div>
      <div class="col-sm-7">
	    <label class="checkbox-inline" ><input type="checkbox"  class="square-green grey " data-name="PaneLeftLine" {{if PaneLeftLine==1 || PaneLeftLine=="true"}} checked="checked"{{/if}}/></label>
      </div>
    </div>
	
	
	
  </div>
  <div class="modal-footer">
    <button type="button" data-dismiss="modal" class="btn btn-light-grey"> Close </button>
    <button type="button" class="btn btn-blue save-changes" > Save changes </button>
  </div>
</script> 
<div class="pageMenu" style="display:none;">
	<div class="select form-control form_default">Undefined</div>
    <ul class="selectlist"></ul>
</div>

 

<script type="text/javascript" src="<%=ModulePath %>Resource/plugins/ckeditor/ckeditor.js"></script>
<script type="text/javascript" src="<%=ModulePath %>Resource/plugins/ckeditor/adapters/jquery.js"></script>

<script type="text/javascript">

var PaneDate = {};
var loading = $("#MegaMenuBox .loading");

loading.show();
jQuery.ajax({
	 type: "POST",
	 url: "<%=ServiceUrl("ServiceGetPanls") %>",
	 dataType: "json",
	 async:false,
	 success: function(data){
		PaneDate=data;
		
		for(i in PaneDate){
			if(PaneDate[i].Options){
				var json = eval('(' + PaneDate[i].Options + ')');
				for(x in json){
					PaneDate[i][json[x]["Key"]]=json[x]["Value"]
				}
			}else{
				PaneDate[i]["PaneTopSpacing"]=20;
				PaneDate[i]["PaneRightSpacing"]=20;
				PaneDate[i]["PaneBottomSpacing"]=20;
				PaneDate[i]["PaneLeftSpacing"]=20;
				PaneDate[i]["PaneTopLine"]=20;
				PaneDate[i]["PaneRightLine"]=0;
				PaneDate[i]["PaneBottomLine"]=0;
				PaneDate[i]["PaneLeftLine"]=0;
			}
		}
		
		loading.hide()
	  }
 });
function jsonID(json,ID){
	for(i in json){
		if(json[i].ID==ID){
			return json[i];
		} 
	}
}

/*Menu*/
jQuery.ajax({
	type: "POST",
	url: "<%=ServiceUrl("ServiceGetTabs") %>",
	dataType: "json",
	async:false,
	success : function (data) {
 
		function convert(source){
		   var tmp={},parent,n;
			for(n in source){
				var item=source[n];
				if(item.TabID==item.ParentId){
					parent=item.TabID;
				}
				if(!tmp[item.TabID]){
					tmp[item.TabID]={};
				}
				tmp[item.TabID].TabName=item.TabName;
				tmp[item.TabID].TabID=item.TabID;
				tmp[item.TabID].ParentId=item.ParentId;
				
				if(!("children" in tmp[item.TabID]))tmp[item.TabID].children=[];
				if(item.TabID!=item.ParentId){   
					if(tmp[item.ParentId]){
						tmp[item.ParentId].children.push(tmp[item.TabID]);
					}
					else{
						tmp[item.ParentId]={children:[tmp[item.TabID]]};
					}
				}
			}
			return tmp[-1];
		}

		var str = "";
		var forTree = function (o) {
			for (var i = 0; i < o.length; i++) {
				var urlstr = "";
				try {
					urlstr = "<li><div class=\"itme\" data-id=\"" + o[i]["TabID"] + "\">" + o[i]["TabName"] + "</div>";
					str += urlstr;
					if (o[i]["children"] != null && o[i]["children"].length > 0) {
						str += "<span class=\"ico\">+</span><ul>";
						forTree(o[i]["children"]);
						str += "</ul>";
					}
					str += "</li>";
				} catch (e) {}
			}
			return str;
		}
		var box = $(".pageMenu");
	
		box.find(".selectlist").html(forTree(convert(data)["children"]))
		box.find(".ico").click(function () {
			$(this).siblings("ul").slideToggle(100);
			$(this).parent().toggleClass("active");
			$(this).parent().siblings().find("ul").slideUp(100);
		})
		box.find(".select").on("click", function () {
			$(this).siblings(".selectlist").slideToggle(100);
		})
		box.find(".itme").on("click", function () {
			var MenuList = $(this).parents(".MenuList");
			MenuList.find(".select").data("value", $(this).data("id"))
			MenuList.find(".selectlist").slideUp(100);
			MenuList.data("value", $(this).data("id"))
	
			var mn = $(this).html(),
			ni = $(this),
			maxs = 0;
			function parentitme() {
				if (!ni.parent().parent().hasClass("selectlist") && maxs < 10) {
					ni = ni.parent().parent().siblings(".itme");
					mn = ni.html() + " > " + mn;
					maxs++;
					parentitme();
				}
			}
			parentitme();
	
			MenuList.find(".select").html(mn);
			MenuList.data("tabName", mn);
		})
	}
 });


/*load data*/
for (var itme in PaneDate) {
	PaneDate[itme].PaneWidth?"":PaneDate[itme].PaneWidth="30";
	PaneDate[itme].PaneName?"":PaneDate[itme].PaneName="Pane_"+PaneDate[itme].ID;

	$("#AddPaneBox").append(jQuery("#AddPaneTmpl").tmpl(PaneDate[itme]));
	
	var box = $("." + PaneDate[itme].PaneName).find(".menu-list");
	var listdate = PaneDate[itme].Rows;
	for (var itme2 in listdate) {
		var rowData=listdate[itme2];
		rowData.TagList?"":rowData.TagList="row_"+rowData.ID;
		rowData.Title?"":rowData.Title="Undefined";
		var tmpls = jQuery("#TypeTmpl").tmpl(rowData);
		if (box.find(".dragList").length == 0) {
			box.html(tmpls)
		} else {
			box.append(tmpls)
		}
	}
}

/*Add Pane*/
$("#AddPane").on("click", function () {
	loading.show();
	jQuery.ajax({
		 type: "POST",
		 url: "<%=ServiceUrl("ServiceSavePanl") %>&PaneID=999||0",
		 dataType: "json",
		 success: function(data){
			data.PaneWidth?"":data.PaneWidth="30";
			data.PaneName?"":data.PaneName="Pane_"+data.ID;
			data.Sort!=0?"":data.Sort=$("#AddPaneBox .dragPane").length;
			data.Rows?"":data.Rows=[];
			var curr=data["ID"];

			data["PaneTopSpacing"]=20;
			data["PaneRightSpacing"]=20;
			data["PaneBottomSpacing"]=20;
			data["PaneLeftSpacing"]=20;
			data["PaneTopLine"]=0;
			data["PaneRightLine"]=0;
			data["PaneBottomLine"]=0;
			data["PaneLeftLine"]=0;

			
			var box = jQuery("#AddPaneTmpl").tmpl(data);
			$("#AddPaneBox").append(box);
			$("#AddPaneBox").dragsort("destroy");
			$(".dragLilstBox .menu-list").dragsort("destroy");
			$(".dragLilstBox .menu-list").dragsort({ 
				dragSelector: "li  .dragListIcon", 
				dragBetween: true, 
				dragEnd: ListSaveOrder, 
				placeHolderTemplate: "<li class=\"placeHolder\"><div></div></li>",
			});
			$("#AddPaneBox").dragsort({ 
				dragSelector: "li .dragPaneIcon", 
				dragBetween: true, 
				dragEnd: PaneSaveOrder, 
				placeHolderTemplate: "<li class=\"placeHolder\"><div></div></li>",
			});
			
			if(!PaneDate){PaneDate=[]};
			PaneDate.push(data);
			
			jQuery.ajax({
				 type: "POST",
				 url: "<%=ServiceUrl("ServiceSavePanl") %>&PaneID="+curr,
				 dataType: "json",
				 data:data,
				 success: function(data){
					 loading.hide();
 				  }
			 });
		  }
	 });
})
/*Delete Pane*/
$("body").delegate("a.PaneDelete", "click", function () {
	
	
	if (confirm("Are you sure you want to delete this pane?")) {
		loading.show();
		
		if ($(this).parents(".dragPane").find(".dragList").length!=0){
			$(this).parents(".dragPane").find(".dragList").each(function(index, element) {
				var rowid=$(this).data("id"); 
				jQuery.ajax({
					 type: "POST",
					 url: "<%=ServiceUrl("ServiceDeleteRow") %>&RowID="+rowid,
					 dataType: "json",
					 async:false,
					 success: function(data){
					}
				 });
            });
		}
		
		var e=$(this),tag =e.parents(".dragPane").data("id");
		
		jQuery.ajax({
			 type: "POST",
			 url: "<%=ServiceUrl("ServiceDeletePanl") %>&PaneID="+tag,
			 dataType: "json",
			 success: function(data){
				for(i in PaneDate){
					if(PaneDate[i].ID==jsonID){
						 PaneDate.splice(i,1);
					} 
				}
				e.parents(".dragPane").remove();
				loading.hide()
			  }
		 });
		
	}
})
/*Update Pane*/
$("body").delegate("a[href ^= #columnedit]", "click", function () {
	
	var columnedit =jsonID(PaneDate,$(this).parents(".dragPane").data("id"));
	
		$("#columnedit").html(jQuery("#columneditTmpl").tmpl(columnedit));
		fromUI($("#columnedit"));
		$("#columnedit .save-changes").one("click", function () {
			$("#columnedit").addClass("modal-loading");
			$("#columnedit .modal-body").find("input[type='text'],select,button,textarea").each(function (index, element) {
				columnedit[$(this).data("name")] = $(this).val();
			});
			
			$("#columnedit .modal-body").find("input[type='checkbox']").each(function () {
				if($(this).attr("checked")){
					columnedit[$(this).data("name")] = 1;
				}else{
					columnedit[$(this).data("name")] = 0;
				}
			})
			
			jQuery.ajax({
				 type: "POST",
				 url: "<%=ServiceUrl("ServiceSavePanl") %>&PaneID="+columnedit.ID,
				 dataType: "json",
				 data:columnedit,
				 success: function(data){
					$("." + columnedit.PaneName).innerWidth(columnedit.PaneWidth+"%");
					$("#columnedit").removeClass("modal-loading");
					$(".modal-scrollable").click();
				  }
			 });
			
		})
})
/*Pane Sort*/
$("#AddPaneBox").dragsort({ 
	dragSelector: "li .dragPaneIcon", 
	dragBetween: true, 
	dragEnd: PaneSaveOrder, 
	placeHolderTemplate: "<li class=\"placeHolder\"><div></div></li>",
});

function PaneSaveOrder(){
		loading.show();
	var sortPane ="[";
		$("#AddPaneBox .dragPane").each(function(index) {
		    jsonID(PaneDate,$(this).data("id")).Sort = index;
			if(index==0){sq=""}else{sq=","};
			sortPane += sq + '{"ID":' + $(this).data("id") + ',"Sort":'+index+'}';
		});
		sortPane+="]";
		sortPane ="SortJson="+ encodeURIComponent(sortPane);
	//	<%=ServiceUrl("ServiceSortPanlRow") %>&PaneID=999||0
		jQuery.ajax({
			 type: "POST",
			 url: "<%=ServiceUrl("ServiceSortPanlRow") %>",
			 dataType: "json",
			 data:sortPane,
			 success: function(data){
				loading.hide();
			}
		 });
}

/*Add Rows*/
$("body").delegate("a[href ^= #addtype]", "click", function () {

	var Pane = jsonID(PaneDate,$(this).parents(".dragPane").data("id"));
		Pane_list = Pane.Rows;

		$("#addtype").html(jQuery("#addtypeTmpl").tmpl(Pane));
		$("#addtype .addtypebtn").one("click", function () {
			
			loading.show();
			var newsType = {};
			var type = $(this).data("addtype"); 
			jQuery.ajax({
				 type: "POST",
				 url: "<%=ServiceUrl("ServiceSaveRow") %>&PaneID="+Pane.ID+"&RowType="+type+"&RowID=999||0",
				 dataType: "json",
				 success: function(data){
					newsType=data;
					newsType.TagList?"":newsType.TagList="row_"+newsType.ID;
					newsType.Title?"":newsType.Title="Undefined";

					var box = jQuery("#TypeTmpl").tmpl(newsType);
					if ($("."+Pane.PaneName).find(".dragList").length == 0) {
						$("."+Pane.PaneName).find(".menu-list").html(box)
					} else {
						$("."+Pane.PaneName).find(".menu-list").append(box)
					}
					if(!Pane_list){
						Pane_list=[];
					}
					Pane_list.push(newsType);
					
					loading.hide()
				  }
			 });
		})

})
/*Delete Rows*/
$("body").delegate("a.listdelete", "click", function () {
	if (confirm("Are you sure you want to delete this item?")) {
		loading.show();
		var e=$(this),rowid=e.parents(".dragList").data("id");
		jQuery.ajax({
			 type: "POST",
			 url: "<%=ServiceUrl("ServiceDeleteRow") %>&RowID="+rowid,
			 dataType: "json",
			 success: function(data){
				
				var p=e.parents(".dragPane");
					e.parents(".dragList").remove();
					if(p.find(".dragList").length==0){
						p.find(".menu-list").html("<li class=\"not-itme\">Please Add Controls</li>")
					}
					
				var newsrow=jsonID(PaneDate,p.data("id"))["Rows"];
					for(i in newsrow){
						if(newsrow[i].ID==rowid){
							 newsrow.splice(i,1);
						} 
					}
					loading.hide()
			  }
		 });
	}
})
/*Update Rows*/
$("body").delegate("a[href ^= #typedataedit]", "click", function () {

    var rowId=$(this).parents(".dragList").data("id"),
		paneId=$(this).parents(".dragPane").data("id");
		
	var rowData = jsonID(jsonID(PaneDate,paneId)["Rows"],rowId) ;
		$("#typedataedit").html(jQuery("#typedataeditTmpl").tmpl(rowData));
		fromUI($("#typedataedit"));

	$("#typedataedit").find("#typedataeditsavechanges").one("click", function () {

		$("#typedataedit").addClass("modal-loading");
		$("#typedataedit .modal-body").find("input[type='text'],select,button,textarea").each(function () {
			rowData[$(this).data("name")] = $(this).val();
		});
		$("#typedataedit .modal-body").find("input[type='checkbox']").each(function () {
			if($(this).attr("checked")){
				rowData[$(this).data("name")]=1
			}else{
				rowData[$(this).data("name")]=0
			}
		})
		$("#typedataedit .modal-body").find(".radio-img-list").each(function () {
			var name=$(this).data("name");
			$(this).find("input").each(function(index, element) {
                if($(this).attr("checked")){
					rowData[name]=$(this).attr("value")
				}
            });
		})
		$("#typedataedit .modal-body").find(".MenuList").each(function () {
			rowData[$(this).data("name")]=$(this).data("value");
		})
		
		 if(rowData.RowType == 0){
			rowData["Title"]=$("#typedataedit .modal-body").find(".MenuList .select").html();
		 }
		 if(rowData.RowType == 1){
			rowData["Title"]=rowData.HTML_Title;
		 }
		 if(rowData.RowType == 2){
			rowData["Title"]=$("#typedataedit .modal-body").find(".MenuList .select").html()+" > "+$("#typedataedit .modal-body").find(".BindModule").data("mdname");
		 }
		jQuery.ajax({
			 type: "POST",
			 url: "<%=ServiceUrl("ServiceSaveRow") %>&RowID="+rowId,
			 dataType: "json",
			 data:rowData,
			 async:false,
			 success: function(data){
				 if(rowData.RowType == 0){
					$(".dragList."+rowData.TagList).find(".type-title").html($("#typedataedit .modal-body").find(".MenuList .select").html());
				 }
				 if(rowData.RowType == 1){
					$(".dragList."+rowData.TagList).find(".type-title").html(rowData.HTML_Title);
				 }
				 if(rowData.RowType == 2){
					$(".dragList."+rowData.TagList).find(".type-title").html($("#typedataedit .modal-body").find(".MenuList .select").html()+" > "+$("#typedataedit .modal-body").find(".BindModule").data("mdname"));
				 }
				
				$(".modal-scrollable").click();
				$("#typedataedit").removeClass("modal-loading");
			  }
		 });
	})
})
/*Rows Sort*/
$(".dragLilstBox .menu-list").dragsort({ 
	dragSelector: "li  .dragListIcon", 
	dragBetween: true, 
	dragEnd: ListSaveOrder, 
	placeHolderTemplate: "<li class=\"placeHolder\"><div></div></li>",
});
var dragParent;
$("body").delegate("a.dragListIcon","mousedown", function () {
	 dragParent = $(this).parents(".dragPane")
})
function ListSaveOrder(){
		loading.show();
		var e=$(this),
		    oddPane=dragParent,
			newPane=e.parents(".dragPane");
	
	var sortPane ="[";
			oddPane.find(".dragList").each(function(index) {
				if(index==0){sq=""}else{sq=","};
				jsonID(jsonID(PaneDate,oddPane.data("id"))["Rows"],$(this).data("id")).Sort=index;
				sortPane += sq + '{"ID":' + $(this).data("id") + ',"Sort":'+index+'}';
			});
			sortPane+="]";
			sortPane ="SortJson="+ encodeURIComponent(sortPane);

			jQuery.ajax({
				 type: "POST",
				 url: "<%=ServiceUrl("ServiceSortPanlRow") %>&PaneID="+oddPane.data("id"),
				 dataType: "json",
				 data:sortPane,
				 success: function(data){
					if(oddPane.data("id") != newPane.data("id")){
						
						var odddata=jsonID(PaneDate,oddPane.data("id"))["Rows"], 
							newdatarow=jsonID(odddata,e.data("id"));
							for(i in odddata){
								if(odddata[i].ID==e.data("id")){
									 odddata.splice(i,1);
								} 
							}
							newdatarow.PaneID=newPane.data("id");
							if(!jsonID(PaneDate,newPane.data("id"))["Rows"]){
								jsonID(PaneDate,newPane.data("id"))["Rows"]=[]
							}	
							jsonID(PaneDate,newPane.data("id"))["Rows"].push(newdatarow)
							
					
						var newsortPane ="[";
							newPane.find(".dragList").each(function(index) {
								if(index==0){sq=""}else{sq=","};
								jsonID(jsonID(PaneDate,newPane.data("id"))["Rows"],$(this).data("id")).Sort=index;
								newsortPane += sq + '{"ID":' + $(this).data("id") + ',"Sort":'+index+'}';
							});
							newsortPane+="]";
							newsortPane ="SortJson="+ encodeURIComponent(newsortPane);
							jQuery.ajax({
								 type: "POST",
								 url: "<%=ServiceUrl("ServiceSortPanlRow") %>&PaneID="+newPane.data("id"),
								 dataType: "json",
								 data:newsortPane,
								 success: function(data){
									 loading.hide();
								}
							 });
							newPane.find(".not-itme").remove();
					}else{
							 loading.hide();
					}
				}
			 });
			 
			if(oddPane.find(".dragList").length==0){
				oddPane.find(".menu-list").html("<li class=\"not-itme\">Please Add Controls</li>")
			}
}



/*from UI */
function fromUI(e){
	e.find("select").each(function() {
		$(this).find("option[value="+$(this).data("value")+"]").attr("selected","selected")
	});
	e.find(".radio-img-list").each(function() {
		$(this).find("input[value="+$(this).data("value")+"]").attr("checked","checked")
	});
	e.find("input[type='checkbox']").each(function () {
		if($(this).attr("checked")){
			$(this).attr("value","1")
		}else{
			$(this).val("value","0")
		}
	})
	CKEDITOR.disableAutoInline = true;
	e.find(".ckeditor").each(function() {
		$(this).ckeditor({
			allowedContent: true
		}); 
	});
 
	e.find(".MenuList").each(function() {
		var e=$(this);
		e.append($(".pageMenu").clone("true").show());
		$(this).find(".itme").each(function() {
            if($(this).data("id")==e.data("value")){
				$(this).addClass("curr");
				
				var mn=$(this).html(),ni=$(this),maxs=0;
				
				function parentitme(){
					if(!ni.parent().parent().hasClass("selectlist") && maxs<10){
						ni=ni.parent().parent().siblings(".itme");
						mn = ni.html() +" > "+mn;
						maxs++;
						parentitme();
					}
				}
				parentitme();
				
				e.find(".select").html(mn)
				
			}
        });
	});
	
	e.find(".BindModule").each(function() {
		var value=$(this).parents(".modal-body").find(".BindTab").data("value");
			box=$(this);
			
			function ModuleList(v){
				$("#columnedit").addClass("modal-loading");
				box.html(" ");
				jQuery.ajax({
					 type: "POST",
					 url: "<%=ServiceUrl("ServiceGetModules") %>&ByTabID="+v,
					 dataType: "json",
					 success: function(data){
						var selected=false;
						for(i in data){
							var title=data[i].ModuleTitle;
							if(title.length>50){
								title=title.substring(0,50)+"..."
							}
							var c="";
							if(box.data("value") == data[i].ModuleID){
								c="selected=\"selected\"";
								box.data("mdname",data[i].ModuleName+"-"+title);
								selected=true;
							}
							box.append("<option "+c+" value=\""+data[i].ModuleID+"\">"+data[i].ModuleName+"-"+title+"</option>");
						}
						if(!selected){
							box.find("option").eq(0).attr("selected","selected");
							box.data("mdname",box.find("option").eq(0).html());
						}
						
						$("#columnedit").removeClass("modal-loading");
					 }
				 });
			} 
			ModuleList(value);
			box.on("change",function(){
				box.data("mdname",box.find("option[value="+$(this).val()+"]").html());
			})			
		
		$(this).parents(".modal-body").find(".BindTab").find(".itme").on("click",function(){
			ModuleList($(this).data("id"));
		})
	});
	
	e.find('input[type="checkbox"], input[type="radio"]').iCheck({
		checkboxClass: 'icheckbox_square-green',
		radioClass: 'iradio_square-green',
		increaseArea: '10%' // optional
	});
}


if ($(".showMegaMenu option:selected").text() == "Mega menu") {
	$("#MegaMenuBox").show();
	
	if($("#MegaMenuWidthvalue").find("input").val()==0){
		$("#MegaMenuWidthvalue").hide()
	}else{
		$("#showMegaMenuWidth option").each(function(index, element) {
            if($(this).html()=="Custom"){
				$(this).attr("selected","selected")
			}
        });
	}
	$(".MegaMenuWidth").show();
}else{
	$("#MegaMenuBox").hide()
	$(".MegaMenuWidth").hide()
}
if($("#MegaMenuWidthvalue").find("input").val()==0){
	$("#MegaMenuWidthvalue").hide()
}		

$(".showMegaMenu").on("change", function () {
    if ($(this).find("option:selected").text() == "Mega menu") {
		$("#MegaMenuBox").show();
		$(".MegaMenuWidth").show();
	}else{
		$("#MegaMenuBox").hide()
		$(".MegaMenuWidth").hide()
	}

})

$("#showMegaMenuWidth").on("change", function () {
    if ($(this).find("option:selected").text() == "Auto") {
		$("#MegaMenuWidthvalue").hide();
		$("#MegaMenuWidthvalue").find("input").attr("value","0");
	}else{
		$("#MegaMenuWidthvalue").show()
	}
})

</script>
 
 
 