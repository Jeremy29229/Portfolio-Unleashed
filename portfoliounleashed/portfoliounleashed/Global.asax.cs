using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using WebMatrix.WebData;

namespace PortfolioUnleashed
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WebSecurity.InitializeDatabaseConnection("PortfolioUnleashed", "User", "Id", "Email", true);
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Server.ClearError();
            HttpException hex = ex as HttpException ?? new HttpException(500, ex.Message, ex);

            createCookie(ex, hex.GetHttpCode());

            UrlHelper url = new UrlHelper(Request.RequestContext);
            if (hex.GetHttpCode() == 404)
            {
                string u = url.RouteUrl(new { controller = "Error", action = "Http404" });
                Response.Redirect(u);
            }
            else if (hex.GetHttpCode() == 500)
            {
                Response.Redirect("~/Error/Http500"); 
            }
            else
            {
                Response.Redirect("~/Error/General");
            }
        }

        private void createCookie(Exception hex, int errorCode)
        {
            if (Request.Cookies["ErrorInfo"] != null)
            {
                var cookieOld = HttpContext.Current.Request.Cookies["ErrorInfo"];
                cookieOld.Values.Clear();
                cookieOld.Expires = DateTime.Now.AddHours(1);
                int index = 0;
                for (int i = 0; i < 5 && index <= 400; i++)
                {
                    int temp = hex.StackTrace.IndexOf("\r\n", index + 4);
                    index = (temp<=400)? temp : index;
                }
                cookieOld.Values["Stack"] = index + hex.StackTrace.Substring(0, index).Replace("<", "[").Replace(">", "]");
                cookieOld.Values["OuterMessage"] = hex.Message;
                cookieOld.Values["InnerMessage"] = (hex.Message.Contains("inner exception")) ? ((hex.Message == hex.InnerException.Message)?hex.InnerException.InnerException.Message:hex.InnerException.Message) : null;
                cookieOld.Values["Code"] = "" + errorCode;
                cookieOld.Values["Source"] = hex.Source;
                
                Response.Cookies.Add(cookieOld);
            }
            else
            {
                HttpCookie cookie = new HttpCookie("ErrorInfo");
                int index = 0;
                for (int i = 0; i < 5 && index <= 400; i++)
                {
                    int temp = hex.StackTrace.IndexOf("\r\n", index + 4);
                    index = (temp <= 400) ? temp : index;
                }
                cookie.Values["Stack"] = index + hex.StackTrace.Substring(0, index).Replace("<","[").Replace(">","]");
                cookie.Values["OuterMessage"] = hex.Message;
                cookie.Values["InnerMessage"] = (hex.Message.Contains("inner exception")) ? ((hex.Message == hex.InnerException.Message) ? hex.InnerException.InnerException.Message : hex.InnerException.Message) : null;
                cookie.Values["Code"] = "" + errorCode;
                cookie.Values["Source"] = hex.Source;
                
                cookie.Expires = DateTime.Now.AddHours(1);
                Response.Cookies.Add(cookie);
            }
        }
    }
}