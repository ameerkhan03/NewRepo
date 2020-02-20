// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $('button[data-toggle="ajax-modal"]').click(function (event) {
       $('#loginModal').modal('show');
        var url = $(this).data('url');
    });

    $('#saveProduct').click(function (event) {
        var prodName = $('#inputProductName').val();
        var price = $('#inputPrice').val();
        var descriptions = $.trim($("#inputDescription").val());
        var product = new Object();
        product.productName = prodName;
        product.price = parseFloat(price);
        product.description = descriptions;
        product.isActive = true;
        product.image = "image";
        product.categoryId = 1;

        var apibaseUrl = "https://localhost:44309/api/v1/Product/ProductAdd";
             
        $.ajax({
            url: apibaseUrl,
            type: 'GET',
            dataType: 'json',
           // headers: { 'Content-Type': 'application/json;' },
            data: product,  
            success: function (data, textStatus, xhr) {
                console.log(data);
                location.reload();
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation');
                location.reload();
            }
        });  
    });

});