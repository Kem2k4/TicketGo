using System;
using System.Collections.Generic;

namespace TicketGo.Domain.Entities
{
    public partial class Train
    {
        public Train()
        {
            Coaches = new HashSet<Coach>();
            Tickets = new HashSet<Ticket>();
        }

        public int IdTrain { get; set; }
        public string? NameTrain { get; set; } = null!;
        public DateTime? DateStart { get; set; }
        public int IdTrainRoute { get; set; }
        public decimal? CoefficientTrain { get; set; }

        public virtual TrainRoute IdTrainRouteNavigation { get; set; } = null!;
        public virtual ICollection<Coach> Coaches { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
