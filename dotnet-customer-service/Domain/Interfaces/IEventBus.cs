namespace Domain.Interfaces
{
    public interface IEventBus
    {
        void Publish(IEvent @event);
    }
}
