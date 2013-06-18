using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BookingTool
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.CreateRoute(
                "api/{controller}/{id}", new {id = RouteParameter.Optional}, null
                );
        }
    }
}
