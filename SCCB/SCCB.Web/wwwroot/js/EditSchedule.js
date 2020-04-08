$(document).ready(function () {
    var placeholderElement = $('#AddLessonPlaceholder');
    var weekdays = ["Понеділок", "Вівторок", "Середа", "Четвер", "П'ятниця"];

    function refreshLessonsList(groupId, weekday) {
        var url = '/Admin/LessonsForDay';

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

    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');

        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
            $('.selectpicker').selectpicker('render');
        });
    });

    placeholderElement.on('click', '#AddLessonSubmit', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {
            var newBody = $('.modal-body', data);

            placeholderElement.find('.modal-body').replaceWith(newBody);
            var isValid = newBody.find('[name="IsValid"]').val() == 'True';

            if (isValid) {
                placeholderElement.find('.modal').modal('hide');

                var groupId = newBody.find('[name="GroupId"]').val();
                var weekday = newBody.find('[name="Weekday"]').val();

                refreshLessonsList(groupId, weekday);
            }
        });
    });

    $('close-button').click(function (event) {
          //TODO
    }
});