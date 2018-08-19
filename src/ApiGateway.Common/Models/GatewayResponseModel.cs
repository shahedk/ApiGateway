using System;
using System.Collections.Generic;
using System.Text;

namespace ApiGateway.Common.Models
{
    public class GatewayResponseModel
    {
        public int Status { get; set; }
        public string Result { get; set; }
        public string Error { get; set; }
    }
}
