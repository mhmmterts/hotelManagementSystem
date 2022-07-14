using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Concrete;
using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IcustomerService
    {
        List<Customer> GetList();
        void CustomerAdd(Customer customer);
        Customer GetByID(int id);
        void AboutDelete(Customer customer);
        void AboutUpdate(Customer customer);



    }
}
