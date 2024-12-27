using System.ComponentModel.DataAnnotations;

namespace YaroffShop.Models
{
    public class Departments
    {
        [Required]
        public decimal DEPT_ID { get; set; }

        [Required]
        [StringLength(20)]
        public string? NAME { get; set; }

        [StringLength(40)]
        public string? INFO { get; set; }
    }
}
