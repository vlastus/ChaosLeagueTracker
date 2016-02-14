using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CLT.Data
{
    public class TeamRepository : GenericRepository<Teams>, ITeamRepository, IDisposable
    {
        public TeamRepository(CLTEntities context) :base(context)
        {
            this.context = context;
            this.dbSet = context.Set<Teams>();
        }

        public IEnumerable<Teams> GetTeamsByGroup(int groupId)
        {
            return context.Teams
                .Where(t => t.CompTeams
                    .Any(ct => ct.Groups.ID == groupId)
                )
                .ToList();
        }

        public IEnumerable<Teams> GetTeams()
        {
            return context.Teams
                .Where(t => t.Status == Status.Active)
                .ToList();
        }

    }
}
