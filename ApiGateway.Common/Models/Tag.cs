using Newtonsoft.Json;

namespace ApiGateway.Common.Models
{
    public class Tag
    {
        public string Name { get; set; }
        public string Value { get; set; } = string.Empty;

        public Tag()
        {

        }

        public Tag(string json)
        {
            var x = JsonConvert.DeserializeObject<Tag>(json);
            this.Name = x.Name;
            this.Value = x.Value;
        }

        public override string ToString()
        {
            var json = JsonConvert.SerializeObject(this);
            return json;
        }
    }
}