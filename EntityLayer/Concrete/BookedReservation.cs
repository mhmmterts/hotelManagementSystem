using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class BookedReservation
    {
        [Key]
        public int BookedReservationId { get; set; }
        public int ReservationId { get; set; }
        public virtual Reservation Reservation { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public decimal TotalPrice { get; set; }
        public int ReservationKey { get; set; }
        public int NrOfCustomers { get; set; }

    }
}
