using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using vecihi.database.model;
using vecihi.helper;
using vecihi.helper.Const;
using vecihi.infrastructure;

namespace vecihi.domain.Modules
{
    public interface IUserService
    {
        Task<ApiResult> Login(LoginDto model);
        Task<ApiResult> Register(RegisterDto model);
        Task<ApiResult> RemindPassword(RemindPasswordDto model);
        Task<ApiResult> ResetPassword(ResetPasswordDto model);
    }

    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public UserService(UserManager<User> userManager, IMapper mapper, IEmailSender emailSender)
        {
            _userManager = userManager;
            _mapper = mapper;
            _emailSender = emailSender;
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
                return new ApiResult { Data = result.Errors, Message = ApiResultMessages.GNE0002 };

            return new ApiResult { Data = authUser.Id, Message = ApiResultMessages.Ok };
        }

        public async Task<ApiResult> RemindPassword(RemindPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                return new ApiResult { Data = model.Email, Message = ApiResultMessages.GNE0001 };

            string resetPassToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            #region Send-Mail

            string resetPassLink = ($"https://my-fe-project-url/ResetPassword?token={resetPassToken}");
            string subject = "Password Change Request";
            string message = ($"You can change your password by clicking this link. { resetPassLink}");

            var mailResult = await _emailSender.Send(user.Email, subject, message);

            if (mailResult.Message != ApiResultMessages.Ok)
                return mailResult;

            #endregion

            return new ApiResult { Data = user.Email, Message = ApiResultMessages.Ok };
        }

        public async Task<ApiResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return new ApiResult { Data = model.Email, Message = ApiResultMessages.GNE0001 };

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (!result.Succeeded)
                return new ApiResult { Data = result.Errors, Message = ApiResultMessages.GNE0002 };

            return new ApiResult { Data = model.Email, Message = ApiResultMessages.Ok };
        }
    }
}