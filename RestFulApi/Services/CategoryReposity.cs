using RestFulApi.Data;
using RestFulApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Services
{
    public class CategoryReposity : ICategoryReposity
    {
        private readonly MyDBContext _context;

        public CategoryReposity(MyDBContext context)
        {
            _context = context;
        }
        public void Delete(int id)
        {
            var category = _context.Categories.SingleOrDefault(cate => cate.CategoryCode == id);
            if (category != null)
            {
                _context.Remove(category);
                _context.SaveChanges();

            } 
        }

        public List<VMCategory> GetAll()
        {
            var catgories = _context.Categories.Select(cate => new VMCategory { 
                CategoryCode = cate.CategoryCode,
                CategoryName = cate.CategoryName
            });

            return catgories.ToList();
        }

        public VMCategory GetById(int id)
        {
            var category = _context.Categories.SingleOrDefault(cate => cate.CategoryCode == id);
            if (category != null)
            {
                return new VMCategory
                {
                    CategoryCode = category.CategoryCode,
                    CategoryName = category.CategoryName
                };
            }

            return null;
        }

        public VMCategory Insert(VMCategory category)
        {
            var _category = new Category
            {
                CategoryName = category.CategoryName
            };

            _context.Add(_category);
            _context.SaveChanges();

            return new VMCategory
            {
                CategoryCode = _category.CategoryCode,
                CategoryName = _category.CategoryName
            };
        }

        public void Update( VMCategory category)
        {
            var _category = _context.Categories.SingleOrDefault(cate => cate.CategoryCode == category.CategoryCode);
            
                _category.CategoryName = category.CategoryName;
                _context.SaveChanges();
           
           
        

        }

      
    }
}
