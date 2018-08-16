using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGateway.Client;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    public class ApiControllerBase : Controller
    {
        private readonly IApiRequestHelper _apiRequestHelper;

        public ApiControllerBase(IApiRequestHelper apiRequestHelper)
        {
            _apiRequestHelper = apiRequestHelper;
        }

        public string ApiKey => _apiRequestHelper.GetApiKey();

        public string ApiSecret => _apiRequestHelper.GetApiSecret();

        public string ServiceApiKey => _apiRequestHelper.GetServiceApiKey();

        public string ServiceApiSecret => _apiRequestHelper.GetServiceApiSecret();
    }
}
