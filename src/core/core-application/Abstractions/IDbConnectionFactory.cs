using System.Data;

namespace core_application.Abstractions
{
    public interface IDbConnectionFactory
    {
        string Context { get; }
        IDbConnection GetOpenConnection();
    }
}
