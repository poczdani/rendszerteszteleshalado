using CarRent.Core.Models.User;
using CarRent.Data;
using CarRent.Data.Entities;
using CarRental.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarRent.Core.Services
{
    public interface IAuthService
    {
        Task Register(User user, string password);
        Task<AuthResponseDto> Login(UserLoginDto loginDto);
        Task<User?> GetUser();
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly CarRentalDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public AuthService(
            IConfiguration configuration, CarRentalDbContext context, IHttpContextAccessor httpContext)
        {
            _configuration = configuration;
            _context = context;
            _httpContext = httpContext;
        }

        public async Task<AuthResponseDto> Login(UserLoginDto loginDto)
        {
            var user = await _context.Users!
                .FirstOrDefaultAsync(x => x.Email.ToLower().Equals(loginDto.Email.ToLower()))
                ?? throw new Exception("Rossz felhasználónév vagy jelszó");

            if (VerifyPasswordHash(loginDto.Password, user.PasswordHash!, user.PasswordSalt!))
            {
                var isAdmin = await _context.UserRoles!
                    .AnyAsync(ur => ur.UserId == user.Id && ur.Role.Name.Equals("Admin"));

                return new AuthResponseDto
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Token = CreateToken(user),
                    IsAdmin = isAdmin
                };
            }
            else
            {
                throw new Exception("Rossz felhasználónév vagy jelszó");
            }
        }

        public async Task Register(User user, string password)
        {
            if (await UserExists(user.Email))
            {
                throw new Exception("A felhasználó már létezik");
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users!.AddAsync(user);
            await _context.SaveChangesAsync();

            var userRole = await _context.Roles!
                .FirstOrDefaultAsync(role => role.Name.Equals("User"))
                ?? throw new Exception("Nem található a szerepkör");

            await _context.UserRoles!.AddAsync(new UserRole { UserId = user.Id, RoleId = userRole.Id });
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users!
                .AnyAsync(user => user.Email.ToLower().Equals(email.ToLower()));
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("Token").Value
                ?? throw new Exception("Token kulcs nincs megadva")));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: cred
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public Task<User?> GetUser()
        {
            var email = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            return _context.Users!
                .FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email!.ToLower()));
        }
    }
}
