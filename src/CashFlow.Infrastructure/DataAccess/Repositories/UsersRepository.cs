using ClashFlow.Domain.Entities;
using ClashFlow.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories
{
    internal class UsersRepository(
        CashFlowDbContext dbContext
    ) : IUsersReadOnlyRepository, IUsersWriteOnlyRepository
    {
        private readonly CashFlowDbContext _dbContext = dbContext;

        public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

        public async Task<bool> ExistActiveUserWithEmail(string email) =>  await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));

        public async Task<User?> GetUserByEmail(string email) => await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email.Equals(email));
    }
}
