using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebMatrix.WebData;

namespace PortfolioUnleashed
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region HomeRoutes
            routes.MapRoute(
                name: "FAQ",
                url: "FAQ/{*id}",
                defaults: new { controller = "Home", action = "FAQ", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Support",
                url: "Support/{*id}",
                defaults: new { controller = "Home", action = "Support", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "About",
                url: "About/{*id}",
                defaults: new { controller = "Home", action = "About", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Contact",
                url: "Contact/{*id}",
                defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "News",
                url: "News/{*id}",
                defaults: new { controller = "Home", action = "News", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "UserAgreement",
                url: "UserAgreement/{*id}",
                defaults: new { controller = "Home", action = "UserAgreement", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "PrivacyPolicy",
                url: "PrivacyPolicy/{*id}",
                defaults: new { controller = "Home", action = "PrivacyPolicy", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "HowTo",
                url: "HowTo/{*id}",
                defaults: new { controller = "Home", action = "HowTo", id = UrlParameter.Optional }
            );
            #endregion

            #region User Routes
            routes.MapRoute(
                name: "Account",
                url: "Account/{id}",
                defaults: new { controller = "User", action = "Account", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "AccountEdit",
                url: "Account/{id}/Edit",
                defaults: new { controller = "User", action = "AccountEdit", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "AccountCreation",
                url: "CreateAccount",
                defaults: new { controller = "User", action = "AccountCreation", id = UrlParameter.Optional }
            );
            #endregion

            #region Project Routes
            routes.MapRoute(
                name: "ProjectCatalog",
                url: "ProjectCatalog/{id}",
                defaults: new { controller = "Project", action = "ProjectCatalog", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ProjectCreate",
                url: "CreateProject",
                defaults: new { controller = "Project", action = "ProjectCreateEdit" }
            );
            routes.MapRoute(
                name: "ProjectEdit",
                url: "Project/{id}/Edit",
                defaults: new { controller = "Project", action = "ProjectCreateEdit", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CollaboratorManagement",
                url: "Project/{id}/ManageCollaborators",
                defaults: new { controller = "Project", action = "CollaboratorManagement", id = UrlParameter.Optional }
            );
            #endregion

            #region
            routes.MapRoute(
                name: "PortfolioCreateEdit",
                url: "CreatePortfolio",
                defaults: new { controller = "Portfolio", action = "PortfolioCreateEdit", id = UrlParameter.Optional }
            );
            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}