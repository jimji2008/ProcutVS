function scrollBody() {
	var headerTable = $('#headerTable');
	var nameTr = $('#nameTr');

	if (nameTr.offset().top - $(window).scrollTop() <= 0) {
		headerTable.show();
	}
	else {
		headerTable.hide();
	}
}
$(document).scroll(scrollBody);

function highlightSummaryItem(item) {
	//alert($('#' + item.id));
	var idParts = item.id.split('_');
	var o = $('#' + idParts[0] + '_1');
	if (idParts[1] == '1')
		o = $('#' + idParts[0] + '_2');
	item = $(item);
	o.css('border-color', '#CCC');
	item.css('border-color', '#CCC');

	$(this).mouseout(function () {
		o.css('border-color', '#FFF');
		item.css('border-color', '#FFF');
	});
}