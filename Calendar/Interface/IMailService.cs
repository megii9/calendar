using Calendar.Domain;

namespace Calendar.Interface
{
    public interface IMailService
    {
        void Send(EventStatus status, Event @event);
    }

    public enum EventStatus
    {
        New, 
        Updated,
        Removed
    }
}
