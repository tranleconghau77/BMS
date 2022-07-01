$(".wrap-action>i.fa.fa-chevron-down").click(function () {
    let result = $(".actionadmin").hasClass("d-none");
    if (result) {
        $(".actionadmin").removeClass("d-none");
        $(".actionadmin").addClass("d-block");
    }
    else {
        $(".actionadmin").removeClass("d-block");
        $(".actionadmin").addClass("d-none");
    }
});
