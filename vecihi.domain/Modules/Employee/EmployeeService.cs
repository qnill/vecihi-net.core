using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using vecihi.database.model;
using vecihi.infrastructure;

namespace vecihi.domain.Modules
{
    public interface IEmployeeService
        : IServiceBase<EmployeeAddDto, EmployeeUpdateDto, EmployeeListDto, EmployeeCardDto, EmployeePagingDto, EmployeeFilterDto, Employee, Guid>
    {
        Task<InfoForJwtDto> InfoForJwt(Guid userId);
    }

    public class EmployeeService
        : ServiceBase<EmployeeAddDto, EmployeeUpdateDto, EmployeeListDto, EmployeeCardDto, EmployeePagingDto, EmployeeFilterDto, Employee, Guid>, IEmployeeService
    {
        public EmployeeService(UnitOfWork<Guid> uow, IMapper mapper) : base(uow, mapper)
        {
        }

        public async Task<InfoForJwtDto> InfoForJwt(Guid userId)
        {
            var employee = await _uow.Repository<Employee>()
                .Query()
                .Where(x => x.UserId == userId)
                .Select(s => new InfoForJwtDto
                {
                    Id = s.Id
                })
                .FirstOrDefaultAsync();

            return employee;
        }
    }
}