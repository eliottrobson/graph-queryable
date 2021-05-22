using System.Collections.Generic;
using GraphQueryable.Server.Models;

namespace GraphQueryable.Server.Graph
{
    public class Query
    {
        private readonly List<Country> _countries = new()
        {
            new Country
            {
                Name = "United Kingdom",
                Capital = "London"
            },
            new Country
            {
                Name = "France",
                Capital = "Paris"
            },
            new Country
            {
                Name = "Germany",
                Capital = "Berlin"
            }
        };
        
        public List<Country> GetCountries() => _countries;
    }
}