// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $('.navbar-toggler').on('click', function () {
        $('.animated-navbar-toggler-icon').toggleClass('open');
    });

    // ajax modals
    //
    // button that opens modal should have following data attributes
    // data-toggle="ajax-modal"
    // data-url - url to action method that returns partial view with modal
    // data-placeholder - id of element which will contain loaded modal markup code
    //
    // in partial view with modal markup code save button should have attribute
    // data-save="modal"
    $('a[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        var placeholderElement = $($(this).data('placeholder'));

        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');

            $('button[data-save="modal"]').click(function (event) {
                event.preventDefault();

                var form = $(this).parents('.modal').find('form');
                var actionUrl = form.attr('action');
                var dataToSend = form.serialize();

                $.post(actionUrl, dataToSend).done(function (data) {
                    var newBody = $('.modal-body', data);
                    placeholderElement.find('.modal-body').replaceWith(newBody);

                    var isValid = newBody.find('[name="IsValid"]').val() == 'True';
                    if (isValid) {
                        placeholderElement.find('.modal').modal('hide');
                    }
                });
            });
        });
    });
});