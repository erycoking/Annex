using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Annex.Models;
using Newtonsoft.Json;

namespace Annex.Controllers
{
    public class CustomersController : ApiController
    {
        private CustomerDbContext db = new CustomerDbContext();

        // GET: api/Customers
        [HttpGet]
        [Route("~/api/Customers")]
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
        [HttpGet]
        [Route("~/api/Customers/{id:int}")]
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

        // GET: api/Customers/fname
        [HttpGet]
        [Route("~/api/Customers/{name}")]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomerByName(string name)
        {
            Customer customer = db.Customers.Where(e => e.FullName.Contains(name)).First();
            if (customer == null)
            {
                return NotFound();
            }


            customer.Photo = getImage(customer.Photo);
            return Ok(customer);
        }

        // GET: api/Customers/NationalId/national_id_no
        [HttpGet]
        [Route("~/api/Customers/NationalId/{id:int}")]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomerByNationalId(int id)
        {
            Customer customer = db.Customers.Where(e => e.NationalId == id).First();
            if (customer == null)
            {
                return NotFound();
            }


            customer.Photo = getImage(customer.Photo);
            return Ok(customer);
        }

        // GET: api/Customers/mobileNo
        [ResponseType(typeof(Customer))]
        [HttpGet]
        [Route("~/api/Customers/mobile/{MobileNo:int}")]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult GetCustomerMobileNo(int MobileNo)
        {
            Customer customer = db.Customers.Where(e => e.MobileNo == MobileNo).First();
            if (customer == null)
            {
                return NotFound();
            }


            customer.Photo = getImage(customer.Photo);
            return Ok(customer);
        }

