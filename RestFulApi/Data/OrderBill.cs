using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Data
{
    public enum OrderStatus
    {
        New = 0, Payment = 1, Complete = 2, Cancel = -1
    }
    public class OrderBill
    {

        public Guid OrderCode { get; set; }
        public DateTime DateOrder { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string Receiver { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<DetailOrderBill> DetailOrderBills { get; set; }

        public OrderBill()
        {
            DetailOrderBills = new List<DetailOrderBill>();
        }
    }
}
