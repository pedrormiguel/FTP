using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FTPLib.Class.Common;
using FTPLib.Class.Dto;
using FTPLib.Class.Entities;

namespace FTPPersistence.Repository
{
    public class DbFileHandler
    {
        private readonly string _pathDbFile;
        private const string NameFile = "DB.txt";
        private const string TempFile = "TEMPFILE.txt";

        public DbFileHandler()
        {
            this._pathDbFile = GetPathFile();
        }
        private string GetPathFile()
        {
            var sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var sFile = Path.Combine(sCurrentDirectory, $"../../../DB/File/txt/{NameFile}");
            return Path.GetFullPath(sFile);
        }

        private string GetPath()
        {
            var sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var sFile = Path.Combine(sCurrentDirectory, $"../../../DB/File/txt/");
            return Path.GetFullPath(sFile);
        }

        public async Task<Response<bool>> Add(BaseEntity credentials)
        {
            var response = new Response<bool>();

            try
            {
                await using (var file = new StreamWriter(_pathDbFile, append: true) )
                {
                     await file.WriteLineAsync(credentials.ToString());
                }
                
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
                var lines =  await Task.Run( () => File.ReadAllLines(_pathDbFile));
                response.Status = true;
                response.Data   = lines;
            }
            catch (Exception ex)
            {
                response.ErrorMapException(ex);

            }

            return response;
        }

        public async Task<Response<string>> Delete(BaseEntity credentials)
        {
            var response = new Response<string>();
            var path = $"{GetPath()}/{TempFile}";

            try
            {

                using (var sr = new StreamReader(_pathDbFile))
                {
                    var sw = new StreamWriter(path);

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

                File.Replace(path, _pathDbFile, null);
            }
            catch (Exception ex)
            {
                response.ErrorMapException(ex);
            }

            return response;
        }

        public async Task<Response<string>> Update(BaseEntity credentials)
        {
            var response = new Response<string>();
            var path = $"{GetPath()}/{TempFile}";

            try
            {
                using (var sr = new StreamReader(_pathDbFile))
                {
                    var sw = new StreamWriter(path);

                    string line;
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        var idLine = DtoConnectionSever.Map(line).Id.ToString();

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
                File.Replace(path, _pathDbFile, null);
            }
            catch (Exception ex)
            {
                response.ErrorMapException(ex);
            }

            return response;
        }
    }
}