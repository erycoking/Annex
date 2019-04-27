using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Annex.Models;

namespace Annex.Controllers
{
    public class CustomersController : ApiController
    {
        private CustomerDbContext db = new CustomerDbContext();

        // GET: api/Customers
        public IHttpActionResult GetCustomers()
        {
            List<Customer> customers = new List<Customer>();
            db.Customers.ToList().ForEach((customer) =>
            {
                customer.Photo = getImage(customer.Photo);
                customers.Add(customer);
            });

            return Ok(customers);
        }

        // GET: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            

            customer.Photo = getImage(customer.Photo);
            return Ok(customer);
        }

        // PUT: api/Customers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, CustomerView customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            Customer customer1 = MapObjectAndSaveImage(customer);
            db.Entry(customer1).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Customers
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer(CustomerView customerView)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Customer customer = MapObjectAndSaveImage(customerView);
            db.Customers.Add(customer);
            db.SaveChanges();

            customer.Photo = getImage(customer.Photo);
            return CreatedAtRoute("DefaultApi", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [ResponseType(typeof(Customer))]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
            db.SaveChanges();

            return Ok(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.CustomerId == id) > 0;
        }

        private Customer MapObjectAndSaveImage(CustomerView customer)
        {
            // map customer view to customer object
            Customer customer1 = new Customer();
            customer1.CustomerId = customer.CustomerId;
            customer1.FirstName = customer.FirstName;
            customer1.OtherNames = customer.OtherNames;
            customer1.Address = customer.Address;
            customer1.NationalId = customer.NationalId;
            customer1.MobileNo = customer.MobileNo;

            // get file name
            string ImageName = new String(Path.GetFileNameWithoutExtension(customer.Photo.FileName)
                .Take(10).ToArray()).Replace(" ", "-");

            // add rename and add timestamp
            ImageName = ImageName + DateTime.Now.ToString("yymmssff") + Path.GetExtension(customer.Photo.FileName);
            string Image = HttpContext.Current.Server.MapPath("~/Content/Images/" + ImageName);

            // save photo
            customer.Photo.SaveAs(Image);

            // set photo name
            customer1.Photo = ImageName;

            return customer1;
        }

        private string getImage(string image)
        {
            return Path.Combine(HttpRuntime.AppDomainAppVirtualPath,
                    String.Format("/Content/Images/{0}", image));
        }
    }
}