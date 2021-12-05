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
    public class ProductController : ControllerBase
    {
        IUnitOfWork unitOfWork;
        IGenericRepository<Product> ProductRepo;

        public ProductController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            ProductRepo = unitOfWork.GetProductRepository();
        }

        [HttpGet]

        public async Task<List<Product>> Get()
        {
            var list = await ProductRepo.GetAsync();
            return list.ToList();
        }
    }
}
