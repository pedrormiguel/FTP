using System.Net;
using System;
using System.IO;
using System.Threading.Tasks;
using ConsoleApp.Interfaces;

namespace ConsoleApp.Class
{
    public class DbFileHandler : IPersistence
    {
        private readonly string PATHDBFILE;

        public DbFileHandler()
        {
            this.PATHDBFILE = GetPathFile();
        }

        private string GetPathFile()
        {
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFile = System.IO.Path.Combine(sCurrentDirectory, @"../../../DB/File/txt/DB.txt");
            return Path.GetFullPath(sFile);
        }

        public async Task<Response> Add(BaseCredentials credentials)
        {
            var response = new Response();

            try
            {
                using StreamWriter file = new StreamWriter(PATHDBFILE, append: true);
                await file.WriteLineAsync(credentials.ToString());
                response.status = true;
            }
            catch (Exception ex)
            {
                response.Error = ex.ToString();
            }

            return response;
        }

        public async Task<Response> ReadAll()
        {
            var response = new Response();

            try
            {
                var lines = await File.ReadAllLinesAsync(PATHDBFILE);
                response.status = true;
                response.Data = lines;
            }
            catch (Exception ex)
            {
                response.Error = ex.ToString();
            }

            return response;
        }

        public async Task<Response> Delete(BaseCredentials credentials)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Response> Update(BaseCredentials credentials)
        {
            throw new System.NotImplementedException();
        }

    }
}