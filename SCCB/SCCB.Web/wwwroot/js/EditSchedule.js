$(document).ready(function () {
    var placeholderElement = $('#AddLessonPlaceholder');

    const weekdays = ["Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця"];

    const refreshLessonsUrl = '/Admin/LessonsForDay';
    const refreshClassroomsUrl = '/Admin/FreeClassrooms';
    const refreshLectorsUrl = '/Admin/FreeLectors';

    function refreshLessonsList(url, groupId, weekday) {
        $.get(url,
            {
                groupId: groupId,
                weekday: weekday
            },
            function (data) {
                const index = weekdays.indexOf(weekday);
                let lessonsList = $('#lessonsFor' + index);
                lessonsList.html(data);
            }
        );
    }

    function refreshClassroomOptions(url, weekday, number, isNumerator, isDenominator) {
        $.get(
            url,
            {
                weekday: weekday,
                number: number,
                isNumerator: isNumerator,
                isDenominator: isDenominator
            },
            function (data) {
                let classroomSelect = placeholderElement.find('#classroomId');
                classroomSelect.html(data);
                classroomSelect.prop('disabled', false);
                classroomSelect.selectpicker({ title: '-- Оберіть аудиторію --' });
                classroomSelect.selectpicker('refresh');
            }
        );
    }

    function refreshLectorOptions(url, weekday, number, isNumerator, isDenominator) {
        $.get(
            url,
            {
                weekday: weekday,
                number: number,
                isNumerator: isNumerator,
                isDenominator: isDenominator
            },
            function (data) {
                let lectorSelect = placeholderElement.find('#lectorId');
                lectorSelect.html(data);
                lectorSelect.prop('disabled', false);
                lectorSelect.selectpicker({ title: '-- Оберіть викладача --' });
                lectorSelect.selectpicker('refresh');
            }
        );
    }

    $('[data-toggle="ajax-modal"]').click(function (event) {
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

    placeholderElement.on('click', '#AddLessonSubmit', function (event) {
        event.preventDefault();

        let form = $(this).parents('.modal').find('form');
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

                    const groupId = newBody.find('[name="GroupId"]').val();
                    const weekday = newBody.find('[name="Weekday"]').val();

                    refreshLessonsList(refreshLessonsUrl, groupId, weekday);
                }
                else {
                    const weekday = placeholderElement.find('#weekday').val();
                    const number = placeholderElement.find('#lessonNumber').val();
                    const isNumerator = placeholderElement.find('#isNumerator').is(':checked');
                    const isDenominator = placeholderElement.find('#isDenominator').is(':checked');

                    refreshClassroomOptions(refreshClassroomsUrl, weekday, number, isNumerator, isDenominator);
                    refreshLectorOptions(refreshLectorsUrl, weekday, number, isNumerator, isDenominator);
                }
            })
            .fail(function (xhr, status, error) {
                console.log(xhr.responseText)
            });
    });

    $('#accordion').on('click', '.remove-lesson', function () {
        const url = $(this).data('url');
        const groupId = $(this).data('group-id');
        const weekday = $(this).data('weekday');

        $.ajax({
            url: url,
            type: 'DELETE',
            success: function () {
                refreshLessonsList(refreshLessonsUrl, groupId, weekday)
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText)
            }
        });
    });

    placeholderElement.on("change", '[data-type="lesson-date"]', function () {
        const weekday = placeholderElement.find('#weekday').val();
        const number = placeholderElement.find('#lessonNumber').val();
        const isNumerator = placeholderElement.find('#isNumerator').is(':checked');
        const isDenominator = placeholderElement.find('#isDenominator').is(':checked');

        refreshClassroomOptions(refreshClassroomsUrl, weekday, number, isNumerator, isDenominator);
        refreshLectorOptions(refreshLectorsUrl, weekday, number, isNumerator, isDenominator);
    });
});