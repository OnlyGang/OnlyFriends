window.onload = function () {
    // Showing the dates format
    let dates = document.getElementsByClassName("show-date");
    for (let i = 0; i < dates.length; i++) {
        dates[i].innerHTML = moment(dates[i].innerHTML, "DD/MM/YY hh:mm:ss").fromNow()
    }
}