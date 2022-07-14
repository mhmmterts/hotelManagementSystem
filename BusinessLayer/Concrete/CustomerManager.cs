using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public class CustomerManager
    {
        Repository<Customer> repocustomer = new Repository<Customer>();
        public int AddCustomer(Customer c)
        {
            return repocustomer.Insert(c);
        }

        public Customer FindCustomer(int id)
        {
            return repocustomer.Find(x => x.CustomerId == id);
        }

        public Customer FindCustomerByMail(string mail)
        {
            return repocustomer.Find(x => x.Mail == mail);
        }

        public List<Customer> GetAll()
        {

            return repocustomer.List();
        }

        public int DeleteCustomer(int c)
        {
            Customer customer = repocustomer.Find(x => x.CustomerId == c);
            return repocustomer.Delete(customer);
        }

        public int ResetCustomerPassword(Customer p)
        {
            Customer customer = repocustomer.Find(x => x.CustomerId == p.CustomerId);
            customer.Password = p.Password;
            return repocustomer.Update(customer);
        }

        public int UpdateCustomer(Customer c)
        {
            Customer cust = repocustomer.Find(x => x.CustomerId == c.CustomerId);
            cust.CustomerName = c.CustomerName;
            cust.Mail = c.Mail;
            cust.Password = c.Password;
            cust.PhoneNumber = c.PhoneNumber;
            return repocustomer.Update(cust);
        }
    }
}
