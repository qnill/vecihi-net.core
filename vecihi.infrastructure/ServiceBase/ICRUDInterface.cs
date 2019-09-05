using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using vecihi.helper;
using vecihi.infrastructure.entity.dtos;

namespace vecihi.infrastructure
{
    public interface ICRUDInterface<AddDto, UpdateDto, ListDto, CardDto, PagingDto, ExportDto, FilterDto, Type>
        where Type : struct
        where UpdateDto : DtoUpdateBase<Type>
        where ListDto : DtoGetBase<Type>
        where CardDto : DtoGetBase<Type>
        where PagingDto : DtoPagingBase<Type, ListDto>, new()
    {
        Task<ApiResult> Add(AddDto model, Type userId, bool isCommit = true);
        Task<ApiResult> Update(UpdateDto model, Type userId, bool isCommit = true, bool checkAuthorize = false);
        Task<ApiResult> Delete(Type id, Type? userId = null, bool isCommit = true, bool checkAuthorize = false);
        Task<CardDto> GetById(Type id, Type? userId = null, bool isDeleted = false);
        Task<IList<AutocompleteDto<Type>>> Autocomplete(FilterDto parameters, Type? id = null, string text = null);
        Task<IList<ListDto>> Get(FilterDto parameters, string sortField = null, bool sortOrder = true);
        Task<PagingDto> GetPaging(FilterDto parameters, string sortField = null, bool sortOrder = true, string sumField = null, int? pageSize = null, int? pageNumber = null);
        Task<MemoryStream> ExportToExcel(FilterDto parameters);
    }
}
