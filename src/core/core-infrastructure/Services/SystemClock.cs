using core_application.Abstractions;

namespace core_infrastructure.Services
{
    public class SystemClock : ISystemClock
    {
        private DateTime _time;

        public  SystemClock()
        {
            _time = DateTime.Now;
        }

        public DateTime Current
        {
            get
            {
                return _time;
            }
        } 
    }
}
