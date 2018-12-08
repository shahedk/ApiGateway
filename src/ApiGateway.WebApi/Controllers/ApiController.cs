using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApiGateway.Client;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Models;
using ApiGateway.Core;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("sys/Api")]
    public class ApiController : ApiControllerBase
    {
        private readonly IApiManager _manager;

        public ApiController(IApiManager manager, IApiRequestHelper apiRequestHelper) : base(apiRequestHelper)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IList<ApiModel>> Get()
        {
            try
            {
                return await _manager.GetAll(ApiKey);
            }
            catch (ApiGatewayException e)
            {
                if (e.ErrorCode == HttpStatusCode.NotFound)
                {
                    Response.StatusCode = (int)e.ErrorCode;
                    
                }
            }
            return null;
        }

        // GET: api/Api/5
        [HttpGet("/sys/api-detail/{id}")]
        public async Task<ApiModel> Get(string id)
        {
            return await _manager.Get(ApiKey, id);
        }
        
        // POST: api/Api
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ApiInsertModel insertModel)
        {
            var model = new ApiModel
            {
                ServiceId = insertModel.ServiceId,
                HttpMethod = insertModel.HttpMethod,
                Name = insertModel.Name, 
                Url = insertModel.Url, 
                CustomHeaders = insertModel.CustomHeaders

            };

            if (ModelState.IsValid)
            {
                var apiModel = await _manager.Create(ApiKey, model);
                return Ok( apiModel);    
            }
            else
            {
                return BadRequest(ModelState);
            }
            
        }
        
        // PUT: api/Api/5
        [HttpPut("{id}")]
        public async Task<ApiModel> Put(string id, [FromBody]ApiModel model)
        {
            model.Id = id;
            var apiModel = await _manager.Update(ApiKey, model);

            return apiModel;
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public  async Task  Delete(string id)
        {
            await _manager.Delete(ApiKey, id);
        }
    }
}
