using Microsoft.AspNetCore.Mvc;

namespace HåndværkervognenAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController
    {
     

        [HttpPost(Name = "Login")]
        public bool Login(string username, string pass)
        {
            return true;
        }
        [HttpPost(Name = "CreateNewUser")]
        public void CreateNewUser(string username, string pass)
        {
     
        }
        [HttpPost(Name = "DeleteUser")]
        public void DeleteUser(string username, string pass)
        {
            
        }
    }
}
