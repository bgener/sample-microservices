using Domain.Interfaces;
using Domain.Models;

namespace Domain.Messages
{
    public class UserCreated : IEvent
    {
        public long UserId { get; set; }
        public UserGroup UserGroup { get; set; }
        public string UserName { get; set; }
    }
}
