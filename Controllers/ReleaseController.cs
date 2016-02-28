using CountDownAPI.Models;
using CountDownAPI.Services;
using Microsoft.AspNet.Mvc;

namespace CountDownAPI.Controllers
{
    [Route("api/[controller]")]
    public class ReleaseController : Controller
    {
        [FromServices]
        public IReleaseRepository ReleaseRepo { get; set; }
        
        [FromServices]
        public ILog _logger { get; set;}
        
        // GET: api/release
        [HttpGet]
        public IActionResult Get()
        {
            _logger.Log($"Get releases");
            return new ObjectResult(ReleaseRepo.GetAll()); 
        }

        // GET api/release/Sputnik
        [HttpGet("{key}", Name = "GetRelease")]
        public IActionResult GetByKey(string key)
        {
            _logger.Log($"key:{key}");
            var item = ReleaseRepo.FindByKey(key);
            
            if (item == null)
            {
                _logger.Log($"GetById: could not find release {key}");
                return HttpNotFound();
            }
            return new ObjectResult(item);
        }

        // POST api/release
        [HttpPost]
        public IActionResult Create([FromBody] Release newRelease)
        {
            _logger.Log($"release");
            if (newRelease == null)
            {
                return HttpBadRequest("couldn't serialise value");
            }
            
            _logger.Log($"adding release with key: {newRelease.Key}");
            
            if(ReleaseRepo.Add(newRelease)) 
            {
                return CreatedAtRoute("GetRelease", new { controller = "Release", key = newRelease.Key }, newRelease); }
            else {
                return HttpBadRequest("unable to save release");
            }
        }

        // PUT api/release/5
        [HttpPut("{key}")]
        public IActionResult Put(string key, [FromBody]Release updateRelease)
        {
            _logger.Log($"key:{updateRelease.Key}");
            
            if (updateRelease == null || updateRelease.Key.ToString() != key)
            {
                return HttpBadRequest();
            }
            
            var release = ReleaseRepo.FindByKey(key);
            if (release == null)
            {
                return HttpNotFound();
            }
            
            if(ReleaseRepo.Update(updateRelease)) 
            {
                return new NoContentResult();
            }
            else 
            {
                return HttpBadRequest("unable to save release");
            }
        }

        // DELETE api/release/5
        [HttpDelete("{key}")]
        public IActionResult Delete(string key)
        {
            _logger.Log($"key:{key}");
            ReleaseRepo.Remove(key);
            return new NoContentResult();
        }
    }
}
