

$("#meeting-time").bind("change paste keyup", function () {
    let datetime = $(this).val() + "";
    console.log(Date.parse(datetime));
    let date = new Date(datetime);

    console.log(date.addDays(5));
    console.log(date.toLocaleString('en-US'));
    $(".expire_date").val(formatDate(new Date(date.addDays(14))));
    const timestamp = new Date().getTime();

    // 👇️ 01/20/2022 10:07:59 (mm/dd/yyyy hh:mm:ss)
    console.log(formatDate(new Date(date.addDays(5))));
});

Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}


//double ticks = double.Parse(startdatetime);
//TimeSpan time = TimeSpan.FromMilliseconds(ticks);
//DateTime startdate = new DateTime(1970, 1, 1) + time;

function padTo2Digits(num) {
    return num.toString().padStart(2, '0');
}

function formatDate(date) {
    return (
        [
            padTo2Digits(date.getMonth() + 1),
            padTo2Digits(date.getDate()),
            date.getFullYear(),
        ].join('/') +
        ' ' +
        [
            padTo2Digits(date.getHours()),
            padTo2Digits(date.getMinutes()),
            padTo2Digits(date.getSeconds()),
        ].join(':')
    );
}

