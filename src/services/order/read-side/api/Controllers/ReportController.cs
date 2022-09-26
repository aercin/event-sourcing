using application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("StatisticOfAllProducts")]
        [HttpGet]
        public async Task<IActionResult> GetStatisticOfAllProducts()
        {
            return Ok(await this._mediator.Send(new ReportProductStatistic.Command()));
        }

        [Route("StatisticOfAllOrders")]
        [HttpGet]
        public async Task<IActionResult> GetStatisticOfAllOrders()
        {
            return Ok(await this._mediator.Send(new ReportOrderStatistic.Command()));
        }
    }
}
