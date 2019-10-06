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
using System.Reflection;
using System.Threading.Tasks;
using vecihi.database.model;
using vecihi.helper;
using vecihi.helper.Attributes;
using vecihi.helper.Const;
using vecihi.helper.Extensions;
using vecihi.infrastructure.entity.dtos;
using vecihi.infrastructure.entity.models;
using static vecihi.helper.Const.Enums;

namespace vecihi.infrastructure
{
    public interface IServiceBase<AddDto, UpdateDto, ListDto, CardDto, PagingDto, ExportDto, FilterDto, Entity, Type>
        : ICRUDInterface<AddDto, UpdateDto, ListDto, CardDto, PagingDto, ExportDto, FilterDto, Type>
        where Type : struct
        where Entity : ModelBase<Type>
        where UpdateDto : IDtoUpdateBase<Type>
        where ListDto : IDtoGetBase<Type>
        where CardDto : IDtoGetBase<Type>
        where PagingDto : DtoPagingBase<Type, ListDto>, new()
    {
        Entity AddMapping(AddDto model, Type userId);
        ICollection<ChildEntity> AddMappingChild<ChildEntity>(ICollection<ChildEntity> entities, Type userId)
           where ChildEntity : ModelBase<Type>;
        ICollection<ChildEntity> AddMappingChild<ChildEntity, ChildModel>(ICollection<ChildEntity> entities, IList<ChildModel> models, Type userId)
            where ChildEntity : ModelBase<Type>
            where ChildModel : IDtoUpdateBase<Type>;
        Task<Entity> UpdateMapping(UpdateDto model, Type userId);
        Task<IList<Type>> UpdateMappingChild<ChildEntity, ChildModel>(IList<ChildModel> models, Type userId)
            where ChildEntity : ModelBase<Type>
            where ChildModel : IDtoUpdateBase<Type>;
        Task<Entity> DeleteMapping(Type id, Type userId, IList<string> navigations = null);
        ICollection<ChildEntity> DeleteMappingChild<ChildEntity>(ICollection<ChildEntity> entities, Type userId)
            where ChildEntity : ModelBase<Type>;
        IQueryable<Entity> PrepareGetQuery(FilterDto parameters, string sortField = null, bool sortOrder = true);
    }

    public abstract class ServiceBase<AddDto, UpdateDto, ListDto, CardDto, PagingDto, ExportDto, FilterDto, Entity, Type>
        : IServiceBase<AddDto, UpdateDto, ListDto, CardDto, PagingDto, ExportDto, FilterDto, Entity, Type>
        where Type : struct
        where Entity : ModelBase<Type>
        where UpdateDto : IDtoUpdateBase<Type>
        where ListDto : IDtoGetBase<Type>
        where CardDto : IDtoGetBase<Type>
        where PagingDto : DtoPagingBase<Type, ListDto>, new()
    {
        protected UnitOfWork<Type> _uow;
        protected IMapper _mapper;

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

            if (entity is IModelAuditBase<Type, User>)
            {
                (entity as IModelAuditBase<Type, User>).CreatedBy = userId;
                (entity as IModelAuditBase<Type, User>).CreatedAt = DateTime.UtcNow;
            }

            return entity;
        }

        public virtual ICollection<ChildEntity> AddMappingChild<ChildEntity>(ICollection<ChildEntity> entities, Type userId)
            where ChildEntity : ModelBase<Type>
        {
            entities.ToList().ForEach(entity =>
            {
                if (entity is IModelAuditBase<Type, User>)
                {
                    (entity as IModelAuditBase<Type, User>).CreatedBy = userId;
                    (entity as IModelAuditBase<Type, User>).CreatedAt = DateTime.UtcNow;
                }
            });

            return entities;
        }

        public virtual ICollection<ChildEntity> AddMappingChild<ChildEntity, ChildModel>(ICollection<ChildEntity> entities, IList<ChildModel> models, Type userId)
            where ChildEntity : ModelBase<Type>
            where ChildModel : IDtoUpdateBase<Type>
        {
            if (models.Count > 0)
            {
                var newRecordsModel = models.Where(x => x.Id.Equals(null) || x.Id.Equals(Guid.Empty)).ToList();
                _mapper.Map(newRecordsModel, entities);
            }

            return AddMappingChild(entities, userId);
        }

        public virtual async Task<ApiResult> Add(AddDto model, Type userId, bool isCommit = true)
        {
            Entity entity = AddMapping(model, userId);

            await _uow.Repository<Entity>().AddAsync(entity);

            if (isCommit)
                await _uow.SaveChangesAsync();

            return new ApiResult { Data = entity.Id, Message = ApiResultMessages.Ok };
        }

