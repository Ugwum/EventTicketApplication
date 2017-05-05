using System;

namespace EventTicket.Service.Interface
{
    public interface ITicketService
    {
        string ReserveTicket(string eventId, int tktQty);

        string PurchaseTicket(Guid reservationId, string eventId);
    }
}