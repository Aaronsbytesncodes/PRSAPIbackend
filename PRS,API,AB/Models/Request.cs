using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRS_API_AB.Models
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
        public string? DeliveryMode { get; set; }
        public string? Status { get; set; }
        public decimal? Total { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string? ReasonForRejection { get; set; }
    }
}
