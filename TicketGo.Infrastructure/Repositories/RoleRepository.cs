using Microsoft.EntityFrameworkCore;
using TicketGo.Domain.Entities;
using TicketGo.Domain.Interfaces;
using TicketGo.Infrastructure.Data;

namespace TicketGo.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _context.Roles
                .Include(r => r.Accounts)
                .ToListAsync();
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            var role = await _context.Roles
                .Include(r => r.Accounts)
                .FirstOrDefaultAsync(r => r.IdRole == id);

            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {id} not found.");
            }

            return role;
        }

        public async Task AddAsync(Role role)
        {
            if (role == null || string.IsNullOrWhiteSpace(role.Name))
            {
                throw new ArgumentException("Role cannot be null and must have a valid name.");
            }

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Role role)
        {
            if (role == null || string.IsNullOrWhiteSpace(role.Name))
            {
                throw new ArgumentException("Role cannot be null and must have a valid name.");
            }

            var existingRole = await _context.Roles.FindAsync(role.IdRole);
            if (existingRole == null)
            {
                throw new KeyNotFoundException($"Role with ID {role.IdRole} not found.");
            }

            _context.Entry(existingRole).CurrentValues.SetValues(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {id} not found.");
            }

            if (role.Accounts.Any())
            {
                throw new InvalidOperationException($"Cannot delete role with ID {id} because it is associated with one or more accounts.");
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }
}