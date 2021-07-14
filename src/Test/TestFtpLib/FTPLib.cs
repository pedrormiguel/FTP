using System.Threading.Tasks;
using FTPLib;
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
            var isConnect = _client.Connect();
            //Assert
            isConnect.ShouldBeTrue();
        }

        [Fact]
        public void Should_Connect_Failure()
        {
            //Arrange
            var client = new Ftp("", "", "");

            //Act
            var isConnect = client.Connect();

            //Assert
            isConnect.ShouldBeFalse();
        }

        [Fact]
        public void Should_ListItems_Successful()
        {
            //Arrange

            //Act
            var line = _client.ListItems();

            //Assert
        }
    }
}