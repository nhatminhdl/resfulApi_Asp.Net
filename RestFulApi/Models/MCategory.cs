using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Models
{
    public class MCategory
    {
        [Required]
        [MaxLength(100)]

        public string CategoryName { get; set; }
    }
}
