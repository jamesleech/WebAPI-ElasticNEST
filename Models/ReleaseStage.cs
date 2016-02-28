using System;
using CountDownAPI.Elastic;

namespace CountDownAPI.Models
{
    public class ReleaseStage : ElasticBase
    {
        public string Description { get; set; }
        public DateTime Scheduled { get; set; }
        public DateTime Actual { get; set; }
        public bool IsComplete { get; set; }
    }
}