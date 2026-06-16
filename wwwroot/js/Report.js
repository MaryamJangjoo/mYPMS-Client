function getDetail(id) {
    $.getJSON("/Report/GetTraffic/?guid=" + id)
        .done(function (data) {
            $("#modalId").val(id);
            $("#xEntryDateTime").html("" + new Date(data.xEntryDateTime).persian(true, '/'));
            $("#xDepartureDateTime").html("" + new Date(data.xDepartureDateTime).persian(true, '/'));
            $("#xLicencePlateEn").html("" + data.xLicencePlateEn);
            $("#xLicencePlateEx").html("" + data.xLicencePlateEx);
            $("#xPaid").html("" + data.xPaid);
            $("#xTariff").html("" + data.xTariff.xTitle);
            $("#xTagId").html("" + data.xTagId);
            $("#modalImageEn").prop("src", `/ParkingImages/${new Date(data.xEntryDateTime).persian()}/en-${data.xId}.png`);
            $("#modalPlateEn").prop("src", `/ParkingImages/${new Date(data.xEntryDateTime).persian()}/p-en-${data.xId}.png`);
            $("#modalImageEx").prop("src", `/ParkingImages/${new Date(data.xDepartureDateTime).persian()}/ex-${data.xId}.png`);
            $("#modalPlateEx").prop("src", `/ParkingImages/${new Date(data.xDepartureDateTime).persian()}/p-ex-${data.xId}.png`);
            $("#modalDetail").show();
        })
        .fail(function () { snack("خطا در دریافت اطلاعات"); });
}
function changeStatus(ns) {
    $.get(`/Report/ChangeStatus/?id=${$("#modalId").val()}&newStatus=${ns}`)
        .done(function () {
            const grid = new MvcGrid(document.querySelector(".mvc-grid"));
            grid.reload();
            $("#modalDetail").hide();
            snack("وضعیت جدید با موفقیت اعمال شد");
        })
        .fail(function (e) { snack(e.responseJSON.detail); });
}
$("#modalEnter").click(function () { changeStatus(1) });
$("#modalDeparture").click(function () { changeStatus(2) });
$("#modalDelete").click(function () { changeStatus(4) });