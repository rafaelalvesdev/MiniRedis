using MiniRedis.Common.Model;

namespace MiniRedis.Services.Storage.Interfaces
{
    public interface IDatabase
    {
        GenericResult Save(string key, DatabaseValue value);

        GenericResult<DatabaseValue> Remove(string key);

        GenericResult<DatabaseValue> Get(string key);

        GenericResult<string[]> GetAllKeys();
    }
}
