namespace PhotoContest.App.Models.BindingModels.Contest
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UpdateContestBindingModel
    {
        [Required]
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 3)]
        [UIHint("TextField")]
        public string Title { get; set; }

        [StringLength(200, MinimumLength = 3)]
        [UIHint("TextField")]
        public string Description { get; set; }

        public DateTime EndDate { get; set; }
    }
}