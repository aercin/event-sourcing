using core_infrastructure.persistence;
using domain.Entities;
using domain.Abstractions;

namespace infrastructure.Persistence
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(PaymentDbContext context) : base(context)
        {
        }
    }
}
