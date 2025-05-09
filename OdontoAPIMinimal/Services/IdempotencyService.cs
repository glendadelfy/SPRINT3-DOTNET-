using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OdontoAPIMinimal.Context.Database;
using OdontoAPIMinimal.Models;

namespace OdontoAPIMinimal.Services
{
    public class IdempotencyService
    {
        private readonly AppDbContext _dbContext;

        public IdempotencyService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsDuplicateRequestAsync(string idempotencyKey)
        {
            // Verifica se a chave já existe no banco
            return await _dbContext.IdempotencyKeys.AnyAsync(key => key.Id == idempotencyKey);
        }

        public async Task SaveIdempotencyKeyAsync(string idempotencyKey, int statusCode, object responseBody)
        {
            var newKey = new IdempotencyKey
            {
                Id = idempotencyKey,
                StatusCode = statusCode,
                ResponseBody = JsonConvert.SerializeObject(responseBody),
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.IdempotencyKeys.Add(newKey);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<object?> GetStoredResponseAsync(string idempotencyKey)
        {
            var entry = await _dbContext.IdempotencyKeys.FirstOrDefaultAsync(key => key.Id == idempotencyKey);
            return entry != null ? JsonConvert.DeserializeObject<object>(entry.ResponseBody) : null;
        }
    }
}

