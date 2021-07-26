
namespace FTPLib.Class.Common
{
    public static class FtpStatusResponse
    {
        public const string Failed  = "The upload or download failed with an error transferring, or the source file did not exist";
        public const string Success = "The upload or download completed successfully";
        public const string Skipped = "The upload or download was skipped because the file already existed on the target";
    }
}