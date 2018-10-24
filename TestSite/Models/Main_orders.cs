using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestSite.Models
{
    public class Simple_User
    {
        [Required, Key]
        public Guid ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public string Address { get; set; }
        public int Discount { get; set; }
    }

    public class Order
    {
        [Required, Key]
        public Guid ID { get; set; }

        [Required]
        public Guid Customer_ID { get; set; }

        [Required]
        public DateTime Order_Date { get; set; }

        public DateTime Shipment_Date { get; set; }

        public int Order_Number { get; set; }
        public string Status { get; set; }
    }

    public class Order_Element
    {
        [Required, Key]
        public Guid ID { get; set; }

        [Required]
        public Guid Order_ID { get; set; }

        [Required]
        public Guid Item_ID { get; set; }

        [Required]
        public int Items_Count { get; set; }

        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Item_Price { get; set; }
    }

    public class Product
    {
        [Required, Key]
        public Guid ID { get; set; }

        [Required]
        public string Code { get; set; }

        public string Name { get; set; }
        public int Price { get; set; }

        [StringLength(60)]
        public string Category { get; set; }
    }

}
