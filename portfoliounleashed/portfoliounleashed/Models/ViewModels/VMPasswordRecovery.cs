using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMPasswordRecovery
    {
        [Required(ErrorMessage = "Input your email so we can send a rest link!")]
        [Display(Name = "Your account email")]
        [EmailAddress(ErrorMessage= "Not a valid email?")]
        public string Email { get; set; }
    }
}