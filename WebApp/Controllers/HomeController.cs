using System.Collections.Generic;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string host = "")
        {
            NsResponse res = new NsResponse("", null, null, null, null, null, null, null, null, null, null, null);
            if (!string.IsNullOrWhiteSpace(host))
            {
                res = NsQuery.Query(host, new List<string> { "a", "aaaa", "mx", "ns", "ptr", "soa", "srv", "txt", "title", "keywords", "description" });
            }
            return View(res);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}
