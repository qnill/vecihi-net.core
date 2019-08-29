using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using vecihi.auth;

namespace vecihi.api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly IJWTService _jwtService;

        public AuthController(UserManager<AuthUser> userManager,IJWTService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [Route("Login"),HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.UserName);

            // ToDo: Const'dan mesaj döndür
            if (user == null || !(await _userManager.CheckPasswordAsync(user, model.Password)))
                return BadRequest("CONST-MESSAGES-DONECEK");

            // Set Claims
            var claimsIdentity = _jwtService.GenerateClaimsIdentity(user.Id, user.UserName);

            var jwt = await _jwtService.GenerateJwt(claimsIdentity, model.UserName);

            return Ok(jwt);
        }

        [Route("Register"), HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // ToDo: Mapping'e bağla
            // Service-Yaz
            var authUser = new AuthUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.Phone
            };

            var result = await _userManager.CreateAsync(authUser, model.Password);
            
            // ToDo: Const'dan mesaj döndür - APIResult'a bağla
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // ToDo: APIResult'a bağla
            return Ok();
        }
    }
}