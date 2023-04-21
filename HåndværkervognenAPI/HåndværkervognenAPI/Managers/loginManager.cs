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

        public bool AuthorizeLogin(string username, string password)
        {
            UserDal user = database.GetUser(username);
            var hashPassword = hashing.GenerateHash(password, user.Salt).ToString();
          
            if (hashPassword == password)
            {
                return true;
            }else return false;
        }

        public void DeleteUser(string uid)
        {
            database.DeleteUser(uid);
        }

        public void RegisterUser(string username, string password)
        {
            var salt = hashing.GenerateSalt();
            var hashPassword = hashing.GenerateHash(password,salt).ToString();
            UserDal user = new UserDal(username, hashPassword, salt);
            database.createUser(user);
        }
    }
}
