using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMCollaborationRequest
    {
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectDescriptionBlurb { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

        public VMCollaborationRequest(Project project, int userId, string userName)
        {
            ProjectId = project.Id;
            ProjectTitle = project.Title;
            int length = (project.Description.Length >= 150) ? 150 : project.Description.Length;
            ProjectDescriptionBlurb = project.Description.Substring(0, length) + "...";
            UserId = userId;
            UserName = userName;
        }
    }
}