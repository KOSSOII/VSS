using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using WebmBot.App_Start;

namespace WebmBot
{
    public class Global : HttpApplication
    {   
        void Application_Start(object sender, EventArgs e)
        {
            
            // Код, выполняемый при запуске приложения
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.Add(new XmlMediaTypeFormatter());

        }

        public void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {

            //BlockedIpHandler biph = new BlockedIpHandler();
            //if (biph.IsIpBlocked(HttpContext.Current.Request.UserHostAddress))
            //{
            //    Server.Transfer("~/Rep.aspx");
            //}

        }


    }
}