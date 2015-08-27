using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortfolioUnleashed.Models.ViewModels;
using PortfolioUnleashed.Data_Layer;
using PortfolioUnleashed.Enums;

namespace PortfolioUnleashed.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        DatabaseDAL DAL = new DatabaseDAL();

        [AllowAnonymous]
        public ActionResult Index(string searchText, string searchType, string orderBy, bool isDescending = false)
        {
            if (searchText == null || searchType == null)
            {
                return View(viewName: "Users");
            }
            else if (searchType.Count() >= 4 && searchType.Substring(0, 4) == "user")
            {
                return Users(searchText, searchType, orderBy, isDescending);
            }
            else if (searchType.Count() >= 9 && searchType.Substring(0, 9) == "portfolio")
            {
                return Portfolios(searchText, searchType, orderBy, isDescending);
            }
            else
            {
                throw new InvalidOperationException(searchType + " does not have a valid search prefix.");
            }
        }

        [AllowAnonymous]
        public ActionResult Users(string searchText, string searchType, string orderBy, bool isDescending = false)
        {
            if (searchText == null || searchType == null)
            {
                return View(viewName: "Users", model: new VMUserSearch { IsDescending = isDescending, OrderBy = "UserFirstName", SearchText = "", SearchType="user_FullName"});
            }
            else if (searchType.Count() >= 4 && searchType.Substring(0, 4) == "user")
            {
                List<User> users = new List<User>();
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    if (searchType == "user_FullName")
                    {
                        string[] searchParts = searchText.Split(' ');

                        if (searchParts.Count() > 1 && (string.IsNullOrWhiteSpace(searchParts[0]) || string.IsNullOrWhiteSpace(searchParts[1])))
                        {
                            searchParts = new string[0];
                        }

                        if (searchParts.Count() > 1)
                        {
                            users = DAL.retrieveAllUsers().Where(u => u.UserSetting.IsPublic && (u.FirstName.ToLower().Contains(searchText.ToLower()) || u.LastName.ToLower().Contains(searchText.ToLower()) || u.FirstName.ToLower().Contains(searchParts[0].ToLower()) || u.LastName.ToLower().Contains(searchParts[0].ToLower()) || u.FirstName.ToLower().Contains(searchParts[1].ToLower()) || u.LastName.ToLower().Contains(searchParts[1].ToLower()))).ToList();
                        }
                        else
                        {
                            users = DAL.retrieveAllUsers().Where(u => u.UserSetting.IsPublic && (u.FirstName.ToLower().Contains(searchText.ToLower()) || u.LastName.ToLower().Contains(searchText.ToLower()))).ToList();
                        }
                    }
                    else if(searchType == "user_FirstName")
                    {
                        users = DAL.retrieveAllUsers().Where(u => u.UserSetting.IsPublic && u.FirstName.ToLower().Contains(searchText.ToLower())).ToList();
                    }
                    else if (searchType == "user_LastName")
                    {
                        users = DAL.retrieveAllUsers().Where(u => u.UserSetting.IsPublic && u.LastName.ToLower().Contains(searchText.ToLower())).ToList();
                    }
                    else if (searchType == "user_Email")
                    {
                        users = DAL.retrieveAllUsers().Where(u => u.UserSetting.IsPublic && (u.Email.ToLower() == searchText.ToLower())).ToList();
                    }
                    else
                    {
                        throw new InvalidOperationException(searchType + " is not a valid searchType for a user");
                    }
                }

                List<VMUserSearchResult> userResults = new List<VMUserSearchResult>();

                foreach (var user in users)
                {
                    userResults.Add(new VMUserSearchResult { UserFirstName = user.FirstName, UserLastName = user.LastName, UserId = user.Id });
                }

                if (orderBy == null || orderBy == "UserFirstName")
                {
                    if (orderBy == null)
                    {
                        orderBy = "UserFirstName";
                    }

                    if(isDescending)
                    {
                        userResults = userResults.OrderByDescending(p => p.UserFirstName).ToList();
                    }
                    else
                    {
                        userResults = userResults.OrderBy(p => p.UserFirstName).ToList();
                    }
                }
                else if(orderBy == "UserLastName")
                {
                    if (isDescending)
                    {
                        userResults = userResults.OrderByDescending(p => p.UserLastName).ToList();
                    }
                    else
                    {
                        userResults = userResults.OrderBy(p => p.UserLastName).ToList();
                    }
                }
                else
                {
                    orderBy = "UserFirstName";
                    if (isDescending)
                    {
                        userResults = userResults.OrderByDescending(p => p.UserFirstName).ToList();
                    }
                    else
                    {
                        userResults = userResults.OrderBy(p => p.UserFirstName).ToList();
                    }
                }

                return View(viewName: "Users", model: new VMUserSearch { Results = userResults, SearchText = searchText, SearchType = searchType, OrderBy = orderBy, IsDescending = isDescending });
            }
            else if (searchType.Count() >= 9 && searchType.Substring(0, 9) == "portfolio")
            {
                return Portfolios(searchText, searchType, orderBy, isDescending);
            }
            else
            {
                throw new InvalidOperationException(searchType + " does not have a valid search prefix.");
            }
        }

        [AllowAnonymous]
        public ActionResult Portfolios(string searchText, string searchType, string orderBy, bool isDescending = false)
        {
            if (searchText == null || searchType == null)
            {
                return View(viewName: "Portfolios", model: new VMPortfolioSearch { IsDescending = isDescending, OrderBy = "PortfolioTitle", SearchText = "", SearchType = "portfolio_Title" });
            }
            else if (searchType.Count() >= 9 && searchType.Substring(0, 9) == "portfolio")
            {
                List<Portfolio> portfolios = new List<Portfolio>();
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    if (searchType == "portfolio_Title")
                    {
                        portfolios = DAL.retrieveAllPortfolios().Where(p => p.Title.ToLower().Contains(searchText.ToLower()) && p.Visibility == (int)VisibilityType.Public).ToList();
                    }
                    else if (searchType == "portfolio_OwnersEmail")
                    {
                        portfolios = DAL.retrieveAllPortfolios().Where(p => p.User.Email.ToLower() == searchText.ToLower() && p.Visibility == (int)VisibilityType.Public).ToList();
                    }
                    else if (searchType == "portfolio_OwnersFullName")
                    {
                        string[] searchParts = searchText.Split(' ');

                        if (searchParts.Count() > 1 && (string.IsNullOrWhiteSpace(searchParts[0]) || string.IsNullOrWhiteSpace(searchParts[1])))
                        {
                            searchParts = new string[0];
                        }

                        if (searchParts.Count() > 1)
                        {
                            portfolios = DAL.retrieveAllPortfolios().Where(u => u.Visibility == (int)VisibilityType.Public && (u.User.FirstName.ToLower().Contains(searchText.ToLower()) || u.User.LastName.ToLower().Contains(searchText.ToLower()) || u.User.FirstName.ToLower().Contains(searchParts[0].ToLower()) || u.User.LastName.ToLower().Contains(searchParts[0].ToLower()) || u.User.FirstName.ToLower().Contains(searchParts[1].ToLower()) || u.User.LastName.ToLower().Contains(searchParts[1].ToLower()))).ToList();
                        }
                        else
                        {
                            portfolios = DAL.retrieveAllPortfolios().Where(u => u.Visibility == (int)VisibilityType.Public && (u.User.FirstName.ToLower().Contains(searchText.ToLower()) || u.User.LastName.ToLower().Contains(searchText.ToLower()))).ToList();
                        }
                    }
                    else if (searchType == "portfolio_OwnersFirstName")
                    {
                        portfolios = DAL.retrieveAllPortfolios().Where(p => p.User.FirstName.ToLower().Contains(searchText.ToLower()) && p.Visibility == (int)VisibilityType.Public).ToList();
                    }
                    else if (searchType == "portfolio_OwnersLastName")
                    {
                        portfolios = DAL.retrieveAllPortfolios().Where(p => p.User.LastName.ToLower().Contains(searchText.ToLower()) && p.Visibility == (int)VisibilityType.Public).ToList();
                    }
                    else
                    {
                        throw new InvalidOperationException(searchType + " is not a valid searchType for a portfolio");
                    }
                }

                List<VMPortfolioSearchResult> portfolioResults = new List<VMPortfolioSearchResult>();

                foreach (var portfolio in portfolios)
                {
                    portfolioResults.Add(new VMPortfolioSearchResult { OwnerId = portfolio.UserId, OwnersFirstName = portfolio.User.FirstName, OwnersLastName = portfolio.User.LastName, PortfolioDescription = portfolio.Description, PortfolioId = portfolio.Id, PortfolioTitle = portfolio.Title, PortfolioURL=portfolio.URL});
                }

                if (orderBy == null || orderBy == "PortfolioTitle")
                {
                    if (orderBy == null)
                    {
                        orderBy = "PortfolioTitle";
                    }

                    if (isDescending)
                    {
                        portfolioResults = portfolioResults.OrderByDescending(p => p.PortfolioTitle).ToList();
                    }
                    else
                    {
                        portfolioResults = portfolioResults.OrderBy(p => p.PortfolioTitle).ToList();
                    }
                }
                else if (orderBy == "PortfolioDescription")
                {
                    if (isDescending)
                    {
                        portfolioResults = portfolioResults.OrderByDescending(p => p.PortfolioDescription).ToList();
                    }
                    else
                    {
                        portfolioResults = portfolioResults.OrderBy(p => p.PortfolioDescription).ToList();
                    }
                }
                else if (orderBy == "PortfolioOwnersFirstName")
                {
                    if (isDescending)
                    {
                        portfolioResults = portfolioResults.OrderByDescending(p => p.OwnersFirstName).ToList();
                    }
                    else
                    {
                        portfolioResults = portfolioResults.OrderBy(p => p.OwnersFirstName).ToList();
                    }
                }
                else if (orderBy == "PortfolioOwnersLastName")
                {
                    if (isDescending)
                    {
                        portfolioResults = portfolioResults.OrderByDescending(p => p.OwnersLastName).ToList();
                    }
                    else
                    {
                        portfolioResults = portfolioResults.OrderBy(p => p.OwnersLastName).ToList();
                    }
                }
                else
                {
                    orderBy = "PortfolioTitle";
           
                    if (isDescending)
                    {
                        portfolioResults = portfolioResults.OrderByDescending(p => p.PortfolioTitle).ToList();
                    }
                    else
                    {
                        portfolioResults = portfolioResults.OrderBy(p => p.PortfolioTitle).ToList();
                    }
                }

                return View(viewName: "Portfolios", model: new VMPortfolioSearch { Results = portfolioResults, SearchText = searchText, SearchType = searchType, OrderBy = orderBy, IsDescending = isDescending });
            }
            else if (searchType.Count() >= 4 && searchType.Substring(0, 4) == "user")
            {
                return Users(searchText, searchType, orderBy, isDescending);
            }
            else
            {
                throw new InvalidOperationException(searchType + " does not have a valid search prefix.");
            }
        }

        [AllowAnonymous]
        public ActionResult Projects(string searchText, string searchType, string orderBy, bool isDescending = false)
        {
            if (searchText == null || searchType == null)
            {
                return View(viewName: "Projects", model: new VMProjectSearch { IsDescending = isDescending, OrderBy = "Portfolio_Title", SearchText = "", SearchType = "project_Title" });
            }
            else if (searchType.Count() >= 7 && searchType.Substring(0, 7) == "project")
            {
                List<Project> projects = new List<Project>();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    if (searchType == "project_Title")
                    {
                        projects = DAL.retrieveAllProjects().Where(p => p.Title.ToLower().Contains(searchText.ToLower()) && p.CollaborationSetting != (int)ContributionSetting.Closed).ToList();
                    }
                    else if (searchType == "project_CollaboratorsEmail")
                    {
                        projects = DAL.retrieveAllProjectsByUserID(DAL.retrieveUserByEmail(searchText).Id).Where(p => p.CollaborationSetting != (int)ContributionSetting.Closed).ToList();
                    }
                    else if (searchType == "project_CollaboratorsFullName")
                    {
                        string[] searchParts = searchText.Split(' ');

                        if (searchParts.Count() > 1 && (string.IsNullOrWhiteSpace(searchParts[0]) || string.IsNullOrWhiteSpace(searchParts[1])))
                        {
                            searchParts = new string[0];
                        }

                        if (searchParts.Count() > 1)
                        {
                            var users = DAL.retrieveAllUsers().Where(u => u.UserSetting.IsPublic && (u.FirstName.ToLower().Contains(searchText.ToLower()) || u.LastName.ToLower().Contains(searchText.ToLower()) || u.FirstName.ToLower().Contains(searchParts[0].ToLower()) || u.LastName.ToLower().Contains(searchParts[0].ToLower()) || u.FirstName.ToLower().Contains(searchParts[1].ToLower()) || u.LastName.ToLower().Contains(searchParts[1].ToLower()))).ToList();

                            List<Contribution> contributions = new List<Contribution>();
                            foreach (var user in users)
                            {
                                contributions.AddRange(DAL.retrieveAllContributions().Where(j => j.UserId == user.Id));
                            }

                            foreach (var contribution in contributions)
                            {
                                if (!projects.Contains(DAL.retrieveProject(contribution.ProjectId)) && DAL.retrieveProject(contribution.ProjectId).CollaborationSetting != (int)ContributionSetting.Closed)
                                {
                                    projects.AddRange(DAL.retrieveAllProjects().Where(p => p.Id == contribution.ProjectId));
                                }
                            }
                        }
                        else
                        {
                            var users = DAL.retrieveAllUsers().Where(u => u.FirstName.ToLower().Contains(searchText.ToLower()) || u.LastName.ToLower().Contains(searchText.ToLower())).ToList();

                            List<Contribution> contributions = new List<Contribution>();
                            foreach (var user in users)
                            {
                                contributions.AddRange(DAL.retrieveAllContributions().Where(j => j.UserId == user.Id));
                            }

                            foreach (var contribution in contributions)
                            {
                                if (!projects.Contains(DAL.retrieveProject(contribution.ProjectId)) && DAL.retrieveProject(contribution.ProjectId).CollaborationSetting != (int)ContributionSetting.Closed)
                                {
                                    projects.AddRange(DAL.retrieveAllProjects().Where(p => p.Id == contribution.ProjectId));
                                }
                            }
                        }
                    }
                    else if (searchType == "project_CollaboratorsFirstName")
                    {
                        var users = DAL.retrieveAllUsers().Where(u => u.FirstName.ToLower().Contains(searchText.ToLower())).ToList();

                        List<Contribution> contributions = new List<Contribution>();
                        foreach (var user in users)
	                    {
		                    contributions.AddRange(DAL.retrieveAllContributions().Where(j => j.UserId == user.Id));
	                    }

                        foreach (var contribution in contributions)
                        {
                            if (!projects.Contains(DAL.retrieveProject(contribution.ProjectId)) && DAL.retrieveProject(contribution.ProjectId).CollaborationSetting != (int)ContributionSetting.Closed)
                            {
                                projects.AddRange(DAL.retrieveAllProjects().Where(p => p.Id == contribution.ProjectId));
                            }
                        }
                    }
                    else if (searchType == "project_CollaboratorsLastName")
                    {
                        var users = DAL.retrieveAllUsers().Where(u => u.LastName.ToLower().Contains(searchText.ToLower())).ToList();

                        List<Contribution> contributions = new List<Contribution>();
                        foreach (var user in users)
                        {
                            contributions.AddRange(DAL.retrieveAllContributions().Where(j => j.UserId == user.Id));
                        }

                        foreach (var contribution in contributions)
                        {
                            if (!projects.Contains(DAL.retrieveProject(contribution.ProjectId)) && DAL.retrieveProject(contribution.ProjectId).CollaborationSetting != (int)ContributionSetting.Closed)
                            {
                                projects.AddRange(DAL.retrieveAllProjects().Where(p => p.Id == contribution.ProjectId));
                            }
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException(searchType + " is not a valid searchType for a project");
                    }
                }

                List<VMProjectSearchResult> projectResults = new List<VMProjectSearchResult>();

                foreach (var project in projects)
                {
                    projectResults.Add(new VMProjectSearchResult { ProjectCollaborationSetting = project.CollaborationSetting, ProjectDescription = project.Description, ProjectId = project.Id, ProjectTitle = project.Title});
                }

                if (orderBy == null || orderBy == "ProjectTitle")
                {
                    if (orderBy == null)
                    {
                        orderBy = "ProjectTitle";
                    }

                    if (isDescending)
                    {
                        projectResults = projectResults.OrderByDescending(p => p.ProjectTitle).ToList();
                    }
                    else
                    {
                        projectResults = projectResults.OrderBy(p => p.ProjectTitle).ToList();
                    }
                }
                else if (orderBy == "ProjectDescription")
                {
                    if (isDescending)
                    {
                        projectResults = projectResults.OrderByDescending(p => p.ProjectDescription).ToList();
                    }
                    else
                    {
                        projectResults = projectResults.OrderBy(p => p.ProjectDescription).ToList();
                    }
                }
                else
                {
                    orderBy = "ProjectTitle";

                    if (isDescending)
                    {
                        projectResults = projectResults.OrderByDescending(p => p.ProjectTitle).ToList();
                    }
                    else
                    {
                        projectResults = projectResults.OrderBy(p => p.ProjectTitle).ToList();
                    }
                }

                return View(viewName: "Projects", model: new VMProjectSearch { Results = projectResults, SearchText = searchText, SearchType = searchType, OrderBy = orderBy, IsDescending = isDescending });
            }
            else
            {
                throw new InvalidOperationException("The Projects action in the SearchController only supports a searchType prefex of \"project\".");
            }
        }

    }
}
