namespace ApiMongoDB.Services.Interfaces
{
    public interface ICacheService
    {
        T Get<T>(string key);

        void Set<T>(string key, T content);

        void Remove(string key);
    }
}
