var NotificationService = function () {

    var getNewNotifications = function (done, fail) {
        $.getJSON("/api/notifications")
            .done(done)
            .fail(fail);
    };

    var dismissNotifications = function (fail) {
        $.ajax({
            url: "/api/notifications",
            method: "PUT"
        })
            .fail(fail);
    };

    return {
        getNewNotifications: getNewNotifications,
        dismissNotifications: dismissNotifications
    };
}();