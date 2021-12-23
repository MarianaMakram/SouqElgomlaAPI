using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string? Address { get; set; }
        public string? PayPalAccount { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "Role is required")]
        //public string Role { get; set; }
    }


    public static class SignUpModelExtention
    {
        public static User ToModel(this SignUpModel signUpModel)
        {
            User user =  new User
            {
                Name = signUpModel.Name,
                Address = signUpModel.Address,
                Email = signUpModel.Email,
                UserName = signUpModel.UserName,
                PayPalAccount = signUpModel.PayPalAccount
            };

            return user;
        }
    }
}
