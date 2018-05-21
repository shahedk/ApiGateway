using System.Collections.Generic;

namespace ApiGateway.Common.Model
{
    public class AccessRuleModel : ModelBase
    {
        public string Id { get; set; }
        public string ServiceId { get; set; }
        public string Name{ get; set; }
        public string Description { get; set; }
        public List<Tag> Tags { get; set; }
        public string Type { get; set; }
        public List<KeyProperty> Properties { get; set; }
    }
}