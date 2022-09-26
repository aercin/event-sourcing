using core_domain.Abstractions;
using domain.Entities;

namespace domain.Abstractions
{
    public interface IStockRepository : IGenericRepository<Stock>
    {
        Stock GetStock();
    }
}
