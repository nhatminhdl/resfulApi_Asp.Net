using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Data
{
    [Table("Products")]
    public class Products
    {
        [Key]
        public Guid ProductCode { get; set; }

        [Required]

        [MaxLength(100)]
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        [Range(0, double.MaxValue)]
        public double ProductPrice { get; set; }

        public byte ProductDiscount { get; set; }

        public int? CategoryCode { get; set; }

        [ForeignKey("CategoryCode")]

        public Category Category { get; set; }

        public ICollection<DetailOrderBill> DetailOrderBills { get; set; }

        public Products()
        {
            DetailOrderBills = new List<DetailOrderBill>();
        }
    }
}

