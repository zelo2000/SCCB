$(document).ready(function () {
    var placeholderElement = $('#EditUserPlaceholder');

    $('.edit-button').click(function (event) {
        const url = $(this).data('url');

        $.get(url)
            .done(function (data) {
                placeholderElement.empty()
                placeholderElement.html(data);
                placeholderElement.find('.modal').modal('show');
                $('.selectpicker').selectpicker('render');
            })
            .fail(function (xhr, status, error) {
                console.log(xhr.responseText);
            });
    });

    placeholderElement.on('click', '#EditUserSubmit', function (event) {
        event.preventDefault();

        let form = $('#EditUser');
        const actionUrl = form.attr('action');
        const dataToSend = form.serialize();

        $.post(actionUrl, dataToSend)
            .done(function (data) {
                var newBody = $('.modal-body', data);

                placeholderElement.find('.modal-body').replaceWith(newBody);
                $('.selectpicker').selectpicker('render');
                const isValid = newBody.find('[name="IsValid"]').val() == 'True';

                if (isValid) {
                    placeholderElement.find('.modal').modal('hide');
                    location.reload();
                }
            })
            .fail(function (xhr, status, error) {
                console.log(xhr.responseText)
            });
    });

    $('.delete-button').click(function (event) {
        const url = $(this).data('url');

        $.ajax({
            url: url,
            type: 'DELETE',
            success: function () {
                location.reload();
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText)
            }
        });
    });
});