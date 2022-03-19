using ApiMongoDB.Entities.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiMongoDB.Entities
{
    public class News : BaseEntity
    {
        public News(string hat, string title, string text, string author, string img, Status status) : base(title, status)
        {
            Hat = hat;
            Title = title;
            Text = text;
            Author = author;
            Img = img;

            ValidateEntity();
        }

        [BsonElement("hat")]
        public string Hat { get; private set; }

        [BsonElement("title")]
        public string Title { get; private set; }

        [BsonElement("text")]
        public string Text { get; private set; }

        [BsonElement("author")]
        public string Author { get; private set; }

        [BsonElement("img")]
        public string Img { get; private set; }

        

        public override void ValidateEntity()
        {
            AssertionConcern.AssertArgumentNotEmpty(Title, "O título não pode estar vazio!");
            AssertionConcern.AssertArgumentNotEmpty(Hat, "O chapéu não pode estar vazio!");
            AssertionConcern.AssertArgumentNotEmpty(Text, "O texto não pode estar vazio!");

            AssertionConcern.AssertArgumentLength(Title, 90, "O título deve ter até 90 caracteres!");
            AssertionConcern.AssertArgumentLength(Hat, 40, "O chapéu deve ter até 40 caracteres!");
        }
    }
}
