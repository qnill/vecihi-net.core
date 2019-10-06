using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using vecihi.auth;
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
    public abstract class ControllerBase<AddDto, UpdateDto, ListDto, CardDto, PagingDto, ExportDto, FilterDto, Service, Type>
        : ControllerBase
        where Type : struct
        where UpdateDto : IDtoUpdateBase<Type>
        where ListDto : IDtoGetBase<Type>
        where CardDto : IDtoGetBase<Type>
        where PagingDto : DtoPagingBase<Type, ListDto>, new()
        where Service : ICRUDInterface<AddDto, UpdateDto, ListDto, CardDto, PagingDto, ExportDto, FilterDto, Type>
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
        public virtual async Task<IActionResult> Add([FromBody]AddDto model)
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
        public virtual async Task<IActionResult> Update([FromBody]UpdateDto model)
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
                return BadRequest(new ApiResult { Data = id, Message = ApiResultMessages.GNE0001 });

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
        public virtual async Task<IActionResult> Get([FromQuery]FilterDto parameters, string sortField = null, bool sortOrder = true)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.Get(parameters, sortField, sortOrder);

            return Ok(result);
        }

        [HttpGet("GetPaging")]
        public virtual async Task<IActionResult> GetPaging([FromQuery]FilterDto parameters, string sortField = null, bool sortOrder = true,
            string sumField = null, int? pageSize = null, int? pageNumber = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.GetPaging(parameters, sortField, sortOrder, sumField, pageSize, pageNumber);

            return Ok(result);
        }

        [HttpGet("ExportToExcel")]
        public virtual async Task<IActionResult> ExportToExcel([FromQuery]FilterDto paramaters, string sortField = null, bool sortOrder = true)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var data = await _service.ExportToExcel(paramaters, sortField, sortOrder);

            var fileName = typeof(Service).Name.Replace("Service", string.Empty).Remove(0, 1);

            return File(
                fileContents: data.ToArray(),
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: $"{fileName}.xlsx");
        }
    }
}