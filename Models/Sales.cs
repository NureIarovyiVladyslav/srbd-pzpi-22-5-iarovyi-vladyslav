using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YaroffShop.Models
{
    public class Sales
    {
        [Key]
        public int SALE_ID { get; set; }
        public int CHECK_NO { get; set; }
        public int QUANTITY { get; set; }
        public DateTime DATE_SALE { get; set; }

        [ForeignKey("Goods")]
        public int GOOD_ID { get; set; }
    }
}
