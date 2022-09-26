using core_domain.Abstractions;

namespace domain.Entities
{
    public class ProductStatistic : IAggregateRoot
    {
        public List<ProductHistory> ProductHistories { get; set; } = new List<ProductHistory>();
    }

    public class ProductHistory
    {
        public int ProductId { get; set; }

        public List<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();
    }

    public class ProductDetail
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
