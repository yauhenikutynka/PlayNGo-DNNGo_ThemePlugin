<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manager_TabList.ascx.cs" Inherits="DNNGo.Modules.xPlugin.Manager_TabList" %>
<!-- start: PAGE HEADER -->
<script src="/DesktopModules/DNNGo_xPlugin/Resource/js/jquery.dragsort-0.5.2.min.js"></script>

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
            <select class="form-control form_default showMegaMenu">
              <option>Standard menu</option>
              <option selected="selected">Mega menu</option>
            </select>
          </div>
          <label  class="col-sm-2 control-label text-right MegaMenuWidth" style="display:none;">Mega Menu Width:</label>
          <div class="col-sm-2 MegaMenuWidth" style="display:none;">
            <input type="text" value="" class="form-control form_default" />  px
          </div>
        </div>
      </div>
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
</div>
<div class="row">
  <div class="col-sm-3"> </div>
  <div class="col-sm-9">
    <input type="submit" value="Update"  class="btn btn-primary" lang="Submit" id="Submit">
    &nbsp;
    <input type="submit" value="Cancel" class="btn btn-default">
    &nbsp; </div>
</div>
<script id="AddPaneTmpl" type="text/x-jquery-tmpl">
  <li class="${tagPane} dragPane" data-tag="${tagPane}" style="width:${PaneWidth}%" >
	<div class="dragLilstBox">
	  <div class="panel-heading"> <i class="fa fa-list-ul"></i>${PaneName}
             <div class="panel-tools ">
				<a class="btn btn-xs btn-bricky PaneDelete"><i class='fa fa-trash-o'></i> Delete</a> 
				<a class="btn btn-xs btn-primary" href="#columnedit" data-toggle="modal"> <i class='fa fa-edit'></i> Edit</a>
				<a href="#addtype" data-toggle="modal" class="btn btn-xs btn-primary"><i class='fa fa-plus-square'></i> Add</a> 
				<a class="btn btn-xs btn-info dragPaneIcon"><i class='fa fa-arrows'></i></a>
			</div>
	  </div>
	  <ul class="menu-list">
		
		<li class="not-itme">请添加条目  </li>
		
	  </ul>
	</div>
  </li>
</script>
<div id="addtype" class="modal fade" tabindex="-1" data-width="760" style="display: none;"></div>
<script id="addtypeTmpl" type="text/x-jquery-tmpl">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"> &times; </button>
    <h4 class="modal-title">添加类型</h4>
  </div>
  <div class="modal-body">
    <div class="panel panel-default">
      <div class="panel-heading"> <i class="fa fa-external-link-square"></i>菜单列表类型
        <div class="panel-tools"><a class="btn btn-xs btn-primary addtypebtn"  data-dismiss="modal" data-addtype="menutype"> <i class='fa clip-checkmark-2'></i> Add</a></div>
      </div>
    </div>
    <div class="panel panel-default">
      <div class="panel-heading"> <i class="fa fa-external-link-square"></i>HTML 类型
        <div class="panel-tools"><a class="btn btn-xs btn-primary addtypebtn"  data-dismiss="modal" data-addtype="htmltype"> <i class='fa clip-checkmark-2'></i> Add</a></div>
      </div>
    </div>
    <div class="panel panel-default">
      <div class="panel-heading"> <i class="fa fa-external-link-square"></i>第三方模块类型
        <div class="panel-tools"><a class="btn btn-xs btn-primary addtypebtn"  data-dismiss="modal" data-addtype="moduletype"> <i class='fa clip-checkmark-2'></i> Add</a></div>
      </div>
    </div>
  </div>
