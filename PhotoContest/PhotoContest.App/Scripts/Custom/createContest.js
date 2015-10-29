$(function () {

    function getDeadlinePartial() {
        $.get("/Contests/GetDeadlineStrategyPartial/" + $("#DeadlineStrategyId").val(), function (result) {
            $("#DeadlineStrategyPartial").html(result);
        });
    }

    function getRewardPartial() {
        $.get("/Contests/GetRewardStrategyPartial/" + $("#RewardStrategyId").val(), function (result) {
            $("#RewardStrategyPartial").html(result);
        });
    }

    getDeadlinePartial();
    getRewardPartial();

    $("#DeadlineStrategyId").change(function() {
        getDeadlinePartial();
    });

    $("#RewardStrategyId").change(function () {
        getRewardPartial();
    });
});