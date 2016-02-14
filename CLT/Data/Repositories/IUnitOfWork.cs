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
        void Save();
        void Dispose(bool disposing);
        void Dispose();
    }
}
