//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PortfolioUnleashed
{
    using System;
    using System.Collections.Generic;
    
    public partial class Portfolio
    {
        public Portfolio()
        {
            this.FeaturedPortfolios = new HashSet<FeaturedPortfolio>();
            this.Projects = new HashSet<Project>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Visibility { get; set; }
        public bool IsMainPortfolio { get; set; }
        public string URL { get; set; }
        public int UserId { get; set; }
    
        public virtual User User { get; set; }
        public virtual ICollection<FeaturedPortfolio> FeaturedPortfolios { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
