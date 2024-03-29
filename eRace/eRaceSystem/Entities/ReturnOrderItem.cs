namespace eRaceSystem.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    internal partial class ReturnOrderItem
    {
        public int ReturnOrderItemID { get; set; }

        public int ReceiveOrderID { get; set; }

        public int? OrderDetailID { get; set; }

        [StringLength(50, ErrorMessage = "UnOrderedItem cannot be longer then 50 characters")]
        public string UnOrderedItem { get; set; }

        public int ItemQuantity { get; set; }

        [StringLength(100, ErrorMessage = "Comment cannot be longer then 100 characters")]
        public string Comment { get; set; }

        [StringLength(25, ErrorMessage = "VendorProductID cannot be longer then 25 characters")]
        public string VendorProductID { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }

        public virtual ReceiveOrder ReceiveOrder { get; set; }
    }
}
