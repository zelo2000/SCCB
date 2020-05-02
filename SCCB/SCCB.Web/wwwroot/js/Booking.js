$(document).ready(function () {

    const refreshClassroomsUrl = '/Booking/FreeClassrooms'
    $('#Date,#LessonNumber').change(function () {
        const date = $('#Date').val();
        const lessonNumber = $('#LessonNumber').val();

        $.get(refreshClassroomsUrl,
            {
                date: date,
                lessonNumber: lessonNumber
            },
            function (data) {
                let classroomSelect = $('#ClassroomId');
                classroomSelect.html(data);
                classroomSelect.prop('disabled', false);
                classroomSelect.selectpicker({ title: '-- Оберіть аудиторію --' });
                classroomSelect.selectpicker('refresh');
            })
            .fail(function (xhr, status, error) {
                console.log(xhr.responseText)
            });
    });
});