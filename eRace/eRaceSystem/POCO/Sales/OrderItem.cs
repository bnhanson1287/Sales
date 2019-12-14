namespace eRaceSystem.POCO.Sales
{
    public class OrderItem
    {
        public int ProductID { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public short ItemPrice { get; set; }
        public short Amount { get; set; } // Calc: ItemPrice * Quantity
    }
}
