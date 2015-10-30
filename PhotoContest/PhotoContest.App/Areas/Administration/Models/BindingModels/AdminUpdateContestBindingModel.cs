namespace PhotoContest.App.Areas.Administration.Models.BindingModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using PhotoContest.App.Infrastructure.Mapping;
    using PhotoContest.Models;
    using PhotoContest.Models.Enums;

    public class AdminUpdateContestBindingModel : IMapFrom<Contest>
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [StringLength(300, MinimumLength = 3 )]
        public string Title { get; set; }

        [StringLength(1000, MinimumLength = 3)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public bool IsOpenForSubmissions { get; set; }

        public ParticipationStrategyType ParticipationStrategyType { get; set; }

    }
}