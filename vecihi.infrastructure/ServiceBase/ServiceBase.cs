﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
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
    public interface IServiceBase<AddDto, UpdateDto, ListDto, CardDto, PagingDto, FilterDto, Entity, Type>
        : ICRUDInterface<AddDto, UpdateDto, ListDto, CardDto, PagingDto, FilterDto, Type>
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

    public abstract class ServiceBase<AddDto, UpdateDto, ListDto, CardDto, PagingDto, FilterDto, Entity, Type>
        : IServiceBase<AddDto, UpdateDto, ListDto, CardDto, PagingDto, FilterDto, Entity, Type>
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

            if (entity is IModelBaseAudit<Type>)
            {
                (entity as IModelBaseAudit<Type>).CreatedBy = userId;
                (entity as IModelBaseAudit<Type>).CreatedAt = DateTime.Now;
            }

            return entity;
        }

        public virtual async Task<ApiResult> Add(AddDto model, Type userId, bool isCommit = true)
        {
            Entity entity = AddMapping(model, userId);

            _uow.Repository<Entity>().Add(entity);

            if (isCommit)
                await _uow.SaveChangesAsync();

            return new ApiResult { Data = entity.Id, Message = ApiResultMessages.Ok };
        }

        public virtual async Task<Entity> UpdateMapping(UpdateDto model, Type userId)
        {
            Entity entity = await _uow.Repository<Entity>().GetById(model.Id);

            if (entity is IModelBaseAudit<Type>)
            {
                (entity as IModelBaseAudit<Type>).UpdatedBy = userId;
                (entity as IModelBaseAudit<Type>).UpdatedAt = DateTime.Now;
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
            if (entity is IModelBaseAudit<Type> && checkAuthorize && !(entity as IModelBaseAudit<Type>).CreatedBy.Equals(userId))
                return new ApiResult() { Data = model.Id, Message = ApiResultMessages.GNW0001 };

            if (isCommit)
                await _uow.SaveChangesAsync();

            return new ApiResult { Data = entity.Id, Message = ApiResultMessages.Ok };
        }

        public virtual async Task<ApiResult> Delete(Type id, Type? userId = null, bool isCommit = true, bool checkAuthorize = false)
        {
            Entity entity = await _uow.Repository<Entity>().GetById(id);

            if (entity == null)
                return new ApiResult { Data = id, Message = ApiResultMessages.GNE0001 };

            if (entity is IModelBaseAudit<Type>)
            {
                // Access Control
                if (checkAuthorize && userId != null && !(entity as IModelBaseAudit<Type>).CreatedBy.Equals(userId.Value))
                    return new ApiResult() { Message = ApiResultMessages.GNW0001 };

                (entity as IModelBaseAudit<Type>).UpdatedAt = DateTime.Now;
                (entity as IModelBaseAudit<Type>).UpdatedBy = userId.Value;
            }

            entity.IsDeleted = true;

            if (isCommit)
                await _uow.SaveChangesAsync();

            return new ApiResult() { Data = id, Message = ApiResultMessages.Ok };
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
                    if (prop.value != null && !string.IsNullOrWhiteSpace(prop.value.ToString()))
                        if (searchType == SearchType.Contains)
                            query = query.Contains(dbName, prop.value.ToString());
                        else if (searchType == SearchType.Equal)
                            query = query.Equal(dbName, prop.value.ToString());
                        else
                            throw new Exception(string.Format(
                                format: "{0} is a string type property. You can not query a property of type {1} {2}.",
                                arg0: dbName, arg1: prop.type, arg2: searchType));
                }

                else if (prop.type == TypeCode.Int32 || prop.type == TypeCode.Double || prop.type == TypeCode.Int16 ||
                        prop.type == TypeCode.Decimal || prop.type == TypeCode.Byte || prop.type == TypeCode.Int64 ||
                        prop.type == TypeCode.UInt16 || prop.type == TypeCode.UInt32 || prop.type == TypeCode.UInt64)
                {
                    if (prop.value != null)
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
                    if (prop.value != null)
                        if (searchType == SearchType.Equal)
                            query = query.Equal(dbName, prop.value);
                        else
                            throw new Exception(string.Format(
                                format: "{0} is a boolean type property. You can not query a property of type {1} {2}.",
                                arg0: dbName, arg1: prop.type, arg2: searchType));
                }

                else if (prop.type == TypeCode.Object)
                {
                    if (prop.value != null && !Guid.Parse(prop.value.ToString()).IsNullOrEmpty())
                        if (searchType == SearchType.Equal)
                            query = query.Equal(dbName, prop.value);
                        else
                            throw new Exception(string.Format(
                                format: "{0} is a object type property. You can not query a property of type {1} {2}.",
                                arg0: dbName, arg1: prop.type, arg2: searchType));
                }

                else if (prop.type == TypeCode.DateTime)
                {
                    if (prop.value != null)
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
            }

            return query;
        }

        public virtual async Task<IList<AutocompleteDto<Type>>> Autocomplete(FilterDto parameters, Type? id = null, string text = null)
        {
            IQueryable<Autocomplete<Type>> query = null;

            if (parameters != null)
                query = _mapper.ProjectTo<Autocomplete<Type>>(PrepareGetQuery(parameters)).AsQueryable();
            else
                query = _mapper.ProjectTo<Autocomplete<Type>>(_uow.Repository<Entity>().Query()).AsQueryable();

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
            string sumField = null, int? first = null, int? rows = null)
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
            {
                var param = Expression.Parameter(typeof(Entity), "sum");
                var prop = Expression.Property(param, sumField);
                var expSum = Expression.Lambda<Func<Entity, double?>>(Expression.Convert(prop, typeof(double?)), param);

                sum = await query.SumAsync(expSum);
            }

            if (first != null && rows != null)
                query = query
                    .Skip(first.Value)
                    .Take(rows.Value);

            var records = await _mapper.ProjectTo<ListDto>(query).ToListAsync();

            return new PagingDto
            {
                DataCount = dataCount ?? 0,
                Sum = Math.Round(sum ?? 0, 2),
                Records = records.Count == 0 ? new List<ListDto>() : records
            };
        }
    }
}