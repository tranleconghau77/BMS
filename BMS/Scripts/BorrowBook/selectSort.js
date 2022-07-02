$("#selectSortbtn").click(function () {
    let conceptName = $('#selectSort').find(":selected").text();
    $("#selectSort").val(conceptName).change();
})

$('#selectSort').on('change', function () {
    // Save value in localstorage
    console.log($(this).val());
    localStorage.setItem("selectSort", $(this).val());
});

$(document).ready(function () {
    if ($('#selectSort').length) {
        $('#selectSort').val(localStorage.getItem("selectSort"));
    }
});