        public virtual async Task<Entity> UpdateMapping(UpdateDto model, Type userId)
        {
            Entity entity = await _uow.Repository<Entity>().GetById(model.Id);

            if (entity is IModelAuditBase<Type, User>)
            {
                (entity as IModelAuditBase<Type, User>).UpdatedBy = userId;
                (entity as IModelAuditBase<Type, User>).UpdatedAt = DateTime.UtcNow;
            }

            _mapper.Map(model, entity);

            return entity;
        }

        public virtual async Task<IList<Type>> UpdateMappingChild<ChildEntity, ChildModel>(IList<ChildModel> models, Type userId)
            where ChildEntity : ModelBase<Type>
            where ChildModel : IDtoUpdateBase<Type>
        {
            var updateRecordsModel = models.Where(x => !(x.Id.Equals(null) || x.Id.Equals(Guid.Empty))).ToList();
            var Ids = updateRecordsModel.Select(s => s.Id).ToList();

            var query = _uow.Repository<ChildEntity>().Get().Where(x => Ids.Contains(x.Id));

            var entities = await query.ToListAsync();

            foreach (var entity in entities)
            {
                _mapper.Map(updateRecordsModel.Where(x => x.Id.Equals(entity.Id)).FirstOrDefault(), entity);

                if (entity is IModelAuditBase<Type, User>)
                {
                    (entity as IModelAuditBase<Type, User>).UpdatedAt = DateTime.UtcNow;
                    (entity as IModelAuditBase<Type, User>).UpdatedBy = userId;
                }
            }

            return Ids;
        }

        public virtual async Task<ApiResult> Update(UpdateDto model, Type userId, bool isCommit = true, bool checkAuthorize = false)
        {
            Entity entity = await UpdateMapping(model, userId);

            if (entity == null)
                return new ApiResult { Data = model.Id, Message = ApiResultMessages.GNE0001 };

            // Access Control
            if (entity is IModelAuditBase<Type, User> && checkAuthorize && !(entity as IModelAuditBase<Type, User>).CreatedBy.Equals(userId))
                return new ApiResult { Data = model.Id, Message = ApiResultMessages.GNW0001 };

            if (isCommit)
                await _uow.SaveChangesAsync();

            return new ApiResult { Data = entity.Id, Message = ApiResultMessages.Ok };
        }

