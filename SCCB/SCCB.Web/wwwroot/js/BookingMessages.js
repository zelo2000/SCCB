$(document).ready(function () {
    let connection = new signalR.HubConnectionBuilder().withUrl("/bookingHub").build();
    connection.start();

    $('#Bookings').on('click', '.approve-btn', function () {
        const userId = $(this).data('user');
        connection.invoke("SendMessage", userId, "підтверджено").catch(function (err) {
            return console.error(err.toString());
        });
    });

    $('#Bookings').on('click', '.reject-btn', function () {
        const userId = $(this).data('user');
        connection.invoke("SendMessage", userId, "відхилено").catch(function (err) {
            return console.error(err.toString());
        });
    });

    $('form[name="main-form"]').on('click', '.book-button', function (event) {
        event.preventDefault();
        setTimeout(function () {
            $('form[name="main-form"]').submit();
        }, 1000);

        var isValid = $('form[name="main-form"]').find('[name="IsValid"]').val() == 'True';

        if (isValid) {
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-top-right",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "0",
                "extendedTimeOut": "0",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }

            Command: toastr["info"]("Ваше бронювання очікує підтвердження")
        }
    });

    connection.on("ReceiveMessage", function (message) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": true,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }

        generalText = "Ваше бронювання "

        if (message == "відхилено") {
            Command: toastr["error"](generalText + message)
        }
        else if (message == "підтверджено") {
            Command: toastr["success"](generalText + message)
        }
    });
});