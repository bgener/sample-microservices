using Data.Entities;
using Data.Repositories;
using Domain.Interfaces;
using Domain.Messages;
using Domain.Models;

namespace Domain.Handlers
{
    public class CreateUserHandler : IHandleCommand<CreateUser>
    {
        private readonly IEventBus _bus;
        private readonly IPersonRepository _repository;


        public CreateUserHandler(IEventBus bus, IPersonRepository repository)
        {
            _bus = bus;
            _repository = repository;
        }


        public void Handle(CreateUser command)
        {
            var userGroup = new UserGroup(command.UserGroupId);
            var user = new User
            {
                UserName = command.UserName,
                UserGroup = userGroup
            };

            //TODO: introduce unit of work
            {
                var person = Convert(user);
                var userId = _repository.Insert(person);

                _bus.Publish(new UserCreated
                {
                    UserId = userId,
                    UserName = user.UserName,
                    UserGroup = user.UserGroup
                });
            }
        }


        private Person Convert(User user)
        {
            var entity = new Person
            {
                Name = user.UserName
            };
            return entity;
        }
    }
}
