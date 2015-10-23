namespace PhotoContest.App.Controllers
{
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
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using PhotoContest.App.Models.ViewModels.Contest;

    public class ContestsController : BaseController
    {
        private const int MaxImageSize = 1000000;

        public ContestsController(IPhotoContestData data)
            : base(data)
        {
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

            return this.View(activeContests);
        }

        [HttpGet]
        public ActionResult MyContests()
        {
            var loggedUserId = this.User.Identity.GetUserId();

            var myContests = this.Data.Contests.All()
                .Where(c => c.OrganizatorId == loggedUserId)
                .Project()
                .To<ContestViewModel>()
                .ToList();


            return this.View(myContests);
        }

        [HttpGet]
        public ActionResult NewContest()
        {
            return this.View("NewContestForm");
        }

        [HttpPost]
        [Authorize]
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

        [HttpGet]
        public ActionResult Join(int id)
        {
            var currentUser = this.User.Identity.GetUserId();
            var user = this.Data.Users.Find(currentUser);
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
            }

            this.TempData["Messages"] = messages;

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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

                foreach(var file in files)
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

                //var viewModel = new PictureViewModel()
                //{
                //    User = user.UserName,
                //    Url = picture.Url
                //};

                this.Response.StatusCode = 200;

                return null;
            }
            catch (InvalidOperationException e)
            {
                this.TempData["message"] = e.Message;
            }

            this.Response.StatusCode = 400;
            return this.Json(new { ErrorMessage = this.TempData["message"]});
        }

        [HttpGet]
        public ActionResult PreviewContest(int id)
        {
            var contest = this.Data.Contests.Find(id);
            if (contest == null)
            {
                return this.HttpNotFound("The selected contest no longer exists");
            }

            var currentUserId = this.User.Identity.GetUserId();
            var isInContest = contest.Participants.Any(p => p.Id == currentUserId) ||
                                contest.OrganizatorId == currentUserId;

            this.ViewBag.IsRegisterForContest = isInContest;
            this.ViewBag.ContestId = contest.Id;
            this.ViewBag.isOrganizator = contest.OrganizatorId == currentUserId;

            var contestViewModel = Mapper.Map<PreviewContestViewModel>(contest);

            return this.View(contestViewModel);
        }

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

            var contestBindingModel = new UpdateContestBindingModel
            {
                Id = id,
                Title = contest.Title,
                Description = contest.Description,
                EndDate = contest.EndDate
            };

            return this.View("ManageContestForm", contestBindingModel);
        }

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
            return this.Content("Contest created successfully");
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
    }
}