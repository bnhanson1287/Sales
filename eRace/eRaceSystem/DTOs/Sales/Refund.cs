using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.DTOs.Sales
{
    public class Refund
    {
        public int ProductID { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public  decimal Amount { get; set; }
        public bool IsRefundable { get; set; }
        public decimal RestockCharge { get; set; }
        public string Reason { get; set; }
        public int OriginalInvoiceID { get; set; }
    }
}
