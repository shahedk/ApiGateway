using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiGateway.Common.Models;

namespace ApiGateway.Data.EFCore.DataAccess
{
    public class AccountData : IAccountData
    {
        public Task<AccountModel> Create(AccountModel model)
        {
            throw new NotImplementedException();
        }

        public Task<AccountModel> Update(AccountModel model)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string ownerKeyId, string id)
        {
            throw new NotImplementedException();
        }

        public Task<AccountModel> Get(string ownerKeyId, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<AccountModel>> GetAll(string ownerKeyId)
        {
            throw new NotImplementedException();
        }

        public Task<AccountModel> GetByLoginName(string ownerKeyId, string loginName)
        {
            throw new NotImplementedException();
        }
    }
}
