namespace core_application.Abstractions
{
    public interface IEventListener
    {
        Task ConsumeEvent<T>(T topic, Func<string, Task> callback, CancellationToken cancellationToken) where T : class;
    }
}
