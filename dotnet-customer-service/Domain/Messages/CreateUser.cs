using Domain.Interfaces;

namespace Domain.Messages
{
    public class CreateUser : ICommand
    {
        public string UserName { get; set; }
        public long UserGroupId { get; set; }
    }
}
