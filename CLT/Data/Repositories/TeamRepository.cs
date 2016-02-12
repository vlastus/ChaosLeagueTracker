using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CLT.Data.Repositories
{
    public class TeamRepository : ITeamRepository, IDisposable
    {
        private CLTEntities context;

        public TeamRepository(CLTEntities context)
        {
            this.context = context;
        }

        public IEnumerable<Teams> GetTeamsByGroup(int groupId)
        {
            return this.context.Teams
                .Where(t => t.CompTeams
                    .Any(ct => ct.Groups.ID == groupId)
                )
                .ToList();
        }

        public Teams GetTeamById(int teamId)
        {
            return this.context.Teams
                .Where(t => t.ID == teamId)
                .FirstOrDefault();
        }

        public IEnumerable<Teams> GetTeams()
        {
            return this.context.Teams
                .Where(t => t.Status == Status.Active)
                .ToList();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
