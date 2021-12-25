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
using Microsoft.AspNetCore.Hosting;

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
            if (UserResult.Status)
            {
                var response = await userRepository.LogIn(new LoginModel
                {
                    Email = signUpModel.Email,
                    Password = signUpModel.Password
                });

                UserResult.Token = response.Token;

                return Ok(UserResult);
            }
                

            return Unauthorized(UserResult);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            UserResult = await userRepository.LogIn(model);
            if (UserResult.Status)
                return Ok(UserResult);

            return Unauthorized();
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
                    
                    if(user.Image != null)
                    {
                        user.Image = url.Scheme + "://" + url.Host.Host + ":" + url.Host.Port + "/Files/" + user.Image;
                    }
                    return Ok(user);
                }
            }
            return Unauthorized();
        }

        [HttpPut("EditPatch")]
        [Authorize]
        public async Task<IActionResult> EditPatch(User document)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var email = GetEmailFromClaim(identity);
                User user = await userRepository.EditPatch(email, document);
                await unitOfWork.Save();

                if(user.Image != null)
                {
                    var url = HttpContext.Request;
                    user.Image = url.Scheme + "://" + url.Host.Host + ":" + url.Host.Port + "/Files/" + user.Image;
                }
                return Ok(user);
            }

            return Unauthorized();
            
        }

        [HttpPost("AddUserImage")]
        [Authorize]
        public async Task<IActionResult> AddUserImage()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var email = GetEmailFromClaim(identity);
                var user = await userRepository.GetUser(email);
                var httpRequest = HttpContext.Request;
                var userImage = httpRequest.Form.Files["image"];
                string imageName = null;

                if (userImage != null)
                {
                    imageName = new String(Path.GetFileNameWithoutExtension(userImage.FileName).Take(10).ToArray()).Replace(" ", "-");
                    imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(userImage.FileName);

                }

               var response = await userRepository.PutImage(email, imageName , userImage);
                await unitOfWork.Save();

                var url = HttpContext.Request;
                response.Image = url.Scheme + "://" + url.Host.Host + ":" + url.Host.Port + "/Files/" + response.Image;

                return Ok(response);
            }

            return Unauthorized();
        }

    }
}
