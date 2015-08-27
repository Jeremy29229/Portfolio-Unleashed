using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortfolioUnleashed.Models.ViewModels
{
    public class VMLink
    {
        public VMLink(Link link)
        {
            Title = link.DisplayText;
            Url = link.URL;
            Id = link.Id;
            UserId = link.UserId;
        }
        public VMLink()
        {
            
        }

        [Display(Name = "Display Text")]
        public string Title { get; set; }
        [Display(Name = "URL")]
        public string Url { get; set; }
        public int Id { get; private set; }
        public int UserId { get; private set; }
    }
}