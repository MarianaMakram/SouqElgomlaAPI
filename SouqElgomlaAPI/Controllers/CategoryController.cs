using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories;
using Models;
using ViewModels;

namespace SouqElgomlaAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        IUnitOfWork unitOfWork;
        IGenericRepository<Category> CategoryRepo;
        ResultViewModel result = new ResultViewModel();

        public CategoryController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            CategoryRepo = unitOfWork.GetCategoryRepository();
        }

        [HttpGet("")]
        public async Task<ResultViewModel> Get()
        {
            var list = await CategoryRepo.GetAsync();
            if(list == null)
            {
                result.Message = "There is no categeries";
            }
            else
            {
                List<CategoryModel> models = new List<CategoryModel>();
                foreach(var item in list)
                {
                    models.Add(item.ToCategoryModel());
                }

                result.Data = models;
            }
            return result;
            
        }

        [HttpGet("{id:int}")]
        public async Task<ResultViewModel> Get(int id)
        {
            var category = await CategoryRepo.GetByIDAsync(id);
            return new ResultViewModel
            {
                Status = true,
                Data = category
            };
        }

    }
}
