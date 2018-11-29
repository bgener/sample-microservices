namespace Domain.Interfaces
{
    public interface IHandleEvent<in T>
        where T : IEvent
    {
        void Handle(T @event);
    }
}