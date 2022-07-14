using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Activity
    {
        [Key]
        public int ActivityId { get; set; }
        [StringLength(50)]
        public string ActivityName { get; set; }
        [StringLength(500)]
        public string ActivityInfo { get; set; }
        public decimal ActivityPrice { get; set; }

    }
}
