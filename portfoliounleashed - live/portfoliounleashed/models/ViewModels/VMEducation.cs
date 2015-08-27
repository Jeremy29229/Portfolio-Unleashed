using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMEducation
    {
        public VMEducation(Education education)
        {
            School = education.School;
            Degree = education.Degree;
            StartYear = (int)education.StartYear;
            EndYear = (int)education.EndYear;
            Id = education.Id;
            UserId = education.UserId;
        }
        public VMEducation()
        {
            
        }

        [Display(Name = "School")]
        public string School { get; set; }
        [Display(Name = "Degree")]
        public string Degree { get; set; }
        [Display(Name = "Start Year")]
        public int StartYear { get; set; }
        [Display(Name = "End Year")]
        public int EndYear { get; set; }
        public int Id { get; private set; }
        public int UserId { get; private set; }
    }
}