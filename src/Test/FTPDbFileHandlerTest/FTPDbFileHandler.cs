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
        public void Should_CreateRouteFile_WhenInstantiateTheDbFileHandler()
        {
            ////Arrange

            //Act

            //Assert ( it's using the intance of the ctor )
            ValidateAndCleanFile().ShouldBeTrue();
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
            response.Success.ShouldBeTrue();
            ValidateAndCleanFile().ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Add_Fail_WhenCredentialIsNotValid()
        {
            //Arrange

            var credential = new Credential() { Port = 0 };

            //Act

            var response = await _dbFileHandler.Add(credential);

            //Assert
            response.Error.ShouldBeNullOrEmpty();
            response.Data.ShouldBeFalse();
            response.Success.ShouldBeFalse();
            response.ValidationErrors.Count.Equals(6);
            
            ValidateAndCleanFile().ShouldBeTrue();
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