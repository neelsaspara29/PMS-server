using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
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

        public AuthController(UserDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterBody body)
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
                userRole = body.userRole // Ensure this role exists or manage roles accordingly.
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var response22 = new StandardResponse(true, "User Register Successfuly.");


            return Ok(response22);
        }
    }
}
