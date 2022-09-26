using core_domain.Abstractions;
using core_domain.Enums;
using domain.Abstractions;
using domain.Dtos;
using domain.Enums;
using domain.Events;
using System.Text.Json.Serialization;

namespace domain.Entities
{
    public class OrderAggregate : ESAggregateRootBase
    {
        public Guid AggregateId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid OrderNo { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }

        private IOrderAggregateProjection OrderAggregateProjection;

        public OrderAggregate(IOrderAggregateProjection orderAggregateProjection)
        {
            this.OrderAggregateProjection = orderAggregateProjection;

            OrderProducts = new List<OrderProduct>();
        }

        private OrderAggregate(Guid customerId, List<OrderProductDto> orderProducts, IOrderAggregateProjection orderAggregateProjection) : this(orderAggregateProjection)
        {
            AddDomainEvent(new OrderPlaced
            {
                OrderNo = Guid.NewGuid(),
                State = DomainEventState.Added,
                CustomerId = customerId,
                OrderProducts = orderProducts.Select(op => OrderProduct.AddNewOne(op.ProductId, op.Quantity, op.UnitPrice)).ToList()
            });
        }

        public static OrderAggregate PlaceOrder(Guid customerId, List<OrderProductDto> orderProducts, IOrderAggregateProjection orderAggregateProjection)
        {
            return new OrderAggregate(customerId, orderProducts, orderAggregateProjection);
        }

        public void AddProduct(OrderProductDto orderProduct)
        {
            if (Status != OrderStatus.Suspend)
            {
                throw new ApplicationException("Henüz beklemede durumunda olan siparişler için içerik düzenlemesi yapılabilir");
            }

            AddDomainEvent(new ProductAdded
            {
                State = DomainEventState.Added,
                AddedProduct = OrderProduct.AddNewOne(orderProduct.ProductId, orderProduct.Quantity, orderProduct.UnitPrice)
            });
        }

        public void RemoveProduct(int productId, int quantity)
        {
            if (Status != OrderStatus.Suspend)
            {
                throw new ApplicationException("Henüz beklemede durumunda olan siparişler için içerik düzenlemesi yapılabilir");
            }

            if (OrderProducts.Where(op => op.ProductId == productId).Sum(op => op.Quantity) < quantity)
            {
                throw new ApplicationException("Sipariş içerisinde yeteri kadar ürün bulunmamaktadır");
            }

            AddDomainEvent(new ProductRemoved
            {
                State = DomainEventState.Added,
                ProductId = productId,
                Quantity = quantity
            });
        }

        public void MarkAsFailed()
        {
            AddDomainEvent(new OrderFailed
            {
                State = DomainEventState.Added
            });
        }

        public void MarkAsSuccessed()
        {
            AddDomainEvent(new OrderSuccessed
            {
                State = DomainEventState.Added
            });
        }

        public override void AddDomainEvent(DomainEventBase domainEvent)
        {
            base.AddDomainEvent(domainEvent);

            this.OrderAggregateProjection.ProjectEventToAggregate(domainEvent, this);
        } 
    }

    public class OrderProduct
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        [JsonConstructor]
        public OrderProduct(int productId, int quantity, decimal unitPrice)
        {
            this.ProductId = productId;
            this.Quantity = quantity;
            this.UnitPrice = unitPrice;
        }

        public static OrderProduct AddNewOne(int productId, int quantity, decimal unitPrice)
        {
            return new OrderProduct(productId, quantity, unitPrice);
        }

        public void IncreaseQuantityWith(int addition)
        {
            this.Quantity += addition;
        }

        public void DecreaseQuantityWith(int exraction)
        {
            this.Quantity -= exraction;
        }

        public void ApplyANewUnitPrice(decimal unitPrice)
        {
            this.UnitPrice = unitPrice;
        }
    }
}
