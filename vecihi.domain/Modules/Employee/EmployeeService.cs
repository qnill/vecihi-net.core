using AutoMapper;
using System;
using vecihi.database.model;
using vecihi.infrastructure;

namespace vecihi.domain.Modules
{
    public interface IEmployeeService
        : IServiceBase<EmployeeAddDto, EmployeeUpdateDto, EmployeeListDto, EmployeeCardDto, EmployeePagingDto, EmployeeFilterDto, Employee, Guid>
    {

    }

    public class EmployeeService
        : ServiceBase<EmployeeAddDto, EmployeeUpdateDto, EmployeeListDto, EmployeeCardDto, EmployeePagingDto, EmployeeFilterDto, Employee, Guid>, IEmployeeService
    {
        public EmployeeService(UnitOfWork<Guid> uow, IMapper mapper) : base(uow, mapper)
        {
        }
    }
}