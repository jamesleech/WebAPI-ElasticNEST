using System.Collections.Generic;
using CountDownAPI.Models;

namespace CountDownAPI.Services
{
    public interface ITeamRepository
    {
        bool Add(Team item);
        bool Update(Team item);
        IEnumerable<Team> GetAll();
        Team FindByKey(string key);
        bool TeamExists(string key);
        Team Remove(string key);

    }
}