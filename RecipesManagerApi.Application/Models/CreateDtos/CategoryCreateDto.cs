using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;

namespace RecipesManagerApi.Application.Models.CreateDtos;

[GraphQLName("CategoryInput")]
public class CategoryCreateDto
{
    public string Name { get; set; }
}
