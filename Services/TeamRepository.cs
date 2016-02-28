using System.Collections.Generic;
using CountDownAPI.Elastic;
using CountDownAPI.Models;

namespace CountDownAPI.Services
{
    public class TeamRepository : ElasticRepository<Team>, ITeamRepository
    {
        //TODO: have config with ES address injected
        public TeamRepository(ILog logger) : base("http://localhost:9200/", "team") 
        {
            _logger= logger;
        }
        
        public bool Add(Team item)
        {
            return base.add(item);
        }

        public Team FindByKey(string key)
        {
            Team team = null;
            
            _logger.Log($"Find: {key}");
            var result = base.getByKey(key);
            
            //TODO: make better, this is just grabbing the first document, there should only be one but hey....
            if(result.Found)
            {
                team = result.Source;
            } 
            
            return team;
        }
        
        public bool TeamExists(string key) 
        {
            return base.Exists(key);
        }   
             
        public IEnumerable<Team> GetAll()
        {
            _logger.Log("TeamRepository GetAll()");
            
            var searchResults = base.search();
                   
            if(!searchResults.IsValid)
            {
                _logger.Log($"search invalid");
                _logger.Log($"debuginfo  : {searchResults.CallDetails.DebugInformation}");
                _logger.Log($"calldetails: {searchResults.CallDetails.ToString()}");
                
                return null;
            }
            
            _logger.Log($"search valid");
            
            return new List<Team>(searchResults.Documents);
        }

        public Team Remove(string key)
        {
            return null; //TODO
        }

        public bool Update(Team item)
        {
            //todo, check key
            //TODO: update with versioning
            
            return base.update(item);
        }
    }
}