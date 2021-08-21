namespace FTPLib.Class.Dto
{
    public class DtoItem
    {
        public string FullName { get; set; }
        public string OwnerPermissions { get; set; }
        public long Size { get; set; }

        public static DtoItem Map(string fullName, string ownerPermissions, long size)
        {
            return new DtoItem() { FullName = fullName, Size = size, OwnerPermissions = ownerPermissions };
        }
    }
}