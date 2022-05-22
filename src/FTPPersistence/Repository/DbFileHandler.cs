using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CORE.Domain.Common;
using CORE.Domain.Entities;
using CORE.Domain.Validation;
using FTPLib.Class.Common;
using FTPPersistence.Interfaces;
using DtoConnectionSever = CORE.Domain.Common.DtoConnectionSever;

namespace FTPPersistence.Repository
{
    public class DbFileHandler : IDbFile
    {
        private string PathDbFile { get; }
        private string FullRouteOfDirectory { get; }
        private const string NameFile = "DB.txt";
        private const string TempFile = "TEMPFILE.txt";
        private readonly string _routeOfDirectory = "../../../DB/File/txt/";
        private readonly string _routeOfFile;

        public DbFileHandler()
        {
            var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            FullRouteOfDirectory = Path.Combine(currentDirectory, _routeOfDirectory);
            _routeOfFile = $"{FullRouteOfDirectory}{NameFile}";

            if (!File.Exists(_routeOfFile))
            {
                Directory.CreateDirectory(FullRouteOfDirectory);
                File.Create(_routeOfFile).DisposeAsync();
            }

            PathDbFile = GetPathFile();
        }

        public string GetPathFile()
        {
            var sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var sFile = Path.Combine(sCurrentDirectory, _routeOfFile);
            return Path.GetFullPath(sFile);
        }

        public string GetPathDirectory() => FullRouteOfDirectory;

        public async Task<Response<bool>> Add(BaseEntity credentials)
        {
            var response = new Response<bool>();
            var credential = (Credential)credentials;
            var validator = new CredentialValidator();
            var isValid = await validator.ValidateAsync(credential);

            if (!isValid.IsValid)
            {
                foreach (var item in isValid.Errors)
                {
                    response.ValidationErrors.Add(item.ErrorMessage);
                }

                response.Success = isValid.IsValid;
                response.Data = false;

                return response;
            }

            try
            {
                await using (var file = new StreamWriter(PathDbFile, append: true))
                {
                    await file.WriteLineAsync(credentials.ToString());
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMapException(ex);
            }

            return response;
        }

        public async Task<Response<IEnumerable<string>>> ReadAll()
        {
            var response = new Response<IEnumerable<string>>();

            try
            {
                var lines = await Task.Run(() => File.ReadAllLines(PathDbFile));
                response.Success = true;
                response.Data = lines;
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
            var tempPah = $"{FullRouteOfDirectory}{TempFile}";

            try
            {
                using (var sr = new StreamReader(PathDbFile))
                {
                    await using var sw = new StreamWriter(tempPah, true);

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

                response.Success = true;

                File.Replace(tempPah, PathDbFile, null);
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
            var path = $"{FullRouteOfDirectory}{TempFile}";
            var idReceived = DtoConnectionSever.Map(credentials.ToString()).Id.ToString();
            var credential = (Credential)credentials;
            var validator = new CredentialValidator();
            var validation = await validator.ValidateAsync(credential);


            if (validation.IsValid)
            {
                try
                {
                    using (var sr = new StreamReader(PathDbFile))
                    {
                        await using var sw = new StreamWriter(path, true);

                        string line;
                        while ((line = await sr.ReadLineAsync()) != null)
                        {
                            var idLine = DtoConnectionSever.Map(line).Id.ToString();

                            if (!idLine.Equals(idReceived))
                            {
                                await sw.WriteLineAsync(line);
                            }
                            else
                            {
                                var newLine = credentials.ToString();
                                await sw.WriteLineAsync(newLine);
                                response.Data = newLine;
                            }
                        }
                    }

                    response.Success = true;
                    File.Replace(path, PathDbFile, null);
                }
                catch (Exception ex)
                {
                    response.ErrorMapException(ex);
                }
            }
            else
            {
                foreach (var item in validation.Errors)
                {
                    response.ValidationErrors.Add(item.ErrorMessage);
                }

                response.Success = validation.IsValid;
                response.Data = credentials.ToString();
            }

            return response;
        }

        public async Task<Response<Credential>> GetById(Guid id)
        {
            var response = new Response<Credential>();

            try
            {
                 foreach (var line in await File.ReadAllLinesAsync(PathDbFile))
                {
                    var currentCredential = DtoConnectionSever.Map(line);

                    if (currentCredential.Id != id) continue;
                    
                    response.Data = currentCredential;
                    break;
                }
                
                if( !(response.Data is null) )
                    response.Success = true;
            }
            catch (Exception ex)
            {
                response.ErrorMapException(ex);
            }
            
            return response;
        }
    }
}