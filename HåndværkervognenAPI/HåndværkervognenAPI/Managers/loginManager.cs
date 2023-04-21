using HåndværkervognenAPI.Database;

namespace HåndværkervognenAPI.Managers
{
    public class loginManager : ILoginService
    {
        private IDatabase _database;


        public IDatabase Database
        {
            get { return _database; }
            set { _database = value; }
        }



        public bool AuthorizeLogin(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(string uid)
        {
            throw new NotImplementedException();
        }

        public void RegisterUser(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
