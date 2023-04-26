using HåndværkervognenAPI.Database;
using HåndværkervognenAPI.Models;
using HåndværkervognenAPI.Security;
using System.Text;

namespace HåndværkervognenAPI.Managers
{
    public class LoginManager : ILoginService
    {
        private IDatabase _database;
        private IHashing _hashing;



        public LoginManager(IDatabase database, IHashing hashing)
        {
            _database = database;
            _hashing = hashing;
        }

        public bool AuthorizeLogin(LoginCredentials loginCredentials)
        {
            UserDal user = _database.GetUser(loginCredentials.Username);
            byte[] hashPassword = _hashing.GenerateHash(loginCredentials.Password, user.Salt);

            if (Encoding.ASCII.GetString(hashPassword) == user.HashPassword)
            {
                return true;
            }
            else return false;
        }

        public void DeleteUser(string username)
        {
            _database.DeleteUser(username);
        }

        public bool RegisterUser(LoginCredentials loginCredentials)
        {
            if (_database.CheckIfUserExists(loginCredentials.Username))
            {
                var salt = _hashing.GenerateSalt();
                var hashPassword = _hashing.GenerateHash(loginCredentials.Password, salt);
                UserDal user = new UserDal(loginCredentials.Username, Encoding.ASCII.GetString(hashPassword), salt);
                _database.CreateUser(user);
                return true;
            }
            return false;
        }
    }
}
