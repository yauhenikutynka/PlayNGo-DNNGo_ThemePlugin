(function($){$.fn.extend({accordionpromulti:function(options){var defaults={accordionpro:'true',speed:300,closedSign:'+',openedSign:'-'};var opts=$.extend(defaults,options);var $this=$(this);$this.find("li a span.menu_arrow").click(function(){var link=false;$(this).parent("a").click(function(){if(link==false){link=true;return false;}});if($(this).parent().parent().find("ul").size()!=0){if(opts.accordionpro){if(!$(this).parent().parent().find("ul").is(':visible')){parents=$(this).parent().parent().parents("ul");visible=$this.find("ul:visible");visible.each(function(visibleIndex){var close=true;parents.each(function(parentIndex){if(parents[parentIndex]==visible[visibleIndex]){close=false;return false;}});if(close){if($(this).parent().parent().find("ul")!=visible[visibleIndex]){$(visible[visibleIndex]).slideUp(opts.speed,function(){$(this).parent("li").find("span.menu_arrow:first").html(opts.closedSign).removeClass().addClass("menu_arrow arrow_opened");$(this).siblings("a").removeClass("current");$(this).parent("li").removeClass("active");});}}});}}
if($(this).parent().parent().find("ul:first").is(":visible")){$(this).parent().parent().find("ul:first").slideUp(opts.speed,function(){$(this).parent("li").find("span.menu_arrow:first").delay(opts.speed).html(opts.closedSign).removeClass().addClass("menu_arrow arrow_opened");$(this).siblings("a").removeClass("current");$(this).parent("li").removeClass("active");});}else{$(this).parent().parent().find("ul:first").slideDown(opts.speed,function(){$(this).parent("li").find("span.menu_arrow:first").delay(opts.speed).html(opts.openedSign).removeClass().addClass("menu_arrow arrow_closed");$(this).siblings("a").addClass("current");$(this).parent("li").addClass("active");});}}});}});})(jQuery);


(function($){$.fn.extend({accordionprohover:function(options){var defaults={accordionpro:'true',speed:300,closedSign:'+',openedSign:'-'};var opts=$.extend(defaults,options);var $this=$(this);var interval;var io=true;var events=function(o){if(o.hasClass("menu_arrow")){var link=false;var e=o;e.parent("a").on("click",function(){if(link==false){link=true;return false;}});}else{var e=o.children(".menu_arrow")
if(e.length==0){return false;}}
if(!io)return false;if(e.parent().parent().find("ul").size()!=0){if(opts.accordionpro){if(!e.parent().parent().find("ul").is(':visible')){parents=e.parent().parent().parents("ul");visible=$this.find("ul:visible");visible.each(function(visibleIndex){var close=true;parents.each(function(parentIndex){if(parents[parentIndex]==visible[visibleIndex]){close=false;return false;}});if(close){if(e.parent().parent().find("ul")!=visible[visibleIndex]){io=false;$(visible[visibleIndex]).slideUp(opts.speed,function(){$(this).parent("li").find("span.menu_arrow:first").html(opts.closedSign).removeClass().addClass("menu_arrow arrow_opened");$(this).siblings("a").removeClass("current");$(this).parent("li").removeClass("active");io=true;});}}});}}
if(e.parent().parent().find("ul:first").is(":visible")){if(o.hasClass("menu_arrow")){io=false;e.parent().parent().find("ul:first").slideUp(opts.speed,function(){$(this).parent("li").find("span.menu_arrow:first").delay(opts.speed).html(opts.closedSign).removeClass().addClass("menu_arrow arrow_opened");$(this).siblings("a").removeClass("current");$(this).parent("li").removeClass("active");io=true;});}}else{io=false;e.parent().parent().find("ul:first").slideDown(opts.speed,function(){$(this).parent("li").find("span.menu_arrow:first").delay(opts.speed).html(opts.openedSign).removeClass().addClass("menu_arrow arrow_closed");$(this).siblings("a").addClass("current");$(this).parent("li").addClass("active");io=true;});}}}
$this.find("li a").on("mouseover",function(){var e=$(this);interval=setTimeout(function(){events(e)},200);}).on("mouseout",function(){clearTimeout(interval);});$this.find("li a span.menu_arrow").on("click",function(){events($(this))});}});})(jQuery);


/*!
 * jQuery Mousewheel 3.1.13
 *
 * Copyright 2015 jQuery Foundation and other contributors
 * Released under the MIT license.
 * http://jquery.org/license
 */
!function(a){"function"==typeof define&&define.amd?define(["jquery"],a):"object"==typeof exports?module.exports=a:a(jQuery)}(function(a){function b(b){var g=b||window.event,h=i.call(arguments,1),j=0,l=0,m=0,n=0,o=0,p=0;if(b=a.event.fix(g),b.type="mousewheel","detail"in g&&(m=-1*g.detail),"wheelDelta"in g&&(m=g.wheelDelta),"wheelDeltaY"in g&&(m=g.wheelDeltaY),"wheelDeltaX"in g&&(l=-1*g.wheelDeltaX),"axis"in g&&g.axis===g.HORIZONTAL_AXIS&&(l=-1*m,m=0),j=0===m?l:m,"deltaY"in g&&(m=-1*g.deltaY,j=m),"deltaX"in g&&(l=g.deltaX,0===m&&(j=-1*l)),0!==m||0!==l){if(1===g.deltaMode){var q=a.data(this,"mousewheel-line-height");j*=q,m*=q,l*=q}else if(2===g.deltaMode){var r=a.data(this,"mousewheel-page-height");j*=r,m*=r,l*=r}if(n=Math.max(Math.abs(m),Math.abs(l)),(!f||f>n)&&(f=n,d(g,n)&&(f/=40)),d(g,n)&&(j/=40,l/=40,m/=40),j=Math[j>=1?"floor":"ceil"](j/f),l=Math[l>=1?"floor":"ceil"](l/f),m=Math[m>=1?"floor":"ceil"](m/f),k.settings.normalizeOffset&&this.getBoundingClientRect){var s=this.getBoundingClientRect();o=b.clientX-s.left,p=b.clientY-s.top}return b.deltaX=l,b.deltaY=m,b.deltaFactor=f,b.offsetX=o,b.offsetY=p,b.deltaMode=0,h.unshift(b,j,l,m),e&&clearTimeout(e),e=setTimeout(c,200),(a.event.dispatch||a.event.handle).apply(this,h)}}function c(){f=null}function d(a,b){return k.settings.adjustOldDeltas&&"mousewheel"===a.type&&b%120===0}var e,f,g=["wheel","mousewheel","DOMMouseScroll","MozMousePixelScroll"],h="onwheel"in document||document.documentMode>=9?["wheel"]:["mousewheel","DomMouseScroll","MozMousePixelScroll"],i=Array.prototype.slice;if(a.event.fixHooks)for(var j=g.length;j;)a.event.fixHooks[g[--j]]=a.event.mouseHooks;var k=a.event.special.mousewheel={version:"3.1.12",setup:function(){if(this.addEventListener)for(var c=h.length;c;)this.addEventListener(h[--c],b,!1);else this.onmousewheel=b;a.data(this,"mousewheel-line-height",k.getLineHeight(this)),a.data(this,"mousewheel-page-height",k.getPageHeight(this))},teardown:function(){if(this.removeEventListener)for(var c=h.length;c;)this.removeEventListener(h[--c],b,!1);else this.onmousewheel=null;a.removeData(this,"mousewheel-line-height"),a.removeData(this,"mousewheel-page-height")},getLineHeight:function(b){var c=a(b),d=c["offsetParent"in a.fn?"offsetParent":"parent"]();return d.length||(d=a("body")),parseInt(d.css("fontSize"),10)||parseInt(c.css("fontSize"),10)||16},getPageHeight:function(b){return a(b).height()},settings:{adjustOldDeltas:!0,normalizeOffset:!0}};a.fn.extend({mousewheel:function(a){return a?this.bind("mousewheel",a):this.trigger("mousewheel")},unmousewheel:function(a){return this.unbind("mousewheel",a)}})});

