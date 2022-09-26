namespace api.Models
{
    public class RemoveProductRequest
    {
        public Guid OrderNo { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
