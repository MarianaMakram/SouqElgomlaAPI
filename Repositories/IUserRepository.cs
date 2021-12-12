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
        Task<ResultViewModel> SignUp(SignUpModel signUpModel);
        Task<ResultViewModel> LogIn(LoginModel loginModel);
        Task<ResultViewModel> GetUser(string email);
        Task<User> EditPatch(string email, JsonPatchDocument document);
    }
}
