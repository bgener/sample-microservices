using System;
using Domain.Interfaces;
using Domain.Messages;
using FakeItEasy;
using FluentAssertions;
using WebApi.Controllers;
using WebApi.Infrastructure;
using WebApi.Validation;
using Xunit;

namespace UnitTests
{
    public class CustomerControllerTests
    {
        [Fact]
        public void When_query_execution_succeeded_it_should_return_customers_collection()
        {
            //Arrange
            var customers = new[] { "CustomerA" };

            var validator = A.Fake<IViewModelValidator>();
            var commandProcessor = A.Fake<ICommandProcessor>();

            var queryExecutor = A.Fake<IQueryExecutor>();
            A.CallTo(() => queryExecutor.Execute(A<GetAllUsers>._)).Returns(new GetAllUsersResult
            {
                Customers = customers
            });

            var controller = new CustomerController(validator, queryExecutor, commandProcessor);

            //Act
            var result = controller.Get();

            //Assert
            result.Should().BeEquivalentTo(customers);
        }

        [Fact]
        public void When_query_execution_failed_it_should_throw_error()
        {
            //Arrange
            var validator = A.Fake<IViewModelValidator>();
            var commandProcessor = A.Fake<ICommandProcessor>();

            var queryExecutor = A.Fake<IQueryExecutor>();
            A.CallTo(() => queryExecutor.Execute(A<IQuery>._)).Throws<Exception>();

            var controller = new CustomerController(validator, queryExecutor, commandProcessor);

            //Act
            Action action = () => controller.Get();

            //Assert
            action.Should().Throw<Exception>();
        }
    }
}
