using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class BookedActivityManager
    {
        Repository<BookedActivity> repobactivity = new Repository<BookedActivity>();
        public List<BookedActivity> GetAll()
        {
            return repobactivity.List();
        }

        public int DeleteBookedActivity(int p)
        {
            BookedActivity bookedactivity = repobactivity.Find(x => x.BookedActivityId == p);
            return repobactivity.Delete(bookedactivity);
        }

        public BookedActivity FindBookedActivity(int id)
        {
            return repobactivity.Find(x => x.BookedActivityId == id);
        }

        public int AddNewBookedActivity(BookedActivity ba)
        {
            return repobactivity.Insert(ba);
        }
    }
}