(function ($) {
	$.fn.menusKeyboard =function(){
				
		var menus=$(this);
		var curr = menus.find(":focus");
		var level_1=true;
		var drop = false;
		var keyCode;
		var shiftKey;
		
		
		function menusondownkey(e,key){
			switch(key){		 			
				case 9:				
					menusonfocus("tab");
				break;
				case 13:
				case 32:
				if(curr.parent("li").hasClass("dir") && !curr.parent("li").hasClass("menu_hover")){
					e.preventDefault();
					menusonfocus("enter");
				}
				break;
				case 37:
				e.preventDefault();
				menusonfocus("left");
				break;
				case 38:
				e.preventDefault();
				menusonfocus("up");
				break;
				case 39:
				e.preventDefault();
				menusonfocus("right");
				break;
				case 40:
				e.preventDefault();
				menusonfocus("down");
				break;
				case 27:
				e.preventDefault();
				menusonfocus("end");
				break;				
			//	case 123:
				//e.preventDefault();
			//	menusonfocus("first");
			//	break;				
				
			}
	
		}
		function menusonup(){
			if(curr.parent("li").prev().length){
				curr.parent("li").prev().mouseenter().children("a").focus();
			}else{
				curr.parent("li").siblings().last().mouseenter().children("a").focus();
			}			
		}
		function menusondown(){
			drop=true;
			if(level_1){
				curr.parent().mouseenter();
				setTimeout(function(){
					curr.siblings("div").find("a").eq(0).focus().mouseenter();
					curr = menus.find(":focus");		
				},150);
			}else{ 
				if(curr.parent("li").next().length){
					curr.parent("li").next().mouseenter().children("a").focus();
				}else{
					curr.parent("li").siblings().first().mouseenter().children("a").focus();
				}
			}
		}
		function menusonright(){
			if(level_1){
				if(curr.parent("li").nextAll().children("a").first().length){
					drop?curr.parent("li").nextAll().children("a").first().focus().parent().mouseenter():curr.parent("li").nextAll().children("a").first().focus();
	
				}else{
					drop?curr.parent("li").siblings().prevAll().children("a").last().focus().parent().mouseenter():curr.parent("li").siblings().prevAll().children("a").last().focus();
				}
			}else{
				setTimeout(function(){
					if(curr.siblings("div").length){
						curr.siblings("div").find("a").eq(0).focus();							
					}else{
						if(curr.parents("li.dir").last().nextAll().children("a").length){
							curr.parents("li.dir").last().nextAll().children("a").first().focus().parent().mouseenter();
						
						}else{
							curr.parents("li.dir").last().prevAll().mouseenter().children("a").last().focus().parent().mouseenter();		
						}															
					}
					curr = menus.find(":focus");	
				},100);
			}
		}
		function menusonleft(){
			if(level_1){
				
				if(curr.parent("li").prevAll().children("a").first().length){
					drop?curr.parent("li").prevAll().children("a").first().focus().parent().mouseenter():curr.parent("li").prevAll().children("a").first().focus();						
				}else{
					drop?curr.parent("li").siblings().nextAll().children("a").last().focus().parent().mouseenter():curr.parent("li").siblings().nextAll().children("a").last().focus();
				}
	
			}else{
				curr.parent("li").parents("li.dir").first().mouseenter().children("a").focus();					
			}
		}
		function menusonfocus(action){
	
			switch(action){	
				case "first":
					menus.find("a").eq(0).focus();	
				break;
				case "down":
					menusondown();		
				break;
				case "up":
					menusonup();		
				break;
				case "right":
					if(curr.parents(".dnngo_boxslide").length){
						menusondown();
					}else{
						menusonright();	
					}
				break;
				case "left":
					if(curr.parents(".dnngo_boxslide").length){
						menusonup();
					}else{
						menusonleft();		
					}
				break;
				case "enter":				
					curr.parent().mouseenter();
				break;
				case "end":	
					if(!level_1){
						curr.parents("li.dir").last().mouseenter().children("a").focus();
					}
					curr.mouseleave();
					drop=false;	
	
				break;
				 
	
				curr = menus.find(":focus");
	
				if(curr.parents("ul").eq(0).hasClass("primary_structure")){
					curr.parent().mouseleave();
					level_1=true;
				}else{
					level_1=false;
				}
	
			}
		}
		document.onkeydown=function(e){	
			
			 keyCode = e.keyCode || e.which || e.charCode; 
			 shiftKey = e.shiftKey ;
			if(menus.find("a").is(":focus") || keyCode==123){
				//curr.mouseout();
				curr = menus.find(":focus");

				if(curr.parents("ul").eq(0).hasClass("primary_structure")){
					level_1=true;
				}else{
					level_1=false;
				}

				if(shiftKey && keyCode == 123) { 
					e.preventDefault();
					menusonfocus("first");	
				}else if(shiftKey && keyCode == 9) {
					menusonfocus("backtab");	
				}else{
					menusondownkey(e,e.keyCode);
				}	
			}
			
		};		 
		document.onkeyup=function(e){
			 
			var keyCode = e.keyCode || e.which || e.charCode;
			if(keyCode == 9) {
				curr = menus.find(":focus");
				
				if(curr.parents("ul").eq(0).hasClass("primary_structure") && drop){
	
					curr.parent().mouseenter();
				}
				if(!curr.parents("ul").eq(0).hasClass("primary_structure")){
					curr.parent().mouseenter();
				}
			}
			 
		};
	};
	})(jQuery);

(function ($) {
	$.fn.dnngomegamenu = function (m) {
		m = $.extend({
				slide_speed : 200,
				delay_show : 150,
				delay_disappear : 500,
				megamenuwidth : "box",
				WidthBoxClassName : ".dnn_layout",
				popUp : "vertical",
				direction : "ltr"
			}, m || {});
		var rtl = m.direction == "rtl" ? true : false;
		return this.each(function (index) {
			var me = $(this),
			primary = me.find(".primary_structure > li"),
			slide = ".dnngo_menuslide",
			subs = ".dnngo_submenu",
			subbox = "dnngo_boxslide",
			hover = "menu_hover",
			slidedefault = "dnngo_slide_menu",
			interval,
			interval2;
			if (rtl) {
				me.addClass("rtl")
			}
			if (!!('ontouchstart' in window)) {
				primary.children("a").on('click', function () {
					if ($(this).siblings("div").css("display") == "none") {
						return false;
					}
				})
			}
			primary.mouseenter(function () {
				var e = $(this),
				slides = e.find(slide);
				clearTimeout(interval);
				clearTimeout(interval2);
				interval2 = setInterval(function () {
						if (slides.css('display') == 'none') {
							e.addClass("menu_hover");
							slides.attr("style"," ");
							var space=20;
							var winwidth = $(window).width()-space,
							width = slides.width();
							var c_width=slides.data("width");
								c_width==0?c_width=false:"";
							var posBox = $(m.WidthBoxClassName).last();
							
							if (m.popUp == "vertical") {
								var left = e.offset().left;
								if (slides.find("ul").hasClass(slidedefault)) {
									if (winwidth - left < width) {
										slides.css("left", '-' + parseInt(width + left - winwidth + 5) + 'px');
									}
								}
								if(c_width){
									if (slides.find("div").hasClass(subbox)) {
										var position=slides.data("position")?slides.data("position"):0;	
										offset=e.innerWidth()/2;
										
										c_width=Math.min(c_width,winwidth);
										
										
										if(m.megamenuwidth=="box"){
											c_width=Math.min(c_width,posBox.innerWidth());
											var posleft= posBox.offset().left;
										}else{
											c_width=Math.min(c_width,winwidth);
											var posleft= space/2;
										}
										var maxleft=left-posleft;
										var maxright=left-posleft-c_width;

										if(position==0){
											var cur= 0;
										}else if(position==1){
											var cur= c_width/2-offset;
										}else if(position==2){
											var cur= c_width-e.innerWidth();
										}
										
										
										var ju = cur;
										if( ju > left-posleft){
											cur=left-posleft;
										}
										if(left+c_width - ju > posBox.innerWidth() + posleft){
											cur=left+c_width-(posBox.innerWidth() + posleft)
										}
										slides.css({
											"width" : c_width,
											"left" : -cur
										})
									}
								}else{
									
									if (m.megamenuwidth == "full") {
										if (slides.find("div").hasClass(subbox)) {
											slides.css({
												"width" : winwidth,
												"max-width" : winwidth,
												"left" : -left + space/2
											})
										}
									}
									if (m.megamenuwidth == "box") {
										if (slides.find("div").hasClass(subbox)) {
											slides.css({
												"width" : posBox.innerWidth(),
												"max-width" : winwidth,
												"left": posBox.offset().left - left - (Math.min(posBox.innerWidth(),winwidth)-posBox.innerWidth())/2
											})
										}
									}
								}
							}
							if (m.popUp == "level") {
								if (slides.find("ul").hasClass(slidedefault)) {
									if (rtl) {
										slides.css({
											"right" : "100%",
											"left" : "auto"
										});
									} else {
										slides.css("left", "100%");
									}
								}
								if (m.megamenuwidth == "box") {
									var subwidth = $(m.WidthBoxClassName).last().innerWidth();
								} else {
									var subwidth = $(window).width();
									if(c_width){
									subwidth = Math.min($(window).width(),c_width);	
									}
								}
								if (slides.find("div").hasClass(subbox)) {
									if (rtl) {
										slides.css({
											"width" : subwidth,
											"max-width" : slides.parent().offset().left-space/2,
											"right" : "100%",
											"left" : "auto"
										})
									} else {
										slides.css({
											"width" : subwidth,
											"max-width" : winwidth - slides.parent().offset().left - slides.parent().innerWidth()+space/2,
											"left" : "100%"
										})
									}
								}
								var top = e.offset().top - $(window).scrollTop(),
								winheight = $(window).height(),
								height = slides.height();
								if (winheight < height + top) {
									if (winheight <= height) {
										slides.css({
											"top" : -top
										})
									} else {
										slides.css({
											"top" : winheight - (top + height)
										})
									}
								} else {
									slides.css({
										"top" : 0
									})
								}
							}
							slides.fadeIn(m.slide_speed);
						}
						clearTimeout(interval2);
					}, m.delay_show);
				e.siblings().find(slide).fadeOut(m.slide_speed);
				e.siblings().find(subs).fadeOut(m.slide_speed);
				e.siblings().find(slide).find("li").removeClass(hover);
				e.siblings().find(subs).find("li").removeClass(hover);
				e.siblings().removeClass(hover);
			}).mouseleave(function () {
				var e = $(this);
				clearTimeout(interval2);
				interval = setInterval(function () {
						e.removeClass(hover);
						e.find("li").removeClass(hover);
						e.find(slide).fadeOut(m.slide_speed);
						e.find(subs).fadeOut(m.slide_speed);
						clearTimeout(interval);
					}, m.slide_speed > m.delay_disappear ? m.slide_speed : m.delay_disappear);
			})
			primary.find("li").mouseenter(function () {
				var subbox = $(this).find("> " + subs);
				if (subbox.css('display') == 'none') {
					$(this).addClass(hover);
					subbox.fadeIn(m.slide_speed);
					sub_l = $(this).offset().left;
					sub_left = sub_l + $(this).width(),
					winwidth = $(window).width(),
					sub_width = subbox.width();
					if (rtl) {
						if (sub_l < sub_width) {
							subbox.css({
								"left" : "100%",
								"right" : "auto"
							});
						} else {
							subbox.css({
								"left" : "auto",
								"right" : "100%"
							});
						}
					} else {
						if (winwidth - sub_left < sub_width) {
							subbox.css({
								"left" : "auto",
								"right" : "100%"
							});
						} else {
							subbox.css({
								"left" : "100%",
								"right" : "auto"
							});
						}
					}
					if (m.popUp == "level") {
						var top = $(this).offset().top - $(window).scrollTop(),
						winheight = $(window).height(),
						height = subbox.height();
						if (winheight < height + top) {
							if (winheight <= height) {
								subbox.css({
									"top" : -top
								})
							} else {
								subbox.css({
									"top" : winheight - (top + height)
								})
							}
						} else {
							subbox.css({
								"top" : 0
							})
						}
					}
				}
				$(this).siblings().removeClass(hover);
				$(this).siblings().find(subs).fadeOut(m.slide_speed);
			})
			function roller(e,defaultTop){
			if (e.offset().top + e.height() - $(window).scrollTop() > $(window).height()) {
			
			
			var s_top = $(window).scrollTop(),
				h = e.innerHeight(),
				w_h = $(window).height(),
				e_top = 0,
				p_height = e.parent().innerHeight(),
				n_w = false,
				min_top,
				max_top,
				rollerEv;
				e.addClass("roller");
			
			var up =$("<div class=\"roller-up\"></div>");
			var down = $("<div class=\"roller-down\"></div>")
			up.css({
				"width":e.width(),
			//	"left":e.offset().left
			})
			down.css({
				"width":e.width(),
			//	"left":e.offset().left
			})
			
			up.insertBefore(e.children("ul,div.dnngo_boxslide")).hide();
			down.insertAfter(e.children("ul,div.dnngo_boxslide"));
			if(e.hasClass("dnngo_submenu") || m.popUp == "level" ){
				p_height=0;
			}
			
			

			if (h < w_h) {
				min_top = p_height;
				max_top =  - (e.offset().top - s_top - (w_h - h) - p_height);
				up.remove();
				n_w = true;
				
			} else {
				if( m.popUp == "level"){
					min_top =  - (e.offset().top  - s_top - p_height) + parseInt(defaultTop);
					max_top =  - (h + e.offset().top - w_h - s_top - p_height) + parseInt(defaultTop);
				}else{
					min_top =  - (e.offset().top  - s_top - p_height);
					max_top =  - (h + e.offset().top - w_h - s_top - p_height);
				}
			}
			var rollerEv;
			
			function up_d() {
				e_top = parseInt(e.css("top")) + 30;
			
				down.show();
			
				if (e_top >= min_top) {
					e.css("top", min_top)
					clearInterval(rollerEv);
					up.hide();
				} else {
					e.css("top", e_top)
				}
			
			}
			function down_d() {
				e_top = parseInt(e.css("top")) - 30;
			
				if (e_top < min_top) {
					up.show()
				} else {
					up.hide()
				}
			
				if (e_top <= max_top) {
					e.css("top", max_top)
					clearInterval(rollerEv);
					down.hide();
				} else {
					e.css("top", e_top)
				}
			}
			up.on("mouseenter", function () {
				rollerEv = setInterval(up_d, 100);
			}).on("mouseleave", function () {
				window.clearInterval(rollerEv);
			})
			down.on("mouseenter", function () {
				rollerEv = setInterval(down_d, 100);
			}).on("mouseleave", function () {
				window.clearInterval(rollerEv);
			})
			
			e.on('mousewheel', function (event) {
				e_top = parseInt(e.css("top")) + event.deltaY * 50;
			
				if (!n_w) {
					if (e_top > min_top) {
			
						if (event.deltaY < 0) {
							e.stop().css({"top":Math.max(e_top, min_top)})
							up.show();
						}
						
						if (event.deltaY > 0 && parseInt(e.css("top")) <= min_top ) {
							e.stop().css({"top":min_top});
							down.show();
						}
						up.hide();
			
					} else if (e_top <= max_top) {
						e.stop().css({"top": max_top})
						down.hide();
						if (event.deltaY < 0 && parseInt(e.css("top")) >= min_top ) {
						up.show();
						}
						
					} else {
						e.stop().css({"top": e_top})
						up.show();
						down.show();
					}
				} else {
					if (event.deltaY < 0) {
						e.stop().css({"top": Math.max(e_top, max_top)})
						if (e_top <= max_top) {
							down.hide();
						}
					}
				}
				event.stopPropagation();
				event.preventDefault();
			});
			
			}
			}
			function removeroller(e,defaultTop){
			if (e.hasClass("roller")) {
				e.css("top",defaultTop);
				e.removeClass("roller");
				e.find(".roller-up , .roller-down").remove();
				e.unbind('mousewheel')
			}
			
			}			 
			me.find(".dnngo_menuslide,.dnngo_submenu").each(function (index, element) {
			var e=$(this),
			defaultTop=e.css("top"),
			rollerinterval; 
			
			e.on("mouseenter", function () {
			if(!e.hasClass("roller")){
				if( m.popUp == "level"){
					defaultTop=e.css("top");
				}
				roller(e,defaultTop)
			}
			clearTimeout(rollerinterval);
			
			})
			if(e.hasClass("dnngo_menuslide")){
			e.on("mouseleave", function () {
				rollerinterval = setTimeout(function () {removeroller(e,defaultTop)}, m.slide_speed > m.delay_disappear ? m.slide_speed+m.slide_speed : m.delay_disappear+m.slide_speed );		
				
			})
			}
			if(e.hasClass("dnngo_submenu")){
			e.parent("li").on("mouseleave", function () { 
				var te=$(this).children(".dnngo_submenu");
				rollerinterval = setTimeout(function () {removeroller(te,defaultTop)}, m.slide_speed > m.delay_disappear ? m.slide_speed+m.slide_speed : m.delay_disappear+m.slide_speed );		
				
			})
			
			}
			});
		});
	};
})(jQuery);



