using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }
        [StringLength(50)]
        public string ReservationName { get; set; }
        [StringLength(1500)]
        public string ReservationInfo { get; set; }
        [StringLength(150)]
        public string ImageURL { get; set; }
        public int ReservationNr { get; set; }
        public int SoldReservationNr { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        public decimal ReservationPrice { get; set; }
    }
}
