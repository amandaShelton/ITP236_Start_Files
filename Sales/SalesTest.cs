using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Data.Entity.Core.Objects;

namespace Sales
{
    public class SalesTest
    {
        public static void Test()
        {
            //TestCriticalThinking();
            //TestView();
            //TestFunction();
            //TestStoredProcedure();
        }
        private static void TestCriticalThinking()
        {
            /*
             * Can an object can be retrieved, updated, deleted, and re-inserted in the 
             * same SaveChanges( ) execution?
             */
            
            
        }
        private static void TestView()
        {

            using (var db = new SalesEntities())
            {
                var summaries = db.SoSummaries;
                foreach (var summary in summaries)
                {
                    Console.WriteLine($"{summary.SalesOrderNumber}\t{summary.Customer}\t{summary.TotalAmount:c}");
                }
            }

        }
        private static void TestFunction()
        {

            using (var db = new SalesEntities())
            {
                var details = db.SoDetail(1);
                foreach (var detail in details)
                {
                    Console.WriteLine($"{detail.SalesOrderNumber}" +
                        $"\t{detail.Customer}" +
                        $"\t{detail.Part}" +
                        $"\t{detail.Quantity}" +
                        $"\t{detail.OrderTotal:c}");
                }

        }
        private static void TestStoredProcedure()
        {

            int soNum = 1;
            Console.WriteLine("Before executing stored procedure.");
            using (var db = new SalesEntities())
            {
                var so = db.SalesOrders.Find(soNum);
                //so.OrderTotal = 0;
                //db.SaveChanges();

                Console.WriteLine($"{so.SalesOrderNumber}\t{so.OrderTotal:c}");
                ObjectParameter orderTotalParm = new ObjectParameter("OrderTotal", 0);
                db.SOP(soNum, orderTotalParm);
            }
            Console.WriteLine("\n\nAfter executing stored procedure.");
            using (var db = new SalesEntities())
            {
                var so = db.SalesOrders.Find(soNum);
                Console.WriteLine($"{so.SalesOrderNumber}\t{so.OrderTotal:c}");
            }

        }
    }
}
