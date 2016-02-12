using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLT.Data.Repositories
{
    interface IPlayerRepository : IDisposable
    {
        IEnumerable<Players> GetPlayers(int teamId);
        Players GetPlayerByID(int playerId);
        void AddPlayer(Players player);
        void UpdatePlayer(Players player);
        void DeactivatePlayer(int playerId);
        void Save();
    }
}
