using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.DatabaseFactory
{
    public interface IDbFactory
    {
        IDatabase GetConnection();
    }
}
