using Microsoft.AspNetCore.Mvc;
using MyFristTP.Models;
using System.Collections.Generic;

namespace MyFirstTP.Controllers
{


    public class CustomerController : Controller
    {

        public IActionResult Index()
        {
            return View(Customer.CUSTOMERS);
        }


        public IActionResult Details(int id)
        {
            Customer customer = Customer.CUSTOMERS.Find(c => c.Id == id);
            if (customer == null)
                return Content("Customer not found");
            return Content(customer.ToString());
        }


    }
}
