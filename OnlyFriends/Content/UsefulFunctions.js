function timeSince(date) {

    var seconds = Math.floor((new Date() - date) / 1000);

    var interval = seconds / 31536000;

    if (interval > 1) {
        return Math.floor(interval) + " years ago";
    }
    interval = seconds / 2592000;
    if (interval > 1) {
        return Math.floor(interval) + " months ago";
    }
    interval = seconds / 86400;
    if (interval > 1) {
        return Math.floor(interval) + " days ago";
    }
    interval = seconds / 3600;
    if (interval > 1) {
        return Math.floor(interval) + " hours ago";
    }
    interval = seconds / 60;
    if (interval > 1) {
        return Math.floor(interval) + " minutes ago";
    }
    return Math.floor(seconds) + " seconds ago";
}

function changeFormat(date) {
    let dateParts = date.split("/")
    return Date.parse(dateParts[1] + "/" + dateParts[0] + "/" + dateParts[2])
}

window.onload = function () {
    let dates = document.getElementsByClassName("show-date");
    for (let i = 0; i < dates.length; i++) {
        dates[i].innerHTML = timeSince(changeFormat(dates[i].innerHTML));
    }
}