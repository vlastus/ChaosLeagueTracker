using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLT.Data
{
    interface IPlayerRepository : IDisposable, IGenericRepository<Players>
    {
        IEnumerable<Players> GetPlayers(int teamId);
    }
}
