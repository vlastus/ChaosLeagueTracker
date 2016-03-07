using System;
using CLT.Data;

namespace CLT.Data
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private CLTEntities context = new CLTEntities();
        private PlayerRepository playerRepository;
        private TeamRepository teamRepository;
        private GenericRepository<Competitions> competitionRepository;
        private GenericRepository<Groups> groupRepository;
        private GenericRepository<PlayerTypes> playerTypeRepository;
        private GenericRepository<Users> userRepository;
        private GenericRepository<CompTeams> compTeamRepository;
        private GenericRepository<TypeGroups> typeGroupRepository;
        private GenericRepository<PlayerSkills> playerSkillRepository;
        private GenericRepository<Fixtures> fixtureRepository;
        private GenericRepository<Matches> matchRepository;
        private GenericRepository<TeamMatchData> teamMatchDataRepository;
        private GenericViewRepository<GroupTables> groupTableRepository;

        public PlayerRepository PlayerRepository
        {
            get
            {
                if (this.playerRepository == null)
                {
                    this.playerRepository = new PlayerRepository(context);
                }
                return playerRepository;
            }
        }

        public TeamRepository TeamRepository
        {
            get
            {
                if (this.teamRepository == null)
                {
                    this.teamRepository = new TeamRepository(context);
                }
                return teamRepository;
            }
        }

        public GenericRepository<Competitions> CompetitionRepository
        {
            get
            {
                if (this.competitionRepository == null)
                {
                    this.competitionRepository = new GenericRepository<Competitions>(context);
                }
                return competitionRepository;
            }
        }

        public GenericRepository<Groups> GroupRepository
        {
            get
            {
                if (this.groupRepository == null)
                {
                    this.groupRepository = new GenericRepository<Groups>(context);
                }
                return groupRepository;
            }
        }

        public GenericRepository<PlayerTypes> PlayerTypeRepository
        {
            get
            {
                if (this.playerTypeRepository == null)
                {
                    this.playerTypeRepository = new GenericRepository<PlayerTypes>(context);
                }
                return playerTypeRepository;
            }
        }

        public GenericRepository<Users> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<Users>(context);
                }
                return userRepository;
            }
        }
        public GenericRepository<CompTeams> CompTeamRepository
        {
            get
            {
                if (this.compTeamRepository == null)
                {
                    this.compTeamRepository = new GenericRepository<CompTeams>(context);
                }
                return compTeamRepository;
            }
        }
        public GenericRepository<TypeGroups> TypeGroupRepository
        {
            get
            {
                if (this.typeGroupRepository == null)
                {
                    this.typeGroupRepository = new GenericRepository<TypeGroups>(context);
                }
                return typeGroupRepository;
            }
        }

        public GenericRepository<PlayerSkills> PlayerSkillRepository
        {
            get
            {
                if (this.playerSkillRepository == null)
                {
                    this.playerSkillRepository = new GenericRepository<PlayerSkills>(context);
                }
                return playerSkillRepository;
            }
        }
        public GenericRepository<Fixtures> FixtureRepository
        {
            get
            {
                if (this.fixtureRepository == null)
                {
                    this.fixtureRepository = new GenericRepository<Fixtures>(context);
                }
                return fixtureRepository;
            }
        }
        public GenericRepository<Matches> MatchRepository
        {
            get
            {
                if (this.matchRepository == null)
                {
                    this.matchRepository = new GenericRepository<Matches>(context);
                }
                return matchRepository;
            }
        }
        public GenericRepository<TeamMatchData> TeamMatchDataRepository
        {
            get
            {
                if (this.teamMatchDataRepository == null)
                {
                    this.teamMatchDataRepository = new GenericRepository<TeamMatchData>(context);
                }
                return teamMatchDataRepository;
            }
        }

        public GenericViewRepository<GroupTables> GroupTableRepository
        {
            get
            {
                if (this.groupTableRepository == null)
                {
                    this.groupTableRepository = new GenericViewRepository<GroupTables>(context);
                }
                return groupTableRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
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