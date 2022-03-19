using ApiMongoDB.Config;
using ApiMongoDB.Entities;
using MongoDB.Driver;

namespace ApiMongoDB.Repository
{
    public interface IMongoRepository<T>
    {
        List<T> Get();
        T Get(string id);
        T GetBySlug(string slug);
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

        public List<T> Get() => _model.Find(model => model.Deleted == false).ToList();

        public T GetBySlug(string slug)
        {
            return _model.Find(model => model.Slug == slug && model.Deleted == false).FirstOrDefault();
        }

        public T Get(string id) =>
            _model.Find(model => model.Id == id && model.Deleted == false).FirstOrDefault();

        public T Create(T model)
        {
            _model.InsertOne(model);
            return model;
        }

        public void Update(string id, T model) => _model.ReplaceOne(entity => entity.Id == id, model);

        public void Remove(string id)
        {
            var model = Get(id);
            model.Deleted = true;

            _model.ReplaceOne(entity => entity.Id == id, model);
        }       
    }
}
