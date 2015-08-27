using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMManagingCollaborator
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public bool CollaboratorIsProjectEditor { get; set; }
        public bool CollaboratorIsProjectMaster { get; set; }
        [Display(Name="Title")]
        public string Title { get; set; }
        [Display(Name = "Description")]
        public string DescriptionShort { get; set; }
        public int ProjectId { get; set; }
        public bool UserIsProjectMaster { get; set; }

        public VMManagingCollaborator(VMContribution contribution, bool collaboratorIsProjectEditor, bool collaboratorIsProjectMaster, bool userIsProjectMaster)
        {
            if (!string.IsNullOrEmpty(contribution.Name))
            {
                UserName = contribution.Name;
            }
            CollaboratorIsProjectEditor = collaboratorIsProjectEditor;
            CollaboratorIsProjectMaster = collaboratorIsProjectMaster;
            UserIsProjectMaster = userIsProjectMaster;
            UserId = contribution.UserId;
            if (!string.IsNullOrEmpty(contribution.Title))
            {
                Title = contribution.Title;
            }
            if (!string.IsNullOrEmpty(contribution.Description))
            {
                int length = (contribution.Description.Length >= 150)? 150: contribution.Description.Length;
                DescriptionShort = contribution.Description.Substring(0, length) + "...";
            }
            ProjectId = contribution.ProjectId;
        }
    }
}