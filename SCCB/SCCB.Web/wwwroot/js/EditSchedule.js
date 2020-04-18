$(document).ready(function () {
    var placeholderElement = $('#AddLessonPlaceholder');
    var placeholderElementEdit = $('#EditLessonPlaceholder');
    // TODO Try to find solution
    var weekdays = ["Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця"];

    var refreshLessonsUrl = '/Admin/LessonsForDay';
    var refreshClassroomsUrl = '/Admin/FreeClassrooms';
    var refreshLectorsUrl = '/Admin/FreeLectors';

    function refreshLessonsList(url, groupId, weekday) {
        $.get(url,
            {
                groupId: groupId,
                weekday: weekday
            },
            function (data) {
                var index = weekdays.indexOf(weekday);
                var lessonsList = $('#lessonsFor' + index);
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
                var classroomSelect = placeholderElement.find('#classroomId');
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
                var lectorSelect = placeholderElement.find('#lectorId');
                lectorSelect.html(data);
                lectorSelect.prop('disabled', false);
                lectorSelect.selectpicker({ title: '-- Оберіть викладача --' });
                lectorSelect.selectpicker('refresh');
            }
        );
    }

    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url)
            .done(function (data) {
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

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();

        $.post(actionUrl, dataToSend)
            .done(function (data) {
                var newBody = $('.modal-body', data);

                placeholderElement.find('.modal-body').replaceWith(newBody);
                $('.selectpicker').selectpicker('render');
                var isValid = newBody.find('[name="IsValid"]').val() == 'True';

                if (isValid) {
                    placeholderElement.find('.modal').modal('hide');

                    var groupId = newBody.find('[name="GroupId"]').val();
                    var weekday = newBody.find('[name="Weekday"]').val();

                    refreshLessonsList(refreshLessonsUrl, groupId, weekday);
                }
                else {
                    var weekday = placeholderElement.find('#weekday').val();
                    var number = placeholderElement.find('#lessonNumber').val();
                    var isNumerator = placeholderElement.find('#isNumerator').is(':checked');
                    var isDenominator = placeholderElement.find('#isDenominator').is(':checked');

                    refreshClassroomOptions(refreshClassroomsUrl, weekday, number, isNumerator, isDenominator);
                    refreshLectorOptions(refreshLectorsUrl, weekday, number, isNumerator, isDenominator);
                }
            })
            .fail(function (xhr, status, error) {
                console.log(xhr.responseText)
            });
    });

    placeholderElement.on("change", '[data-type="lesson-date"]', function () {
        var weekday = placeholderElement.find('#weekday').val();
        var number = placeholderElement.find('#lessonNumber').val();
        var isNumerator = placeholderElement.find('#isNumerator').is(':checked');
        var isDenominator = placeholderElement.find('#isDenominator').is(':checked');

        refreshClassroomOptions(refreshClassroomsUrl, weekday, number, isNumerator, isDenominator);
        refreshLectorOptions(refreshLectorsUrl, weekday, number, isNumerator, isDenominator);
    });
});