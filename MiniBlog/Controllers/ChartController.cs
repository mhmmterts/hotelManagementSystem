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
    public class ChartController : Controller
    {
        CustomerManager customerm = new CustomerManager();
        BookedReservationManager breservationm = new BookedReservationManager();
        ReservationManager reservationm = new ReservationManager();
        RatingManager ratingm = new RatingManager();
        // GET: Chart

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Chart1()
        {
            return View();
        }

        public ActionResult Statistics()
        {
            ViewBag.nrOfCustomers = customerm.GetAll().Count;
            ViewBag.nrOfSoldReservations = breservationm.GetAll().Count;
            ViewBag.nrOfPackets = reservationm.GetAll().Count;
            List<Rating> ratings = ratingm.GetAll();
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
            averageResult /= ratings.Count() * 2;
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