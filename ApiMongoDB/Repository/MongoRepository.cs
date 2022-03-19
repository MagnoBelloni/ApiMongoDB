using ApiMongoDB.Config;
using ApiMongoDB.Entities;
using MongoDB.Driver;

namespace ApiMongoDB.Repository
{
    public interface IMongoRepository<T>
    {
        Result<T> Get(int page, int quantity);
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

        public Result<T> Get(int page, int quantity)
        {
            var result = new Result<T>();
            result.Page = page;
            result.Quantity = quantity;

            var filter = Builders<T>.Filter.Eq(entity => entity.Deleted, false);

            result.Data = _model.Find(filter)
                .SortByDescending(entity => entity.PublishDate)
                .Skip((page - 1) * quantity)
                .Limit(quantity)
                .ToList();

            result.Total = _model.CountDocuments(filter);
            result.TotalPages = result.Total / quantity;

            return result;
        }

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
