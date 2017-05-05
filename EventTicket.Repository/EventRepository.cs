using System;
using System.Linq;
using EventTicket.Entity;
using EventTicket.Repository.Interface;
using Library.Repository.Pattern.DataContext;
using Library.Repository.Pattern.EntityFramework;
using Library.Repository.Pattern.UnitOfWork;


namespace EventTicket.Repository
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(IDataContextAsync context, IUnitOfWorkAsync unitOfWork) : base(context, unitOfWork)
        {
        }
 
    }
}
