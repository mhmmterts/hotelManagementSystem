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
using System.Net.Mail;
using System.Net;

namespace MiniBlog.Controllers
{
    [AllowAnonymous]
    public class HotelController : Controller
    {
        CustomerManager customerm = new CustomerManager();
        ActivityManager activitym = new ActivityManager();
        ReservationManager reservationm = new ReservationManager();
        BookedReservationManager breservationm = new BookedReservationManager();
        BookedActivityManager bactivitym = new BookedActivityManager();
        public static List<Activity> BookedActivities = new List<Activity>();
        public static int NrOfCustomers = 1;
        public static int controlLogin = 1;

        //otel anasayfasini acar
        public ActionResult Index()
        {
            return View();
        }

        //tatil paketlerinin listelendigi sayfayi acar
        [HttpGet]
        public ActionResult BookPackage(string startdate = "", int page = 1)
        {
            var customers = customerm.GetAll();
            var bookedReservations = breservationm.GetAll().Select(x => x.CustomerId);
            foreach (var item in customers)
            {
                if (!bookedReservations.Contains(item.CustomerId))
                {
                    customerm.DeleteCustomer(item.CustomerId);
                }
            }
            if (User.Identity.IsAuthenticated && !User.IsInRole("A"))
            {
                var email = Session["Mail"];
                var customer = customerm.FindCustomerByMail(email.ToString());
                ViewBag.name = customer.CustomerName;
                ViewBag.mail = email;
                ViewBag.phone = customer.PhoneNumber;
            }


            ViewBag.activities = activitym.GetAll().ToList();
            Context c = new Context();
            if (!string.IsNullOrEmpty(startdate))
            {
                DateTime date = DateTime.Parse(startdate);
                IQueryable<Reservation> elements = c.Reservations.Where(x => x.StartDate >= date);
                var searchedReservations = elements.ToList().ToPagedList(page, 6);
                return View(searchedReservations);
            }
            var packages = reservationm.GetAll().ToPagedList(page, 6);
            return View(packages);
        }

        //tatil paketini satin alma isleminde musteriyi Customers tablosuna ekler ve odeme sayfasina yonlendirir
        [HttpPost]
        public ActionResult BookPackage(Customer c, Reservation r, FormCollection fr, BookedReservation br)
        {
            var email = Session["Mail"];
            Customer cust = customerm.FindCustomerByMail(c.Mail);
            if (cust == null && email == null)
            {
                c.Password = CreatePassword();
                customerm.AddCustomer(c);
            }
            else
            {
                if (!User.Identity.IsAuthenticated)
                {
                    controlLogin = -1;
                    return RedirectToAction("CustomerLogin", "Login");
                }
                else
                {
                    c = customerm.FindCustomerByMail(email.ToString());
                }

            }
            NrOfCustomers = br.NrOfCustomers;
            List<Activity> activities = activitym.GetAll();
            List<Activity> bookedActivities = new List<Activity>();
            foreach (var activity in activities)
            {
                if (!string.IsNullOrEmpty(fr[activity.ActivityName]))
                {
                    bookedActivities.Add(activity);
                }
            }
            BookedActivities = bookedActivities;
            return RedirectToAction("ReservePackage", "Hotel", new { customerID = c.CustomerId, reservationID = r.ReservationId });
        }

        //tatil paketinin odeme sayfasina yonlendirir
        [HttpGet]
        public ActionResult ReservePackage(int customerID, int reservationID)
        {
            Customer customer = customerm.FindCustomer(customerID);
            Reservation reservation = reservationm.FindReservation(reservationID);
            ViewBag.reservation = reservation;
            ViewBag.bookedActivities = BookedActivities;
            var totalPrice = reservation.ReservationPrice;
            foreach (var item in BookedActivities)
            {
                totalPrice += item.ActivityPrice * NrOfCustomers;
            }
            ViewBag.totalPrice = totalPrice;
            ViewBag.nrofcustomers = NrOfCustomers;
            return View(customer);
        }

