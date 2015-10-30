namespace PhotoContest.App.Models.ViewModels.Strategy.Deadline
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ByTimeViewModel : AbstractDeadlineStrategyViewModel
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime SubmissionDeadline { get; set; } 
    }
}