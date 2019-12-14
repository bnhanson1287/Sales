using System;

namespace eRaceSystem.POCO.Sales
{
    public class OrderDetails
    {
        public DateTime InvoiceDate { get; set; }
        public int EmployeeID { get; set; }
        public short SubTotal { get; set; }
        public short GST { get; set; }
        public short Total { get; set; }
    }
}
