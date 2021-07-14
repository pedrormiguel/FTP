using System.Threading.Tasks;
using FTPLib;
using FTPLib.Class;
using Xunit;
using Shouldly;

namespace TestFtpLib
{
    public class FtpLib
    {
        private readonly Ftp _client;

        public FtpLib()
        {
            _client = new Ftp(host: "ftp://ftp.dlptest.com", user: "dlpuser", password: "rNrKYTX9g7z3RgJRmxWuGHbeu",
                port: 21);
        }

        [Fact]
        public void Should_Connect_Successful()
        {
            //Arrange
            //Act
            var response = _client.Connect();
            //Assert
            response.Status.ShouldBeTrue();
        }

        [Fact]
        public void Should_Connect_Failure()
        {
            //Arrange
            var client = new Ftp("", "", "");

            //Act
            var response = client.Connect();

            //Assert
            response.Status.ShouldBeFalse();
        }

        [Fact]
        public async Task Should_ListItems_Successful()
        {
            //Arrange

            //Act
            var response = await _client.GetListItems();

            //Assert
        }
    }
}