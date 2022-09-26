using application.Notifications;
using core_application.Abstractions;
using domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace application
{
    public static class ProjectionOfSuccessedOrderToProductStatistic
    {
        public class NotificationHandler : INotificationHandler<OrderSuccessedNotification>
        {
            private readonly IRedisRepository<ProductStatistic> _redisRepository;
            private readonly IConfiguration _configuration;
            public NotificationHandler(IRedisRepository<ProductStatistic> redisRepository, IConfiguration configuration)
            {
                this._redisRepository = redisRepository;
                this._configuration = configuration;
            }

            public async Task Handle(OrderSuccessedNotification request, CancellationToken cancellationToken)
            {
                ProductStatistic productStatistic;

                if (await this._redisRepository.IsKeyExistAsync(this._configuration.GetValue<string>("Redis:Table")))
                {
                    productStatistic = await this._redisRepository.GetAsync<ProductStatistic>(this._configuration.GetValue<string>("Redis:Table"));

                    foreach (var newItem in request.Items)
                    {
                        var existedProductHistory = productStatistic.ProductHistories.SingleOrDefault(x => x.ProductId == newItem.ProductId);
                        if (existedProductHistory != null)
                        {//Sipariş içerisinde yer alan ürün ile ilgili bir istatistik bilgisi mevcutta bulunmakta
                            var existedProductDetail = existedProductHistory.ProductDetails.SingleOrDefault(x => x.UnitPrice == newItem.UnitPrice);
                            if (existedProductDetail != null)
                            {
                                existedProductDetail.Quantity += newItem.Quantity;
                            }
                            else
                            {
                                existedProductHistory.ProductDetails.Add(new ProductDetail
                                {
                                    Quantity = newItem.Quantity,
                                    UnitPrice = newItem.UnitPrice
                                });
                            }
                        }
                        else
                        {//Sipariş içerisinde yer alan ürün ile ilgili bir istatistik bilgisi şuana kadar bulunmamakta
                            productStatistic.ProductHistories.Add(new ProductHistory
                            {
                                ProductId = newItem.ProductId,
                                ProductDetails = new List<ProductDetail>
                                        {
                                          new ProductDetail
                                          {
                                              UnitPrice = newItem.UnitPrice,
                                              Quantity = newItem.Quantity
                                          }
                                        }
                            });
                        }
                    }
                }
                else
                {
                    productStatistic = new ProductStatistic();
                    foreach (var newItem in request.Items)
                    {
                        productStatistic.ProductHistories.Add(new ProductHistory
                        {
                            ProductId = newItem.ProductId,
                            ProductDetails = new List<ProductDetail>
                                    {
                                        new ProductDetail
                                        {
                                            Quantity = newItem.Quantity,
                                            UnitPrice=newItem.UnitPrice
                                        }
                                    }
                        });
                    }
                }

                await this._redisRepository.SetAsync<ProductStatistic>(this._configuration.GetValue<string>("Redis:Table"), productStatistic);
            }
        }
    }
}