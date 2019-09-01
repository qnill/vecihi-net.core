using Microsoft.AspNetCore.Authorization;
using System;
using vecihi.domain.Modules;

namespace vecihi.api.Controllers
{
    [AllowAnonymous]
    public class EmployeeController
        : ControllerBase<EmployeeAddDto, EmployeeUpdateDto, EmployeeListDto, EmployeeCardDto, EmployeePagingDto, EmployeeFilterDto, IEmployeeService, Guid>
    {
        public EmployeeController(IEmployeeService service, IdentityClaimsValue identityClaimsValue) : base(service, identityClaimsValue)
        {
        }
    }
}