using core_domain.Abstractions;
using core_domain.Enums;
using core_message;
using core_message.Common;
using core_messages;
using domain.Abstractions;
using domain.Entities;
using domain.Enums;
using domain.Events;

namespace infrastructure.Services
{
    public class OrderAggregateProjection : IOrderAggregateProjection
    {
        public void ProjectEventToAggregate(DomainEventBase domainEvent, OrderAggregate aggregate)
        {
            if (domainEvent.GetType() == typeof(OrderPlaced))
            {
                var orderPlacedEvent = (OrderPlaced)domainEvent;

                aggregate.AggregateId = orderPlacedEvent.OrderNo;
                aggregate.OrderNo = orderPlacedEvent.OrderNo;
                aggregate.CustomerId = orderPlacedEvent.CustomerId;
                aggregate.Status = OrderStatus.Suspend;
                aggregate.OrderDate = DateTime.Now;
                aggregate.OrderProducts.AddRange(orderPlacedEvent.OrderProducts);

                if (orderPlacedEvent.State == DomainEventState.Added)
                {
                    aggregate.AddIntegrationEvent(new IE_OrderPlaced
                    {
                        OrderNo = aggregate.OrderNo,
                        Items = orderPlacedEvent.OrderProducts.Select(op => new OrderItem
                        {
                            ProductId = op.ProductId,
                            Quantity = op.Quantity,
                            UnitPrice = op.UnitPrice
                        }).ToList()
                    });
                }
            }
            else if (domainEvent.GetType() == typeof(ProductAdded))
            {
                var productAddedEvent = (ProductAdded)domainEvent;

                if (aggregate.OrderProducts.Any(x => x.ProductId == productAddedEvent.AddedProduct.ProductId))
                {
                    var existedOrderProduct = aggregate.OrderProducts.Single(x => x.ProductId == productAddedEvent.AddedProduct.ProductId);
                    existedOrderProduct.IncreaseQuantityWith(productAddedEvent.AddedProduct.Quantity);
                    existedOrderProduct.ApplyANewUnitPrice(productAddedEvent.AddedProduct.UnitPrice);
                }
                else
                {
                    aggregate.OrderProducts.Add(OrderProduct.AddNewOne(productAddedEvent.AddedProduct.ProductId, productAddedEvent.AddedProduct.Quantity, productAddedEvent.AddedProduct.UnitPrice));
                }

                if (productAddedEvent.State == DomainEventState.Added)
                {
                    aggregate.AddIntegrationEvent(new IE_ProductAdded
                    {
                        OrderNo = aggregate.OrderNo,
                        Item = new OrderItem
                        {
                            ProductId = productAddedEvent.AddedProduct.ProductId,
                            Quantity = productAddedEvent.AddedProduct.Quantity,
                            UnitPrice = productAddedEvent.AddedProduct.UnitPrice
                        }
                    });
                }
            }
            else if (domainEvent.GetType() == typeof(ProductRemoved))
            {
                var productRemovedEvent = (ProductRemoved)domainEvent;

                var existedOrderProduct = aggregate.OrderProducts.Single(x => x.ProductId == productRemovedEvent.ProductId);

                existedOrderProduct.DecreaseQuantityWith(productRemovedEvent.Quantity);

                if (productRemovedEvent.State == DomainEventState.Added)
                {
                    aggregate.AddIntegrationEvent(new IE_ProductRemoved
                    {
                        OrderNo = aggregate.OrderNo,
                        ProductId = productRemovedEvent.ProductId,
                        Quantity = productRemovedEvent.Quantity
                    });
                }
            }
            else if (domainEvent.GetType() == typeof(OrderFailed))
            {
                var orderFailedEvent = (OrderFailed)domainEvent;

                aggregate.Status = OrderStatus.Failed;

                if (orderFailedEvent.State == DomainEventState.Added)
                {
                    aggregate.AddIntegrationEvent(new IE_OrderFailed
                    {
                        OrderNo = aggregate.OrderNo
                    });
                }
            }
            else if (domainEvent.GetType() == typeof(OrderSuccessed))
            {
                var orderSuccessedEvent = (OrderSuccessed)domainEvent;

                aggregate.Status = OrderStatus.Successed;

                if (orderSuccessedEvent.State == DomainEventState.Added)
                {
                    aggregate.AddIntegrationEvent(new IE_OrderSuccessed
                    {
                        OrderNo = aggregate.OrderNo,
                        Items = aggregate.OrderProducts.Select(op => new OrderItem
                        {
                            ProductId = op.ProductId,
                            Quantity = op.Quantity,
                            UnitPrice = op.UnitPrice
                        }).ToList()
                    });
                }
            }
        }
    }
}
