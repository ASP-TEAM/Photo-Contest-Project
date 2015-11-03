using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoContest.Infrastructure.Models.ViewModels.Strategy.Deadline
{
    public class ByTimeViewModel : AbstractDeadlineStrategyViewModel
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime SubmissionDeadline { get; set; } 
    }
}