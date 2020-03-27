//js code for showing password while clicking on checkboxes
$(document).ready(function () {
    $('body').on('click', ".showPass", function () {
        var id = $(this).data("id");
        var checked = $(this).prop('checked');
        
        if (checked) {
            $(id).attr("type", "text");
        } else {
            $(id).attr("type", "password");
        }
    });
});