using domain.Dtos;

namespace api.Models
{
    public class PlaceOrderRequest
    {
        public Guid CustomerId { get; set; }

        public List<OrderProductDto> Items { get; set; }
    }
}
