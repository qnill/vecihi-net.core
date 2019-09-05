using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;
using vecihi.auth;
using vecihi.domain.Modules;

namespace vecihi.api.Controllers
{
    public class AutoCodeController
        : ControllerBase<AutoCodeAddDto, AutoCodeUpdateDto, AutoCodeListDto, AutoCodeCardDto, AutoCodePagingDto, AutoCodeExportDto, AutoCodeFilterDto, IAutoCodeService, Guid>
    {
        private readonly IAutoCodeLogService _autoCodeLogService;

        public AutoCodeController(IAutoCodeService service, IdentityClaimsValue identityClaimsValue, IAutoCodeLogService autoCodeLogService)
            : base(service, identityClaimsValue)
        {
            _autoCodeLogService = autoCodeLogService;
        }

        [HttpGet("Generate")]
        public async Task<IActionResult> Generate([BindRequired]string screenCode)
        {
            var result = await _service.Generate(screenCode, _identityClaimsValue.UserId());

            return Ok(result);
        }

        [HttpGet("Logs")]
        [Produces(typeof(AutoCodeLogListDto))]
        public async Task<IActionResult> Logs(string screenCode = null)
        {
            var result = await _autoCodeLogService.Get(screenCode);

            return Ok(result);
        }
    }
}