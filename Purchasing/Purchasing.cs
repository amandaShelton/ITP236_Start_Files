#define stub
#undef stub

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchasing
{
    class Purchasing
    {
#if stub
        static PurchasingEntities db = new PurchasingEntities();
#endif
        public static List<VendorPurchase> VendorPurchases
        {
            get
            {
                //--< Replace the following line with your code >--//
                return new List<VendorPurchase>();                    
            }
        }
    }
    public class VendorPurchase
    {
        public int VendorId { get; set; }
        public string Vendor { get; set; }
        public decimal TotalPurchases { get; set; }
    }
    public partial class Part
    {
#if stub
        static PurchasingEntities db = new PurchasingEntities();
#endif
        public static List<PartPurchase> PartPurchases
        {
            get
            {
                //--< Replace the following line with your code >--//
                return new List<PartPurchase>();
            }
        }
    }
    public class PartPurchase
    {
        public int PartId { get; set; }
        public string Part { get; set; }
        public decimal TotalPurchases { get; set; }
    }
}
