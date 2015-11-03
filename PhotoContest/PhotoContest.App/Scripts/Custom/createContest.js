$(function () {

    function getDeadlinePartial() {
        $("#DeadlineStrategyPartial").html("");

        $.get("/Strategies/GetDeadlineStrategyPartial/" + $("#DeadlineStrategyId").val(), function (result) {
            $("#DeadlineStrategyPartial").html(result);
        });
    }

    function getRewardPartial() {
        $("#RewardStrategyPartial").html("");

        $.get("/Strategies/GetRewardStrategyPartial/" + $("#RewardStrategyId").val(), function (result) {
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