</script> 
<script id="TypeTmpl" type="text/x-jquery-tmpl">
{{if type == "menutype"}}
	<li class="dragList ${tagList}" data-tag="${tagList}">
	  <div class="panel-heading">菜单列表类型 - <span class="type-title">${menutype.name}</span>
		<div class="panel-tools">
		<a class="btn btn-xs btn-bricky listdelete"><i class='fa fa-trash-o' ></i> Delete</a> 
		<a class="btn btn-xs btn-primary"  href="#typedataedit" data-toggle="modal"><i class='fa fa-edit'></i> Edit</a> 
		<a class="btn btn-xs btn-info dragListIcon"><i class='fa fa-arrows'></i></a>
		
		</div>
	  </div>
	</li>
{{/if}}	
{{if type == "htmltype"}}
	<li class="dragList ${tagList}" data-tag="${tagList}">
	  <div class="panel-heading">HTML 类型 - <span class="type-title">${htmltype.name}</span>
		<div class="panel-tools">
		
		<a class="btn btn-xs btn-bricky listdelete"><i class='fa fa-trash-o'></i> Delete</a> 
		<a class="btn btn-xs btn-primary"  href="#typedataedit" data-toggle="modal"><i class='fa fa-edit'></i> Edit</a> 
		<a class="btn btn-xs btn-info dragListIcon"><i class='fa fa-arrows'></i></a>
		
		</div>
	  </div>
	</li>
 {{/if}}	
{{if type == "moduletype"}}
	   <li class="dragList ${tagList}" data-tag="${tagList}">
		  <div class="panel-heading">第三方模块类型 - <span class="type-title">${moduletype.name}</span>
			<div class="panel-tools">
			<a class="btn btn-xs btn-bricky listdelete"><i class='fa fa-trash-o'></i> Delete</a> 
			<a class="btn btn-xs btn-primary"  href="#typedataedit" data-toggle="modal"><i class='fa fa-edit'></i> Edit</a> 
			<a class="btn btn-xs btn-info dragListIcon"><i class='fa fa-arrows'></i></a> 
			</div>
		  </div>
		</li>
 {{/if}}	
</script>
<div id="typedataedit" class="modal fade" tabindex="-1" data-width="760" style="display: none;"> </div>
<script id="typedataeditTmpl" type="text/x-jquery-tmpl">
  <div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"> &times; </button>
    <h4 class="modal-title">菜单列表类型</h4>
  </div>

{{if type == "menutype"}}
  <div class="modal-body form-horizontal">
  
    <div class="row form-group">
      <div class="col-sm-2 text-right control-label"> 菜单列表： </div>
      <div class="col-sm-10">
        <select class="form-control form_default" data-name="name">
          <option {{if menutype.name == "Home"}} selected="selected" {{/if}} >Home</option>
          <option {{if menutype.name == "Portfolios"}} selected="selected" {{/if}}>Portfolios</option>
          <option {{if menutype.name == "Blog"}} selected="selected" {{/if}}>Blog</option>
        </select>
      </div>
    </div>
    <div class="row form-group">
      <div class="col-sm-2 text-right control-label"> Level: </div>
      <div class="col-sm-10">
        <select class="form-control form_default" data-name="level">
          <option value="0">all</option>
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
		 <label class="checkbox-inline" ><input type="checkbox"  class="square-green grey " data-name="displaytitle"/></label>
      </div>
    </div>
    <div class="row form-group">
      <div class="col-sm-2 text-right control-label"  data-name="menusytle"> Menu Sytle: </div>
      <div class="col-sm-10">
	  <div class="radio-img-list" >
		  <label for="Ctl_contentlayout_48636_0" class="radio-inline">
			  <input id="Ctl_contentlayout_48636_0" type="radio" name="Ctl$contentlayout$48636" value="full" checked="checked" class=" grey" />
			  <img src="http://hyw.test.dnngo.net/Portals/4/logo2.png" class="img-responsive"/>
			  
		  </label>
		  <label for="Ctl_contentlayout_48636_1" class="radio-inline">
			  <input id="Ctl_contentlayout_48636_1" type="radio" name="Ctl$contentlayout$48636" value="boxed"  class=" grey" />
			  <img src="http://hyw.test.dnngo.net/Portals/4/logo2.png" class="img-responsive"/>
		  </label>		
	  </div>
      </div>
    </div>
	
  </div>
{{/if}}	

