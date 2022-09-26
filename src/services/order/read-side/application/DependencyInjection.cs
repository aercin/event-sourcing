using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ReportProductStatistic).Assembly);
            services.AddAutoMapper(typeof(ReportProductStatistic).Assembly);

            return services;
        }
    }
}