        //tatil paketinin odeme islemi gerceklestirilerek musteri-rezervasyon / musteri-aktivite tablolarina bilgileri ekleniyor
        [HttpPost]
        public ActionResult ReservePackage(BookedReservation br)
        {
            Reservation reservation = reservationm.FindReservation(br.ReservationId);
            reservation.SoldReservationNr += 1;
            Customer customer = customerm.FindCustomer(br.CustomerId);
            var totalPrice = reservation.ReservationPrice;
            Random r = new Random();
            var i = r.Next() * 9999999;
            foreach (var item in BookedActivities)
            {
                BookedActivity ba = new BookedActivity();
                ba.ActivityId = item.ActivityId;
                ba.CustomerId = br.CustomerId;
                ba.ReservationId = br.ReservationId;
                ba.ReservationKey = i;
                bactivitym.AddNewBookedActivity(ba);
                totalPrice += item.ActivityPrice * NrOfCustomers;
            }
            br.TotalPrice = totalPrice;
            br.ReservationKey = i;
            br.NrOfCustomers = NrOfCustomers;
            NrOfCustomers = 1;
            breservationm.AddNewBookedReservation(br);
            BookedActivities.Clear();
            if (User.Identity.IsAuthenticated)
            {
                SendEmail(customer.Mail, null);
            }
            else
            {
                SendEmail(customer.Mail, customer.Password);
            }
            reservationm.UpdateReservation(reservation);
            return View("Index");
        }

        //admin panelinde aktivite tablosunun bulundugu sayfaya yonlendirir ve aktiviteleri veri tabanindan cekerek sayfaya gonderir
        public ActionResult AdminActivityList()
        {
            var activitylist = activitym.GetAll();
            return View(activitylist);
        }

        //aktivite silme islemini gerceklestirir
        public ActionResult DeleteActivity(int id)
        {
            activitym.DeleteActivity(id);
            return RedirectToAction("AdminActivityList");
        }

        //tabloda aktivite guncelle butonuna basilinca guncelleme sayfasina aktiviteyi bularak ceker
        [HttpGet]
        public ActionResult UpdateActivity(int id)
        {
            Activity activity = activitym.FindActivity(id);
            return View(activity);
        }

        //aktivite guncelleme sayfasinda guncelleme islemini yaparak tablo sayfasina geri yonlendirir
        [HttpPost]
        public ActionResult UpdateActivity(Activity activity)
        {
            activitym.UpdateActivity(activity);
            return RedirectToAction("AdminActivityList");
        }

        public ActionResult AdminCustomerList(string arama = "", int page = 1)
        {
            int result;
            Context c = new Context();
            if (Int32.TryParse(arama, out result) && !string.IsNullOrEmpty(arama))
            {
                IQueryable<Customer> elements = c.BookedReservations.Where(item => item.ReservationId == result).Select(item => item.Customer);
                var searchedCustomers = elements.ToList().ToPagedList(page, 6);
                return View(searchedCustomers);
            }
            if (!string.IsNullOrEmpty(arama))
            {
                IQueryable<Customer> elements = c.Customers.Where(item => item.CustomerName.Contains(arama) || item.Mail.Contains(arama));
                var searchedCustomers = elements.ToList().ToPagedList(page, 6);
                return View(searchedCustomers);
            }
            var customerList = customerm.GetAll().ToList().ToPagedList(page, 6);
            return View(customerList);
        }

        [HttpGet]
        public ActionResult UpdateCustomer(int id)
        {
            Customer c = customerm.FindCustomer(id);
            return View(c);
        }

        [HttpPost]
        public ActionResult UpdateCustomer(Customer c)
        {
            customerm.UpdateCustomer(c);
            return RedirectToAction("AdminCustomerList");
        }

        public ActionResult ShowReservations(int id)
        {
            Context c = new Context();
            var customer = customerm.FindCustomer(id);
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
            return View(customer);
        }

