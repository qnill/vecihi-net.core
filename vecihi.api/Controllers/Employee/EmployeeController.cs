﻿using System;
using vecihi.auth;
using vecihi.domain.Modules;

namespace vecihi.api.Controllers
{
    public class EmployeeController
        : ControllerBase<EmployeeAddDto, EmployeeUpdateDto, EmployeeListDto, EmployeeCardDto, EmployeePagingDto, EmployeeExportDto, EmployeeFilterDto, IEmployeeService, Guid>
    {
        public EmployeeController(IEmployeeService service, IdentityClaimsValue identityClaimsValue) : base(service, identityClaimsValue)
        {
        }
    }
}