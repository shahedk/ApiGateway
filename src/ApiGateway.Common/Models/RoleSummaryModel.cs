namespace ApiGateway.Common.Models
{
    public class RoleSummaryModel : RoleModel
    {
        public int KeyCount { get; set; }
        public int ApiCount { get; set; }
        public int AccessRuleCount { get; set; }

        public RoleSummaryModel(){}
        
        public RoleSummaryModel(RoleModel model)
        {
            ServiceId = model.ServiceId;
            Name = model.Name;
            IsDisabled = model.IsDisabled;

            Id = model.Id;
            OwnerId = model.OwnerId;
            CreateDate = model.CreateDate;
            ModifiedDate = model.ModifiedDate;
        }
    }
}