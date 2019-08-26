//plugin definition

(function($) {
	$.fn.extend({
		//pass the options variable to the function
		accordionprohover: function(options) {
			var defaults = {
				accordionpro: 'true',
				speed: 300,
				closedSign: '+',
				openedSign: '-'
			};

			// Extend our default options with those provided.
			var opts = $.extend(defaults, options);
			//Assign current element to variable, in this case is UL element
			var $this = $(this);
			var interval ;

     

			var events =function(o) {
					//avoid jumping to the top of the page when the href is an #
					if(o.hasClass("menu_arrow")){
						var link = false;
						var e=o;
					   e.parent("a").on("click",function() {
						if(link == false){
								link = true;
								return false;
							}
						});	
					}else{
						var e=o.children(".menu_arrow")
						if(e.length==0){return false;}
				}
				if (e.parent().parent().find("ul").size() != 0) {
						if (opts.accordionpro) {
							//Do nothing when the list is open
							if (!e.parent().parent().find("ul").is(':visible')) {
								parents = e.parent().parent().parents("ul");
								visible = $this.find("ul:visible");
								visible.each(function(visibleIndex) {
									var close = true;
									parents.each(function(parentIndex) {
										if (parents[parentIndex] == visible[visibleIndex]) {
											close = false;
											return false;
										}
									});
									if (close) {
										if (e.parent().parent().find("ul") != visible[visibleIndex]) {
											$(visible[visibleIndex]).slideUp(opts.speed,
											function() {
												$(this).parent("li").find("span.menu_arrow:first").html(opts.closedSign).removeClass().addClass("menu_arrow arrow_opened");
												$(this).siblings("a").removeClass("current");
												$(this).parent("li").removeClass("active");
											});
										}
									}
								});
							}
						}
						if (e.parent().parent().find("ul:first").is(":visible")) {
							if(o.hasClass("menu_arrow")){
								e.parent().parent().find("ul:first").slideUp(opts.speed,
								function() {
									$(this).parent("li").find("span.menu_arrow:first").delay(opts.speed).html(opts.closedSign).removeClass().addClass("menu_arrow arrow_opened");
									$(this).siblings("a").removeClass("current");
									$(this).parent("li").removeClass("active");
								});
							}
						} else {
							e.parent().parent().find("ul:first").slideDown(opts.speed,
							function() {
								$(this).parent("li").find("span.menu_arrow:first").delay(opts.speed).html(opts.openedSign).removeClass().addClass("menu_arrow arrow_closed");
								$(this).siblings("a").addClass("current");
								$(this).parent("li").addClass("active");
							});
						}
					}

			} 
				
			$this.find("li a").on("mouseover",function() { var e=$(this); interval = setTimeout( function (){events(e)}, 200);}).on("mouseout",function() { clearTimeout(interval); });
			$this.find("li a span.menu_arrow").on("click",function() {events($(this))});
		
		}
	});
})(jQuery);