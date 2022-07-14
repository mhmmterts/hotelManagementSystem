using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MiniBlog.Controllers
{
    [AllowAnonymous]
    public class MailSubscribeController : Controller
    {
        SubscribeMailManager smmanager = new SubscribeMailManager();
        // GET: MailSubscribe
        [HttpGet]
        public PartialViewResult AddMail()
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult AddMail(SubscribeMail p)
        {
            Context c = new Context();
            var subscriber = c.SubscribeMails.FirstOrDefault(x => x.Mail == p.Mail);
            if (subscriber == null)
            {
                SubscribeMailManager sm = new SubscribeMailManager();
                sm.BLAdd(p);
                SendEmail(p.Mail, null);
            }
            return PartialView();
        }

        public ActionResult SubscribedMails(string arama = "", int page = 1)
        {
            Context c = new Context();
            if (!string.IsNullOrEmpty(arama))
            {
                IQueryable<SubscribeMail> elements = c.SubscribeMails.Where(item => item.Mail.Contains(arama));
                var searchedSubscribers = elements.ToList().ToPagedList(page, 6);
                return View(searchedSubscribers);
            }
            var subscriberList = smmanager.GetAll().ToList().ToPagedList(page, 6);
            return View(subscriberList);
        }

        public ActionResult DeleteMail(int id)
        {
            smmanager.Unsubscribe(id);
            return RedirectToAction("SubscribedMails");
        }

        public static void SendEmail(string thisEmail, string thisPassword)
        {
            var fromAddress = new MailAddress("info.tau.hotel@gmail.com");
            var toAddress = new MailAddress(thisEmail, "Sayın Müşterimiz");
            const string fromPassword = "Admin123!";
            const string subject = "Rezervasyon Bilgileri TAU Hotel";
            string body = "Sayın Müşterimiz,\nTau Hotel'in Blog bültenine abone olduğunuz için teşekkür ederiz. Haftalık ve aylık olarak bültenimizde yayınlanan yeni bloglarımızdan sizleri haberdar edeceğiz.\nİyi günler dileriz.\n\n" +
                "TAU Hotel\n" +
                "İletişim No: +901234567890\n" +
                "Adres: Bizim Mahalle";


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
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