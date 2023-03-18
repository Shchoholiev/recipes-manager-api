﻿using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models;
using RecipesManagerApi.Application.Paging;
using RecipesManagerApi.Domain.Entities;
using System.Linq.Expressions;

namespace RecipesManagerApi.Infrastructure.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository _repository;

        private readonly IMapper _mapper;

        public CategoriesService(ICategoriesRepository repository, IMapper mapper)
        {
            this._mapper = mapper;
            this._repository = repository;
        }

        public async Task AddCategoryAsync(CategoryDto dto, CancellationToken cancellationToken)
        {
            var entity = this._mapper.Map<Category>(dto);
            await this._repository.AddAsync(entity, cancellationToken);
        }

        public async Task<CategoryDto> GetCategoryAsync(string id, CancellationToken cancellationToken)
        {
            if (!ObjectId.TryParse(id, out var objectId)) {
                throw new Exception();
            }
            var entity = await this._repository.GetCategoryAsync(objectId, cancellationToken);
            return this._mapper.Map<CategoryDto>(entity);
        }

        public async Task<PagedList<CategoryDto>> GetCategoriesPageAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var entities = await this._repository.GetPageAsync(pageNumber, pageSize, cancellationToken);
            var dtos = this._mapper.Map<List<CategoryDto>>(entities);
            var count = await this._repository.GetTotalCountAsync();
            return new PagedList<CategoryDto>(dtos, pageNumber, pageSize, count);
        }
    }
}
