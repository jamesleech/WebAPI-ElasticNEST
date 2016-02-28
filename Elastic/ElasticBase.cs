using System;

namespace CountDownAPI.Elastic
{
    public class ElasticBase : IElasticBase
    {
        public string Key { get; set; }
        public string LastUpdatedBy {get;set;}
        public DateTime LastUpdate {get;set;}
        public int Version {get;set;}

    }
}