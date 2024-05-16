using CarRent.Data;
using CarRent.Data.Entities;
using CarRental.Data;

namespace CarRent.Core.Services.Init
{
    public class RoleInit
    {
        private readonly CarRentalDbContext _context;

        public RoleInit(CarRentalDbContext context)
        {
            _context = context;
        }

        public async Task Init()
        {
            if (!_context.Roles.Any())
            {
                await _context.Roles.AddAsync(new Role { Name = "Admin" });
                await _context.Roles.AddAsync(new Role { Name = "User" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
