using EntityLayer.Concrete;
using System.Collections.Generic;

namespace MiniBlog.Controllers
{
    public class ViewModel
    {
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }
    }
}