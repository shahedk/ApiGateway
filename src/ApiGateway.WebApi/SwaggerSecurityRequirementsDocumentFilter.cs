using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiGateway.WebApi
{
    public class SwaggerSecurityRequirementsDocumentFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();
                

            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "apikey",
                In = "header",
                Type = "string",
                Required = true,
                Default = ""
            });
            
            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "apisecret",
                In = "header",
                Type = "string",
                Required = true,
                Default = ""
            });
            
            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "servicekey",
                In = "header",
                Type = "string",
                Required = true,
                Default = ""
            });
            
            operation.Parameters.Add(new NonBodyParameter
            {
                Name = "servicesecret",
                In = "header",
                Type = "string",
                Required = true,
                Default = ""
            });
        }
    }
}