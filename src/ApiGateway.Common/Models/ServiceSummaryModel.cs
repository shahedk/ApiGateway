namespace ApiGateway.Common.Models
{
    public class ServiceSummaryModel : ServiceModel
    {
        public int ActiveRoleCount { get; set; }
        public int DisabledRoleCount { get; set; }
        public int AccessRuleCount { get; set; }
        
        public int ApiCount { get; set; }
        
        public ServiceSummaryModel(){}

        public ServiceSummaryModel(ServiceModel model)
        {
            Id = model.Id;
            OwnerKeyId = model.OwnerKeyId;
            CreateDate = model.CreateDate;
            ModifiedDate = model.ModifiedDate;
            
            Name = model.Name;   
        }
    }
}