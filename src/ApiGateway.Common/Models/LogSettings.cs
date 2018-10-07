namespace ApiGateway.Common.Models
{
    public class LogSettings
    {
        public string ElasticSearchUrl { get; set; }
        public bool UseElasticSearch { get; set; }
        public bool WriteToFile { get; set; }
        public bool WriteToConsole { get; set; }

        public int LogEventLevel { get; set; }
        public string ServiceName { get; set; }
        public string IndexNamePrefix { get; set; }
    }
}