{{if type == "htmltype"}}
  <div class="modal-body form-horizontal ">
  
  	<div class="row form-group">
	 <div class="col-sm-12">
    <p>Title:</p>
	<input type="text" value="${htmltype.name}" data-name="name"/>
	</div>
	</div>
  	<div class="row form-group">
	 <div class="col-sm-12">
    <p>Content Text:</p>
    <textarea class="ckeditor form-control" cols="10" rows="10"   data-name="content">${htmltype.content}</textarea>
	</div>
	</div>
	
  </div>
 {{/if}}	
 
{{if type == "moduletype"}}
  <div class="modal-body form-horizontal">
    <div class="row form-group">
      <div class="col-sm-2 text-right control-label" >
      Pages:
      </div>
      <div class="col-sm-10">
        <select class="form-control form_default"  data-name="pages">
          <option {{if moduletype.pages == "Home"}} selected="selected" {{/if}}>Home</option>
          <option {{if moduletype.pages == "Portfolios"}} selected="selected" {{/if}}>Portfolios</option>
          <option {{if moduletype.pages == "Blog"}} selected="selected" {{/if}}>Blog</option>
        </select>
      </div>
    </div>
    <div class="row form-group">
      <div class="col-sm-2 text-right control-label" >
        Modules:
      </div>
      <div class="col-sm-10">
        <select class="form-control form_default"  data-name="modules">
          <option {{if moduletype.modules == "0"}} selected="selected" {{/if}} value="0">DNN_HTML - Text/HTML</option>
          <option {{if moduletype.modules == "1"}} selected="selected" {{/if}} value="1">DNN_HTML - Text/HTML</option>
          <option {{if moduletype.modules == "2"}} selected="selected" {{/if}} value="2">DNN_HTML - Text/HTML</option>
        </select>
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
    <h4 class="modal-title">列配置</h4>
  </div>
  <div class="modal-body form-horizontal">
    <div class="row  form-group">
      <div class="col-sm-2 text-right control-label"> Pane Width： </div>
      <div class="col-sm-7">
        <input type="text" placeholder=" " class="form-control form_default" value="${PaneWidth}" data-name="PaneWidth" > %
      </div>
    </div>
  </div>
  <div class="modal-footer">
    <button type="button" data-dismiss="modal" class="btn btn-light-grey"> Close </button>
    <button type="button" class="btn btn-blue save-changes" > Save changes </button>
  </div>
</script> 
<script type="text/javascript">

var data = {
	"settings" : {},
	"megamenu" : {
		"Pane_1" : {
			"id" : "1",
			"sort" : "1",
			"tagPane" : "Pane_1",
			"PaneName" : "Pane 1",
			"PaneWidth" : "50",
			"list" : {
				"list_1_1" : {
					"id" : "1",
					"sort" : "1",
					"tagList" : "list_1_1",
					"type" : "menutype",
					"menutype" : {
						"name" : "Portfolios"
					},
					"htmltype" : {
						"name" : "未指定"
					},
					"moduletype" : {
						"name" : "未指定"
					}
				},
				"list_1_2" : {
					"id" : "2",
					"sort" : "2",
					"tagList" : "list_1_2",
					"type" : "htmltype",
					"menutype" : {
						"name" : "Portfolios"
					},
					"htmltype" : {
						"name" : "未指定"
					},
					"moduletype" : {
						"name" : "未指定"
					}
				}
			}
		}
	}
}

var PaneDate = data.megamenu;

for (var itme in PaneDate) {
	$("#AddPaneBox").append(jQuery("#AddPaneTmpl").tmpl(PaneDate[itme]));

	var box = $("." + PaneDate[itme].tagPane).find(".menu-list");
	var listdate = PaneDate[itme].list;
	for (var itme2 in listdate) {
		var tmpls = jQuery("#TypeTmpl").tmpl(listdate[itme2]);

		if (box.find(".dragList").length == 0) {
			box.html(tmpls)
		} else {
			box.append(tmpls)
		}

	}
}

