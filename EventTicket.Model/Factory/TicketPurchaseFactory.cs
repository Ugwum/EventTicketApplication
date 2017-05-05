using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicket.Model
{
    public class TicketPurchaseFactory
    {
        public static TicketPurchaseModel CreateTicket(EventModel Event, int tktQty)
        {
            TicketPurchaseModel ticket = new TicketPurchaseModel();
            ticket.Id = Guid.NewGuid();
            ticket.Event = Event;
            ticket.TicketQuantity = tktQty;

            return ticket;
        }

    }
}
