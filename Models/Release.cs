using System;
using System.Collections.Generic;
using CountDownAPI.Elastic;

namespace CountDownAPI.Models
{
    public class Release : ElasticBase, IElasticBase
    {
        public string Team {get;set;}
        public string Description { get; set; } 
        public Uri Image { get;set;}
        public bool IsComplete { get; set; }
        public List<ReleaseStage> Stages {get; set; }
        
    }
}