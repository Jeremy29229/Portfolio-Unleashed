﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PortfolioUnleashedContext : DbContext
    {
        public PortfolioUnleashedContext()
            : base("name=PortfolioUnleashedContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<ContactInfo> ContactInfoes { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<ContributionMedium> ContributionMediums { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<FeaturedPortfolio> FeaturedPortfolios { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectMedium> ProjectMediums { get; set; }
        public DbSet<ProjectPermission> ProjectPermissions { get; set; }
        public DbSet<QuickReference> QuickReferences { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSetting> UserSettings { get; set; }
    }
}
