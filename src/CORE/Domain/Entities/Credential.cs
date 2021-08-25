using CORE.Domain.Common;

namespace CORE.Domain.Entities
{
    public class Credential : BaseEntity
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; } = 21;

        public override string ToString()
        {
            return $"{Id};{HostName};{UserName};{Password};{Port};";
        }
    }
}