using System;
using Autofac;
using Domain.Interfaces;

namespace WebApi.Infrastructure
{
    public interface ICommandProcessor
    {
        void Process<TCommand>(TCommand command);
    }

    public class CommandProcessor : ICommandProcessor
    {
        private static readonly Type HandlerType = typeof(IHandleCommand<>);
        private readonly ILifetimeScope _lifetimeScope;


        public CommandProcessor(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }


        public void Process<TCommand>(TCommand command)
        {
            var handlerType = HandlerType.MakeGenericType(command.GetType());
            dynamic handler = _lifetimeScope.ResolveOptional(handlerType);
            if (handler == null)
            {
                throw new InvalidOperationException("Command handler was not found" + handlerType.FullName);
            }

            handler.Handle(command);
        }
    }
}
