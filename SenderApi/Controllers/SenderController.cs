using Microsoft.AspNetCore.Mvc;
using SenderApi.Services;

namespace SenderApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SenderController : Controller
    {
        private readonly IMessageService _messageService;
        public SenderController(IMessageService messageService)
        {
            _messageService=messageService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            _messageService.Enqueue("deneme1");
            return Ok();
        }
    }
}
