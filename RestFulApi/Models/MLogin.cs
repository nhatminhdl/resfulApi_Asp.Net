using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Models
{
    public class MLogin
    {

        [Required]

        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(250)]
        public string PassWord { get; set; }
    }
}
