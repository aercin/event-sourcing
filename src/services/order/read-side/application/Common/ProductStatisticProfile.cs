using AutoMapper;
using domain.Entities;

namespace application.Common
{
    public class ProductStatisticProfile : Profile
    {
        public ProductStatisticProfile()
        {
            CreateMap<ProductStatistic, ReportProductStatistic.ProductStatisticResponse>();
            CreateMap<ProductHistory, ReportProductStatistic.ProductHistory>();
            CreateMap<ProductDetail, ReportProductStatistic.ProductDetail>();
        }
    }
}
