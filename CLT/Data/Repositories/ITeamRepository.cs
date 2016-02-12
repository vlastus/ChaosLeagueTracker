using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLT.Data.Repositories
{
    interface ITeamRepository : IDisposable
    {
        IEnumerable<Teams> GetTeamsByGroup(int groupId);

        Teams GetTeamById(int teamId);

        IEnumerable<Teams> GetTeams();
    }
}
