namespace Domain.Models
{
    public class UserGroup
    {
        public UserGroup(long id, bool isDefault = true)
        {
            Id = id;
            IsDefault = isDefault;
        }


        public long Id { get; set; }
        public string Name { get; set; }

        public bool IsDefault { get; set; }
    }
}
