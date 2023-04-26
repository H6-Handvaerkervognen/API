namespace HåndværkervognenAPI.Models
{
    public class UserDal
    {
		private string username;

		private string hashPassword;

		private byte[] salt;

        private string token;

		public string UserName { get; private set; }
        public string HashPassword { get; private set; }
        public byte[] Salt { get; private set; }
        public string Token { get; private set; }

        public UserDal(string userName, string hashPassword, byte[] salt, string token)
        {
            UserName = userName;
            HashPassword = hashPassword;
            Salt = salt;
            Token = token;
        }
    }
}
