namespace PhotoContest.App.Controllers
{
    #region
    using System;
    using System.Net;
    using System.Web;
    using PhotoContest.Models;

    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using PhotoContest.App.Models.BindingModels.Contest;
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Data.Strategies;

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using PhotoContest.App.Models.ViewModels.Contest;
    using PhotoContest.App.Models.ViewModels.Invitation;
    using PhotoContest.App.Models.ViewModels.Strategy;
    using PhotoContest.Models.Enums;
    #endregion

    public class ContestsController : BaseController
    {
        private const int MaxImageSize = 1000000;

        public ContestsController(IPhotoContestData data)
            : base(data)
        {
        }

        public ActionResult AllContests()
        {
            var allContests =
                this.Data.Contests.All()
                    .OrderByDescending(c => c.StartDate)
                    .Project()
                    .To<ContestViewModel>()
                    .ToList();

            return this.PartialView("_AllContestsPartial", allContests);
        }

        [HttpGet]
        public ActionResult Contests()
        {
            return this.View();
        }

        [HttpGet]
        public ActionResult Contest(int id)
        {
            return null;
        }

        [HttpGet]
        public ActionResult ActiveContests()
        {
            var activeContests = this.Data.Contests.All()
                .Where(c => c.IsActive)
                .Project()
                .To<ContestViewModel>()
                .ToList();

            return this.PartialView(activeContests);
        }

        [HttpGet]
        public ActionResult InactiveContests()
        {
            var activeContests = this.Data.Contests.All()
                  .Where(c => c.IsActive == false)
                  .Project()
                  .To<ContestViewModel>()
                  .ToList();

            return this.PartialView("_InactiveContestsPartial", activeContests);
        }

        [Authorize]
        [HttpGet]
        public ActionResult MyContests()
        {
            var loggedUserId = this.User.Identity.GetUserId();

            var myContests = this.Data.Contests.All()
                .Where(c => c.OrganizatorId == loggedUserId)
                .Project()
                .To<ContestViewModel>()
                .ToList();

            return this.PartialView(myContests);
        }

