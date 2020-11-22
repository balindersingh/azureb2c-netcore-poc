using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AzureB2CDemoApp.Models;
using System.Security.Claims;
using AzureB2CDemoApp.Data;
using System.Net;
using Microsoft.Extensions.Options;

namespace AzureB2CDemoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly CosmosDBOptions _cosmosDBOptions;
        public HomeController(IOptions<CosmosDBOptions> options)
        {
            _cosmosDBOptions = options.Value;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Claims()
        {
            return View();
        }

        [Authorize]
        public IActionResult Desk(string did)
        {
            if (string.IsNullOrEmpty(did))
            {
                ViewData["message"] = "";
                return View();
            }
            string returnMessage = "";
            HttpStatusCode httpCode = (new UserDAO(_cosmosDBOptions)).CreateUser(User, did).GetAwaiter().GetResult();
            if(httpCode == HttpStatusCode.OK || httpCode == HttpStatusCode.Created)
            {
                returnMessage = string.Format("Your request with Desk Id {0} is submitted successfully.", did);
            }
            else
            {
                returnMessage = "Something went wrong. Please contact us if you keep seeing this issue.";
            }
            ViewData["message"] = returnMessage;
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}