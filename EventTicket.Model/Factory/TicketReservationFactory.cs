using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicket.Model.Factory
{
    public class TicketReservationFactory
    {
        public static TicketReservationModel CreateReservation(EventModel Event, int tktQty)
        {
            TicketReservationModel reservation = new TicketReservationModel();
            reservation.Id = Guid.NewGuid();
            reservation.Event = Event;
            reservation.ExpiryTime = DateTime.Now.AddMinutes(1);
            reservation.TicketQuantity = tktQty;

            return reservation;
        }
    }
}
