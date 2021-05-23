using System.Collections.Generic;
using System.Linq;
using GraphQueryable.Server.Models;
using JetBrains.Annotations;

namespace GraphQueryable.Server.Graph
{
    public class Query
    {
        [UsedImplicitly]
        public List<Country> GetCountries() => Data.Countries.Values.ToList();
    }
}