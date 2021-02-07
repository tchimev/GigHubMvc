var FollowingService = function () {

    var followUser = function (userId, done, fail) {
        $.post('/api/followings', { FolloweeId: userId })
            .done(done)
            .fail(fail);
    };

    var unfollowUser = function (userId, done, fail) {
        $.ajax({
            url: "/api/followings/" + userId,
            method: "DELETE"
        })
        .done(done)
        .fail(fail);
    };

    return {
        followUser: followUser,
        unfollowUser: unfollowUser
    };
}();