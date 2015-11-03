namespace PhotoContest.App.Controllers
{
    using PhotoContest.Infrastructure.Interfaces;
    using System;
    using System.Web.Mvc;

    public class StrategiesController : Controller
    {
        private IStrategyService _strategyService;

        public StrategiesController(IStrategyService strategyService)
        {
            this._strategyService = strategyService;
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetRewardStrategyPartial(int id)
        {
            try
            {
                var viewModel = this._strategyService.GetRewardStrategyOptions(id);

                return PartialView("Reward/_" + viewModel.Type + "Partial", viewModel);
            }
            catch (Exception)
            {
                return this.Content("");
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetDeadlineStrategyPartial(int id)
        {
            try
            {
                var viewModel = this._strategyService.GetDeadlineStrategyOptions(id);

                return PartialView("Deadline/_" + viewModel.Type + "Partial", viewModel);
            }
            catch (Exception)
            {
                return this.Content("");
            }
        }
    }
}