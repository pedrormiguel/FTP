using Microsoft.Win32.SafeHandles;
using System.Net;
using System;
using System.IO;
using System.Threading.Tasks;
using FTPConsole.Class.Dto;
using FTPConsole.Interfaces;
using FTPLib.Class.Common;

namespace ConsoleApp.Class
{
    public class DbFileHandler : IPersistence
    {
        private readonly string PATHDBFILE;
        private readonly string NAMEFILE = "DB.txt";

        public DbFileHandler()
        {
            this.PATHDBFILE = GetPathFile();
        }

        private string GetPathFile()
        {
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFile = System.IO.Path.Combine(sCurrentDirectory, $"../../../DB/File/txt/{NAMEFILE}");
            return Path.GetFullPath(sFile);
        }

        private string GetPath()
        {
            string sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string sFile = System.IO.Path.Combine(sCurrentDirectory, $"../../../DB/File/txt/");
            return Path.GetFullPath(sFile);
        }

        public async Task<Response> Add(BaseCredentials credentials)
        {
            var response = new Response();

            try
            {
                using StreamWriter file = new StreamWriter(PATHDBFILE, append: true);
                await file.WriteLineAsync(credentials.ToString());
                response.Status = true;
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
                response.Status = true;
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
            var response = new Response();
            string line;
            var path = $"{GetPath()}/tempfile.txt";

            try
            {
                using var sw = new StreamWriter(path);

                using (var sr = new StreamReader(PATHDBFILE))
                {
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        if (!line.Contains(credentials.ToString()))
                        {
                            await sw.WriteLineAsync(line);
                        }
                    }
                }

                response.Status = true;

                File.Replace(path, PATHDBFILE, null);
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }

            return response;
        }

        public async Task<Response> Update(BaseCredentials credentials)
        {
            var response = new Response();
            string line;
            var path = $"{GetPath()}/tempfile.txt";

            try
            {
                using var sw = new StreamWriter(path);

                using (var sr = new StreamReader(PATHDBFILE))
                {
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        var idLine = DtoConnectioSever.Map(line).Id.ToString();

                        if (!idLine.Contains(credentials.Id.ToString()))
                        {
                            await sw.WriteLineAsync(line);
                        }
                        else
                        {
                            await sw.WriteAsync(credentials.ToString());
                        }
                    }
                }

                response.Status = true;

                File.Replace(path, PATHDBFILE, null);
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }

            return response;
        }
    }
}