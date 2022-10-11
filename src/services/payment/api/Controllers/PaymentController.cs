using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [Route("All")]
        [HttpGet]
        public async Task<IActionResult> GetOrderPayments()
        {
            return Ok(await this._mediator.Send(new application.GetOrderPayments.Query()));
        }
    }
}
