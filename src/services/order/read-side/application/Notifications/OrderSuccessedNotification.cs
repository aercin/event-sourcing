using application.Common;
using MediatR;

namespace application.Notifications
{
    public class OrderSuccessedNotification : INotification
    {
        public Guid OrderNo { get; set; }
        public List<OrderItem> Items { get; set; }
    } 
}
