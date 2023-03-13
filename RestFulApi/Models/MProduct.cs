using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Models
{
    public class MProduct
    {
        public Guid ProductCode { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public string Category { get; set; }
    }
}
