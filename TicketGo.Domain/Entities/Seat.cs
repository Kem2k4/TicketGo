using System;
using System.Collections.Generic;

namespace TicketGo.Domain.Entities
{
    public partial class Seat
    {
        public Seat()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int? IdSeat { get; set; }
        public string NameSeat { get; set; }
        public bool State { get; set; }
        public int IdCoach { get; set; }

        public virtual Coach IdCoachNavigation { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
