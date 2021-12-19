using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Repositories
{
    public interface IUserRepository
    {
        Task<UserResultViewModel> SignUp(SignUpModel signUpModel);
        Task<UserResultViewModel> LogIn(LoginModel loginModel);
        Task<User> GetUser(string email);
        Task<User> EditPatch(string email, User document);
        Task<User> PutImage(string email, string imageName, IFormFile userImage);
    }
}
