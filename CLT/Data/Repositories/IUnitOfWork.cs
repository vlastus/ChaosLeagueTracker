using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLT.Data
{
    public interface IUnitOfWork
    {
        PlayerRepository PlayerRepository { get; }
        TeamRepository TeamRepository { get; }
        GenericRepository<Competitions> CompetitionRepository { get; }
        GenericRepository<Groups> GroupRepository { get;  }
        GenericRepository<PlayerTypes>PlayerTypeRepository { get; }
        GenericRepository<Users> UserRepository { get; }
        GenericRepository<CompTeams> CompTeamRepository { get; }
        GenericRepository<TypeGroups> TypeGroupRepository { get; }
        GenericRepository<PlayerSkills> PlayerSkillRepository { get; }
        GenericRepository<Fixtures> FixtureRepository { get; }
        GenericRepository<Matches> MatchRepository { get; }
        GenericRepository<TeamMatchData> TeamMatchDataRepository { get; }
        GenericViewRepository<GroupTables> GroupTableRepository { get; }
        void Save();
        void Dispose(bool disposing);
        void Dispose();
    }
}
