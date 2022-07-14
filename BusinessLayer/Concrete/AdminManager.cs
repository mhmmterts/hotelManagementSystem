using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class AdminManager
    {
        Repository<Admin> repoadmin = new Repository<Admin>();
        public List<Admin> GetAll()
        {
            return repoadmin.List();
        }

        public int DeleteAdmin(int p)
        {
            Admin admin = repoadmin.Find(x => x.AdminId == p);
            return repoadmin.Delete(admin);
        }

        public Admin FindAdmin(int id)
        {
            return repoadmin.Find(x => x.AdminId == id);
        }

        public int UpdateAdmin(Admin a)
        {
            Admin admin = repoadmin.Find(x => x.AdminId == a.AdminId);
            admin.Name = a.Name;
            admin.AdminRole = a.AdminRole;
            admin.Password = a.Password;
            admin.Username = a.Username;
            return repoadmin.Update(admin);

            //return repoactivity.Update(activity);
        }

        public int AddNewAdmin(Admin a)
        {
                return repoadmin.Insert(a);
        }
    }
}
