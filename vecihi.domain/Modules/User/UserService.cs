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
        Task<ApiResult> SendEmailForActivation(User entity);
        Task<ApiResult> SendEmailForActivation(SendEmailForActivationDto model);
        Task<ApiResult> ConfirmEmail(ConfirmEmailDto model);
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

            // if you want to use email confirmation
            //if (!await _userManager.IsEmailConfirmedAsync(user))
            //    return new ApiResult { Data = user.Email, Message = ApiResultMessages.AUE0007 };

            return new ApiResult { Data = user.Id, Message = ApiResultMessages.Ok };
        }

        public async Task<ApiResult> Register(RegisterDto model)
        {
            var user = _mapper.Map<User>(model);

            user.Id = Guid.NewGuid();
            user.EmailConfirmed = true; // If you want to activate by mail, set it to false

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return new ApiResult { Data = result.Errors, Message = ApiResultMessages.GNE0002 };
            
            // if you want to use email confirmation
            //var emailConfirmationResult = await SendEmailForActivation(user);
            //if (emailConfirmationResult.Message != ApiResultMessages.Ok)
            //    return emailConfirmationResult;

            return new ApiResult { Data = user.Id, Message = ApiResultMessages.Ok };
        }

        public async Task<ApiResult> RemindPassword(RemindPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                return new ApiResult { Data = model.Email, Message = ApiResultMessages.GNE0001 };

            string resetPassToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            #region Send-Mail

            string resetPassLink = ($"https://fe-url/reset-password?token={resetPassToken}");
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

        private async Task<ApiResult> SendEmailForActivation(string email, string emailConfirmationToken)
        {
            string emailConfirmationLink = ($"https://fe-url/email-confirmation?token={emailConfirmationToken}&email={email}");
            string subject = "Active your email";
            string message = ($"You can activate your e-mail by clicking this link. { emailConfirmationLink}");

            var mailResult = await _emailSender.Send(email, subject, message);

            if (mailResult.Message != ApiResultMessages.Ok)
                return mailResult;

            return new ApiResult { Data = email, Message = ApiResultMessages.Ok };
        }

        public async Task<ApiResult> SendEmailForActivation(User entity)
        {
            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(entity);

            if (await _userManager.IsEmailConfirmedAsync(entity))
                return new ApiResult { Data = entity.Email, Message = ApiResultMessages.AUE0008 };

            return await SendEmailForActivation(entity.Email, emailConfirmationToken);
        }

        public async Task<ApiResult> SendEmailForActivation(SendEmailForActivationDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return new ApiResult { Data = model.Email, Message = ApiResultMessages.GNE0001 };

            if (await _userManager.IsEmailConfirmedAsync(user))
                return new ApiResult { Data = user.Email, Message = ApiResultMessages.AUE0008 };

            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return await SendEmailForActivation(model.Email, emailConfirmationToken);
        }

        public async Task<ApiResult> ConfirmEmail(ConfirmEmailDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return new ApiResult { Data = model.Email, Message = ApiResultMessages.GNE0001 };

            if (await _userManager.IsEmailConfirmedAsync(user))
                return new ApiResult { Data = user.Email, Message = ApiResultMessages.AUE0008 };

            var result = await _userManager.ConfirmEmailAsync(user, model.Token);

            if (!result.Succeeded)
                return new ApiResult { Data = result.Errors, Message = ApiResultMessages.GNE0002 };

            return new ApiResult { Data = model.Email, Message = ApiResultMessages.Ok };
        }
    }
}