using RestFulApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Services
{
    public interface IProductReposity
    {
        List<MProduct> GetAll(string search, double? from, double? to, string sortBy, int page = 1);
    }
}
