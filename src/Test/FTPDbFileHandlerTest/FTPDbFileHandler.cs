using System.IO;
using System.Threading.Tasks;
using CORE.Domain.Entities;
using FTPPersistence.Repository;
using Shouldly;
using Xunit;

namespace FTPDbFileHandlerTest
{
    public class FtpDbFileHandler
    {
        private readonly DbFileHandler _dbFileHandler;
        private readonly string _pathOFile;
        private readonly string _pathOfDirectory;
        public FtpDbFileHandler()
        {
            _dbFileHandler = new DbFileHandler();
            _pathOFile = _dbFileHandler.PathDbFile;
            _pathOfDirectory = _dbFileHandler.fullRouteOfDirectory;
        }

        [Fact]
        public void Should_CreateRouteFile()
        {
            ////Arrange

            //Act

            //Assert
            var output = ValidateAndCleanFile();
            output.ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Add_Success()
        {
            //Arrange
            var credential = new Credential() { UserName = "TESTUser", HostName = "TESTHost", Password = ">TESTPass", Port = 21 };

            //Act
            var response = await _dbFileHandler.Add(credential);

            //Assert
            response.Error.ShouldBeNullOrEmpty();
            response.Status.ShouldBeTrue();

            var output = ValidateAndCleanFile();
            output.ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Add_Failer()
        {
            //Arrange

            var credential = new Credential();

            //Act

            var response = await _dbFileHandler.Add(credential);

            //Assert
            response.Error.ShouldNotBeNullOrEmpty();
            response.Data.ShouldBeFalse();
            response.Status.ShouldBeFalse();

        }

        private bool ValidateAndCleanFile()
        {
            if (!File.Exists(_pathOFile))
                return false;

            File.Delete(_pathOFile);
            Directory.Delete(_pathOfDirectory);
            var output = File.Exists(_pathOFile) || Directory.Exists(_pathOfDirectory);

            return !output;
        }
    }
}