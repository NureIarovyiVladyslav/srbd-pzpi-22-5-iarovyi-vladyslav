using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YaroffShop.Models
{
    public class Workers
    {
        [Key]
        public int WORKERS_ID { get; set; }
        public string? NAME { get; set; }
        public string? ADRESS { get; set; }
        public string? INFORMATION { get; set; }

        [ForeignKey("Departments")]
        public decimal DEPT_ID { get; set; }

    }
}
