using System.Collections.Generic;
using System.Linq;
using GraphQueryable.Server.Models;
using HotChocolate.Data;
using JetBrains.Annotations;

namespace GraphQueryable.Server.Graph
{
    public class Query
    {
        [UsedImplicitly]
        [UseProjection, UseFiltering]
        public List<Country> GetCountries() => Data.Countries.Values.ToList();
    }
}