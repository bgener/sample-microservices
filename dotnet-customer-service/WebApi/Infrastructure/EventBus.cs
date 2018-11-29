using System;
using Autofac;
using Domain.Interfaces;

namespace WebApi.Infrastructure
{
    public class EventBus : IBus
    {
        private static readonly Type HandlerType = typeof(IHandleEvent<>);
        private readonly ILifetimeScope _lifetimeScope;


        public EventBus(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }


        public void Publish(IEvent @event)
        {
            var handlerType = HandlerType.MakeGenericType(@event.GetType());
            dynamic handler = _lifetimeScope.ResolveOptional(handlerType);
            if (handler == null)
            {
                throw new InvalidOperationException("Event handler was not found" + handlerType.FullName);
            }

            handler.Handle(@event);
        }
    }
}
