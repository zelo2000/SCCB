﻿$(document).ready(function () {
    $('.cancel-button ').on('click', function () {
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

});