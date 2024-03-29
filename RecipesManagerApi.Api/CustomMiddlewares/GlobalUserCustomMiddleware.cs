﻿using MongoDB.Bson;
using RecipesManagerApi.Application.GlodalInstances;
using System.Security.Claims;

namespace RecipesManagerApi.Api.CustomMiddlewares;

public class GlobalUserCustomMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalUserCustomMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if(ObjectId.TryParse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out ObjectId id))
        {
            GlobalUser.Id = id;
        }
        GlobalUser.Name = httpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        GlobalUser.Email = httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        GlobalUser.Phone = httpContext.User.FindFirst(ClaimTypes.MobilePhone)?.Value;
        foreach(var role in httpContext.User.FindAll(ClaimTypes.Role))
        {
            GlobalUser.Roles.Add(role.Value);
        }
        await this._next(httpContext);
    }
}