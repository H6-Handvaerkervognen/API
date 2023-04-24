using HåndværkervognenAPI.Database;
using HåndværkervognenAPI.Models;
using HåndværkervognenAPI.Security;

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
            var hashPassword = hashing.GenerateHash(loginCredentials.Password, user.Salt).ToString();

            if (hashPassword == loginCredentials.Password)
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
            var hashPassword = hashing.GenerateHash(loginCredentials.Password, salt).ToString();
            UserDal user = new UserDal(loginCredentials.Username, hashPassword, salt);
            database.CreateUser(user);
        }
    }
}
