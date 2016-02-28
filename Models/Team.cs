using System;
using CountDownAPI.Elastic;

namespace CountDownAPI.Models
{
    public class Team : ElasticBase, IElasticBase
    {
        public string Description { get; set; } 
        public Uri Image { get;set;}
        
        //public List<User> Members {get; set; }
        
    }
}