using Microsoft.AspNetCore.Mvc;

namespace ComicSystem.Controllers
{
    public class HomeController : Controller
    {
        // Index action which is the default action for HomeController
        public IActionResult Index()
        {
            // You can add some data to pass to the view
            ViewData["Message"] = "Welcome to the Comic System!";
            return View(); // Returns the corresponding View (Index.cshtml)
        }

        // You can add additional actions like Error or other pages if needed
        public IActionResult Error()
        {
            return View(); // Returns the Error view
        }
    }
}
