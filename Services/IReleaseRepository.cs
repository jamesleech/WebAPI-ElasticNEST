using System.Collections.Generic;
using CountDownAPI.Models;

namespace CountDownAPI.Services
{
    public interface IReleaseRepository
    {
        bool Add(Release item);
        IEnumerable<Release> GetAll();
        Release FindByKey(string key);
        IEnumerable<Release> FindByTeam(string team);
        Release Remove(string key);
        bool Update(Release item);
    }
}