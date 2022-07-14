using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class BookedReservationManager
    {
        Repository<BookedReservation> repobreservation = new Repository<BookedReservation>();
        public List<BookedReservation> GetAll()
        {
            return repobreservation.List();
        }

        public int DeleteBookedReservation(int p)
        {
            BookedReservation breservation = repobreservation.Find(x => x.ReservationId == p);
            return repobreservation.Delete(breservation);
        }

        public BookedReservation FindBookedReservation(int id)
        {
            return repobreservation.Find(x => x.ReservationId == id);
        }

        public int AddNewBookedReservation(BookedReservation br)
        {
            return repobreservation.Insert(br);
        }
    }
}