$("#AddPane").on("click", function () {
	PaneLegth = 0;
	for (var itme in PaneDate) {
		PaneLegth = Math.max(PaneLegth, PaneDate[itme].id)
	}
	PaneLegth++;
	var name = "Pane_" + PaneLegth;
	var newsPane = {
		"id" : PaneLegth,
		"tagPane" : name,
		"PaneName" : "Pane " + PaneLegth,
		"PaneWidth" : "25",
		"list" : {}
	}
	PaneDate[name] = newsPane;

	var box = jQuery("#AddPaneTmpl").tmpl(PaneDate["Pane_" + PaneLegth]);
	
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


})

$("body").delegate("a.PaneDelete", "click", function () {
	if (confirm("确定删除这列么 ?")) {
		delete PaneDate[$(this).parents(".dragPane").data("tag")];
		$(this).parents(".dragPane").remove();
	}
})

$("body").delegate("a[href ^= #addtype]", "click", function () {

	var Pane = PaneDate[$(this).parents(".dragPane").data("tag")],
	Pane_list = Pane.list;

	$("#addtype").html(jQuery("#addtypeTmpl").tmpl(Pane));

	$("#addtype .addtypebtn").one("click", function () {

		var TypeLegth = 0;
		for (var itme in Pane_list) {
			TypeLegth = Math.max(TypeLegth, Pane_list[itme].id)
		}

		TypeLegth++;

		var name = "list_" + Pane.id + "_" + TypeLegth;
		var newsType = {
			"id" : TypeLegth,
			"sort" : "1",
			"tagList" : "list_" + Pane.id + "_" + TypeLegth,
			"type" : $(this).data("addtype"),
			"menutype" : {
				"name" : "未指定"
			},
			"htmltype" : {
				"name" : "未指定"
			},
			"moduletype" : {
				"name" : "未指定"
			}
		}
		var box = jQuery("#TypeTmpl").tmpl(newsType);

		if (TypeLegth == 1) {
			$("." + Pane.tagPane).find(".menu-list").html(box)
		} else {
			$("." + Pane.tagPane).find(".menu-list").append(box)
		}
		Pane_list[name] = newsType;
	})

})
$("body").delegate("a.listdelete", "click", function () {

	if (confirm("确定删除这条么 ?")) {
		delete PaneDate[$(this).parents(".dragPane").data("tag")]["list"][$(this).parents(".dragList").data("tag")];
		var p=$(this).parents(".dragPane");
		
		$(this).parents(".dragList").remove();
		
		if(p.find(".dragList").length==0){
			p.find(".menu-list").html("<li class=\"not-itme\">请添加条目 </li>")
		}
		
	}
})
$("body").delegate("a[href ^= #typedataedit]", "click", function () {

    var edit=$(this),
		tag=$(this).parents(".dragList").data("tag");
		
	var itme = PaneDate[$(this).parents(".dragPane").data("tag")]["list"][tag];

		$("#typedataedit").html(jQuery("#typedataeditTmpl").tmpl(itme));

		fromUI($("#typedataedit"))

	$("#typedataedit").find("#typedataeditsavechanges").one("click", function () {

		$("#typedataedit .modal-body").find("input,select,button,textarea").each(function (index, element) {
			itme[itme.type][$(this).data("name")] = $(this).val();
		});
		
		if(itme.type=="moduletype"){
			var modulesname;
		$("#typedataedit select[data-name ^= modules]").find("option").each(function() {
			if($(this).val()== itme[itme.type]["modules"]){
				modulesname=$(this).html();
			}
        });
			itme[itme.type]["name"]=itme[itme.type]["pages"] +" > "+modulesname
		}
					
		$("."+tag).find(".type-title").html(itme[itme.type].name)  
		$(".modal-scrollable").click();
	})

})

