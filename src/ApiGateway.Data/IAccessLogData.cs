using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data
{
    public interface IAccessLogData: IEntityData<AccessLogModel>
    {
        Task<IList<AccessLogModel>> Get(string ownerKeyId, string serviceId, DateTime start, DateTime end);
        Task<IList<AccessLogModel>> Get(string ownerKeyId, string serviceId, string apiId, DateTime start, DateTime end);
    }
}