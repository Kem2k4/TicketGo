using System.Collections.Generic;

namespace TicketGo.Domain.Entities
{
    public partial class TrainRoute
    {
        public TrainRoute()
        {
            Trains = new HashSet<Train>();
        }

       

        public int IdTrainRoute { get; set; }
        public string PointStart { get; set; } = null!;
        public string PointEnd { get; set; } = null!;

        public virtual ICollection<Train> Trains { get; set; }
    }
}
