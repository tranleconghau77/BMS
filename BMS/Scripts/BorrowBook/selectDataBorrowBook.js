$(document).ready(function () {
    $(".borrowbook.table>tbody>tr.action").hide();

});


$(".borrowbook.table>tbody>tr").on('click', function () {
    let id = $(this).data("id");
    //  ret = DetailsView.GetProject($(this).attr("#data-id"), OnComplete, OnTimeOut, OnError);
    $(`.borrowbook.table>tbody>tr.action[data-id="${id}"]`).toggle();
    
});

