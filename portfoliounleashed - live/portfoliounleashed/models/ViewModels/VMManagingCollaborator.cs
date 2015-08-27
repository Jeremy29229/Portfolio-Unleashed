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
        public bool IsProjectEditor { get; set; }
        public string Title { get; set; }
        public string DescriptionShort { get; set; }
        public int ProjectId { get; set; }
        public VMManagingCollaborator(VMContribution contribution, bool isProjectEditor)
        {
            if (!string.IsNullOrEmpty(contribution.Name))
            {
                UserName = contribution.Name;
            }
            IsProjectEditor = isProjectEditor;
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