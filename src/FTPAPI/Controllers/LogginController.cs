using CORE.Domain.Entities;
using FTPAPI.Modelos;
using FTPLib.Class;
using FTPLib.Interfaces;
using FTPPersistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FTPAPI.Controllers
{
    [ApiController()]
    [Route("[controller]")]
    public class LogginController : ControllerBase
    {
        public IFtp ftpServices { get; set; }

        private IDbFile dbContext;

        public LogginController(IDbFile _dbContext)
        {
            dbContext = _dbContext;
        }

        [HttpPost("Connection")]
        public async Task<ActionResult> Index([FromBody]Guid Id)
        {
            var result = await ConnectWithFtp(Id);

            return Ok(result);
        }

        [HttpPost("listItems")]
        public async Task<ActionResult> GetListItems([FromBody] Ruta ruta)
        {
            var FTP = await ConnectWithFtp(ruta.Id);

            var listItems = await FTP.GetListItemsFiles(ruta.ruta);

            return Ok(listItems);
        }
    
        [HttpPost("Download")]
        public async Task<IActionResult> DownloadFile([FromBody] RutaFile ruta)
        {
           var FTP = await ConnectWithFtp(ruta.Id);

           var result = await FTP.DownloadFile(ruta.LocalPath,ruta.RemotePath);

            return Ok(result);
        }
    
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFile([FromBody] RutaFile ruta)
        {
            var FTP = await ConnectWithFtp(ruta.Id);

            var result = await FTP.UploadFile(ruta.LocalPath, ruta.RemotePath);

            return Ok(result);
        }

        private async Task<Ftp> ConnectWithFtp(Guid Id)
        {
            var credential = await dbContext.GetById(Id);

            var result = new Ftp(credential.Data);

            result.Connect();

            return result;
        }

    }
}
