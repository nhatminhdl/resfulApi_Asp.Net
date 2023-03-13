using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Data
{
    [Table("Category")]
    public class Category
    {   
        [Key]
        public int CategoryCode { get; set; }

        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        public virtual ICollection<Products> Products { get; set; }
        
    }
}
