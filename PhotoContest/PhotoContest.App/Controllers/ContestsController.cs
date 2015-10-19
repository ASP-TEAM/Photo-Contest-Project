﻿namespace PhotoContest.App.Controllers
{
    using System;
    using System.Net;
    using System.Web;
    using PhotoContest.App.Models.ViewModels;
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
    using PhotoContest.App.Models.ViewModels.Picture;

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
                                  IsOpenForSubmissions = model.IsOpenForSubmissions,
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
            
            HttpPostedFileBase file = this.Request.Files[0];

            var result = this.ValidateImageData(file);

            if (result != null)
            {
                return result;
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

                var base64PictureString = GetBase64String(file);

                Picture picture = new Picture
                {
                    UserId = user.Id,
                    Url = base64PictureString,
                    ContestId = contest.Id
                };

                this.Data.Pictures.Add(picture);
                this.Data.SaveChanges();

                var viewModel = new PictureViewModel()
                {
                    User = user.UserName,
                    Url = picture.Url
                };

                this.Response.StatusCode = 200;

                return PartialView("_PicturePartial", viewModel);
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

            var contestViewModel =
                contest.Pictures.AsQueryable()
                       .Project()
                       .To<FullPictureViewModel>()
                       .ToList();

            return this.View(contestViewModel);
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

            return Convert.ToBase64String(fileBuffer);
        }
    }
}