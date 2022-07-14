using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ReservationManager
    {
        Repository<Reservation> reporeservation = new Repository<Reservation>();
        public List<Reservation> GetAll()
        {
            return reporeservation.List();
        }

        public int DeleteReservation(int p)
        {
            Reservation reservation = reporeservation.Find(x => x.ReservationId == p);
            return reporeservation.Delete(reservation);
        }

        public Reservation FindReservation(int id)
        {
            return reporeservation.Find(x => x.ReservationId == id);
        }

        public int UpdateReservation(Reservation a)
        {
            Reservation reservation = reporeservation.Find(x => x.ReservationId == a.ReservationId);
            reservation.ReservationName = a.ReservationName;
            reservation.ReservationInfo = a.ReservationInfo;
            reservation.ReservationPrice = a.ReservationPrice;
            reservation.ImageURL = a.ImageURL;
            reservation.StartDate = a.StartDate;
            reservation.EndDate = a.EndDate;
            return reporeservation.Update(reservation);
        }

        public int AddNewReservation(Reservation a)
        {
            if (a.ReservationName == "" || a.ReservationName == "")
            {
                return -1;
            }
            return reporeservation.Insert(a);
        }
    }
}
