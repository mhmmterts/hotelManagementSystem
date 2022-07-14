using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class SubscribeMailManager
    {
        Repository<SubscribeMail> reposucscribemail = new Repository<SubscribeMail>();
        public int BLAdd(SubscribeMail p)
        {
            if (p.Mail.Length <= 10 || p.Mail.Length>=50)
            {
                return -1;
            }
            return reposucscribemail.Insert(p);

        }

        public List<SubscribeMail> GetAll()
        {
            return reposucscribemail.List();
        }

        public int Unsubscribe(int p)
        {
            SubscribeMail sm = reposucscribemail.Find(x => x.MailId == p);
            return reposucscribemail.Delete(sm);
        }
    }
}
