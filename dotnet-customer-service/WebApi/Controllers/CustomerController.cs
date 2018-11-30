using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using Domain.Messages;
using FluentValidation.Results;
using WebApi.Infrastructure;
using WebApi.Models;
using WebApi.Validation;

namespace WebApi.Controllers
{
    public class CustomerController : ApiController
    {
        private readonly IViewModelValidator _validator;
        private readonly IQueryExecutor _queryExecutor;
        private readonly ICommandProcessor _commandProcessor;


        public CustomerController(IViewModelValidator validator, IQueryExecutor queryExecutor, ICommandProcessor commandProcessor)
        {
            _validator = validator;
            _queryExecutor = queryExecutor;
            _commandProcessor = commandProcessor;
        }


        // GET: api/Customer
        public IEnumerable<string> Get()
        {
            var query = new GetAllUsers();
            var queryResult = _queryExecutor.Execute(query);

            if (queryResult == null)
            {
                //TODO: log error
                return Enumerable.Empty<string>();
            }

            var customers = ((GetAllUsersResult) queryResult).Customers;
            return customers;
        }


        // POST: api/Customer
        public IHttpActionResult Post([FromBody] CreateUserViewModel viewModel)
        {
            var validationResult = _validator.Validate(viewModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult);
            }

            var command = new CreateUser
            {
                UserName = viewModel.UserName,
                UserGroupId = viewModel.GroupId
            };
            _commandProcessor.Process(command);
            return Ok();
        }


        private IHttpActionResult BadRequest(ValidationResult validationResult)
        {
            foreach (var validationFailure in validationResult.Errors)
            {
                ModelState.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
            }
            return BadRequest(ModelState);
        }
    }
}
