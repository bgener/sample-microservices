using Data.Entities;
using Data.Repositories;
using Domain.Interfaces;
using Domain.Messages;

namespace Domain.Handlers
{
    internal class UserCreatedHandler : IHandleEvent<UserCreated>
    {
        private readonly IUserProjectionRepository _repository;


        public UserCreatedHandler(IUserProjectionRepository repository)
        {
            _repository = repository;
        }


        public void Handle(UserCreated @event)
        {
            var projection = Convert(@event);
            _repository.Insert(projection);
        }


        private UserProjection Convert(UserCreated @event)
        {
            var entity = new UserProjection
            {
                UserId = @event.UserId,
                UserName = @event.UserName,
                Details = $"User {@event.UserName} assigned to the group {@event.UserGroup.Name}."
            };
            return entity;
        }
    }
}