        // PUT: api/Customers/5
        [HttpPut]
        [Route("~/api/Customers/{id:int}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id)
        {

            var httpRequest = HttpContext.Current.Request;
            Dictionary<string, string> cust = JsonConvert.DeserializeObject<Dictionary<string, string>>(httpRequest.Form["customer"]);
            var imageFile = httpRequest.Files["imageFile"];

            if (id != Int32.Parse(cust["CustomerId"]))
            {
                return BadRequest();
            }

            if (imageFile == null)
            {
                return BadRequest("Image required");
            }

            if (cust["FirstName"] == null || cust["FirstName"] == "")
            {
                return BadRequest("FirstName required");
            }

            if (cust["OtherNames"] == null || cust["OtherNames"] == "")
            {
                return BadRequest("OtherNames required");
            }

            if (cust["Address"] == null || cust["Address"] == "")
            {
                return BadRequest("Address required");
            }

            if (cust["NationalId"] == null || cust["NationalId"] == "")
            {
                return BadRequest("NationalId required");
            }

            if (cust["MobileNo"] == null || cust["MobileNo"] == "")
            {
                return BadRequest("MobileNo required");
            }

            CustomerView customerView = new CustomerView()
            {
                CustomerId = Int32.Parse(cust["CustomerId"]),
                FirstName = cust["FirstName"],
                OtherNames = cust["OtherNames"],
                Address = cust["Address"],
                NationalId = Int32.Parse(cust["NationalId"]),
                MobileNo = Int32.Parse(cust["MobileNo"])
            };

            Customer customer1 = MapObjectAndSaveImage(customerView);

            // get file without extension
            string imageWithoutExtension = Path.GetFileNameWithoutExtension(imageFile.FileName);
            // get extension
            string imageExtension = Path.GetExtension(imageFile.FileName);
            // get file name, add rename and add timestamp
            string ImageName = imageWithoutExtension + DateTime.Now.ToString("yymmssff") + imageExtension;
            // set photo name
            customer1.Photo = ImageName;

            // save photo
            imageFile.SaveAs(HttpContext.Current.Server.MapPath("~/Content/Images/" + ImageName));

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
                    return BadRequest("Ensure all fields are filled correctly");
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Customers
        [HttpPost]
        [Route("~/api/Customers", Name = "GetCustomer")]
        [ResponseType(typeof(Customer))]
        public IHttpActionResult PostCustomer()
        {
            var httpRequest = HttpContext.Current.Request;
            Dictionary<string, string> cust = JsonConvert.DeserializeObject<Dictionary<string, string>>(httpRequest.Form["customer"]);
            var imageFile = httpRequest.Files["imageFile"];

            if (imageFile == null)
            {
                return BadRequest("Image required");
            }

            if (cust["FirstName"] == null || cust["FirstName"] == "")
            {
                return BadRequest("FirstName required");
            }

            if (cust["OtherNames"] == null || cust["OtherNames"] == "")
            {
                return BadRequest("OtherNames required");
            }

            if (cust["Address"] == null || cust["Address"] == "")
            {
                return BadRequest("Address rerequiredquied");
            }

            if (cust["NationalId"] == null || cust["NationalId"] == "")
            {
                return BadRequest("NationalId required");
            }

            if (cust["MobileNo"] == null || cust["MobileNo"] == "")
            {
                return BadRequest("MobileNo required");
            }

            CustomerView customerView = new CustomerView()
            {
                FirstName = cust["FirstName"],
                OtherNames = cust["OtherNames"],
                Address = cust["Address"],
                NationalId = Int32.Parse(cust["NationalId"]),
                MobileNo = Int32.Parse(cust["MobileNo"])
            };


            Customer customer = MapObjectAndSaveImage(customerView);

            // get file without extension
            string imageWithoutExtension = Path.GetFileNameWithoutExtension(imageFile.FileName);
            // get extension
            string imageExtension = Path.GetExtension(imageFile.FileName);
            // get file name, add rename and add timestamp
            string ImageName = imageWithoutExtension + DateTime.Now.ToString("yymmssff") + imageExtension;
            // set photo name
            customer.Photo = ImageName;

            db.Customers.Add(customer);

            try
            {
                // string ImageStoragePath = HttpContext.Current.Server.MapPath("~/Content/Images/");

                // save photo
                imageFile.SaveAs(HttpContext.Current.Server.MapPath("~/Content/Images/" + ImageName));

                db.SaveChanges();

                // send notification
                sendNotification(customer);
            }
            catch
            {
                return BadRequest("Ensure all input are filled");
            }

            customer.Photo = getImage(customer.Photo);
            return CreatedAtRoute("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete]
        [Route("~/api/Customers/{id:int}")]
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
            customer1.FullName = customer.FirstName + " " + customer.OtherNames;

            return customer1;
        }

        private string getImage(string image)
        {
            return Path.Combine(HttpRuntime.AppDomainAppVirtualPath,
                    String.Format("/Content/Images/{0}", image));
        }

        public void sendNotification(Customer customer)
        {
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            //serverKey - Key from Firebase cloud messaging server  
            tRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAA7d7tzg8:APA91bE6aATljQdU_pwk7Md4uyiRZ52jQTDy9aWaYeYqouP7IAVOzOpqkGHXpWZd1tfbRFXHP8hvRCv2OVUfa3Chf0ZDXNwT5vT8XHamG8_fyEUiOyffX9-0fKMD6X22xfENM-YNKgR3"));
            //Sender Id - From firebase project setting  
            tRequest.Headers.Add(string.Format("Sender: id={0}", "1021647375887"));
            tRequest.ContentType = "application/json";
            var payload = new
            {
                to = "1:1021647375887:android:d7bcdabe21e11bc2",
                priority = "high",
                content_available = true,
                notification = new
                {
                    body = customer.FullName + " Successfully Added",
                    title = "Annex",
                    badge = 1
                },
            };

            string postbody = JsonConvert.SerializeObject(payload).ToString();
            Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
            tRequest.ContentLength = byteArray.Length;
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                //result.Response = sResponseFromServer;
                            }
                    }
                }
            }
        }

    }
}