using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMContactInfo
    {
        public VMContactInfo(ContactInfo c)
        {
            Title = c.Title;
            Information = c.Information;
            Id = c.Id;
            UserId = c.UserId;
        }
        public VMContactInfo()
        {
            
        }

        [Display(Name = "Contact Type")]
        public string Title { get; set; }
        [Display(Name = "Contact Information")]
        public string Information { get; set; }
        public int Id { get; private set; }
        public int UserId { get; private set; }
    }
}