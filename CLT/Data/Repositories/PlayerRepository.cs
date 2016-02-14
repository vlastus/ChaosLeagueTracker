using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLT.Data
{
    public class PlayerRepository : GenericRepository<Players>, IPlayerRepository, IDisposable
    {
        public PlayerRepository(CLTEntities context) : base(context)
        {
            this.context = context;
            this.dbSet = context.Set<Players>();
        }

        public IEnumerable<Players> GetPlayers(int teamId)
        {
            return this.context.Players
                .Where(p => p.Status == Status.Active)
                .Where(p => p.Team == teamId)
                .ToList();
        }
    }
}
