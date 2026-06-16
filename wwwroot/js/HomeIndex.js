let traffic = {}, tag = "", lastTag = "", queue = [], fastMode = false, zoom = 100;
$(document).ready(function () {
    $("#fastMode").click(function () {
        fastMode = !fastMode;
    });
    $.unblockUI();
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    getHistory();
    setInterval(getHistory, 60000);
});

$(document).keyup(function (e) {
    if ($("#modalTraffic").css("display") == "block" && e.keyCode == 27) {
        $("#modalTraffic").hide();
        return;
    }
    else if ($("#modalTraffic").css("display") == "block" && e.keyCode == 13) {
        $("#modalDelete").click();
        return;
    }
    else if ($("#modalTraffic").css("display") == "block" || $(window).data('blockUI.isBlocked')) return;
    if (e.keyCode == 27) { tag = ""; }
    else if (e.keyCode == 13) {
        if (!isNaN(tag) && tag.length > 8) {
            if (lastTag == tag && fastMode) {
                snack("کارت تکراری");
            }
            else if (fastMode) {
                getHistory(tag);
            }
            else {
                confirmData(tag);
            }
        } else {
            getHistory();
            snack("کارت معتبر نیست");
        }
        lastTag = tag;
        tag = "";
    } else {
        tag += e.key;
    }
    $("#tag").val(tag);
});

$("#modalCancel").click(function () { $("#modalTraffic").hide(); });

$("#modalDelete").click(function () {
    $.getJSON("./Home/DeleteTraffic/" + traffic.xId)
        .done(function (data) {
            $("#modalTraffic").hide();
            snack(data.message);
        })
        .fail(function (data) { snack(data.responseText); });
});

$("#modalSubmit").click(function () {
    let temp = {};
    $("#modalSubmit").prop("disabled", true);
    temp.xId = traffic.xId;
    temp.xTagId = traffic.xTagId;
    temp.xLicencePlateEn = $("#xLicencePlateEn").val();
    temp.xLicencePlateEx = $("#xLicencePlateEx").val();
    $.get("/Home/Traffic?id=" + traffic.xTagId, temp)
        .done((d) => {
            getHistory();
            $("#modalTraffic").hide();
        })
        .fail((d) => {
            console.log(d.toString());
            $("#modalSubmit").prop("disabled", false);
        });
});

function confirmData(id = "") {
    try {
        $.getJSON(`/Home/GetTraffic?tag=${id}`, function (data) {
            if (dir == 1 && data.xStatusCode != 1) {
                snack("کارت با شناسه " + id + " قبلا وارد شده است");
                return;
            }
            if (dir == 2 && data.xStatusCode != 2) {
                snack("کارت با شناسه " + id + " قبلا خارج شده است");
                return;
            }
            let imageStartName, caption;
            $("#xLicencePlateEn").prop("disabled", true);
            $("#xLicencePlateEx").prop("disabled", true);
            if (data.xStatusCode == 1) {
                imageStartName = "en-";
                caption = "ثبت ورود";
                $("#xLicencePlateEn").prop("disabled", false);

            }
            else if (data.xStatusCode == 2) {
                imageStartName = "ex-";
                caption = "ثبت خروج";
                $("#xLicencePlateEx").prop("disabled", false);
            }
            let enDate = data.xEntryDateTime == null ? "" : new Date(data.xEntryDateTime).persian();
            traffic = data;
            $("#modalSubmit").prop("disabled", false);
            $("#formTraffic #xTagId").html(data.xTagId);
            $("#formTraffic #xEntryDateTime").html(new Date(data.xEntryDateTime).persian(true, '/'));
            $("#formTraffic #xDepartureDateTime").html(data.xDepartureDateTime == null ? "" : new Date(data.xDepartureDateTime).persian(true, '/'));
            $("#formTraffic #xLicencePlateEn").val(data.xLicencePlateEn);
            $("#formTraffic #xLicencePlateEx").val(data.xLicencePlateEx);
            $("#formTraffic #xPaid").html(data.xPaid);
            $("#formTraffic #xTariff").html(data.xTariff.xTitle);
            $("#formTraffic #modalImage").attr("src", "./temp/" + imageStartName + data.xId + ".png?" + performance.now());
            if (data.xStatusCode == 1) $("#formTraffic #modalPlateEn").attr("src", "./temp/p-en-" + data.xId + ".png?" + performance.now());
            else $("#formTraffic #modalPlateEn").attr("src", "./ParkingImages/" + enDate + "/p-en-" + data.xId + ".png?" + performance.now());
            $("#formTraffic #modalPlateEx").attr("src", "./temp/p-ex-" + data.xId + ".png?" + performance.now());
            $("#modalSubmit").html(caption);
            $("#modalDelete").prop("disabled", (data.xStatusCode == 1));
            $("#modalTraffic").show();
        }).fail(function (e) {
            traffic = {};
            snack(`خطا ${e.status}<br/>${e.responseJSON.detail}`);
        });
    } catch (ex) {
        snack('خطا در صحت سنجی اطلاعات تردد');
    }
}

