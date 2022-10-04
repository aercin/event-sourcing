using Microsoft.AspNetCore.Mvc;

namespace notificationWebHook.Controllers
{
    [Route("api")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        [HttpPost("notification")] 
        public IActionResult HandleNotification(NotificationRequest request)
        {
            Console.WriteLine(request.message);

            return Ok();
        }

        public class NotificationRequest
        {
            public string message { get; set; }
        }
    }
}
