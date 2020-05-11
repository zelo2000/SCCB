$(document).ready(function () {

    $('#Members').on('click', '.remove-btn', function () {
        const url = $(this).data('url');

        $.ajax({
            url: url,
            type: 'DELETE',
            success: function () {
                location.reload();
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
            }
        });
    });

    $("#Remove").on('click', function () {
        const url = $('button[data-target="#confirmModal"]').data('url');

        $.ajax({
            url: url,
            type: 'DELETE',
            success: function () {
                location.href = '/Groups';
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
            }
        });
    });
});