$("body").delegate("a[href ^= #columnedit]", "click", function () {
	
	var columnedit = PaneDate[$(this).parents(".dragPane").data("tag")]
		$("#columnedit").html(jQuery("#columneditTmpl").tmpl(columnedit));

	$("#columnedit .save-changes").one("click", function () {
		$("#columnedit .modal-body").find("input,select,button,textarea").each(function (index, element) {
			columnedit[$(this).data("name")] = $(this).val();
		});
		$("." + columnedit.tagPane).innerWidth(columnedit.PaneWidth+"%");
		$(".modal-scrollable").click();
	})
})


/**/
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

var dragParent,dragItme;

$("body").delegate("a.dragListIcon","mousedown", function () {
	 dragParent = $(this).parents(".dragPane").data("tag")
	 dragItme = $(this).parents(".dragPane").data("name")
})


function ListSaveOrder(){
		var e=$(this),
		    odd=$("#AddPaneBox  .dragPane[data-tag ^= "+dragParent+"]"),
			n=e.parents(".dragPane").data("tag"),
			newb=$("#AddPaneBox  .dragPane[data-tag ^= "+n+"]");
		
		if(e.parents(".dragPane").data("tag") == dragParent){
			odd.find(".dragList").each(function(index) {
				PaneDate[dragParent]["list"][$(this).data("tag")].sort=index
			});
			
		}else{

			odd.find(".dragList").each(function(index) {
				PaneDate[dragParent]["list"][$(this).data("tag")].sort=index
			});
			
			if(odd.find(".dragList").length==0){
			odd.find(".menu-list").html("<li class=\"not-itme\">请添加条目  </li>")
			}
			
			
			var lg=0;
			for(i in PaneDate[n]["list"]){
				
				lg=Math.max(lg,PaneDate[n]["list"][i].id)
			}
			lg++;
			var name="list_"+PaneDate[n].id+"_"+lg
			
			PaneDate[n]["list"][name] = PaneDate[dragParent]["list"][e.data("tag")];
			
			delete PaneDate[dragParent]["list"][e.data("tag")];
			
			$(this).data("tag",name);
			PaneDate[n]["list"][name].id=lg;
			PaneDate[n]["list"][name].tagList=name;
		
			newb.find(".not-itme").remove();
		
			newb.find(".dragList").each(function(index) {
				PaneDate[n]["list"][$(this).data("tag")].sort=index
			});
			
			
		}
	
}

function PaneSaveOrder(){
	$("#AddPaneBox .dragPane").each(function(index) {
        PaneDate[$(this).data("tag")].sort=index
    });
}

$("#Submit").click(function(e){
	e.preventDefault()
	console.log(data)
})

if($(".showMegaMenu").val()=="Mega menu"){
	$("#MegaMenuBox").show();
	$(".MegaMenuWidth").show()
}else{
	$("#MegaMenuBox").hide()
	$(".MegaMenuWidth").hide()
}


$(".showMegaMenu").on("change",function(){
	if($(this).val()=="Mega menu"){
		$("#MegaMenuBox").show();
		$(".MegaMenuWidth").show()
	}else{
		$("#MegaMenuBox").hide()
		$(".MegaMenuWidth").hide()
	}

})

function fromUI(e){
	
 e.find('input[type="checkbox"], input[type="radio"]').iCheck({
		checkboxClass: 'icheckbox_square-green',
		radioClass: 'iradio_square-green',
		increaseArea: '10%' // optional
	});

}




