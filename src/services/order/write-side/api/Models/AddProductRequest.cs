using domain.Dtos;

namespace api.Models
{
    public class AddProductRequest
    {
        public Guid OrderNo { get; set; }
        public OrderProductDto Product { get; set; }
    }
}
