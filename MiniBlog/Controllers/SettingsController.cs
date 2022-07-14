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
using System.Web.Security;

namespace MiniBlog.Controllers
{
    public class SettingsController : Controller
    {
        AdminManager adminm = new AdminManager();
        // GET: Settings
        public ActionResult Index()
        {
            var adminlist = adminm.GetAll();
            return View(adminlist);
        }

        public ActionResult GetAdmin(int id)
        {
            var person = adminm.FindAdmin(id);
            return View(person);
        }

        public ActionResult UpdateAdmin(Admin a)
        {
            Context c = new Context();
            int x = 0;
            var person = adminm.FindAdmin(a.AdminId);
            var userName = c.Admins.FirstOrDefault(y => y.Username == a.Username);
            if (userName != null && userName.AdminId != person.AdminId)
            {
                ViewBag.Message = "Bu kullanıcı adı mevcut.";
                return View("GetAdmin", person);
            }
            var nr = c.Admins.FirstOrDefault(defAdmin => defAdmin.Name == "default");
            if (a.Name == "default" && nr.AdminId != person.AdminId)
            {
                ViewBag.Message = "'default' kullanıcı adını kullanamazsın. Başka bir kullanıcı adı seç.";
                return View("GetAdmin", person);
            }
            person.Name = a.Name;
            person.Password = a.Password;

            if (a.Name == "default")
            {
                person.AdminRole = "A";
                a.AdminRole = "A";
            }

            if (Convert.ToInt32(Session["ID"]) == a.AdminId)
            {
                if (person.Username != a.Username || person.AdminRole != a.AdminRole)
                {
                    x = 1;
                }
            }

            if (a.Name == "default")
            {
                person.AdminRole = "A";
            }
            else
            {
                person.AdminRole = a.AdminRole;
            }
            person.Username = a.Username;
            if (x == 1)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("AdminLogin", "Login");
            }
            adminm.UpdateAdmin(person);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAdmin(Admin a)
        {
            Context c = new Context();
            var userName = c.Admins.FirstOrDefault(y => y.Username == a.Username.Trim());
            if (userName != null)
            {
                ViewBag.Message = "Bu kullanıcı adı mevcut.";
                return View("AddAdmin");
            }
            if (a.Name == "default")
            {
                ViewBag.Message = "'default' kullanıcı adını kullanamazsın. Başka bir kullanıcı adı seç. ";
                return View("AddAdmin");
            }
            adminm.AddNewAdmin(a);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            adminm.DeleteAdmin(id);
            return RedirectToAction("Index");
        }
    }
}