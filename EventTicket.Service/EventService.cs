using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EventTicket.Entity;
using EventTicket.Model;
using EventTicket.Repository;
using EventTicket.Service.Interface;
using Library.Repository.Pattern.UnitOfWork;

namespace EventTicket.Service
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;
        private readonly EventRepository _eventRepository;

        public EventService(IUnitOfWorkAsync unitOfWorkAsync, EventRepository eventRepository)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
            _eventRepository = eventRepository;
            Mapper.CreateMap<Event, EventModel>().AfterMap((src, dest) =>
            {
                
            });
            Mapper.CreateMap<EventModel, Event>().AfterMap((src, dest) =>
            {

            });

        }

        public EventModel FindBy(string id)
        {
            var entity = _eventRepository.Find(id);
            if (entity == null)
            {
                throw new ObjectNotFoundException("Event does not Exist");
            }
            return Mapper.Map<Event, EventModel>(entity);
            

        }

        public void Save(EventModel Event)
        {
            try
            {
                _unitOfWorkAsync.BeginTransaction();
                var eventEntity = _eventRepository.Find(Event.Id);
                RemovePurchasedAndReservedTicketsFrom(eventEntity);
                _eventRepository.Insert(Mapper.Map<EventModel,Event>(Event));
                InsertPurchasedTicketsFrom(Event);
                InsertReservedTicketsFrom(Event);
                _eventRepository.SaveChanges();
            }
            catch (Exception)
            {
                _unitOfWorkAsync.Rollback();
                throw;
            }
           
        }

        private void InsertReservedTicketsFrom(EventModel model)
        {
            var ticketReservationRepo = _eventRepository.GetRepository<ReservedTicket>();
            foreach (var ticketReservationModel in model.ReservedTickets)
            {
                ticketReservationRepo.Insert(Mapper.Map<TicketReservationModel,ReservedTicket>(ticketReservationModel));
            }
            ticketReservationRepo.SaveChanges();
        }

        private void InsertPurchasedTicketsFrom(EventModel model)
        {
            var ticketPurchaseRepo = _eventRepository.GetRepository<PurchaseTicket>();
            foreach (var ticketPurchaseModel in model.PurchasedTickets)
            {
                ticketPurchaseRepo.Insert(Mapper.Map<TicketPurchaseModel, PurchaseTicket>(ticketPurchaseModel));
            }
            ticketPurchaseRepo.SaveChanges();
        }

        private void RemovePurchasedAndReservedTicketsFrom(Event entity)
        {
             
            var ticketPurchaseRepo = _eventRepository.GetRepository<PurchaseTicket>();
            var ticketReservationRepo = _eventRepository.GetRepository<ReservedTicket>();
            ticketPurchaseRepo.DeleteRange(entity.PurchaseTickets);
            ticketReservationRepo.DeleteRange(entity.ReservedTickets);
            ticketPurchaseRepo.SaveChanges();
            ticketReservationRepo.SaveChanges();


        }
    }
}
