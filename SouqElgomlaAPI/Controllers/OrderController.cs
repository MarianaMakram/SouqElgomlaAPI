using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
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

        [HttpPost("CheckOut")]
        [Authorize]
        public async Task<IActionResult> Post(PostOrderModel orderModel)
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
                        State = OrderDeliveredState.Pending,
                        Name = orderModel.name,
                        Address = orderModel.address,
                        Phone = orderModel.phone
                    };

                    if(orderModel.payment == 1)
                    {
                        order.PaymentType = PaymentType.Cash;
                    }
                    else if(orderModel.payment == 2)
                    {
                        order.PaymentType = PaymentType.PayPal;
                    }

                    var recordOrder = await OrderRepo.Add(order);
                    await unitOfWork.Save();

                    foreach (var item in orderModel.orderviewModels)
                    {
                        Product product = await ProductRepo.GetByIDAsync(item.productID);

                        if (product != null)
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
                    return Ok(recordOrder.ID);
                }
            }

            return Unauthorized();
        }

        #endregion

        #region Get order

        [HttpGet("GetOrder")]
        [Authorize]

        public async Task<IActionResult> Get()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if(identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var email = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
                var user = await userRepository.GetUser(email.Value);

                if(user != null)
                {
                    IList<OrderResultViewModel> orderResults = new List<OrderResultViewModel>();

                    IEnumerable<Order> orders = await OrderRepo.GetAsync();
                    orders = orders.Where(order => order.UserId == user.Id).ToList();

                    #region product order reference loop

                    IEnumerable<ProductOrder> productorders = await ProductOrderRepo.GetAsync();
                    productorders = productorders.ToList();

                    foreach (var iterator in orders)
                    {
                        var productorder = productorders.Where(item => item.Order == iterator);

                        OrderResultViewModel orderResult = new OrderResultViewModel {
                            orderId = iterator.ID,
                            userId = iterator.UserId,
                            orderDate = iterator.OrderDate,
                            state = ((int)iterator.State),
                            paymentType = ((int)iterator.PaymentType),
                            name = iterator.Name,
                            address = iterator.Address,
                            phone = iterator.Phone
                        };

                        IList<ProductOrderViewModel> productOrderView = new List<ProductOrderViewModel>();
                        foreach (var prodOrder in productorder)
                        {
                            ProductOrderViewModel productView = new ProductOrderViewModel();
                            productView.productOrderId = prodOrder.ID;
                            productView.productID = prodOrder.ProductID;
                            productView.quantity = prodOrder.Quantity;

                            productOrderView.Add(productView);
                        }

                        orderResult.productOrderViewModels = productOrderView;
                        orderResults.Add(orderResult);
                    }
                    #endregion
                    return Ok(orderResults);
                }
            }
            return Unauthorized();
        }
        #endregion
    }
}
