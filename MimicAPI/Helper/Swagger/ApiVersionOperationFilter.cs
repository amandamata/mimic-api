using Microsoft.AspNetCore.Mvc.Controllers;  
using  Microsoft.OpenApi.Models;  
using  Swashbuckle.AspNetCore.SwaggerGen;  
using  Swashbuckle.AspNetCore.Swagger;  
using  System.Collections.Generic;
using System.Linq;

namespace MimicAPI.Helper.Swagger
{
    public class ApiVersionOperationFilter
    {
        public void Apply(Microsoft.AspNetCore.JsonPatch.Operations.Operation operation, OperationFilterContext context)
        {
            var actionApiVersionModel = context.ApiDescription.ActionDescriptor?.GetApiVersion();
            if (actionApiVersionModel == null)
            {
                return;
            }

            if (actionApiVersionModel.DeclaredApiVersions.Any())
            {
                operation.op.SelectMany(p => actionApiVersionModel.DeclaredApiVersions
                    .Select(version => $"{p};v={version.ToString()}")).ToList();
            }
            else
            {
                operation.op.SelectMany(p => actionApiVersionModel.ImplementedApiVersions.OrderByDescending(v => v)
                    .Select(version => $"{p};v={version.ToString()}")).ToList();
            }
        }
    }
}
