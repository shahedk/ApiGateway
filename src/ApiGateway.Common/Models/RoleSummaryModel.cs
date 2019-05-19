namespace ApiGateway.Common.Models
{
    public class RoleSummaryModel : RoleModel
    {
        public int KeyCount { get; set; }
        public int ApiCount { get; set; }
        public int AccessRuleCount { get; set; }

        public int ServiceCount { get; set; }

        public RoleSummaryModel(){}
        
        public RoleSummaryModel(RoleModel model)
        {
            Name = model.Name;
            IsDisabled = model.IsDisabled;

            Id = model.Id;
            OwnerKeyId = model.OwnerKeyId;
            CreateDate = model.CreateDate;
            ModifiedDate = model.ModifiedDate;
        }
    }
}