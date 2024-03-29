﻿using RecipesManagerApi.Application.IServices;
using RecipesManagerApi.Application.Models.CreateDtos;
using RecipesManagerApi.Application.Models.Dtos;
using RecipesManagerApi.Application.Models.Operations;

namespace RecipesManagerApi.Infrastructure.Mutations;

[ExtendObjectType(OperationTypeNames.Mutation)]
public class SharedRecipesMutation
{
	public Task<SharedRecipeDto> AddSharedRecipeAsync(SharedRecipeCreateDto dto, CancellationToken cancellationToken,
	[Service] ISharedRecipesService recipesService)
	=> recipesService.AddSharedRecipeAsync(dto, cancellationToken);
	
	public Task<OperationDetails> DeleteSharedRecipeAsync(string id, CancellationToken cancellationToken,
	[Service] ISharedRecipesService recipesService)
	=> recipesService.DeleteSharedRecipeAsync(id, cancellationToken);
	
	public Task<SharedRecipeDto> AccessSharedRecipeAsync(string id, CancellationToken cancellationToken,
	[Service] ISharedRecipesService recipesService)
	=> recipesService.AccessSharedRecipeAsync(id, cancellationToken);
}
