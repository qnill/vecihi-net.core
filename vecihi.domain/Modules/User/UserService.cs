using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using vecihi.database.model;
using vecihi.helper;
using vecihi.helper.Const;

namespace vecihi.domain.Modules
{
    public interface IUserService
    {
        Task<ApiResult> Login(LoginDto model);
        Task<ApiResult> Register(RegisterDto model);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ApiResult> Login(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
                return new ApiResult { Data = model.UserName, Message = ApiResultMessages.AUE0001 };

            return new ApiResult { Data = user.Id, Message = ApiResultMessages.Ok };
        }

        public async Task<ApiResult> Register(RegisterDto model)
        {
            var authUser = _mapper.Map<User>(model);

            authUser.Id = Guid.NewGuid();

            var result = await _userManager.CreateAsync(authUser, model.Password);

            if (!result.Succeeded)
                return new ApiResult { Data = result.Errors, Message = ApiResultMessages.GNE0004 };

            return new ApiResult { Data = authUser.Id, Message = ApiResultMessages.Ok };
        }
    }
}