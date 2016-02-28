using System.Collections.Generic;
using CountDownAPI.Models;
using CountDownAPI.Services;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CountDownAPI.Controllers
{
    [Route("api/[controller]")]
    public class TeamController : Controller
    {        
        [FromServices]
        public ITeamRepository TeamRepo { get; set; }
        
        [FromServices]
        public IReleaseRepository ReleaseRepo { get; set; }
        
        [FromServices]
        public ILog _logger { get; set;}
        
        // GET: api/team
        [HttpGet]
        public IEnumerable<Team> Get()
        {
            _logger.Log($"Get teams");
            return TeamRepo.GetAll(); 
        }

        // GET api/team/SharedServices
        [HttpGet("{key}", Name = "GetTeam")]
        public IActionResult GetByKey(string key)
        {
            _logger.Log($"key:{key}");
            var item = TeamRepo.FindByKey(key);
            
            if (item == null)
            {
                _logger.Log($"GetById: could not find Team {key}");
                return HttpNotFound();
            }
            return new ObjectResult(item);
        }
        
        // Get api/team/SharedServices/releases
        [HttpGet("{key}/releases", Name = "GetTeamReleases")]
        public IActionResult GetByTeam(string key)
        {
            _logger.Log($"key:{key}");
            var releases = ReleaseRepo.FindByTeam(key);
            
            if (releases == null)
            {
                _logger.Log($"GetById: could not find Team {key}");
                return HttpNotFound();
            }
            return new ObjectResult(releases);
        }
        
        // POST api/team
        [HttpPost]
        public IActionResult Post([FromBody]Team newTeam)
        {
            _logger.Log($"add team");
            if (newTeam == null)
            {
                return HttpBadRequest("couldn't serialise value");
            }
            
            _logger.Log($"adding release with key: {newTeam.Key}");
            TeamRepo.Add(newTeam);
            return CreatedAtRoute("GetTeam", new { controller = "Team", key = newTeam.Key }, newTeam);
        }

        // PUT api/team/SharedServices
        [HttpPut("{id}")]
        public IActionResult Put(string key, [FromBody]Team updateTeam)
        {
            _logger.Log($"key:{updateTeam.Key}");
            
            if (updateTeam == null || updateTeam.Key.ToString() != key)
            {
                return HttpBadRequest();
            }
            
            var release = TeamRepo.FindByKey(key);
            if (release == null)
            {
                return HttpNotFound();
            }
            
            TeamRepo.Update(updateTeam);
            return new NoContentResult();
        }

        // DELETE api/team/SharedServices
        [HttpDelete("{key}")]
        public IActionResult Delete(string key)
        {
            _logger.Log($"key:{key}");
            TeamRepo.Remove(key);
            return new NoContentResult();
        }
    }
}
