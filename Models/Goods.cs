using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YaroffShop.Models
{
    public class Goods
    {
        [Key]
        public int GOOD_ID { get; set; }
        public string? NAME { get; set; }
        public double PRICE { get; set; }
        public int QUANTITY { get; set; }
        public string? PRODUCER { get; set; }
        public string? DESCRIPTION { get; set; }

        [ForeignKey("Departments")]
        public decimal DEPT_ID { get; set; }
    }
}
