using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRaceSystem.DTOs.Sales
{
    public class CartItems
    {
        public int EmployeeID { get; set; }
        public int ProductID { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal Amount { get; set; }
    }
}
