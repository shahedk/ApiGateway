using System;

namespace ApiGateway.Common.Model
{
    public class ModelBase
    {
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}