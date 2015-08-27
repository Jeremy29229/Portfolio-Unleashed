using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortfolioUnleashed.Models;
using PortfolioUnleashed.Models.ViewModels;

namespace PortfolioUnleashed.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/
        [AllowAnonymous]
        public ActionResult Http404()
        {
            VMErrorInformation info = new VMErrorInformation();
            if (Request.Cookies["ErrorInfo"] != null)
            {
                HttpCookie c = Request.Cookies["ErrorInfo"];
                string omessage = c.Values["OuterMessage"];
                string imessage = c.Values["InnerMessage"];
                string code = c.Values["Code"];
                string source = c.Values["Source"];
                string stack = c.Values["Stack"];
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
                info = new VMErrorInformation(omessage, imessage, code, source, stack);
            }
            if (TempData["ErrorMessages"] != null)
            {
                ViewBag.ErrorMessages = TempData["ErrorMessages"];
                TempData["ErrorMessages"] = null;
            }
            return View(model: info);
        }

        [AllowAnonymous]
        public ActionResult Http500()
        {
            VMErrorInformation info = new VMErrorInformation();
            if (Request.Cookies["ErrorInfo"] != null)
            {
                HttpCookie c = Request.Cookies["ErrorInfo"];
                string omessage = c.Values["OuterMessage"];
                string imessage = c.Values["InnerMessage"];
                string code = c.Values["Code"];
                string source = c.Values["Source"];
                string stack = c.Values["Stack"];
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
                info = new VMErrorInformation(omessage, imessage, code, source, stack);
            }
            if (TempData["ErrorMessages"] != null)
            {
                ViewBag.ErrorMessages = TempData["ErrorMessages"];
                TempData["ErrorMessages"] = null;
            }
            return View(model:info);
        }

        [AllowAnonymous]
        public ActionResult General()
        {
            VMErrorInformation info = new VMErrorInformation();
            if (Request.Cookies["ErrorInfo"] != null)
            {
                HttpCookie c = Request.Cookies["ErrorInfo"];
                string omessage = c.Values["OuterMessage"];
                string imessage = c.Values["InnerMessage"];
                string code = c.Values["Code"];
                string source = c.Values["Source"];
                string stack = c.Values["Stack"];
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
                info = new VMErrorInformation(omessage, imessage, code, source, stack);
            }
            if (TempData["ErrorMessages"] != null)
            {
                ViewBag.ErrorMessages = TempData["ErrorMessages"];
                TempData["ErrorMessages"] = null;
            }
            return View(model: info);
        }

    }
}
