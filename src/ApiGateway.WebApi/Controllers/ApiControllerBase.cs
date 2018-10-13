using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Constants;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [ApiController]
    public class ApiControllerBase : Controller
    {
        private readonly IApiRequestHelper _apiRequestHelper;

        public ApiControllerBase(IApiRequestHelper apiRequestHelper)
        {
            _apiRequestHelper = apiRequestHelper;
        }

        protected string ApiKeyType => _apiRequestHelper.GetApiKeyType();

        protected string ApiKey => _apiRequestHelper.GetApiKey();

        protected string ApiSecret => _apiRequestHelper.GetApiSecret();
    }
}
