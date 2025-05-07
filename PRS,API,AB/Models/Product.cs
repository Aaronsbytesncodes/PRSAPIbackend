using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRSBackendAB.models
{
    [Table("Product")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public  int ID {  get; set; }
        public  required int VendorID { get; set; }
        public required string PartNumber { get; set; }
        public required string Name { get; set; }
        public required int Price {  get; set; }
        public string?  Unit {  get; set; }
        public string? PhotoPath { get; set; }

    }
}
