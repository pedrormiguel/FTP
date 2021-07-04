using System;
using System.IO;
using System.Threading.Tasks;
using src.Interfaces;

namespace src.Class
{
    public class DbFileHandler : IPersistence
    {
        private readonly string PATHDBFILE = @"./DB/File/txt/DB.txt";
        public async Task<bool> Add(BaseCredentials credentials)
        {
            try
            {
                 using StreamWriter file = new StreamWriter(PATHDBFILE, append:true);
                 await file.WriteLineAsync(credentials.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.ToString()}");
                return false;
            }

            return true;
        }
        public async Task Delete(BaseCredentials credentials)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        public async Task Read(BaseCredentials credentials)
        {
            throw new System.NotImplementedException();
        }
        public async Task Update(BaseCredentials credentials)
        {
            throw new System.NotImplementedException();
        }
    
    }
}