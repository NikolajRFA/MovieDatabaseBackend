using Microsoft.AspNetCore.Mvc;

namespace WebServer.Controllers
{
    public class RatingController : Controller
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
