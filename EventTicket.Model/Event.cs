using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventTicket.Model.Factory;

namespace EventTicket.Model
{
    public class EventModel
    {
        public EventModel()
        {
            ReservedTickets = new List<TicketReservationModel>();
            PurchasedTickets = new List<TicketPurchaseModel>();
        }

        public EventModel Id { get; set; }
        public string Name { get; set; }
        public int Allocation { get; set; }
        public List<TicketPurchaseModel> PurchasedTickets { get; set; }
        public List<TicketReservationModel> ReservedTickets { get; set; }


        //This method calculates the number of tickets available to sell
        //based on the initial number of tickets available to an event minus the number of tickets
        //already sold and the number of tickets that are currently reserved.
        public int AvailableAllocation()
        {
            int salesAndReservations = 0;

            PurchasedTickets.ForEach(t => salesAndReservations += t.TicketQuantity);

            ReservedTickets.FindAll(r => r.StillActive()).ForEach(r => salesAndReservations += r.TicketQuantity);

            return Allocation - salesAndReservations;
        }

        //This method determines if a ticket can be purchased based on the reservation ID.
        public bool CanPurchaseTicketWith(Guid reservationId)
        {
            if (HasReservationWith(reservationId))
                return GetReservationWith(reservationId).StillActive();
            return false;
        }

        //This method creates a TicketPurchase that matches the reserved ticket.
        public TicketPurchaseModel PurchaseTicketWith(Guid reservationId)
        {

            //check if ticket purchase is possible with a certain reservation Id
            if (!CanPurchaseTicketWith(reservationId))
            {
                throw new ApplicationException(
                    DetermineWhyTicketCannotbePurchasedWith(reservationId));
            }

            //get the reservation assigned to the reservation Id
            TicketReservationModel reservation = GetReservationWith(reservationId);

            //Create a new ticket 
            TicketPurchaseModel ticket = TicketPurchaseFactory.CreateTicket(this, reservation.TicketQuantity);

            reservation.HasBeenRedeemed = true;

            PurchasedTickets.Add(ticket);

            return ticket;

        }

        //This method returns a string detailing why a ticket cannot be purchased based on a reservation ID.
        public string DetermineWhyTicketCannotbePurchasedWith(Guid reservationId)
        {
            string reservationIssue = "";
            if (HasReservationWith(reservationId))
            {
                TicketReservationModel reservation = GetReservationWith(reservationId);
                if (reservation.HasExpired())
                    reservationIssue = String.Format("Ticket reservation '{0}' has expired", reservationId.ToString());

                else if (reservation.HasBeenRedeemed)
                {
                    reservationIssue = String.Format("Ticket reservation '{0}' has expired", reservationId.ToString());
                }

            }
            else
                reservationIssue = String.Format("There is no ticket reservation with the Id '{0}'", reservationId.ToString());

            return reservationIssue;
        }


        private void ThrowExceptionWithDetailsOnWhyTicketsCannotBeReserved()
        {
            throw new ApplicationException("There are not tickets available to reserve.");
        }

        //This method returns the TicketReservation that matches the passed in reservation ID.
        private TicketReservationModel GetReservationWith(Guid reservationId)
        {
            
            if(!HasReservationWith(reservationId))
                throw new ApplicationException(String.Format("No reservation ticket with matching Id of '{0}'", 
                reservationId.ToString()));

            return ReservedTickets.FirstOrDefault(t => t.Id == reservationId);
        }

        //This method returns a Boolean that determines whether a TicketReservation exists.
        private bool HasReservationWith(Guid reservationId)
        {
            return ReservedTickets.Exists(t => t.Id == reservationId);
        }


        //This method checks whether there are enough tickets available for reservation.
        public bool CanReserveTicket(int qty)
        {
            return AvailableAllocation() > qty;
        }

        //This method creates a new TicketReservation and adds it to the Events collection
        public TicketReservationModel ReserveTicket(int tktQty)
        {
            if(!CanReserveTicket(tktQty))
                ThrowExceptionWithDetailsOnWhyTicketsCannotBeReserved();

                TicketReservationModel  reservation = TicketReservationFactory.CreateReservation(this, tktQty);

                ReservedTickets.Add(reservation);

            return reservation;
        }
    }
}
