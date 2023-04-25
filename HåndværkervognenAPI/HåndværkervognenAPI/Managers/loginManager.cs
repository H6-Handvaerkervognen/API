using HåndværkervognenAPI.Database;
using HåndværkervognenAPI.Models;
using HåndværkervognenAPI.Security;
using System.Text;

namespace HåndværkervognenAPI.Managers
{
    public class loginManager : ILoginService
    {
        private IDatabase database;
        private IHashing hashing;



        public loginManager(IDatabase database, IHashing hashing)
        {
            this.database = database;
            this.hashing = hashing;
        }

      

        public bool AuthorizeLogin(LoginCredentials loginCredentials)
        {
            UserDal user = database.GetUser(loginCredentials.Username);
            byte[] hashPassword = hashing.GenerateHash(loginCredentials.Password, user.Salt);

            if (Encoding.ASCII.GetString(hashPassword) == user.HashPassword)
            {
                return true;
            }
            else return false;
        }

        public void DeleteUser(string username)
        {
            database.DeleteUser(username);
        }

        public void RegisterUser(LoginCredentials loginCredentials)
        {
            var salt = hashing.GenerateSalt();
            var hashPassword = hashing.GenerateHash(loginCredentials.Password, salt);
            UserDal user = new UserDal(loginCredentials.Username, Encoding.ASCII.GetString(hashPassword), salt);
            database.CreateUser(user);
        }
    }
}
