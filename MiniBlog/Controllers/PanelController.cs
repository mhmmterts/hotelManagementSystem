using EntityLayer.Concrete;
using BusinessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Text;
using System.Dynamic;
using DataAccessLayer.Concrete;

namespace MiniBlog.Controllers
{
    [AllowAnonymous]
    public class PanelController : Controller
    {
        CustomerManager customerm = new CustomerManager();
        ReservationManager reservationm = new ReservationManager();
        ActivityManager activitym = new ActivityManager();
        RatingManager ratingm = new RatingManager();
        // GET: Panel
        public ActionResult Index()
        {
            Context c = new Context();
            var user = (string)Session["Mail"];
            var customer = customerm.FindCustomerByMail(user);
            var bookedReservation = c.BookedReservations.Where(x => x.CustomerId == customer.CustomerId);
            List<Reservation> reservations = new List<Reservation>();
            List<int> bookedRes = new List<int>();
            List<int> nrOfCustomers = new List<int>();
            foreach (var item in bookedReservation)
            {
                reservations.Add(reservationm.FindReservation(item.ReservationId));
                bookedRes.Add(item.ReservationKey);
                nrOfCustomers.Add(item.NrOfCustomers);
            }
            ViewBag.reservations = reservations;
            ViewBag.reservationKey = bookedRes;
            ViewBag.nrofcustomers = nrOfCustomers;
            Rating mycustomer = ratingm.FindRating(customer.CustomerId);
            if (mycustomer == null) {
                ViewBag.customerId = customer.CustomerId;
            }
            return View(customer);
        }

        public ActionResult ReservationDetail(int resKey)
        {
            Context c = new Context();
            var user = (string)Session["Mail"];
            var customer = customerm.FindCustomerByMail(user);
            var bookedReservation = c.BookedReservations.Where(x => x.CustomerId == customer.CustomerId && x.ReservationKey == resKey).FirstOrDefault();
            Reservation reservation = reservationm.FindReservation(bookedReservation.ReservationId);
            var bookedActivities = c.BookedActivities.Where(x => x.CustomerId == customer.CustomerId && x.ReservationKey == bookedReservation.ReservationKey);
            List<Activity> bookedActivitiesList = new List<Activity>();
            foreach (var item in bookedActivities)
            {
                bookedActivitiesList.Add(activitym.FindActivity(item.ActivityId));
            }
            ViewBag.reservation = reservation;
            ViewBag.bookedReservation = bookedReservation;
            ViewBag.bookedActivities = bookedActivitiesList;
            return View("ReservationDetail", customer);
        }

        [HttpGet]
        public ActionResult RatingForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RatingForm(Rating r)
        {
            var user = (string)Session["Mail"];
            var customer = customerm.FindCustomerByMail(user);
            r.CustomerId = customer.CustomerId;
            ratingm.AddNewRating(r);
            return RedirectToAction("Index", "Panel");
        }

        public ActionResult RatingResults()
        {
            List<Rating> ratings=ratingm.GetAll();
            double averageResult = 0;
            double rating1 = 0;
            double rating2 = 0;
            double rating3 = 0;
            double rating4 = 0;
            double rating5 = 0;
            double rating6 = 0;
            foreach (var item in ratings)
            {
                rating1 += item.Rating1;
                rating2 += item.Rating2;
                rating3 += item.Rating3;
                rating4 += item.Rating4;
                rating5 += item.Rating5;
                rating6 += item.Rating6;
                averageResult += item.Rating7;
            }
            rating1 /= ratings.Count();
            rating2 /= ratings.Count();
            rating3 /= ratings.Count();
            rating4 /= ratings.Count();
            rating5 /= ratings.Count();
            rating6 /= ratings.Count();
            averageResult /= ratings.Count();
            averageResult /= 2;
            ViewBag.rating1 = rating1;
            ViewBag.rating2 = rating2;
            ViewBag.rating3 = rating3;
            ViewBag.rating4 = rating4;
            ViewBag.rating5 = rating5;
            ViewBag.rating6 = rating6;
            ViewBag.rating7 = averageResult;
            ViewBag.nrOfCustomers = ratings.Count();
            return View();
        }



    }
}