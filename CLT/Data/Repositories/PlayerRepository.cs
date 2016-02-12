using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLT.Data.Repositories
{
    public class PlayerRepository : IPlayerRepository, IDisposable
    {
        private CLTEntities context;

        public PlayerRepository(CLTEntities context)
        {
            this.context = context;
        }

        public IEnumerable<Players> GetPlayers(int teamId)
        {
            return this.context.Players
                .Where(p => p.Status == Status.Active)
                .Where(p => p.Team == teamId)
                .ToList();
        }

        public Players GetPlayerByID(int playerId)
        {
            return this.context.Players
                .Where(p => p.ID == playerId)
                .FirstOrDefault();
        }

        public void AddPlayer(Players player)
        {

        }

        public void UpdatePlayer(Players player)
        {

        }

        public void DeactivatePlayer(int playerId)
        {

        }
        public void Save()
        {

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
