namespace core_application.Abstractions
{
    public interface IEventKeeper
    {
        public Task StoreEventsAsync();
    }
}
