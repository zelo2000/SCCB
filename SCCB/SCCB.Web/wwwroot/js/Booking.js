$(document).ready(function () {
    const refreshClassroomsUrl = '/Booking/FreeClassrooms'
    var classroomSelect = $('#ClassroomId');

    $(".input-validation-error").parent().css('margin-top', 20);

    if ($('#Date').val() != "" && $('#LessonNumber').val() != "") {
        classroomSelect.removeAttr('disabled')
        classroomSelect.selectpicker({ title: '-- Оберіть аудиторію --' });
        classroomSelect.selectpicker('refresh');
    }

    $('#Date,#LessonNumber').change(function () {
        const date = $('#Date').val();
        const lessonNumber = $('#LessonNumber').val();

        if ($('#Date').val() != "" && $('#LessonNumber').val() != "") {
            $.get(refreshClassroomsUrl,
                {
                    date: date,
                    lessonNumber: lessonNumber
                },
                function (data) {
                    classroomSelect.html(data);
                    classroomSelect.removeAttr('disabled')
                    classroomSelect.selectpicker({ title: '-- Оберіть аудиторію --' });
                    classroomSelect.selectpicker('refresh');
                })
                .fail(function (xhr, status, error) {
                    console.log(xhr.responseText)
                });
        }
    });
});