using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories;
using Models;
using ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

namespace SouqElgomlaAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    
    public class ProductController : ControllerBase
    {
        IUnitOfWork unitOfWork;
        IGenericRepository<Product> ProductRepo;
        IGenericRepository<RetailerReviewProduct> ProductReviewRepo;

        ResultViewModel result = new ResultViewModel();

        public ProductController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            ProductRepo = unitOfWork.GetProductRepository();
            ProductReviewRepo = unitOfWork.GetProductReview();
        }

        [HttpGet]
        public async Task<ResultViewModel> Get()
        {
            var list = await ProductRepo.GetAsync();
            if (list == null)
            {
                result.Message = "There is no categeries";
            }
            else
            {
                List<ProductModel> productModels = new List<ProductModel>();

                foreach(var item in list.ToList())
                {
                    var RateList = (await ProductReviewRepo.GetAsync()).ToList().FindAll(i => i.ProductID == item.ID);
                    var ProductRate = RateList.Sum(i => i.Rate);
                    var count = RateList.Count;
                    if (count == 0)
                    {
                        ProductRate = 0;
                    }
                    else
                    {
                        ProductRate = ProductRate / count;
                    }
                    productModels.Add(item.ToProductModel(ProductRate));
                }
                result.Data = productModels;
            }
            
            return result;
        }

        [HttpGet("{id:int}")]
        public async Task<ResultViewModel> Get(int? id)
        {
            if (id == null || id <= 0)
            {
                result.Message = " Invalid Id";
            }
            else
            {
                Product Temp = await ProductRepo.GetByIDAsync(id.Value);

                if (Temp == null)
                {
                    result.Message = "There is no user with this Id";
                }
                else
                {
                    var ProductRateList = (await ProductReviewRepo.GetAsync()).ToList()
                                        .FindAll(i => i.ProductID == id);
                    var ProductRate = ProductRateList.Sum(i => i.Rate)/ ProductRateList.Count();
                    
                    result.Data = Temp.ToProductModel(ProductRate);
                }
            }

            return result;
        }

        [HttpPost]
        [Authorize(Roles = "Supplier")]
        public async Task<ResultViewModel> Post(Product product)
        {
            await ProductRepo.Add(product);
            await unitOfWork.Save();
            result.Data = await ProductRepo.GetAsync();
            return result;
        }

        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Supplier")]
        public async Task<Product> UpdatePatch(int id,JsonPatchDocument document)
        {
            await ProductRepo.UpdatePatch(id, document);
            await unitOfWork.Save();
            return await ProductRepo.GetByIDAsync(id);
        }

    }
}
