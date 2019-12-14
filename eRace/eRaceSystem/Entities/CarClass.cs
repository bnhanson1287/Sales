namespace eRaceSystem.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    internal partial class CarClass
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CarClass()
        {
            Cars = new HashSet<Car>();
        }

        public int CarClassID { get; set; }

        [Required(ErrorMessage = "CarClassName is required")]
        [StringLength(30, ErrorMessage = "CarClassName cannot be longer then 30 characters")]
        public string CarClassName { get; set; }

        public decimal MaxEngineSize { get; set; }

        [Required(ErrorMessage = "CertificationLevel is required")]
        [StringLength(1, ErrorMessage = "CertificationLevel cannot be longer then 1 characters")]
        public string CertificationLevel { get; set; }

        [Column(TypeName = "money")]
        public decimal RaceRentalFee { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(255, ErrorMessage = "Description cannot be longer then 225 characters")]
        public string Description { get; set; }

        public virtual Certification Certification { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Car> Cars { get; set; }
    }
}
