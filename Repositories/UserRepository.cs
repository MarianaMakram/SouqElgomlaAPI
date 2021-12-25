using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        /**UserManager : Microsoft Repository in identity core
         * To add , signUp , Login for  User*/

        UserManager<User> UserManager;
        RoleManager<IdentityRole> RoleManager;

        /**used to access json file of appsettings yo inclue information of JwtSecurityToken in RunTime*/
        public IConfiguration Configuration { get; set; }

        private readonly IWebHostEnvironment webHostEnvironment;
        public UserRepository(UserManager<User> userManager,
                              IConfiguration configuration, 
                              RoleManager<IdentityRole> roleManager,
                              IWebHostEnvironment hostEnvironment)
        {
            UserManager = userManager;
            Configuration = configuration;
            RoleManager = roleManager;
            webHostEnvironment = hostEnvironment;
        }

        public async Task<UserResultViewModel> SignUp(SignUpModel signUpModel)
        {
            var userExists = await UserManager.FindByEmailAsync(signUpModel.Email);
            if (userExists != null)
            {
                return new UserResultViewModel
                {
                    Status = false,
                    Message = "User already exists!"
                };
            }

            User Temp = signUpModel.ToModel();

            /**To hashing password and add it to User which we want to Create it (Temp)*/

            var Result = await UserManager.CreateAsync(Temp, signUpModel.Password);
            if (!Result.Succeeded)
            {
                List<string> errors = new List<string>();
                foreach (var item in Result.Errors)
                {
                    errors.Add(item.Description);
                }
                
                return new UserResultViewModel
                {
                    Status = false,
                    Message = "User creation failed! Please check user details and try again.",
                    Error = errors
                };
                
            }

            #region AddRole

            //bool IsRoleExists = await RoleManager.RoleExistsAsync(signUpModel.Role);
            //if (!IsRoleExists)
            //{
            //    var RoleResult = await RoleManager.CreateAsync(new IdentityRole(signUpModel.Role));
            //}

            //var UserRoleResult = await UserManager.AddToRoleAsync(Temp, signUpModel.Role);


            #endregion
            

            return new UserResultViewModel
            {
                Status = true,
                Message = "User creation successed",
            };

        }

        public async Task<UserResultViewModel> LogIn(LoginModel loginModel)
        {
            var user = await UserManager.FindByEmailAsync(loginModel.Email);
            
            if (user != null && await UserManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var Token = GenerateJwtToken(user);

                return new UserResultViewModel
                {
                    Status = true,
                    Token = Token.Result
                };
            }

            return new UserResultViewModel
            {
                Status = false,
                Message = "Invalid Email or password"
            };
        }

        private async Task<string> GenerateJwtToken(User user)
        {

            #region Create Security Token
            /**Create Security Token
             * Json Web Token
             */

            /**Encoding Secret in appsettings.json to Secret key
             * SymmetricSecurityKey included in cyriptography
             */
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]));

            /**Information about user be included in his token*/

            // Get User roles and add them to claims
            //var roles = await UserManager.GetRolesAsync(user);

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Email,user.Email),
                //new Claim(ClaimTypes.Role,roles[0]),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var Token = new JwtSecurityToken
                (
                    /**Who create Token (Web Api)*/
                    issuer: Configuration["JWT:ValidIssuer"],

                    /**How use and receive Token*/
                    audience: Configuration["JWT:ValidAudience"],

                    /**When this token will be expired*/
                    expires: DateTime.Now.AddDays(15),

                    signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256Signature),
                    claims: userClaims
                );
            #endregion

            /**to return Token as string*/
        
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        public async Task<User> GetUser(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            //if(user != null)
            //{
            //    return new ResultViewModel
            //    {
            //        Status = true,
            //        Data = user
            //    };
            //}
            //return new ResultViewModel
            //{
            //    Status = false
            //};
            return user;
        }

        public async Task<User> EditPatch(string email , User document)
        {
           User user = await UserManager.FindByEmailAsync(email);
            user.Name = document.Name;
            user.Email = document.Email;
            user.UserName = document.UserName;
            user.Address = document.Address;
            await UserManager.UpdateAsync(user);
           // document.ApplyTo(user);
            return user;
        }

        public async Task<User> PutImage(string email, string imageName, IFormFile userImage)
        {
            User user = await UserManager.FindByEmailAsync(email);
            user.Image = imageName;


            UploadedFile(user, userImage);

            await UserManager.UpdateAsync(user);
             
            return user;
        }

        private string UploadedFile(User model, IFormFile userImage)
        {
            string uniqueFileName = null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine("wwwroot", "Files");
                uniqueFileName = model.Image;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    userImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

    }
}
