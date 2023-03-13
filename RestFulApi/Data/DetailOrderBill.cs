using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Data
{
    public class DetailOrderBill
    {
        public Guid ProductCode { get; set; }
        public Guid OrderCode { get; set; }

        public int Quantity { get; set; }
        public double Price { get; set; }
        public byte Discount { get; set; }

        //Relationship

        public OrderBill OrderBill { get; set; }
        public Products Products { get; set; }
    }
}
