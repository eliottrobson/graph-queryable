using System.Collections.Generic;
using GraphQueryable.Server.Models;

namespace GraphQueryable.Server.Graph
{
    public class Data
    {
        public static readonly Dictionary<string, Continent> Continents = new()
        {
            {
                "EU", new Continent
                {
                    Code = "EU",
                    Name = "Europe"
                }
            }
        };

        public static readonly Dictionary<string, Country> Countries = new()
        {
            {
                "GB", new Country
                {
                    Code = "GB",
                    Name = "United Kingdom",
                    Continent = Continents["EU"]
                }
            },
            {
                "FR", new Country
                {
                    Code = "FR",
                    Name = "France",
                    Continent = Continents["EU"]
                }
            },
            {
                "DE", new Country
                {
                    Code = "DE",
                    Name = "Germany",
                    Continent = Continents["EU"]
                }
            }
        };
    }
}