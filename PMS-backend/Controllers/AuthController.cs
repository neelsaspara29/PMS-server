using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMS_backend.DataContext;
using PMS_backend.Dto;
using PMS_backend.Model;
using PMS_backend.Services;

namespace PMS_backend.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserDbContext _context;
        private readonly PasswordHasher _passwordHasher;
        private readonly string _jwtSecret;

        public AuthController(UserDbContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _passwordHasher = new PasswordHasher();
            _jwtSecret = configuration["Jwt:Key"];
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody]RegisterBody body)
        {
            if (await _context.Users.AnyAsync(x => x.email == body.email))
            {
                var response = new StandardResponse(false, "User Already Exist!");
                return Ok(response);
            }
            var user = new UserModel
            {
                userName = body.userName,
                email = body.email,
                password = _passwordHasher.HashPassword(body.password),
                userRole = body.userRole 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var response22 = new StandardResponse(true, "User Register Successfuly.");


            return Ok(response22);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginBody body)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == body.email);

            if (user == null || !_passwordHasher.VerifyPassword(user.password, body.password))
            {
                // await _emailService.SendEmailAsync("neelsaspara2023@gmail.com", "hello", "hello");
                var invalidCredResponse = new StandardResponse(false, "Invalid Credentials.");
                return Ok(invalidCredResponse);
            }

            var token = JwtTokenGenerator.GenerateToken(user.email, _jwtSecret);

            var finalResponse = new StandardResponse(true, "User Login Success.", new {  token});

            return Ok(finalResponse);
        }

    }
}
