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
                userRole = "",
                active_status= "pending",
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var response22 = new StandardResponse(true, "User Register Successfuly.");


            return Ok(response22);
        }

        [HttpPost("admin/add")]
        public async Task<IActionResult> AddAdmin([FromBody] RegisterBody body)
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
                userRole = body.userRole,
                active_status = "active"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var response22 = new StandardResponse(true, "User Added Successfuly.");


            return Ok(response22);
        }

        [HttpGet("user/get/all")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _context.Users.ToListAsync();
            var response = new StandardResponse(true, "User Retrieve Successfully", users);
            return Ok(response);
        }

        [HttpGet("user/approve/{userId}/{role}")]
        public async Task<IActionResult> ApproveUser(int userId, string role)
        {
            var users = await _context.Users.Where(user => user.Id == userId).FirstAsync();
            users.active_status = "active";
            users.userRole = role;

            await _context.SaveChangesAsync();

            var response = new StandardResponse(true, "User Retrieve Successfully", users);
            return Ok(response);
        }

        [HttpGet("user/reject/{userId}")]
        public async Task<IActionResult> RejectUser(int userId, string role)
        {
            var users = await _context.Users.Where(user => user.Id == userId).FirstAsync();
            users.active_status = "block";;

            await _context.SaveChangesAsync();

            var response = new StandardResponse(true, "User REjected Successfully", users);
            return Ok(response);
        }

        [HttpGet("user/delete/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var users = await _context.Users.Where(user => user.Id == userId).FirstAsync();
             _context.Users.Remove(users);
            await _context.SaveChangesAsync();

            var response = new StandardResponse(true, "User Deleted Successfully", users);
            return Ok(response);
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

            if(user.active_status == "pending") {
                var pendingStatusResponse = new StandardResponse(false, "You are not approved by admin");
                return Ok(pendingStatusResponse);
            }

            if(user.active_status == "block")
            {
                var blockStatusResponse = new StandardResponse(false, "Your Request Denied By Admin");
                return Ok(blockStatusResponse);
            }

            var token = JwtTokenGenerator.GenerateToken(user.email, _jwtSecret);

            var finalResponse = new StandardResponse(true, "User Login Success.", new {  token, user});

            return Ok(finalResponse);
        }

    }
}
