$(document).ready(function () {

    function refreshBookingsList() {
        const url = '/Admin/Bookings';
        const date = $('#Date').val();
        const lessonNumber = $('#LessonNumber').val();
        const classroomId = $('#ClassroomId').val();

        $.get(url,
            {
                date: date,
                lessonNumber: lessonNumber,
                classroomId: classroomId
            },
            function (data) {
                $('#Bookings').html(data);
            }
        );
    }

    $('#Bookings').on('click', '.approve-btn', function () {
        const url = $(this).data('url');
        console.log(url);
        $.post(url)
            .done(refreshBookingsList)
            .fail(function (xhr, status, error) {
                console.log(xhr.responseText);
            });
    });

    $('#Bookings').on('click', '.reject-btn', function () {
        const url = $(this).data('url');
        console.log(url);
        $.ajax({
            url: url,
            type: 'DELETE',
            success: refreshBookingsList,
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
            }
        });
    });
})