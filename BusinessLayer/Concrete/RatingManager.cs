using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLayer.Concrete
{
    public class RatingManager
    {
        Repository<Rating> reporating = new Repository<Rating>();
        public List<Rating> GetAll()
        {
            return reporating.List();
        }

        public Rating FindRating(int id)
        {
            return reporating.Find(x => x.CustomerId == id);
        }

        public int AddNewRating(Rating r)
        {
            return reporating.Insert(r);
        }
    }
}
