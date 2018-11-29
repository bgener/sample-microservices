using System.Linq;
using Data.Entities;
using Data.Repositories;
using Domain.Interfaces;
using Domain.Messages;
using Domain.Models;

namespace Domain.Handlers
{
    public class GetAllUsersHandler : IHandleQuery<GetAllUsers>
    {
        private readonly IPersonRepository _repository;


        public GetAllUsersHandler(IPersonRepository repository)
        {
            _repository = repository;
        }


        public IQueryResult Handle(GetAllUsers query)
        {
            var users = _repository.Get()
                .Select(x => Convert(x));

            return new GetAllUsersResult
            {
                Customers = users.Select(x => x.UserName)
            };
        }


        private User Convert(Person entity)
        {
            var user = new User
            {
                UserName = entity.Name
            };
            return user;
        }
    }
}
