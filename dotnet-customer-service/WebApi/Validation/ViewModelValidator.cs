using System;
using Autofac;
using FluentValidation;
using FluentValidation.Results;
using WebApi.Models;

namespace WebApi.Validation
{
    public interface IViewModelValidator
    {
        ValidationResult Validate(CreateUserViewModel viewModel);
    }

    public class ViewModelValidator : IViewModelValidator
    {
        private readonly ILifetimeScope _lifetimeScope;
        private static readonly Type ValidatorType = typeof(IValidator<>);


        public ViewModelValidator(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }


        public ValidationResult Validate(CreateUserViewModel viewModel)
        {
            var validatorType = ValidatorType.MakeGenericType(viewModel.GetType());
            var validator = (IValidator) _lifetimeScope.ResolveOptional(validatorType);
            if (validator == null)
            {
                return new ValidationResult();
            }

            return validator.Validate(viewModel);
        }
    }
}