        public ActionResult ReservationDetail(int resKey, int id)
        {
            Context c = new Context();
            var customer = customerm.FindCustomer(id);
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

        //yeni aktivite ekleme islemini gerceklestirir
        [HttpPost]
        public ActionResult AddNewActivity(Activity activity)
        {
            activitym.AddNewActivity(activity);
            return RedirectToAction("AdminActivityList");
        }

        //yeni aktivite ekleme sayfasina yonlendirir
        [HttpGet]
        public ActionResult AddNewActivity()
        {
            return View();
        }

        //admin panelinde tatil paketleri tablosunun bulundugu sayfaya yonlendirir ve tatil paketlerini veri tabanindan cekerek sayfaya gonderir
        public ActionResult AdminReservationList(string startdate = "", int page = 1)
        {
            Context c = new Context();
            if (!string.IsNullOrEmpty(startdate))
            {
                DateTime date = DateTime.Parse(startdate);
                IQueryable<Reservation> elements = c.Reservations.Where(x => x.StartDate >= date);
                var searchedReservations = elements.ToList().ToPagedList(page, 6);
                ViewBag.date = date;
                return View(searchedReservations);
            }
            var reservationlist = reservationm.GetAll().Reverse<Reservation>().ToList().ToPagedList(page, 6);
            return View(reservationlist);
        }

        //tatil paketi silme islemini gerceklestirir
        public ActionResult DeleteReservation(int id)
        {
            reservationm.DeleteReservation(id);
            return RedirectToAction("AdminReservationList");
        }

        //tabloda tatil paketi guncelle butonuna basilinca guncelleme sayfasina tatil paketini bularak ceker
        [HttpGet]
        public ActionResult UpdateReservation(int id)
        {
            Reservation reservation = reservationm.FindReservation(id);
            ViewBag.start = reservation.StartDate.ToString("yyyy-MM-dd");
            ViewBag.end = reservation.EndDate.ToString("yyyy-MM-dd");
            return View(reservation);
        }

        //tatil paketi guncelleme sayfasinda guncelleme islemini yaparak tablo sayfasina geri yonlendirir
        [HttpPost]
        public ActionResult UpdateReservation(Reservation reservation)
        {
            reservationm.UpdateReservation(reservation);
            return RedirectToAction("AdminReservationList");
        }

        //yeni tatil paketi ekleme islemini gerceklestirir
        [HttpPost]
        public ActionResult AddNewReservation(Reservation reservation)
        {
            reservationm.AddNewReservation(reservation);
            return RedirectToAction("AdminReservationList");
        }

        //yeni tatil paketi ekleme sayfasina yonlendirir
        [HttpGet]
        public ActionResult AddNewReservation()
        {
            return View();
        }

        public ActionResult GetCustomerExcelFile()
        {
            var customerList = customerm.GetAll();
            return View(customerList);
        }


        //8 karakterli random password uretir
        public static string CreatePassword()
        {
            int length = 8;
            const string valid = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public static void SendEmail(string thisEmail, string thisPassword)
        {
            const string subject = "Rezervasyon Bilgileri TAU Hotel";
            string body;
            if (thisPassword == null)
            {
                body = "Sayın Müşterimiz,\nTau Hotel'i tercih ettiğiniz için teşekkür ederiz. Rezervasyonunuza ait ayrıntılı bilgilere ulaşmak için websitemize kayıtlı email hesabınız ile erişerek " +
                "rezervasyon içeriğini kullanıcı paneli altında görüntüleyebilir ve seçtiğiniz aktiviteler hakkında bilgi alabilirsiniz. Ayrıca otelimiz ve tatil yöresi hakkında faydalı ve güzel içeriklere otelimizin blog sayfasından erişebilirsiniz.\nİyi tatiller dileriz.\n\n" +
                "TAU Hotel\n" +
                "İletişim No: +901234567890\n" +
                "Adres: Bizim Mahalle";
            }
            else
            {
                body = "Sayın Müşterimiz,\nTau Hotel'i tercih ettiğiniz için teşekkür ederiz. Rezervasyonunuza ait ayrıntılı bilgilere ulaşmak için websitemize email adresinizi kullanarak " +
                "aşağıdaki şifre ile giriş yapabilirsiniz. Rezervasyon içeriğini kullanıcı paneli altında görüntüleyebilir ve seçtiğiniz aktiviteler hakkında bilgi alabilirsiniz. Ayrıca otelimiz ve tatil yöresi hakkında faydalı ve güzel içeriklere otelimizin blog sayfasından erişebilirsiniz.\nİyi tatiller dileriz.\n\n" +
                "Giriş için kullanacağınız şifre: " + thisPassword + "\n\n" +
                "TAU Hotel\n" +
                "İletişim No: +901234567890\n" +
                "Adres: Bizim Mahalle";
            }


            var smtp = new SmtpClient
            {
                Host = "smtp.zoho.eu",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("info.tau.hotel@zohomail.eu", "Admin123!123")
            };
            using (var message = new MailMessage("info.tau.hotel@zohomail.eu",thisEmail)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }



    }
}