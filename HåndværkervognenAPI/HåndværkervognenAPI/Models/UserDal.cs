namespace HåndværkervognenAPI.Models
{
    public class UserDal
    {
		private string username;

		private string hashPassword;

		private byte[] salt;

		public string UserName
		{
			get { return username; }
			set { username = value; }
		}
        public string HashPassword
        {
            get { return hashPassword; }
            set { hashPassword = value; }
        }
        public byte[] Salt
        {
            get { return salt; }
            set { salt = value; }
        }

        public UserDal(string userName, string hashPassword, byte[] salt)
        {
            UserName = userName;
            HashPassword = hashPassword;
            Salt = salt;
        }
    }
}
