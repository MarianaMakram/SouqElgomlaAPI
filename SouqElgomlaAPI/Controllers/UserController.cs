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
using System.IO;

namespace SouqElgomlaAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUnitOfWork unitOfWork;
        IUserRepository userRepository;
        ResultViewModel result = new ResultViewModel();
        UserResultViewModel UserResult = new UserResultViewModel();
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
            UserResult = await userRepository.SignUp(signUpModel);
            if (result.Status)
                return Ok(result);

            return Unauthorized(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            UserResult = await userRepository.LogIn(model);
            //if (result.Status)
                return Ok(UserResult);

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
                var user = await userRepository.GetUser(email);

                if (user != null)
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
                    if(user.Image != null)
                    {
                        user.Image = schema + "://" + url.Host.Host + ":" + url.Host.Port + "/Files/" + user.Image;
                    }
                    return Ok(user);
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddUserImage()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var email = GetEmailFromClaim(identity);
                var user = await userRepository.GetUser(email);
                var httpRequest = HttpContext.Request;
                var userImage = httpRequest.Form.Files["userImage"];
                string imageName = null;

                if (userImage != null)
                {
                    imageName = new String(Path.GetFileNameWithoutExtension(userImage.FileName).Take(10).ToArray()).Replace(" ", "-");
                    imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(userImage.FileName);
                }
                var response = await userRepository.PutImage(email, imageName);

                return Ok(response);
            }

            return Unauthorized();
        }
    }
}
