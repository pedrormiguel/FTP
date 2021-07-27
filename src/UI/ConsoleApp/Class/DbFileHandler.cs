using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ConsoleApp.Class;
using FTPConsole.Class.Dto;
using FTPConsole.Interfaces;
using FTPLib.Class.Common;

namespace FTPConsole.Class
{
    public class DbFileHandler : IPersistence
    {
        private readonly string _pathdbfile;
        private const string NameFile = "DB.txt";
        private const string Tempfile = "TEMPFILE.txt";

        public DbFileHandler()
        {
            this._pathdbfile = GetPathFile();
        }
        private string GetPathFile()
        {
            var sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var sFile = System.IO.Path.Combine(sCurrentDirectory, $"../../../DB/File/txt/{NameFile}");
            return Path.GetFullPath(sFile);
        }

        private string GetPath()
        {
            var sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var sFile = System.IO.Path.Combine(sCurrentDirectory, $"../../../DB/File/txt/");
            return Path.GetFullPath(sFile);
        }

        public async Task<Response<bool>> Add(BaseCredentials credentials)
        {
            var response = new Response<bool>();

            try
            {
                await using var file = new StreamWriter(_pathdbfile, append: true);
                await file.WriteLineAsync(credentials.ToString());
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Error = ex.ToString();
            }

            return response;
        }

        public async Task<Response<IEnumerable<string>>> ReadAll()
        {
            var response = new Response<IEnumerable<string>>();

            try
            {
                var lines = await File.ReadAllLinesAsync(_pathdbfile);
                response.Status = true;
                response.Data   = lines;
            }
            catch (Exception ex)
            {
                response.ErrorMapException(ex);

            }

            return response;
        }

        public async Task<Response<string>> Delete(BaseCredentials credentials)
        {
            var response = new Response<string>();
            var path = $"{GetPath()}/{Tempfile}";

            try
            {
                using var sw = new StreamWriter(path);

                using (var sr = new StreamReader(_pathdbfile))
                {
                    string line;
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        if (!line.Contains(credentials.ToString()))
                        {
                            await sw.WriteLineAsync(line);
                        }
                        else 
                        {
                            response.Data = line;
                        }
                    }
                }

                response.Status = true;

                File.Replace(path, _pathdbfile, null);
            }
            catch (Exception ex)
            {
                response.ErrorMapException(ex);
            }

            return response;
        }

        public async Task<Response<string>> Update(BaseCredentials credentials)
        {
            var response = new Response<string>();
            var path = $"{GetPath()}/{Tempfile}";

            try
            {
                using var sw = new StreamWriter(path);

                using (var sr = new StreamReader(_pathdbfile))
                {
                    string line;
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        var idLine = DtoConnectioSever.Map(line).Id.ToString();

                        if (!idLine.Contains(credentials.Id.ToString()))
                        {
                            await sw.WriteLineAsync(line);
                        }
                        else
                        {
                            var newLine = credentials.ToString();
                            await sw.WriteAsync(newLine);
                            response.Data = newLine;
                        }
                    }
                }

                response.Status = true;
                File.Replace(path, _pathdbfile, null);
            }
            catch (Exception ex)
            {
                response.ErrorMapException(ex);
            }

            return response;
        }
    }
}