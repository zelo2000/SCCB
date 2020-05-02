$(document).ready(function () {
    let connection = new signalR.HubConnectionBuilder().withUrl("/bookingHub").build();
    connection.start();

    $('#Bookings').on('click', '.approve-btn', function () {
        const userId = $(this).data('user');
        connection.invoke("SendMessage", userId, "Approved").catch(function (err) {
            return console.error(err.toString());
        });
    });

    $('#Bookings').on('click', '.reject-btn', function () {
        const userId = $(this).data('user');
        connection.invoke("SendMessage", userId, "Rejected").catch(function (err) {
            return console.error(err.toString());
        });
    });

    connection.on("ReceiveMessage", function (message) {
        console.log(message);
    });
});