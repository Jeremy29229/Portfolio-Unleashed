using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMCreatingUser
    {
        public VMCreatingUser(User user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            ContactInfos = new List<VMContactInfo>();
            if (user.ContactInfoes != null && user.ContactInfoes.Count > 0)
            {
                foreach (ContactInfo c in user.ContactInfoes)
                {
                    ContactInfos.Add(new VMContactInfo(c));
                }
            }
            Education = new List<VMEducation>();
            if (user.Educations != null && user.Educations.Count > 0)
            {
                foreach (Education e in user.Educations)
                {
                    Education.Add(new VMEducation(e));
                }
            }
            Links = new List<VMLink>();
            if (user.Links != null && user.Links.Count > 0)
            {
                foreach (Link l in user.Links)
                {
                    Links.Add(new VMLink(l));
                }
            }
            UserId = user.Id;
        }
        public VMCreatingUser()
        {
            ContactInfos = new List<VMContactInfo>();
            Education = new List<VMEducation>();
            Links = new List<VMLink>();
        }

        public int UserId { get; private set; }

        [Required(ErrorMessage = "The First Name field is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The First Name field is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "You must enter a valid email address.")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must enter a valid email address.")]
        [Display(Name = "Confirm Email")]
        [EmailAddress]
        public string ConfirmationEmail { get; set; }

        [Required(ErrorMessage = "You must enter a password.")]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmationPassword { get; set; }

        public List<VMEducation> Education { get; set; }
        public List<VMContactInfo> ContactInfos { get; set; }
        public List<VMLink> Links { get; set; }
    }
}