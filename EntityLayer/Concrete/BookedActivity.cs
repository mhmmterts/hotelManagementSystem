using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class BookedActivity
    {
        [Key]
        public int BookedActivityId { get; set; }
        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int ReservationId { get; set; }
        public int ReservationKey{ get; set; }

    }
}
