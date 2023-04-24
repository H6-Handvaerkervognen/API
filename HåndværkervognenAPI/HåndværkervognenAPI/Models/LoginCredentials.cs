namespace HåndværkervognenAPI.Models
{
    public class LoginCredentials
    {
		private string username;

		private string password;


		public string Username { get; private set; }

        public string Password { get; private set; }

        public LoginCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
