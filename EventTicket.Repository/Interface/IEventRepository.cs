using System;
using System.Linq;
using EventTicket.Entity;
using Library.Repository.Pattern.Repositories;

namespace EventTicket.Repository.Interface
{
    public interface IEventRepository : IRepositoryAsync<Event>
    {

    }
}
