namespace PhotoContest.App.Models.BindingModels.Contest
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateContestBindingModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        [UIHint("TextField")]
        public string Title { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        [UIHint("TextField")]
        public string Description { get; set; }

        [Required]
        public int ParticipantsLimit { get; set; }

        public bool IsOpenForSubmissions { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

    }
}