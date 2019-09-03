using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using vecihi.helper;
using vecihi.helper.Const;

namespace vecihi.auth
{
    public interface IAuthService
    {
        Task<ApiResult> Login(LoginDto model);
        Task<ApiResult> Register(RegisterDto model);
    }

    public class AuthService: IAuthService
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly IJWTService _jwtService;

        public AuthService(UserManager<AuthUser> userManager, IJWTService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<ApiResult> Login(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
                return new ApiResult { Data = model.UserName, Message = ApiResultMessages.AUE0001 };

            var jwt = await _jwtService.GenerateJwt(user.Id, model.UserName);

            return new ApiResult { Data = jwt, Message = ApiResultMessages.Ok };
        }

        public async Task<ApiResult> Register(RegisterDto model)
        {
            var authUser = new AuthUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.Phone
            };

            var result = await _userManager.CreateAsync(authUser, model.Password);

            if (!result.Succeeded)
                return new ApiResult { Data = result.Errors, Message = ApiResultMessages.GNE0004 };

            return new ApiResult { Data = authUser.Id, Message = ApiResultMessages.Ok };
        }
    }
}