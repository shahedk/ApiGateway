﻿using System;
using System.Threading.Tasks;
using ApiGateway.Common.Constants;
using ApiGateway.Common.Exceptions;
using ApiGateway.Common.Models;
using Xunit;

namespace ApiGateway.WebApi.Test
{
    public class ApiTest : WebApiTestBase
    {
        private async Task<ServiceModel> GetServiceModel()
        {
            var serviceTest = new ServiceTest();
            var serviceModel = await serviceTest.Create();

            return serviceModel;
        }

        [Fact]
        public async Task<ApiModel> Create()
        {
            var controller = await GetApiController();

            var serviceModel = await GetServiceModel();
            
            var apiModel = new ApiModel()
            {
                Name = "Test Api " + DateTime.Now,
                ServiceId = serviceModel.Id,
                HttpMethod = ApiHttpMethods.Get,
                Url = "http://testapi.com"
            };

            var savedApiModel = await controller.Post(apiModel);

            Assert.Equal(apiModel.Name, savedApiModel.Name);

            return savedApiModel;
        }

        [Fact]
        public async Task Update()
        {
            var controller = await GetApiController();

            var apiModel = await Create();

            apiModel.Name = DateTime.Now.ToString();

            var savedApiModel = await controller.Put(apiModel.Id, apiModel);

            Assert.Equal(apiModel.Name, savedApiModel.Name);

        }

        public async Task AddApiInRole()
        {

        }

        public async Task RemoveApiFromRole()
        {

        }

        [Fact]
        public async Task Get()
        {
            var controller = await GetApiController();
            var existing = await Create();

            var savedModel = await controller.Get(existing.Id);

            Assert.Equal(existing.Name, savedModel.Name);
        }

        [Fact]
        public async Task Delete()
        {
            var controller = await GetApiController();
            var existing = await Create();

            await controller.Delete(existing.Id);

            try
            {
                var saved = await controller.Get(existing.Id);
                Assert.Null(saved);
            }
            catch (Exception ex)
            {
                Assert.True( ex is ItemNotFoundException);
            }
        }
    }
}