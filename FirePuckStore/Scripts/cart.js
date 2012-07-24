var draggedID;

$(document).ready(function () {
    $("#categorySection").accordion({ active: true });

    subscribeAtCardClickEvent();
    subscribeAtDragDropEvents();

    var src = document.getElementById("categorySection");
    src.ondragstart = function (e) {
        draggedID = $(e.target).attr("cardid");
        e.target.classList.add("dragged");
    };
});


function handleDrag(e) {
    e.preventDefault();
}

function subscribeAtDragDropEvents() {

    var shoppingCart = document.getElementById("shoppingCart");
    shoppingCart.ondragenter = handleDrag;
    shoppingCart.ondragover = handleDrag;
    shoppingCart.ondrop = function (e) {

        var card = { CardId: draggedID };

        $.ajax({
            url: '/Cart/Add',
            type: 'POST',
            data: card,
            error: function (data) {
                if (data.status = 403) {
                    alert("You can not add this item because cart was modified by another user");
                }
            },
            success: function (data) {
                $("aside").replaceWith(data);
                subscribeAtDragDropEvents();
                subscribeAtCardClickEvent();
                var cards = $("img[cardid~=" + draggedID + "]");
                var quantites = cards.parent().find("span:first");
                var oldQuantity = quantites.first().text();
                var newQuantity = oldQuantity - 1;
                if (newQuantity == 0) {
                    var noStockHtml = $("<br/><h6>Not in stock</h6><br /><h6 />");
                    cards.attr('draggable', "false");
                    cards.addClass('noItemsInStore');
                    cards.nextAll().remove();
                    cards.parent().append(noStockHtml);
                    return;
                }

                quantites.each(function () {
                    $(this).text(newQuantity);
                });

            }
        });
    };
}

function subscribeAtCardClickEvent() {
    $(".cartImg").bind('click', function (e) {
        var card = { CardId: $(e.target).attr("ordercardid") };
        $.ajax({
            url: '/Cart/Delete',
            type: 'POST',
            data: card,
            async: false,
            error: function (data) {
                if (data.status = 403) {
                    alert("You can not delete this item because cart was modified by another user");
                }
            },
            success: function (data) {
                $("aside").replaceWith(data);
                subscribeAtDragDropEvents();
                subscribeAtCardClickEvent();

                var cards = $("img[cardid~=" + card.CardId + "]");
                if (cards.first().hasClass('noItemsInStore')) {
                    var quantity = parseInt($(e.target).parent().find(".pricequantity").text());
                    var price = $(e.target).parent().find(".pricelabel").text();
                    var pricePerItem = (parseFloat(price) / quantity).toFixed(2);
                    var noStockHtml = $("<br/><h6>Qnty:</h6><span> 1</span><br/><h6>Price:</h6><span> " + pricePerItem + "</span>");

                    cards.attr('draggable', "true");
                    cards.removeClass('noItemsInStore');
                    cards.nextAll().remove();
                    cards.parent().append(noStockHtml);
                    return;
                }

                var quantites = cards.parent().find("span:first");
                var oldQuantity = quantites.first().text();
                var newQuantity = parseInt(oldQuantity) + 1;
                quantites.each(function () {
                    $(this).text(' ' + newQuantity);
                });
            }
        });
    });
}