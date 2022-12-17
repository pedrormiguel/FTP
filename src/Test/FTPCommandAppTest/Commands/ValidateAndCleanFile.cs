using System.IO;

namespace FTPCommandAppTest.Commands
{
    public static class ValidateAndCleanFile
    {
        public static bool CleanFile(string _pathOFile, string _pathOfDirectory)
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
