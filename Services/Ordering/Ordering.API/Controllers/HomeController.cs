using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.Controllers;

public class HomeController : Controller
{
    //Get : /<controller>/

    public IActionResult Index()
    {
        return new RedirectResult("~/swagger");
    }
}