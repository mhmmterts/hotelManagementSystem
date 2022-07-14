using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ActivityManager
    {
        Repository<Activity> repoactivity = new Repository<Activity>();
        public List<Activity> GetAll()
        {
            return repoactivity.List();
        }

        public int DeleteActivity(int p)
        {
            Activity activity = repoactivity.Find(x => x.ActivityId == p);
            return repoactivity.Delete(activity);
        }

        public Activity FindActivity(int id)
        {
            return repoactivity.Find(x => x.ActivityId == id);
        }

        public int UpdateActivity(Activity a)
        {
            Activity activity = repoactivity.Find(x => x.ActivityId == a.ActivityId);
            activity.ActivityName = a.ActivityName;
            activity.ActivityInfo = a.ActivityInfo;
            activity.ActivityPrice = a.ActivityPrice;
            return repoactivity.Update(activity);
        }

        public int AddNewActivity(Activity a)
        {
            if (a.ActivityName == "" || a.ActivityInfo == "")
            {
                return -1;
            }
            return repoactivity.Insert(a);
        }
    }
}
