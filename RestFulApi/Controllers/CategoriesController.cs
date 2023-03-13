using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestFulApi.Models;
using RestFulApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryReposity _categoryRepository;

        public CategoriesController(ICategoryReposity categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_categoryRepository.GetAll());
            }catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
               var data = _categoryRepository.GetById(id);
                if(data != null)
                {
                    return Ok(data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, VMCategory model)
        {
            if(id != model.CategoryCode)
            {
                return NotFound();
            }
            try
            {
               _categoryRepository.Update(model);
                return StatusCode(StatusCodes.Status200OK);
               
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
           
            try
            {
                _categoryRepository.Delete(id);
                return Ok();

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Insert(VMCategory model)
        {
            try
            {
                var data = _categoryRepository.Insert(model);
                return Ok();

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
