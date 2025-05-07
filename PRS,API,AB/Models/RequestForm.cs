using System.ComponentModel.DataAnnotations;

namespace PRSBackendAB.Models;

    public class RequestForm
{
    [Required]
    public int UserID { get; set; }

    [Required]
    [StringLength(255)]
    public string Description { get; set; }

    [Required]
    [StringLength(255)]
    public string Justification { get; set; }

    [Required]
    public DateTime DateNeeded { get; set; }

    [Required]
    [StringLength(50)]
    public string DeliveryMode { get; set; }
}
