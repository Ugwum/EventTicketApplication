using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventTicket.Model
{
    public class TicketPurchaseModel
    {
        public Guid Id { get; set; }
        public EventModel Event { get; set; }
        public int TicketQuantity { get; set; }
    }
}