(function($){$.fn.hoverIntent=function(f,g){var cfg={sensitivity:7,interval:100,timeout:0};cfg=$.extend(cfg,g?{over:f,out:g}:f);var cX,cY,pX,pY;var track=function(ev){cX=ev.pageX;cY=ev.pageY};var compare=function(ev,ob){ob.hoverIntent_t=clearTimeout(ob.hoverIntent_t);if((Math.abs(pX-cX)+Math.abs(pY-cY))<cfg.sensitivity){$(ob).unbind("mousemove",track);ob.hoverIntent_s=1;return cfg.over.apply(ob,[ev])}else{pX=cX;pY=cY;ob.hoverIntent_t=setTimeout(function(){compare(ev,ob)},cfg.interval)}};var delay=function(ev,ob){ob.hoverIntent_t=clearTimeout(ob.hoverIntent_t);ob.hoverIntent_s=0;return cfg.out.apply(ob,[ev])};var handleHover=function(e){var ev=jQuery.extend({},e);var ob=this;if(ob.hoverIntent_t){ob.hoverIntent_t=clearTimeout(ob.hoverIntent_t)}if(e.type=="mouseenter"){pX=ev.pageX;pY=ev.pageY;$(ob).bind("mousemove",track);if(ob.hoverIntent_s!=1){ob.hoverIntent_t=setTimeout(function(){compare(ev,ob)},cfg.interval)}}else{$(ob).unbind("mousemove",track);if(ob.hoverIntent_s==1){ob.hoverIntent_t=setTimeout(function(){delay(ev,ob)},cfg.timeout)}}};return this.bind('mouseenter',handleHover).bind('mouseleave',handleHover)}})(jQuery);





(function($){$.fn.mobile_menu=function(op){var sf=$.fn.mobile_menu,c=sf.c,$arrow=$(['<span class="',c.arrowClass,'"> &#187;</span>'].join('')),over=function(){var $$=$(this),menu=getMenu($$);clearTimeout(menu.sfTimer);$$.showmobile_menuUl().siblings().hidemobile_menuUl();},out=function(){var $$=$(this),menu=getMenu($$),o=sf.op;clearTimeout(menu.sfTimer);menu.sfTimer=setTimeout(function(){o.retainPath=($.inArray($$[0],o.$path)>-1);$$.hidemobile_menuUl();if(o.$path.length&&$$.parents(['li.',o.hoverClass].join('')).length<1){over.call(o.$path);}},o.delay);},getMenu=function($menu){var menu=$menu.parents(['ul.',c.menuClass,':first'].join(''))[0];sf.op=sf.o[menu.serial];return menu;},addArrow=function($a){$a.addClass(c.anchorClass).append($arrow.clone());};return this.each(function(){var s=this.serial=sf.o.length;var o=$.extend({},sf.defaults,op);o.$path=$('li.'+o.pathClass,this).slice(0,o.pathLevels).each(function(){$(this).addClass([o.hoverClass,c.bcClass].join(' ')).filter('li:has(ul)').removeClass(o.pathClass);});sf.o[s]=sf.op=o;$('li:has(ul)',this)[($.fn.hoverIntent&&!o.disableHI)?'hoverIntent':'hover'](over,out).each(function(){if(o.autoArrows)addArrow($('>a:first-child',this));}).not('.'+c.bcClass).hidemobile_menuUl();var $a=$('a',this);$a.each(function(i){var $li=$a.eq(i).parents('li');});o.onInit.call(this);}).each(function(){var menuClasses=[c.menuClass];if(sf.op.dropShadows&&!($.browser.msie&&$.browser.version<7))menuClasses.push(c.shadowClass);$(this).addClass(menuClasses.join(' '));});};var sf=$.fn.mobile_menu;sf.o=[];sf.op={};sf.IE7fix=function(){var o=sf.op;if($.browser.msie&&$.browser.version>6&&o.dropShadows&&o.animation.opacity!=undefined)
this.toggleClass(sf.c.shadowClass+'-off');};sf.c={bcClass:'sf-breadcrumb',menuClass:'sf-js-enabled',anchorClass:'sf-with-ul',arrowClass:'sf-sub-indicator',shadowClass:'sf-shadow'};sf.defaults={hoverClass:'sfHover',pathClass:'overideThisToUse',pathLevels:2,delay:1000,animation:{height:'show'},speed:'normal',autoArrows:false,dropShadows:false,disableHI:false,onInit:function(){},onBeforeShow:function(){},onShow:function(){},onHide:function(){}};$.fn.extend({hidemobile_menuUl:function(){var o=sf.op,not=(o.retainPath===true)?o.$path:'';o.retainPath=false;var $ul=$(['li.',o.hoverClass].join(''),this).add(this).not(not).removeClass(o.hoverClass).find('>ul').hide();o.onHide.call($ul);return this;},showmobile_menuUl:function(){var o=sf.op,sh=sf.c.shadowClass+'-off',$ul=this.not('.accorChild').addClass(o.hoverClass).find('>ul:hidden');sf.IE7fix.call($ul);o.onBeforeShow.call($ul);$ul.animate(o.animation,o.speed,function(){sf.IE7fix.call($ul);o.onShow.call($ul);});return this;}});})(jQuery);

