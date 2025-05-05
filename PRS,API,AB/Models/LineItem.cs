

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRS_API_AB.Models
{
    [Table("LineItem")]
    public class LineItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RequestID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
