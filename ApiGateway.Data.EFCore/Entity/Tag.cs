namespace ApiGateway.Data.EFCore.Entity
{
    public class Tag : EntityBase
    {
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string DataType { get; set; }
    }
}