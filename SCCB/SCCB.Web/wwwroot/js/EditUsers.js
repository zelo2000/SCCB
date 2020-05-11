function selectAdder(selectValue, inputId) {
    if ($('#role').find("option:selected").text() == selectValue) {
        $(inputId).fadeIn();
    }
    else {
        $(inputId).hide();
    }
}

$(document).ready(function () {
    var placeholderElement = $('#EditUserPlaceholder');

    $('.edit-button').click(function () {
        const url = $(this).data('url');

        $.get(url)
            .done(function (data) {
                placeholderElement.empty()
                placeholderElement.html(data);
                placeholderElement.find('.modal').modal('show');
                selectAdder("Lector", "#position-hidden");
                selectAdder("Student", "#studentId-hidden");
                $('.selectpicker').selectpicker('render');

                $('#role').change(function () {
                    selectAdder("Lector", "#position-hidden");
                    selectAdder("Student", "#studentId-hidden");
                });
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
                selectAdder("Lector", "#position-hidden");
                selectAdder("Student", "#studentId-hidden");
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

    $('.close-button').click(function () {
        const url = $('button[data-target="#confirmModal"]').data('url');

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