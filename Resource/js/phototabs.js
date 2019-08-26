//pictab.js--------------------------

(function($){$.fn.phototabs=function(options){var el=$(this);el.each(function(){var m={'switchtime':2000,'animationtime':1000,'startpic':0,'autoPaly':true,'showArrow':true};if(el.find("li").length==0){return false}
if(options){$.extend(m,options)};var l=1;var pic_tab=function(n){var current=parseInt(m['startpic']+n);if(current>el.find("li").length-1){current=0}else if(current<0){current=el.find("li").length-1}
el.find("li").css("opacity","0").stop(true);el.addClass("preloader");l++;$('<img alt='+l+' src='+el.find("li").eq(current).css("backgroundImage").slice(4,el.find("li").eq(current).css("backgroundImage").length-1)+' />').load(function(){if(l==$(this).attr("alt")){el.removeClass("preloader");el.find("li").eq(current).addClass("selected").css("display","block").animate({"opacity":1},m["animationtime"]);};$(this).remove();});if(current!=m['startpic']){el.find("li").eq(m['startpic']).css("opacity",1).animate({"opacity":0},m["animationtime"]).removeClass("selected");}
m['startpic']=current;}
pic_tab(0);if(el.find("li").length>1){if(m["autoPaly"]){var pic_play=setInterval(function(){pic_tab(1)},m['switchtime']);}
if(m["showArrow"]){el.append("<div class='pic_tab_arrow'><a  href='javascript:;' class='last_page'><</a><a  href='javascript:;' class='next_page'>></a></div>");el.find(".next_page").click(function(){clearTimeout(pic_play);pic_tab(1);if(m["autoPaly"]){pic_play=setInterval(function(){pic_tab(1)},m['switchtime']);}})
el.find(".last_page").click(function(){clearTimeout(pic_play);pic_tab(-1);if(m["autoPaly"]){pic_play=setInterval(function(){pic_tab(1)},m['switchtime']);}})}}});}})(jQuery);
