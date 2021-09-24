// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var httpurl = 'https://localhost:44330/';
var pageCount = 10;
var regexPhone = RegExp('^09[0-9]{8}$', 'g');
var regexTel = RegExp('^0[0-9-+]+$', 'g');
var regexEmail = RegExp(/^\w{1,100}@@[a-zA-Z0-9]{2,100}\.[a-zA-Z]{2,100}(\.[a-zA-Z]{2,100})?$/, 'g');

function SetAlertModel(title, msg) {
    $("#alertTitle").text(title);
    $("#alertMsg").text(msg);
    $("#alert").modal("show");
}

function setDateFormat(date) {
    if (date == null) return '';

    var date = moment(date).format('YYYY/MM/DD hh:mm:ss');
    if (date == '0001/01/01 12:00:00') return '';

    return date;
}

function setDateFormatToMins(date) {
    if (date == null) return '';

    var mins = moment(date).format('YYYY/MM/DD hh:mm');
    if (mins == '0001/01/01 12:00') return '';

    return mins;
}

function setDateFormatToDay(date) {
    if (date == null) return '';

    var today = moment(date).format('YYYY/MM/DD');
    if (today == '0001/01/01') return '';

    return today;
}

function transformMins(value) {
    var min = parseInt(value / 60);
    var second = Math.floor(value % 60).toString();

    if (second.length === 1) return min + ':0' + second;

    return min.toString() + ':' + second;
}

function CalYear(value) {
    return moment().diff(value, 'years');
}

function setMenu() {
    $("#news").removeClass('active');
    $("#activity").removeClass('active');
    $("#download").removeClass('active');
    $("#faq").removeClass('active');

    var Item = Cookies.get('HomeMenu');

    if (Item != undefined) {
        if (Item == 'News') $("#news").addClass('active');
        if (Item == 'FAQ') $("#faq").addClass('active');
        if (Item == 'DownLoad') $("#download").addClass('active');
        if (Item == 'Activity') $("#activity").addClass('active');
    }
}

function showInfo(value, title, cookieName) {
    if (value != undefined) {
        $("#alertTitle").text(title);
        $("#alertMsg").text(value);
        $("#alert").modal('show');
        Cookies.remove(cookieName);
    }
}
