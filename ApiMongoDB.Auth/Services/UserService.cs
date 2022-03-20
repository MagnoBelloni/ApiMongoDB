using ApiMongoDB.Auth.Models;

namespace ApiMongoDB.Auth.Services
{
    public class UserService
    {
        public bool ValidateUser(UserViewModel userViewModel)
        {
            return userViewModel.UserName == "admin" && userViewModel.Password == "123";
        }
    }
}
