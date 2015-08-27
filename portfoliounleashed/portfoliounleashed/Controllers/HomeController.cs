using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortfolioUnleashed.Models;
using PortfolioUnleashed.Models.ViewModels;
using PortfolioUnleashed.Enums;
using PortfolioUnleashed.Data_Layer;
using PortfolioUnleashed.DataLayer;
namespace PortfolioUnleashed.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        DatabaseDAL db = new DatabaseDAL();

        [AllowAnonymous]
        public ActionResult Index()
        {
            VMHomePage page = new VMHomePage();
            page.FeaturedPortfolios = new List<VMPortfolio>();
            foreach (Portfolio p in db.retrieveFeaturedPortfolios())
            {
                page.FeaturedPortfolios.Add(new VMPortfolio(p));
            }
            return View(page);
        }
        
        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult FAQ()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Support()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Glossary()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult HowTo()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult UserAgreement()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult News()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}
