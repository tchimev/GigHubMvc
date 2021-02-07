var GigService = function () {

    var cancelGig = function (gigId, done, fail) {
        $.ajax({
            url: "/api/gigs/" + gigId,
            method: "DELETE"
        })
            .done(done)
            .fail(fail);
    };

    return {
        cancelGig: cancelGig
    };
}();