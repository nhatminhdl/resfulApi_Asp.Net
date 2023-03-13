using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Data
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int Id { get; set; }
        [Required]

        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(250)]
        public string PassWord { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
