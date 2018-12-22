namespace ApiGateway.Common.Models
{
    public class RoleSummaryModel : RoleModel
    {
        public int ApiCount { get; set; }
        public int AccessRuleCount { get; set; }
    }
}