</script>
<style>
#MegaMenuBox .placeHolder{
	max-width:100%;
}
#MegaMenuBox .placeHolder div {
	background-color:#f2fcff !important;
	border: dashed 1px gray !important;
    height: 100%;
    vertical-align: middle;
    display: inline-block;
    width: 100%;
    margin-top: -2px;
	list-style:none;
}
#MegaMenuBox .dragPane,
#MegaMenuBox .placeHolder,
#MegaMenuBox .panel.panel-default{
	list-style:none;
}
#MegaMenuBox .AddPaneTitle{
	font-size:14px;
	margin-bottom:10px;
}
#MegaMenuBox .AddPaneTitle h3{
	margin:0;
	padding:0;
}
#MegaMenuBox #AddPaneBox{
	margin:0;
	padding:0;
	list-style:none;
	min-height:200px;
	position:relative;
	margin-bottom:20px;
	margin-right:-1px;
}
#MegaMenuBox #AddPaneBox:before{
	content:"";
	border:1px dashed #e3e3e3;
	position:absolute;
	top:0;
	left:0;
	right:1px;
	bottom:-1px;
}

#MegaMenuBox #AddPaneBox:after{
	content:"";
	clear:both;
	display:block;
	height:0;
}
#MegaMenuBox #AddPaneBox > li{
	float:left;
	margin:0 -1px -1px 0;
}

#MegaMenuBox .menu-list{
	margin:0;
	padding:0;
	list-style:none;
}
#MegaMenuBox .dragLilstBox {
	min-height:74px;
	border:1px solid #ddd;
}
#MegaMenuBox .dragList > .panel-heading{
	background:none;
	padding-left:10px;
	transition: 		background-color ease-in 200ms;
	-moz-transition: 	background-color ease-in 200ms; /* Firefox 4 */
	-webkit-transition:	background-color ease-in 200ms; /* Safari and Chrome */
	-o-transition: 		background-color ease-in 200ms; /* Opera */
	-ms-transition: 	background-color ease-in 200ms; /* IE9? */
}
#MegaMenuBox .dragList > .panel-heading:hover{
	background:#f7f7f7;
}
#MegaMenuBox .menu-list .dragList:last-child .panel-heading{
	border:none;
}
#MegaMenuBox .menu-list .placeHolder + .not-itme{
	display:none;
}
#MegaMenuBox .menu-list .not-itme {
	color:#999;
	text-align:center;
	height:36px;
	line-height:36px;
}
#MegaMenuBox .menu-list .not-itme + .placeHolder {
	display:none;
}

#MegaMenuBox .dragLilstBox > .panel-heading{
	background:#f9f9f9;
	border-radius:0px;
	-moz-border-radius:0px;
	-webkit-border-radius:0px;
}



