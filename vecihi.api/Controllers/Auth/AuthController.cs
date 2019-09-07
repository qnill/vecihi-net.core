using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using vecihi.auth;
using vecihi.domain.Modules;
using vecihi.helper.Const;

namespace vecihi.api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _jwtService.GenerateRefreshToken(model.Token, model.RefreshToken);

            if (result.Message != ApiResultMessages.Ok)
                return BadRequest(result);

            return Ok(result.Data);
        }
    }
}