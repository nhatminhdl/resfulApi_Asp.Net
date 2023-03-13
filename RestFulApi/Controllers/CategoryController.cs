using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestFulApi.Data;
using RestFulApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly MyDBContext _context;

        public CategoryController(MyDBContext context)
        {
            _context = context;
        }

        [HttpGet]

        public IActionResult GetAll()
        {
            var listCategory = _context.Categories.ToList();
            return Ok(listCategory);
        }

        [HttpGet("{id}")]

        public IActionResult GetById(int id)
        {
            var listCategory = _context.Categories.SingleOrDefault(category => 
            category.CategoryCode == id);

            if(listCategory != null)
            {
                return Ok(listCategory);
            }
            else
            {
                return NotFound();

            }
            
        }

        [HttpPost]
        [Authorize]

        public IActionResult createNew(MCategory model)
        {
            try
            {
                var category = new Category
                {
                    CategoryName = model.CategoryName
                };
                _context.Add(category);
                _context.SaveChanges();

                return Ok(category);
            }
            catch
            {
                return BadRequest();
            }
       
        }

        [HttpPut("{id}")]

        public IActionResult UpdateCategoryById(int id, MCategory model)
        {
            var listCategory = _context.Categories.SingleOrDefault(category =>
            category.CategoryCode == id);

            if (listCategory != null)
            {
                listCategory.CategoryName = model.CategoryName;
                _context.SaveChanges();
                return Ok(listCategory);
            }
            else
            {
                return NotFound();

            }

        }

        [HttpDelete("{id}")]

        public IActionResult DeleteCategoryById(int id, MCategory model)
        {
            var Category = _context.Categories.SingleOrDefault(category =>
            category.CategoryCode == id);

            if (Category != null)
            {
                Category.CategoryName = model.CategoryName;
                _context.Remove(Category);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();

            }

        }
    }
}
