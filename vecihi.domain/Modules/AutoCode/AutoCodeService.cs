using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using vecihi.database.model;
using vecihi.helper;
using vecihi.helper.Const;
using vecihi.infrastructure;

namespace vecihi.domain.Modules
{
    public interface IAutoCodeService
        : IServiceBase<AutoCodeAddDto, AutoCodeUpdateDto, AutoCodeListDto, AutoCodeCardDto, AutoCodePagingDto, AutoCodeExportDto, AutoCodeFilterDto, AutoCode, Guid>
    {
        Task<object> Generate(string screenCode, Guid userId);
    }

    public class AutoCodeService
        : ServiceBase<AutoCodeAddDto, AutoCodeUpdateDto, AutoCodeListDto, AutoCodeCardDto, AutoCodePagingDto, AutoCodeExportDto, AutoCodeFilterDto, AutoCode, Guid>, IAutoCodeService
    {
        private readonly IAutoCodeLogService _autoCodeLogService;

        public AutoCodeService(UnitOfWork<Guid> uow, IMapper mapper, IAutoCodeLogService autoCodeLogService) : base(uow, mapper)
        {
            _autoCodeLogService = autoCodeLogService;
        }

        /// <summary>
        /// This controls the writing status of '{0}' in the code format.
        /// </summary>
        /// <param name="codeFormat"></param>
        /// <returns></returns>
        public bool CheckCodeFormat(string codeFormat)
        {
            return codeFormat.Contains("{0}");
        }

        /// <summary>
        /// Controls whether the screen code is in the <see cref="ScreenCodes"/> class.
        /// </summary>
        /// <param name="screenCode"></param>
        /// <returns></returns>
        public bool CheckScreenCode(string screenCode)
        {
            return typeof(ScreenCodes).GetFields().Any(x => x.Name == screenCode);
        }

        public override async Task<ApiResult> Add(AutoCodeAddDto model, Guid userId, bool isCommit = true)
        {
            if (!CheckScreenCode(model.ScreenCode))
                return new ApiResult { Message = ApiResultMessages.ACW0002 };

            if (!CheckCodeFormat(model.CodeFormat))
                return new ApiResult { Message = ApiResultMessages.ACW0001 };

            return await base.Add(model, userId, isCommit);
        }

        public override async Task<ApiResult> Update(AutoCodeUpdateDto model, Guid userId, bool isCommit = true, bool checkAuthorize = false)
        {
            if (!CheckCodeFormat(model.CodeFormat))
                return new ApiResult { Message = ApiResultMessages.ACW0001 };

            return await base.Update(model, userId, isCommit, checkAuthorize);
        }

        public async Task<object> Generate(string screenCode, Guid userId)
        {
            string code = null;

            var entity = await _uow.Repository<AutoCode>()
                .Get()
                .Where(x => x.ScreenCode == screenCode)
                .FirstOrDefaultAsync();

            if (entity != null)
            {
                int lastCodeNumber = ++entity.LastCodeNumber;
                code = string.Format(entity.CodeFormat, lastCodeNumber);

                entity.LastCodeNumber = lastCodeNumber;

                // Log
                await _autoCodeLogService.Add(new AutoCodeLog
                {
                    CodeNumber = lastCodeNumber,
                    CodeGenerationDate = DateTime.Now,
                    AutoCodeId = entity.Id,
                    GeneratedBy = userId
                }, false);

                await _uow.SaveChangesAsync();
            }

            return new { code };
        }
    }
}