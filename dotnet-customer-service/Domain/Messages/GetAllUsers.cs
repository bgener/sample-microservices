using System.Collections.Generic;
using Domain.Interfaces;

namespace Domain.Messages
{
    public class GetAllUsers : IQuery {}

    public class GetAllUsersResult : IQueryResult
    {
        public IEnumerable<string> Customers { get; set; }
    }
}
