using AutoMapper;
using ClosedXML.Excel;
using FastMember;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using vecihi.helper;
using vecihi.helper.Attributes;
using vecihi.helper.Const;
using vecihi.helper.Extensions;
using vecihi.infrastructure.entity.dtos;
using vecihi.infrastructure.entity.models;
using static vecihi.helper.Const.Enums;

namespace vecihi.infrastructure
{
    public interface IServiceBase<AddDto, UpdateDto, ListDto, CardDto, PagingDto,ExportDto, FilterDto, Entity, Type>
        : ICRUDInterface<AddDto, UpdateDto, ListDto, CardDto, PagingDto, ExportDto, FilterDto, Type>
        where Type : struct
        where Entity : ModelBase<Type>
        where UpdateDto : DtoUpdateBase<Type>
        where ListDto : DtoGetBase<Type>
        where CardDto : DtoGetBase<Type>
        where PagingDto : DtoPagingBase<Type, ListDto>, new()
    {
        Entity AddMapping(AddDto model, Type userId);
        Task<Entity> UpdateMapping(UpdateDto model, Type userId);
        IQueryable<Entity> PrepareGetQuery(FilterDto parameters);
    }

    public abstract class ServiceBase<AddDto, UpdateDto, ListDto, CardDto, PagingDto, ExportDto, FilterDto, Entity, Type>
        : IServiceBase<AddDto, UpdateDto, ListDto, CardDto, PagingDto, ExportDto, FilterDto, Entity, Type>
        where Type : struct
        where Entity : ModelBase<Type>
        where UpdateDto : DtoUpdateBase<Type>
        where ListDto : DtoGetBase<Type>
        where CardDto : DtoGetBase<Type>
        where PagingDto : DtoPagingBase<Type, ListDto>, new()
    {
        protected UnitOfWork<Type> _uow;
        private readonly IMapper _mapper;

        public ServiceBase(UnitOfWork<Type> uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public virtual Entity AddMapping(AddDto model, Type userId)
        {
            Entity entity = _mapper.Map<Entity>(model);

            var pkType = default(Type);

            if (pkType is Guid)
            {
                if ((entity.Id as Guid?).IsNullOrEmpty())
                    entity.Id = (Type)TypeDescriptor.GetConverter(typeof(Type)).ConvertFromInvariantString(Guid.NewGuid().ToString());
            }

            if (entity is IModelAuditBase<Type>)
            {
                (entity as IModelAuditBase<Type>).CreatedBy = userId;
                (entity as IModelAuditBase<Type>).CreatedAt = DateTime.UtcNow;
            }

            return entity;
        }

        public virtual async Task<ApiResult> Add(AddDto model, Type userId, bool isCommit = true)
        {
            Entity entity = AddMapping(model, userId);

            await _uow.Repository<Entity>().Add(entity);

            if (isCommit)
                await _uow.SaveChangesAsync();

            return new ApiResult { Data = entity.Id, Message = ApiResultMessages.Ok };
        }

        public virtual async Task<Entity> UpdateMapping(UpdateDto model, Type userId)
        {
            Entity entity = await _uow.Repository<Entity>().GetById(model.Id);

            if (entity is IModelAuditBase<Type>)
            {
                (entity as IModelAuditBase<Type>).UpdatedBy = userId;
                (entity as IModelAuditBase<Type>).UpdatedAt = DateTime.UtcNow;
            }

            _mapper.Map(model, entity);

            return entity;
        }

        public virtual async Task<ApiResult> Update(UpdateDto model, Type userId, bool isCommit = true, bool checkAuthorize = false)
        {
            Entity entity = await UpdateMapping(model, userId);

            if (entity == null)
                return new ApiResult { Data = model.Id, Message = ApiResultMessages.GNE0001 };

            // Access Control
            if (entity is IModelAuditBase<Type> && checkAuthorize && !(entity as IModelAuditBase<Type>).CreatedBy.Equals(userId))
                return new ApiResult { Data = model.Id, Message = ApiResultMessages.GNW0001 };

            if (isCommit)
                await _uow.SaveChangesAsync();

            return new ApiResult { Data = entity.Id, Message = ApiResultMessages.Ok };
        }

        public virtual async Task<ApiResult> Delete(Type id, Type? userId = null, bool isCommit = true, bool checkAuthorize = false)
        {
            Entity entity = await _uow.Repository<Entity>().GetById(id);

            if (entity == null)
                return new ApiResult { Data = id, Message = ApiResultMessages.GNE0001 };

            if (entity is IModelAuditBase<Type>)
            {
                // Access Control
                if (checkAuthorize && userId != null && !(entity as IModelAuditBase<Type>).CreatedBy.Equals(userId.Value))
                    return new ApiResult { Message = ApiResultMessages.GNW0001 };

                (entity as IModelAuditBase<Type>).UpdatedAt = DateTime.UtcNow;
                (entity as IModelAuditBase<Type>).UpdatedBy = userId.Value;
            }

            entity.IsDeleted = true;

            if (isCommit)
                await _uow.SaveChangesAsync();

            return new ApiResult { Data = id, Message = ApiResultMessages.Ok };
        }

        public virtual async Task<CardDto> GetById(Type id, Type? userId = null, bool isDeleted = false)
        {
            var query = _uow.Repository<Entity>().Query(isDeleted).Equal("Id", id);

            // Access Control
            if (userId != null)
                query = query.Equal("CreateBy", userId.Value);

            return await _mapper.ProjectTo<CardDto>(query).FirstOrDefaultAsync();
        }

        public virtual IQueryable<Entity> PrepareGetQuery(FilterDto parameters)
        {
            var query = _uow.Repository<Entity>().Query();

            var properties = typeof(FilterDto).GetProperties()
                .Select(s => new
                {
                    name = s.Name,
                    type = s.PropertyType.IsGenericType && s.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                            ? System.Type.GetTypeCode(s.PropertyType.GetGenericArguments()[0])
                            : System.Type.GetTypeCode(s.PropertyType),
                    value = s.GetValue(parameters),
                    atts = s.CustomAttributes
                        .Where(x => x.AttributeType == typeof(FilterAttribute))
                        .Select(st => st.ConstructorArguments
                            .Select(sc => sc.Value)
                            .ToList())
                        .FirstOrDefault()
                })
                .Where(x => x.value != null)
                .ToList();

            foreach (var prop in properties)
            {
                string dbName = prop.name;
                SearchType searchType = 0;

                // if use filter attribute.
                if (prop.atts != null)
                {
                    if ((bool)prop.atts[2])
                        continue;

                    dbName = (prop.atts[1] != null && !string.IsNullOrWhiteSpace(prop.atts[1].ToString())) ? prop.atts[1].ToString() : dbName;
                    searchType = (SearchType)prop.atts[0];
                }

                if (prop.type == TypeCode.String)
                {
                    if (!string.IsNullOrWhiteSpace(prop.value.ToString()))
                    {
                        if (searchType == SearchType.Contains)
                            query = query.Contains(dbName, prop.value.ToString());
                        else if (searchType == SearchType.Equal)
                            query = query.Equal(dbName, prop.value.ToString());
                        else
                            throw new Exception(string.Format(
                                format: "{0} is a string type property. You can not query a property of type {1} {2}.",
                                arg0: dbName, arg1: prop.type, arg2: searchType));
                    }
                }

                else if (prop.type == TypeCode.Int32 || prop.type == TypeCode.Double || prop.type == TypeCode.Int16 ||
                        prop.type == TypeCode.Decimal || prop.type == TypeCode.Byte || prop.type == TypeCode.Int64 ||
                        prop.type == TypeCode.UInt16 || prop.type == TypeCode.UInt32 || prop.type == TypeCode.UInt64)
                {
                    switch (searchType)
                    {
                        case SearchType.Equal:
                            query = query.Equal(dbName, prop.value);
                            break;
                        case SearchType.GreaterThan:
                            query = query.GreaterThan(dbName, prop.value);
                            break;
                        case SearchType.GreaterThanOrEqual:
                            query = query.GreaterThanOrEqual(dbName, prop.value);
                            break;
                        case SearchType.LessThan:
                            query = query.LessThan(dbName, prop.value);
                            break;
                        case SearchType.LessThanOrEqual:
                            query = query.LessThanOrEqual(dbName, prop.value);
                            break;
                        default:
                            throw new Exception(string.Format(
                                format: "{0} is a numeric type property. You can not query a property of type {1} {2}.",
                                arg0: dbName, arg1: prop.type, arg2: searchType));
                    }
                }

                else if (prop.type == TypeCode.Boolean)
                {
                    if (searchType == SearchType.Equal)
                        query = query.Equal(dbName, prop.value);
                    else
                        throw new Exception(string.Format(
                            format: "{0} is a boolean type property. You can not query a property of type {1} {2}.",
                            arg0: dbName, arg1: prop.type, arg2: searchType));
                }

                else if (prop.type == TypeCode.Object)
                {
                    if (!Guid.Parse(prop.value.ToString()).IsNullOrEmpty())
                    {
                        if (searchType == SearchType.Equal)
                            query = query.Equal(dbName, prop.value);
                        else
                            throw new Exception(string.Format(
                                format: "{0} is a object type property. You can not query a property of type {1} {2}.",
                                arg0: dbName, arg1: prop.type, arg2: searchType));
                    }
                }

                else if (prop.type == TypeCode.DateTime)
                {
                    switch (searchType)
                    {
                        case SearchType.Equal:
                            query = query.DiffDaysEqual(dbName, prop.value);
                            break;
                        case SearchType.GreaterThanOrEqual:
                            query = query.DiffDaysGreaterThan(dbName, prop.value);
                            break;
                        case SearchType.LessThanOrEqual:
                            query = query.DiffDaysLessThan(dbName, prop.value);
                            break;
                        default:
                            throw new Exception(string.Format(
                                format: "{0} is a datetime type property. You can not query a property of type {1} {2}.",
                                arg0: dbName, arg1: prop.type, arg2: searchType));
                    }
                }
            }

            return query;
        }

        public virtual async Task<IList<AutocompleteDto<Type>>> Autocomplete(FilterDto parameters, Type? id = null, string text = null)
        {
            var query = _mapper.ProjectTo<Autocomplete<Type>>(PrepareGetQuery(parameters)).AsQueryable();

            var pkType = default(Type);

            if ((pkType is Guid && !(id as Guid?).IsNullOrEmpty()) || id != null)
                query = query.Where(x => (object)x.Id == (object)id);

            if (text != null)
                query = query.Where(x => x.Search.Contains(text));

            return await _mapper.ProjectTo<AutocompleteDto<Type>>(query.OrderBy(x => x.Text)).ToListAsync();
        }

        public virtual async Task<IList<ListDto>> Get(FilterDto parameters, string sortField = null, bool sortOrder = true)
        {
            var query = PrepareGetQuery(parameters);

            if (!string.IsNullOrWhiteSpace(sortField))
            {
                if (sortOrder)
                    query = query.OrderBy(sortField);
                else
                    query = query.OrderByDescending(sortField);
            }

            return await _mapper.ProjectTo<ListDto>(query).ToListAsync();
        }

        public virtual async Task<PagingDto> GetPaging(FilterDto parameters, string sortField = null, bool sortOrder = true,
            string sumField = null, int? pageSize = null, int? pageNumber = null)
        {
            var query = PrepareGetQuery(parameters);

            if (!string.IsNullOrWhiteSpace(sortField))
            {
                if (sortOrder)
                    query = query.OrderBy(sortField);
                else
                    query = query.OrderByDescending(sortField);
            }

            // Get records count.
            int? dataCount = await query.CountAsync();

            /// It takes the sum according to the value of 
            /// <param name="sumField"></param>.
            double? sum = null;
            if (!string.IsNullOrWhiteSpace(sumField))
                sum = await query.SumAsync(sumField);

            if (pageSize != null && pageNumber != null)
            {
                var skip = (pageNumber - 1) * pageSize;

                query = query
                    .Skip(skip.Value)
                    .Take(pageSize.Value);
            }

            var records = await _mapper.ProjectTo<ListDto>(query).ToListAsync();

            return new PagingDto
            {
                DataCount = dataCount ?? 0,
                Sum = Math.Round(sum ?? 0, 2),
                Records = records.Count == 0 ? new List<ListDto>() : records
            };
        }

        public virtual async Task <MemoryStream> ExportToExcel(FilterDto parameters, string sortField = null, bool sortOrder = true)
        {
            var query = PrepareGetQuery(parameters);

            if (!string.IsNullOrWhiteSpace(sortField))
            {
                if (sortOrder)
                    query = query.OrderBy(sortField);
                else
                    query = query.OrderByDescending(sortField);
            }

            var data = await _mapper.ProjectTo<ExportDto>(query).ToListAsync();

            // Converts incoming data to datatable in the order of Dto.
            string[] props = null;
            if (data.FirstOrDefault() != null)
            {
                props = (data.FirstOrDefault().GetType())
                    .GetProperties()
                    .Select(s => s.Name.ToString())
                    .ToArray();
            }

            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(data, props))
            {
                table.Load(reader);
            }

            string sheetName = typeof(Entity).Name;

            using (XLWorkbook workBook = new XLWorkbook())
            {
                workBook.Worksheets.Add(table, sheetName);

                MemoryStream stream = new MemoryStream();
                workBook.SaveAs(stream);

                return stream;
            }
        }
    }
}