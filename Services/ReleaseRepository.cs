using System.Collections.Generic;
using CountDownAPI.Elastic;
using CountDownAPI.Models;

namespace CountDownAPI.Services
{
    public class ReleaseRepository : ElasticRepository<Release>, IReleaseRepository
    {
        private ITeamRepository _teamRepo;

        //TODO: have config with ES address injected
        public ReleaseRepository(ILog logger, ITeamRepository teamRepo) : base("http://localhost:9200/", "release") 
        {
            _logger= logger;
            _teamRepo = teamRepo;
        }
        
        public IEnumerable<Release> GetAll()
        {
            _logger.Log("ReleaseRepository GetAll()");
            
            var searchResults = base.search();
                   
            if(!searchResults.IsValid)
            {
                _logger.Log($"search invalid");
                _logger.Log($"debuginfo  : {searchResults.CallDetails.DebugInformation}");
                _logger.Log($"calldetails: {searchResults.CallDetails.ToString()}");
                
                return null;
            }
            
            _logger.Log($"search valid");
            
            return new List<Release>(searchResults.Documents);
        }
                
        public bool Add(Release release)
        {
            if(string.IsNullOrWhiteSpace(release.Team)){
                return false;
            }
            
            if(_teamRepo.TeamExists(release.Team)) {
                return base.add(release);
            } else {
                return false; //TODO: yeah bool's suck
            }
                        
        }
        
        public bool Update(Release release)
        {
            //todo, check key
            //TODO: update with versioning
            if(string.IsNullOrWhiteSpace(release.Team)){
                return false;
            }
            
            if(_teamRepo.TeamExists(release.Team)) {
                return base.update(release);
            } else {
                return false; //TODO: yeah bool's suck
            }
        }

        public Release FindByKey(string key)
        {
            Release release = null;
            
            _logger.Log($"Find: {key}");
            var result = base.getByKey(key);
            
            //TODO: make better, this is just grabbing the first document, there should only be one but hey....
            if(result.Found)
            {
                release = result.Source;
            } 
            
            return release;
        }
        
        public IEnumerable<Release> FindByTeam(string team)
        {
            _logger.Log($"ReleaseRepository FindByTeam({team})");
                        
            var searchResults =  elastic.Search<Release>(s=>s
                .From(0).Size(50)
                .Query(q => q
                    .Match(m => m
                        .Field("team")
                        .Query(team)
                        )
                    )
                ); 
                   
            if(!searchResults.IsValid)
            {
                _logger.Log($"search invalid");
                _logger.Log($"debuginfo  : {searchResults.CallDetails.DebugInformation}");
                _logger.Log($"calldetails: {searchResults.CallDetails.ToString()}");
                
                return null;
            }
            
            _logger.Log($"search team valid");
                _logger.Log($"debuginfo  : {searchResults.CallDetails.DebugInformation}");
                _logger.Log($"calldetails: {searchResults.CallDetails.ToString()}");
            
            return new List<Release>(searchResults.Documents); 
        }
        
        public Release Remove(string key)
        {
            //TODO: is this needed ?
            return null;
        }
    }
}