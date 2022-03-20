using ApiMongoDB.Config;
using ApiMongoDB.Entities.Enums;
using ApiMongoDB.Services;
using FluentAssertions;
using Xunit;

namespace ApiMongoDB.Tests.Services
{
    public class UploadServiceTests
    {
        [Theory]
        [InlineData(Media.Image, "image.webp")]
        [InlineData(Media.Video, "video.mp4")]
        public void Should_verify_if_Type_is_Image_or_Video(Media media, string filename)
        {
            //Arrange
            var service = new UploadService();

            //Act
            var result = service.GetTypeMedia(filename);

            //Assert
            Assert.Equal(media, result);
        }


        [Theory]
        [InlineData(Media.Image, "image.psd")]
        [InlineData(Media.Video, "video.mp3")]
        public void Should_verify_if_Type_isent_Image_or_Video(Media media, string filename)
        {
            //Arrange
            var service = new UploadService();

            //Act
            var act = () => service.GetTypeMedia(filename);

            //Assert
            act.Should().ThrowExactly<DomainException>().WithMessage("Formato errado de arquivo!");
        }
    }
}