</style>
<div class="row">
  <div class="col-sm-12"> 
    <!--
          <div class="form-group">
            <div class="row">
              <div class="col-sm-8 ">
               
                  <div class="btn-group">
                   <asp:HyperLink runat="server" ID="hlAllArticle" CssClass="btn btn-default" Text="All"></asp:HyperLink> 
                    <asp:HyperLink runat="server" ID="hlPublishedArticle" CssClass="btn btn-default" Text="Published"></asp:HyperLink>  
                    <asp:HyperLink runat="server" ID="hlPendingArticle" CssClass="btn btn-default" Text="Pending"></asp:HyperLink>   
                    <asp:HyperLink runat="server" ID="hlDraftsArticle" CssClass="btn btn-default" Text="Drafts"></asp:HyperLink> 
                  <asp:HyperLink runat="server" ID="hlRecycleBinArticle" CssClass="btn btn-default" Text="Recycle Bin"></asp:HyperLink> 
                  </div>
              </div>
              <div class="col-sm-4 input-group text_right">
             
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Width="100%" placeholder="Search Text Field" x-webkit-speech></asp:TextBox>
                <div class="input-group-btn">
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary" 
                      Text="<i class='fa fa-search'></i>" onclick="btnSearch_Click" />
				</div>
              
              </div>
            </div>
          </div>
          -->
    <div class="form-group">
      <div class="row">
        <div class="col-sm-9"> 
          <!--
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control form_default">
                    <asp:ListItem Value="-1" Text="Bulk Actions"  resourcekey="ddlStatus_BulkActions"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Published" resourcekey="ddlStatus_Published"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Pending" resourcekey="ddlStatus_Pending"></asp:ListItem>
                    <asp:ListItem Value="4" Text="Delete" resourcekey="ddlStatus_Delete"></asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnApply" runat="server" CssClass="btn btn-default" Text="Apply" resourcekey="btnApply" onclick="btnApply_Click" OnClientClick="return ApplyStatus();" />
                --> 
          
        </div>
        <div class="col-sm-3 text_right"> <br/>
          <!--
          <asp:Label ID="lblRecordCount" runat="server"></asp:Label>
           --> 
        </div>
      </div>
    </div>
    <div class="form-group"> 
      <!--
      <asp:GridView ID="gvArticleList" runat="server" AutoGeneratePanes="False" OnRowDataBound="gvArticleList_RowDataBound" OnRowCreated="gvArticleList_RowCreated" OnSorting="gvArticleList_Sorting" AllowSorting="true"
                        Width="100%" CellPadding="0" cellspacing="0" border="0" CssClass="table table-striped table-bordered table-hover"  GridLines="none" >
        <Columns>
        <asp:BoundField DataField="IndentedTabName" HeaderText="TabName"  />
        <asp:BoundField DataField="Title" HeaderText="Title"  HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" />
        <asp:BoundField DataField="IsVisible" HeaderText="Visible" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" />
        <asp:BoundField DataField="DisableLink" HeaderText="DisableLink" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" />
        <asp:BoundField DataField="TabType" HeaderText="TabType" HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" />
        <asp:BoundField DataField="CreatedOnDate" HeaderText="CreatedDate" DataFormatString="{0:d}"  HeaderStyle-CssClass="hidden-xs" ItemStyle-CssClass="hidden-xs" />
        <asp:TemplateField HeaderText="Icon" HeaderStyle-CssClass="visible-md visible-lg hidden-sm hidden-xs" ItemStyle-CssClass="visible-md visible-lg hidden-sm hidden-xs" HeaderStyle-Width="150" >
          <ItemTemplate>
            <div class="visible-md visible-lg hidden-sm hidden-xs">
              <asp:HyperLink ID="hlIconEdit" runat="server" CssClass="tooltips" data-original-title="Edit Icon" data-placement="top" Text="[Edit]"></asp:HyperLink>
            </div>
          </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Background" HeaderStyle-CssClass="visible-md visible-lg hidden-sm hidden-xs"  ItemStyle-CssClass="visible-md visible-lg hidden-sm hidden-xs" HeaderStyle-Width="100" >
          <ItemTemplate>
            <div class="visible-md visible-lg hidden-sm hidden-xs">
              <asp:HyperLink ID="hlBackgroundEdit" runat="server" CssClass="btn btn-xs btn-teal tooltips" data-original-title="Edit Background" data-placement="top" Text="<i class='fa clip-images'></i>"></asp:HyperLink>
            </div>
          </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Breadcrumb" HeaderStyle-CssClass="visible-md visible-lg hidden-sm hidden-xs"  ItemStyle-CssClass="visible-md visible-lg hidden-sm hidden-xs" HeaderStyle-Width="100">
          <ItemTemplate>
            <div class="visible-md visible-lg hidden-sm hidden-xs">
              <asp:HyperLink ID="hlBreadcrumbEdit" runat="server" CssClass="btn btn-xs btn-teal tooltips" data-original-title="Edit Breadcrumb" data-placement="top" Text="<i class='fa clip-images'></i>"></asp:HyperLink>
            </div>
          </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Megamenu" HeaderStyle-CssClass="visible-md visible-lg hidden-sm hidden-xs"  ItemStyle-CssClass="visible-md visible-lg hidden-sm hidden-xs" HeaderStyle-Width="100">
          <ItemTemplate>
            <div class="visible-md visible-lg hidden-sm hidden-xs">
              <asp:HyperLink ID="hlMegamenuEdit" runat="server" CssClass="btn btn-xs btn-teal tooltips" data-original-title="Edit Megamenu" data-placement="top" Text="<i class='fa clip-images'></i>"></asp:HyperLink>
            </div>
          </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Action" HeaderStyle-CssClass="visible-xs visible-sm hidden-md hidden-lg" ItemStyle-CssClass="center visible-xs visible-sm hidden-md hidden-lg" HeaderStyle-Width="100">
          <ItemTemplate>
            <div class="visible-xs visible-sm hidden-md hidden-lg">
              <div class="btn-group"> <a href="#" data-toggle="dropdown" class="btn btn-primary dropdown-toggle btn-sm"> <i class="fa fa-cog"></i> <span class="caret"></span> </a>
                <ul class="dropdown-menu pull-right" role="menu">
                  <li role="presentation">
                    <asp:HyperLink ID="hlMobileBackgroundEdit" runat="server" tabindex="-1" role="menuitem" Text="<i class='fa clip-images'></i> Edit Background"></asp:HyperLink>
                  </li>
                  <li role="presentation">
                    <asp:HyperLink ID="hlMobileBreadcrumbEdit" runat="server" tabindex="-1" role="menuitem" Text="<i class='fa clip-images'></i> Edit Breadcrumb"></asp:HyperLink>
                  </li>
                  <li role="presentation">
                    <asp:HyperLink ID="hlMobileMegamenuEdit" runat="server" tabindex="-1" role="menuitem" Text="<i class='fa clip-images'></i> Edit Megamenu"></asp:HyperLink>
                  </li>
                </ul>
              </div>
            </div>
          </ItemTemplate>
        </asp:TemplateField>
        </Columns>
        <PagerSettings Visible="False" />
      </asp:GridView>
      --> 
      <!--<ul id="paginator-ArticleList" class="pagination-purple"></ul>
                    <script type="text/javascript">
                        $(document).ready(function () {
                            $('#paginator-ArticleList').bootstrapPaginator({
                                bootstrapMajorVersion: 3,
                                currentPage: <%=PageIndex %>,
                                totalPages: <%=RecordPages %>,
                                numberOfPages:7,
                                useBootstrapTooltip:true,
                                onPageClicked: function (e, originalEvent, type, page) {
                                    window.location.href='<%=CurrentUrl %>&PageIndex='+ page;
                                }
                            });
                        });
                    </script>--> 
    </div>
  </div>
  
  <!-- end: PAGE CONTENT--> 
