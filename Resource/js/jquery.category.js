jQuery(document).ready(function ($) {
    var $category_menue_items = $('.category_wrapper li');

    $category_menue_items.click(function (e) {
        var $target = $(e.target),
			$clicked,
			$child_list,
			$collexpand;

        if ($target.is('div') || $target.is('a.collexpand')) {
            $clicked = $(this);
            $child_list = $clicked.find('> ul');
            $collexpand = $clicked.find('> div > a.collexpand');

            if ($child_list.length == 1) {
                if ($child_list.is(':visible')) {
                    $child_list.slideUp(200);
                    $collexpand.removeClass('collapse');
                } else {
                    $child_list.slideDown(200);
                    $collexpand.addClass('collapse');
                }
            }

            return false;
        }
    });
});