function getHistory(id = "") {
    $.getJSON(`Home/GetInquiry/${id}`)
        .done(function (result) {
            if (dir == 1 && result != null && id != "") {
                snack("کارت با شناسه " + id + " قبلا وارد شده است");
                return;
            }
            if (dir == 2 && result == null && id != "") {
                snack("کارت با شناسه " + id + " قبلا خارج شده است");
                return;
            }
            $.getJSON(`Home/Traffic?id=${id}`, function (data) {
                try {
                    queue = data;
                    if (queue.length > 0) UpdateLC();
                    if (queue.length > 1) UpdateHistory();
                } catch (ex) {
                    snack(`خطا<br/>${ex.message}`);
                }
            }).fail(function (e) {
                try {
                    snack(`خطا ${e.status}<br/>${e.responseJSON.detail}`);
                } catch (ex) {
                    snack(`خطا ${e.status}<br/>${e.responseText}`);
                }
            });
        })
        .fail(function () {
            snack("خطا در استعلام وضعیت کارت");
        });
    return;
}

function UpdateLC() {
    try {
        let len = queue.length;
        if (len == 0) return;
        $("#lcDelete").prop("disabled", false);
        let enDate = queue[len - 1].xEntryDateTime == null ? "" : new Date(queue[len - 1].xEntryDateTime).persian();
        let exDate = queue[len - 1].xDepartureDateTime == null ? "" : new Date(queue[len - 1].xDepartureDateTime).persian();
        $("#lcImgEn").attr("src", "./ParkingImages/" + enDate + "/en-" + queue[len - 1].xId + ".png?" + performance.now());
        $("#lcImgEn").attr("data-src", $("#lcImgEn").attr("src"));
        $("#lcImgEn").on("click", () => showModalImage($("#lcImgEn")));
        $("#lcImgEx").attr("src", "./ParkingImages/" + enDate + "/ex-" + queue[len - 1].xId + ".png?" + performance.now());
        $("#lcImgEx").attr("data-src", $("#lcImgEn").attr("src"));
        $("#lcImgEx").on("click", () => showModalImage($("#lcImgEx")));
        $("#lcImgEnP").attr("src", "./ParkingImages/" + enDate + "/p-en-" + queue[len - 1].xId + ".png?" + performance.now());
        $("#lcImgExP").attr("src", "./ParkingImages/" + enDate + "/p-ex-" + queue[len - 1].xId + ".png?" + performance.now());
        $("#lcPrice").html(queue[len - 1].xPaid);
        $("#lcTariff").html(queue[len - 1].xTariff.xTitle);
        $("#lcPlateEn").html(queue[len - 1].xLicencePlateEn);
        $("#lcPlateEx").html(queue[len - 1].xLicencePlateEx);
        $("#lcEntryTime").html(new Date(queue[len - 1].xEntryDateTime).persian(true, '/'));
        $("#lcDepartureTime").html(queue[len - 1].xDepartureDateTime == null ? "" : new Date(queue[len - 1].xDepartureDateTime).persian(true, '/'));
        $("#lcDelete").click(function () {
            $.getJSON("./Home/DeleteTraffic/" + queue[len - 1].xId)
                .done(function (data) {
                    $("#lcDelete").prop("disabled", true);
                    snack(data.message);
                })
                .fail(function (data) { snack(data.responseText); });
        });
    } catch (ex) { snack('خطا در بروزرسانی اطلاعات آخرین تردد'); }
}

function UpdateHistory() {
    try {
        let len = queue.length;
        if (len == 0) return;
        $("#history").html("");
        queue.reverse().slice(1, len).forEach(c => {
            let enDate = c.xEntryDateTime == null ? "" : new Date(c.xEntryDateTime).persian();
            let exDate = c.xDepartureDateTime == null ? "" : new Date(c.xDepartureDateTime).persian();
            let enDateTime = c.xEntryDateTime == null ? "" : new Date(c.xEntryDateTime).persian();
            let exDateTime = c.xDepartureDateTime == null ? "" : new Date(c.xDepartureDateTime).persian();
            let imgEn = `./ParkingImages/${enDate}/en-${c.xId}.png`, imgEx = `./ParkingImages/${exDate}/ex-${c.xId}.png`, imgEnp = `./ParkingImages/${enDate}/p-en-${c.xId}.png`;
            let card = `
                        <div class="w3-col s6 m4 l2 w3-padding-small">
                            <div id="ttt" class="w3-card w3-round-xlarge">
                                    <div class="w3-row">
                                        <div class="w3-col s6">
                                            <img class="w3-image w3-round w3-hover-opacity" src="${imgEn}" data-src="${imgEn}" onclick="showModalImage(this)" onerror="this.src='/imgs/cnf404.png'" />
                                        </div>
                                        <div class="w3-col s6">
                                                <img class="w3-image w3-round w3-hover-opacity" src="${imgEx}" data-src="${imgEx}" onclick="showModalImage(this)"  onerror="this.src='/imgs/cnf404.png'"/>
                                        </div>
                                    </div>
                                    <div class="w3-row w3-center">
                                                <img class="w3-image w3-round-large" src="${imgEnp}" onerror="this.src='/imgs/pnf404.png'" />
                                        <p>زمان ورود: ${enDateTime}</p>
                                        <p>زمان خروج: ${exDateTime}</p>
                                        <p><b>مبلغ ${c.xPaid} ریال</b></p>
                                    </div>
                            </div>
                        </div>
                    `;
            $("#history").append(card);
        });
    } catch (ex) { snack('خطا در بروزرسانی تاریخچه ترددها'); }
}

function zoomVideo(dir = 0, canvas = 0) {
    if (dir == 1) zoom += 20;
    else zoom -= 20;
    if (zoom < 20) zoom = 20;
    else if (zoom > 100) zoom = 100;
    // canvas zoom disabled - using MJPEG
    console.log("Zoom requested but disabled for MJPEG mode");
}