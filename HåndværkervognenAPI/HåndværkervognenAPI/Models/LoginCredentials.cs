namespace HåndværkervognenAPI.Models
{
    public class LoginCredentials
    {
        public string Username { get; private set; }

        public string Password { get; private set; }

        public LoginCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
