using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories;
using Models;
using ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SouqElgomlaAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        IUnitOfWork unitOfWork;
        IGenericRepository<Category> CategoryRepo;
        IGenericRepository<Product> productRepo;
        ResultViewModel result = new ResultViewModel();

        public CategoryController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            CategoryRepo = unitOfWork.GetCategoryRepository();
            productRepo = unitOfWork.GetProductRepository();
        }

        [HttpGet("")]
        public async Task<ResultViewModel> Get()
        {
            var list = await CategoryRepo.GetAsync();
            if(list == null)
            {
                result.Status = false;
                result.Message = "There is no categeries";
            }
            else
            {
                var allProducts = await productRepo.GetAsync();
                foreach(var iterator in list.ToList())
                {
                    iterator.Products = allProducts.ToList().FindAll(item => item.CategoryID == iterator.ID);
                }

                list = list.Where(itemList => itemList.Products != null);
                result.Status = true;
                result.Data = list;
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
