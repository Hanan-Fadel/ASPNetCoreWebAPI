using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIApp.Repositories;

namespace WebAPIApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;

        //inject the UserRepository inside the construcor
        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
        {
            this.userRepository = userRepository;
            this.tokenHandler = tokenHandler;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
            // Validate the incoming request using the Fluent Validation that will happen automatically 
            // see implementation inside LoginRequestValidator class 

            // Check if user is authenticated using the UserRepository
            // Check username and password
            var user = await userRepository.AuthenticateAsync(loginRequest.Username, loginRequest.Password);

            if(user !=null)
            {
                //user is authenticated, so generate a Jwt Token
                var token = await tokenHandler.CreateTokenAsync(user);
                return Ok(token);
            }
            return BadRequest("Username or Password is incorrect.");

        }
    }
}
