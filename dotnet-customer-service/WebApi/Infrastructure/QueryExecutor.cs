using System;
using Autofac;
using Domain.Interfaces;

namespace WebApi.Infrastructure
{
    public interface IQueryExecutor
    {
        IQueryResult Execute<TQuery>(TQuery query)
            where TQuery : IQuery;
    }

    public class QueryExecutor : IQueryExecutor
    {
        private static readonly Type HandlerType = typeof(IHandleQuery<>);
        private readonly ILifetimeScope _lifetimeScope;


        public QueryExecutor(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }


        public IQueryResult Execute<TQuery>(TQuery query)
            where TQuery : IQuery
        {
            var handlerType = HandlerType.MakeGenericType(query.GetType());
            dynamic handler = _lifetimeScope.ResolveOptional(handlerType);
            if (handler == null)
            {
                throw new InvalidOperationException("Query handler was not found: " + handlerType.FullName);
            }

            var queryResult = handler.Handle(query);
            return queryResult;
        }
    }
}
