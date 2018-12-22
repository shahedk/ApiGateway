namespace ApiGateway.Common.Models
{
    public class ApiSummaryModel : ApiModel
    {
        public int ActiveRoleCount { get; set; }
        public int DisabledRoleCount { get; set; }
        public ApiSummaryModel(){}

        public ApiSummaryModel(ApiModel model)
        {
            ServiceId = model.ServiceId;
            Name = model.Name;
            HttpMethod = model.HttpMethod;
            CustomHeaders = model.CustomHeaders;
            Url = model.Url;
            
            // Roles = model.Roles;
            
            Id = model.Id;
            OwnerKeyId = model.OwnerKeyId;
            CreateDate = model.CreateDate;
            ModifiedDate = model.ModifiedDate;
        }
    }
}