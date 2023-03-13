using Microsoft.EntityFrameworkCore;
using RestFulApi.Data;
using RestFulApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Services
{
    public class ProductReposity : IProductReposity
    {
        private readonly MyDBContext _context;
        public static int PAGE_SIZE { get; set; } = 5;

        public ProductReposity(MyDBContext context)
        {
            _context = context;
        }
        public List<MProduct> GetAll(string search, double? from, double? to, string sortBy, int page = 1)
        {

            var allProducts = _context.Products.Include(pro => pro.Category).AsQueryable();

            #region filtering
            if (!string.IsNullOrEmpty(search))
            {
                 allProducts = _context.Products.Where(Pro => Pro.ProductName.Contains(search));
            }
            if (from.HasValue)
            {
                allProducts = allProducts.Where(Pro => Pro.ProductPrice >= from);
            }
            if (to.HasValue)
            {
                allProducts = allProducts.Where(Pro => Pro.ProductPrice <= to);
            }
            #endregion

            #region Sorting

            // Default sort by Name (ProductName)

            allProducts = allProducts.OrderBy(pro => pro.ProductName);

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "pro_desc": 
                        allProducts = allProducts.OrderByDescending(pro => pro.ProductName);
                        break;
                    case "price_asc":
                        allProducts = allProducts.OrderBy(pro => pro.ProductPrice);
                        break;
                    case "price_desc":
                        allProducts = allProducts.OrderByDescending(pro => pro.ProductPrice);
                        break;
                }
            }
            #endregion

            //#region Paging

            //allProducts = allProducts.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);                 
            //#endregion


            //var result = allProducts.Select(Pro => new MProduct
            //{
            //    ProductCode = Pro.ProductCode,
            //    ProductName = Pro.ProductName,
            //    ProductPrice = Pro.ProductPrice,
            //    Category = Pro.Category.CategoryName
            //});

            //return result.ToList();


            var result = PaginatedList<Products>.Create(allProducts, page, PAGE_SIZE);

            return result.Select(Pro => new MProduct
            {
                ProductCode = Pro.ProductCode,
                ProductName = Pro.ProductName,
                ProductPrice = Pro.ProductPrice,
                Category = Pro.Category?.CategoryName
            }).ToList();
            
        }

        
    }
}