        [Authorize]
        [HttpGet]
        public ActionResult NewContest()
        {
            var viewModel = new CreateContestViewModel();

            viewModel.RewardStrategies = this.Data.RewardStrategies.All()
                .Select(r => new StrategyViewModel() { Id = r.Id, Description = r.Description, Name = r.Name })
                .ToList();

            viewModel.ParticipationStrategies = this.Data.ParticipationStrategies.All()
                .Select(p => new StrategyViewModel() { Id = p.Id, Description = p.Description, Name = p.Name })
                .ToList();

            viewModel.VotingStrategies = this.Data.VotingStrategies.All()
                .Select(v => new StrategyViewModel() { Id = v.Id, Description = v.Description, Name = v.Name })
                .ToList();

            viewModel.DeadlineStrategies = this.Data.DeadlineStrategies.All()
                .Select(dl => new StrategyViewModel() { Id = dl.Id, Description = dl.Description, Name = dl.Name})
                .ToList();

            return this.View("NewContestForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateContest(CreateContestBindingModel model)
        {
            if (model == null)
            {
                this.Response.StatusCode = 400;
                return this.Content("Missing Data");
            }

            if (!this.ModelState.IsValid)
            {
                this.Response.StatusCode = 400;
                return this.Json(this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }

            var loggedUserId = this.User.Identity.GetUserId();

            var contest = new Contest
            {
                Title = model.Title,
                Description = model.Description,
                IsActive = true,
                RewardStrategyId = model.RewardStrategyId,
                VotingStrategyId = model.VotingStrategyId,
                ParticipationStrategyId = model.ParticipationStrategyId,
                DeadlineStrategyId = model.DeadlineStrategyId,
                ParticipantsLimit = model.ParticipantsLimit,
                IsOpenForSubmissions = true,
                StartDate = DateTime.Now,
                EndDate = model.EndDate,
                OrganizatorId = loggedUserId,
            };

            this.Data.Contests.Add(contest);
            this.Data.SaveChanges();

            return new HttpStatusCodeResult(200);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Join(int id)
        {
            var user = this.Data.Users.Find(this.User.Identity.GetUserId());
            var contest = this.Data.Contests.Find(id);

            var messages = new List<string>();

            try
            {
                this.DeadlineStrategy =
                    StrategyFactory.GetDeadlineStrategy(contest.DeadlineStrategy.DeadlineStrategyType);

                this.DeadlineStrategy.Deadline(this.Data, contest, user);

                this.ParticipationStrategy =
                    StrategyFactory.GetParticipationStrategy(contest.ParticipationStrategy.ParticipationStrategyType);

                this.ParticipationStrategy.Participate(this.Data, user, contest);

                if (contest.Participants.Contains(user))
                {
                    throw new ArgumentException("You already participate in this contest");
                }

                contest.Participants.Add(user);
                this.Data.Contests.Update(contest);
                this.Data.SaveChanges();
            }
            catch (ArgumentException e)
            {
                messages.Add(e.Message);
            }
            catch (InvalidOperationException e)
            {
                messages.Add(e.Message);
                return this.Content(e.Message);
            }

            this.TempData["Messages"] = messages;

            return null;
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(int id)
        {
            if (this.Request.Files.Count < 1)
            {
                this.Response.StatusCode = 400;
                return this.Json(new { ErrorMessage = "No file data" });
            }

            var files = new List<HttpPostedFileBase>();

            for (int i = 0; i < this.Request.Files.Count; i++)
            {
                var result = this.ValidateImageData(this.Request.Files[i]);

                if (result != null)
                {
                    return result;
                }

                files.Add(this.Request.Files[i]);
            }

            var contest = this.Data.Contests.Find(id);

            if (contest == null)
            {
                throw new HttpException(404, "Contest with such id does not exists");
            }

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            try
            {
                this.DeadlineStrategy = StrategyFactory.GetDeadlineStrategy(contest.DeadlineStrategy.DeadlineStrategyType);

                this.DeadlineStrategy.Deadline(this.Data, contest, user);

                foreach (var file in files)
                {
                    var base64PictureString = GetBase64String(file);

                    Picture picture = new Picture
                    {
                        UserId = user.Id,
                        Url = base64PictureString,
                        ContestId = contest.Id
                    };

                    this.Data.Pictures.Add(picture);
                }

                this.Data.SaveChanges();

                this.Response.StatusCode = 200;

                return null;
            }
            catch (InvalidOperationException e)
            {
                this.TempData["message"] = e.Message;
            }

            this.Response.StatusCode = 400;
            return this.Json(new { ErrorMessage = this.TempData["message"] });
        }

        [HttpGet]
        public ActionResult PreviewContest(int id)
        {
            // TODO Change login based on is contest active or not!
            var contest = this.Data.Contests.Find(id);

            if (contest == null)
            {
                return this.HttpNotFound("The selected contest no longer exists");
            }

            if (!contest.IsActive)
            {
                //TODO show reward's given for this contest
                throw new NotImplementedException("This contest is not active");
            }

            var currentUserId = this.User.Identity.GetUserId();
            var isInContest = contest.Participants.Any(p => p.Id == currentUserId) ||
                                contest.OrganizatorId == currentUserId;

            this.ViewBag.IsRegisterForContest = isInContest;
            this.ViewBag.isOrganizator = contest.OrganizatorId == currentUserId;

            var contestViewModel = Mapper.Map<PreviewContestViewModel>(contest);

            return this.View(contestViewModel);
        }

        [Authorize]
        [HttpGet]
        public ActionResult ManageContest(int id)
        {
            var contest = this.Data.Contests.Find(id);

            if (contest == null)
            {
                return this.HttpNotFound("The selected contest does not exist");
            }

            var loggedUserId = this.User.Identity.GetUserId();

            if (contest.OrganizatorId != loggedUserId)
            {
                this.Response.StatusCode = 401;
                return this.Content("Logged user is not the contest organizator");
            }

            var contestBindingModel = Mapper.Map<UpdateContestBindingModel>(contest);

            return this.View("ManageContestForm", contestBindingModel);
        }

        [Authorize]
        [HttpPatch]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateContest(UpdateContestBindingModel model)
        {
            if (model == null)
            {
                this.Response.StatusCode = 400;
                return this.Content("Missing Data");
            }

            if (!this.ModelState.IsValid)
            {
                this.Response.StatusCode = 400;
                return this.Json(this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }

            var contest = this.Data.Contests.Find(model.Id);

            if (contest == null)
            {
                return this.HttpNotFound("The selected contest does not exist");
            }

            var loggedUserId = this.User.Identity.GetUserId();

            if (contest.OrganizatorId != loggedUserId)
            {
                this.Response.StatusCode = 401;
                return this.Content("Logged user is not the contest organizator");
            }

            if (!string.IsNullOrWhiteSpace(model.Title))
            {
                contest.Title = model.Title;
            }

            if (!string.IsNullOrWhiteSpace(model.Description))
            {
                contest.Description = model.Description;
            }

            if (model.EndDate != default(DateTime))
            {
                contest.EndDate = model.EndDate;
            }

            this.Data.Contests.Update(contest);
            this.Data.SaveChanges();

            this.Response.StatusCode = 200;
            return this.Content("Contest updated successfully");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InviteUser(string username, int contestId, InvitationType type)
        {
            var contest = this.Data.Contests.Find(contestId);

            if (contest == null)
            {
                this.Response.StatusCode = 404;
                return this.Content(string.Format("Contest with id {0} not found", contestId));
            }

            if (!contest.IsOpenForSubmissions)
            {
                this.Response.StatusCode = 400;
                return this.Content("The contest is closed for submissions/registrations.");
            }

            var userToInvite = this.Data.Users.All().FirstOrDefault(u => u.UserName == username);

            if (userToInvite == null)
            {
                this.Response.StatusCode = 404;
                return this.Content(string.Format("User with username {0} not found", username));
            }

            var loggedUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (userToInvite.UserName == loggedUser.UserName)
            {
                this.Response.StatusCode = 400;
                return this.Content("Users cannot invite themselves.");
            }

            if (userToInvite.PendingInvitations.Any(i => i.ContestId == contestId && i.Type == type))
            {
                this.Response.StatusCode = 400;
                return this.Content("User is already invited and has not confirmed.");
            }

            var invitation = new Invitation
                                    {
                                        ContestId = contestId,
                                        InviterId = loggedUser.Id,
                                        InvitedId = userToInvite.Id,
                                        DateOfInvitation = DateTime.Now,
                                        Type = type,
                                        Status = InvitationStatus.Neutral
                                    };

            userToInvite.PendingInvitations.Add(invitation);
            loggedUser.SendedInvitations.Add(invitation);
            this.Data.SaveChanges();

            var notification = new NotificationViewModel
                                  {
                                      Sender = loggedUser.UserName,
                                      Type = type.ToString()
                                  };

            this.HubContext.Clients.User(username).notificationReceived(this.RenderViewToString("_Notification", notification));

            this.Response.StatusCode = 200;

            return this.Content(string.Format("User with username {0} successfully invited", username));
        }

        private ActionResult ValidateImageData(HttpPostedFileBase file)
        {
            if (!file.ContentType.Contains("image"))
            {
                this.Response.StatusCode = 400;
                return this.Json(new { ErrorMessage = "The file is not a picture" });
            }

            if (file.ContentLength > MaxImageSize)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { ErrorMessage = "Picture size must be in range [1 - 1024 kb]" });
            }

            return null;
        }

        private string GetBase64String(HttpPostedFileBase file)
        {
            byte[] fileBuffer = new byte[file.ContentLength];
            file.InputStream.Read(fileBuffer, 0, file.ContentLength);

            string type = "data:image/" + file.ContentType + ";base64,";

            return type + Convert.ToBase64String(fileBuffer);
        }

        public string RenderViewToString(string viewName, object model)
        {
            this.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(this.ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(this.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}