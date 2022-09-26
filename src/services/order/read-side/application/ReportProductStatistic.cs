using AutoMapper;
using core_application.Abstractions;
using domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace application
{
    public static class ReportProductStatistic
    {
        #region Command 
        public class Command : IRequest<ProductStatisticResponse>
        {

        }
        #endregion

        #region Command Handler
        public class CommandHandler : IRequestHandler<Command, ProductStatisticResponse>
        {
            private readonly IRedisRepository<ProductStatistic> _redisRepository;
            private readonly IConfiguration _configuration;
            private readonly IMapper _mapper;

            public CommandHandler(IRedisRepository<ProductStatistic> redisRepository, IConfiguration configuration, IMapper mapper)
            {
                this._redisRepository = redisRepository;
                this._configuration = configuration;
                this._mapper = mapper;
            }

            public async Task<ProductStatisticResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                if (!await this._redisRepository.IsKeyExistAsync(this._configuration.GetValue<string>("Redis:Table")))
                    throw new ApplicationException("Herhangi bir ürün istatistik bilgisi bulunmamaktadır");

                var storedProductStatistic = await this._redisRepository.GetAsync<ProductStatistic>(this._configuration.GetValue<string>("Redis:Table"));

                return this._mapper.Map<ProductStatisticResponse>(storedProductStatistic); 
            }
        }
        #endregion

        #region Response
        public class ProductStatisticResponse
        {
            public List<ProductHistory> ProductHistories { get; set; }
        }

        public class ProductHistory
        {
            public int ProductId { get; set; }

            public List<ProductDetail> ProductDetails { get; set; }
        }

        public class ProductDetail
        {
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }
        #endregion  
    }
}
