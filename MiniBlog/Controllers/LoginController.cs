using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MiniBlog.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        CustomerManager customerm = new CustomerManager();
        [HttpGet]
        public ActionResult CustomerLogin()
        {
            if(HotelController.controlLogin == -1)
            {
                ViewBag.message = "Kullandığınız mail hesabı sistemimize tanımlıdır. Mevcut mail adresiniz ile giriş yaparak rezervasyon yapabilirsiniz.";
                ViewBag.control = HotelController.controlLogin;
            }
            HotelController.controlLogin = 1;
            return View();
        }
        [HttpPost]
        public ActionResult CustomerLogin(Customer p)
        {
            Context c = new Context();
            var userinfo = c.Customers.FirstOrDefault(x => x.Mail == p.Mail && x.Password == p.Password);
            if (userinfo != null)
            {
                FormsAuthentication.SetAuthCookie(userinfo.Mail, false);
                Session["Mail"] = userinfo.Mail.ToString();
                return RedirectToAction("Index", "Panel");
            }
            else
            {
                ViewBag.Message = "Geçersiz kullanıcı adı veya şifre.";
                return View();
            }

        }

        public ActionResult ResetPassword(string email)
        {
            Customer c = customerm.FindCustomerByMail(email);
            if (c!=null)
            {
                var pass=HotelController.CreatePassword();
                c.Password = pass;
                customerm.ResetCustomerPassword(c);
                SendPassword(c.Mail, pass);
            }
            return View("CustomerLogin");
        }

        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(Admin p)
        {
            Context c = new Context();
            var userinfo = c.Admins.FirstOrDefault(x => x.Username == p.Username && x.Password == p.Password);
            if (userinfo != null)
            {
                FormsAuthentication.SetAuthCookie(userinfo.Username, false);
                Session["Username"] = userinfo.Username.ToString();
                return RedirectToAction("AdminBlogList", "Blog");
            }
            else { return RedirectToAction("AdminLogin", "Login"); }

        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Hotel");
        }

        public static void SendPassword(string thisEmail, string thisPassword)
        {
            const string subject = "Şifre Resetleme Talebi Hk.";
            string body = "Sayın Müşterimiz,\nşifre resetleme talebiniz üzerine yeni şifreniz oluşturulmuştur. Aşağıda paylaşmış olduğumuz şifre ile sistemimize" +
                " giriş yapabilirsiniz.\nİyi günler dileriz.\n\nŞifre: " + thisPassword;
            var smtp = new SmtpClient
            {
                Host = "smtp.zoho.eu",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("info.tau.hotel@zohomail.eu", "Admin123!123")
            };
            using (var message = new MailMessage("info.tau.hotel@zohomail.eu", thisEmail)
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