/*---------------------*/
jQuery(function ($) { if($('.sf-menu').length!=0){ $('.sf-menu').mobile_menu() } });


//pictab.js--------------------------

(function($){$.fn.phototabs=function(options){var el=$(this);el.each(function(){var m={'switchtime':2000,'animationtime':1000,'startpic':0,'autoPaly':true,'showArrow':true};if(el.find("li").length==0){return false};if(options){$.extend(m,options)};var l=1;var pic_tab=function(n){var current=parseInt(m['startpic']+n);if(current>el.find("li").length-1){current=0}else if(current<0){current=el.find("li").length-1};el.find("li").css("opacity","0").stop(true);el.addClass("preloader");l++;$('<img alt='+l+' src='+el.find("li").eq(current).css("backgroundImage").slice(4,el.find("li").eq(current).css("backgroundImage").length-1)+' />').load(function(){if(l==$(this).attr("alt")){el.removeClass("preloader");el.find("li").eq(current).addClass("selected").css("display","block").animate({"opacity":1},m["animationtime"])};$(this).remove()});if(current!=m['startpic']){el.find("li").eq(m['startpic']).css("opacity",1).animate({"opacity":0},m["animationtime"]).removeClass("selected")};m['startpic']=current};if(el.find("li").length>1){if(m["autoPaly"]){var pic_play=setInterval(function(){pic_tab(1)},m['switchtime'])};if(m["showArrow"]){el.append("<div class='pic_tab_arrow'><a  href='javascript:;' class='last_page'><</a><a  href='javascript:;' class='next_page'>></a></div>");el.find(".next_page").click(function(){clearTimeout(pic_play);pic_tab(1);if(m["autoPaly"]){pic_play=setInterval(function(){pic_tab(1)},m['switchtime'])}});el.find(".last_page").click(function(){clearTimeout(pic_play);pic_tab(-1);if(m["autoPaly"]){pic_play=setInterval(function(){pic_tab(1)},m['switchtime'])}})}}})}})(jQuery);

/*
 * jQuery mmenu v5.5.3
 * @requires jQuery 1.7.0 or later
 *
 * mmenu.frebsite.nl
 *	
 * Copyright (c) Fred Heusschen
 * www.frebsite.nl
 *
 * Licensed under the MIT license:
 * http://en.wikipedia.org/wiki/MIT_License
 */
