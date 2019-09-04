using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using vecihi.helper;
using vecihi.helper.Const;
using vecihi.infrastructure;
using vecihi.infrastructure.entity.dtos;

namespace vecihi.api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiController]
    public abstract class ControllerBase<AddDto, UpdateDto, ListDto, CardDto, PagingDto, FilterDto, Service, Type>
        : ControllerBase
        where Type : struct
        where UpdateDto : DtoUpdateBase<Type>
        where ListDto : DtoGetBase<Type>
        where CardDto : DtoGetBase<Type>
        where PagingDto : DtoPagingBase<Type, ListDto>, new()
        where Service : ICRUDInterface<AddDto, UpdateDto, ListDto, CardDto, PagingDto, FilterDto, Type>
    {
        protected Service _service;
        protected readonly IdentityClaimsValue _identityClaimsValue;

        public ControllerBase(Service service, IdentityClaimsValue identityClaimsValue)
        {
            _service = service;
            _identityClaimsValue = identityClaimsValue;
        }

        [HttpPost("Add")]
        [ProducesResponseType(typeof(ApiResult), 200)]
        public virtual async Task<IActionResult> Add(AddDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.Add(model, _identityClaimsValue.UserId<Type>());

            if (result.Message != ApiResultMessages.Ok)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("Update")]
        [ProducesResponseType(typeof(ApiResult), 200)]
        public virtual async Task<IActionResult> Update(UpdateDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.Update(model, _identityClaimsValue.UserId<Type>());

            if (result.Message != ApiResultMessages.Ok)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(typeof(ApiResult), 200)]
        public virtual async Task<IActionResult> Delete([BindRequired]Type id)
        {
            var result = await _service.Delete(id, _identityClaimsValue.UserId<Type>());

            if (result.Message != ApiResultMessages.Ok)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("GetById")]
        public virtual async Task<IActionResult> GetById([BindRequired]Type id)
        {
            var result = await _service.GetById(id);

            if (result == null)
                return BadRequest(ApiResultMessages.GNE0001);

            return Ok(result);
        }

        [HttpGet("Autocomplete")]
        public virtual async Task<IActionResult> Autocomplete([FromQuery]FilterDto parameters, Type? id = null, string text = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.Autocomplete(parameters, id, text);

            return Ok(result);
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get([FromQuery]FilterDto parameters, string sortField = null, bool sortOrder = true)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.Get(parameters, sortField, sortOrder);

            return Ok(result);
        }

        [HttpGet("GetPaging")]
        public async Task<IActionResult> GetPaging([FromQuery]FilterDto parameters, string sortField = null, bool sortOrder = true,
            string sumField = null, int? first = null, int? rows = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.GetPaging(parameters, sortField, sortOrder, sumField, first, rows);

            return Ok(result);
        }
    }
}