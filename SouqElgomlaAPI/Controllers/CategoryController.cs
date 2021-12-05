using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories;
using Models;

namespace SouqElgomlaAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        IUnitOfWork unitOfWork;
        IGenericRepository<Category> CategoryRepo;

        public CategoryController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            CategoryRepo = unitOfWork.GetCategoryRepository();
        }

        [HttpGet("")]
        public async Task<List<Category>> Get()
        {
            var list = await CategoryRepo.GetAsync();
            return list.ToList();
        }
    }
}
