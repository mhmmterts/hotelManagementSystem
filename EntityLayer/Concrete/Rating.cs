using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int Rating1 { get; set; }
        public int Rating2 { get; set; }
        public int Rating3 { get; set; }
        public int Rating4 { get; set; }
        public int Rating5 { get; set; }
        public int Rating6 { get; set; }
        public int Rating7 { get; set; }
        [StringLength(500)]
        public string Rating8 { get; set; }
    }
}
