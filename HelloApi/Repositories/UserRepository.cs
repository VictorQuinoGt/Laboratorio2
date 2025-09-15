using HelloApi.Models;
using HelloApi.Repositories.Interfaces;
using HelloApi.Data;
using Microsoft.EntityFrameworkCore;

namespace HelloApi.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<User?> GetUserWithRoleAsync(string username, string password)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        /// <summary>
        /// Busca el usuario en la base de datos y retorna la coincidencia
        /// </summary>
        /// <param name="username">string required</param>
        /// <returns>Retorna el primer registro segun el filtro (WHERE en el SELECT)</returns>
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}