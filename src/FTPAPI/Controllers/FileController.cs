using CORE.Domain.Entities;
using FTPLib.Class.Common;
using FTPPersistence.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FTPAPI.Controllers
{
    [Route("[controller]")]
    [ApiController()]
    public class FileController : ControllerBase
    {
        private readonly IDbFile dbContext;

        public FileController(IDbFile dbFile)
        {
            dbContext = dbFile;
        }

        [HttpGet("ListRecords")]
        public async Task<IActionResult> ListAllRecords()
        {
            var result = await dbContext.ReadAll();

            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddRecord([FromBody] Credential credential)
        {
            var result = await dbContext.Add(credential);

            return Ok(result);
        }

        [HttpGet("GetRecordById")]
        public async Task<IActionResult> GetRecordById(Guid id)
        {
            var result = await dbContext.GetById(id);

            return Ok(result);
        }
    
        [HttpDelete("DeleteRecordById")]
        public async Task<IActionResult> DeleteRecordById(Guid Id)
        {
            var credential = await dbContext.GetById(Id);

            var result = await dbContext.Delete(credential.Data);

            return Ok(result);
        }
    }
}
