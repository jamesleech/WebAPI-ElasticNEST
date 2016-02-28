using System;

namespace CountDownAPI.Elastic
{
    public interface IElasticBase
    {
        string Key {get;set;}
        string LastUpdatedBy {get;set;}
        DateTime LastUpdate {get;set;}        
    }
}