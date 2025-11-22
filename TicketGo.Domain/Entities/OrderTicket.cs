using System;
using System.Collections.Generic;

namespace TicketGo.Domain.Entities
{
    public partial class OrderTicket
    {
        public int Id { get; set; }
        public int IdOrder { get; set; } 
        public int IdTicket { get; set; }

        public virtual Order IdOrderNavigation { get; set; } = null!;
        public virtual Ticket IdTicketNavigation { get; set; } = null!;
    }
}
