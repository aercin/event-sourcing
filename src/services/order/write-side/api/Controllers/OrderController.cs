using api.Models;
using application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost("/Order")]
        public async Task<IActionResult> PlaceOrder(PlaceOrderRequest request)
        {
            return Ok(await this._mediator.Send(new PlaceOrder.Command
            {
                CustomerId = request.CustomerId,
                Items = request.Items
            }));
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProductToOrder(AddProductRequest request)
        {
            return Ok(await this._mediator.Send(new AddProduct.Command
            {
                OrderNo = request.OrderNo,
                Product = request.Product
            }));
        }

        [HttpPost("RemoveProduct")]
        public async Task<IActionResult> RemoveProductFromOrder(RemoveProductRequest request)
        {
            return Ok(await this._mediator.Send(new RemoveProduct.Command
            {
                OrderNo = request.OrderNo,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            }));
        }

    }
}
