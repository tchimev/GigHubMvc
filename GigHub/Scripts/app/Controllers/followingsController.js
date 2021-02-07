var FollowingsController = function (followingService) {
    var btn;

    var init = function (container) {
        $(container).on("click", ".js-toggle-follow", toggleFollow);
    };

    var toggleFollow = function (e) {
        btn = $(e.target);
        var userId = btn.attr('data-user-id');
        if (btn.hasClass("btn-default"))
            followingService.followUser(userId, done, fail);
        else
            followingService.unfollowUser(userId, done, fail);
    };

    var done = function () {
        var str = (btn.text() == "Follow") ? "Following" : "Follow";
        btn.toggleClass("btn-info").toggleClass("btn-default").text(str);
    };

    var fail = function () {
        alert('Error follow');
    };

    return {
        init: init
    };
}(FollowingService);