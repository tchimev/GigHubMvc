var NotificationsController = function (notificationService) {

    var init = function () {
        notificationService.getNewNotifications(done, fail);
    };

    var done = function (notifications) {
        if (notifications.length == 0)
            return;

        $(".js-notifications-count")
            .text(notifications.length)
            .addClass("animated bounceInDown");

        $(".notifications").popover({
            html: true,
            title: "Notifications",
            content: renderNotifications(notifications),
            placement: "bottom"
        }).on("shown.bs.popover", notificationService.dismissNotifications(fail));
    };

    var fail = function () {
        console.log("Error notifications");
    };

    var renderNotifications = function (data) {
        return Mustache.render($("#notifications-template").html(), { notifications: data });
    };

    return {
        init: init
    };

}(NotificationService);