</div>
<!--<script type="text/javascript">


     function SelectAll() {
         var e = document.getElementsByTagName("input");
         var IsTrue;
         if (document.getElementById("CheckboxAll").value == "0") {
             IsTrue = true;
             document.getElementById("CheckboxAll").value = "1"
         }
         else {
             IsTrue = false;
             document.getElementById("CheckboxAll").value = "0"
         }
         for (var i = 0; i < e.length; i++) {
             if (e[i].type == "checkbox") {
                 e[i].checked = IsTrue;
             }
         }
     }
     function ApplyStatus() {
         var StatusSelected = $("#<%=ddlStatus.ClientID %>").find("option:selected").val();
         if (StatusSelected >= 0) {
             var checkok = false;
             $("#<%=gvArticleList.ClientID %> input[type='checkbox'][type-item='true']").each(function (i, n) {
                 if ($(this).prop('checked')) {
                     checkok = true;
                 }
             });

             if (checkok) {
                 return confirm('<%=ViewResourceText("Confirm_ApplyStatus", "Are you sure to operate the records you choose?") %>');
             }
             alert('<%=ViewResourceText("Alert_NoItems", "Please operate with one line of record selected at least.") %>');

         } else {
             alert('<%=ViewResourceText("Alert_NoActions", "Please choose the operation you need.") %>');
         }
         return false;
     }
   
 
//
    </script> 
   -->