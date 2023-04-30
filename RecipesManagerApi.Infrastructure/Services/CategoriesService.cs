﻿using AutoMapper;
using MongoDB.Bson;
using RecipesManagerApi.Application.Exceptions;
using RecipesManagerApi.Application.IRepositories;
using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
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

        public async Task<CategoryDto> AddCategoryAsync(CategoryCreateDto dto, CancellationToken cancellationToken)
        {
            var entity = this._mapper.Map<Category>(dto);
            var newEntity = await this._repository.AddAsync(entity, cancellationToken);
            return this._mapper.Map<CategoryDto>(entity);
        }

        public async Task<CategoryDto> GetCategoryAsync(string id, CancellationToken cancellationToken)
        {
            if (!ObjectId.TryParse(id, out var objectId)) {
                throw new InvalidDataException("Provided id is invalid.");
            }
            
            var entity = await this._repository.GetCategoryAsync(objectId, cancellationToken);
            if (entity == null)
            {
                throw new EntityNotFoundException<Category>();
            }

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
