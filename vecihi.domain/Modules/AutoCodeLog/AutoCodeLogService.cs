using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vecihi.database.model;
using vecihi.infrastructure;

namespace vecihi.domain.Modules
{
    public interface IAutoCodeLogService
    {
        Task Add(AutoCodeLog entity, bool isCommit = true);
        Task<IList<AutoCodeLogListDto>> Get(string screenCode = null);
    }

    public class AutoCodeLogService: IAutoCodeLogService
    {
        private readonly UnitOfWork<Guid> _uow;
        private readonly IMapper _mapper;

        public AutoCodeLogService(UnitOfWork<Guid> uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task Add(AutoCodeLog entity, bool isCommit = true)
        {
            _uow.Repository<AutoCodeLog>().Add(entity);

            if (isCommit)
                await _uow.SaveChangesAsync();
        }

        public async Task<IList<AutoCodeLogListDto>> Get(string screenCode = null)
        {
            var query = _uow.Repository<AutoCodeLog>().Query();

            if (screenCode != null)
                query = query.Where(x => x.AutoCode.ScreenCode.Contains(screenCode));

            var result = await _mapper.ProjectTo<AutoCodeLogListDto>(query)
                .OrderByDescending(x => x.Code)
                .ToListAsync();

            return result;
        }
    }
}