namespace ApiGateway.Common.Models
{
    public class KeySummaryModel : KeyModel
    {
        public int ActiveRoleCount { get; set; }
        public int DisabledRoleCount { get; set; }

        public KeySummaryModel()
        {
            
        }

        public KeySummaryModel(KeyModel model)
        {
            IsDisabled = model.IsDisabled;
            PublicKey = model.PublicKey;
            Type = model.Type;
            Properties = model.Properties;
            
            //Roles = model.Roles;

            Id = model.Id;
            OwnerKeyId = model.OwnerKeyId;
            CreateDate = model.CreateDate;
            ModifiedDate = model.ModifiedDate;
        }
    }
}