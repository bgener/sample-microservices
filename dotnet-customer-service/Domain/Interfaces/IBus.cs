namespace Domain.Interfaces
{
    public interface IBus
    {
        void Publish(IEvent @event);
    }
}
