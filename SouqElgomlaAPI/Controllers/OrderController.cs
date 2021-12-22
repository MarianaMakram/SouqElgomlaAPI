using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ViewModels;

namespace SouqElgomlaAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IUnitOfWork unitOfWork;
        IGenericRepository<Order> OrderRepo;
        IGenericRepository<ProductOrder> ProductOrderRepo;
        IGenericRepository<Product> ProductRepo;
        IUserRepository userRepository;

        public OrderController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            OrderRepo = unitOfWork.GetOrderRepository();
            ProductOrderRepo = unitOfWork.GetProductOrderRepository();
            ProductRepo = unitOfWork.GetProductRepository();
            userRepository = unitOfWork.GetUserRepository();
        }

        #region post order

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(IEnumerable<OrderviewModel> models)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if(identity!= null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var email = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
                var user = await userRepository.GetUser(email.Value);

                if(user != null)
                {
                    Order order = new Order
                    {
                        UserId = user.Id,
                        OrderDate = DateTime.Now,
                        State = OrderDeliveredState.Pending 
                    };

                    var recordOrder = await OrderRepo.Add(order);
                    await unitOfWork.Save();

                    foreach(var item in models)
                    {
                        Product product = await ProductRepo.GetByIDAsync(item.productID);

                        if(product != null)
                        {
                            product.Quantity -= item.quantity;

                            await ProductRepo.Update(product);
                            await unitOfWork.Save();

                            ProductOrder productOrder = new ProductOrder
                            {
                                OrderID = recordOrder.ID,
                                ProductID = item.productID,
                                Quantity = item.quantity
                            };

                            await ProductOrderRepo.Add(productOrder);
                            await unitOfWork.Save();
                        } 
                    }
                    return Ok();
                }
            }

            return Unauthorized();
        }

        #endregion
    }
}
