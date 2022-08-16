


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;           //--< Be sure to "Include" this <<<

using System.Xml;
using System.Xml.Linq;

namespace Sales
{

    public partial class SalesOrder
    {
        static SalesEntities db = new SalesEntities();

        public int TotalQuantity => SalesOrderParts.Sum(sop => sop.Quantity);
        public decimal TotalSales => SalesOrderParts.Sum(sop => sop.Quantity * sop.UnitPrice);
        public decimal Variance => OrderTotal - TotalSales;
        public void Taxes()
        {
            //--< Method Syntax >--//
            var taxes = db.SalesOrders.Select(
                so => new
                {
                    OrderNum = so.SalesOrderNumber,
                    so.OrderTotal,
                    Customer = so.Customer.FirstName + so.Customer.LastName,
                    Taxes = so.OrderTotal * .06m
                }
                ).ToList(); //--< List of anonymous objects <<<
            //--< Query Style >--//
            taxes = (from so in db.SalesOrders
                     let tax = so.OrderTotal * .06m
                     select new
                     {
                         OrderNum = so.SalesOrderNumber,
                         so.OrderTotal,
                         Customer = so.Customer.FirstName + so.Customer.LastName,
                         Taxes = so.OrderTotal * .06m
                     }
                     ).ToList();
            foreach (var tax in taxes)
            {
                Console.WriteLine(
                    $"{tax.OrderNum}\t{tax.OrderTotal}\t {tax.Taxes}\t {tax.Customer}");
            }
        }

 ETL
        public string ToCsv(char delimiter)
        {
            var csv = $"{SalesOrderNumber}{delimiter}{CustomerId}{delimiter}" +
                $"{OrderDate:d}{delimiter}" +
                $"{OrderTotal:c}{delimiter}" +
                $"{OrderCost}{delimiter}" +
                $"{SalesStatusId}";
            return csv;
        }
        public string ToCsv()
        {
            return ToCsv(',');
        }
        public static XDocument ToXml()
        {
            using (var db = new SalesEntities())
            {
                var salesOrders = db.SalesOrders.ToArray();
                return ToXml(salesOrders);
            }
        }
        public static XDocument ToXml(IEnumerable<SalesOrder> salesOrders)
        {
            using (XmlWriter xw = XmlWriter.Create("XML/SalesOrders.xml"))
            {
                var xdoc = new XDocument();
                var xmlSalesOrders = new XElement("SalesOrders");
                foreach (var salesOrder in salesOrders)
                {
                    var xmlSalesOrder = new XElement("SalesOrder");
                    xmlSalesOrder.Add(
                        new XAttribute("SalesOrderNumber",
                        salesOrder.SalesOrderNumber.ToString())
                    );
                    xmlSalesOrder.Add(
                        new XAttribute("CustomerId",
                        salesOrder.CustomerId.ToString())
                    );
                    xmlSalesOrder.Add(
                        new XAttribute("OrderDate",
                        salesOrder.OrderDate.ToString())
                    );

                    xmlSalesOrder.Add(
                        new XAttribute("OrderTotal",
                        salesOrder.OrderTotal.ToString())
                    );

                    xmlSalesOrder.Add(
                        new XAttribute("OrderCost",
                        salesOrder.OrderCost.ToString())
                    );

                    xmlSalesOrder.Add(
                        new XAttribute("SalesStatusId",
                        salesOrder.SalesStatusId.ToString())
                    );
                    xmlSalesOrders.Add(xmlSalesOrder);
                }
                xdoc.Add(xmlSalesOrders);
                xdoc.Save(xw);
                return xdoc;
            }
        }
        public static SalesOrder FromXml(XElement xelem)
        {
            var salesStatus = xelem.Attribute("SalesStatusId").Value;
            Enum.TryParse(salesStatus, out SalesStatus salesStatusId);
            var salesOrder = new SalesOrder()
            {
                SalesOrderNumber = Convert.ToInt32(xelem.Attribute("SalesOrderNumber").Value),
                CustomerId = Convert.ToInt32(xelem.Attribute("CustomerId").Value),
                OrderDate = Convert.ToDateTime(xelem.Attribute("OrderDate").Value),
                OrderTotal = Convert.ToDecimal(xelem.Attribute("OrderTotal").Value),
                OrderCost = Convert.ToDecimal(xelem.Attribute("OrderCost").Value),
                SalesStatusId = salesStatusId
            };
            return salesOrder;
        }


    }
    public partial class Customer
    {
        static SalesEntities db = new SalesEntities();
        public decimal TotalSales => SalesOrders.Sum(so => so.OrderTotal);
        public static decimal TotalSalesAllCustomers => (
            from cust in db.Customers
            from sos in cust.SalesOrders    //--< From From <<<
            select sos.OrderTotal
            ).Sum();
        public static decimal AccountsReceivable => (
            from cust in db.Customers
            select cust.Balance ?? 0m
            ).Sum();
        public static List<Customer> LargestCustomers =>
            db.Customers.ToArray().OrderByDescending(c => c.TotalSales).Take(5).ToList();
        public static Dictionary<int, Customer> CustomerDictionary =>
            db.Customers
            .Include(c => c.SalesOrders.Select(so => so.SalesOrderParts))
            .ToDictionary(c => c.CustomerId);
    }
    public partial class Customer   //--< Last years >--//
    {
        public string Name => $"{FirstName} {LastName}";
        public static string Student => "Bob Dust";
        public int OrderCount => SalesOrders != null ? SalesOrders.Count() : -1;
        public decimal TotalSales2
        {
            get
            {
                if (SalesOrders == null)
                    SalesOrders = db.SalesOrders
                        .Where(so => so.CustomerId == CustomerId)
                        .ToList();
                return SalesOrders.Sum(so => so.OrderTotal);
            }
        }
        public static List<Customer> CurrentCustomers
        {
            get
            {
                var thisYear = db.SalesOrders
                    .Where(so => so.OrderDate.Year == DateTime.Now.Year)
                    .Select(so => so.Customer)
                    .Include(so => so.SalesOrders)  //--< Eager Loading <<<
                    .Distinct()
                    .ToArray()      //--< Converts them to POCOs exposing the added properties <<<
                    .OrderByDescending(so => so.TotalSales)
                    .ToList();
                return thisYear;
            }
        }
        public static List<CustomerSale> CustomerSales
        {
            get
            {
                return
                    (from sop in db.SalesOrderParts
                     group sop by new
                     {
                         sop.SalesOrder.CustomerId,
                         Customer = sop.SalesOrder.Customer.FirstName + " " +
                            sop.SalesOrder.Customer.LastName
                     } into custGroup
                     select new CustomerSale()
                     {
                         CustomerId = custGroup.Key.CustomerId,
                         CustomerName = custGroup.Key.Customer,
                         TotalSales = custGroup.Sum(cg => cg.Quantity * cg.UnitPrice)
                     }
                     ).ToList();
            }
        }
    }

    public class CustomerSale
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalSales { get; set; }
    }

}


