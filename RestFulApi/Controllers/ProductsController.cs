using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestFulApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductReposity _productReposity;

        public ProductsController(IProductReposity productReposity)
        {
            _productReposity = productReposity;
        }

        [HttpGet]
        public IActionResult GetAll(string search,  double? from, double? to, string sortBy, int page = 1)
        {
            try {

                var result = _productReposity.GetAll(search, from, to, sortBy, page);
                return Ok(result);
            } catch
            {
                return BadRequest("We cant get product!");
            }
        }
    }
}
