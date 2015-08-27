using PortfolioUnleashed.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMEditingPortfolio
    {
        [Display(Name = "Portfolio Title")]
        [Required(ErrorMessage="Your portfolio must have a Title.")]
        public string Title { get; set; }

        [Display(Name = "Cover Image (URL)")]
        public string URL { get; set; }

        [Display(Name = "Brief Description")]
        [Required(ErrorMessage="Your portfolio must have a Description.")]
        public string Description { get; set; }

        public List<VMProject> Projects { get; set; }
        [Display(Name = "Portfolio Visibility")]
        public VisibilityType Visibility { get; set; }
        [Display(Name = "Is The Main Account Portfolio")]
        public bool IsMainPortfolio { get; set; }
        public List<VMProject> ProjectCatalog { get; set; }

        public int UserId { get; set; }
        public int Id { get; set; }


        public VMEditingPortfolio(Portfolio portfolio,  int userId, List<Project> projectCatalog = null)
        {
            Title = portfolio.Title;
            URL = portfolio.URL;
            Description = portfolio.Description;
            Projects = new List<VMProject>();
            if (portfolio.Projects != null && portfolio.Projects.Count > 0)
            {
                foreach (Project u in portfolio.Projects)
                {
                    Projects.Add(new VMProject(u, portfolio.UserId));
                }
            }
            ProjectCatalog = new List<VMProject>();
            if (projectCatalog != null && projectCatalog.Count > 0)
            {
                foreach (Project u in projectCatalog)
                {
                    //Projects.Add(new VMProject(u, userId));
                    ProjectCatalog.Add(new VMProject(u, userId));
                }
            }
            Visibility = (VisibilityType)portfolio.Visibility;

            UserId = portfolio.UserId;
            Id = portfolio.Id;
            IsMainPortfolio = portfolio.IsMainPortfolio;
        }
        public VMEditingPortfolio()
        {
            Projects = new List<VMProject>();
            ProjectCatalog = new List<VMProject>();
            Visibility = VisibilityType.Public;
        }
    }
}