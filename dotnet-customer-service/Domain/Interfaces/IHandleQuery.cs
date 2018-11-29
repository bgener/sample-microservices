namespace Domain.Interfaces
{
    public interface IHandleQuery<in TQuery>
        where TQuery : IQuery
    {
        IQueryResult Handle(TQuery query);
    }
}
