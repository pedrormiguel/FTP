using System.IO;
using System.Threading.Tasks;
using FTPLib.Class;
using FTPLib.Class.Common;
using Shouldly;
using Xunit;

namespace FTPTestLib
{
    public class FtpLib
    {
        private readonly Ftp _client;

        public FtpLib()
        {
            _client = new Ftp(host: "ftp://ftp.dlptest.com", user: "dlpuser", password: "rNrKYTX9g7z3RgJRmxWuGHbeu", port: 21);
        }

        [Fact]
        public void Should_Connect_Successful()
        {
            //Arrange

            //Act
            var response = _client.Connect();

            //Assert
            response.Success.ShouldBeTrue();

        }

        [Fact]
        public void Should_Connect_Failure()
        {
            //Arrange
            var client = new Ftp("", "", "");

            //Act
            var response = client.Connect();

            //Assert
            response.Success.ShouldBeFalse();
        }

        [Fact]
        public async Task Should_ListItems_Successful()
        {
            //Arrange
            var client = new Ftp(host: "demo.wftpserver.com", "demo", "demo");
            client.Connect();

            //Act
            var response = await client.GetListItems();

            //Assert
            response.Data.Length.ShouldBe(2);
            response.Success.ShouldBeTrue();
            response.Error.ShouldBeNullOrEmpty();
        }

        [Fact]
        public async Task Should_DownloadFile_Successful()
        {
            //Arrange
            var pathDirectory = "/Users/pedro/Projects/FtpClientConsole/src/Test/TestLibrary/download";
            var localPathToDownload = $"{pathDirectory}/test.jpg";
            var remotePathFile = "/download/Winter.jpg";
            var client = new Ftp(host: "demo.wftpserver.com", "demo", "demo");

            //ACT
            client.Connect();
            var response = await client.DownloadFile(localPathToDownload, remotePathFile);

            //Assert
            response.Data.ShouldBe(FtpStatusResponse.Success);
            response.Success.ShouldBeTrue();
            response.Error.ShouldBeNullOrWhiteSpace();
            File.Exists(localPathToDownload).ShouldBeTrue();
            File.Delete(localPathToDownload);
        }

        [Fact]
        public async Task Should_DownloadFile_SkippedFile()
        {
            //Arrange
            var pathDirectory = "/Users/pedro/Projects/FtpClientConsole/src/Test/TestLibrary/download";
            var localPathToDownload = $"{pathDirectory}/Summer.jpg";
            var remotePathFile = "/download/Summer.jpg";
            var client = new Ftp(host: "demo.wftpserver.com", "demo", "demo");

            //Act
            client.Connect();
            var response = await client.DownloadFile(localPathToDownload, remotePathFile);

            //Assert
            response.Data.ShouldBe(FtpStatusResponse.Skipped);
            response.Success.ShouldBeTrue();
            response.Error.ShouldBeNullOrEmpty();
        }

        [Fact]
        public async Task Should_DownloadFile_WithNonExistingFile()
        {
            //Arrange
            var pathDirectory = "/Users/pedro/Projects/FtpClientConsole/src/Test/TestLibrary/download";
            var localPathToDownload = $"{pathDirectory}/test.jpg";
            var remotePathFile = "/download/FileDoesnotExist.jpg";
            var client = new Ftp(host: "demo.wftpserver.com", "demo", "demo");

            //Act 
            client.Connect();
            var response = await client.DownloadFile(localPathToDownload, remotePathFile);

            //Assert
            response.Data.ShouldBe(FtpStatusResponse.Failed);
            response.Success.ShouldBeFalse();
            response.Error.ShouldBeNullOrEmpty();
        }
    }
}