using PortfolioUnleashed.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMProjectMedium
    {
        public VMProjectMedium(ProjectMedium medium)//ProjectMedium medium)
        {
            Link = medium.Link;
            MediaType = (MediaType)medium.MediumType;
            Caption = medium.Caption;
            Id = medium.Id;
            ProjectId = medium.ProjectId;
            //ProjectId = medium.ContributionId;
        }
        public VMProjectMedium()
        {

        }

        public int ProjectId { get; private set; }
        [Display(Name = "Link to the media")]
        public string Link { get; set; }
        [Display(Name = "Media Type")]
        public MediaType MediaType { get; set; }
        [Display(Name = "Caption")]
        public string Caption { get; set; }
        public int Id { get; private set; } 
    }
}