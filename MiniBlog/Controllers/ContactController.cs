using BusinessLayer.Concrete;
using EntityLayer.Concrete;
using DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace MiniBlog.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        ContactManager cm = new ContactManager();
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult AddContact()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AddContact(Contact c)
        {
            c.MessageDate = DateTime.Now;
            cm.BLContactAdd(c);
            return View();
        }
        public ActionResult SendBox(string arama = "", int page = 1)
        {
            Context c = new Context();
            if (!string.IsNullOrEmpty(arama))
            {
                IQueryable<Contact> elements = c.Contacts.Where(item => item.Mail.Contains(arama) || (item.Name+" "+item.Surname).Contains(arama));
                var searchedCustomers = elements.ToList().ToPagedList(page, 6);
                return View(searchedCustomers);
            }
            var messagelist = cm.GetAll().ToList().ToPagedList(page, 6); ;
            return View(messagelist);
        }
        public ActionResult MessageDetails(int id)
        {
            Contact contact = cm.GetContactDetails(id);
            return View(contact);
        }

        public ActionResult DeleteMessage(int id)
        {
            cm.Deletecontact(id);
            return RedirectToAction("SendBox");
        }
    }
}