using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CORE.Domain.Entities;
using FTPLib.Class.Dto;
using FTPPersistence.Interfaces;
using FTPPersistence.Repository;
using Shouldly;
using Xunit;

namespace FTPDbFileHandlerTest
{
    public class FtpDbFileHandler
    {
        private readonly IDbFile _dbFileHandler;
        private readonly string _pathOFile;
        private readonly string _pathOfDirectory;

        public FtpDbFileHandler()
        {
            _dbFileHandler = new DbFileHandler();
            _pathOFile = _dbFileHandler.GetPathFile();
            _pathOfDirectory = _dbFileHandler.GetPathDirectory();
        }

        [Fact]
        public void Should_CreateRouteFile_WhenInstantiateTheDbFileHandler()
        {
            ////Arrange

            //Act

            //Assert ( it's using the instance of the ctor )
            _pathOfDirectory.ShouldNotBeNullOrEmpty();
            _pathOFile.ShouldNotBeNullOrEmpty();
            ValidateAndCleanFile().ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Add_Success()
        {
            //Arrange
            var credential = new Credential() { UserName = "TESTUser", HostName = "TESTHost", Password = "TESTPass", Port = 21 };

            //Act
            var response = await _dbFileHandler.Add(credential);

            //Assert
            response.Error.ShouldBeNullOrEmpty();
            response.Success.ShouldBeTrue();
            response.ValidationErrors.Count.ShouldBe(0);

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
            response.ValidationErrors.Count.ShouldBe(5);
            response.ValidationErrors.ElementAt(0).ShouldBe("\'Host Name\' must not be empty.");
            response.ValidationErrors.ElementAt(1).ShouldBe($"{nameof(credential.HostName)} must not be null.");
            response.ValidationErrors.ElementAt(2).ShouldBe("\'User Name\' must not be empty.");
            response.ValidationErrors.ElementAt(3).ShouldBe("\'Password\' must not be empty.");
            response.ValidationErrors.ElementAt(4).ShouldBe("\'Port\' must be greater than \'0\'.");

            ValidateAndCleanFile().ShouldBeTrue();
        }

        [Fact]
        public async Task Should_ReadAll_Successfully()
        {
            // Arrange
            var credentialTest = new Credential() { HostName = "HNT", Password = "PT", Port = 23, UserName = "UNT" };
            var credentialTest2 = new Credential() { HostName = "HNT2", Password = "PT2", Port = 24, UserName = "UNT2" };

            // Act
            await _dbFileHandler.Add(credentialTest);
            await _dbFileHandler.Add(credentialTest2);
            var response = await _dbFileHandler.ReadAll();
            var credentialTestRead = DtoConnectionSever.Map(response.Data.ElementAt(0));
            var credentialTest2Read = DtoConnectionSever.Map(response.Data.ElementAt(1));

            // Assert
            response.Success.ShouldBeTrue();
            response.Error.ShouldBeNullOrEmpty();
            response.Data.Count().ShouldBe(2);
            response.ValidationErrors.Count.ShouldBe(0);

            credentialTestRead.HostName.ShouldBe(credentialTest.HostName);
            credentialTestRead.UserName.ShouldBe(credentialTest.UserName);
            credentialTestRead.Password.ShouldBe(credentialTest.Password);
            credentialTestRead.Port.ShouldBe(credentialTest.Port);

            credentialTest2Read.HostName.ShouldBe(credentialTest2.HostName);
            credentialTest2Read.UserName.ShouldBe(credentialTest2.UserName);
            credentialTest2Read.Password.ShouldBe(credentialTest2.Password);
            credentialTest2Read.Port.ShouldBe(credentialTest2.Port);

            ValidateAndCleanFile().ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Delete_Successfully()
        {
            // Arrange
            var credentialTest = new Credential() { HostName = "HNT", Password = "PT", Port = 23, UserName = "UNT" };
            var credentialTest2 = new Credential() { HostName = "HNT2", Password = "PT2", Port = 24, UserName = "UNT2" };

            // Act
            await _dbFileHandler.Add(credentialTest);
            await _dbFileHandler.Add(credentialTest2);
            var response = await _dbFileHandler.Delete(credentialTest);
            var counter = await _dbFileHandler.ReadAll();

            // Assert
            response.Success.ShouldBeTrue();
            response.Error.ShouldBeNullOrEmpty();
            response.ValidationErrors.Count.ShouldBe(0);
            counter.Data.Count().ShouldBe(1);
            counter.Data.ElementAt(0).ShouldBe(credentialTest2.ToString());

            ValidateAndCleanFile().ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Update_Successfully()
        {
            // Arrange
            var credentialTest = new Credential() { HostName = "HNT", Password = "PT", Port = 23, UserName = "UNT" };
            const string change = "CHANGED";

            // Act
            await _dbFileHandler.Add(credentialTest);
            credentialTest.HostName = change;
            credentialTest.UserName = change;
            credentialTest.Password = change;
            credentialTest.Port = 21;
            var response = await _dbFileHandler.Update(credentialTest);

            // Assert
            response.Success.ShouldBeTrue();
            response.Error.ShouldBeNullOrEmpty();
            response.ValidationErrors.Count.ShouldBe(0);
            response.Data.ShouldBe(credentialTest.ToString());


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