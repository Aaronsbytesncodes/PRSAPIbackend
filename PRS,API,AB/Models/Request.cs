using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRSBackendAB.models
{
    [Table("Request")]
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; } // Primary Key
        public int UserID { get; set; } // Foreign Key

        public string RequestNumber { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Justification { get; set; }
        public DateTime? DateNeeded { get; set; }
        public string? DeliveryMode { get; set; } = "Pickup";
        public string? Status { get; set; } = "NEW";
        public decimal Total { get; set; } = 0;
        public DateTime? SubmittedDate { get; set; }
        public string? ReasonForRejection { get; set; }
    }
}


