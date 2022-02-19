using Microsoft.AspNetCore.Mvc;

namespace Receiver1Api.Controllers
{
    public class ReceiverController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
