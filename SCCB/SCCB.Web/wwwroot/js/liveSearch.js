//js code for live searching
$(document).ready(function () {
    $("#filter").keyup(function () {

        // retrieve the input field text and reset the count to zero
        var filter = $(this).val(), count = 0;

        // loop through the comment list
        $(".liveSearchItem").each(function () {
            var searchItems = $(this).find('span');

            substring = $(searchItems).text().substring(0, filter.length);

            // if the list item does not contain the text phrase fade it out
            if (substring.search(new RegExp(filter, "i")) < 0) {
                $(this).fadeOut();

                // show the list item if the phrase matches and increase the count by 1
            } else {
                $(this).show();
                count++;
            }
        });

        // update the count
        $("#filter-count").text("збігів найдено: " + count);
    });
});