using System.Collections.Generic;
using SolarPhobia.Domain;

namespace SolarPhobia.Domain.Repositories
{
    public interface IGhostRepository
    {
        IEnumerable<Ghost> GetAll();
        Ghost GetById(string id);
        void Save(Ghost ghost);
    }
}

