using RestFulApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestFulApi.Services
{
    public interface ICategoryReposity
    {
        List<VMCategory> GetAll();
        VMCategory GetById(int id);

        VMCategory Insert(VMCategory category);
        void Update(VMCategory category);

        void Delete(int id);


    }
}
