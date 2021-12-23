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
using System.Security.Claims;
using System.IO;

namespace SouqElgomlaAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    
    public class ProductController : ControllerBase
    {
        IUnitOfWork unitOfWork;
        IGenericRepository<Product> ProductRepo;
        IGenericRepository<RetailerReviewProduct> ProductReviewRepo;
        IUserRepository userRepository;

        ResultViewModel result = new ResultViewModel();

        public ProductController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            ProductRepo = unitOfWork.GetProductRepository();
            ProductReviewRepo = unitOfWork.GetProductReview();
            userRepository = unitOfWork.GetUserRepository();
        }

        #region Get All products
        [HttpGet]
        public async Task<ResultViewModel> Get()
        {
            var list = await ProductRepo.GetAsync();
            /*if there is no products*/
            if (list == null)
            {
                result.Status = false;
                result.Message = "There is no Products";
            }
            else
            {
                result.Status = true;
                List<ProductModel> productModels = new List<ProductModel>();
                list = list.Where(item => item.Quantity > 0 && item.IsApproved==true);

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

        #endregion

        #region Get product by id
        [HttpGet("{id:int}")]
        public async Task<ResultViewModel> Get(int? id)
        {
            var url = HttpContext.Request;
            
            string schema;
            if (url.IsHttps)
            {
               schema = "https";
            }
            else
            {
                schema = "http";
            }
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
                    var ProductRate = ProductRateList.Sum(i => i.Rate)/ ProductRateList.Count;

                    //Temp.Image = schema + "://" + url.Host.Host + ":" + url.Host.Port + "/Files/" +Temp.Image;
                    result.Data = Temp.ToProductModel(ProductRate);
                }
            }

            return result;
        }

        #endregion

        #region Get products by category ID
        [HttpGet("GetProdByCatID/{categoryID:int}")]
        public async Task<ResultViewModel> GetProdByCatID(int categoryID)
        {
            var response = await Get();
            if (response.Status)
            {
                var data = (List<ProductModel>)response.Data;
                data = data.FindAll(item => item.CategoryId == categoryID);
                response.Data = data;
            }
            return response;
        }
        #endregion

        #region Add product
        [HttpPost]
        [Authorize(Roles = "Supplier")]
        public async Task<ResultViewModel> Post()
        {
            Product product = new Product();
            var httpRequest = HttpContext.Request;

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var email = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
                var user = await userRepository.GetUser(email.Value);
                if (user !=null)
                {
                    /*get product data from FormData*/

                    product.Name = httpRequest.Form.Files["prodName"].ToString();
                    product.Price = Convert.ToDouble(httpRequest.Form.Files["prodPrice"].ToString());
                    product.Quantity = Convert.ToInt32(httpRequest.Form.Files["prodPrice"].ToString());
                    product.Description = httpRequest.Form.Files["prodDescription"].ToString();
                    product.UnitWeight = httpRequest.Form.Files["prodUnitWeight"].ToString();
                    product.ExpireDate = Convert.ToDateTime(httpRequest.Form.Files["prodExpireDate"].ToString());
                    product.ProductionDate = Convert.ToDateTime(httpRequest.Form.Files["prodProductionDate"].ToString());
                    product.CategoryID = Convert.ToInt32(httpRequest.Form.Files["prodCategoryID"].ToString());

                    var ProdImage = httpRequest.Form.Files["ProdImage"];
                    string imageName = null;
                    if (ProdImage != null)
                    {
                        imageName = new String(Path.GetFileNameWithoutExtension(ProdImage.FileName).Take(10).ToArray()).Replace(" ", "-");
                        imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(ProdImage.FileName);
                    }
                    product.ImageUrl = imageName;
                    product.UserId = user.Id;
                    product.IsApproved = false;
                    await ProductRepo.Add(product);
                    await unitOfWork.Save();
                    result.Status = true;
                    return result;
                }
            }
            result.Status = false;
            return result;
        }

        #endregion

        #region patch product
        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Supplier")]
        public async Task<Product> UpdatePatch(int id,JsonPatchDocument document)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var email = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
                var user = await userRepository.GetUser(email.Value);
                if (user != null)
                {
                    await ProductRepo.UpdatePatch(id, document);
                    await unitOfWork.Save();
                    return await ProductRepo.GetByIDAsync(id);
                }
            }
            return null;
        }

        #endregion

        #region Get products for specific user supplier

        [HttpGet("GetProductsForUser")]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> GetProductsForUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var email = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
                var user = await userRepository.GetUser(email.Value);
                if (user != null)
                {
                    var list = await ProductRepo.GetAsync();
                    var data = list.ToList().FindAll(item => item.UserId == user.Id);

                    result.Status = true;
                    result.Data = data;
                    
                }
                result.Status = false;
                result.Message = "there is no products";

                return Ok(result);
            }
            return Unauthorized();
        }
        #endregion

        #region Edit product Quantity
        /*
         * will not used because this action will done in order controller
         * edit product quantity by decrement it according to number of selled items
         */

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Supplier")]

        public async Task<IActionResult> Put(int id, int quantity)
        {
            var product = await ProductRepo.GetByIDAsync(id);
            product.Quantity -= quantity;
            await ProductRepo.Update(product);
            await unitOfWork.Save();
            return Ok();
        }
        #endregion
    }
}
