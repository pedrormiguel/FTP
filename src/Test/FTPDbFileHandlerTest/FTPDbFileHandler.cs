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
        private const string PathOFile = "/Users/pedromiguelruiznunez/Projects/FtpClientConsole/src/Test/FTPDbFileHandlerTest/DB/File/txt/DB.txt";

        public FtpDbFileHandler()
        {
            _dbFileHandler = new DbFileHandler();
        }
        
        [Fact]
        public async Task Should_CreateRouteFile()
        {
            ////Arrange
            //var dbFile = new DbFileHandler();

            //Act

            //Assert
            var output = await ValidateAndCleanFile();
            output.ShouldBeTrue();
        }

        [Fact]
        public async Task Should_Add_Success()
        {
            //Arrange
            //var dbFile = new DbFileHandler();
            var credential = new Credential() { UserName = "TESTUser", HostName = "TESTHost", Password = ">TESTPass", Port = 21 };

            //Act
            var response = await _dbFileHandler.Add(credential);

            //Assert
            response.Error.ShouldBeNullOrEmpty();
            response.Status.ShouldBeTrue();

            var output = await ValidateAndCleanFile();
            output.ShouldBeTrue();
        }

        private async Task<bool> ValidateAndCleanFile()
        {
            bool output = false;

            if (!File.Exists(PathOFile))
                return false;

            await Task.Run(() => File.Delete(PathOFile));
            output = File.Exists(PathOFile);

            return !output;
        }
    }
}