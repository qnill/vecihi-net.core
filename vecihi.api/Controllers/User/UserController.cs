using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using vecihi.auth;
using vecihi.domain.Modules;
using vecihi.helper;
using vecihi.helper.Const;

namespace vecihi.api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UserController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Login(model);

            if (result.Message != ApiResultMessages.Ok)
                return BadRequest(result);

            var jwt = await _jwtService.GenerateJwt(result.Data.ToString(), model.UserName);

            return Ok(jwt);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Register(model);

            if (result.Message != ApiResultMessages.Ok)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("RemindPassword")]
        [ProducesResponseType(typeof(ApiResult), 200)]
        public async Task<IActionResult> RemindPassword([FromBody]RemindPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RemindPassword(model);

            if (result.Message != ApiResultMessages.Ok)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        [ProducesResponseType(typeof(ApiResult), 200)]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.ResetPassword(model);

            if (result.Message != ApiResultMessages.Ok)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("SendEmailForActivation")]
        [ProducesResponseType(typeof(ApiResult), 200)]
        public async Task<IActionResult> SendEmailForActivation([FromBody]SendEmailForActivationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.SendEmailForActivation(model);

            if (result.Message != ApiResultMessages.Ok)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("ConfirmEmail")]
        [ProducesResponseType(typeof(ApiResult), 200)]
        public async Task<IActionResult> ConfirmEmail([FromBody]ConfirmEmailDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.ConfirmEmail(model);

            if (result.Message != ApiResultMessages.Ok)
                return BadRequest(result);

            return Ok(result);
        }
    }
}