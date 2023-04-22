namespace HåndværkervognenAPI.Models
{
    public class LoginCredentials
    {
		private string username;

		private string password;


		public string Username
		{
			get { return username; }
			set { username = value; }
		}

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public LoginCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
