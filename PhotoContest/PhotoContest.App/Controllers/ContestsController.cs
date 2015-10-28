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

        [HttpGet]
        public ActionResult AllContests()
        {
            var allContests =
                this.Data.Contests.All()
                    .OrderByDescending(c => c.StartDate)
                    .Project()
                    .To<ContestViewModel>()
                    .ToList();

            
            this.ApplyRights(allContests);

            return this.PartialView("_AllContestsPartial", allContests);
        }

        private void ApplyRights(IList<ContestViewModel> contests)
        {
            if (this.User.Identity.GetUserId() != null)
            {
                var user = this.Data.Users.Find(this.User.Identity.GetUserId());

                for (int i = 0; i < contests.Count(); i++)
                {
                    if (user.Id == contests[i].OrganizatorId)
                    {
                        contests[i].CanManage = true;
                        continue;
                    }

                    if (contests[i].ParticipationStrategyType != ParticipationStrategyType.Closed 
                        && !user.InContests.Any(c => c.Id == contests[i].Id) 
                        && !user.CommitteeInContests.Any(c => c.Id == contests[i].Id))
                    {
                        contests[i].CanParticipate = true;
                    }
                }
            }
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
                .OrderByDescending(c => c.StartDate)
                .Project()
                .To<ContestViewModel>()
                .ToList();

            this.ApplyRights(activeContests);

            return this.PartialView("_ActiveContestsPartial", activeContests);
        }

        [HttpGet]
        public ActionResult InactiveContests()
        {
            var activeContests = this.Data.Contests.All()
                  .Where(c => c.IsActive == false)
                  .OrderByDescending(c => c.StartDate)
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
                .OrderByDescending(c => c.StartDate)
                .Project()
                .To<ContestViewModel>()
                .ToList();

            return this.PartialView("_MyContestsPartial", myContests);
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

            if (model.EndDate < DateTime.Now)
            {
                this.Response.StatusCode = 400;
                return this.Content("Contest end date cannot be before today`s date");
            }

            if (this.Data.RewardStrategies.Find(model.RewardStrategyId) == null)
            {
                throw new ArgumentException("Not existing reward strategy");
            }

            if (this.Data.ParticipationStrategies.Find(model.ParticipationStrategyId) == null)
            {
                throw new ArgumentException("Not existing participation strategy");
            }

            if (this.Data.VotingStrategies.Find(model.VotingStrategyId) == null)
            {
                throw new ArgumentException("Not existing voting strategy");
            }

            if (this.Data.DeadlineStrategies.Find(model.DeadlineStrategyId) == null)
            {
                throw new ArgumentException("Not existing deadline strategy");
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
                if (contest.OrganizatorId == user.Id)
                {
                    throw new InvalidOperationException("You cannot join contest created by you");
                }

                this.DeadlineStrategy =
                    StrategyFactory.GetDeadlineStrategy(contest.DeadlineStrategy.DeadlineStrategyType);

                this.DeadlineStrategy.CheckDeadline(this.Data, contest, user);

                this.ParticipationStrategy =
                    StrategyFactory.GetParticipationStrategy(contest.ParticipationStrategy.ParticipationStrategyType);

                this.ParticipationStrategy.CheckPermission(this.Data, user, contest);

                if (contest.Participants.Contains(user))
                {
                    throw new ArgumentException("You already participate in this contest");
                }

                if (contest.Committee.Contains(user))
                {
                    throw new ArgumentException("You cannot participate in this contest, you are in the committee");
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
            }

            this.TempData["Messages"] = messages;

            if (messages.Count > 0)
            {
                this.Response.StatusCode = 400;
                return this.Content(string.Join("\n", messages));
            }

            return new HttpStatusCodeResult(200);
        }

        [Authorize]
        [HttpGet]
        public ActionResult JoinCommittee(int id)
        {
            var contest = this.Data.Contests.Find(id);

            if (!contest.IsActive)
            {
                this.Response.StatusCode = 400;
                return this.Content("The contest is not active");
            }

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            var invitation = contest.Organizator.SendedInvitations.FirstOrDefault(i => i.ContestId == contest.Id && i.InvitedId == user.Id && i.Type == InvitationType.Committee);

            if (invitation == null)
            {
                this.Response.StatusCode = 400;
                return this.Content("You don't have an invitation");
            }

            if (invitation.Status != InvitationStatus.Neutral)
            {
                this.Response.StatusCode = 400;
                return this.Content("You already have responded to the invitation");
            }

            invitation.Status = InvitationStatus.Accepted;
            contest.Committee.Add(user);

            this.Data.SaveChanges();

            return new HttpStatusCodeResult(200);
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

                this.DeadlineStrategy.CheckDeadline(this.Data, contest, user);

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
                var contestWinners = 
                    this.Data.ContestWinners.All()
                    .Where(c => c.ContestId == contest.Id)
                    .Select(c => new ContestWinnerViewModel
                    {
                        Id = c.ContestId,
                        ContestTitle = c.Contest.Title,
                        ContestDescription = c.Contest.Description,
                        Place = c.Place,
                        Winner = c.Winner.UserName
                    })
                    .ToList();

                return this.View("PreviewInactiveContest", contestWinners);
            }
            
            var currentUserId = this.User.Identity.GetUserId();
            var isInContest = contest.Participants.Any(p => p.Id == currentUserId) ||
                                contest.OrganizatorId == currentUserId || contest.Committee.Any(u => u.Id == currentUserId);

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

            var loggedUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (contest.OrganizatorId != loggedUser.Id)
            {
                this.Response.StatusCode = 400;
                return this.Content("Only the contest organizator can invite users.");
            }

            if (type == InvitationType.Committee 
                && contest.VotingStrategy.VotingStrategyType != VotingStrategyType.Closed)
            {
                this.Response.StatusCode = 400;
                return this.Content("The contest voting strategy type is not 'CLOSED'.");
            }

            if (type == InvitationType.ClosedContest 
                && contest.ParticipationStrategy.ParticipationStrategyType != ParticipationStrategyType.Closed)
            {
                this.Response.StatusCode = 400;
                return this.Content("The contest participation strategy type is not 'CLOSED'.");
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

            if (userToInvite.UserName == loggedUser.UserName)
            {
                this.Response.StatusCode = 400;
                return this.Content("Users cannot invite themselves.");
            }

            if (userToInvite.PendingInvitations.Any(i => i.ContestId == contestId && i.Type == type))
            {
                this.Response.StatusCode = 400;
                return this.Content(string.Format("User is already invited to contest with id {0}", contest.Id));
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

            if (type == InvitationType.ClosedContest)
            {
                contest.InvitedUsers.Add(userToInvite);
            }

            userToInvite.PendingInvitations.Add(invitation);
            loggedUser.SendedInvitations.Add(invitation);

            this.Data.SaveChanges();

            var notification = new NotificationViewModel
                                  {
                                      InvitationId = invitation.Id,
                                      Sender = loggedUser.UserName,
                                      Type = type.ToString()
                                  };

            this.HubContext.Clients.User(username).notificationReceived(this.RenderViewToString("_Notification", notification));

            this.Response.StatusCode = 200;

            return this.Content(string.Format("User with username {0} successfully invited", username));
        }

        [HttpPost]
        public ActionResult FinalizeContest(int id)
        {
            var contest = this.Data.Contests.Find(id);

            if (contest == null)
            {
                this.Response.StatusCode = 404;
                return this.Content(string.Format("Contest with id {0} not found", id));
            }

            if (contest.IsActive == false)
            {
                this.Response.StatusCode = 404;
                return this.Content(string.Format("Contest with id {0} is not active", id));
            }

            var loggedUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (contest.OrganizatorId != loggedUser.Id)
            {
                this.Response.StatusCode = 400;
                return this.Content("Only the contest organizator can finalize it.");
            }

            contest.IsOpenForSubmissions = false;
            contest.IsActive = false;
            contest.EndDate = DateTime.Now;

            this.Data.SaveChanges();

            this.RewardStrategy =
                    StrategyFactory.GetRewardStrategy(contest.RewardStrategy.RewardStrategyType);

            this.RewardStrategy.ApplyReward(this.Data, contest);


            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        public ActionResult DismissContest(int id)
        {
            var contest = this.Data.Contests.Find(id);

            if (contest == null)
            {
                this.Response.StatusCode = 404;
                return this.Content(string.Format("Contest with id {0} not found", id));
            }

            if (contest.IsActive == false)
            {
                this.Response.StatusCode = 404;
                return this.Content(string.Format("Contest with id {0} is not active", id));
            }

            var loggedUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (contest.OrganizatorId != loggedUser.Id)
            {
                this.Response.StatusCode = 400;
                return this.Content("Only the contest organizator can dismiss it.");
            }

            contest.IsOpenForSubmissions = false;
            contest.IsActive = false;
            contest.EndDate = DateTime.Now;

            this.Data.SaveChanges();

            return new HttpStatusCodeResult(200);
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