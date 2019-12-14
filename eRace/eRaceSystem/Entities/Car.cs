namespace eRaceSystem.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    internal partial class Car
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Car()
        {
            RaceDetails = new HashSet<RaceDetail>();
        }

        public int CarID { get; set; }

        [Required(ErrorMessage = "SerialNumber is required")]
        [StringLength(15, ErrorMessage = "SerialNumber cannot be longer then 15 characters")]
        public string SerialNumber { get; set; }

        [Required(ErrorMessage = "Ownership is required")]
        [StringLength(6, ErrorMessage = "Ownership cannot be longer then 6 characters")]
        public string Ownership { get; set; }

        public int CarClassID { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(10, ErrorMessage = "State cannot be longer then 10 characters")]
        public string State { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(255, ErrorMessage = "Description cannot be longer then 225 characters")]
        public string Description { get; set; }

        public int? MemberID { get; set; }

        public virtual CarClass CarClass { get; set; }

        public virtual Member Member { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RaceDetail> RaceDetails { get; set; }
    }
}