!function(e){function n(){e[t].glbl||(l={$wndw:e(window),$html:e("html"),$body:e("body")},a={},i={},r={},e.each([a,i,r],function(e,n){n.add=function(e){e=e.split(" ");for(var t=0,s=e.length;s>t;t++)n[e[t]]=n.mm(e[t])}}),a.mm=function(e){return"mm-"+e},a.add("wrapper menu panels panel nopanel current highest opened subopened navbar hasnavbar title btn prev next listview nolistview inset vertical selected divider spacer hidden fullsubopen"),a.umm=function(e){return"mm-"==e.slice(0,3)&&(e=e.slice(3)),e},i.mm=function(e){return"mm-"+e},i.add("parent sub"),r.mm=function(e){return e+".mm"},r.add("transitionend webkitTransitionEnd mousedown mouseup touchstart touchmove touchend click keydown"),e[t]._c=a,e[t]._d=i,e[t]._e=r,e[t].glbl=l)}var t="mmenu",s="5.5.3";if(!(e[t]&&e[t].version>s)){e[t]=function(e,n,t){this.$menu=e,this._api=["bind","init","update","setSelected","getInstance","openPanel","closePanel","closeAllPanels"],this.opts=n,this.conf=t,this.vars={},this.cbck={},"function"==typeof this.___deprecated&&this.___deprecated(),this._initMenu(),this._initAnchors();var s=this.$pnls.children();return this._initAddons(),this.init(s),"function"==typeof this.___debug&&this.___debug(),this},e[t].version=s,e[t].addons={},e[t].uniqueId=0,e[t].defaults={extensions:[],navbar:{add:!0,title:"Menu",titleLink:"panel"},onClick:{setSelected:!0},slidingSubmenus:!0},e[t].configuration={classNames:{divider:"Divider",inset:"Inset",panel:"Panel",selected:"Selected",spacer:"Spacer",vertical:"Vertical"},clone:!1,openingInterval:25,panelNodetype:"ul, ol, div",transitionDuration:400},e[t].prototype={init:function(e){e=e.not("."+a.nopanel),e=this._initPanels(e),this.trigger("init",e),this.trigger("update")},update:function(){this.trigger("update")},setSelected:function(e){this.$menu.find("."+a.listview).children().removeClass(a.selected),e.addClass(a.selected),this.trigger("setSelected",e)},openPanel:function(n){var s=n.parent();if(s.hasClass(a.vertical)){var i=s.parents("."+a.subopened);if(i.length)return this.openPanel(i.first());s.addClass(a.opened)}else{if(n.hasClass(a.current))return;var r=this.$pnls.children("."+a.panel),l=r.filter("."+a.current);r.removeClass(a.highest).removeClass(a.current).not(n).not(l).not("."+a.vertical).addClass(a.hidden),e[t].support.csstransitions||l.addClass(a.hidden),n.hasClass(a.opened)?n.nextAll("."+a.opened).addClass(a.highest).removeClass(a.opened).removeClass(a.subopened):(n.addClass(a.highest),l.addClass(a.subopened)),n.removeClass(a.hidden).addClass(a.current),setTimeout(function(){n.removeClass(a.subopened).addClass(a.opened)},this.conf.openingInterval)}this.trigger("openPanel",n)},closePanel:function(e){var n=e.parent();n.hasClass(a.vertical)&&(n.removeClass(a.opened),this.trigger("closePanel",e))},closeAllPanels:function(){this.$menu.find("."+a.listview).children().removeClass(a.selected).filter("."+a.vertical).removeClass(a.opened);var e=this.$pnls.children("."+a.panel),n=e.first();this.$pnls.children("."+a.panel).not(n).removeClass(a.subopened).removeClass(a.opened).removeClass(a.current).removeClass(a.highest).addClass(a.hidden),this.openPanel(n)},togglePanel:function(e){var n=e.parent();n.hasClass(a.vertical)&&this[n.hasClass(a.opened)?"closePanel":"openPanel"](e)},getInstance:function(){return this},bind:function(e,n){this.cbck[e]=this.cbck[e]||[],this.cbck[e].push(n)},trigger:function(){var e=this,n=Array.prototype.slice.call(arguments),t=n.shift();if(this.cbck[t])for(var s=0,a=this.cbck[t].length;a>s;s++)this.cbck[t][s].apply(e,n)},_initMenu:function(){this.opts.offCanvas&&this.conf.clone&&(this.$menu=this.$menu.clone(!0),this.$menu.add(this.$menu.find("[id]")).filter("[id]").each(function(){e(this).attr("id",a.mm(e(this).attr("id")))})),this.$menu.contents().each(function(){3==e(this)[0].nodeType&&e(this).remove()}),this.$pnls=e('<div class="'+a.panels+'" />').append(this.$menu.children(this.conf.panelNodetype)).prependTo(this.$menu),this.$menu.parent().addClass(a.wrapper);var n=[a.menu];this.opts.slidingSubmenus||n.push(a.vertical),this.opts.extensions=this.opts.extensions.length?"mm-"+this.opts.extensions.join(" mm-"):"",this.opts.extensions&&n.push(this.opts.extensions),this.$menu.addClass(n.join(" "))},_initPanels:function(n){var t=this,s=this.__findAddBack(n,"ul, ol");this.__refactorClass(s,this.conf.classNames.inset,"inset").addClass(a.nolistview+" "+a.nopanel),s.not("."+a.nolistview).addClass(a.listview);var r=this.__findAddBack(n,"."+a.listview).children();this.__refactorClass(r,this.conf.classNames.selected,"selected"),this.__refactorClass(r,this.conf.classNames.divider,"divider"),this.__refactorClass(r,this.conf.classNames.spacer,"spacer"),this.__refactorClass(this.__findAddBack(n,"."+this.conf.classNames.panel),this.conf.classNames.panel,"panel");var l=e(),d=n.add(n.find("."+a.panel)).add(this.__findAddBack(n,"."+a.listview).children().children(this.conf.panelNodetype)).not("."+a.nopanel);this.__refactorClass(d,this.conf.classNames.vertical,"vertical"),this.opts.slidingSubmenus||d.addClass(a.vertical),d.each(function(){var n=e(this),s=n;n.is("ul, ol")?(n.wrap('<div class="'+a.panel+'" />'),s=n.parent()):s.addClass(a.panel);var i=n.attr("id");n.removeAttr("id"),s.attr("id",i||t.__getUniqueId()),n.hasClass(a.vertical)&&(n.removeClass(t.conf.classNames.vertical),s.add(s.parent()).addClass(a.vertical)),l=l.add(s)});var o=e("."+a.panel,this.$menu);l.each(function(){var n=e(this),s=n.parent(),r=s.children("a, span").first();if(s.is("."+a.panels)||(s.data(i.sub,n),n.data(i.parent,s)),!s.children("."+a.next).length&&s.parent().is("."+a.listview)){var l=n.attr("id"),d=e('<a class="'+a.next+'" href="#'+l+'" data-target="#'+l+'" />').insertBefore(r);r.is("span")&&d.addClass(a.fullsubopen)}if(!n.children("."+a.navbar).length&&!s.hasClass(a.vertical)){if(s.parent().is("."+a.listview))var s=s.closest("."+a.panel);else var r=s.closest("."+a.panel).find('a[href="#'+n.attr("id")+'"]').first(),s=r.closest("."+a.panel);var o=e('<div class="'+a.navbar+'" />');if(s.length){var l=s.attr("id");switch(t.opts.navbar.titleLink){case"anchor":_url=r.attr("href");break;case"panel":case"parent":_url="#"+l;break;case"none":default:_url=!1}o.append('<a class="'+a.btn+" "+a.prev+'" href="#'+l+'" data-target="#'+l+'" />').append(e('<a class="'+a.title+'"'+(_url?' href="'+_url+'"':"")+" />").text(r.text())).prependTo(n),t.opts.navbar.add&&n.addClass(a.hasnavbar)}else t.opts.navbar.title&&(o.append('<a class="'+a.title+'">'+t.opts.navbar.title+"</a>").prependTo(n),t.opts.navbar.add&&n.addClass(a.hasnavbar))}});var c=this.__findAddBack(n,"."+a.listview).children("."+a.selected).removeClass(a.selected).last().addClass(a.selected);c.add(c.parentsUntil("."+a.menu,"li")).filter("."+a.vertical).addClass(a.opened).end().not("."+a.vertical).each(function(){e(this).parentsUntil("."+a.menu,"."+a.panel).not("."+a.vertical).first().addClass(a.opened).parentsUntil("."+a.menu,"."+a.panel).not("."+a.vertical).first().addClass(a.opened).addClass(a.subopened)}),c.children("."+a.panel).not("."+a.vertical).addClass(a.opened).parentsUntil("."+a.menu,"."+a.panel).not("."+a.vertical).first().addClass(a.opened).addClass(a.subopened);var h=o.filter("."+a.opened);return h.length||(h=l.first()),h.addClass(a.opened).last().addClass(a.current),l.not("."+a.vertical).not(h.last()).addClass(a.hidden).end().appendTo(this.$pnls),l},_initAnchors:function(){var n=this;l.$body.on(r.click+"-oncanvas","a[href]",function(s){var i=e(this),r=!1,l=n.$menu.find(i).length;for(var d in e[t].addons)if(r=e[t].addons[d].clickAnchor.call(n,i,l))break;if(!r&&l){var o=i.attr("href");if(o.length>1&&"#"==o.slice(0,1))try{var c=e(o,n.$menu);c.is("."+a.panel)&&(r=!0,n[i.parent().hasClass(a.vertical)?"togglePanel":"openPanel"](c))}catch(h){}}if(r&&s.preventDefault(),!r&&l&&i.is("."+a.listview+" > li > a")&&!i.is('[rel="external"]')&&!i.is('[target="_blank"]')){n.__valueOrFn(n.opts.onClick.setSelected,i)&&n.setSelected(e(s.target).parent());var p=n.__valueOrFn(n.opts.onClick.preventDefault,i,"#"==o.slice(0,1));p&&s.preventDefault(),n.__valueOrFn(n.opts.onClick.close,i,p)&&n.close()}})},_initAddons:function(){for(var n in e[t].addons)e[t].addons[n].add.call(this),e[t].addons[n].add=function(){};for(var n in e[t].addons)e[t].addons[n].setup.call(this)},__api:function(){var n=this,t={};return e.each(this._api,function(){var e=this;t[e]=function(){var s=n[e].apply(n,arguments);return"undefined"==typeof s?t:s}}),t},__valueOrFn:function(e,n,t){return"function"==typeof e?e.call(n[0]):"undefined"==typeof e&&"undefined"!=typeof t?t:e},__refactorClass:function(e,n,t){return e.filter("."+n).removeClass(n).addClass(a[t])},__findAddBack:function(e,n){return e.find(n).add(e.filter(n))},__filterListItems:function(e){return e.not("."+a.divider).not("."+a.hidden)},__transitionend:function(e,n,t){var s=!1,a=function(){s||n.call(e[0]),s=!0};e.one(r.transitionend,a),e.one(r.webkitTransitionEnd,a),setTimeout(a,1.1*t)},__getUniqueId:function(){return a.mm(e[t].uniqueId++)}},e.fn[t]=function(s,a){return n(),s=e.extend(!0,{},e[t].defaults,s),a=e.extend(!0,{},e[t].configuration,a),this.each(function(){var n=e(this);if(!n.data(t)){var i=new e[t](n,s,a);n.data(t,i.__api())}})},e[t].support={touch:"ontouchstart"in window||navigator.msMaxTouchPoints,csstransitions:function(){if("undefined"!=typeof Modernizr&&"undefined"!=typeof Modernizr.csstransitions)return Modernizr.csstransitions;var e=document.body||document.documentElement,n=e.style,t="transition";if("string"==typeof n[t])return!0;var s=["Moz","webkit","Webkit","Khtml","O","ms"];t=t.charAt(0).toUpperCase()+t.substr(1);for(var a=0;a<s.length;a++)if("string"==typeof n[s[a]+t])return!0;return!1}()};var a,i,r,l}}(jQuery);
/*	
 * jQuery mmenu offCanvas addon
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
!function(e){var t="mmenu",o="offCanvas";e[t].addons[o]={setup:function(){if(this.opts[o]){var s=this.opts[o],i=this.conf[o];a=e[t].glbl,this._api=e.merge(this._api,["open","close","setPage"]),("top"==s.position||"bottom"==s.position)&&(s.zposition="front"),"string"!=typeof i.pageSelector&&(i.pageSelector="> "+i.pageNodetype),a.$allMenus=(a.$allMenus||e()).add(this.$menu),this.vars.opened=!1;var r=[n.offcanvas];"left"!=s.position&&r.push(n.mm(s.position)),"back"!=s.zposition&&r.push(n.mm(s.zposition)),this.$menu.addClass(r.join(" ")).parent().removeClass(n.wrapper),this.setPage(a.$page),this._initBlocker(),this["_initWindow_"+o](),this.$menu[i.menuInjectMethod+"To"](i.menuWrapperSelector)}},add:function(){n=e[t]._c,s=e[t]._d,i=e[t]._e,n.add("offcanvas slideout blocking modal background opening blocker page"),s.add("style"),i.add("resize")},clickAnchor:function(e){if(!this.opts[o])return!1;var t=this.$menu.attr("id");if(t&&t.length&&(this.conf.clone&&(t=n.umm(t)),e.is('[href="#'+t+'"]')))return this.open(),!0;if(a.$page){var t=a.$page.first().attr("id");return t&&t.length&&e.is('[href="#'+t+'"]')?(this.close(),!0):!1}}},e[t].defaults[o]={position:"left",zposition:"back",blockUI:!0,moveBackground:!0},e[t].configuration[o]={pageNodetype:"div",pageSelector:null,noPageSelector:[],wrapPageIfNeeded:!0,menuWrapperSelector:"body",menuInjectMethod:"prepend"},e[t].prototype.open=function(){if(!this.vars.opened){var e=this;this._openSetup(),setTimeout(function(){e._openFinish()},this.conf.openingInterval),this.trigger("open")}},e[t].prototype._openSetup=function(){var t=this,r=this.opts[o];this.closeAllOthers(),a.$page.each(function(){e(this).data(s.style,e(this).attr("style")||"")}),a.$wndw.trigger(i.resize+"-"+o,[!0]);var p=[n.opened];r.blockUI&&p.push(n.blocking),"modal"==r.blockUI&&p.push(n.modal),r.moveBackground&&p.push(n.background),"left"!=r.position&&p.push(n.mm(this.opts[o].position)),"back"!=r.zposition&&p.push(n.mm(this.opts[o].zposition)),this.opts.extensions&&p.push(this.opts.extensions),a.$html.addClass(p.join(" ")),setTimeout(function(){t.vars.opened=!0},this.conf.openingInterval),this.$menu.addClass(n.current+" "+n.opened)},e[t].prototype._openFinish=function(){var e=this;this.__transitionend(a.$page.first(),function(){e.trigger("opened")},this.conf.transitionDuration),a.$html.addClass(n.opening),this.trigger("opening")},e[t].prototype.close=function(){if(this.vars.opened){var t=this;this.__transitionend(a.$page.first(),function(){t.$menu.removeClass(n.current).removeClass(n.opened),a.$html.removeClass(n.opened).removeClass(n.blocking).removeClass(n.modal).removeClass(n.background).removeClass(n.mm(t.opts[o].position)).removeClass(n.mm(t.opts[o].zposition)),t.opts.extensions&&a.$html.removeClass(t.opts.extensions),a.$page.each(function(){e(this).attr("style",e(this).data(s.style))}),t.vars.opened=!1,t.trigger("closed")},this.conf.transitionDuration),a.$html.removeClass(n.opening),this.trigger("close"),this.trigger("closing")}},e[t].prototype.closeAllOthers=function(){a.$allMenus.not(this.$menu).each(function(){var o=e(this).data(t);o&&o.close&&o.close()})},e[t].prototype.setPage=function(t){var s=this,i=this.conf[o];t&&t.length||(t=a.$body.find("form"),i.noPageSelector.length&&(t=t.not(i.noPageSelector.join(", "))),t.length>1&&i.wrapPageIfNeeded&&(t=t.wrapAll("<"+this.conf[o].pageNodetype+" />").parent())),t.each(function(){e(this).attr("id",e(this).attr("id")||s.__getUniqueId())}),t.addClass(n.page+" "+n.slideout),a.$page=t,this.trigger("setPage",t)},e[t].prototype["_initWindow_"+o]=function(){a.$wndw.off(i.keydown+"-"+o).on(i.keydown+"-"+o,function(e){return a.$html.hasClass(n.opened)&&9==e.keyCode?(e.preventDefault(),!1):void 0});var e=0;a.$wndw.off(i.resize+"-"+o).on(i.resize+"-"+o,function(t,o){if(1==a.$page.length&&(o||a.$html.hasClass(n.opened))){var s=a.$wndw.height();(o||s!=e)&&(e=s,a.$page.css("minHeight",s))}})},e[t].prototype._initBlocker=function(){var t=this;this.opts[o].blockUI&&(a.$blck||(a.$blck=e('<div id="'+n.blocker+'" class="'+n.slideout+'" />')),a.$blck.appendTo(a.$body).off(i.touchstart+"-"+o+" "+i.touchmove+"-"+o).on(i.touchstart+"-"+o+" "+i.touchmove+"-"+o,function(e){e.preventDefault(),e.stopPropagation(),a.$blck.trigger(i.mousedown+"-"+o)}).off(i.mousedown+"-"+o).on(i.mousedown+"-"+o,function(e){e.preventDefault(),a.$html.hasClass(n.modal)||(t.closeAllOthers(),t.close())}))};var n,s,i,a}(jQuery);
/*	
 * jQuery mmenu autoHeight addon
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
!function(t){var e="mmenu",s="autoHeight";t[e].addons[s]={setup:function(){if(this.opts.offCanvas){switch(this.opts.offCanvas.position){case"left":case"right":return}var n=this,o=this.opts[s];if(this.conf[s],h=t[e].glbl,"boolean"==typeof o&&o&&(o={height:"auto"}),"object"!=typeof o&&(o={}),o=this.opts[s]=t.extend(!0,{},t[e].defaults[s],o),"auto"==o.height){this.$menu.addClass(i.autoheight);var u=function(t){var e=parseInt(this.$pnls.css("top"),10)||0;_bot=parseInt(this.$pnls.css("bottom"),10)||0,this.$menu.addClass(i.measureheight),t=t||this.$pnls.children("."+i.current),t.is("."+i.vertical)&&(t=t.parents("."+i.panel).not("."+i.vertical).first()),this.$menu.height(t.outerHeight()+e+_bot).removeClass(i.measureheight)};this.bind("update",u),this.bind("openPanel",u),this.bind("closePanel",u),this.bind("open",u),h.$wndw.off(a.resize+"-autoheight").on(a.resize+"-autoheight",function(){u.call(n)})}}},add:function(){i=t[e]._c,n=t[e]._d,a=t[e]._e,i.add("autoheight measureheight"),a.add("resize")},clickAnchor:function(){}},t[e].defaults[s]={height:"default"};var i,n,a,h}(jQuery);
/*	
 * jQuery mmenu backButton addon
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
!function(o){var t="mmenu",n="backButton";o[t].addons[n]={setup:function(){if(this.opts.offCanvas){var i=this,e=this.opts[n];if(this.conf[n],a=o[t].glbl,"boolean"==typeof e&&(e={close:e}),"object"!=typeof e&&(e={}),e=o.extend(!0,{},o[t].defaults[n],e),e.close){var c="#"+i.$menu.attr("id");this.bind("opened",function(){location.hash!=c&&history.pushState(null,document.title,c)}),o(window).on("popstate",function(o){a.$html.hasClass(s.opened)?(o.stopPropagation(),i.close()):location.hash==c&&(o.stopPropagation(),i.open())})}}},add:function(){return window.history&&window.history.pushState?(s=o[t]._c,i=o[t]._d,void(e=o[t]._e)):void(o[t].addons[n].setup=function(){})},clickAnchor:function(){}},o[t].defaults[n]={close:!1};var s,i,e,a}(jQuery);
/*	
 * jQuery mmenu counters addon
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
!function(t){var n="mmenu",e="counters";t[n].addons[e]={setup:function(){var s=this,o=this.opts[e];this.conf[e],c=t[n].glbl,"boolean"==typeof o&&(o={add:o,update:o}),"object"!=typeof o&&(o={}),o=this.opts[e]=t.extend(!0,{},t[n].defaults[e],o),this.bind("init",function(n){this.__refactorClass(t("em",n),this.conf.classNames[e].counter,"counter")}),o.add&&this.bind("init",function(n){n.each(function(){var n=t(this).data(a.parent);n&&(n.children("em."+i.counter).length||n.prepend(t('<em class="'+i.counter+'" />')))})}),o.update&&this.bind("update",function(){this.$pnls.find("."+i.panel).each(function(){var n=t(this),e=n.data(a.parent);if(e){var c=e.children("em."+i.counter);c.length&&(n=n.children("."+i.listview),n.length&&c.html(s.__filterListItems(n.children()).length))}})})},add:function(){i=t[n]._c,a=t[n]._d,s=t[n]._e,i.add("counter search noresultsmsg")},clickAnchor:function(){}},t[n].defaults[e]={add:!1,update:!1},t[n].configuration.classNames[e]={counter:"Counter"};var i,a,s,c}(jQuery);
/*	
 * jQuery mmenu dividers addon
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
!function(i){var e="mmenu",s="dividers";i[e].addons[s]={setup:function(){var n=this,a=this.opts[s];if(this.conf[s],l=i[e].glbl,"boolean"==typeof a&&(a={add:a,fixed:a}),"object"!=typeof a&&(a={}),a=this.opts[s]=i.extend(!0,{},i[e].defaults[s],a),this.bind("init",function(){this.__refactorClass(i("li",this.$menu),this.conf.classNames[s].collapsed,"collapsed")}),a.add&&this.bind("init",function(e){switch(a.addTo){case"panels":var s=e;break;default:var s=i(a.addTo,this.$pnls).filter("."+d.panel)}i("."+d.divider,s).remove(),s.find("."+d.listview).not("."+d.vertical).each(function(){var e="";n.__filterListItems(i(this).children()).each(function(){var s=i.trim(i(this).children("a, span").text()).slice(0,1).toLowerCase();s!=e&&s.length&&(e=s,i('<li class="'+d.divider+'">'+s+"</li>").insertBefore(this))})})}),a.collapse&&this.bind("init",function(e){i("."+d.divider,e).each(function(){var e=i(this),s=e.nextUntil("."+d.divider,"."+d.collapsed);s.length&&(e.children("."+d.subopen).length||(e.wrapInner("<span />"),e.prepend('<a href="#" class="'+d.subopen+" "+d.fullsubopen+'" />')))})}),a.fixed){var o=function(e){e=e||this.$pnls.children("."+d.current);var s=e.find("."+d.divider).not("."+d.hidden);if(s.length){this.$menu.addClass(d.hasdividers);var n=e.scrollTop()||0,t="";e.is(":visible")&&e.find("."+d.divider).not("."+d.hidden).each(function(){i(this).position().top+n<n+1&&(t=i(this).text())}),this.$fixeddivider.text(t)}else this.$menu.removeClass(d.hasdividers)};this.$fixeddivider=i('<ul class="'+d.listview+" "+d.fixeddivider+'"><li class="'+d.divider+'"></li></ul>').prependTo(this.$pnls).children(),this.bind("openPanel",o),this.bind("init",function(e){e.off(t.scroll+"-dividers "+t.touchmove+"-dividers").on(t.scroll+"-dividers "+t.touchmove+"-dividers",function(){o.call(n,i(this))})})}},add:function(){d=i[e]._c,n=i[e]._d,t=i[e]._e,d.add("collapsed uncollapsed fixeddivider hasdividers"),t.add("scroll")},clickAnchor:function(i,e){if(this.opts[s].collapse&&e){var n=i.parent();if(n.is("."+d.divider)){var t=n.nextUntil("."+d.divider,"."+d.collapsed);return n.toggleClass(d.opened),t[n.hasClass(d.opened)?"addClass":"removeClass"](d.uncollapsed),!0}}return!1}},i[e].defaults[s]={add:!1,addTo:"panels",fixed:!1,collapse:!1},i[e].configuration.classNames[s]={collapsed:"Collapsed"};var d,n,t,l}(jQuery);
/*	
 * jQuery mmenu dragOpen addon
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
!function(e){function t(e,t,n){return t>e&&(e=t),e>n&&(e=n),e}var n="mmenu",o="dragOpen";e[n].addons[o]={setup:function(){if(this.opts.offCanvas){var i=this,a=this.opts[o],p=this.conf[o];if(r=e[n].glbl,"boolean"==typeof a&&(a={open:a}),"object"!=typeof a&&(a={}),a=this.opts[o]=e.extend(!0,{},e[n].defaults[o],a),a.open){var d,f,c,u,h,l={},m=0,g=!1,v=!1,w=0,_=0;switch(this.opts.offCanvas.position){case"left":case"right":l.events="panleft panright",l.typeLower="x",l.typeUpper="X",v="width";break;case"top":case"bottom":l.events="panup pandown",l.typeLower="y",l.typeUpper="Y",v="height"}switch(this.opts.offCanvas.position){case"right":case"bottom":l.negative=!0,u=function(e){e>=r.$wndw[v]()-a.maxStartPos&&(m=1)};break;default:l.negative=!1,u=function(e){e<=a.maxStartPos&&(m=1)}}switch(this.opts.offCanvas.position){case"left":l.open_dir="right",l.close_dir="left";break;case"right":l.open_dir="left",l.close_dir="right";break;case"top":l.open_dir="down",l.close_dir="up";break;case"bottom":l.open_dir="up",l.close_dir="down"}switch(this.opts.offCanvas.zposition){case"front":h=function(){return this.$menu};break;default:h=function(){return e("."+s.slideout)}}var b=this.__valueOrFn(a.pageNode,this.$menu,r.$page);"string"==typeof b&&(b=e(b));var y=new Hammer(b[0],a.vendors.hammer);y.on("panstart",function(e){u(e.center[l.typeLower]),r.$slideOutNodes=h(),g=l.open_dir}).on(l.events+" panend",function(e){m>0&&e.preventDefault()}).on(l.events,function(e){if(d=e["delta"+l.typeUpper],l.negative&&(d=-d),d!=w&&(g=d>=w?l.open_dir:l.close_dir),w=d,w>a.threshold&&1==m){if(r.$html.hasClass(s.opened))return;m=2,i._openSetup(),i.trigger("opening"),r.$html.addClass(s.dragging),_=t(r.$wndw[v]()*p[v].perc,p[v].min,p[v].max)}2==m&&(f=t(w,10,_)-("front"==i.opts.offCanvas.zposition?_:0),l.negative&&(f=-f),c="translate"+l.typeUpper+"("+f+"px )",r.$slideOutNodes.css({"-webkit-transform":"-webkit-"+c,transform:c}))}).on("panend",function(){2==m&&(r.$html.removeClass(s.dragging),r.$slideOutNodes.css("transform",""),i[g==l.open_dir?"_openFinish":"close"]()),m=0})}}},add:function(){return"function"!=typeof Hammer||Hammer.VERSION<2?void(e[n].addons[o].setup=function(){}):(s=e[n]._c,i=e[n]._d,a=e[n]._e,void s.add("dragging"))},clickAnchor:function(){}},e[n].defaults[o]={open:!1,maxStartPos:100,threshold:50,vendors:{hammer:{}}},e[n].configuration[o]={width:{perc:.8,min:140,max:440},height:{perc:.8,min:140,max:880}};var s,i,a,r}(jQuery);
/*	
 * jQuery mmenu fixedElements addon
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
!function(s){var i="mmenu",t="fixedElements";s[i].addons[t]={setup:function(){if(this.opts.offCanvas){var n=this.opts[t];this.conf[t],d=s[i].glbl,n=this.opts[t]=s.extend(!0,{},s[i].defaults[t],n);var a=function(s){var i=this.conf.classNames[t].fixed;this.__refactorClass(s.find("."+i),i,"slideout").appendTo(d.$body)};a.call(this,d.$page),this.bind("setPage",a)}},add:function(){n=s[i]._c,a=s[i]._d,e=s[i]._e,n.add("fixed")},clickAnchor:function(){}},s[i].configuration.classNames[t]={fixed:"Fixed"};var n,a,e,d}(jQuery);
/*	
 * jQuery mmenu iconPanels addon
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
!function(e){var n="mmenu",i="iconPanels";e[n].addons[i]={setup:function(){var a=this,l=this.opts[i];if(this.conf[i],d=e[n].glbl,"boolean"==typeof l&&(l={add:l}),"number"==typeof l&&(l={add:!0,visible:l}),"object"!=typeof l&&(l={}),l=this.opts[i]=e.extend(!0,{},e[n].defaults[i],l),l.visible++,l.add){this.$menu.addClass(s.iconpanel);for(var t=[],o=0;o<=l.visible;o++)t.push(s.iconpanel+"-"+o);t=t.join(" ");var c=function(n){var i=a.$pnls.children("."+s.panel).removeClass(t),d=i.filter("."+s.subopened);d.removeClass(s.hidden).add(n).slice(-l.visible).each(function(n){e(this).addClass(s.iconpanel+"-"+n)})};this.bind("openPanel",c),this.bind("init",function(n){c.call(a,a.$pnls.children("."+s.current)),l.hideNavbars&&n.removeClass(s.hasnavbar),n.each(function(){e(this).children("."+s.subblocker).length||e(this).prepend('<a href="#'+e(this).closest("."+s.panel).attr("id")+'" class="'+s.subblocker+'" />')})})}},add:function(){s=e[n]._c,a=e[n]._d,l=e[n]._e,s.add("iconpanel subblocker")},clickAnchor:function(){}},e[n].defaults[i]={add:!1,visible:3,hideNavbars:!1};var s,a,l,d}(jQuery);
/*	
 * jQuery mmenu navbar addon
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
!function(n){var a="mmenu",t="navbars";n[a].addons[t]={setup:function(){var r=this,s=this.opts[t],c=this.conf[t];if(i=n[a].glbl,"undefined"!=typeof s){s instanceof Array||(s=[s]);var d={};n.each(s,function(i){var o=s[i];"boolean"==typeof o&&o&&(o={}),"object"!=typeof o&&(o={}),"undefined"==typeof o.content&&(o.content=["prev","title"]),o.content instanceof Array||(o.content=[o.content]),o=n.extend(!0,{},r.opts.navbar,o);var l=o.position,h=o.height;"number"!=typeof h&&(h=1),h=Math.min(4,Math.max(1,h)),"bottom"!=l&&(l="top"),d[l]||(d[l]=0),d[l]++;var f=n("<div />").addClass(e.navbar+" "+e.navbar+"-"+l+" "+e.navbar+"-"+l+"-"+d[l]+" "+e.navbar+"-size-"+h);d[l]+=h-1;for(var v=0,p=o.content.length;p>v;v++){var u=n[a].addons[t][o.content[v]]||!1;u?u.call(r,f,o,c):(u=o.content[v],u instanceof n||(u=n(o.content[v])),u.each(function(){f.append(n(this))}))}var b=Math.ceil(f.children().not("."+e.btn).length/h);b>1&&f.addClass(e.navbar+"-content-"+b),f.children("."+e.btn).length&&f.addClass(e.hasbtns),f.prependTo(r.$menu)});for(var o in d)r.$menu.addClass(e.hasnavbar+"-"+o+"-"+d[o])}},add:function(){e=n[a]._c,r=n[a]._d,s=n[a]._e,e.add("close hasbtns")},clickAnchor:function(){}},n[a].configuration[t]={breadcrumbSeparator:"/"},n[a].configuration.classNames[t]={panelTitle:"Title",panelNext:"Next",panelPrev:"Prev"};var e,r,s,i}(jQuery),/*	
 * jQuery mmenu navbar addon breadcrumbs content
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
function(n){var a="mmenu",t="navbars",e="breadcrumbs";n[a].addons[t][e]=function(t,e,r){var s=n[a]._c,i=n[a]._d;s.add("breadcrumbs separator"),t.append('<span class="'+s.breadcrumbs+'"></span>'),this.bind("init",function(a){a.removeClass(s.hasnavbar).each(function(){for(var a=[],t=n(this),e=n('<span class="'+s.breadcrumbs+'"></span>'),c=n(this).children().first(),d=!0;c&&c.length;){c.is("."+s.panel)||(c=c.closest("."+s.panel));var o=c.children("."+s.navbar).children("."+s.title).text();a.unshift(d?"<span>"+o+"</span>":'<a href="#'+c.attr("id")+'">'+o+"</a>"),d=!1,c=c.data(i.parent)}e.append(a.join('<span class="'+s.separator+'">'+r.breadcrumbSeparator+"</span>")).appendTo(t.children("."+s.navbar))})});var c=function(){var n=this.$pnls.children("."+s.current),a=t.find("."+s.breadcrumbs),e=n.children("."+s.navbar).children("."+s.breadcrumbs);a.html(e.html())};this.bind("openPanel",c),this.bind("init",c)}}(jQuery),/*	
 * jQuery mmenu navbar addon close content
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
function(n){var a="mmenu",t="navbars",e="close";n[a].addons[t][e]=function(t){var e=n[a]._c,r=n[a].glbl;t.append('<a class="'+e.close+" "+e.btn+'" href="#"></a>');var s=function(n){t.find("."+e.close).attr("href","#"+n.attr("id"))};s.call(this,r.$page),this.bind("setPage",s)}}(jQuery),/*	
 * jQuery mmenu navbar addon next content
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
function(n){var a="mmenu",t="navbars",e="next";n[a].addons[t][e]=function(e){var r=n[a]._c;e.append('<a class="'+r.next+" "+r.btn+'" href="#"></a>');var s=function(n){n=n||this.$pnls.children("."+r.current);var a=e.find("."+r.next),s=n.find("."+this.conf.classNames[t].panelNext),i=s.attr("href"),c=s.html();a[i?"attr":"removeAttr"]("href",i),a[i||c?"removeClass":"addClass"](r.hidden),a.html(c)};this.bind("openPanel",s),this.bind("init",function(){s.call(this)})}}(jQuery),/*	
 * jQuery mmenu navbar addon prev content
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
function(n){var a="mmenu",t="navbars",e="prev";n[a].addons[t][e]=function(e){var r=n[a]._c;e.append('<a class="'+r.prev+" "+r.btn+'" href="#"></a>'),this.bind("init",function(n){n.removeClass(r.hasnavbar)});var s=function(){var n=this.$pnls.children("."+r.current),a=e.find("."+r.prev),s=n.find("."+this.conf.classNames[t].panelPrev);s.length||(s=n.children("."+r.navbar).children("."+r.prev));var i=s.attr("href"),c=s.html();a[i?"attr":"removeAttr"]("href",i),a[i||c?"removeClass":"addClass"](r.hidden),a.html(c)};this.bind("openPanel",s),this.bind("init",s)}}(jQuery),/*	
 * jQuery mmenu navbar addon searchfield content
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
function(n){var a="mmenu",t="navbars",e="searchfield";n[a].addons[t][e]=function(t){var e=n[a]._c,r=n('<div class="'+e.search+'" />').appendTo(t);"object"!=typeof this.opts.searchfield&&(this.opts.searchfield={}),this.opts.searchfield.add=!0,this.opts.searchfield.addTo=r}}(jQuery),/*	
 * jQuery mmenu navbar addon title content
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
function(n){var a="mmenu",t="navbars",e="title";n[a].addons[t][e]=function(e,r){var s=n[a]._c;e.append('<a class="'+s.title+'"></a>');var i=function(n){n=n||this.$pnls.children("."+s.current);var a=e.find("."+s.title),i=n.find("."+this.conf.classNames[t].panelTitle);i.length||(i=n.children("."+s.navbar).children("."+s.title));var c=i.attr("href"),d=i.html()||r.title;a[c?"attr":"removeAttr"]("href",c),a[c||d?"removeClass":"addClass"](s.hidden),a.html(d)};this.bind("openPanel",i),this.bind("init",function(){i.call(this)})}}(jQuery);
/*	
 * jQuery mmenu searchfield addon
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
!function(e){function s(e){switch(e){case 9:case 16:case 17:case 18:case 37:case 38:case 39:case 40:return!0}return!1}var n="mmenu",a="searchfield";e[n].addons[a]={setup:function(){var o=this,d=this.opts[a],c=this.conf[a];r=e[n].glbl,"boolean"==typeof d&&(d={add:d}),"object"!=typeof d&&(d={}),d=this.opts[a]=e.extend(!0,{},e[n].defaults[a],d),this.bind("close",function(){this.$menu.find("."+l.search).find("input").blur()}),this.bind("init",function(n){if(d.add){switch(d.addTo){case"panels":var a=n;break;default:var a=e(d.addTo,this.$menu)}a.each(function(){var s=e(this);if(!s.is("."+l.panel)||!s.is("."+l.vertical)){if(!s.children("."+l.search).length){var n=c.form?"form":"div",a=e("<"+n+' class="'+l.search+'" />');if(c.form&&"object"==typeof c.form)for(var t in c.form)a.attr(t,c.form[t]);a.append('<input placeholder="'+d.placeholder+'" type="text" autocomplete="off" />'),s.hasClass(l.search)?s.replaceWith(a):s.prepend(a).addClass(l.hassearch)}if(d.noResults){var i=s.closest("."+l.panel).length;if(i||(s=o.$pnls.children("."+l.panel).first()),!s.children("."+l.noresultsmsg).length){var r=s.children("."+l.listview).first();e('<div class="'+l.noresultsmsg+'" />').append(d.noResults)[r.length?"insertAfter":"prependTo"](r.length?r:s)}}}}),d.search&&e("."+l.search,this.$menu).each(function(){var n=e(this),a=n.closest("."+l.panel).length;if(a)var r=n.closest("."+l.panel),c=r;else var r=e("."+l.panel,o.$menu),c=o.$menu;var h=n.children("input"),u=o.__findAddBack(r,"."+l.listview).children("li"),f=u.filter("."+l.divider),p=o.__filterListItems(u),v="> a",m=v+", > span",b=function(){var s=h.val().toLowerCase();r.scrollTop(0),p.add(f).addClass(l.hidden).find("."+l.fullsubopensearch).removeClass(l.fullsubopen).removeClass(l.fullsubopensearch),p.each(function(){var n=e(this),a=v;(d.showTextItems||d.showSubPanels&&n.find("."+l.next))&&(a=m),e(a,n).text().toLowerCase().indexOf(s)>-1&&n.add(n.prevAll("."+l.divider).first()).removeClass(l.hidden)}),d.showSubPanels&&r.each(function(){var s=e(this);o.__filterListItems(s.find("."+l.listview).children()).each(function(){var s=e(this),n=s.data(t.sub);s.removeClass(l.nosubresults),n&&n.find("."+l.listview).children().removeClass(l.hidden)})}),e(r.get().reverse()).each(function(s){var n=e(this),i=n.data(t.parent);i&&(o.__filterListItems(n.find("."+l.listview).children()).length?(i.hasClass(l.hidden)&&i.children("."+l.next).not("."+l.fullsubopen).addClass(l.fullsubopen).addClass(l.fullsubopensearch),i.removeClass(l.hidden).removeClass(l.nosubresults).prevAll("."+l.divider).first().removeClass(l.hidden)):a||(n.hasClass(l.opened)&&setTimeout(function(){o.openPanel(i.closest("."+l.panel))},1.5*(s+1)*o.conf.openingInterval),i.addClass(l.nosubresults)))}),c[p.not("."+l.hidden).length?"removeClass":"addClass"](l.noresults),this.update()};h.off(i.keyup+"-searchfield "+i.change+"-searchfield").on(i.keyup+"-searchfield",function(e){s(e.keyCode)||b.call(o)}).on(i.change+"-searchfield",function(){b.call(o)})})}})},add:function(){l=e[n]._c,t=e[n]._d,i=e[n]._e,l.add("search hassearch noresultsmsg noresults nosubresults fullsubopensearch"),i.add("change keyup")},clickAnchor:function(){}},e[n].defaults[a]={add:!1,addTo:"panels",search:!0,placeholder:"Search",noResults:"No results found.",showTextItems:!1,showSubPanels:!0},e[n].configuration[a]={form:!1};var l,t,i,r}(jQuery);
/*	
 * jQuery mmenu sectionIndexer addon
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
!function(e){var a="mmenu",r="sectionIndexer";e[a].addons[r]={setup:function(){var i=this,d=this.opts[r];this.conf[r],t=e[a].glbl,"boolean"==typeof d&&(d={add:d}),"object"!=typeof d&&(d={}),d=this.opts[r]=e.extend(!0,{},e[a].defaults[r],d),this.bind("init",function(a){if(d.add){switch(d.addTo){case"panels":var r=a;break;default:var r=e(d.addTo,this.$menu).filter("."+n.panel)}r.find("."+n.divider).closest("."+n.panel).addClass(n.hasindexer)}if(!this.$indexer&&this.$pnls.children("."+n.hasindexer).length){this.$indexer=e('<div class="'+n.indexer+'" />').prependTo(this.$pnls).append('<a href="#a">a</a><a href="#b">b</a><a href="#c">c</a><a href="#d">d</a><a href="#e">e</a><a href="#f">f</a><a href="#g">g</a><a href="#h">h</a><a href="#i">i</a><a href="#j">j</a><a href="#k">k</a><a href="#l">l</a><a href="#m">m</a><a href="#n">n</a><a href="#o">o</a><a href="#p">p</a><a href="#q">q</a><a href="#r">r</a><a href="#s">s</a><a href="#t">t</a><a href="#u">u</a><a href="#v">v</a><a href="#w">w</a><a href="#x">x</a><a href="#y">y</a><a href="#z">z</a>'),this.$indexer.children().on(s.mouseover+"-sectionindexer "+n.touchstart+"-sectionindexer",function(){var a=e(this).attr("href").slice(1),r=i.$pnls.children("."+n.current),s=r.find("."+n.listview),t=!1,d=r.scrollTop(),h=s.position().top+parseInt(s.css("margin-top"),10)+parseInt(s.css("padding-top"),10)+d;r.scrollTop(0),s.children("."+n.divider).not("."+n.hidden).each(function(){t===!1&&a==e(this).text().slice(0,1).toLowerCase()&&(t=e(this).position().top+h)}),r.scrollTop(t!==!1?t:d)});var t=function(e){i.$menu[(e.hasClass(n.hasindexer)?"add":"remove")+"Class"](n.hasindexer)};this.bind("openPanel",t),t.call(this,this.$pnls.children("."+n.current))}})},add:function(){n=e[a]._c,i=e[a]._d,s=e[a]._e,n.add("indexer hasindexer"),s.add("mouseover touchstart")},clickAnchor:function(e){return e.parent().is("."+n.indexer)?!0:void 0}},e[a].defaults[r]={add:!1,addTo:"panels"};var n,i,s,t}(jQuery);
/*	
 * jQuery mmenu toggles addon
 * mmenu.frebsite.nl
 *
 * Copyright (c) Fred Heusschen
 */
