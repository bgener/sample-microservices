using Data.Repositories;
using FluentValidation;
using WebApi.Models;

namespace WebApi.Validation
{
    public class CreateUserViewModelValidator : AbstractValidator<CreateUserViewModel>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IPersonRepository _personRepository;

        public CreateUserViewModelValidator(IPersonRepository personRepository, IGroupRepository groupRepository)
        {
            _personRepository = personRepository;
            _groupRepository = groupRepository;

            Validate();
        }

        private void Validate()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .Must(BeUniqueUserName);

            RuleFor(x => x.GroupId)
                .GreaterThan(0)
                .Must(BeExistingUserGroup);
        }

        private bool BeUniqueUserName(string userName)
        {
            var hasUser = _personRepository.Exists(userName);
            return !hasUser;
        }


        private bool BeExistingUserGroup(int groupId)
        {
            var hasGroup = _groupRepository.Exists(groupId);
            return hasGroup;
        }
    }
}
