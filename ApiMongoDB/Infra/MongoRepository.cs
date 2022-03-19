using ApiMongoDB.Entities;
using MongoDB.Driver;

namespace ApiMongoDB.Infra
{
    public interface IMongoRepository<T>
    {
        List<T> Get();
        T Get(string id);
        T Create(T model);
        void Update(string id, T model);
        void Remove(string id);
    }

    public class MongoRepository<T> : IMongoRepository<T> where T : BaseEntity
    {
        private readonly IMongoCollection<T> _model;

        public MongoRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _model = database.GetCollection<T>(typeof(T).Name.ToLower());
        }

        public List<T> Get() => _model.Find(active => true).ToList();

        public T Get(string id) =>
            _model.Find(news => news.Id == id).FirstOrDefault();

        public T Create(T model)
        {
            _model.InsertOne(model);
            return model;
        }

        public void Update(string id, T model) => _model.ReplaceOne(entity => entity.Id == id, model);

        public void Remove(string id) => _model.DeleteOne(entity => entity.Id == id);
    }
}
