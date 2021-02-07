var GigsController = function (gigService, attendanceService) {
    var btn;
    var cancelGigLink;

    var initAttendance = function (container) {
        $(container).on("click", ".js-toggle-attendance", toggleAttendance);
    };

    var initCancelGig = function () {
        $(".js-cancel-gig").click(cancelGig);
    };

    var cancelGig = function (e) {
        cancelGigLink = $(e.target);
        var gigId = cancelGigLink.attr("data-gig-id");
        bootbox.dialog({
            message: "Are you sure you want to cancel this Gig?",
            title: "Confirm",
            buttons: {
                No: {
                    label: "No",
                    className: "btn-default",
                    callback: function () {
                        bootbox.hideAll();
                    }
                },
                Yes: {
                    label: "Yes",
                    className: "btn-danger",
                    callback: function () {
                        gigService.cancelGig(gigId, doneCancelGig, failCancelGig);
                    }
                }
            }
        });
    };

    var doneCancelGig = function () {
        cancelGigLink.parents("li")
                                    .fadeOut(function () {
                                        $(this).remove();
                                    });
    };

    var failCancelGig = function () {
        alert("Error cancel Gig");
    };

    var toggleAttendance = function (e) {
        btn = $(e.target);
        var gigId = btn.attr("data-gig-id");
        if (btn.hasClass("btn-default")) {
            attendanceService.createAttendance(gigId, done, fail);
        }
        else {
            attendanceService.deleteAttendance(gigId, done, fail);
        }
    };

    var done = function () {
        var str = (btn.text() == "Going") ? "Going?" : "Going";
        btn
            .toggleClass("btn-info")
            .toggleClass("btn-default")
            .text(str);
    };

    var fail = function () {
        alert("Error attendance");
    };

    return {
        initAttendance: initAttendance,
        initCancelGig: initCancelGig
    }
}(GigService, AttendanceService);
