$(document).ready(function () {
    var classroomSelect = $('#ClassroomId');

    $(".clear-all").click(function () {
        const form = $('form[name="main-form"]');

        form.find(".selectpicker").each(function () {
            $(this).val('default');
            $(this).selectpicker('render');
        });

        form.find("input, textarea").val("");

        if (($('#Date').val() == "" || $('#LessonNumber').val() == null) &&
            form.attr('method') == 'post')
        {
            classroomSelect.prop('disabled', true)
            classroomSelect.selectpicker({ title: '-- Спершу вкажіть дату та номер пари --' });
            classroomSelect.selectpicker('refresh');
        }

        if (form.attr('method') == 'get') {
            form.submit();
        }
    });
});