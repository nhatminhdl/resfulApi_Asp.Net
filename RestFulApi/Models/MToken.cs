using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Models
{
    public class MToken
    {
        public string AccessToken { get; set; }
        public string  RefreshToken { get; set; } 
    }
}