!function(t){var e="mmenu",c="toggles";t[e].addons[c]={setup:function(){var n=this;this.opts[c],this.conf[c],l=t[e].glbl,this.bind("init",function(e){this.__refactorClass(t("input",e),this.conf.classNames[c].toggle,"toggle"),this.__refactorClass(t("input",e),this.conf.classNames[c].check,"check"),t("input."+s.toggle+", input."+s.check,e).each(function(){var e=t(this),c=e.closest("li"),i=e.hasClass(s.toggle)?"toggle":"check",l=e.attr("id")||n.__getUniqueId();c.children('label[for="'+l+'"]').length||(e.attr("id",l),c.prepend(e),t('<label for="'+l+'" class="'+s[i]+'"></label>').insertBefore(c.children("a, span").last()))})})},add:function(){s=t[e]._c,n=t[e]._d,i=t[e]._e,s.add("toggle check")},clickAnchor:function(){}},t[e].configuration.classNames[c]={toggle:"Toggle",check:"Check"};var s,n,i,l}(jQuery);

(function(e) {
	e.fn.mobile_menu = function(op) {
		op = $.extend({
			slidingSubmenus: true,
			counters: true,
			navbartitle: "Menu",
			headerbox: ".menu_header",
			footerbox: ".menu_footer"
		},
		op || {});
		var nav = $(this);
		nav.mmenu({
			slidingSubmenus: op.slidingSubmenus,
			searchfield: false,
			counters: op.counters,
			navbar: {
				title: op.navbartitle
			},
			navbars: [{
				position: 'top',
				content: ['<div class="menu_header_box"></div>']
			},
			{
				position: 'top',
				content: ['prev', 'title', 'close']
			},
			{
				position: 'bottom',
				content: ['<div class="menu_footer_box"></div>']
			}]
		});
		var header = nav.find(".menu_header_box"),
		footer = nav.find(".menu_footer_box");
		$(op.headerbox).prependTo(header);
		$(op.footerbox).prependTo(footer);
		nav.addClass("mm-current");
		var th,
		bh,
		th2;
		var h = function() {
			th = header.outerHeight();
			bh = footer.outerHeight();
			th2 = nav.find(".mm-navbar-top-2").outerHeight();
			nav.find(".mm-navbar-top-2").css({
				top: th
			});
			nav.find(".mm-panels").css({
				top: th + th2,
				bottom: bh
			});
		}
		h();
		$(window).resize(function(e) {
			h();
		});
		nav.removeClass("mm-current");
	}
})(jQuery);




$(document).ready(function() { 
	$(".primary_structure").menusKeyboard();
})















