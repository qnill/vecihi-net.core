using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using vecihi.database;
using vecihi.database.model;
using vecihi.helper;
using vecihi.helper.Const;

namespace vecihi.domain.Modules
{
    public interface IAuthService
    {
        Task<ApiResult> UseRefreshToken(string refreshToken, string jti);
        Task<string> SaveRefreshToken(string jwtId);
    }

    public class AuthService : IAuthService
    {
        private readonly VecihiDbContext _context;

        public AuthService(VecihiDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult> UseRefreshToken(string refreshToken, string jti)
        {
            var storedRefreshToken = await _context.RefreshToken.Where(x => x.Token == refreshToken).SingleOrDefaultAsync();

            if (storedRefreshToken == null)
                return new ApiResult { Message = ApiResultMessages.AUE0003 };

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                return new ApiResult { Message = ApiResultMessages.AUE0004 };

            if (storedRefreshToken.Used)
                return new ApiResult { Message = ApiResultMessages.AUE0005 };

            if (storedRefreshToken.JwtId != jti)
                return new ApiResult { Message = ApiResultMessages.AUE0006 };

            storedRefreshToken.Used = true;

            await _context.SaveChangesAsync();

            return new ApiResult { Message = ApiResultMessages.Ok };
        }

        public async Task<string> SaveRefreshToken(string jwtId)
        {
            var refreshToken = new RefreshToken
            {
                JwtId = jwtId,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _context.RefreshToken.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }
    }
}