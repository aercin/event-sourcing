using domain.Dtos;
using domain.Entities;

namespace domain.Abstractions
{
    public interface IStockDomainService
    {
        bool IsStockAvailable(Stock stock, List<OrderItem> orderItems);
    }
}
