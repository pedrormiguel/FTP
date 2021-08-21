namespace FTPLib.Class.Entities
{
    public class Credential : BaseEntity
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; } = 21;
    }
}