using EventTicket.Model;

namespace EventTicket.Service.Interface
{
    public interface IEventService
    {
        EventModel FindBy(string id);

        void Save(EventModel Event);
    }
}
