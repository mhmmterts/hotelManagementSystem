﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class SubscribeMail
    {
        [Key]
        public int MailId { get; set; }
        [StringLength(50)]
        public string Mail { get; set; }
        
    }
}
