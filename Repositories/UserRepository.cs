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

        public UserRepository(UserManager<User> userManager,
                              IConfiguration configuration, 
                              RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            Configuration = configuration;
            RoleManager = roleManager;
        }

        public async Task<ResultViewModel> SignUp(SignUpModel signUpModel)
        {
            var userExists = await UserManager.FindByNameAsync(signUpModel.UserName);
            if (userExists != null)
            {
                return new ResultViewModel
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
                return new ResultViewModel { 
                    Status = false, 
                    Message = "User creation failed! Please check user details and try again."
                };
            }

            #region AddRole

            bool IsRoleExists = await RoleManager.RoleExistsAsync(signUpModel.Role);
            if (!IsRoleExists)
            {
                var RoleResult = await RoleManager.CreateAsync(new IdentityRole(signUpModel.Role));
            }

            var UserRoleResult = await UserManager.AddToRoleAsync(Temp, signUpModel.Role);


            #endregion

            var Token = GenerateJwtToken(Temp);

            return new ResultViewModel
            {
                Status = true,
                Message = Token.Result,
                Data = await UserManager.FindByNameAsync(Temp.UserName)
            };

        }

        public async Task<ResultViewModel> LogIn(LoginModel loginModel)
        {
            var user = await UserManager.FindByNameAsync(loginModel.Username);
            if (user != null && await UserManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var Token = GenerateJwtToken(user);

                return new ResultViewModel
                {
                    Status = true,
                    Message = Token.Result,
                    Data = user
                };
            }

            return new ResultViewModel
            {
                Status = false
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
            var SignupKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]));

            /**Information about user be included in his token*/

            // Get User roles and add them to claims
            var roles = await UserManager.GetRolesAsync(user);

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Email,user.Email),
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

                    signingCredentials: new SigningCredentials(SignupKey, SecurityAlgorithms.HmacSha256Signature),
                    claims: userClaims
                );
            #endregion

            /**to return Token as string*/
        
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

    }
}
