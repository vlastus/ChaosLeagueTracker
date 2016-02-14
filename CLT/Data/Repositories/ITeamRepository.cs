using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLT.Data
{
    interface ITeamRepository : IDisposable, IGenericRepository<Teams>
    {
        IEnumerable<Teams> GetTeamsByGroup(int groupId);
    }
}
