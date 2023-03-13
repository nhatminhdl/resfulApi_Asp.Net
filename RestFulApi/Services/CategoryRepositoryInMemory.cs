using RestFulApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Services
{
    public class CategoryRepositoryInMemory : ICategoryReposity
    {
        static List<VMCategory> categories = new List<VMCategory>
        {
            new VMCategory {CategoryCode = 1, CategoryName = "Tea" },
            new VMCategory {CategoryCode = 2, CategoryName = "frigde" },
            new VMCategory {CategoryCode = 3, CategoryName = "hat" },
            new VMCategory {CategoryCode = 4, CategoryName = "floor" },
            new VMCategory {CategoryCode = 5, CategoryName = "model" },
        };
        public void Delete(int id)
        {
            var _category = categories.SingleOrDefault(cate => cate.CategoryCode == id);
            categories.Remove(_category);

        }

        public List<VMCategory> GetAll()
        {
            return categories;
        }

        public VMCategory GetById(int id)
        {
            return categories.SingleOrDefault(cate => cate.CategoryCode == id);
        }

        public VMCategory Insert(VMCategory category)
        {
            var _category = new VMCategory
            {
                CategoryCode = categories.Max(cate => cate.CategoryCode) + 1,
                CategoryName = category.CategoryName

            };

            categories.Add(_category);
            return _category;
            
        }

        public void Update(VMCategory category)
        {
            var _category = categories.SingleOrDefault(cate => cate.CategoryCode == category.CategoryCode);

            _category.CategoryName = category.CategoryName;
            


                
        }
    }
}
