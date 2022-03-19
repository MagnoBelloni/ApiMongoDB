using ApiMongoDB.Entities.Enums;
using ApiMongoDB.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiMongoDB.Entities
{
    public abstract class BaseEntity
    {
        public BaseEntity(string slugText, Status status)
        {
            PublishDate = DateTime.Now;
            Slug = SlugHelper.GenerateSlug(slugText);
            Status = status;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public bool Deleted { get; set; }
        public string Slug { get; set; }

        [BsonElement("publishDate")]
        public DateTime PublishDate { get; private set; }

        [BsonElement("active")]
        public Status Status { get; private set; }

        public abstract void ValidateEntity();

        public Status ChangeStatus(Status status)
        {
            switch (status)
            {
                case Status.Active:
                    status = Status.Active;
                    break;
                case Status.Inactive:
                    status = Status.Inactive;
                    break;
                case Status.Draft:
                    status = Status.Draft;
                    break;
            }

            return status;
        }
    }
}
