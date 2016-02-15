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