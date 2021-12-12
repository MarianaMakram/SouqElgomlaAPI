using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories;
using ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.JsonPatch;
using Models;

namespace SouqElgomlaAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUnitOfWork unitOfWork;
        IUserRepository userRepository;
        ResultViewModel result = new ResultViewModel();
        public UserController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            userRepository = unitOfWork.GetUserRepository();
        }

        private string GetEmailFromClaim(ClaimsIdentity claimsIdentity)
        {
            IEnumerable<Claim> claims = claimsIdentity.Claims;
            var email = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
            return email.Value;
        }
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpModel signUpModel)
        {
            result = await userRepository.SignUp(signUpModel);
            if (result.Status)
                return Ok(result);

            return Unauthorized(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            result = await userRepository.LogIn(model);
            //if (result.Status)
                return Ok(result);

           // return Unauthorized();
        }

        [HttpGet("GetUser")]
        [Authorize]
        public async Task<IActionResult> GetUser()
        {
            /*to get user email from token which added as claim*/

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var email = GetEmailFromClaim(identity);
                var Response = await userRepository.GetUser(email);

                if (Response.Status)
                {
                    return Ok(Response.Data);
                }
            }
            return Unauthorized();
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> EditPatch(JsonPatchDocument document)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var email = GetEmailFromClaim(identity);
                User user = await userRepository.EditPatch(email, document);
                await unitOfWork.Save();
                return Ok(user);
            }

            return Unauthorized();
            
        }
    }
}