        public virtual async Task<Entity> DeleteMapping(Type id, Type userId, IList<string> navigations = null)
        {
            var query = _uow.Repository<Entity>().Get().Equal("Id", id);

            if (navigations != null)
            {
                foreach (var navigation in navigations)
                    query = query.Include(navigation);
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual ICollection<ChildEntity> DeleteMappingChild<ChildEntity>(ICollection<ChildEntity> entities, Type userId)
            where ChildEntity : ModelBase<Type>
        {
            entities.ToList().ForEach(entity =>
            {
                if (entity is IModelAuditBase<Type, User>)
                {
                    (entity as IModelAuditBase<Type, User>).UpdatedAt = DateTime.UtcNow;
                    (entity as IModelAuditBase<Type, User>).UpdatedBy = userId;
                }

                entity.IsDeleted = true;
            });

            return entities;
        }

        public virtual async Task<ApiResult> Delete(Type id, Type userId, bool isCommit = true, bool checkAuthorize = false)
        {
            Entity entity = await DeleteMapping(id, userId);

            if (entity == null)
                return new ApiResult { Data = id, Message = ApiResultMessages.GNE0001 };

            if (entity is IModelAuditBase<Type, User>)
            {
                // Access Control
                if (checkAuthorize && !(entity as IModelAuditBase<Type, User>).CreatedBy.Equals(userId))
                    return new ApiResult { Message = ApiResultMessages.GNW0001 };

                (entity as IModelAuditBase<Type, User>).UpdatedAt = DateTime.UtcNow;
                (entity as IModelAuditBase<Type, User>).UpdatedBy = userId;
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

        public virtual IQueryable<Entity> OrderQuery(IQueryable<Entity> query, string sortField = null, bool sortOrder = true)
        {
            if (string.IsNullOrWhiteSpace(sortField))
                return query;

            var prop = typeof(ListDto).GetProperty(sortField, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            if (prop == null)
                return query;

            var att = prop.CustomAttributes
                .Where(x => x.AttributeType == typeof(OrderAttribute))
                .Select(st => st.ConstructorArguments
                    .Select(sc => sc.Value)
                    .ToList())
                .FirstOrDefault();

            // if use order attribute.
            if (att != null)
            {
                if ((bool)att[1])
                    return query;

                sortField = (att[0] != null && !string.IsNullOrWhiteSpace(att[0].ToString())) ? att[0].ToString() : sortField;
            }

            if (sortOrder)
                query = query.OrderBy(sortField);
            else
                query = query.OrderByDescending(sortField);

            return query;
        }

        public virtual IQueryable<Entity> PrepareGetQuery(FilterDto parameters, string sortField = null, bool sortOrder = true)
        {
            var query = _uow.Repository<Entity>().Query();

            var properties = typeof(FilterDto)
                .GetProperties()
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
                        query = searchType switch
                        {
                            SearchType.Contains => query.Contains(dbName, prop.value.ToString()),
                            SearchType.Equal => query.Equal(dbName, prop.value.ToString()),
                            _ => throw new Exception(string.Format(
                               format: "{0} is a string type property. You can not query a property of type {1} {2}.",
                               arg0: dbName, arg1: prop.type, arg2: searchType)),
                        };
                    }
                }

                else if (prop.type == TypeCode.Int32 || prop.type == TypeCode.Double || prop.type == TypeCode.Int16 ||
                        prop.type == TypeCode.Decimal || prop.type == TypeCode.Byte || prop.type == TypeCode.Int64 ||
                        prop.type == TypeCode.UInt16 || prop.type == TypeCode.UInt32 || prop.type == TypeCode.UInt64)
                {
                    query = searchType switch
                    {
                        SearchType.Equal => query.Equal(dbName, prop.value),
                        SearchType.GreaterThan => query.GreaterThan(dbName, prop.value),
                        SearchType.GreaterThanOrEqual => query.GreaterThanOrEqual(dbName, prop.value),
                        SearchType.LessThan => query.LessThan(dbName, prop.value),
                        SearchType.LessThanOrEqual => query.LessThanOrEqual(dbName, prop.value),
                        _ => throw new Exception(string.Format(
                                format: "{0} is a numeric type property. You can not query a property of type {1} {2}.",
                                arg0: dbName, arg1: prop.type, arg2: searchType)),
                    };
                }

                else if (prop.type == TypeCode.Boolean)
                {
                    query = searchType switch
                    {
                        SearchType.Equal => query = query.Equal(dbName, prop.value),
                        _ => throw new Exception(string.Format(
                            format: "{0} is a boolean type property. You can not query a property of type {1} {2}.",
                            arg0: dbName, arg1: prop.type, arg2: searchType))
                    };
                }

                else if (prop.type == TypeCode.Object)
                {
                    if (!Guid.Parse(prop.value.ToString()).IsNullOrEmpty())
                    {
                        query = searchType switch
                        {
                            SearchType.Equal => query.Equal(dbName, prop.value),
                            _ => throw new Exception(string.Format(
                               format: "{0} is a object type property. You can not query a property of type {1} {2}.",
                               arg0: dbName, arg1: prop.type, arg2: searchType)),
                        };
                    }
                }

                else if (prop.type == TypeCode.DateTime)
                {
                    query = searchType switch
                    {
                        SearchType.Equal => query.DiffDaysEqual(dbName, prop.value),
                        SearchType.GreaterThanOrEqual => query.DiffDaysGreaterThan(dbName, prop.value),
                        SearchType.LessThanOrEqual => query.DiffDaysLessThan(dbName, prop.value),
                        _ => throw new Exception(string.Format(
                                format: "{0} is a datetime type property. You can not query a property of type {1} {2}.",
                                arg0: dbName, arg1: prop.type, arg2: searchType)),
                    };
                }
            }

            return OrderQuery(query, sortField, sortOrder);
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
            var query = PrepareGetQuery(parameters, sortField, sortOrder);

            return await _mapper.ProjectTo<ListDto>(query).ToListAsync();
        }

        public virtual async Task<PagingDto> GetPaging(FilterDto parameters, string sortField = null, bool sortOrder = true,
            string sumField = null, int? pageSize = null, int? pageNumber = null)
        {
            var query = PrepareGetQuery(parameters, sortField, sortOrder);

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

        public virtual async Task<MemoryStream> ExportToExcel(FilterDto parameters, string sortField = null, bool sortOrder = true)
        {
            var query = PrepareGetQuery(parameters, sortField, sortOrder);

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

            using XLWorkbook workBook = new XLWorkbook();
            workBook.Worksheets.Add(table, sheetName);

            MemoryStream stream = new MemoryStream();
            workBook.SaveAs(stream);

            return stream;
        }
    }
}