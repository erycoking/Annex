namespace Annex.Migrations
{
    using Annex.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<Annex.Models.CustomerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Annex.Models.CustomerDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            List<Customer> CustomerList = new List<Customer>();
            CustomerList.Add(new Customer()
            {
                FirstName = "Erick",
                OtherNames = "Loningo Lomunyak",
                FullName = "Erick Loningo Lomunyak",
                Address = "PO BOX 536, Nakuru",
                NationalId = 30765210,
                MobileNo = 0702554146,
                Photo = "erycoking.jpg"
            });

            CustomerList.Add(new Customer()
            {
                FirstName = "Berverly",
                OtherNames = "Nemayian",
                FullName = "Berverly Nemayian",
                Address = "PO BOX 100, Migori",
                NationalId = 30765211,
                MobileNo = 0702554141,
                Photo = "nesh.jpg"
            });

            CustomerList.Add(new Customer()
            {
                FirstName = "Kevin",
                OtherNames = "Kamau",
                FullName = "Kevin Kevin",
                Address = "PO BOX 536, Nakuru",
                NationalId = 30764210,
                MobileNo = 0702554156,
                Photo = "kevo.jpg"
            });

            CustomerList.Add(new Customer()
            {
                FirstName = "Bill",
                OtherNames = "Gates",
                FullName = "Bill Gates",
                Address = "PO BOX 100, NewYork",
                NationalId = 30765230,
                MobileNo = 0703554146,
                Photo = "bill.jpg"
            });

            CustomerList.Add(new Customer()
            {
                FirstName = "Patrick",
                OtherNames = "Ngumo",
                FullName = "Patrick Ngumo",
                Address = "PO BOX 10, Nakuru",
                NationalId = 30775210,
                MobileNo = 0702514146,
                Photo = "pato.jpg"
            });

            CustomerList.Add(new Customer()
            {
                FirstName = "Emmanuel",
                OtherNames = "Waganda",
                FullName = "Emmanuel Waganda",
                Address = "PO BOX 200, Kisumo",
                NationalId = 30765010,
                MobileNo = 0702554106,
                Photo = "emma.jpg"
            });

            context.Customers.AddRange(CustomerList);
            base.Seed(context);

        }
    }
}
