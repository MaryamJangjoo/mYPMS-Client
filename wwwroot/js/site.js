$(document).ready(function () {
    Date.prototype.persian = function (full = false, separator = '-') {
        return mts(this, full, separator);
        let mm = this.getMonth() + 1;
        let dd = this.getDate();
        let hh = this.getHours();
        let mmm = this.getMinutes();
        return [hh + ":" + mmm + " - " + this.getFullYear(),
        (mm > 9 ? '' : '0') + mm,
        (dd > 9 ? '' : '0') + dd].join('/');
    }
    $("#search").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#dataTable tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
    $("#btFullScreen").on("click", function () {
        document.fullScreenElement && null !== document.fullScreenElement || !document.mozFullScreen && !document.webkitIsFullScreen ? document.documentElement.requestFullScreen ? document.documentElement.requestFullScreen() : document.documentElement.mozRequestFullScreen ? document.documentElement.mozRequestFullScreen() : document.documentElement.webkitRequestFullScreen && document.documentElement.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT) : document.cancelFullScreen ? document.cancelFullScreen() : document.mozCancelFullScreen ? document.mozCancelFullScreen() : document.webkitCancelFullScreen && document.webkitCancelFullScreen();
        $("#btFullScreen").blur();
    });
});
function showModalImage(element) {
    document.getElementById("modalImageContent").src = $(element).data("src") + "?" + performance.now();
    document.getElementById("modalImage").style.display = "block";
}
function snack(text = '') {
    // Get the snackbar DIV
    var x = document.getElementById("snackbar");
    x.innerHTML = text;
    // Add the "show" class to DIV
    x.className = "show";
    // After 3 seconds, remove the show class from DIV
    setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
}
function mts(date, full = false, separator = '-') {
    var d = new Date(date);
    var gy = d.getFullYear(), gm = d.getMonth() + 1, gd = d.getDate();
    var g_d_m, jy, jm, jd, gy2, days;
    g_d_m = [0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334];
    gy2 = (gm > 2) ? (gy + 1) : gy;
    days = 355666 + (365 * gy) + ~~((gy2 + 3) / 4) - ~~((gy2 + 99) / 100) + ~~((gy2 + 399) / 400) + gd + g_d_m[gm - 1];
    jy = -1595 + (33 * ~~(days / 12053));
    days %= 12053;
    jy += 4 * ~~(days / 1461);
    days %= 1461;
    if (days > 365) {
        jy += ~~((days - 1) / 365);
        days = (days - 1) % 365;
    }
    if (days < 186) {
        jm = 1 + ~~(days / 31);
        jd = 1 + (days % 31);
    } else {
        jm = 7 + ~~((days - 186) / 30);
        jd = 1 + ((days - 186) % 30);
    }
    var jm2 = '' + jm, jd2 = '' + jd;
    if (jm2.length < 2)
        jm2 = '0' + jm2;
    if (jd2.length < 2)
        jd2 = '0' + jd2;
    return full ?
        [jy + separator + jm2 + separator + jd2 + ' ' + d.getHours() + ':' + d.getMinutes() + ':' + d.getSeconds()] :
        [jy + separator + jm2 + separator + jd2];
}
MvcGrid.lang = {
    default: {
        "equals": "برابر",
        "not-equals": "نابرابر"
    },
    text: {
        "contains": "حاوی",
        "equals": "برابر",
        "not-equals": "نابرابر",
        "starts-with": "شروع با",
        "ends-with": "پایان با"
    },
    number: {
        "equals": "برابر",
        "not-equals": "نابرابر",
        "less-than": "کمتر از",
        "greater-than": "بیشتر از",
        "less-than-or-equal": "کمتر یا برابر با",
        "greater-than-o-requal": "بیشتر یا برابر با"
    },
    date: {
        "equals": "برابر",
        "not-equals": "نابرابر",
        "earlier-than": "کمتر از",
        "later-than": "بیشتر از",
        "earlier-than-or-equal": "کمتر یا برابر با",
        "later-than-or-equal": "بیشتر یا برابر با"
    },
    guid: {
        "equals": "برابر",
        "not-equals": "نابرابر",
    },
    filter: {
        "apply": "✓",
        "remove": "✘"
    },
    operator: {
        "select": "",
        "and": "و",
        "or": "یا"
    }
};
