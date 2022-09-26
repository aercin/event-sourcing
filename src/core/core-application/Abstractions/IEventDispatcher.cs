namespace core_application.Abstractions
{
    public interface IEventDispatcher
    {
        Task DispatchEvent<T>(T topic, string message) where T : class;
    }
}
