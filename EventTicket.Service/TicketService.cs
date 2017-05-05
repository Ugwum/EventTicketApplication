using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventTicket.Entity;
using EventTicket.Model;
using EventTicket.Service.Interface;
using Library.Repository.Pattern.UnitOfWork;

namespace EventTicket.Service
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly EventService _eventService;
        public TicketService(IUnitOfWorkAsync unitOfWorkAsync, EventService eventService)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _eventService = eventService;
        }

        public string ReserveTicket(string eventId, int tktQty)
        {
             
                _unitOfWorkAsync.BeginTransaction();
                EventModel Event = _eventService.FindBy(eventId);
                if (!Event.CanReserveTicket(tktQty))
                {
                    _unitOfWorkAsync.Rollback();
                   return String.Format("There are {0} ticket(s) available.", Event.AvailableAllocation());
                  
                }
                Event.ReserveTicket(tktQty);
                _eventService.Save(Event);

                _unitOfWorkAsync.Commit();
                return String.Format("{0} ticket(s) has been reserved for you for the event with Id {1}.", tktQty, Event.Id); ;
           
        }

        public string PurchaseTicket(Guid reservationId, string eventId)
        {
                _unitOfWorkAsync.BeginTransaction();
            EventModel Event = _eventService.FindBy(eventId);
            if (!Event.CanPurchaseTicketWith(reservationId))
            {
                _unitOfWorkAsync.Rollback();
                return Event.DetermineWhyTicketCannotbePurchasedWith(reservationId);
            }
            Event.PurchaseTicketWith(reservationId);
            _eventService.Save(Event);
            _unitOfWorkAsync.Commit();
            return String.Format("Ticket purchase successful for the event with Id {0}.", Event.Id); ;

        }
    }

  
}
